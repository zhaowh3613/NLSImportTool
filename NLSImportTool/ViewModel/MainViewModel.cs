using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Xps.Packaging;
using System.Xml;
using Microsoft.Office.Interop.Word;
using System.Threading.Tasks;
using NLSImportTool.Utilities;
using NLSImportTool.Model;
using System.Resources;

namespace NLSImportTool.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
        }


        private string _processStatus;

        public string ProcessStatus
        {
            get { return _processStatus; }
            set
            {
                _processStatus = value;
                RaisePropertyChanged("ProcessStatus");
            }
        }


        private List<Data> _keyList;

        public List<Data> KeyList
        {
            get { return _keyList ?? (_keyList = new List<Data>()); }
            set
            {
                _keyList = value;
                RaisePropertyChanged("KeyList");
            }
        }

        private string _nLSPath = @"D:\\NLSTest";

        public string NLSPath
        {
            get { return _nLSPath; }
            set
            {
                _nLSPath = value;
                RaisePropertyChanged("NLSPath");

            }
        }

        private string _importTargetPath = @"D:\\strings";

        public string ImportTargetPath
        {
            get { return _importTargetPath; }
            set
            {
                _importTargetPath = value;
                RaisePropertyChanged("ImportTargetPath");

            }
        }

        private bool _isManualMode = false;

        public bool IsManualMode
        {
            get { return _isManualMode; }
            set
            {
                _isManualMode = value;
                RaisePropertyChanged("ImportTargetPath");
            }
        }

        private int _keysCount;

        public int KeysCount
        {
            get { return _keysCount; }
            set
            {
                _keysCount = value;
                RaisePropertyChanged("KeysCount");
            }
        }

        private double _progressBarValue;

        public double ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                RaisePropertyChanged("ProgressBarValue");
            }
        }


        private string _validateText = "";

        public string ValidateText
        {
            get { return _validateText; }
            set
            {
                _validateText = value;
                RaisePropertyChanged("ValidateText");
            }
        }

        public void LoadKeyFromInput(string keyStr)
        {
            try
            {
                var str = "";
                if (!string.IsNullOrWhiteSpace(keyStr))
                {
                    str = keyStr.Trim();
                }
                if (!XmlValidate(str))
                {
                    ValidateText = "please check your xml fomat";
                    return;
                }
                ValidateText = "";
                var xmlStr = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><root>{0}</root>", str);
                var res = Serializer.Deserialize<Resources>(xmlStr);
                if (res != null && res.DataList != null && res.DataList.Count() > 0)
                {
                    KeyList.Clear();
                    KeyList.AddRange(res.DataList);
                    KeysCount = KeyList.Count();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadKey Error {0}", ex.Message);
            }
        }


        public bool XmlValidate(string xmlStr)
        {
            if (String.IsNullOrWhiteSpace(xmlStr))
            {
                return false;
            }
            xmlStr.Trim();
            if (!xmlStr.StartsWith("<data"))
            {
                return false;
            }
            if (!xmlStr.EndsWith("</data>"))
            {
                return false;
            }
            return true;
        }

        //public void LoadKey(String rootPath = @"D:\\NLS\\Key.docx")
        //{
        //    try
        //    {
        //        rootPath = "D:\\NLS\\en\\resources.resw";
        //        FileInfo file = new FileInfo(rootPath);
        //        if (file == null)
        //        {
        //            Console.WriteLine("LoadKey Error file is null");
        //            return;
        //        }
        //        var keys = GetKey(file);
        //        if (keys != null && keys.Count() > 0)
        //        {
        //            KeyList.Clear();
        //            KeyList.AddRange(keys);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogText = String.Format("LoadWords Error {0} null", ex.Message);
        //        Console.WriteLine("LoadWords Error {0}", ex.Message);
        //    }

        //}

        public async void ExcuteMainProcess()
        {
            //foreach (String line in File.ReadAllLines(filePath))
            //{
            //    Encoding.Default.GetString(line);
            //    //string[] tokens = line.Split(new char[] { ' ' });
            //    //for (int i = 0; i < 500; i++)
            //    //{
            //    //    words.Add(tokens[Random.Next(tokens.Count())]);
            //    //}
            //}
            try
            {
                //LoadKey(rootPath + "\\Key.docx");
                // LoadKey(keyFileName);
                ProgressBarValue = 0;
                KillProcess();
                LoadResources();
                //// Loop through all words in the document.
                //int count = document.Words.Count;
                //for (int i = 1; i <= count; i++)
                //{
                //    // Write the word.
                //    string text = document.Words[i].Text;
                //    Console.WriteLine("Word {0} = {1}", i, text);
                //}

            }
            catch (Exception ex)
            {

                ProcessStatus = String.Format("LoadWords Error {0} null", ex.Message);
                Console.WriteLine("LoadWords Error {0}", ex.Message);
            }
            finally
            {
            }
        }

        public async void LoadResources()
        {
            try
            {
                var rootPath = NLSPath;
                if (string.IsNullOrWhiteSpace(rootPath))
                {
                    ProcessStatus = String.Format("NLSPath {0} is null", rootPath);
                    Console.WriteLine("NLSPath {0} is null", rootPath);
                    return;
                }
                var dirArray = GetDirectories(NLSPath);
                if (dirArray == null)
                {
                    ProcessStatus = String.Format("GetDirectories {0} null", rootPath);
                    Console.WriteLine("GetDirectories {0} null", rootPath);
                    return;
                }
                //foreach (var dir in dirArray)
                for(int i = 0; i < dirArray.Count(); i++)
                {
                    //var file = dir.GetFiles("*.DOCX", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    var file = dirArray[i].GetFiles("*.DOCX", SearchOption.TopDirectoryOnly).FirstOrDefault();
                    if (file == null) continue;
                    await GetDocument(file);
                    
                    if (dirArray.Count() != 0)
                    {
                       var per = 100.00 / dirArray.Count();
                        ProgressBarValue = (i + 1) * per;
                    }
                }
                KillProcess();
                ProcessStatus = String.Format("all complete!");
            }
            catch (Exception ex)
            {
                KillProcess();
                Console.WriteLine("LoadResources Error {0}", ex.Message);
            }
        }



        public DirectoryInfo[] GetDirectories(string path)
        {
            DirectoryInfo[] array = null;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                array = dir.GetDirectories();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDirectories Error: " + ex.Message);
            }
            return array;
        }

        public string[] GetFiles(string path)
        {
            string[] array = null;
            try
            {
                FileInfo file = new FileInfo(path);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetFiles Error: " + ex.Message);
            }
            return array;
        }

        public void SaveFile(Dictionary<string, string> keyValue, StreamWriter stream)
        {
            try
            {
                var key = keyValue.Keys.FirstOrDefault();
                var value = keyValue.Values.FirstOrDefault();
                stream.WriteLine("<data name=\"{0}\" xml:space=\"preserve\">", key);
                stream.WriteLine("<value>{0}</value>", value);
                stream.WriteLine("</data>");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveFile Error: " + ex.Message);
            }
        }

        public string[] ComparerKeyList = {
            "Power_Section_OkDesc",
            "Power_Section_CancelDesc",
            "Power_SectionPowerAgenda_SwitchPowerPlanDesc",
            "Power_SectionPowerAgenda_SetMonitorBrightnessDesc",
            "Power_SectionPowerAgenda_DailyDesc",
            "Power_SectionPowerAgenda_WeeklyDesc",
            "Power_SectionPowerAgenda_SundayDesc",
            "Power_SectionPowerAgenda_MondayDesc",
            "Power_SectionPowerAgenda_TuesdayDesc",
            "Power_SectionPowerAgenda_WednesdayDesc",
            "Power_SectionPowerAgenda_ThursdayDesc",
            "Power_SectionPowerAgenda_FridayDesc",
            "Power_SectionPowerAgenda_SaturdayDesc",
            "Power_SectionPowerPlan_RestoreDefaultConfirmDesc",
            "Power_SectionPowerPlan_ExistsConfirmDesc",
            "Power_SectionPowerPlan_DeleteConfirmDescs",
            "Power_SectionPowerGlobal_PowerButton_DonothingDesc",
            "Power_SectionPowerGlobal_PowerButton_SleepDesc",
            "Power_SectionPowerGlobal_PowerButton_HibernateDesc",
            "Power_SectionPowerGlobal_PowerButton_ShutdownDesc",
            "Power_SectionPowerPlan_CpuSpeed_HighestDesc",
            "Power_SectionPowerPlan_CpuSpeed_AdaptiveDesc",
            "Power_SectionPowerPlan_CpuSpeed_LowDesc",
            "Power_SectionPowerPlan_CpuSpeed_LowestDesc",
            "Power_SectionPowerPlan_RestoreDefaultDesc",
            "Power_SectionPowerPlan_DeleteTitle",
            "Power_SectionPowerPlan_DeleteTitle",
            "Power_SectionPowerAgenda_DeleteTitle",
            "Power_SectionPowerAgenda_NewTitle"
 };
        public async Task<bool> GetDocument(FileInfo file, List<Data> filter = null)
        {
            bool isSuccess = false;
            if (file == null)
            {
                Console.WriteLine("generate new resource file error: file is null");
                return false;
            }
            await System.Threading.Tasks.Task.Run(() => {
                try
                {
                    ProcessStatus = String.Format("generating is in progress: {0}", file.FullName);
                    Application application = new Application();
                    Document document = application.Documents.Open(file.FullName);
                   
                    var content = document.Content.Text.Trim();
                    var strArray = content.Split('\r');

                    var nLSList = NLSList.Instance.List;
                    var targetPath = "";

                    if (IsManualMode)
                    {
                        targetPath = file.DirectoryName + "\\" + nLSList.Where(x => x.Name == file.Directory.Name).FirstOrDefault()?.ISOCode + "_resource.txt";
                        ProcessStatus = String.Format("create and import to a new file {0}", targetPath);
                        Console.WriteLine("ImportTargetPath: {0}", targetPath);
                        ImportToNewFile(strArray, targetPath, filter);
                    }
                    else
                    {
                        targetPath = ImportTargetPath + "\\" + nLSList.Where(x => x.Name == file.Directory.Name).FirstOrDefault()?.ISOCode + "\\" + "resources.resw";
                        ProcessStatus = String.Format("import to {0}", targetPath);
                        Console.WriteLine("ImportTargetPath: {0}", targetPath);
                        ImportToTarget(strArray, targetPath);
                    }
                  
                    isSuccess = true;
                    ProcessStatus = String.Format("generating is done: {0}", file.FullName);
                    Console.WriteLine("generate new resource file done: {0}", file.FullName);
                }
                catch (Exception ex)
                {
                    ProcessStatus = String.Format("generating {0} error: {1}", file.FullName, ex.Message);
                    Console.WriteLine("generating {0} error: {1}", file.FullName, ex.Message);
                }
                finally
                {

                }
            });
          
            return isSuccess;
        }


        public List<Data> GetKey(FileInfo file)
        {
            var keyList = new List<Data>();
            //Document document = null;
            try
            {
                //Application application = new Application();
                //document = application.Documents.Open(file.FullName);
                //var content = document.Content.Text.Trim();
                var content = FileManager.FileRead(file.FullName);
                if (file.Extension == ".resw" && !string.IsNullOrWhiteSpace(content))
                {
                    var res = Serializer.Deserialize<Resources>(content);
                    if (res != null && res.DataList != null)
                    {
                        keyList.AddRange(res.DataList);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetKeyList {0} error: {1}", file.FullName, ex.Message);
            }
            finally
            {
                //KillProcess();
            }
            Console.WriteLine("GetKeyList done: {0}", file.FullName);
            return keyList;
        }

        public void ImportToNewFile(string[] strArray, string targetPath = "", List<Data> filter = null)
        {
            bool hasFilter = filter == null ? false : true;
            using (var stream = File.CreateText(targetPath))
            {
                for (int i = 0; i < KeyList.Count(); i++)
                {
                    Dictionary<string, string> keyValue = new Dictionary<string, string>();
                    if (hasFilter)
                    {
                        var finder = filter.Find(x => x.Name == KeyList[i].Name);
                        // if (filter.Contains(KeyList[i].Name))
                        if (finder != null)
                        {
                            keyValue.Add(KeyList[i].Name, strArray[i]);
                            SaveFile(keyValue, stream);
                        }
                    }
                    else
                    {
                        keyValue.Add(KeyList[i].Name, strArray[i]);
                        SaveFile(keyValue, stream);
                    }

                }
                //foreach (var sentance in strArray)
                //{
                //    //if (string.IsNullOrWhiteSpace(sentance)) continue;
                //    SaveFile(sentance, stream);
                //    //Console.WriteLine("Word line {0} ", sentance);
                //}
                stream.Close();
            }
        }

        public void ImportToTarget(string[] strArray, string targetPath = @"D:\\strings\\zh-Hans\\resources.resw", List<Data> filter = null)
        {

            try
            {
                bool hasFilter = filter == null ? false : true;
                var endStr = "</root>";
                var nLSList = NLSList.Instance.List;
                Encoding encoder = Encoding.UTF8;
                var str = "";
                //var filename = nLSList.Where(x => x.Name == file.Directory.Name).FirstOrDefault()?.ISOCode + "_resource.txt";
                using (FileStream fs = File.OpenWrite(targetPath))
                {
                    for (int i = 0; i < KeyList.Count(); i++)
                    {
                        str += string.Format("<data name=\"{0}\" xml:space=\"preserve\">\n", KeyList[i].Name);
                        str += string.Format("<value>{0}</value>\n", strArray[i]);
                        str += string.Format("</data>\n");
                        if (i == KeyList.Count() -1)
                        {
                            str += string.Format("</root>");
                        }

                    }
                    byte[] bytes = encoder.GetBytes(str);
                    fs.Position = fs.Length - endStr.Length;
                    fs.Write(bytes, 0, bytes.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("ImportToTarget {0} error: {1}", targetPath, ex.Message);
            }

            //using (var stream = File.(file.DirectoryName + "\\" + filename))
            //{
            //    for (int i = 0; i < KeyList.Count(); i++)
            //    {
            //        Dictionary<string, string> keyValue = new Dictionary<string, string>();
            //        if (hasFilter)
            //        {
            //            var finder = filter.Find(x => x.Name == KeyList[i].Name);
            //            // if (filter.Contains(KeyList[i].Name))
            //            if (finder != null)
            //            {
            //                keyValue.Add(KeyList[i].Name, strArray[i]);
            //                SaveFile(keyValue, stream);
            //            }
            //        }
            //        else
            //        {
            //            keyValue.Add(KeyList[i].Name, strArray[i]);
            //            SaveFile(keyValue, stream);
            //        }

            //    }
            //    stream.Close();
            //}
        }

        private long FindEndPosition(string targetPath)
        {
            long positon = 0;
            var content = FileManager.FileRead(targetPath);
            positon = content.IndexOf("</root>");
            return positon;
        }

        public void KillProcess(string processName = "WINWORD")
        {
            //获得进程对象，以用来操作  
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            //得到所有打开的进程   
            try
            {
                //获得需要杀死的进程名  
                //var processes = System.Diagnostics.Process.GetProcesses();
                var processesByName = System.Diagnostics.Process.GetProcessesByName(processName);
                foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcessesByName(processName))
                {
                    //立即杀死进程   www.2cto.com
                    thisproc.Kill();
                }
                Console.WriteLine("Kill process done: {0}", processesByName.Count());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kill process error: {0}", ex.Message);
            }
        }

        //private string TransferFilenameFromNLSToISO()
        //{

        //}

        /// <summary>
        /// 将word文档转换为xps文档
        /// </summary>
        /// <param name="wordDocName">word文档全路径</param>
        /// <param name="xpsDocName">xps文档全路径</param>
        /// <returns></returns>
        private void ConvertWordToXPS(string wordDocName, string xpsDocName)
        {
            //XpsDocument result = null;

            //创建一个word文档，并将要转换的文档添加到新创建的对象
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();

            try
            {

                //wordApplication.Documents.Add(wordDocName);
                //Document doc = wordApplication.ActiveDocument;
                //doc.ExportAsFixedFormat(xpsDocName, WdExportFormat.wdExportFormatXPS, false, WdExportOptimizeFor.wdExportOptimizeForPrint, WdExportRange.wdExportAllDocument, 0, 0, WdExportItem.wdExportDocumentContent, true, true, WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, true, false, Type.Missing);
               // result = new XpsDocument(xpsDocName, System.IO.FileAccess.ReadWrite);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);
            }

            wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);

           // return result;
        }
    }
}