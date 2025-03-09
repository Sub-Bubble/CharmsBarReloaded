namespace CharmsBarReloaded.Updater
{
    partial class AboutWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutWindow));
            updaterNameLabel = new Label();
            contributorsLinkLabel = new LinkLabel();
            okBtn = new Button();
            versionString = new Label();
            buildString = new Label();
            label4 = new Label();
            label5 = new Label();
            lgplv3LicenseLinkLabel = new LinkLabel();
            SuspendLayout();
            // 
            // updaterNameLabel
            // 
            updaterNameLabel.AutoSize = true;
            updaterNameLabel.Font = new Font("Segoe UI", 15F);
            updaterNameLabel.Location = new Point(12, 9);
            updaterNameLabel.Name = "updaterNameLabel";
            updaterNameLabel.Size = new Size(175, 28);
            updaterNameLabel.TabIndex = 1;
            updaterNameLabel.Text = "AppName updater";
            // 
            // contributorsLinkLabel
            // 
            contributorsLinkLabel.AutoSize = true;
            contributorsLinkLabel.BackColor = Color.Transparent;
            contributorsLinkLabel.Location = new Point(217, 96);
            contributorsLinkLabel.Name = "contributorsLinkLabel";
            contributorsLinkLabel.Size = new Size(72, 15);
            contributorsLinkLabel.TabIndex = 5;
            contributorsLinkLabel.TabStop = true;
            contributorsLinkLabel.Text = "contributors";
            contributorsLinkLabel.LinkClicked += contributorsLinkLabel_LinkClicked;
            // 
            // okBtn
            // 
            okBtn.Location = new Point(309, 135);
            okBtn.Name = "okBtn";
            okBtn.Size = new Size(75, 23);
            okBtn.TabIndex = 0;
            okBtn.Text = "OK";
            okBtn.UseVisualStyleBackColor = true;
            okBtn.Click += okBtn_Click;
            // 
            // versionString
            // 
            versionString.AutoSize = true;
            versionString.Location = new Point(18, 43);
            versionString.Name = "versionString";
            versionString.Size = new Size(51, 15);
            versionString.TabIndex = 2;
            versionString.Text = "Version: ";
            // 
            // buildString
            // 
            buildString.AutoSize = true;
            buildString.Location = new Point(18, 57);
            buildString.Name = "buildString";
            buildString.Size = new Size(37, 15);
            buildString.TabIndex = 3;
            buildString.Text = "Build:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Location = new Point(18, 96);
            label4.Name = "label4";
            label4.Size = new Size(200, 15);
            label4.TabIndex = 4;
            label4.Text = "Application made by SubBubble and";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Location = new Point(18, 111);
            label5.Name = "label5";
            label5.Size = new Size(107, 15);
            label5.TabIndex = 6;
            label5.Text = "Licensed under the";
            // 
            // lgplv3LicenseLinkLabel
            // 
            lgplv3LicenseLinkLabel.AutoSize = true;
            lgplv3LicenseLinkLabel.BackColor = Color.Transparent;
            lgplv3LicenseLinkLabel.Location = new Point(121, 111);
            lgplv3LicenseLinkLabel.Name = "lgplv3LicenseLinkLabel";
            lgplv3LicenseLinkLabel.Size = new Size(84, 15);
            lgplv3LicenseLinkLabel.TabIndex = 7;
            lgplv3LicenseLinkLabel.TabStop = true;
            lgplv3LicenseLinkLabel.Text = "LGPLv3 license";
            lgplv3LicenseLinkLabel.LinkClicked += mitLicenseLinkLabel_LinkClicked;
            // 
            // AboutWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(396, 170);
            Controls.Add(contributorsLinkLabel);
            Controls.Add(lgplv3LicenseLinkLabel);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(buildString);
            Controls.Add(versionString);
            Controls.Add(okBtn);
            Controls.Add(updaterNameLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutWindow";
            StartPosition = FormStartPosition.CenterParent;
            Text = "About";
            Load += AboutWindow_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label updaterNameLabel;
        private LinkLabel contributorsLinkLabel;
        private Button okBtn;
        private Label versionString;
        private Label buildString;
        private Label label4;
        private Label label5;
        private LinkLabel lgplv3LicenseLinkLabel;
    }
}