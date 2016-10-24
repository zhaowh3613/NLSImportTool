using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities.Storage
{
    public class WinFile : IFile
    {
        internal WinFile(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentException("File info was null, need the information");
            }

            _fileInfo = fileInfo;
        }

        /// <summary>
        /// The name of the file (with file extension)
        /// </summary>
        public string Filename
        {
            get { return _fileInfo.Name; }
        }

        /// <summary>
        /// The name of the directory the file is in
        /// </summary>
        public string DirectoryName
        {
            get { return _fileInfo.Directory.Name; }
        }

        /// <summary>
        /// The full path of the file
        /// </summary>
        public string FullPath
        {
            get { return _fileInfo.FullName; }
        }

        /// <summary>
        /// The full path to the directory that the file is contained within
        /// </summary>
        public string DirectoryPath
        {
            get { return _fileInfo.DirectoryName; }
        }

        /// <summary>
        /// The directory that the file is in
        /// </summary>
        public IDirectory ParentDirectory
        {
            get { return new WinDirectory(_fileInfo.Directory); }
        }

        /// <summary>
        /// Returns whether or not the file exists
        /// </summary>
        public bool Exists
        {
            get { return _fileInfo.Exists; }
        }

        /// <summary>
        /// Returns file length
        /// </summary>
        public long Length
        {
            get { return _fileInfo.Length; }
        }

        /// <summary>
        /// Returns the date the file was last modified, as a DateTime object
        /// </summary>
        public DateTime DateLastModified
        {
            get { return _fileInfo.LastWriteTime; }
        }

        /// <summary>
        /// Reads all of the contents of a file and returns it as a string
        /// </summary>
        /// <returns></returns>
        public Task<string> ReadContentsAsync()
        {
            AssertFileExists();

            return Task.Factory.StartNew(() =>
                {
                    return File.ReadAllText(this.FullPath);
                });
        }

        /// <summary>
        /// Writes the contents to this file, with the option to append or replace existing contents
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="writingOption"></param>
        /// <returns></returns>
        public Task<bool> WriteContentsAsync(string contents, WritingOption writingOption)
        {
            AssertFileExists();

            return Task.Factory.StartNew(() =>
                {
                    bool wasWritten = false;

                    switch (writingOption)
                    {
                        case WritingOption.Append:
                            File.AppendAllText(this.FullPath, contents);
                            wasWritten = true;
                            break;
                        case WritingOption.Replace:
                            File.WriteAllText(this.FullPath, contents);
                            wasWritten = true;
                            break;
                        default:
                            goto case WritingOption.Append;
                    }

                    return wasWritten;
                });
        }

        /// <summary>
        /// Moves the file to the specified directory path
        /// </summary>
        /// <param name="destinationDirectoryPath"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption option)
        {
            AssertFileExists();

            return Task.Factory.StartNew(() =>
                {
                    bool wasMoved = false;
                    string fullDestinationPath = Environment.ExpandEnvironmentVariables(Path.Combine(destinationDirectoryPath, this.Filename));

                    switch (option)
                    {
                        case CollisionOption.ReplaceExisting:
                            if (File.Exists(fullDestinationPath))
                            {
                                File.Delete(fullDestinationPath);
                            }
                            _fileInfo.MoveTo(fullDestinationPath);
                            wasMoved = true;
                            break;
                        case CollisionOption.ThrowExisting:
                            if (File.Exists(fullDestinationPath))
                            {
                                throw new Exceptions.FileAlreadyExistsException("File already exists in the target directory");
                            }
                            goto case CollisionOption.ReplaceExisting;
                        default:
                            goto case CollisionOption.ReplaceExisting;
                    }

                    return wasMoved;
                });
        }

        /// <summary>
        /// Copies this file to the target directory
        /// </summary>
        /// <param name="destinationDirectoryPath"></param>
        /// <param name="overwriteExisting"></param>
        /// <returns></returns>
        public Task<bool> CopyAsync(string destinationDirectoryPath, bool overwriteExisting)
        {
            AssertFileExists();

            return Task.Factory.StartNew(() =>
                {
                    bool wasCopied = false;
                    string destinationFullPath = Environment.ExpandEnvironmentVariables(Path.Combine(destinationDirectoryPath, this.Filename));

                    _fileInfo.CopyTo(destinationFullPath, overwriteExisting);
                    wasCopied = true;

                    return wasCopied;
                });
        }

        /// <summary>
        /// Renames this file using the given file name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Task<bool> RenameAsync(string fileName)
        {
            AssertFileExists();

            return Task.Factory.StartNew(() =>
                {
                    bool wasRenamed = false;
                    string newNamePath = Path.Combine(this.DirectoryPath, fileName);

                    File.Move(this.FullPath, newNamePath);
                    _fileInfo = new FileInfo(newNamePath);
                    wasRenamed = true;

                    return wasRenamed;
                });
        }

        /// <summary>
        /// Deletes this file
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAsync()
        {
            AssertFileExists();

            return Task.Factory.StartNew(() =>
                {
                    bool wasDeleted = false;

                    _fileInfo.Delete();
                    wasDeleted = true;

                    return wasDeleted;
                });
        }        

        private void AssertFileExists()
        {
            if (_fileInfo != null)
            {
                if (!_fileInfo.Exists)
                {
                    throw new FileNotFoundException("The File does not exist!");
                }
            }
        }

        private FileInfo _fileInfo;
    }
}
