using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded
{
    public partial class App
    {
        private void CharmsBarReloaded_Update(object? sender, System.Timers.ElapsedEventArgs e)
        {
            CharmsBar_Update();
            charmsClock.Dispatcher.Invoke(() => { charmsClock.Update(charmsConfig.charmsClockConfig); });
            Settings_Update();
        }
        private void CharmsBar_Update()
        {
            charmsBar.Dispatcher.Invoke(() =>
            {
                Point cursorPos = SystemConfig.GetMouseLocation;

                if (cursorPos.X + 1 == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width && cursorPos.Y == SystemConfig.GetDesktopWorkingArea.Top && charmsConfig.charmsBarConfig.IsEnabled && !charmsBar.windowVisible)
                {
                    charmsBar.windowVisible = true;
                    if (!charmsBar.isAnimating)
                    {
                        charmsBar.Background = GetBrush.GetSpecialBrush("Transparent");
                        charmsBar.charmsStack.Visibility = Visibility.Visible;
                        charmsBar.SlideInButtons();
                    }
                }
                if (Keyboard.IsKeyDown(Key.LWin) && Keyboard.IsKeyDown(Key.C) && charmsConfig.charmsBarConfig.EnableKeyboardShortcut && (charmsConfig.charmsBarConfig.IsEnabled || charmsConfig.charmsBarConfig.KeyboardShortcutOverridesOffSetting))
                {
                    if (!charmsBar.isAnimating && !charmsBar.windowVisible)
                    {
                        charmsBar.Background = GetBrush.GetBrushFromHex(charmsConfig.charmsBarConfig.BackgroundColor);
                        charmsBar.charmsStack.Visibility = Visibility.Visible;
                        charmsBar.windowVisible = true;
                        charmsBar.isAnimating = true;
                        charmsBar.SlideInButtons();
                        charmsBar.BeginStoryboard(charmsBar.fadeIn);
                        if (charmsConfig.EnableAnimations)
                            charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeIn);
                        else
                            charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationIn);
                        foreach (Grid grid in charmsBar.charmsStack.Children)
                        {
                            foreach (var item in grid.Children)
                            {
                                if (item.GetType() == typeof(Image))
                                {
                                    string source = ((Image)item).Source.ToString();
                                    ((Image)item).Source = new BitmapImage(new Uri(source.Replace("Preview", "")));
                                }
                                if (item.GetType() == typeof(Label))
                                    ((Label)item).Visibility = Visibility.Visible;
                                if (item.GetType() == typeof(Grid))
                                    ((Grid)item).Background = SystemConfig.GetAccentColor;
                            }
                        }
                    }
                    charmsBar.Height = SystemParameters.PrimaryScreenHeight;
                    charmsBar.Top = 0;
                }
                if (Keyboard.IsKeyDown(Key.Escape) && charmsConfig.charmsBarConfig.EnableKeyboardShortcut)
                {
                    charmsBar.HideWindow();
                    if (charmsConfig.EnableAnimations && charmsClock.Opacity != 0)
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeOut);
                    else
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationOut);
                }
            });
        }
        private void CharmsBarReloaded_WifiUpdate(object? sender, System.Timers.ElapsedEventArgs e)
        {
            charmsClock.Dispatcher.Invoke(charmsClock.UpdateInternetStatus);
            settingsHome.Dispatcher.Invoke(settingsHome.UpdateInternetStatus);
        }
        int i = 0;
        private void Settings_Update()
        {
            if (i == 20)
            {
                settingsHome.Dispatcher.Invoke(() =>
                {
                    if (SystemConfig.GetDeviceVolume == 0 || SystemConfig.IsVolumeMuted)
                        settingsHome.volumeImage.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/VolumeMute.png", UriKind.Relative));
                    else settingsHome.volumeImage.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/Volume.png", UriKind.Relative));

                    int brightness = SystemConfig.DeviceBrightness;
                    if (brightness == -1)
                    {
                        settingsHome.brightnessText.Text = translationManager.GetTranslation("CharmsSettings.Home.Unavailable");
                        settingsHome.brightnessSlider.IsEnabled = false;
                        settingsHome.brightnessSliderText.Text = translationManager.GetTranslation("CharmsSettings.Home.Unavailable");

                    }
                    else settingsHome.brightnessText.Text = brightness.ToString();

                    settingsHome.keyboardLayout.Text = SystemConfig.GetKeyboardLayout;

                    settingsHome.volumeText.Text = SystemConfig.GetDeviceVolume.ToString();
                });
                i = 0;
            }
            i++;
        }
    }
}
