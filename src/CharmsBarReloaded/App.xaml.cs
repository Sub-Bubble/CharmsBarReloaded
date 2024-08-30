using System.Diagnostics;
using System.Windows;
using CharmsBarReloaded.CharmsSettings;
using CharmsBarReloaded.CharmsSettings.Pages;
using CharmsBarReloaded.Config;
using Timer = System.Timers.Timer;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static CharmsBar.CharmsBar charmsBar;
        private static CharmsClock.CharmsClock charmsClock;
        private static CharmsConfig charmsConfig;
        private static SettingsWindow charmsSettings;
        private static Home settingsHome;
        private static General settingsGeneral;
        private static Personalization settingsPersonalization;
        private static About settingsAbout;
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

            Log.Info("Welcome!");
            Log.Info("Loading Configuration");
            charmsConfig = new CharmsConfig().Load();

            Log.Info("Loading Translations");
            translationManager = new TranslationManager().Load(charmsConfig.CurrentLocale);

            Log.Info("Loading Charms Bar");
            LoadCharmsBar();
            charmsBar.Height = SystemParameters.PrimaryScreenHeight;

            Log.Info("Loading Charms Clock");
            LoadCharmsClock();

            Log.Info("Loading Settings");
            LoadSettings();


            Timer timer = new Timer();
            timer.Interval = 10;
            timer.Elapsed += CharmsBarReloaded_Update;
            timer.Start();

        }
    }
}
