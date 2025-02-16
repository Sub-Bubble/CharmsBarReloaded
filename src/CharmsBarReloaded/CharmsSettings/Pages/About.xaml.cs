using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.CharmsSettings.Pages
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
            ReloadStrings();
        }
        #region back button
        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButtonClicked.png", UriKind.Relative));
        }
        private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
            App.ClickHandler("SettingsHome");
        }
        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButtonMouseOver.png", UriKind.Relative));
        }
        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
        }
        #endregion back button
        public void ReloadStrings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            versionString.Text = $"{App.translationManager.GetTranslation("CharmsSettings.About.Version")}: {assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";
            buildString.Text = $"{App.translationManager.GetTranslation("CharmsSettings.About.Build")}: {assembly.GetName().Version.Major}";
            branchString.Text = $"{App.translationManager.GetTranslation("CharmsSettings.About.Branch")}: {(App.charmsConfig.isBetaBranch ? App.translationManager.GetTranslation("CharmsSettings.About.Branch.Beta") : App.translationManager.GetTranslation("CharmsSettings.About.Branch.Stable"))}";
            aboutText.Text = App.translationManager.GetTranslation("CharmsSettings.Home.About");
            UpdateButton.Content = App.translationManager.GetTranslation("CharmsSettings.About.CheckForUpdates");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            App.ClickHandler("CheckForUpdates");
        }
    }
}
