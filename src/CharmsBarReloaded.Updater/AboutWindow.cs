using System.Diagnostics;
using System.Reflection;

namespace CharmsBarReloaded.Updater
{
    public partial class AboutWindow : Form
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contributorsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/Sub-Bubble/CharmsBarReloaded/graphs/contributors") { UseShellExecute = true });
        }

        private void mitLicenseLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/Sub-Bubble/CharmsBarReloaded/blob/master/src/CharmsBarReloaded.Updater/LICENSE.txt") { UseShellExecute = true });
        }

        private void AboutWindow_Load(object sender, EventArgs e)
        {
            updaterNameLabel.Text = $"{Program.AppDisplayName} updater";
            versionString.Text = $"Version: {assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";
            buildString.Text = $"Build: {assembly.GetName().Version.Major}";
        }
    }
}
