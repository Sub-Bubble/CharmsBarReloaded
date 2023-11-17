using CharmsBarWin10.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CharmsBarWin10
{
    /// <summary>
    /// Interaction logic for CharmsBar.xaml
    /// </summary>
    public partial class CharmsBar : Window
    {

        public CharmsBar()
        {
            /// initialing config and setting window location
            Config.ButtonConfig.SetVars();
            //this.Height = System.Windows.SystemParameters.PrimaryScreenHeight + 20;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight -1;
            InitializeComponent(); //init window
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            MouseLeave += Window_MouseLeave;

            this.Left = desktopWorkingArea.Right - this.Width - 12;
            //this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Top+1;

            /// setting theme based on windows settings
            switch (Config.SystemConfig.IsLightTheme())
            {
                case true:
                    //this.Background = Brushes.White;
                    this.Background = Brushes.Black;
                    MessageBox.Show("Sorry, but light theme is not supported. Maybe in a future.\nMeanwhile, enjoy dark mode experience!", "Light mode coming soon", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case false:
                    this.Background = Brushes.Black;
                    break;
            }
            this.Loaded += delegate
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += delegate
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        Mouse.Capture(this);
                        Point pointToWindow = Mouse.GetPosition(this);
                        Point pointToScreen = PointToScreen(pointToWindow);
                        Mouse.Capture(null);
                        if (pointToScreen.X + 1 == desktopWorkingArea.Right && pointToScreen.Y == desktopWorkingArea.Top)
                        {
                            //this.Opacity = 1.0;
                            var bc = new BrushConverter();
                            this.Background = (Brush)bc.ConvertFrom("#01000000");
                            CharmsGrid.Visibility = Visibility.Visible;
                            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight-1;
                            this.Top = desktopWorkingArea.Top+1;
                        }
                    }));
                };
                timer.Interval = 1;
                timer.Start();
            };
        }
        /*    This SHOULD work, but doesn't
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        private const int ExtendedWindowStyleToolWindow = WS_EX_TOOLWINDOW;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public static void SetWindowStyle(Window window, int style)
        {
            var hwnd = new WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, currentStyle | style);
            //SetWindowLong(hwnd, GWL_EXSTYLE, (GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_TOOLWINDOW) & ~ExtendedWindowStyleToolWindow);
        }*/

        private void OnButtonClick(object sender, MouseButtonEventArgs e)
        {
            Grid button = sender as Grid;
            if (Config.GlobalConfig.HideWindowAfterClick)
                HideWindow();
            if (button != null)
                switch (button.Name)
                {
                    case "one":
                        ClickHandler.Do(1);
                        break;
                    case "two":
                        ClickHandler.Do(2);
                        break;
                    case "three":
                        ClickHandler.Do(3);
                        break;
                    case "four":
                        ClickHandler.Do(4);
                        break;
                    case "five":
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
            var bc = new BrushConverter();
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Top = System.Windows.SystemParameters.WorkArea.Top;
            this.Background = (Brush)bc.ConvertFrom("#FF000000");
        }
        public void HideWindow()
        {
            var bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom("#00000000");
            CharmsGrid.Visibility = Visibility.Collapsed;
        }
    }
}
