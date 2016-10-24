using NLSImportTool.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using NLSImportTool.Utilities;
using NLSImportTool.Model;

namespace NLSImportTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //RootFolderName = SelectedFolderTexBox_NLS.Text;
            // KeyFileName = SelectedFolderTexBox_key.Text;
            this.DataContext = MainViewModel.Instance;
            if (MainViewModel.Instance.IsManualMode)
            {
                ManualRaidoButton.IsChecked = true;
                AutoModeGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                AutoImportRaidoButton.IsChecked = true;
                AutoModeGrid.Visibility = Visibility.Visible;
            }
        }

        public string RootFolderName { get; set; }
        public string KeyFileName { get; set; }

       

        private void LoadKey_Button_Click(object sender, RoutedEventArgs e)
        {
            //ViewModelLocator.MainViewModelStatic.LoadKey();
        }

        private void Choose_TargeFolder_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Choose_KeyFile_Button_Click(object sender, RoutedEventArgs e)
        {
            //@"D:\\Documents\\Feature DPM contract plugin\\TU.DOCX"
            //SelectedFolderTexBox_key.Text = "";
            //ChooseKeyFileErrorText.Text = "";
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.RestoreDirectory = true;
            //openFileDialog.Filter = "Word documents(*.doc;*.docx)|*.doc;*.docx";

            //bool? result = openFileDialog.ShowDialog();
            //if (result == null || result == false) return;
            //KeyFileName = openFileDialog.FileName;
            //if (string.IsNullOrWhiteSpace(KeyFileName))
            //{
            //    ChooseKeyFileErrorText.Text = "chose your key file first";
            //    return;
            //}
            //SelectedFolderTexBox_key.Text = KeyFileName;
            string mFilePath = Environment.CurrentDirectory + "//" + "NLSKeys.config";
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            this.KeyContentRtb.Document.Blocks.Clear();
            MainViewModel.Instance.KillProcess();
        }

        private string GetText(RichTextBox richTextBox)
        {
            TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            return textRange.Text;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (SecondPageGrid.Visibility == Visibility.Visible)
            {
                FirstPageGrid.Visibility = Visibility.Visible;
                SecondPageGrid.Visibility = Visibility.Collapsed;
                BackButton.IsEnabled = false;
                NextButton.Visibility = Visibility.Visible;
                OKButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FirstPageGrid.Visibility == Visibility.Visible)
                {
                    FirstPageGrid.Visibility = Visibility.Collapsed;
                    SecondPageGrid.Visibility = Visibility.Visible;
                    BackButton.IsEnabled = true;
                    NextButton.Visibility = Visibility.Collapsed;
                    OKButton.Visibility = Visibility.Visible;

                    var text = GetText(this.KeyContentRtb);
                    MainViewModel.Instance.LoadKeyFromInput(text);
                }
            }
            catch (Exception ex)
            {

            }
        }

      
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var nlsPath = MainViewModel.Instance.NLSPath;
            
            MainViewModel.Instance.ExcuteMainProcess();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    
        private void Choose_TargetFolder_Button_Click(object sender, RoutedEventArgs e)
        {
            var folderDialo = new WPFFolderBrowser.WPFFolderBrowserDialog();
            bool? result = folderDialo.ShowDialog();
            if (result == null || result == false) return;
            MainViewModel.Instance.ImportTargetPath = folderDialo.FileName;
            //if (string.IsNullOrWhiteSpace(targetPath))
            //{
            //    // ChooseNLSrootFolderErrorText.Text = "chose your NLS root folder first";
            //    return;
            //}
            //SelectedFolderTexBox_ImportTarget.Text = targetPath;
        }
        private void Choose_Folder_Button_Click(object sender, RoutedEventArgs e)
        {
            //@"D:\\Documents\\Feature DPM contract plugin\\TU.DOCX"
            //SelectedFolderTexBox_NLS.Text = "";
            //ChooseNLSrootFolderErrorText.Text = "";
            var folderDialo = new WPFFolderBrowser.WPFFolderBrowserDialog();
            bool? result = folderDialo.ShowDialog();
            if (result == null || result == false) return;
            MainViewModel.Instance.NLSPath = folderDialo.FileName;
            //if (string.IsNullOrWhiteSpace(RootFolderName))
            //{
            //   // ChooseNLSrootFolderErrorText.Text = "chose your NLS root folder first";
            //    return;
            //} 
            // SelectedFolderTexBox_NLS.Text = RootFolderName;


            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.RestoreDirectory = true;
            //openFileDialog.Filter = "Word documents(*.doc;*.docx)|*.doc;*.docx";

            //bool? result = openFileDialog.ShowDialog();
            //if (result == true)
            //{
            //    if (openFileDialog.FileName.Length > 0)
            //    {
            //    }
            //}
        }
        private void ManualRaidoButton_Checked(object sender, RoutedEventArgs e)
        {
            MainViewModel.Instance.IsManualMode = (ManualRaidoButton.IsChecked == true) ? true : false;
            if (MainViewModel.Instance.IsManualMode)
            {
                AutoModeGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                AutoModeGrid.Visibility = Visibility.Visible;
            }
        }
    }
}
