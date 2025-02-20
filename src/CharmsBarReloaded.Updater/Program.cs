using System.Security.Principal;
using System.Text.Json;

namespace CharmsBarReloaded.Updater
{
    internal static class Program
    {
        public const string AppName = "CharmsBarReloaded";
        public const string AppDisplayName = "CharmsBar: Reloaded";
        public const string AboutURL = "https://github.com/Sub-Bubble/CharmsBarReloaded";
        public const string IssueLink = "https://github.com/Sub-Bubble/CharmsBarReloaded/issues";
        public const string Publisher = "Sub-Bubble";
        public readonly static string DefaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CharmsBarReloaded", "updaterConfig.json");
        public static bool IsElevated => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            if (args.Length > 0)
            {
                bool useCustomServer = true;
                string customServerUrl = @"http://localhost";
                if (File.Exists(DefaultConfigPath))
                {
                    try
                    {
                        var settings = JsonSerializer.Deserialize<UpdaterSettings>(File.ReadAllText(DefaultConfigPath));
                        customServerUrl = settings.customUpdateServer;
                        useCustomServer = settings.useCustomUpdateServer;
                    } catch { }
                }
                
                if (args.Contains("-checkforupdates"))
                {
                    if (args.Contains("beta"))
                        RemoteServer.CheckForUpdates(true, useCustomServer, customServerUrl).GetAwaiter().GetResult();
                    else if (args.Contains("stable") || args.Length == 1)
                        RemoteServer.CheckForUpdates(false, useCustomServer, customServerUrl).GetAwaiter().GetResult();
                }
                if (args.Contains("-includebetas"))
                    Application.Run(new UpdaterForm(true));
                if (args[0] == "-install")
                {
                    string version = args[Array.IndexOf(args, "-version")+1];
                    int build = int.Parse(args[Array.IndexOf(args, "-build") + 1]);

                    string CustomServer = args.Contains("-customserver") ? args[Array.IndexOf(args, "-customserver") + 1] : string.Empty;
                    bool isPortable = args.Contains("-portable");
                    bool includeBetas = args.Contains("-includebetas");
                    bool includeLegacy = args.Contains("-includelegacy"); ;
                    //Application.Run(new UpdaterForm(includeBetas, includeLegacy, version, build, isPortable, CustomServer));
                }
            }
            else
                Application.Run(new UpdaterForm());
        }
    }
}