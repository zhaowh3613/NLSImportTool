using System;
using System.IO;
using System.Text.RegularExpressions;

namespace NLSImportTool.Utilities
{
    public class SystemPathMapper : ISystemPathMapper
    {
	   private SystemPathMapper()
	   {
		  //_userInformation = UserInformationProvider.Instance.GetUserInformation();
        }

	   #region Static Properties

	   private static ISystemPathMapper instance;
	   public static ISystemPathMapper Instance
	   {
		  get
		  {
			 if (instance == null)
			 {
				instance = new SystemPathMapper();
			 }
			 return instance;
		  }
	   }

        #endregion

        string ISystemPathMapper.GetUserContextFolder(string path)
        {
            Regex regex = new Regex("%(.*)%");
            var match = regex.Match(path);
            string environmentVariable = string.Empty;
            if (match.Length > 0)
            {
                //environmentVariable = match.Groups[1].ToString();
                //if (!String.IsNullOrEmpty(environmentVariable))
                //{
                //    switch (environmentVariable.ToUpper())
                //    {
                //        case "APPDATA"://C:\Users\X320Tablet\AppData\Roaming
                //            path = path.Replace("%" + environmentVariable + "%", _userInformation.UserProfileFolder + Path.DirectorySeparatorChar + @"AppData\Roaming");
                //            break;
                //        case "HOMEPATH"://\Users\X320Tablet
                //            path = path.Replace("%" + environmentVariable + "%", _userInformation.UserProfileFolder.Substring(4));
                //            break;
                //        case "TEMP"://C:\Users\X320TA~1\AppData\Local\Temp
                //            path = path.Replace("%" + environmentVariable + "%", _userInformation.UserProfileFolder + Path.DirectorySeparatorChar + @"AppData\Local\Temp");
                //            break;
                //        case "TMP"://C:\Users\X320TA~1\AppData\Local\Temp
                //            path = path.Replace("%" + environmentVariable + "%", _userInformation.UserProfileFolder + Path.DirectorySeparatorChar + @"AppData\Local\Temp");
                //            break;
                //        case "USERPROFILE"://C:\Users\X320Tablet
                //            path = path.Replace("%" + environmentVariable + "%", _userInformation.UserProfileFolder);
                //            break;
                //        case "USERNAME":
                //            path = path.Replace("%" + environmentVariable + "%", _userInformation.UserName);
                //            break;
                //        case "LOCALAPPDATA"://C:\Users\X320Tablet\AppData\Local
                //            path = path.Replace("%" + environmentVariable + "%", _userInformation.UserProfileFolder + Path.DirectorySeparatorChar + @"AppData\Local");
                //            break;
                //        default:
                //            break;
                //    }
                //}
            }
            return path;
        }

        #region Private Variables

        //private UserInformation _userInformation;

        #endregion
    }

    public interface ISystemPathMapper
    {
	   string GetUserContextFolder(string path);
    }
}
