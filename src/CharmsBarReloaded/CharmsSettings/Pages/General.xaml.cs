using CharmsBarReloaded.Config;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WinRT;
namespace CharmsBarReloaded.CharmsSettings.Pages
{
    /// <summary>
    /// Interaction logic for General.xaml
    /// </summary>
    public partial class General : Page
    {
        string onText;
        string offText;
        public General()
        {
            InitializeComponent();

            ReloadStrings();
            LoadLanguageSelector();
            LanguageNeedRestart.Visibility = Visibility.Collapsed;
            EnableAnimationsToggle.IsChecked = App.charmsConfig.EnableAnimations;
            RunOnStartupToggle.IsChecked = SystemConfig.StartupKeyExists;
            BarEnabledToggle.IsChecked = App.charmsConfig.charmsBarConfig.IsEnabled;
            BarHideAfterClickToggle.IsChecked = App.charmsConfig.charmsBarConfig.HideWindowAfterClick;
            ClockEnabledToggle.IsChecked = App.charmsConfig.EnableAnimations;
            KeyboardShortcutsToggle.IsChecked = App.charmsConfig.EnableAnimations;
            KeyboardShortcutOverrideToggle.IsChecked = App.charmsConfig.EnableAnimations;
            AutoCheckForUpdatesToggle.IsChecked = App.charmsConfig.AutoCheckForUpdates;
            BetaProgramOptInToggle.IsChecked = App.charmsConfig.BetaProgramOptIn;
        }
        #region language selector loading
        private void LoadLanguageSelector()
        {
            if (!Directory.Exists(@"lang"))
            {
                Log.Error("Language folder doesn't exist. That's bad. Expect errors");
            }

            string[] langFiles = Directory.GetFiles("lang");
            string codes = string.Empty;
            foreach (string langFile in langFiles)
            {
                string langCode = Path.GetFileNameWithoutExtension(langFile);
                try
                {
                    CultureInfo cultureInfo = new CultureInfo(langCode);
                    string displayName = cultureInfo.DisplayName;
                    int percentage = (int)Math.Round((App.translationManager.GetKeysAmount(langCode) / (double)App.translationManager.TotalKeys) * 100);
                    LanguageItem languageItem = new LanguageItem
                    {
                        DisplayName = $"{displayName} | {percentage}%",
                        LanguageCode = langCode,
                    };

                    LanguageSelector.Items.Add(languageItem);

                    if (langCode == App.charmsConfig.CurrentLocale)
                        LanguageSelector.SelectedItem = languageItem;

                }
                catch (CultureNotFoundException)
                {
                    LanguageSelector.Items.Add(new LanguageItem
                    {
                        DisplayName = langCode,
                        LanguageCode = langCode,
                    });
                }
            }

            if (LanguageSelector.SelectedIndex == -1)
            {
                LanguageSelector.SelectedItem = LanguageSelector.Items.Cast<LanguageItem>().FirstOrDefault(item => item.LanguageCode == "en-us");
            }

        }
        private class LanguageItem
        {
            public string DisplayName { get; set; }
            public string LanguageCode { get; set; }
            public override string ToString()
            {
                return DisplayName;
            }
        }
        #endregion language selector loading
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
        #region loading strings
        public void ReloadStrings()
        {

            onText = App.translationManager.GetTranslation("CharmsSettings.On");
            offText = App.translationManager.GetTranslation("CharmsSettings.Off");

            generalTitle.Text = App.translationManager.GetTranslation("CharmsSettings.Home.General");

            LanguageText.Text = App.translationManager.GetTranslation("CharmsSettings.General.Language");
            LanguageNeedRestart.Text = App.translationManager.GetTranslation("CharmsSettings.General.LanguageNeedRestart");

            EnableAnimationText.Text = App.translationManager.GetTranslation("CharmsSettings.General.EnableAnimations");
            if (App.charmsConfig.EnableAnimations) EnableAnimationsOnOff.Text = onText;
            else EnableAnimationsOnOff.Text = offText;

            RunOnStartupText.Text = App.translationManager.GetTranslation("CharmsSettings.General.RunOnStartup");
            if (SystemConfig.StartupKeyExists) RunOnStartupOnOff.Text = onText;
            else RunOnStartupOnOff.Text = offText;

            BarEnabledText.Text = App.translationManager.GetTranslation("CharmsSettings.General.CharmsBarEnabled");
            if (App.charmsConfig.charmsBarConfig.IsEnabled) BarEnabledOnOff.Text = onText;
            else BarEnabledOnOff.Text = offText;

            BarHideAfterClickText.Text = App.translationManager.GetTranslation("CharmsSettings.General.HideBarAfterClick");
            if (App.charmsConfig.charmsBarConfig.HideWindowAfterClick) BarHideAfterClickOnOff.Text = onText;
            else BarHideAfterClickOnOff.Text = offText;

            ClockEnabledText.Text = App.translationManager.GetTranslation("CharmsSettings.General.CharmsClockEnabled");
            if (App.charmsConfig.EnableAnimations) ClockEnabledOnOff.Text = onText;
            else ClockEnabledOnOff.Text = offText;

            KeyboardShortcutsText.Text = App.translationManager.GetTranslation("CharmsSettings.General.EnableKeyboardShortcuts");
            if (App.charmsConfig.EnableAnimations) KeyboardShortcutsOnOff.Text = onText;
            else KeyboardShortcutsOnOff.Text = offText;

            KeyboardShortcutOverrideText.Text = App.translationManager.GetTranslation("CharmsSettings.General.KeyboardShortcutsOverrideCharmsBarOff");
            if (App.charmsConfig.EnableAnimations) KeyboardShortcutOverrideOnOff.Text = onText;
            else KeyboardShortcutOverrideOnOff.Text = offText;

            AutoCheckForUpdatesText.Text = App.translationManager.GetTranslation("CharmsSettings.General.AutomaticCheckForUpdates");
            if (App.charmsConfig.EnableAnimations) AutoCheckForUpdatesOnOff.Text = onText;
            else AutoCheckForUpdatesOnOff.Text = offText;

            BetaProgramOptInText.Text = App.translationManager.GetTranslation("CharmsSettings.General.BetaProgramOptIn");
            if (App.charmsConfig.EnableAnimations) BetaProgramOptInOnOff.Text = onText;
            else BetaProgramOptInOnOff.Text = offText;

            OpenUpdaterText.Text = App.translationManager.GetTranslation("CharmsSettings.General.OpenUpdater");
            OpenUpdaterBtn.Content = App.translationManager.GetTranslation("CharmsSettings.General.Updater");
        }
        #endregion loading strings

        private void LanguageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.charmsConfig.CurrentLocale = LanguageSelector.SelectedItem.As<LanguageItem>().LanguageCode;
            LanguageNeedRestart.Visibility = Visibility.Visible;
        }

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
        private void BarHideAfterClickToggle_Click(object sender, RoutedEventArgs e)
        {
            App.charmsConfig.charmsBarConfig.HideWindowAfterClick = (bool)BarHideAfterClickToggle.IsChecked;
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

        private void AutoCheckForUpdatesToggle_Checked(Object sender, RoutedEventArgs e)
        {
            App.charmsConfig.AutoCheckForUpdates = (bool)AutoCheckForUpdatesToggle.IsChecked;
        }

        private void BetaProgramOptInToggle_Checked(Object sender, RoutedEventArgs e)
        {
            App.charmsConfig.BetaProgramOptIn = (bool)BetaProgramOptInToggle.IsChecked;
        }

        private void OpenUpdaterBtn_Click(object sender, RoutedEventArgs e)
        {
            App.ClickHandler("OpenUpdater");
        }
    }
}
