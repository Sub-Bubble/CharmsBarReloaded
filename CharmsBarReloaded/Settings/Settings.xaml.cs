using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using CharmsBarReloaded.Settings;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for CharmsSettings.xaml
    /// </summary>
    public partial class CharmsSettings : Window
    {
        /// hiding window from alttab      
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;

        public static bool windowBusy = false;
        
        
        Storyboard slideInWindow;
        Storyboard slideOutWindow;
        System.Timers.Timer timer = new System.Timers.Timer();
        public CharmsSettings()
        {
            InitializeComponent();
            slideInWindow = (Storyboard)FindResource("SlideInAnimation");
            slideOutWindow = (Storyboard)FindResource("SlideOutAnimation");
            SettingsFrame.Content = new SettingsHome();

            this.Height = SystemParameters.PrimaryScreenHeight;
            SettingsGrid.Height = SystemParameters.PrimaryScreenHeight;
            this.Left = SystemConfig.DesktopWorkingArea.Right - this.Width;
            this.Top = SystemConfig.DesktopWorkingArea.Top;
            
            if (!GlobalConfig.OverrideAccentColorEnabled)
                SettingsGrid.Background = SystemConfig.AccentColor();
            else SettingsGrid.Background = GlobalConfig.GetConfig("overrideAccentColor");
            this.Loaded += delegate 
            { 
                SetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE, (GetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
                Animate();
                slideOutWindow.Completed += delegate { this.Hide(); SettingsFrame.Content = new SettingsHome(); /*Just in case*/ };
                timer.Elapsed += delegate
                {
                    this.Dispatcher.Invoke(new Action((delegate
                    {
                        if (!GlobalConfig.OverrideAccentColorEnabled)
                            SettingsGrid.Background = SystemConfig.AccentColor();
                        else SettingsGrid.Background = GlobalConfig.GetConfig("overrideAccentColor");
                    })));

            };
                timer.Interval = 100;
                timer.Start();
            };
        }
        public void Animate()
        {
            if (GlobalConfig.EnableAnimations)
                BeginStoryboard(slideInWindow);
        }
        public void HideWindow()
        {
            if (!windowBusy)
            {
                if (GlobalConfig.EnableAnimations)
                    BeginStoryboard(slideOutWindow);
                else
                {
                    this.Hide();
                    SettingsFrame.Content = new SettingsHome();
                }
            }
        }
    }
}
