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
using System.Text.RegularExpressions;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for SettingsBeta.xaml
    /// </summary>
    public partial class SettingsBeta : Window
    {
        public SettingsBeta()
        {
            InitializeComponent();
            VersionString.Content = $"CharmsBar: Reloaded {GlobalConfig.VersionString}\nBuild {GlobalConfig.Build}";
            //RunOnStartup.IsChecked = false;
            HideOnClick.IsChecked = GlobalConfig.HideWindowAfterClick;
            BackgroundColor.Text = GlobalConfig.BackgroundColor;
            TextColor.Text = GlobalConfig.TextColor;
            HoverColor.Text = GlobalConfig.HoverColor;
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            if (TextColor.Text.Length != 6 || BackgroundColor.Text.Length != 6 || HoverColor.Text.Length != 6) {
                MessageBox.Show("Invalid hex colors.\nConfig not saved.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GlobalConfig.HideWindowAfterClick = (bool)HideOnClick.IsChecked;
            GlobalConfig.BackgroundColor = BackgroundColor.Text;
            GlobalConfig.TextColor = TextColor.Text;
            GlobalConfig.HoverColor = HoverColor.Text;
            GlobalConfig.ShowChargingOnDesktop = (bool)ShowChargingOnDesktop.IsChecked;
            GlobalConfig.SaveConfig();
            //checking config, just in case
            try
            {
                GlobalConfig.GetConfig("bg");
                GlobalConfig.GetConfig("text");
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
    }
}
