using System.Text.Json;

namespace CharmsBarReloaded.Updater
{
    internal static class Program
    {
        public const string AppName = "CharmsBarReloaded";
        public readonly static string DefaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CharmsBarReloaded", "updaterConfig.json");
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
            }
            else
                Application.Run(new UpdaterForm());
        }
    }
}