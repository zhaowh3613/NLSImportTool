using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLSImportTool.Utilities
{
    public static class FileManager
    {
        private const string DirectorySeparatorChar = "\\";
        private const string XmlExtension = ".xml";
        public static void FileWrite(string content, string fileName, string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (UnauthorizedAccessException e)
                {
                }
                catch (Exception e)
                { }
            }
            string fullName = Path.Combine(path, fileName);
            try
            {
                FileStream fs = new FileStream(fullName, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (FileNotFoundException ex)
            {
            }
            catch (IOException ex)
            {
            }
        }

        public static string FileRead(string filePath)
        {
            string ret = "";

            try
            {
                string tmpLine;
                StreamReader sr = new StreamReader(filePath, true);
                {
                    tmpLine = sr.ReadToEnd();
                    ret += tmpLine;
                }
                sr.Close();
            }
            catch (FileNotFoundException e)
            {
                //throw e;
            }
            catch (IOException e)
            {

            }
            return ret;
        }


    }
}
