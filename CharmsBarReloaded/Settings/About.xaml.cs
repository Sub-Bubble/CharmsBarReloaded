using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
            versionString.Text = $"Version: {GlobalConfig.VersionString}";
            buildString.Text = $"Build: {GlobalConfig.Build}";
        }
        #region back button
        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/BackButtonClicked.png", UriKind.Relative));
        }
        private void BackButton_Click(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(0);
        }

        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/BackButtonMouseOver.png", UriKind.Relative));
        }

        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
        }
        #endregion back button
    }
}
