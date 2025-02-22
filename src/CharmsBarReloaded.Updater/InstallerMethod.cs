using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Windows.Forms;

namespace CharmsBarReloaded.Updater
{
    public partial class UpdaterForm
    {
        private static readonly string TempFolder = Path.Combine(Path.GetTempPath(), Program.AppName);
        public async Task DownloadAsync(string installPath, string downloadLink, bool isPortable, string packagePath = "")
        {
            if (!string.IsNullOrWhiteSpace(packagePath))
                Install(installPath, packagePath, isPortable);
            packagePath = Path.Combine(TempFolder, Program.AppName + "_UpdatePackage");
            try
            {
                bool isDownloaded = false;
                UpdateStatus("Connecting to server...");
                if (!Directory.Exists(TempFolder))
                    Directory.CreateDirectory(TempFolder);
                await RemoteServer.DownloadPackage(downloadLink, packagePath, new Progress<int>(percentage =>
                {
                    if (!isDownloaded)
                    {
                        progressBar.Value = percentage;
                        statusLabel.Text = $"Downloading: {percentage}%";
                    }
                }), RemoteServer.cancelToken.Token);
                isDownloaded = true;
            }
            catch (OperationCanceledException ex)
            {
                Directory.Delete(TempFolder, true);
                progressBar.Value = 0;
                RemoteServer.cancelToken.Dispose();
                RemoteServer.cancelToken = new CancellationTokenSource();
                UpdateStatus("Ready", "info");
            }
            Install(installPath, packagePath, isPortable);
            return;
        }

        private void Install(string installPath, string packagePath, bool isPortable)
        {
            cancelButton.Enabled = false;
            try
            {
                if (Path.Exists(TempFolder))
                    foreach (var dir in Directory.GetDirectories(TempFolder))
                        Directory.Delete(dir, true);
                else
                    Directory.CreateDirectory(TempFolder);

                UpdateStatus("Extracting package");
                progressBar.Value = 0;
                ZipFile.ExtractToDirectory(packagePath, Path.Combine(TempFolder, "UpdatePackage"));

                string updatePackagePath = Path.Combine(TempFolder, "UpdatePackage");

                UpdateStatus("Replacing files");
                if (Path.Exists(installPath))
                {
                    foreach (var file in Directory.GetFiles(installPath))
                        File.Delete(file);
                    foreach (var dir in Directory.GetDirectories(installPath))
                        Directory.Delete(dir, true);
                }
                else
                    Directory.CreateDirectory(installPath);

                progressBar.Value = 50;
                foreach (string dirPath in Directory.GetDirectories(updatePackagePath, "*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(updatePackagePath, installPath));

                foreach (string newPath in Directory.GetFiles(updatePackagePath, "*.*", SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(updatePackagePath, installPath), true);



                if (isPortable)
                {
                    Cleanup(TempFolder);
                    ApplicationInstallSuccess(true);
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


                UpdateStatus("Adding registry keys");
                progressBar.Value = 90;
                if (Program.IsElevated)
                    RegistryEditor.AddInstallKey(Registry.LocalMachine, $"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{Program.AppName}", versionString, build, installPath);
                else
                    RegistryEditor.AddInstallKey(Registry.CurrentUser, $"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{Program.AppName}", versionString, build, installPath);

                UpdateStatus("Cleaning up");
                Cleanup(TempFolder);

                ApplicationInstallSuccess(false);
            }
            catch (UnauthorizedAccessException)
            {
                if (MessageBox.Show($"Failed to install {Program.AppDisplayName} to \"{installPath}\".{(Program.IsElevated ? " Please, choose another folder." : "Do you want to try installing with administrator privileges?")}",
                    "File access error", (Program.IsElevated ? MessageBoxButtons.OK : MessageBoxButtons.YesNo), MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    var args = new List<string>
                    {
                        "-install",
                        $"-version {filteredUpdates[versionSelector.SelectedIndex].versionName}",
                        $"-build {filteredUpdates[versionSelector.SelectedIndex].build}",
                        $"-installpath \"{installPath}\""
                    };

                    if (customServerRadio.Checked)
                        args.Add($"-customserver \"{customServerTextBox.Text}\"");

                    if (isPortable)
                        args.Add("-portable");

                    if (includeBetasCheckbox.Checked)
                        args.Add("-includebetas");

                    if (includeLegacyCheckbox.Checked)
                        args.Add("-includelegacy");

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{AppDomain.CurrentDomain.FriendlyName}.exe"),
                        Arguments = string.Join(" ", args),
                        Verb = "runas",
                        UseShellExecute = true
                    });
                    Environment.Exit(0);
                }
                else
                {
                    Cleanup(TempFolder);
                    UpdateStatus("Failed to copy files", "error");
                }
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("Broken update archive, cannot extract!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cleanup(TempFolder);
                UpdateStatus("Broken archive", "error");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unknown error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Cleanup(TempFolder);
                UpdateStatus("Unknown error", "error");
            }
        }

        private void Cleanup(string Folder)
        {
            UpdateStatus("Cleaning up", "info");
            Directory.Delete(Folder, true);
        }
    }
}
