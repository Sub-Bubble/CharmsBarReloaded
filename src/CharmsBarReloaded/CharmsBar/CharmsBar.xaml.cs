using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.CharmsBar
{
    /// <summary>
    /// Interaction logic for CharmsBar.xaml
    /// </summary>
    public partial class CharmsBar : Window
    {
        public CharmsBar(ref CharmsClock.CharmsClock charmsClock)
        {
            var charmsClockRef = charmsClock;
            InitializeComponent();
            this.Loaded += CharmsBar_Loaded;
            this.MouseEnter += (s, e) => { CharmsBar_MouseEnter(ref charmsClockRef); };
            this.MouseLeave += (s, e) => { CharmsBar_MouseLeave(ref charmsClockRef); };
        }
        public void HideWindow()
        {
            BeginAnimation(UIElement.OpacityProperty, fadeOut);
            foreach (Grid grid in charmsStack.Children)
            {
                foreach (var item in grid.Children)
                {
                    if (item.GetType() == typeof(Image))
                    {
                        string source = ((Image)item).Source.ToString();
                        if (!source.Contains("Preview")) ((Image)item).Source = new BitmapImage(new Uri(source.Insert(source.LastIndexOf(".png"), "Preview")));
                    }
                    if (item.GetType() == typeof(Label))
                        ((Label)item).Visibility = Visibility.Collapsed;
                    if (item.GetType() == typeof(Grid))
                        ((Grid)item).Background = GetBrush.GetSpecialBrush("White");
                }
            }
        }
        public bool windowVisible;
        public bool isAnimating = false;
        public int windowWidth;
        public void Window_Reload()
        {
            Log.Info("Reloading Charms Bar...");
            InitializeAnimations();
            SetupButtons();
            
            this.Background = GetBrush.GetBrushFromHex(App.charmsConfig.charmsBarConfig.BackgroundColor);
            PrepareButtons(App.charmsConfig.EnableAnimations);
            HideWindow();
        }
        private void FadeOut_Completed(object? sender, EventArgs e)
        {
            PrepareButtons(false);
            isAnimating = false;
            windowVisible = false;
            this.Background = GetBrush.GetSpecialBrush("Hide");
            charmsStack.Visibility = Visibility.Collapsed;
            BeginAnimation(OpacityProperty, backTo1Opacity);
        }
        private void CharmsBar_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = SystemConfig.GetDesktopWorkingArea.Top + 1;
            this.Window_Reload();
            this.Left = SystemConfig.GetDesktopWorkingArea.Right - this.Width;
            this.charmsStack.Width = this.windowWidth;
            this.Width = this.windowWidth;
            this.MinHeight = SystemParameters.PrimaryScreenHeight - 1;
            this.Height = SystemParameters.PrimaryScreenHeight;
        }
        private void CharmsBar_MouseLeave(ref CharmsClock.CharmsClock charmsClock)
        {
            HideWindow();
            if (App.charmsConfig.EnableAnimations)
                charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeOut);
            else
                charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationOut);
            this.Top = 1;
        }
        private void CharmsBar_MouseEnter(ref CharmsClock.CharmsClock charmsClock)
        {
            if (!isAnimating)
            {
                isAnimating = true;
                BeginStoryboard(fadeIn);
                if (App.charmsConfig.EnableAnimations)
                    charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeIn);
                else
                    charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationIn);
            }
            foreach (Grid grid in charmsStack.Children)
            {
                foreach (var item in grid.Children)
                {
                    if (item.GetType() == typeof(Image))
                    {
                        string source = ((Image)item).Source.ToString();
                        ((Image)item).Source = new BitmapImage(new Uri(source.Replace("Preview", "")));
                    }
                    if (item.GetType() == typeof(Label))
                        ((Label)item).Visibility = Visibility.Visible;
                    if (item.GetType() == typeof(Grid))
                        ((Grid)item).Background = SystemConfig.GetAccentColor;
                }
            }

            this.Top = 0;
        }

        public void CharmsBar_Update(ref CharmsClock.CharmsClock charmsClock)
        {
            Point cursorPos = SystemConfig.GetMouseLocation;

            if (cursorPos.X + 1 == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width && cursorPos.Y == SystemConfig.GetDesktopWorkingArea.Top && App.charmsConfig.charmsBarConfig.IsEnabled && !windowVisible)
            {
                this.windowVisible = true;
                if (!isAnimating)
                {
                    this.Background = GetBrush.GetSpecialBrush("Transparent");
                    this.charmsStack.Visibility = Visibility.Visible;
                    SlideInButtons();
                }
            }
            if (Keyboard.IsKeyDown(Key.LWin) && Keyboard.IsKeyDown(Key.C) && App.charmsConfig.charmsBarConfig.EnableKeyboardShortcut && (App.charmsConfig.charmsBarConfig.IsEnabled || App.charmsConfig.charmsBarConfig.KeyboardShortcutOverridesOffSetting))
            {
                if (!isAnimating && !windowVisible)
                {
                    this.Background = GetBrush.GetBrushFromHex(App.charmsConfig.charmsBarConfig.BackgroundColor);
                    charmsStack.Visibility = Visibility.Visible;
                    windowVisible = true;
                    isAnimating = true;
                    SlideInButtons();
                    BeginStoryboard(fadeIn);
                    if (App.charmsConfig.EnableAnimations)
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeIn);
                    else
                        charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationIn);
                    foreach (Grid grid in charmsStack.Children)
                    {
                        foreach (var item in grid.Children)
                        {
                            if (item.GetType() == typeof(Image))
                            {
                                string source = ((Image)item).Source.ToString();
                                ((Image)item).Source = new BitmapImage(new Uri(source.Replace("Preview", "")));
                            }
                            if (item.GetType() == typeof(Label))
                                ((Label)item).Visibility = Visibility.Visible;
                            if (item.GetType() == typeof(Grid))
                                ((Grid)item).Background = SystemConfig.GetAccentColor;
                        }
                    }
                }
                this.Height = SystemParameters.PrimaryScreenHeight;
                this.Top = 0;
            }
            if (Keyboard.IsKeyDown(Key.Escape) && App.charmsConfig.charmsBarConfig.EnableKeyboardShortcut)
            {
                this.HideWindow();
                if (App.charmsConfig.EnableAnimations && charmsClock.Opacity != 0)
                    charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.fadeOut);
                else
                    charmsClock.BeginAnimation(UIElement.OpacityProperty, charmsClock.noAnimationOut);
            }
        }
    }
}
