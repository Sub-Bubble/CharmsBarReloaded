using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for CharmsBar.xaml
    /// </summary>
    public partial class CharmsBar : Window
    {
        #region hiding window from alttab
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;
        #endregion hiding window from alttab

        #region vars
        CharmsClock charmsClock;
        System.Timers.Timer timer = new System.Timers.Timer();
        bool isAnimating = false;
        bool windowVisible = false;
        #endregion vars
        #region animations
        public static ColorAnimation fadeIn;
        DoubleAnimation fadeOut = new DoubleAnimation
        {
            From = 1.0,
            To = 0.0,
            Duration = TimeSpan.FromMilliseconds(100)
        };
        DoubleAnimation backTo1Opacity = new DoubleAnimation { From = 0.0, To = 1.0, Duration = TimeSpan.FromMilliseconds(1)};
        Storyboard slideInButtons;
        Storyboard prepareButtons;
        Storyboard noAnimations;
        #endregion animations

        public CharmsBar()
        {
            /// initialing config and setting window location
            ButtonConfig.SetVars();
            GlobalConfig.LoadConfig();
            charmsClock = new CharmsClock();
            fadeIn = new ColorAnimation
            {
                To = (Color)ColorConverter.ConvertFromString($"#FF{GlobalConfig.BackgroundColor.ToUpper()}"),
                Duration = TimeSpan.FromMilliseconds(100),
            };
            InitializeComponent(); //init window
            
            UpdateHover();

            slideInButtons = (Storyboard)FindResource("SlideInAnimation");
            prepareButtons = (Storyboard)FindResource("PrepareButtons");
            noAnimations = (Storyboard)FindResource("NoAnimations");


            ///position
            this.Height = SystemParameters.PrimaryScreenHeight - 1;
            this.Left = SystemConfig.DesktopWorkingArea.Right - this.Width - 12;
            this.Top = SystemConfig.DesktopWorkingArea.Top + 1;

            /// hiding window
            HideWindow();

            charmsClock.Update(true);

            /// checking for cursor location
            this.Loaded += delegate { CheckCursorLocation(); };
        }
        void CheckCursorLocation()
        {
            SetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE, (GetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Elapsed += delegate
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    UpdateHover();
                    GlobalConfig.SaveConfig();
                }
                ));


            };
            timer1.Interval = 1000;
            timer1.Start();
            timer.Elapsed += delegate
            {
                this.Dispatcher.Invoke(new Action(delegate
                {

                    Point cursorPosition = GetMouseLocation.GetMousePosition();

                    if (cursorPosition.X + 1 == System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width && cursorPosition.Y == SystemConfig.DesktopWorkingArea.Top && GlobalConfig.IsEnabled && !windowVisible)
                    {
                        var bc = new BrushConverter();
                        this.Background = GlobalConfig.GetConfig("Transparent");
                        CharmsGrid.Visibility = Visibility.Visible;

                        if (!isAnimating && GlobalConfig.EnableAnimations)
                        {
                            isAnimating = true;
                            BeginStoryboard(slideInButtons);
                        }
                        else if (!GlobalConfig.EnableAnimations)
                        {
                            BeginStoryboard(noAnimations);
                        }

                        this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 1;
                        this.Top = SystemConfig.DesktopWorkingArea.Top + 1;
                    }

                    if (Keyboard.IsKeyDown(Key.LWin) && Keyboard.IsKeyDown(Key.C) && GlobalConfig.EnableKeyboardShortcut && (GlobalConfig.IsEnabled || GlobalConfig.OverrideCharmsBarOffSetting))
                    {
                        this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
                        this.Top = System.Windows.SystemParameters.WorkArea.Top;
                        CharmsGrid.Visibility = Visibility.Visible;


                        if (!isAnimating && GlobalConfig.EnableAnimations)
                        {
                            isAnimating = true;
                            windowVisible = true;
                            BeginStoryboard(slideInButtons);
                            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("(Window.Background).(SolidColorBrush.Color)"));
                            Storyboard storyboard = new Storyboard();
                            storyboard.Children.Add(fadeIn);
                            storyboard.Begin(this);
                            storyboard.Completed += delegate { isAnimating = false; };
                            charmsClock.Update();
                        }
                        else if (!GlobalConfig.EnableAnimations)
                        {
                            windowVisible = true;
                            BeginStoryboard(noAnimations);
                            this.Background = GlobalConfig.GetConfig("bg");
                            charmsClock.Update();
                        }



                        StartButtonIcon.Background = SystemConfig.AccentColor();
                    }
                    if (Keyboard.IsKeyDown(Key.Escape) && GlobalConfig.EnableKeyboardShortcut)
                    {
                        isAnimating = true;
                        HideWindow();
                    }
                }));

            };
            timer.Interval = 1;
            timer.Start();
        }
        private void OnButtonClick(object sender, MouseButtonEventArgs e)
        {
            Grid? button = sender as Grid;
            if (GlobalConfig.HideWindowAfterClick)
                HideWindow();
            if (button != null)
                switch (button.Name)
                {
                    case "Button1":
                        ClickHandler.Do(1);
                        break;
                    case "Button2":
                        ClickHandler.Do(2);
                        break;
                    case "Button3":
                        ClickHandler.Do(3);
                        break;
                    case "Button4":
                        ClickHandler.Do(4);
                        break;
                    case "Button5":
                        ClickHandler.Do(5);
                        break;
                    default:
                        MessageBox.Show($"Error: Unknown button with id \"{button.Name}\"", "Error: Unknown Button", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            else
                MessageBox.Show($"Error: Null button", "Error: Unknown Button", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void Window_MouseLeave(Object sender, MouseEventArgs e)
        {
            HideWindow();
        }

        private void CharmsGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Top = System.Windows.SystemParameters.WorkArea.Top;
            if (GlobalConfig.EnableAnimations)
            {
                Storyboard.SetTargetProperty(fadeIn, new PropertyPath("(Window.Background).(SolidColorBrush.Color)"));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(fadeIn);
                storyboard.Begin(this);
                StartButtonIcon.Background = SystemConfig.AccentColor();
            }
            else
            {
                this.Background = GlobalConfig.GetConfig("bg");
            }

            if (charmsClock.Opacity != 1) 
                charmsClock.Update();

        }
        public void HideWindow()
        {
            fadeOut.Completed += delegate
            {
                BeginStoryboard(prepareButtons);
                isAnimating = false;
                windowVisible = false;
                this.Background = GlobalConfig.GetConfig("Hide");
                charmsClock.HideClock();
                CharmsGrid.Visibility = Visibility.Collapsed;
                StartButtonIcon.Background = GlobalConfig.GetConfig("White");
                BeginAnimation(OpacityProperty, backTo1Opacity);
            };

            if (!GlobalConfig.EnableAnimations)
            {
                BeginStoryboard(prepareButtons);
                isAnimating = false;
                windowVisible = false;
                this.Background = GlobalConfig.GetConfig("Hide");
                charmsClock.HideClock();
                CharmsGrid.Visibility = Visibility.Collapsed;
                StartButtonIcon.Background = GlobalConfig.GetConfig("White");
                return;
            }
            BeginAnimation(UIElement.OpacityProperty, fadeOut);

        }
        void UpdateHover()
        {
            /// hover color
            Style hoverStyle = new Style
            {
                TargetType = typeof(Grid),
                Triggers =
                {
                    new Trigger
                    { Property = IsMouseOverProperty,  Value = true,
                        Setters = { new Setter { Property = BackgroundProperty, Value = GlobalConfig.GetConfig("hover")} }
                    }
                }
            };
            Button1.Style = hoverStyle;
            Button2.Style = hoverStyle;
            Button3.Style = hoverStyle;
            Button4.Style = hoverStyle;
            Button5.Style = hoverStyle;
            
            /// setting text and hover color
            if (Text1.Foreground != GlobalConfig.GetConfig("text"))
                try
                {
                    Text1.Foreground = GlobalConfig.GetConfig("text");
                    Text2.Foreground = GlobalConfig.GetConfig("text");
                    Text3.Foreground = GlobalConfig.GetConfig("text");
                    Text4.Foreground = GlobalConfig.GetConfig("text");
                    Text5.Foreground = GlobalConfig.GetConfig("text");
                }
                catch { }
        }
    }
}
