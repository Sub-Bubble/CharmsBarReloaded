using System.ComponentModel;
using System.Text.Json;


namespace CharmsBarReloaded.Updater
{
    [DesignerCategory("Form")]
    public partial class UpdaterForm : Form
    {
        List<UpdateItem> updates;
        private bool showAdvancedSettings = false;

        private string autoAction = string.Empty;
        private int? buildAutoSelect = null;

        private Progress<int> percentage;
        private Progress<StatusMessage> message;
        public UpdaterForm(string action = "", bool includeBetas = false, bool includeLegacy = false,
            bool isPortable = false, int? build = null, string installPath = "", string customServerUrl = "")
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
                if (InstallDetector.AdminInstall && !Program.IsElevated)
                    ShieldIcon.AddToButton(uninstallBtn);
            }
            else if (!Program.IsElevated)
                ShieldIcon.AddToButton(installButton);

            AdvancedSettings.Hide();

            if (includeBetas)
                includeBetasCheckbox.Checked = true;
            if (includeLegacy)
                includeLegacyCheckbox.Checked = true;
            if (isPortable)
                portableInstallCheckBox.Checked = true;
            if (build.HasValue)
                buildAutoSelect = build.Value;
            if (!string.IsNullOrWhiteSpace(action))
                autoAction = action;

            if (!string.IsNullOrWhiteSpace(customServerUrl))
            {
                officialServerRadio.Checked = false;
                customServerRadio.Checked = true;
                customServerTextBox.Enabled = true;
                customServerTextBox.Text = customServerUrl;
                applyUpdateServerPathBtn.Enabled = true;
            }
            if (!string.IsNullOrWhiteSpace(installPath))
                installPathTextBox.Text = installPath;

            // custom made config is always prioritized over a checkbox
            if (File.Exists(Program.DefaultConfigPath))
                LoadConfig(Program.DefaultConfigPath);
            else
                FetchRemoteUpdates(customServerRadio.Checked);
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

        List<UpdateItem> filteredUpdates = new List<UpdateItem>();

