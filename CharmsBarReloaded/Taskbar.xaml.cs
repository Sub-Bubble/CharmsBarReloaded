using System.Windows;

namespace CharmsBarReloaded
{
    public partial class Taskbar
    {
        internal void OpenOldSettings(object sender, RoutedEventArgs e)
        {
            ClickHandler.Do(-2);
        }
        internal void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            ClickHandler.Do(-1);
        }
        private void ToggleCharmsBar(object sender, RoutedEventArgs e)
        {
            Checkmark.IsChecked = !GlobalConfig.IsEnabled;
            GlobalConfig.IsEnabled = !GlobalConfig.IsEnabled;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            Checkmark.IsChecked = GlobalConfig.IsEnabled;
        }
    }
}
