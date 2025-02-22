using Microsoft.Win32;
using System.Diagnostics;
namespace CharmsBarReloaded.Updater
{
    public static class Installer
    {
        public static void Uninstall()
        {
            string exeName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName);
            InstallDetector.IsInstalled();
            try
            {
                foreach (string file in Directory.GetFiles(InstallDetector.InstallPath, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    catch
                    {
                    }
                }

                if (Program.IsElevated)
                {
                    RegistryEditor.RemoveRegistryKey(Registry.LocalMachine, $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
                    RegistryEditor.RemoveRegistryKey(Registry.LocalMachine, $"SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall");
                }
                RegistryEditor.RemoveRegistryKey(Registry.CurrentUser, $"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall");

                MessageBox.Show("Done!");


                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/C ping 1.1.1.1 -n 1 -w 3000 > Nul & RD /s /q " + Path.GetDirectoryName(Application.ExecutablePath),
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = Path.GetTempPath()
                };
                Process.Start(psi);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The installed had crashed!\nError message: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}