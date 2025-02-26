using Microsoft.Win32;
using System.Diagnostics;
using System.IO.Compression;
namespace CharmsBarReloaded.Updater
{
    public static class Installer
    {
        public static async Task DownloadAsync(string installPath, string downloadLink, bool isPortable, IProgress<int> percentage, IProgress<StatusMessage> message)
        {
            string packagePath = Path.Combine(Program.TempFolder, Program.AppName + "_UpdatePackage");
            message.Report(new StatusMessage { Message = "Connecting to server...", Status = Status.Info });
            if (!Directory.Exists(Program.TempFolder))
                Directory.CreateDirectory(Program.TempFolder);

            bool isDownloaded = false;
            var progress = new Progress<int>(downloadPercentage =>
            {
                if (!isDownloaded)
                {
                    message.Report(new StatusMessage { Message = $"Downloading: {downloadPercentage}%", Status = Status.Info });
                    percentage.Report(downloadPercentage);
                }
            });

            await RemoteServer.DownloadPackage(downloadLink, packagePath, progress, RemoteServer.cancelToken.Token);
            isDownloaded = true;
            progress = null;
            Install(installPath, packagePath, isPortable, percentage, message);
            return;
        }

        public static void Install(string installPath, string packagePath, bool isPortable, IProgress<int> percentage, IProgress<StatusMessage> message)
        {
            try
            {
                if (Path.Exists(Program.TempFolder))
                    foreach (var dir in Directory.GetDirectories(Program.TempFolder))
                        Directory.Delete(dir, true);
                else
                    Directory.CreateDirectory(Program.TempFolder);

                message.Report(new StatusMessage { Message = $"Extracting package", Status = Status.Info });
                percentage.Report(0);

                ZipFile.ExtractToDirectory(packagePath, Path.Combine(Program.TempFolder, "UpdatePackage"));

                string updatePackagePath = Path.Combine(Program.TempFolder, "UpdatePackage");

                message.Report(new StatusMessage { Message = $"Replacing files", Status = Status.Info });
                if (Path.Exists(installPath))
                {
                    foreach (var file in Directory.GetFiles(installPath))
                        File.Delete(file);
                    foreach (var dir in Directory.GetDirectories(installPath))
                        Directory.Delete(dir, true);
                }
                else
                    Directory.CreateDirectory(installPath);

                percentage.Report(50);
                foreach (string dirPath in Directory.GetDirectories(updatePackagePath, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(updatePackagePath, installPath));

                foreach (string newPath in Directory.GetFiles(updatePackagePath, "*.*", SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(updatePackagePath, installPath), true);



                if (isPortable)
                {
                    Cleanup(message);
                    message.Report(new StatusMessage { Message = "Success!", Status = Status.Success });
                    return;
                }

                string versionString = "1.0";
                int build = 1;

                foreach (var file in new DirectoryInfo(installPath).GetFiles("*", SearchOption.AllDirectories))
                {
                    if (file.Name.Equals($"{Program.AppName}.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(file.FullName);
                        versionString = fileInfo.ProductVersion ?? "1.0";
                        if (Version.TryParse(fileInfo.FileVersion, out Version version))
                            build = version.Major;
                    }
                }

                message.Report(new StatusMessage { Message = $"Adding registry keys", Status = Status.Info });
                percentage.Report(90);

                if (Program.IsElevated)
                    RegistryEditor.AddInstallKey(Registry.LocalMachine, $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{Program.AppName}", versionString, build, installPath);
                else
                    RegistryEditor.AddInstallKey(Registry.CurrentUser, $"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{Program.AppName}", versionString, build, installPath);

                Cleanup(message);

                message.Report(new StatusMessage { Message = "Success!", Status = Status.Success });
                percentage.Report(100);
            }
            catch (UnauthorizedAccessException)
            {
                Cleanup(message);
                message.Report(new StatusMessage { Message = "File access error", Status = Status.Error });
            }
            catch (InvalidDataException)
            {
                Cleanup(message);
                message.Report(new StatusMessage { Message = "Broken archive", Status = Status.Error });
            }
            catch (Exception ex)
            {
                Cleanup(message);
                message.Report(new StatusMessage { Message = "Unknown error", Status = Status.Error });
            }
        }

        private static void Cleanup(IProgress<StatusMessage> message)
        {
            message.Report(new StatusMessage { Message = $"Cleaning up", Status = Status.Info });
            Directory.Delete(Program.TempFolder, true);
        }
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
                    catch { }
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
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The uninstaller had crashed!\n\nError message: {ex.Message}\n\nStack trace: {ex.StackTrace}", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}