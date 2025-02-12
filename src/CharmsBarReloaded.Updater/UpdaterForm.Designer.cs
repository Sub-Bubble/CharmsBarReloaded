namespace CharmsBarReloaded.Updater
{
    partial class UpdaterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterForm));
            progressBar = new ProgressBar();
            statusLabel = new Label();
            installButton = new Button();
            cancelButton = new Button();
            advancedSettingsButton = new Button();
            versionSelector = new ComboBox();
            installedVersionLabel = new Label();
            latestVersionLabel = new Label();
            AdvancedSettings = new GroupBox();
            loadSettings = new Button();
            applyUpdateServerPathBtn = new Button();
            detectInstallPath = new Button();
            setDefaultPathButton = new Button();
            setPathButton = new Button();
            installPathTextBox = new TextBox();
            installPathText = new Label();
            versionSelectorOptionsText = new Label();
            saveSettings = new Button();
            customServerTextBox = new TextBox();
            customServerRadio = new RadioButton();
            officialServerRadio = new RadioButton();
            updateServerText = new Label();
            includeLegacyCheckbox = new CheckBox();
            includeBetasCheckbox = new CheckBox();
            updaterMenu = new MenuStrip();
            windowToolStripMenuItem = new ToolStripMenuItem();
            checkForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            chechForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            installCustomUpdatePackageToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem1 = new ToolStripMenuItem();
            AdvancedSettings.SuspendLayout();
            updaterMenu.SuspendLayout();
            SuspendLayout();
            // 
            // progressBar
            // 
            progressBar.Location = new Point(10, 49);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(483, 23);
            progressBar.TabIndex = 0;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusLabel.Location = new Point(10, 16);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(53, 21);
            statusLabel.TabIndex = 1;
            statusLabel.Text = "Ready";
            // 
            // installButton
            // 
            installButton.Enabled = false;
            installButton.Location = new Point(418, 78);
            installButton.Name = "installButton";
            installButton.Size = new Size(75, 23);
            installButton.TabIndex = 2;
            installButton.Text = "Install";
            installButton.UseVisualStyleBackColor = true;
            installButton.Click += installButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Enabled = false;
            cancelButton.Location = new Point(337, 78);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // advancedSettingsButton
            // 
            advancedSettingsButton.Location = new Point(10, 78);
            advancedSettingsButton.Margin = new Padding(3, 3, 3, 10);
            advancedSettingsButton.Name = "advancedSettingsButton";
            advancedSettingsButton.Size = new Size(87, 23);
            advancedSettingsButton.TabIndex = 4;
            advancedSettingsButton.Text = "▼ Advanced";
            advancedSettingsButton.UseVisualStyleBackColor = true;
            advancedSettingsButton.Click += advancedSettingsButton_Click;
            // 
            // versionSelector
            // 
            versionSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            versionSelector.DropDownWidth = 121;
            versionSelector.FormattingEnabled = true;
            versionSelector.Location = new Point(103, 78);
            versionSelector.Name = "versionSelector";
            versionSelector.Size = new Size(121, 23);
            versionSelector.TabIndex = 5;
            // 
            // installedVersionLabel
            // 
            installedVersionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            installedVersionLabel.Location = new Point(266, 7);
            installedVersionLabel.Name = "installedVersionLabel";
            installedVersionLabel.Size = new Size(227, 15);
            installedVersionLabel.TabIndex = 6;
            installedVersionLabel.Text = "Installed: Unknown";
            installedVersionLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // latestVersionLabel
            // 
            latestVersionLabel.Location = new Point(266, 22);
            latestVersionLabel.Name = "latestVersionLabel";
            latestVersionLabel.Size = new Size(227, 15);
            latestVersionLabel.TabIndex = 7;
            latestVersionLabel.Text = "Latest: Unknown";
            latestVersionLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // AdvancedSettings
            // 
            AdvancedSettings.Controls.Add(loadSettings);
            AdvancedSettings.Controls.Add(applyUpdateServerPathBtn);
            AdvancedSettings.Controls.Add(detectInstallPath);
            AdvancedSettings.Controls.Add(setDefaultPathButton);
            AdvancedSettings.Controls.Add(setPathButton);
            AdvancedSettings.Controls.Add(installPathTextBox);
            AdvancedSettings.Controls.Add(installPathText);
            AdvancedSettings.Controls.Add(versionSelectorOptionsText);
            AdvancedSettings.Controls.Add(saveSettings);
            AdvancedSettings.Controls.Add(customServerTextBox);
            AdvancedSettings.Controls.Add(customServerRadio);
            AdvancedSettings.Controls.Add(officialServerRadio);
            AdvancedSettings.Controls.Add(updateServerText);
            AdvancedSettings.Controls.Add(includeLegacyCheckbox);
            AdvancedSettings.Controls.Add(includeBetasCheckbox);
            AdvancedSettings.Location = new Point(15, 118);
            AdvancedSettings.Margin = new Padding(3, 3, 3, 10);
            AdvancedSettings.Name = "AdvancedSettings";
            AdvancedSettings.Size = new Size(481, 201);
            AdvancedSettings.TabIndex = 8;
            AdvancedSettings.TabStop = false;
            AdvancedSettings.Text = "Advanced";
            // 
            // loadSettings
            // 
            loadSettings.Location = new Point(322, 172);
            loadSettings.Name = "loadSettings";
            loadSettings.Size = new Size(75, 23);
            loadSettings.TabIndex = 14;
            loadSettings.Text = "Load";
            loadSettings.UseVisualStyleBackColor = true;
            loadSettings.Click += loadSettings_Click;
            // 
            // applyUpdateServerPathBtn
            // 
            applyUpdateServerPathBtn.Location = new Point(183, 142);
            applyUpdateServerPathBtn.Name = "applyUpdateServerPathBtn";
            applyUpdateServerPathBtn.Size = new Size(47, 23);
            applyUpdateServerPathBtn.TabIndex = 13;
            applyUpdateServerPathBtn.Text = "Apply";
            applyUpdateServerPathBtn.UseVisualStyleBackColor = true;
            applyUpdateServerPathBtn.Click += applyUpdateServerPathBtn_Click;
            // 
            // detectInstallPath
            // 
            detectInstallPath.Enabled = false;
            detectInstallPath.Location = new Point(332, 66);
            detectInstallPath.Name = "detectInstallPath";
            detectInstallPath.Size = new Size(75, 23);
            detectInstallPath.TabIndex = 12;
            detectInstallPath.Text = "Detect";
            detectInstallPath.UseVisualStyleBackColor = true;
            detectInstallPath.Click += detectInstallPath_Click;
            // 
            // setDefaultPathButton
            // 
            setDefaultPathButton.Location = new Point(413, 66);
            setDefaultPathButton.Name = "setDefaultPathButton";
            setDefaultPathButton.Size = new Size(62, 23);
            setDefaultPathButton.TabIndex = 11;
            setDefaultPathButton.Text = "Default";
            setDefaultPathButton.UseVisualStyleBackColor = true;
            setDefaultPathButton.Click += setDefaultPathButton_Click;
            // 
            // setPathButton
            // 
            setPathButton.Location = new Point(251, 66);
            setPathButton.Name = "setPathButton";
            setPathButton.Size = new Size(75, 23);
            setPathButton.TabIndex = 10;
            setPathButton.Text = "Browse...";
            setPathButton.UseVisualStyleBackColor = true;
            setPathButton.Click += setPathButton_Click;
            // 
            // installPathTextBox
            // 
            installPathTextBox.Location = new Point(251, 37);
            installPathTextBox.Name = "installPathTextBox";
            installPathTextBox.Size = new Size(224, 23);
            installPathTextBox.TabIndex = 9;
            installPathTextBox.Text = "C:\\Program Files\\CharmsBarReloaded";
            // 
            // installPathText
            // 
            installPathText.AutoSize = true;
            installPathText.Location = new Point(251, 19);
            installPathText.Name = "installPathText";
            installPathText.Size = new Size(68, 15);
            installPathText.TabIndex = 8;
            installPathText.Text = "Install path:";
            // 
            // versionSelectorOptionsText
            // 
            versionSelectorOptionsText.AutoSize = true;
            versionSelectorOptionsText.Location = new Point(6, 19);
            versionSelectorOptionsText.Name = "versionSelectorOptionsText";
            versionSelectorOptionsText.Size = new Size(92, 15);
            versionSelectorOptionsText.TabIndex = 7;
            versionSelectorOptionsText.Text = "Version selector:";
            // 
            // saveSettings
            // 
            saveSettings.Location = new Point(400, 172);
            saveSettings.Name = "saveSettings";
            saveSettings.Size = new Size(75, 23);
            saveSettings.TabIndex = 6;
            saveSettings.Text = "Save";
            saveSettings.UseVisualStyleBackColor = true;
            saveSettings.Click += saveSettings_Click;
            // 
            // customServerTextBox
            // 
            customServerTextBox.Location = new Point(15, 142);
            customServerTextBox.Name = "customServerTextBox";
            customServerTextBox.Size = new Size(162, 23);
            customServerTextBox.TabIndex = 5;
            // 
            // customServerRadio
            // 
            customServerRadio.AutoSize = true;
            customServerRadio.Location = new Point(15, 119);
            customServerRadio.Name = "customServerRadio";
            customServerRadio.Size = new Size(67, 19);
            customServerRadio.TabIndex = 4;
            customServerRadio.TabStop = true;
            customServerRadio.Text = "Custom";
            customServerRadio.UseVisualStyleBackColor = true;
            customServerRadio.CheckedChanged += customServerRadio_CheckedChanged;
            // 
            // officialServerRadio
            // 
            officialServerRadio.AutoSize = true;
            officialServerRadio.Enabled = false;
            officialServerRadio.Location = new Point(15, 97);
            officialServerRadio.Name = "officialServerRadio";
            officialServerRadio.Size = new Size(63, 19);
            officialServerRadio.TabIndex = 3;
            officialServerRadio.TabStop = true;
            officialServerRadio.Text = "Official";
            officialServerRadio.UseVisualStyleBackColor = true;
            // 
            // updateServerText
            // 
            updateServerText.AutoSize = true;
            updateServerText.Location = new Point(6, 79);
            updateServerText.Name = "updateServerText";
            updateServerText.Size = new Size(82, 15);
            updateServerText.TabIndex = 2;
            updateServerText.Text = "Update server:";
            // 
            // includeLegacyCheckbox
            // 
            includeLegacyCheckbox.AutoSize = true;
            includeLegacyCheckbox.Location = new Point(15, 57);
            includeLegacyCheckbox.Name = "includeLegacyCheckbox";
            includeLegacyCheckbox.Size = new Size(148, 19);
            includeLegacyCheckbox.TabIndex = 1;
            includeLegacyCheckbox.Text = "Include legacy versions";
            includeLegacyCheckbox.UseVisualStyleBackColor = true;
            includeLegacyCheckbox.CheckedChanged += includeLegacyCheckbox_CheckedChanged;
            // 
            // includeBetasCheckbox
            // 
            includeBetasCheckbox.AutoSize = true;
            includeBetasCheckbox.Location = new Point(15, 37);
            includeBetasCheckbox.Name = "includeBetasCheckbox";
            includeBetasCheckbox.Size = new Size(137, 19);
            includeBetasCheckbox.TabIndex = 0;
            includeBetasCheckbox.Text = "Include beta versions";
            includeBetasCheckbox.UseVisualStyleBackColor = true;
            includeBetasCheckbox.CheckedChanged += includeBetasCheckbox_CheckedChanged;
            // 
            // updaterMenu
            // 
            updaterMenu.AllowMerge = false;
            updaterMenu.BackColor = SystemColors.Control;
            updaterMenu.Items.AddRange(new ToolStripItem[] { windowToolStripMenuItem, aboutToolStripMenuItem });
            updaterMenu.Location = new Point(0, 0);
            updaterMenu.Name = "updaterMenu";
            updaterMenu.Size = new Size(508, 24);
            updaterMenu.TabIndex = 9;
            updaterMenu.Text = "updaterMenu";
            updaterMenu.Visible = false;
            updaterMenu.VisibleChanged += updaterMenu_VisibleChanged;
            // 
            // windowToolStripMenuItem
            // 
            windowToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { checkForUpdatesToolStripMenuItem, chechForUpdatesToolStripMenuItem, installCustomUpdatePackageToolStripMenuItem });
            windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            windowToolStripMenuItem.Size = new Size(63, 20);
            windowToolStripMenuItem.Text = "Window";
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            checkForUpdatesToolStripMenuItem.Enabled = false;
            checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            checkForUpdatesToolStripMenuItem.Size = new Size(252, 22);
            checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            // 
            // chechForUpdatesToolStripMenuItem
            // 
            chechForUpdatesToolStripMenuItem.Enabled = false;
            chechForUpdatesToolStripMenuItem.Name = "chechForUpdatesToolStripMenuItem";
            chechForUpdatesToolStripMenuItem.Size = new Size(252, 22);
            chechForUpdatesToolStripMenuItem.Text = "Chech for updates (include betas)";
            // 
            // installCustomUpdatePackageToolStripMenuItem
            // 
            installCustomUpdatePackageToolStripMenuItem.Enabled = false;
            installCustomUpdatePackageToolStripMenuItem.Name = "installCustomUpdatePackageToolStripMenuItem";
            installCustomUpdatePackageToolStripMenuItem.Size = new Size(252, 22);
            installCustomUpdatePackageToolStripMenuItem.Text = "Install custom update package";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem1 });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(52, 20);
            aboutToolStripMenuItem.Text = "About";
            // 
            // aboutToolStripMenuItem1
            // 
            aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            aboutToolStripMenuItem1.Size = new Size(107, 22);
            aboutToolStripMenuItem1.Text = "About";
            aboutToolStripMenuItem1.Click += aboutToolStripMenuItem1_Click;
            // 
            // UpdaterForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(508, 327);
            Controls.Add(AdvancedSettings);
            Controls.Add(latestVersionLabel);
            Controls.Add(installedVersionLabel);
            Controls.Add(versionSelector);
            Controls.Add(advancedSettingsButton);
            Controls.Add(cancelButton);
            Controls.Add(installButton);
            Controls.Add(statusLabel);
            Controls.Add(progressBar);
            Controls.Add(updaterMenu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = updaterMenu;
            MaximizeBox = false;
            Name = "UpdaterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Updater";
            AdvancedSettings.ResumeLayout(false);
            AdvancedSettings.PerformLayout();
            updaterMenu.ResumeLayout(false);
            updaterMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar;
        private Label statusLabel;
        private Button installButton;
        private Button cancelButton;
        private Button advancedSettingsButton;
        private ComboBox versionSelector;
        private Label installedVersionLabel;
        private Label latestVersionLabel;
        private GroupBox AdvancedSettings;
        private Label updateServerText;
        private CheckBox includeLegacyCheckbox;
        private CheckBox includeBetasCheckbox;
        private TextBox customServerTextBox;
        private RadioButton customServerRadio;
        private RadioButton officialServerRadio;
        private Button saveSettings;
        private Label versionSelectorOptionsText;
        private TextBox installPathTextBox;
        private Label installPathText;
        private MenuStrip updaterMenu;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private ToolStripMenuItem installCustomUpdatePackageToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem1;
        private Button setPathButton;
        private Button detectInstallPath;
        private Button setDefaultPathButton;
        private Button applyUpdateServerPathBtn;
        private Button loadSettings;
        private ToolStripMenuItem chechForUpdatesToolStripMenuItem;
    }
}