using CharmsBarReloaded.CharmsSettings;
using CharmsBarReloaded.CharmsSettings.Pages;
using CharmsBarReloaded.Config;
using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.Windows;
using Timer = System.Timers.Timer;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
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
        private static CharmsBar.CharmsBar charmsBar;
        private static CharmsClock.CharmsClock charmsClock;
        public static CharmsConfig charmsConfig { get; set; }
        private static SettingsWindow charmsSettings;
        private static Home settingsHome;
        private static General settingsGeneral;
        private static Personalization settingsPersonalization;
        private static About settingsAbout;
        private static TaskbarIcon tray;
        public static TranslationManager translationManager { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            Log.ClearPreviousLog();
            if (!Debugger.IsAttached)
            {
                this.DispatcherUnhandledException += (sender, e) =>
                {
                    MessageBox.Show("Application has just crashed!\nYou can find error logs in %AppData%\\Roadming\\CharmsBarReloaded\\latest.log");
                    Log.Fatal(e.Exception, "Unhandled exception!");
                    Environment.Exit(-1);
                };
                AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
                {
                    MessageBox.Show("Application has just crashed!\nYou can find error logs in %AppData%\\Roadming\\CharmsBarReloaded\\latest.log");
                    Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception!");
                    Environment.Exit(-1);
                };
            }

            Log.Info("Starting CharmsBarReloaded...");
            Log.Info("Loading Configuration");
            charmsConfig = new CharmsConfig().Load();

            Log.Info("Loading Translations");
            translationManager = new TranslationManager().Load(charmsConfig.CurrentLocale);
            
            Log.Info("Loading Charms Clock");
            charmsClock = new();
            charmsClock.Show();
            HideWindowFromAltTab(charmsClock);
            Log.Info("Loaded Charms Clock successfully!");

            Log.Info("Loading Charms Bar");
            charmsBar = new CharmsBar.CharmsBar(ref charmsClock);
            charmsBar.Show();
            HideWindowFromAltTab(charmsBar);
            charmsBar.Height = SystemParameters.PrimaryScreenHeight;
            Log.Info("Loaded Charms Bar successfully!");

            Log.Info("Loading Settings");
            charmsSettings = new();
            settingsHome = new();
            settingsGeneral = new();
            settingsPersonalization = new();
            settingsAbout = new();
            charmsSettings.Show();
            HideWindowFromAltTab(charmsSettings);
            charmsSettings.Hide();
            charmsSettings.frame.Content = settingsHome;
            charmsSettings.settingsSlideOut.Completed += (sender, args) =>
            {
                charmsSettings.frame.Content = settingsHome;
                charmsSettings.Hide();
            };
            Log.Info("Charms Settings loaded!");

            Log.Info("Loading Tray icon");
            tray = (TaskbarIcon)FindResource("TaskbarIcon");
            Log.Info("Tray icon loaded!");


            Timer timer = new();
            timer.Interval = 10;
            timer.Elapsed += CharmsBarReloaded_Update;
            timer.Start();

            Timer wifiTimer = new();
            wifiTimer.Interval = 2500;
            wifiTimer.Elapsed += CharmsBarReloaded_WifiUpdate;
            wifiTimer.Start();

            if (charmsConfig.AutoCheckForUpdates)
                ClickHandler("CheckForUpdatesSilent");
        }
    }
}
