using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities.Storage
{
    public class SystemContextDirectory : IDirectory
    {
        public SystemContextDirectory(DirectoryInfo dirInfo)
        {
            // Throw exception if the directory info is null
            if (dirInfo == null)
            {
                throw new ArgumentException("Directory info was null, need the information");
            }

            _directoryInfo = dirInfo;

            _systemPathMapper = SystemPathMapper.Instance;
        }

        /// <summary>
        /// Gets the name of the directory
        /// </summary>
        public string Name
        {
            get { return _directoryInfo.Name; }
        }

        /// <summary>
        /// Gets the full path of the directory
        /// </summary>
        public string FullPath
        {
            get { return _directoryInfo.FullName; }
        }

        /// <summary>
        /// Parent directory for this directory
        /// </summary>
        public IDirectory ParentDirectory
        {
            get { return new SystemContextDirectory(_directoryInfo.Parent); }
        }

        /// <summary>
        /// Returns whether or not the directory exists
        /// </summary>
        public bool Exists
        {
            get { return _directoryInfo.Exists; }
        }

        /// <summary>
        /// Directory Size
        /// </summary>
        public long Length { get { return _length; } }

        /// <summary>
        /// Gets a list of files contained in the directory
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<IFile>> GetFilesAsync()
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                IEnumerable<IFile> files = new List<IFile>();

              // Get the list of files in the directory
              var fileInfoList = _directoryInfo.GetFiles();

                if (fileInfoList != null && fileInfoList.Any())
                {
                  // Select each file and create a new WinFile object and add it to the list
                  files = fileInfoList.Select((file, i) => new SystemContextFile(file));
                }

                return files;
            });
        }

        /// <summary>
        /// Gets the specified File by the file name or a file nested inside a subdirectory owned by this directory
        /// </summary>
        /// <param name="pathToFile">Information.txt OR Lenovo\Information\Information.txt</param>
        /// <returns></returns>
        public Task<IFile> GetFileAsync(string pathToFile)
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                IFile file = null;

                if (!String.IsNullOrWhiteSpace(pathToFile))
                {
                  // Get the full path to the desired file
                  string filePath = Path.Combine(_systemPathMapper.GetUserContextFolder(this.FullPath), pathToFile);

                    if (File.Exists(filePath))
                    {
                      // Create new file info object, use this Directory's path and the passed in path to get it
                      var fileInfo = new FileInfo(filePath);

                      // Make sure the file info is null
                      //if (fileInfo != null)
                      //  {
                          // Create new WinFile object using file info
                          file = new SystemContextFile(fileInfo);
                        //}
                    }
                }

                return file;
            });
        }

        /// <summary>
        /// Gets a list of the directories contained in the directory
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<IDirectory>> GetDirectoriesAsync()
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                IEnumerable<IDirectory> directories = new List<IDirectory>();

              // Get the list of directories
              var directoryInfoList = _directoryInfo.GetDirectories();

                if (directoryInfoList != null && directoryInfoList.Any())
                {
                    // Create new WinDirectory object from each directory info and add it to the list
                    directories = directoryInfoList.Select((directory, i) => new SystemContextDirectory(directory));
                    directories.ToList().ForEach(directory => directory.CalculateDirectorySize());
                }

                return directories;
            });
        }

        /// <summary>
        /// Gets the directory specified by the directoryName, can be immediate directory or nested subdirectory
        /// </summary>
        /// <param name="pathToDirectory">Lenovo OR Lenovo\Information\Test</param>
        /// <returns></returns>
        public Task<IDirectory> GetDirectoryAsync(string pathToDirectory)
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                IDirectory directory = null;

                if (!String.IsNullOrWhiteSpace(pathToDirectory))
                {
                  // Get full path to the desired directory
                  string directoryPath = Path.Combine(_systemPathMapper.GetUserContextFolder(this.FullPath), pathToDirectory);

                    if (Directory.Exists(directoryPath))
                    {
                      // Create a new directory info object for the desired directory
                      var directoryInfo = new DirectoryInfo(directoryPath);

                        //if (directoryInfo != null)
                        //{
                            // Create new win directory object using directoryInfo
                            directory = new SystemContextDirectory(directoryInfo);
                        //}
                    }
                }

                return directory;
            });
        }

        public void CalculateDirectorySize()
        {
            try
            {
                _length = _directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
            }
            catch (UnauthorizedAccessException)
            {
                _length = 0;
            }
        }

        /// <summary>
        /// Creates a directory using the given path and creation option
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="collisionOption"></param>
        /// <returns></returns>
        public Task<IDirectory> CreateDirectoryAsync(string directoryPath, CreationOption collisionOption)
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                IDirectory createdDirectory = null;
                string expandedPath = _systemPathMapper.GetUserContextFolder(directoryPath);

              // Check to see if the path is a relative or full path
              if (!Path.IsPathRooted(expandedPath))
                {
                  // If it's a relative path, then create the new full path using this directory's full path
                  expandedPath = Path.Combine(this.FullPath, directoryPath);
                }

              // Verify the directory exists
              if (Directory.Exists(expandedPath))
                {
                    switch (collisionOption)
                    {
                        case CreationOption.OpenIfExists:
                          // Just open the directory if it already exists
                          createdDirectory = new SystemContextDirectory(new DirectoryInfo(expandedPath));
                            break;
                        case CreationOption.ReplaceExisting:
                          // Replace the existing directory with a new one, must first delete it (recursively delete all sub directories as well)
                          Directory.Delete(expandedPath, true);
                            createdDirectory = new SystemContextDirectory(Directory.CreateDirectory(expandedPath));
                            break;
                        case CreationOption.ThrowIfExists:
                            throw new Exceptions.DirectoryAlreadyExistsException("The directory to be created already exists!");
                        default:
                            goto case CreationOption.ReplaceExisting;
                    }
                }
                else
                {
                  // Directory does not already exist, just go ahead and create it
                  createdDirectory = new SystemContextDirectory(Directory.CreateDirectory(expandedPath));
                }

                return createdDirectory;
            });
        }

        /// <summary>
        /// Creates a file using the given file path, contents, and creation option
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contents"></param>
        /// <param name="collisionOption"></param>
        /// <returns></returns>
        public Task<IFile> CreateFileAsync(string fileName, string contents, CreationOption collisionOption)
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                IFile createdFile = null;
                string filePath = Path.Combine(_systemPathMapper.GetUserContextFolder(this.FullPath), fileName);

              // File exists
              if (File.Exists(filePath))
                {
                    switch (collisionOption)
                    {
                        case CreationOption.OpenIfExists:
                            File.AppendAllText(filePath, contents);
                            break;
                        case CreationOption.ReplaceExisting:
                            File.WriteAllText(filePath, contents);
                            break;
                        case CreationOption.ThrowIfExists:
                            throw new Exceptions.FileAlreadyExistsException("The file to be created already exists in the target directory");
                        default:
                            goto case CreationOption.ReplaceExisting;
                    }
                }
              // File does not already exist
              else
                {
                    if (contents != null)
                    {
                      // Contents are not null, write all the text to the new file
                      File.WriteAllText(filePath, contents);
                    }
                    else
                    {
                      // Contents were null, just create a new, empty file
                      File.Create(filePath).Dispose();
                    }
                }

              // Create a new WinFile object using FileInfo for the filePath
              createdFile = new SystemContextFile(new FileInfo(filePath));

                return createdFile;
            });
        }

        /// <summary>
        /// Moves the directory to the destination directory specified
        /// </summary>
        /// <param name="destinationDirectoryPath"></param>
        /// <param name="collisionOption"></param>
        /// <returns></returns>
        public Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption collisionOption)
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                bool wasMoved = false;

                switch (collisionOption)
                {
                    case CollisionOption.ReplaceExisting:
                        string fullDestination = Path.Combine(_systemPathMapper.GetUserContextFolder(destinationDirectoryPath), this.Name);
                        if (Directory.Exists(fullDestination))
                        {
                            Directory.Delete(fullDestination);
                        }
                        Directory.Move(this.FullPath, fullDestination);
                        _directoryInfo = new DirectoryInfo(fullDestination);
                        wasMoved = true;
                        break;
                    case CollisionOption.ThrowExisting:
                        throw new Exceptions.DirectoryAlreadyExistsException("Directory already exists in the target directory");
                    default:
                        goto case CollisionOption.ReplaceExisting;
                }

                return wasMoved;
            });
        }

        /// <summary>
        /// Copies the contents of this directory to the target destination
        /// CAUTION: If you want to directly copy this directory you must specify this directory name in the destination path
        /// Example: CopyAsync(C:\ProgramData, CollisionOption) will just copy the files and subdirectories
        /// in the directory to ProgramData. CopyAsync(C:\ProgramData\[NameOfDirectory], CollisionOption) will be like copying
        /// the folder itself
        /// </summary>
        /// <returns></returns>
        public Task<bool> CopyAsync(string destinationPath, CollisionOption collisionOption)
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                bool wasCopied = false;

              // Expand the destination directory and add this directory name to the target directory
              string expandedDestination = _systemPathMapper.GetUserContextFolder(destinationPath);

              // Call the copy directory function, requires new method due to recursion
              wasCopied = CopyDirectory(_directoryInfo.FullName, expandedDestination, collisionOption);

                return wasCopied;
            });
        }

        /// <summary>
        /// Recursive function to copy directories
        /// </summary>
        private bool CopyDirectory(string source, string destination, CollisionOption collisionOption)
        {
            bool wasCopied = false;

            // Get the source directory
            DirectoryInfo sourceDir = new DirectoryInfo(source);

            // Create the directory if it does not already exist
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            // Get the files & sub directories for the source directory
            var files = sourceDir.GetFiles();
            var subDirectories = sourceDir.GetDirectories();

            switch (collisionOption)
            {
                // Replaces any existing folders or files
                case CollisionOption.ReplaceExisting:
                    // Iterate through each file in the directory
                    foreach (var file in files)
                    {
                        // Get the file info for the destination path, to see if it already exists
                        FileInfo fileDestInfo = new FileInfo(Path.Combine(destination, file.Name));
                        if (fileDestInfo.Exists)
                        {
                            // Delete the existing target file
                            fileDestInfo.Delete();
                        }

                        // Copy the file to the new destination
                        file.CopyTo(Path.Combine(destination, file.Name), true);
                    }

                    // Iterate through each folder in the directory
                    foreach (var subDirectory in subDirectories)
                    {
                        // Get the directory info for the destination path, to see if it already exists
                        DirectoryInfo dirDestInfo = new DirectoryInfo(Path.Combine(destination, subDirectory.Name));
                        if (dirDestInfo.Exists)
                        {
                            // Delete the existing target directory 
                            dirDestInfo.Delete(true);
                        }

                        // Copy the directory to the new destination
                        CopyDirectory(subDirectory.FullName, dirDestInfo.FullName, collisionOption);
                    }

                    wasCopied = true;
                    break;
                // Throws exception if there are any pre-existing files or folders
                case CollisionOption.ThrowExisting:
                    // Iterate through file in the directory
                    foreach (var file in files)
                    {
                        // Get the file info for the destination path, to see if it already exists
                        FileInfo fileDestInfo = new FileInfo(Path.Combine(destination, file.Name));
                        if (!fileDestInfo.Exists)
                        {
                            // Copy the file to the new destination
                            file.CopyTo(fileDestInfo.FullName, true);
                        }
                        else
                        {
                            throw new Exceptions.FileAlreadyExistsException(String.Format("File path {0} already exists.", file.FullName));
                        }
                    }

                    // Iterate through each sub-directory
                    foreach (var subDirectory in subDirectories)
                    {
                        // Get the directory info for the destination path, to see if it already exists
                        DirectoryInfo dirDestInfo = new DirectoryInfo(Path.Combine(destination, subDirectory.Name));
                        if (!dirDestInfo.Exists)
                        {
                            // Copy the directory to the new destination
                            CopyDirectory(subDirectory.FullName, dirDestInfo.FullName, collisionOption);
                        }
                        else
                        {
                            throw new Exceptions.FileAlreadyExistsException(String.Format("Directory path {0} already exists.", subDirectory.FullName));
                        }
                    }

                    wasCopied = true;
                    break;
                default:
                    goto case CollisionOption.ReplaceExisting;
            }

            return wasCopied;
        }

        /// <summary>
        /// Renames this directory using the given newName
        /// </summary>
        /// <param name="newName"></param>
        /// <returns></returns>
        public Task<bool> RenameAsync(string newName)
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                bool wasRenamed = false;

              // Create the new path for the renamed directory
              var newNamePath = Path.Combine(_directoryInfo.Parent.FullName, newName);

              // Make sure the new path is valid
              if (!String.IsNullOrWhiteSpace(newNamePath))
                {
                    Directory.Move(this.FullPath, newNamePath);
                    _directoryInfo = new DirectoryInfo(newNamePath);
                    wasRenamed = true;
                }

                return wasRenamed;
            });
        }

        /// <summary>
        /// Deletes this directory
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAsync()
        {
            AssertDirectoryExists();

            return Task.Factory.StartNew(() =>
            {
                bool wasDeleted = false;

                _directoryInfo.Delete(true);
                wasDeleted = true;

                return wasDeleted;
            });
        }

        /// <summary>
        /// Checks to make sure the directory exists
        /// </summary>
        private void AssertDirectoryExists()
        {
            if (_directoryInfo != null)
            {
                if (!_directoryInfo.Exists)
                {
                    throw new FileNotFoundException("The Directory does not exist!");
                }
            }
        }

        private DirectoryInfo _directoryInfo;

        private ISystemPathMapper _systemPathMapper;

        private long _length;
    }
}
