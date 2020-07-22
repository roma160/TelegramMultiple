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
using System.Windows.Forms;
using TelegramMultiple.Classes;
using Application = System.Windows.Forms.Application;

namespace TelegramMultiple.Pages
{
    /// <summary>
    /// Interaction logic for DownloadTelegram.xaml
    /// </summary>
    public partial class DownloadTelegram : Page
    {
        public DownloadTelegram()
        {
            InitializeComponent();
        }

        private void DownloadTelegramButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(new PopUpWindow("Telegram downloading",
                "Specify location, in which you want to save Telegram.",
                PopUpWindow.PopUpType.Cancelable).ShowDialog() ?? false)) return;
            
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select folder to save Telegram";
            dialog.SelectedPath = Application.StartupPath;
            dialog.ShowNewFolderButton = true;
            if (dialog.ShowDialog() != DialogResult.OK) return;

            PopUpLoadingProcess loadingWindow = new PopUpLoadingProcess(
                "Downloading Telegram", "Downloading packages...");
            loadingWindow.ShowOnTop();
            TelegramLocating.DownloadTelegram(dialog.SelectedPath, 
                progress =>
                {
                    if(progress == 100)
                        loadingWindow.UpdateMessageText("Extracting files...");
                    else loadingWindow.UpdateProgress(progress);
                },
                progress =>
                {
                    if (progress == 100)
                    {
                        loadingWindow.Close();
                        new PopUpWindow("Downloading finished", "").ShowDialog();
                    }
                    else if(progress == -1)
                    {
                        loadingWindow.Close();
                        new PopUpWindow("Downloading failed", 
                            "Can`t find Telegram.exe in archive.").ShowDialog();
                    }
                    else loadingWindow.UpdateProgress(progress);
                });
        }
    }
}
