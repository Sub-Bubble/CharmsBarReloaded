using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;


namespace CharmsBarReloaded.Updater
{
    [DesignerCategory("Form")]
    public partial class UpdaterForm : Form
    {
        List<UpdateItem> updates;
        private bool showAdvancedSettings = false;
        public UpdaterForm(bool includeBetas = false, bool includeLegacy = false)
        {
            InitializeComponent();
            if (InstallDetector.IsInstalled())
            {
                this.Text = "Updater";
                installedVersionLabel.Text = $"Installed: {InstallDetector.VersionString}";
                installPathTextBox.Text = InstallDetector.InstallPath;
                portableInstallCheckBox.Checked = InstallDetector.IsPortable;
                checkForUpdatesToolStripMenuItem.Enabled = true;
                checkForUpdatesincludeBetasToolStripMenuItem.Enabled = true;
                uninstallBtn.Enabled = true;
                if (InstallDetector.AdminInstall)
                    ShieldIcon.AddToButton(uninstallBtn);
            }
            else
            {
                this.Text = "Installer";
                installedVersionLabel.Text = $"Not installed!";
                portableInstallCheckBox.Checked = false;
                checkForUpdatesToolStripMenuItem.Enabled = false;
                checkForUpdatesincludeBetasToolStripMenuItem.Enabled = false;
                uninstallBtn.Enabled = false;
            }

            AdvancedSettings.Hide();

            // custom made config is always prioritized over a checkbox
            if (File.Exists(Program.DefaultConfigPath))
                LoadConfig(Program.DefaultConfigPath);
            else
            {
                if (includeBetas)
                    includeBetasCheckbox.Checked = true;
                if (includeLegacy)
                    includeLegacyCheckbox.Checked = true;

                officialServerRadio.Checked = false;

                customServerRadio.Checked = true;
                customServerTextBox.Enabled = true;
                customServerTextBox.Text = "http://localhost"; // temporary solution
                applyUpdateServerPathBtn.Enabled = true;
                if (!Program.IsElevated)
                    ShieldIcon.AddToButton(installButton);

                FetchRemoteUpdates(true);
            }
        }

        void LoadConfig(string configPath)
        {
            var settings = JsonSerializer.Deserialize<UpdaterSettings>(File.ReadAllText(configPath));
            includeBetasCheckbox.Checked = settings.includeBeta;
            includeLegacyCheckbox.Checked = settings.includeLegacy;
            portableInstallCheckBox.Checked = settings.doPortableInstall;
            if (settings.useCustomUpdateServer)
            {
                customServerRadio.Checked = true;
                officialServerRadio.Checked = false;
                customServerTextBox.Enabled = true;
                applyUpdateServerPathBtn.Enabled = true;
            }
            else
            {
                customServerRadio.Checked = false;
                officialServerRadio.Checked = true;
                customServerTextBox.Enabled = false;
                applyUpdateServerPathBtn.Enabled = false;
            }
            customServerTextBox.Text = settings.customUpdateServer;
            installPathTextBox.Text = settings.customInstallPath;

            FetchRemoteUpdates(settings.useCustomUpdateServer);
        }

        #region UI modification
        List<UpdateItem> filteredUpdates = new List<UpdateItem>();
        private void RepopulateUpdatesList()
        {
            versionSelector.Items.Clear();
            filteredUpdates.Clear();

            if (updates == null || updates.Count == 0)
            {
                UpdateStatus("Error parsing update list", "error", false);
                return;
            }

            versionSelector.Enabled = true;
            latestVersionLabel.ForeColor = Color.Black;
            statusLabel.ForeColor = Color.Black;

            foreach (var update in updates)
            {
                if ((update.isBeta && includeBetasCheckbox.Checked && !update.isLegacy) ||
                    (!update.isBeta && update.isLegacy && includeLegacyCheckbox.Checked) ||
                    (update.isBeta && includeBetasCheckbox.Checked && update.isLegacy && includeLegacyCheckbox.Checked) ||
                    (!update.isBeta && !update.isLegacy))
                {
                    versionSelector.Items.Add(update.versionName);
                    filteredUpdates.Add(update);
                }
            }
            versionSelector.SelectedIndex = 0;
            latestVersionLabel.Text = $"Latest: {filteredUpdates[0].versionName}";

            if (filteredUpdates[0].build > InstallDetector.BuildNumber && InstallDetector.IsInstalled())
                UpdateStatus("Update available!", "info", true, true);
            else
                UpdateStatus("Ready", "info");
            installButton.Enabled = true;
        }

