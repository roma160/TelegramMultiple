using System;
using System.Windows;

namespace TelegramMultiple.Pages
{
    /// <summary>
    /// Interaction logic for PopUpWindow.xaml
    /// </summary>
    public partial class PopUpWindow : Window
    {
        public enum PopUpType
        {
            Default,
            Cancelable
        }

        public PopUpWindow(String popupTitle, String popupText, PopUpType type = PopUpType.Default)
        {
            InitializeComponent();

            MessageTitle.Text = popupTitle;
            MessageText.Text = popupText;

            if (type == PopUpType.Default)
                CancelButtonBorder.Visibility = Visibility.Collapsed;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
