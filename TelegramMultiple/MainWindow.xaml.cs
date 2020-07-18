using System;
using System.Windows;
using TelegramMultiple.Pages;

namespace TelegramMultiple
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Deactivated(object sender, EventArgs e) => TitleBar.Opacity = 0.5;
        private void Window_Activated(object sender, EventArgs e) => TitleBar.Opacity = 1;

        private void CloseButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
        private void HideButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