        void UpdateStatus(string statusMessage, string type = "info", bool updatesAvailable = true, bool hasNewUpdate = false)
        {
            switch (type)
            {
                case "info":
                    if (!updatesAvailable) latestVersionLabel.Text = $"Latest version: Fetching";
                    if (hasNewUpdate) latestVersionLabel.ForeColor = Color.Green;
                    else latestVersionLabel.ForeColor = Color.Black;
                    statusLabel.ForeColor = Color.Black;
                    break;
                case "warning":
                    latestVersionLabel.ForeColor = Color.Orange;
                    statusLabel.ForeColor = Color.Orange;
                    break;
                case "error":
                    if (!updatesAvailable)
                    {
                        latestVersionLabel.ForeColor = Color.Red;
                        latestVersionLabel.Text = $"Latest version: Unavailable";
                    }
                    statusLabel.ForeColor = Color.Red;
                    break;
                case "success":
                    statusLabel.ForeColor = Color.Green;
                    break;
            }
            if (!updatesAvailable)
            {
                versionSelector.Enabled = false;
                versionSelector.Items.Add("Unavailable");
                versionSelector.SelectedIndex = 0;
            }

            statusLabel.Text = statusMessage;
            return;
        }
        #endregion UI modification
        #region UI interaction
        #region toolbar
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.Menu))
            {
                if (!this.updaterMenu.Visible)
                {
                    this.updaterMenu.Visible = true;
                    var OnMenuKey = updaterMenu.GetType().GetMethod("OnMenuKey",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);
                    OnMenuKey.Invoke(this.updaterMenu, null);
                }
                else
                {
                    this.updaterMenu.Visible = false;
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void updaterMenu_VisibleChanged(object sender, EventArgs e)
        {
            int menuHeight = updaterMenu.Height;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl != updaterMenu)
                {
                    ctrl.Top = updaterMenu.Visible ? ctrl.Top + menuHeight : ctrl.Top - menuHeight;
                }
            }
        }

        AboutWindow aboutWindow = new();
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            aboutWindow.ShowDialog();
            aboutWindow.Focus();
        }
        #endregion toolbar
        #region main ui
        private void installButton_Click(object sender, EventArgs e)
        {
            if (installPathTextBox.Text.Contains("\\Users\\") || portableInstallCheckBox.Checked || Program.IsElevated)
            {
                cancelButton.Enabled = true;
                installButton.Enabled = false;
                DownloadAsync(installPathTextBox.Text, filteredUpdates[versionSelector.SelectedIndex].downloadLink, portableInstallCheckBox.Checked);
            }
            else
            {
                var args = new List<string>
                    {
                        "-install",
                        $"-version {filteredUpdates[versionSelector.SelectedIndex].versionName}",
                        $"-build {filteredUpdates[versionSelector.SelectedIndex].build}",
                        $"-installpath \"{installPathTextBox.Text}\""
                    };

                if (customServerRadio.Checked)
                    args.Add($"-customserver \"{customServerTextBox.Text}\"");

                if (portableInstallCheckBox.Checked)
                    args.Add("-portable");

                if (includeBetasCheckbox.Checked)
                    args.Add("-includebetas");

                if (includeLegacyCheckbox.Checked)
                    args.Add("-includelegacy");
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = $"{AppDomain.CurrentDomain.BaseDirectory}{AppDomain.CurrentDomain.FriendlyName}.exe",
                        Arguments = $"-install -version {filteredUpdates[versionSelector.SelectedIndex].versionName} -build {filteredUpdates[versionSelector.SelectedIndex].build} -installpath {"\"" + installPathTextBox.Text + "\""} {(customServerRadio.Checked ? $"-customserver {"\"" + customServerTextBox.Text + "\""}" : "")} {(portableInstallCheckBox.Checked ? "-portable" : "")} {(includeBetasCheckbox.Checked ? "-includebetas" : "")} {(includeLegacyCheckbox.Checked ? "-includelegacy" : "")} ",
                        Verb = "runas",
                        UseShellExecute = true
                    });
                    Environment.Exit(0);
                }
                catch
                {
                    MessageBox.Show("failed to get administrator privileges!\nplease, install a portable instalation to a users folder or accept UAC prompt");
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            RemoteServer.StopDownload();
            installButton.Enabled = true;
            cancelButton.Enabled = false;
        }

        private void advancedSettingsButton_Click(object sender, EventArgs e)
        {
            if (showAdvancedSettings)
            {
                AdvancedSettings.Hide();
                showAdvancedSettings = false;
                advancedSettingsButton.Text = "▼ Advanced";
            }
            else
            {
                AdvancedSettings.Show();
                showAdvancedSettings = true;
                advancedSettingsButton.Text = "▲ Advanced";
            }
        }
        #endregion main ui
        #region advanced
        private void includeBetasCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            RepopulateUpdatesList();
        }

        private void includeLegacyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            RepopulateUpdatesList();
        }

        private void customServerRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (customServerRadio.Checked)
            {
                customServerTextBox.Enabled = true;
                applyUpdateServerPathBtn.Enabled = true;
            }
            else
            {
                customServerTextBox.Enabled = false;
                applyUpdateServerPathBtn.Enabled = false;
                FetchRemoteUpdates(false);
            }
        }

        private void applyUpdateServerPathBtn_Click(object sender, EventArgs e)
        {
            customServerTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(customServerTextBox.Text))
            {
                MessageBox.Show("Update server link can't be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!customServerTextBox.Text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !customServerTextBox.Text.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                customServerTextBox.Text = "http://" + customServerTextBox.Text;

            if (customServerTextBox.Text.EndsWith("/"))
                customServerTextBox.Text = customServerTextBox.Text.TrimEnd('/');
            FetchRemoteUpdates(true);
        }

        private void setPathButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    installPathTextBox.Text = fbd.SelectedPath + $"\\{Program.AppName}";
                }
            }
        }

        private void detectInstallPath_Click(object sender, EventArgs e)
        {
            if (!InstallDetector.IsInstalled())
                MessageBox.Show("Install not detected or app is not installed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else installPathTextBox.Text = InstallDetector.InstallPath;
        }

        private void setDefaultPathButton_Click(object sender, EventArgs e)
        {
            installPathTextBox.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "CharmsBarReloaded");
        }

        SaveFileDialog saveFileDialog = new SaveFileDialog();
        private void saveSettings_Click(object sender, EventArgs e)
        {
            UpdaterSettings updaterSettings = new UpdaterSettings
            {
                includeBeta = includeBetasCheckbox.Checked,
                includeLegacy = includeLegacyCheckbox.Checked,
                customInstallPath = installPathTextBox.Text,
                customUpdateServer = customServerTextBox.Text,
                useCustomUpdateServer = customServerRadio.Checked

            };

            saveFileDialog.Filter = "JSON document|*.json";
            saveFileDialog.Title = "Save config file";
            saveFileDialog.FileName = "updaterConfig.json";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!File.Exists(saveFileDialog.FileName))
                {
                    using (FileStream fs = File.Create(saveFileDialog.FileName)) { }
                }
                File.WriteAllText(saveFileDialog.FileName, JsonSerializer.Serialize(updaterSettings, new JsonSerializerOptions { WriteIndented = true }));
            }
        }

        OpenFileDialog openFileDialog = new OpenFileDialog();
        private void loadSettings_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "JSON document|*.json";
            openFileDialog.Title = "Load config file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadConfig(openFileDialog.FileName);
            }
        }
        #endregion advanced
        #endregion UI interaction

        private void versionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InstallDetector.IsInstalled() && filteredUpdates.Count > 0)
            {
                if (filteredUpdates[versionSelector.SelectedIndex].build > InstallDetector.BuildNumber)
                    installButton.Text = "Update";
                if (filteredUpdates[versionSelector.SelectedIndex].build == InstallDetector.BuildNumber)
                    installButton.Text = "Reinstall";
                if (filteredUpdates[versionSelector.SelectedIndex].build < InstallDetector.BuildNumber)
                    installButton.Text = "Install";
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteServer.CheckForUpdates(false, customServerRadio.Checked, customServerTextBox.Text);
        }

        private void checkForUpdatesincludeBetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteServer.CheckForUpdates(true, customServerRadio.Checked, customServerTextBox.Text);
        }

        private void installPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (installPathTextBox.Text.Contains("\\Users\\") || portableInstallCheckBox.Checked || Program.IsElevated)
                ShieldIcon.RemoveFromButton(installButton);
            else
                ShieldIcon.AddToButton(installButton);
        }

        private void portableInstallCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (installPathTextBox.Text.Contains("\\Users\\") || portableInstallCheckBox.Checked || Program.IsElevated)
                ShieldIcon.RemoveFromButton(installButton);
            else
                ShieldIcon.AddToButton(installButton);
        }

        private void installCustomUpdatePackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Pick an update file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                Install(installPathTextBox.Text, openFileDialog.FileName, portableInstallCheckBox.Checked);
        }
        private void ApplicationInstallSuccess(bool isPortable)
        {
            UpdateStatus("Installed!", "success");
            progressBar.Value = 100;
            installButton.Enabled = true;
            if (isPortable)
                return;

            InstallDetector.IsInstalled(true);
            this.Text = "Updater";
            installedVersionLabel.Text = $"Installed: {InstallDetector.VersionString}";
            installPathTextBox.Text = InstallDetector.InstallPath;
            portableInstallCheckBox.Checked = InstallDetector.IsPortable;
            checkForUpdatesToolStripMenuItem.Enabled = true;
            checkForUpdatesincludeBetasToolStripMenuItem.Enabled = true;
        }

        private void uninstallBtn_Click(object sender, EventArgs e)
        {
            Uninstall();
        }
    }
}
