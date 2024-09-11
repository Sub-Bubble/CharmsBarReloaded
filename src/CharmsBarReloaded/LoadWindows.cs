using CharmsBarReloaded.CharmsSettings;
using CharmsBarReloaded.CharmsSettings.Pages;
using CharmsBarReloaded.Config;
using Hardcodet.Wpf.TaskbarNotification;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded
{
    public partial class App
    {
        #region hide window from alt tab
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private static void HideWindowFromAltTab(Window Window)
        {
            SetWindowLong(new WindowInteropHelper(Window).Handle, -20, (GetWindowLong(new WindowInteropHelper(Window).Handle, -20) | 0x00000080) & ~0x00040000);
        }
        #endregion hide window from alt tab
        private static void LoadCharmsBar()
        {
            charmsBar = new CharmsBar.CharmsBar();
            charmsBar.MouseLeave += (sender, args) =>
            {
                charmsBar.HideWindow();
                if (charmsConfig.EnableAnimations)
                    charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeOut);
                else
                    charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationOut);
                charmsBar.Top = 1;
            };
            charmsBar.Loaded += (sender, args) =>
            {
                charmsBar.Top = SystemConfig.GetDesktopWorkingArea.Top + 1;
                HideWindowFromAltTab(charmsBar);
                charmsBar.Window_Reload();
                charmsBar.Left = SystemConfig.GetDesktopWorkingArea.Right - charmsBar.Width;
                charmsBar.charmsStack.Width = charmsBar.windowWidth;
                charmsBar.Width = charmsBar.windowWidth;
                charmsBar.MinHeight = SystemParameters.PrimaryScreenHeight - 1;
                charmsBar.HideWindow();
            };
            charmsBar.charmsStack.MouseEnter += (sender, args) =>
            {
                if (!charmsBar.isAnimating)
                {
                    charmsBar.isAnimating = true;
                    charmsBar.BeginStoryboard(charmsBar.fadeIn);
                    if (charmsConfig.EnableAnimations)
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeIn);
                    else
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationIn);
                }
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

                charmsBar.Top = 0;
            };

            charmsBar.Height = SystemParameters.PrimaryScreenHeight;
            charmsBar.Show();
        }
        private void LoadCharmsClock()
        {
            charmsClock = new CharmsClock.CharmsClock();
            charmsClock.Loaded += (sender, args) =>
            {
                charmsClock.Top = SystemConfig.GetDesktopWorkingArea.Bottom - 188;
                charmsClock.Left = 50;
                if (!charmsConfig.charmsClockConfig.SyncClockSettings)
                {
                    charmsClock.Background = GetBrush.GetBrushFromHex(charmsConfig.charmsClockConfig.BackgroundColor);
                    var brush = GetBrush.GetBrushFromHex(charmsConfig.charmsClockConfig.TextColor);
                    charmsClock.Hours.Foreground = brush;
                    charmsClock.Minutes.Foreground = brush;
                    charmsClock.Separator.Foreground = brush;
                    charmsClock.Date.Foreground = brush;
                }
                else
                    //omitting using charms bar text color is not a mistake
                    charmsClock.Background = GetBrush.GetBrushFromHex(charmsConfig.charmsBarConfig.BackgroundColor);
                HideWindowFromAltTab(charmsClock);
                charmsClock.Update(charmsConfig.charmsClockConfig);
                charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationOut);
            };
            charmsClock.Show();
            Log.Info("Loaded Charms Clock successfully!");
        }
        private void LoadSettings()
        {
            charmsSettings = new SettingsWindow();
            settingsHome = new Home();

            charmsSettings.Left = SystemConfig.GetDesktopWorkingArea.Width - 360;
            charmsSettings.Height = SystemConfig.GetDesktopWorkingArea.Height;
            charmsSettings.Top = 0;
            charmsSettings.frame.Background = SystemConfig.GetAccentColor;
            charmsSettings.frame.Content = settingsHome;
            Storyboard settingsSlideIn = (Storyboard)charmsSettings.FindResource("SlideIn");
            Storyboard settingsSlideOut = (Storyboard)charmsSettings.FindResource("SlideOut");
            settingsSlideOut.Completed += (sender, args) =>
            {
                charmsSettings.frame.Content = settingsHome;
                charmsSettings.Hide();
            };
            charmsSettings.Loaded += (sender, args) =>
            {
                if (charmsConfig.EnableAnimations)
                    charmsSettings.BeginStoryboard(settingsSlideIn);
                HideWindowFromAltTab(charmsSettings);
            };
            charmsSettings.Deactivated += (sender, args) =>
            {
                if (charmsSettings.isBusy) { return; }
                if (charmsSettings.frame.Content != settingsHome)
                {
                    charmsConfig.Save();
                    translationManager = new TranslationManager().Load(charmsConfig.CurrentLocale);
                    charmsBar = new();
                    LoadCharmsBar();
                    settingsHome.ReloadStrings();
                }
                if (charmsConfig.EnableAnimations)
                {
                    charmsSettings.BeginStoryboard(settingsSlideOut);
                }
                else
                {
                    charmsSettings.frame.Content = settingsHome;
                    charmsSettings.Hide();
                }
            };
            charmsSettings.Activated += (sender, args) =>
            {
                if (charmsConfig.EnableAnimations && !charmsSettings.isBusy)
                    charmsSettings.BeginStoryboard(settingsSlideIn);
                charmsSettings.isBusy = false;
            };

            Log.Info("Charms Settings loaded!");
        }
        private void LoadTray()
        {
            tray = (TaskbarIcon)FindResource("TaskbarIcon");
            Log.Info("Tray loaded!");
        }
    }
}
