using CharmsBarReloaded.Config;
using System.Windows;

namespace CharmsBarReloaded.CharmsBar
{
    /// <summary>
    /// Interaction logic for CharmsBar.xaml
    /// </summary>
    public partial class CharmsBar : Window
    {
        public CharmsBar()
        {
            InitializeComponent();
        }
        public void HideWindow()
        {
            BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }
        public bool windowVisible;
        public bool isAnimating = false;
        public void Window_Reload(CharmsConfig appConfig)
        {
            InitializeAnimations(appConfig.EnableAnimations, appConfig.charmsBarConfig.BackgroundColor);
            SetupButtons(appConfig.charmsBarConfig.ButtonsAmount + 2, appConfig.charmsBarConfig.ButtonActions, appConfig.charmsBarConfig.HideWindowAfterClick, appConfig.charmsBarConfig.HoverColor);

            Log.Info("Loaded Charms Bar successfully!");
            this.Height = SystemParameters.PrimaryScreenHeight - 1;
            this.Background = GetBrush.GetBrushFromHex(appConfig.charmsBarConfig.BackgroundColor);
            PrepareButtons(appConfig.EnableAnimations);
        }
        private void FadeOut_Completed(object? sender, EventArgs e)
        {
            PrepareButtons(true, false);
            isAnimating = false;
            windowVisible = false;
            this.Background = GetBrush.GetSpecialBrush("Hide");
            charmsStack.Visibility = Visibility.Collapsed;
            BeginAnimation(OpacityProperty, backTo1Opacity);
        }
    }
}
