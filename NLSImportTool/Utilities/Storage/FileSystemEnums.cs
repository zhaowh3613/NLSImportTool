using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities.Storage
{
    public enum CreationOption
    {
        /// <summary>
        /// Opens the file if it already exists
        /// </summary>
        OpenIfExists,
        /// <summary>
        /// Replaces the file if it's already existing
        /// </summary>
        ReplaceExisting,
        /// <summary>
        /// Throws an exception if the file already exists
        /// </summary>
        ThrowIfExists,
    }

    public enum CollisionOption
    {
        /// <summary>
        /// Replaces the already existing file
        /// </summary>
        ReplaceExisting,
        /// <summary>
        /// Throws an exception if it already exists
        /// </summary>
        ThrowExisting,
    }

    public enum WritingOption
    {
        /// <summary>
        /// Overwrites the contents
        /// </summary>
        Replace,
        /// <summary>
        /// Adds to the contents
        /// </summary>
        Append
    }
    
}
