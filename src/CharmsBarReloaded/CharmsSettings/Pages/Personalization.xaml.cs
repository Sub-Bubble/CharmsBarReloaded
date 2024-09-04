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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CharmsBarReloaded.CharmsSettings.Pages
{
    /// <summary>
    /// Interaction logic for Personalization.xaml
    /// </summary>
    public partial class Personalization : Page
    {
        string onText;
        string offText;
        public Personalization()
        {
            InitializeComponent();

            ReloadStrings();
            BarBackgroundPicker.SelectedColor = (Color)ColorConverter.ConvertFromString($"#FF{App.charmsConfig.charmsBarConfig.BackgroundColor}");
            BarHoverPicker.SelectedColor = (Color)ColorConverter.ConvertFromString($"#FF{App.charmsConfig.charmsBarConfig.HoverColor}");
            BarTextColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString($"#FF{App.charmsConfig.charmsBarConfig.TextColor}");

            ShowChargingOnDesktopToggle.IsChecked = App.charmsConfig.charmsClockConfig.ShowChargingOnDesktop;
            SyncClockSettingsToggle.IsChecked = App.charmsConfig.charmsClockConfig.SyncClockSettings;

            ClockBackgroundPicker.SelectedColor = (Color)ColorConverter.ConvertFromString($"#FF{App.charmsConfig.charmsClockConfig.BackgroundColor}");
            ClockTextColorPicker.SelectedColor = (Color)ColorConverter.ConvertFromString($"#FF{App.charmsConfig.charmsClockConfig.TextColor}");
        }
        #region back button
        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButtonClicked.png", UriKind.Relative));
        }
        private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
            Color color = BarBackgroundPicker.SelectedColor;
            App.charmsConfig.charmsBarConfig.BackgroundColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            color = BarHoverPicker.SelectedColor;
            App.charmsConfig.charmsBarConfig.HoverColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            color = BarTextColorPicker.SelectedColor;
            App.charmsConfig.charmsBarConfig.TextColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            color = ClockBackgroundPicker.SelectedColor;
            App.charmsConfig.charmsClockConfig.BackgroundColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            color = ClockTextColorPicker.SelectedColor;
            App.charmsConfig.charmsClockConfig.TextColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";

            App.ClickHandler("SettingsHome");
        }
        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButtonMouseOver.png", UriKind.Relative));
        }
        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
        }
        #endregion back button
        #region loading strings
        public void ReloadStrings()
        {
            onText = App.translationManager.GetTranslation("CharmsSettings.On");
            offText = App.translationManager.GetTranslation("CharmsSettings.Off");

            personalizationTitle.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Personalization");

            CharmsBar.Text = App.translationManager.GetTranslation("CharmsSettings.CharmsBar");
            BarBackgroundText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.BarBackground");
            BarHoverText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.BarHover");
            BarTextColorText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.BarText");

            CharmsClock.Text = App.translationManager.GetTranslation("CharmsSettings.CharmsClock");
            ShowChargingOnDesktopText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.ShowChargingOnDesktop");
            if (App.charmsConfig.charmsClockConfig.ShowChargingOnDesktop) ShowChargingOnDesktopOnOff.Text = onText;
            else ShowChargingOnDesktopOnOff.Text = offText;
            SyncClockSettingsText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.SyncClockSettings");
            if (App.charmsConfig.charmsClockConfig.SyncClockSettings) SyncClockSettingsOnOff.Text = onText;
            else SyncClockSettingsOnOff.Text = offText;
            ClockBackgroundText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.ClockBackground");
            ClockTextColorText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.ClockText");
        }
        #endregion loading strings

        private void ShowChargingOnDesktopToggle_Click(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.charmsClockConfig.ShowChargingOnDesktop = (bool)ShowChargingOnDesktopToggle.IsChecked;
        }

        private void SyncClockSettingsToggle_Click(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.charmsClockConfig.SyncClockSettings = (bool)SyncClockSettingsToggle.IsChecked;
        }
    }
}
