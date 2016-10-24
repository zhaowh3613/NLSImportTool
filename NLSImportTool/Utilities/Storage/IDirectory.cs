using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities.Storage
{
    public interface IDirectory
    {
        /// <summary>
        /// Name of the directory
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Full path to the directory
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// Parent directory for this directory
        /// </summary>
        IDirectory ParentDirectory { get; }

        /// <summary>
        /// Returns whether or not the directory exists
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets a list of IFile from the directory;
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IFile>> GetFilesAsync();

        /// <summary>
        /// Gets the specified File by the file name or a file nested inside a subdirectory owned by this directory
        /// </summary>
        /// <param name="pathToFile">Information.txt OR Lenovo\Information\Information.txt</param>
        /// <returns></returns>
        Task<IFile> GetFileAsync(string pathToFile);

        /// <summary>
        /// Gets the full list of directories inside this directory
        /// </summary>
        Task<IEnumerable<IDirectory>> GetDirectoriesAsync();

        /// <summary>
        /// Gets the directory specified by the directoryName, can be immediate directory or nested subdirectory
        /// </summary>
        /// <param name="pathToDirectory">Lenovo OR Lenovo\Information\Test</param>
        /// <returns></returns>
        Task<IDirectory> GetDirectoryAsync(string pathToDirectory);

        /// <summary>
        /// Creates a directory with the given directory path
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        Task<IDirectory> CreateDirectoryAsync(string directoryPath, CreationOption option);

        /// <summary>
        /// Creates a file using the given file name, contents, and creation option
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contents"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IFile> CreateFileAsync(string fileName, string contents, CreationOption option);

        /// <summary>
        /// Moves this directory to the specified destination directory
        /// </summary>
        /// <param name="destinationDirectoryPath"></param>
        /// <param name="collisionOption"></param>
        /// <returns></returns>
        Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption collisionOption);

        /// <summary>
        /// Copies this directory to the target destination
        /// </summary>
        /// <returns></returns>
        Task<bool> CopyAsync(string destinationPath, CollisionOption collisionOption);

        /// <summary>
        /// Renames this directory
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Returns true/false if this was successful</returns>
        Task<bool> RenameAsync(string newName);

        /// <summary>
        /// Deletes this directory
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync();


        /// <summary>
        /// Directory Size
        /// </summary>
        long Length { get;}

        /// <summary>
        /// Calcuate directory size
        /// </summary>
        void CalculateDirectorySize();
    }
}
