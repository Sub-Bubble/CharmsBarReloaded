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
                        charmsBar.SlideInButtons(charmsConfig.EnableAnimations);
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
                        charmsBar.SlideInButtons(charmsConfig.EnableAnimations);
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
                    if (charmsConfig.EnableAnimations)
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeOut);
                    else
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationOut);
                }
            });
        }
    }
}