        #region toolbar
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

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteServer.CheckForUpdates(false, customServerRadio.Checked, customServerTextBox.Text);
        }

        private void checkForUpdatesincludeBetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteServer.CheckForUpdates(true, customServerRadio.Checked, customServerTextBox.Text);
        }

        AboutWindow aboutWindow = new();
        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            aboutWindow.ShowDialog();
            aboutWindow.Focus();
        }

        private void installCustomUpdatePackageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Pick an update file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                percentage = new Progress<int>(progressPercentage =>
                {
                    progressBar.Invoke((Action)(() => progressBar.Value = progressPercentage));
                });
                message = new Progress<StatusMessage>(statusMessage =>
                {
                    this.Invoke((Action)(() => UpdateStatus(statusMessage.Message, statusMessage.Status)));
                });
                Installer.Install(installPathTextBox.Text, openFileDialog.FileName, portableInstallCheckBox.Checked, percentage, message);
                percentage = null;
                message = null;
                ApplicationInstallSuccess();
            }
        }
        #endregion toolbar
        #region main installer
        void UpdateStatus(string statusMessage, Status status, bool updatesAvailable = true, bool hasNewUpdate = false)
        {
            switch (status)
            {
                case Status.Info:
                    if (!updatesAvailable) latestVersionLabel.Text = $"Latest version: Fetching";
                    if (hasNewUpdate) latestVersionLabel.ForeColor = Color.Green;
                    else latestVersionLabel.ForeColor = Color.Black;
                    statusLabel.ForeColor = Color.Black;
                    break;
                case Status.Warning:
                    latestVersionLabel.ForeColor = Color.Orange;
                    statusLabel.ForeColor = Color.Orange;
                    break;
                case Status.Error:
                    if (!updatesAvailable)
                    {
                        latestVersionLabel.ForeColor = Color.Red;
                        latestVersionLabel.Text = $"Latest version: Unavailable";
                    }
                    statusLabel.ForeColor = Color.Red;
                    break;
                case Status.Success:
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

        private void uninstallBtn_Click(object sender, EventArgs e)
        {
            if ((InstallDetector.AdminInstall && Program.IsElevated) || !InstallDetector.AdminInstall)
                Installer.Uninstall();
            else
                Program.Elevate(["-uninstall"]);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            RemoteServer.cancelToken.Cancel();
            installButton.Enabled = true;
            cancelButton.Enabled = false;
            progressBar.Value = 0;
        }

        private async void installButton_Click(object sender, EventArgs e)
        {
            if (installPathTextBox.Text.Contains("\\Users\\") || portableInstallCheckBox.Checked || Program.IsElevated)
            {
                cancelButton.Enabled = true;
                installButton.Enabled = false;
                try
                {
                    percentage = new Progress<int>(progressPercentage =>
                    {
                        progressBar.Invoke((Action)(() => progressBar.Value = progressPercentage));
                    });
                    message = new Progress<StatusMessage>(statusMessage =>
                    {
                        UpdateStatus(statusMessage.Message, statusMessage.Status);
                    });
                    await Installer.DownloadAsync(installPathTextBox.Text, filteredUpdates[versionSelector.SelectedIndex].downloadLink, portableInstallCheckBox.Checked, percentage, message);
                    percentage = null;
                    message = null;

                }
                catch (OperationCanceledException ex)
                {
                    Directory.Delete(Program.TempFolder, true);
                    RemoteServer.cancelToken.Dispose();
                    RemoteServer.cancelToken = new CancellationTokenSource();
                    UpdateStatus("Ready", Status.Info);
                }
                cancelButton.Enabled = false;
                installButton.Enabled = true;
            }
            else
            {
                string[] args =
                    {
                        "-install",
                        $"-build {filteredUpdates[versionSelector.SelectedIndex].build}",
                        $"-installpath \"{installPathTextBox.Text}\""
                    };

                if (customServerRadio.Checked)
                    args.Append($"-customserver \"{customServerTextBox.Text}\"");
                if (portableInstallCheckBox.Checked)
                    args.Append("-portable");
                if (includeBetasCheckbox.Checked)
                    args.Append("-includebetas");
                if (includeLegacyCheckbox.Checked)
                    args.Append("-includelegacy");

                Program.Elevate(args);
            }
        }
        #endregion main installer
        #region advanced settings
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

        private void installPathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (installPathTextBox.Text.Contains("\\Users\\") || portableInstallCheckBox.Checked || Program.IsElevated)
                ShieldIcon.RemoveFromButton(installButton);
            else
                ShieldIcon.AddToButton(installButton);
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

        private void portableInstallCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (installPathTextBox.Text.Contains("\\Users\\") || portableInstallCheckBox.Checked || Program.IsElevated)
                ShieldIcon.RemoveFromButton(installButton);
            else
                ShieldIcon.AddToButton(installButton);
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
        #endregion advanced settings
        private void RepopulateUpdatesList()
        {
            versionSelector.Items.Clear();
            filteredUpdates.Clear();

            if (updates == null || updates.Count == 0)
            {
                UpdateStatus("Update list is empty!", Status.Error, false);
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
                UpdateStatus("Update available!", Status.Info, true, true);
            else
                UpdateStatus("Ready", Status.Info);
            installButton.Enabled = true;

            if (buildAutoSelect.HasValue)
                foreach (var version in filteredUpdates)
                    if (version.build == buildAutoSelect.Value)
                        versionSelector.SelectedIndex = filteredUpdates.IndexOf(version);

            if (!string.IsNullOrWhiteSpace(autoAction))
                installButton_Click(null, null);
        }

        private async void FetchRemoteUpdates(bool isCustomUrl)
        {
            installButton.Enabled = false;
            cancelButton.Enabled = false;
            UpdateStatus("Connecting to a remote server", Status.Info, false);
            string remoteUrl = @"http://localhost/updates.json"; //will be released later
            if (isCustomUrl)
                remoteUrl = customServerTextBox.Text;

            try
            {
                UpdateStatus("Connecting to a remote server", Status.Info, false);

                var updatesList = await RemoteServer.FetchUpdates(remoteUrl);

                if (string.IsNullOrWhiteSpace(updatesList))
                {
                    UpdateStatus("Failed to fetch updates!", Status.Error, false);
                    return;
                }

                updates = JsonSerializer.Deserialize<List<UpdateItem>>(updatesList, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                updates.Sort((a, b) => b.build.CompareTo(a.build));
                versionSelector.Items.Clear();
                RepopulateUpdatesList();
            }
            catch (UriFormatException)
            {
                UpdateStatus("Failed to connect to a server", Status.Error, false);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == null)
            {
                UpdateStatus("Failed to connect to a server", Status.Error, false);
            }
            catch (JsonException)
            {
                UpdateStatus("Failed to parse updates", Status.Error, false);
            }
            catch
            {
                UpdateStatus("Failed to fetch updates!", Status.Error, false);
            }
        }

        private void ApplicationInstallSuccess()
        {
            installButton.Enabled = true;
            InstallDetector.IsInstalled(true);
            uninstallBtn.Enabled = true;
            this.Text = "Updater";
            installedVersionLabel.Text = $"Installed: {InstallDetector.VersionString}";
            installPathTextBox.Text = InstallDetector.InstallPath;
            portableInstallCheckBox.Checked = InstallDetector.IsPortable;
            checkForUpdatesToolStripMenuItem.Enabled = true;
            checkForUpdatesincludeBetasToolStripMenuItem.Enabled = true;
        }
    }
}
