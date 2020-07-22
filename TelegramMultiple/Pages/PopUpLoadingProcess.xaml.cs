using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TelegramMultiple.Pages
{
    /// <summary>
    /// Interaction logic for PopUpLoadingProcess.xaml
    /// </summary>
    public partial class PopUpLoadingProcess : Window
    {
        public PopUpLoadingProcess(
            String messageTitle,
            String messageText = "",
            int initialProgress = 0)
        {
            InitializeComponent();

            MessageTitle.Text = messageTitle;
            MessageText.Text = messageText;
            Progress.Value = initialProgress;
        }

        public void ShowOnTop()
        {
            Thread showingThread = new Thread(() => ShowDialog());
            showingThread.Start();
        }

        public void UpdateProgress(int newProgress)
            => Progress.Value = newProgress;

        public void UpdateMessageText(String newText)
            => MessageText.Text = newText;
    }
}
