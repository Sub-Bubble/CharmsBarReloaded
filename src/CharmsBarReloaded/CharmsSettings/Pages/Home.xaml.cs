﻿using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

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

            SleepText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Power.Sleep");
            ShutdownText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Power.Shutdown");
            RestartText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Power.Restart");

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
        public void UpdateInternetStatus()
        {
            networkImage.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/CharmsClock/{SystemConfig.GetInternetStatus}.png"));
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
            brightnessPopup.IsOpen = !brightnessPopup.IsOpen;
        }

        private void brightnessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            brightnessSlider.Value = Math.Round(brightnessSlider.Value, 0);
            brightnessText.Text = brightnessSlider.Value.ToString();
            SystemConfig.DeviceBrightness = (int)brightnessSlider.Value;
        }

        private void Power_MouseDown(object sender, MouseButtonEventArgs e)
        {
            shutdownPopup.IsOpen = !shutdownPopup.IsOpen;
        }

        private void Sleep_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("Sleep");
        }

        private void Shutdown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("Shutdown");
        }

        private void Restart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            App.ClickHandler("Restart");
        }
    }
}