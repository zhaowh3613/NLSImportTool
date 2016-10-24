using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NLSImportTool.Utilities.Storage
{
    public class WinFileSystem : IFileSystem
    {
        public IDirectory LoadDirectory(string path)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(path);

            DirectoryInfo dirInfo = new DirectoryInfo(expandedPath);

            return new WinDirectory(dirInfo);
        }
    }
}
