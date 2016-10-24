using System;
using System.IO;

namespace NLSImportTool.Utilities.Storage
{
    public class SystemContextFileSystem : IFileSystem
    {
	   public SystemContextFileSystem()
	   {
		  _systemPathMapper = SystemPathMapper.Instance;
        }
	   public IDirectory LoadDirectory(string path)
	   {
		  DirectoryInfo dirInfo = new DirectoryInfo(_systemPathMapper.GetUserContextFolder(path));
		  return new SystemContextDirectory(dirInfo);
	   }

	   private ISystemPathMapper _systemPathMapper;
    }
}
