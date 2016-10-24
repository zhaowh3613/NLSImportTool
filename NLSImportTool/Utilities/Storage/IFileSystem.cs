using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities.Storage
{
    public interface IFileSystem
    {
        IDirectory LoadDirectory(string path);
    }
}
