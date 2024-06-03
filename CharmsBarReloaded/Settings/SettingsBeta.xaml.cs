using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for SettingsBeta.xaml
    /// </summary>
    public partial class SettingsBeta : Window
    {
        public SettingsBeta()
        {
            InitializeComponent();/*
            this.Loaded += delegate {
                MessageBox.Show("Old settings are going to be deleted once a5.0 is released.\nLast version to include them will not have this warning.",
                    "Old settings deprecation", MessageBoxButton.OK, MessageBoxImage.Information);
            };*/
            /// about
            VersionString.Content = $"CharmsBar: Reloaded {GlobalConfig.VersionString}\nBuild {GlobalConfig.Build}";

            /// general
            // general
            if (!SystemConfig.StartupKeyExists())
            {
                RunOnStartup.IsChecked = false;
            }
            else
            {
                RunOnStartup.IsChecked = true;
            }
            HideOnClick.IsChecked = GlobalConfig.HideWindowAfterClick;
            EnableCharmsBar.IsChecked = GlobalConfig.IsEnabled;
            EnableCharmsClock.IsChecked = GlobalConfig.CharmsClockEnabled;

            //keyboard shortcut
            EnableKeyboardShortcut.IsChecked = GlobalConfig.EnableKeyboardShortcut;
            OverrideDisabledCharmsBar.IsChecked = GlobalConfig.OverrideCharmsBarOffSetting;

            /// customization
            // general
            EnableAnimations.IsChecked = GlobalConfig.EnableAnimations;
            OverrideAccentColor_Toggle.IsChecked = GlobalConfig.OverrideAccentColorEnabled;
            OverrideAccentColor.IsEnabled = GlobalConfig.OverrideAccentColorEnabled;
            OverrideAccentColor.Text = GlobalConfig.OverrideAccentColor;

            //charmsBar.Color
            BackgroundColor.Text = GlobalConfig.BackgroundColor;
            TextColor.Text = GlobalConfig.TextColor;
            HoverColor.Text = GlobalConfig.HoverColor;

            //charmsClock.General
            ShowChargingOnDesktop.IsChecked = GlobalConfig.ShowChargingOnDesktop;
            //charmsClock.Color
            //todo
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            /// general stuff
            if (TextColor.Text.Length != 6 || BackgroundColor.Text.Length != 6 || HoverColor.Text.Length != 6) {
                MessageBox.Show("Invalid hex colors.\nConfig not saved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // fail if we try to delete the key that didnt exist. 
            try
            {
                SystemConfig.SetupStartupKey((bool)RunOnStartup.IsChecked);
            }
            catch { }


            /// general
            // general
            GlobalConfig.HideWindowAfterClick = (bool)HideOnClick.IsChecked;
            GlobalConfig.IsEnabled = (bool)EnableCharmsBar.IsChecked;
            GlobalConfig.CharmsClockEnabled = (bool)EnableCharmsClock.IsChecked;

            // keyboard shortcut
            GlobalConfig.EnableKeyboardShortcut = (bool)EnableKeyboardShortcut.IsChecked;
            GlobalConfig.OverrideCharmsBarOffSetting = (bool)OverrideDisabledCharmsBar.IsChecked;

            /// customization
            // general
            GlobalConfig.EnableAnimations = (bool)EnableAnimations.IsChecked;
            GlobalConfig.OverrideAccentColorEnabled = (bool)OverrideAccentColor_Toggle.IsChecked;
            GlobalConfig.OverrideAccentColor = OverrideAccentColor.Text;

            // charmsBar.color
            GlobalConfig.BackgroundColor = BackgroundColor.Text;
            GlobalConfig.TextColor = TextColor.Text;
            GlobalConfig.HoverColor = HoverColor.Text;
            // charmsclock.general
            GlobalConfig.ShowChargingOnDesktop = (bool)ShowChargingOnDesktop.IsChecked;
            //charmsclock.color
            /// todo
            

            GlobalConfig.SaveConfig();
            
            //checking config, just in case
            try
            {
                GlobalConfig.GetConfig("bg");
                GlobalConfig.GetConfig("hover");
                GlobalConfig.GetConfig("text");
                GlobalConfig.GetConfig("overrideAccentColor");
                GlobalConfig.SaveConfig();
                MessageBox.Show("Config saved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Error setting config.\nMaybe wrong formatting?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ValidHexCheck(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9A-Fa-f]{1}$");
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void NewSettings(object sender, RoutedEventArgs e)
        {
            ClickHandler.Do(-1);
            this.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CustomizatonScrollBar.Height = this.Height - 200;
        }

        private void OverrideAccentColor_Toggle_Click(object sender, RoutedEventArgs e)
        {
            OverrideAccentColor.IsEnabled = (bool)OverrideAccentColor_Toggle.IsChecked;
        }
    }
}
