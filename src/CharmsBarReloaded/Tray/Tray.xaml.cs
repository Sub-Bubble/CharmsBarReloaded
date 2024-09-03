using System.Windows;

namespace CharmsBarReloaded.Tray
{
    public partial class Tray
    {
        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            checkmark.Header = App.translationManager.GetTranslation("CharmsSettings.General.CharmsBarEnabled");
            openSettings.Header = App.translationManager.GetTranslation("CharmsBar.Settings");
            exitApp.Header = App.translationManager.GetTranslation("Tray.Exit");
            checkmark.IsChecked = App.charmsConfig.charmsBarConfig.IsEnabled;
        }
        private void ExitApp(object sender, EventArgs e)
        {
            Log.Info("Exiting Charms Bar: Reloaded");
            Environment.Exit(0);
        }
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            App.ClickHandler("Settings");
        }
        private void ToggleCharmsBar(object sender, RoutedEventArgs e)
        {
            checkmark.IsChecked = !App.charmsConfig.charmsBarConfig.IsEnabled;
            App.charmsConfig.charmsBarConfig.IsEnabled = !App.charmsConfig.charmsBarConfig.IsEnabled;
            App.charmsConfig.Save();
        }
    }
}
