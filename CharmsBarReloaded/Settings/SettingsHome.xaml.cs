using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CharmsBarReloaded.Settings
{
    /// <summary>
    /// Interaction logic for SettingsHome.xaml
    /// </summary>
    public partial class SettingsHome : Page
    {
        public SettingsHome()
        {
            InitializeComponent();
        }

        private void General_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(1);
        }

        private void Custiomization_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(2);
        }

        private void About_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(3);
        }
        private void OldSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.Do(-2);
        }

        private void OsSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.Do(-3);
        }

        private void ControlPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.Do(-4);
        }
    }
}
