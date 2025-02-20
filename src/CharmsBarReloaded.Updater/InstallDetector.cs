using Microsoft.Win32;
using System.Diagnostics;

namespace CharmsBarReloaded.Updater
{
    public class InstallDetector
    {
        private static bool? appInstalled = null;
        private static readonly string[] systemRegKeys =
                {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
            };
        private static readonly string userRegKey = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";

        public static string InstallPath { get; private set; } = string.Empty;
        public static string InstallRegPath { get; private set; } = string.Empty;
        public static bool IsPortable { get; private set; } = false;
        public static string VersionString { get; private set; } = string.Empty;
        public static int BuildNumber { get; private set; } = -1;
        public static bool AdminInstall { get; private set; } = false;

        public static bool IsInstalled(bool recheck = false)
        {
            if (appInstalled != null && !recheck)
                return appInstalled.Value;

            foreach (var key in systemRegKeys)
            {
                appInstalled = CheckRegistryKey(Registry.LocalMachine, key);
                if (appInstalled.Value == true)
                {
                    AdminInstall = true;
                    return true;
                }

            }

            appInstalled = CheckRegistryKey(Registry.CurrentUser, userRegKey);

            // checking installed in a the same folder as an executable if using old install or portable
            if (appInstalled.Value == false)
            {
                DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                foreach (var file in directory.GetFiles())
                    if (file.Name.Equals($"{Program.AppName}.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(file.FullName);
                        VersionString = fileInfo.ProductVersion ?? "Unknown";
                        if (Version.TryParse(fileInfo.FileVersion, out Version version))
                            BuildNumber = version.Major;
                        appInstalled = true;
                        InstallPath = AppDomain.CurrentDomain.BaseDirectory;
                        IsPortable = true;
                    }
            }

            return appInstalled.Value;
        }

        private static bool CheckRegistryKey(RegistryKey rootKey, string registryPath)
        {
            using (RegistryKey registryKey = rootKey.OpenSubKey(registryPath))
            {
                if (registryKey == null)
                    return false;
                foreach (var subKeys in registryKey.GetSubKeyNames())
                    if (Program.AppName.Equals((string)subKeys, StringComparison.OrdinalIgnoreCase))
                        using (var subKey = registryKey.OpenSubKey(subKeys))
                        {
                            InstallRegPath = subKey.ToString();
                            InstallPath = (string)subKey.GetValue("InstallLocation");
                            VersionString = (string)subKey.GetValue("DisplayVersion");
                            BuildNumber = (int)subKey.GetValue("MajorVersion");
                            IsPortable = false;
                            return true;
                        }
            }
            return false;
        }
    }
}
