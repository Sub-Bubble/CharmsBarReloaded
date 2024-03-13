using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.Settings
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : Page
    {
        public General()
        {
            InitializeComponent();
            hideOnClick.IsChecked = GlobalConfig.HideWindowAfterClick;
            enableCharmsBar.IsChecked = GlobalConfig.IsEnabled;
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

        private void HideOnClick_Update(object sender, RoutedEventArgs e)
        {
            GlobalConfig.HideWindowAfterClick = (bool)hideOnClick.IsChecked;
            GlobalConfig.SaveConfig();
        }

        private void CharmsBarEnabled_Update(object sender, RoutedEventArgs e)
        {
            GlobalConfig.IsEnabled = (bool)enableCharmsBar.IsChecked;
        }
        private void CharmsClockEnabled_Update(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException("not implemented yet.");
        }
    }
}
