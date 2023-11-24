using CharmsBarWin10.Worker;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
        /// hidind window from alttab
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;


        public CharmsBar()
        {
            /// initialing config and setting window location
            ButtonConfig.SetVars();
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            InitializeComponent(); //init window

            ///position
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 1;
            this.Left = desktopWorkingArea.Right - this.Width - 12;
            this.Top = desktopWorkingArea.Top + 1;
            MouseLeave += Window_MouseLeave;

            /// hiding window
            HideWindow();

            /// Disabled, will be reworked in a future build
            /*
            /// setting theme based on windows settings
            switch (Config.SystemConfig.IsLightTheme())
            {
                case true:
                    //this.Background = Brushes.White;
                    this.Background = Brushes.White;
                    MessageBox.Show("Sorry, but light theme is not supported. Maybe in a future.\nMeanwhile, enjoy dark mode experience!", "Light mode coming soon", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                case false:
                    this.Background = Brushes.Black;
                    break;
            }*/
            /// checking for cursor location
            this.Loaded += delegate
            {
                SetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE, (GetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += delegate
                {
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        Point cursorPosition = GetMouseLocation.GetMousePosition();
                        if (cursorPosition.X + 1 == desktopWorkingArea.Right && cursorPosition.Y == desktopWorkingArea.Top)
                        {
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
        private void OnButtonClick(object sender, MouseButtonEventArgs e)
        {
            Grid button = sender as Grid;
            if (GlobalConfig.HideWindowAfterClick)
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
            this.Background = (Brush)bc.ConvertFrom($"#FF{GlobalConfig.UserColor}");
        }
        public void HideWindow()
        {
            var bc = new BrushConverter();
            this.Background = (Brush)bc.ConvertFrom("#00000000");
            CharmsGrid.Visibility = Visibility.Collapsed;
        }
    }
}
