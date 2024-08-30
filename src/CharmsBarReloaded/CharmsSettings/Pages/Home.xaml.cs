using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CharmsBarReloaded.CharmsSettings.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            settingsTitle.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Title");
            settingsSubTitle.Text = App.translationManager.GetTranslation("CharmsSettings.Home.SubTitle");
            
            generalSettings.Text = App.translationManager.GetTranslation("CharmsSettings.Home.General");
            personalizationSettings.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Personalization");
            aboutSettings.Text = App.translationManager.GetTranslation("CharmsSettings.Home.About");

            osSettingsSubTitle.Text = App.translationManager.GetTranslation("CharmsSettings.Home.OsSettingsSubTitle");
            osSettings.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Settings");
            controlPanel.Text = App.translationManager.GetTranslation("CharmsSettings.Home.ControlPanel");

            networkText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Network");
            notificationsText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Notifications");
            powerText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Power");
            keyboardText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Keyboard");

        }

        private void General_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("SettingsGeneral");
        }

        private void Personalization_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("SettingsPersonalization");
        }

        private void About_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("SettingsAbout");
        }

        private void OsSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("OsSettings");
        }

        private void ControlPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("ControlPanel");
        }

        private void Network_MouseUp(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("Network");
        }
        private void KeyboardLayout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("ChangeKeyboardLayout");
        }

        private void volume_MouseUp(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("VolumeSettings");
        }

        private void Notifications_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("Notifications");
        }

        private void Brightness_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("in the works");
        }

        private void Power_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("in the works");
        }

        private void Sleep_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("in the works");
        }

        private void Shutdown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("in the works");
        }

        private void Restart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("in the works");
        }
    }
}
