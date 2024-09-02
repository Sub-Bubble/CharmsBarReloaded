using CharmsBarReloaded.Config;
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
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : Page
    {
        string onText;
        string offText;
        public General(CharmsConfig charmsConfig)
        {
            InitializeComponent();

            onText = App.translationManager.GetTranslation("CharmsSettings.On");
            offText = App.translationManager.GetTranslation("CharmsSettings.Off");

            generalTitle.Text = App.translationManager.GetTranslation("CharmsSettings.Home.General");

            LanguageText.Text = App.translationManager.GetTranslation("CharmsSettings.General.Language");

            EnableAnimationText.Text = App.translationManager.GetTranslation("CharmsSettings.General.EnableAnimations");
            EnableAnimationsToggle.IsChecked = charmsConfig.EnableAnimations;
            if (charmsConfig.EnableAnimations) EnableAnimationsOnOff.Text = onText;
            EnableAnimationsOnOff.Text = offText;

            RunOnStartupText.Text = App.translationManager.GetTranslation("CharmsSettings.General.RunOnStartup");
            RunOnStartupToggle.IsChecked = SystemConfig.StartupKeyExists;
            if (SystemConfig.StartupKeyExists) RunOnStartupOnOff.Text = onText;
            RunOnStartupOnOff.Text = offText;

            BarEnabledText.Text = App.translationManager.GetTranslation("CharmsSettings.General.CharmsBarEnabled");
            BarEnabledToggle.IsChecked = charmsConfig.EnableAnimations;
            if (charmsConfig.EnableAnimations) BarEnabledOnOff.Text = onText;
            BarEnabledOnOff.Text = offText;

            ClockEnabledText.Text = App.translationManager.GetTranslation("CharmsSettings.General.CharmsClockEnabled");
            ClockEnabledToggle.IsChecked = charmsConfig.EnableAnimations;
            if (charmsConfig.EnableAnimations) ClockEnabledOnOff.Text = onText;
            ClockEnabledOnOff.Text = offText;

            KeyboardShortcutsText.Text = App.translationManager.GetTranslation("CharmsSettings.General.EnableKeyboardShortcuts");
            KeyboardShortcutsToggle.IsChecked = charmsConfig.EnableAnimations;
            if (charmsConfig.EnableAnimations) KeyboardShortcutsOnOff.Text = onText;
            KeyboardShortcutsOnOff.Text = offText;

            KeyboardShortcutOverrideText.Text = App.translationManager.GetTranslation("CharmsSettings.General.KeyboardShortcutsOverrideCharmsBarOff");
            KeyboardShortcutOverrideToggle.IsChecked = charmsConfig.EnableAnimations;
            if (charmsConfig.EnableAnimations) KeyboardShortcutOverrideOnOff.Text = onText;
            KeyboardShortcutOverrideOnOff.Text = offText;
        }
        #region back button
        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButtonClicked.png", UriKind.Relative));
        }
        private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
            App.charmsConfig.Save();
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

        private void EnableAnimationsToggle_Checked(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.EnableAnimations = (bool)EnableAnimationsToggle.IsChecked;
        }

        private void RunOnStartupToggle_Checked(object sender, RoutedEventArgs e)
        {
            SystemConfig.SetupStartupKey(RunOnStartupToggle.IsChecked);
        }

        private void BarEnabledToggle_Checked(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.charmsBarConfig.IsEnabled = (bool)BarEnabledToggle.IsChecked;
        }

        private void ClockEnabledToggle_Checked(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.charmsClockConfig.IsEnabled = (bool)ClockEnabledToggle.IsChecked;
        }

        private void KeyboardShortcutsToggle_Checked(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.charmsBarConfig.EnableKeyboardShortcut = (bool)KeyboardShortcutsToggle.IsChecked;
        }

        private void KeyboardShortcutOverrideToggle_Checked(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.charmsBarConfig.KeyboardShortcutOverridesOffSetting = (bool)KeyboardShortcutOverrideToggle.IsChecked;
        }
    }
}
