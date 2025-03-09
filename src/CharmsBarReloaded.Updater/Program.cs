using System.Diagnostics;
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
        public readonly static string TempFolder = Path.Combine(Path.GetTempPath(), AppName);
        public static readonly string[] systemRegKeys =
        {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
        };
        public const string UpdateServerUrl = @"https://raw.githubusercontent.com/Sub-Bubble/CharmsBarReloaded/updates/updates.json";
        public const string userRegKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
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
                string installPath = string.Empty;
                int? build = null;
                string customServerUrl = UpdateServerUrl;

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
                        RemoteServer.CheckForUpdates(true, useCustomServer, customServerUrl, args.Contains("quiet")).GetAwaiter().GetResult();
                    else if (args.Contains("stable") || args.Length == 1)
                        RemoteServer.CheckForUpdates(false, useCustomServer, customServerUrl, args.Contains("quiet")).GetAwaiter().GetResult();
                    Environment.Exit(0);
                }

                if (args[0] == "-uninstall")
                    Installer.Uninstall();

                if (args[0] == ("-install"))
                    action = "install";
                if (args.Contains("-build"))
                    build = int.Parse(args[Array.IndexOf(args, "-build") + 1]);
                if (args.Contains("-installpath"))
                    installPath = args[Array.IndexOf(args, "-installpath") + 1];
                if (args.Contains("-customserver"))
                    customServerUrl = args[Array.IndexOf(args, "-customserver") + 1];
                Application.Run(new UpdaterForm(action, args.Contains("-includebetas"), args.Contains("-includelegacy"), args.Contains("-portable"),
                    build, installPath, customServerUrl));
            }
        }
        public static void Elevate(string[] args)
        {/*
            try
            {*/
                string tempExePath = Path.Combine(TempFolder, "CharmsBarReloaded.Updater.exe");
                if (!Directory.Exists(TempFolder))
                    Directory.CreateDirectory(TempFolder);
                File.Copy(Process.GetCurrentProcess().MainModule.FileName, tempExePath, true);
                Process.Start(new ProcessStartInfo
                {
                    FileName = tempExePath,
                    Arguments = string.Join(" ", args),
                    Verb = "runas",
                    UseShellExecute = true
                });
                Environment.Exit(0);/*
            }
            catch
            {
                MessageBox.Show("Failed to get administrator privileges!\nPlease, accept UAC prompt", "Admin request rejected!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }
        public static void UpdateRestart(string[] args)
        {
            string tempExePath = Path.Combine(TempFolder, "CharmsBarReloaded.Updater.exe");
            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
            File.Copy(Process.GetCurrentProcess().MainModule.FileName, tempExePath, true);
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = tempExePath,
                    Arguments = string.Join(" ", args),
                    UseShellExecute = true
                });
                Environment.Exit(0);
            }
            catch
            {
                MessageBox.Show("Could not restart updater from temp folder!");
            }
        }
    }
}