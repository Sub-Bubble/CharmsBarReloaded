using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

            UpdateButtonsStack();
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
            ButtonMappingText.Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.ButtonMapping");
            ButtonMappingPlus.Content = App.translationManager.GetTranslation("CharmsSettings.Plus");
            ButtonMappingMinus.Content = App.translationManager.GetTranslation("CharmsSettings.Minus");

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

        private void UpdateButtonsStack()
        {
            ButtonsStack.Children.Clear();

            for (int i = 0; i < App.charmsConfig.charmsBarConfig.ButtonActions.Length; i++)
            {
                int j = i;

                StackPanel stack = new StackPanel
                {
                    Width = 100,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                ComboBox comboBox = new ComboBox
                {
                    ItemsSource = CharmsConfig.CharmsBarConfig.ValidActions,
                    SelectedItem = App.charmsConfig.charmsBarConfig.ButtonActions[j],
                    Width = 100
                };
                comboBox.SelectionChanged += (sender, args) =>
                {
                    App.charmsConfig.charmsBarConfig.ButtonActions[j] = comboBox.SelectedItem.ToString();
                };
                CheckBox dynamicColorCheck = new CheckBox
                {
                    IsChecked = App.charmsConfig.charmsBarConfig.UsesDynamicColor[j],
                };
                dynamicColorCheck.Content = new TextBlock { TextWrapping = TextWrapping.Wrap, Text = App.translationManager.GetTranslation("CharmsSettings.Personalization.UseDynamicColor") };

                dynamicColorCheck.Click += (sender, args) =>
                {
                    App.charmsConfig.charmsBarConfig.UsesDynamicColor[j] = (bool)dynamicColorCheck.IsChecked;
                };

                stack.Children.Add(comboBox);
                stack.Children.Add(dynamicColorCheck);

                ButtonsStack.Children.Add(stack);
            }
        }
        private void ButtonMappingPlus_Click(object sender, RoutedEventArgs e)
        {
            string[] actions = App.charmsConfig.charmsBarConfig.ButtonActions;
            Array.Resize(ref actions, actions.Length + 1);
            App.charmsConfig.charmsBarConfig.ButtonActions = actions;

            bool[] dynamicColors = App.charmsConfig.charmsBarConfig.UsesDynamicColor;
            Array.Resize(ref dynamicColors, dynamicColors.Length + 1);
            App.charmsConfig.charmsBarConfig.UsesDynamicColor = dynamicColors;

            App.charmsConfig.charmsBarConfig.ButtonActions[App.charmsConfig.charmsBarConfig.ButtonActions.Length - 1] = "Search";
            App.charmsConfig.charmsBarConfig.UsesDynamicColor[App.charmsConfig.charmsBarConfig.UsesDynamicColor.Length - 1] = false;

            UpdateButtonsStack();
            App.charmsConfig.Save();
        }

        private void ButtonMappingMinus_Click(object sender, RoutedEventArgs e)
        {
            string[] actions = App.charmsConfig.charmsBarConfig.ButtonActions;
            Array.Resize(ref actions, actions.Length - 1);
            App.charmsConfig.charmsBarConfig.ButtonActions = actions;

            bool[] dynamicColors = App.charmsConfig.charmsBarConfig.UsesDynamicColor;
            Array.Resize(ref dynamicColors, dynamicColors.Length - 1);
            App.charmsConfig.charmsBarConfig.UsesDynamicColor = dynamicColors;

            UpdateButtonsStack();
        }

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
