using System;
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
                string action = string.Empty;
                bool includeBetas = false;
                bool includeLegacy = false;
                bool isPortable = false;
                string version = string.Empty;
                int build = -1;

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
                    Application.Exit();
                }
                if (args[0] == "-uninstall")
                {
                    Installer.Uninstall();
                    Environment.Exit(0);
                }

                if (args.Contains("-includebetas"))
                    includeBetas = true;
                if (args.Contains("-includelegacy"))
                    includeLegacy = true;
                if (args.Contains("-portable"))
                    isPortable = true;
                if (args.Contains("-build"))
                    build = int.Parse(args[Array.IndexOf(args, "-build") + 1]);
                if (args.Contains("-version"))
                    version = args[Array.IndexOf(args, "-version") + 1];
                if (args.Contains("-customserver"))
                    customServerUrl = args[Array.IndexOf(args, "-customserver") + 1];
                if (args.Contains("-install"))
                    action = "install";
                Application.Run(new UpdaterForm(action, includeBetas, includeLegacy));
            }
            else
                Application.Run(new UpdaterForm());
        }
    }
}