using Microsoft.Win32;

namespace CharmsBarReloaded.Updater
{
    public class RegistryEditor
    {
        public static void AddInstallKey(RegistryKey rootKey, string registryPath, string displayVersion, int build, string installLocation)
        {
            using (RegistryKey registryKey = rootKey.CreateSubKey(registryPath))
            {
                registryKey.SetValue("DisplayName", Program.AppDisplayName, RegistryValueKind.String);
                registryKey.SetValue("UninstallString", Path.Combine(installLocation, "Updater.exe"), RegistryValueKind.String);
                registryKey.SetValue("DisplayVersion", displayVersion, RegistryValueKind.String);
                registryKey.SetValue("MajorVersion", build, RegistryValueKind.DWord);
                registryKey.SetValue("MinorVersion", 0, RegistryValueKind.DWord);
                registryKey.SetValue("Publisher", Program.Publisher, RegistryValueKind.String);
                registryKey.SetValue("InstallLocation", installLocation, RegistryValueKind.String);
                registryKey.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"), RegistryValueKind.String);
                registryKey.SetValue("HelpLink", Program.IssueLink, RegistryValueKind.String);
                registryKey.SetValue("URLInfoAbout", Program.AboutURL, RegistryValueKind.String);
                registryKey.SetValue("DisplayIcon", Path.Combine(installLocation, $"{Program.AppName}.exe"), RegistryValueKind.String);
                registryKey.SetValue("ModifyPath", Path.Combine(installLocation, "Updater.exe"), RegistryValueKind.String);
            }
        }
        public static void RemoveRegistryKey(RegistryKey rootKey, string registryPath)
        {
            using (RegistryKey registryKey = rootKey.OpenSubKey(registryPath, true))
            {
                if (registryKey == null)
                    return;
                foreach (var subKeys in registryKey.GetSubKeyNames())
                    if (Program.AppName.Equals((string)subKeys, StringComparison.OrdinalIgnoreCase))
                        registryKey.DeleteSubKey(subKeys);
            }
        }
    }
}
