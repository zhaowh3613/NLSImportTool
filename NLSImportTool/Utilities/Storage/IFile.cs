using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities.Storage
{
    public interface IFile
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        string Filename { get; }

        /// <summary>
        /// The name of the directory the file is located in
        /// </summary>
        string DirectoryName { get; }

        /// <summary>
        /// The full file path to the file
        /// </summary>
        string FullPath { get; }

        /// <summary>
        /// File Size
        /// </summary>
        long Length { get; }

        /// <summary>
        /// The full path to the directory that owns this file
        /// </summary>
        string DirectoryPath { get; }

        /// <summary>
        /// The parent directory of the file, returns actual IDirectory object
        /// </summary>
        IDirectory ParentDirectory { get; }

        /// <summary>
        /// Returns whether or not the file exists
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Date the file was last modified
        /// </summary>
        DateTime DateLastModified { get; }

        /// <summary>
        /// Reads all the contents of the file and returns a string
        /// </summary>
        /// <returns></returns>
        Task<string> ReadContentsAsync();

        /// <summary>
        /// Writes contents to this file, with the option to append or replace existing contents
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="writingOption"></param>
        /// <returns></returns>
        Task<bool> WriteContentsAsync(string contents, WritingOption writingOption);

        /// <summary>
        /// Moves this file to the specified destination directory
        /// </summary>
        /// <param name="destinationDirectoryPath"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<bool> MoveAsync(string destinationDirectoryPath, CollisionOption option);

        /// <summary>
        /// Copies this file to the specified destination directory
        /// </summary>
        /// <param name="destinationDirectoryPath"></param>
        /// <param name="overwriteExisting"></param>
        /// <returns></returns>
        Task<bool> CopyAsync(string destinationDirectoryPath, bool overwriteExisting);

        /// <summary>
        /// Renames this file using the given filename
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<bool> RenameAsync(string fileName);

        /// <summary>
        /// Delets this file
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAsync();
    }
}
