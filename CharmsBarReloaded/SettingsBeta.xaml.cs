using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            InitializeComponent();
            //RunOnStartup.IsChecked = false;
            HideOnClick.IsChecked = GlobalConfig.HideWindowAfterClick;
            BackgroundColor.Text = GlobalConfig.BackgroundColor;
            TextColor.Text = GlobalConfig.TextColor;
            HoverColor.Text = GlobalConfig.HoverColor;

        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            try
            {
                GlobalConfig.HideWindowAfterClick = (bool)HideOnClick.IsChecked;
                GlobalConfig.BackgroundColor = BackgroundColor.Text;
                GlobalConfig.TextColor = TextColor.Text;
                GlobalConfig.HoverColor = HoverColor.Text;
                GlobalConfig.SaveConfig();
            }
            catch
            {
                MessageBox.Show("Error setting config.\nMaybe wrong formatting?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
