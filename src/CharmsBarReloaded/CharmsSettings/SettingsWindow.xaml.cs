using System.Windows;
using System.Windows.Media.Animation;
using CharmsBarReloaded.Config;

namespace CharmsBarReloaded.CharmsSettings
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public Storyboard settingsSlideIn;
        public Storyboard settingsSlideOut;
        public bool isBusy = false;

        public SettingsWindow()
        {
            InitializeComponent();
            this.Loaded += SettingsWindow_Loaded;
            this.Activated += SettingsWindow_Activated;
            this.Deactivated += SettingsWindow_Deactivated;
            
            settingsSlideIn = (Storyboard)FindResource("SlideIn");
            settingsSlideOut = (Storyboard)FindResource("SlideOut");
        }
        void SettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemConfig.GetDesktopWorkingArea.Width - 360;
            this.Height = SystemConfig.GetDesktopWorkingArea.Height;
            this.Top = 0;
            this.frame.Background = SystemConfig.GetAccentColor;
            if (App.charmsConfig.EnableAnimations)
                BeginStoryboard(settingsSlideIn);
        }
        void SettingsWindow_Activated(object? sender, EventArgs e)
        {
            if (App.charmsConfig.EnableAnimations && !isBusy)
                BeginStoryboard(settingsSlideIn);
            isBusy = false;
        }
        void SettingsWindow_Deactivated(object? sender, EventArgs e)
        {
            if (isBusy)
                return;

            App.charmsConfig.Save();
            App.ClickHandler("Reload");
        }
    }
}
