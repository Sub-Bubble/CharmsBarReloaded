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

            if (args.Length == 0)
                Application.Run(new UpdaterForm());
            else
            {
                string action = string.Empty;
                bool includeBetas = false;
                bool includeLegacy = false;
                bool isPortable = false;
                string installPath = string.Empty;
                int? build = null;
                string customServerUrl = @"http://localhost/updates.json";

                if (args[0] == ("-checkforupdates"))
                {
                    bool useCustomServer = true;
                    if (File.Exists(DefaultConfigPath))
                    {
                        try
                        {
                            var settings = JsonSerializer.Deserialize<UpdaterSettings>(File.ReadAllText(DefaultConfigPath));
                            customServerUrl = settings.customUpdateServer;
                            useCustomServer = settings.useCustomUpdateServer;
                        }
                        catch { }
                    }
                    if (args.Contains("beta"))
                        RemoteServer.CheckForUpdates(true, useCustomServer, customServerUrl).GetAwaiter().GetResult();
                    else if (args.Contains("stable") || args.Length == 1)
                        RemoteServer.CheckForUpdates(false, useCustomServer, customServerUrl).GetAwaiter().GetResult();
                    Application.Exit();
                }

                if (args[0] == "-uninstall")
                    Installer.Uninstall();

                if (args[0] == ("-install"))
                    action = "install";

                if (args.Contains("-includebetas"))
                    includeBetas = true;
                if (args.Contains("-includelegacy"))
                    includeLegacy = true;
                if (args.Contains("-portable"))
                    isPortable = true;
                if (args.Contains("-build"))
                    build = int.Parse(args[Array.IndexOf(args, "-build") + 1]);
                if (args.Contains("-installpath"))
                    installPath = args[Array.IndexOf(args, "-installpath") + 1];
                if (args.Contains("-customserver"))
                    customServerUrl = args[Array.IndexOf(args, "-customserver") + 1];
                Application.Run(new UpdaterForm(action, includeBetas, includeLegacy, isPortable, build, installPath, customServerUrl));
            }
        }
    }
}