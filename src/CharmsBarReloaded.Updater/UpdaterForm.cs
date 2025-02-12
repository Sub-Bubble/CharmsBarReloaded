using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CharmsBarReloaded.Updater
{
    public partial class UpdaterForm : Form
    {
        public struct UpdateItem
        {
            public string versionName { get; set; }
            public int build { get; set; }
            public string downloadLink { get; set; }
            public bool isBeta { get; set; }
            public bool isLegacy { get; set; }
        }
        private struct UpdaterSettings
        {
            public bool includeBeta { get; set; }
            public bool includeLegacy { get; set; }
            public bool useCustomUpdateServer { get; set; }
            public string customUpdateServer { get; set; }
            public string customInstallPath { get; set; }
        }
        List<UpdateItem> updates;
        private bool showAdvancedSettings = false;
        string defaultConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CharmsBarReloaded", "updaterConfig.json");
        public UpdaterForm()
        {
            InitializeComponent();
            AdvancedSettings.Hide();

            if (File.Exists(defaultConfigPath))
                LoadConfig(defaultConfigPath);
            else
            {
                officialServerRadio.Checked = false;

                customServerRadio.Checked = true;
                customServerTextBox.Enabled = true;
                customServerTextBox.Text = "http://localhost"; // temporary solution
                applyUpdateServerPathBtn.Enabled = true;

                FetchRemoteUpdates(true);
            }
        }

        void LoadConfig(string configPath)
        {
            var settings = JsonSerializer.Deserialize<UpdaterSettings>(File.ReadAllText(configPath));
            includeBetasCheckbox.Checked = settings.includeBeta;
            includeLegacyCheckbox.Checked = settings.includeLegacy;
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

        #region fetch remote server
        private async void FetchRemoteUpdates(bool isCustomUrl)
        {
            UpdateStatus("Connecting to a remote server", "info", false);
            string remoteUrl = @""; //will be released later
            if (isCustomUrl)
                remoteUrl = customServerTextBox.Text;
            if (!isCustomUrl)
            {
                MessageBox.Show("Update list server is not up yet! Stay tuned for later updates.");
                return;
            }

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(customServerTextBox.Text);
                    client.DefaultRequestHeaders.Add("CharmsBarReloaded-Updater", "v1.0");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string updatesList = await FindRemoteJson(client, customServerTextBox.Text);

                    if (string.IsNullOrWhiteSpace(updatesList))
                    {
                        UpdateStatus("Failed to fetch updates!", "error", false);
                        return;
                    }

                    updates = JsonSerializer.Deserialize<List<UpdateItem>>(updatesList, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    versionSelector.Items.Clear();
                    RepopulateUpdatesList();
                    UpdateStatus("Ready", "info");
                }
                catch (UriFormatException)
                {
                    UpdateStatus("Failed to connect to a server", "error", false);
                }
                catch (HttpRequestException ex) when (ex.StatusCode == null)
                {
                    UpdateStatus("Failed to connect to a server", "error", false);
                }
                catch
                {
                    UpdateStatus("Failed to fetch updates!", "error", false);
                }
            }
        }
        private async Task<string> FindRemoteJson(HttpClient client, string url)
        {
            if (url.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                return await GetRemoteJson(client, url);
            else
            {
                try
                {
                    string json = await GetRemoteJson(client, url);
                    if (!string.IsNullOrWhiteSpace(json))
                        return json;
                } catch { }
                return await GetRemoteJson(client, $"{url}/updates.json");
            }
        }
        private async Task<string> GetRemoteJson(HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        #endregion fetch remote server

        #region UI modification
        private void RepopulateUpdatesList()
        {
            versionSelector.Items.Clear();
            if (updates == null || updates.Count == 0)
            {
                UpdateStatus("Error parsing update list", "error", false);
                return;
            }
            versionSelector.Enabled = true;
            latestVersionLabel.ForeColor = Color.Black;
            statusLabel.ForeColor = Color.Black;
            for (int i = 0; i < updates.Count; i++)
            {
                if (updates[i].isBeta && updates[i].isLegacy && includeBetasCheckbox.Checked && includeLegacyCheckbox.Checked)
                    versionSelector.Items.Add(updates[i].versionName);
                if (updates[i].isBeta && !updates[i].isLegacy && includeBetasCheckbox.Checked)
                    versionSelector.Items.Add(updates[i].versionName);
                else if (updates[i].isLegacy && !updates[i].isBeta && includeLegacyCheckbox.Checked)
                    versionSelector.Items.Add(updates[i].versionName);
                else if (!updates[i].isLegacy && !updates[i].isBeta)
                    versionSelector.Items.Add(updates[i].versionName);
            }
            versionSelector.SelectedIndex = 0;
            latestVersionLabel.Text = $"Latest: {versionSelector.Items[0]}";
            statusLabel.Text = "Ready";
        }
        void UpdateStatus(string statusMessage, string type = "info", bool updatesAvailable = true)
        {
            switch (type)
            {
                case "info":
                    if (!updatesAvailable) latestVersionLabel.Text = $"Latest version: Fetching";
                    latestVersionLabel.ForeColor = Color.Black;
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
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
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
                    installPathTextBox.Text = fbd.SelectedPath;
                }
            }
        }

        private void detectInstallPath_Click(object sender, EventArgs e)
        {
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
    }
}
