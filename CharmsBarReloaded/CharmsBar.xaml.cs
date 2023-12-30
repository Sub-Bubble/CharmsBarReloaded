using CharmsBarReloaded.Properties;
using CharmsBarReloaded.Worker;
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

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for CharmsBar.xaml
    /// </summary>
    public partial class CharmsBar : Window
    {
        /// hiding window from alttab      
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;
        CharmsClock charmsClock = new CharmsClock();

        public CharmsBar()
        {
            /// initialing config and setting window location
            ButtonConfig.SetVars();
            GlobalConfig.LoadConfig();
            InitializeComponent(); //init window

            ///position
            this.Height = SystemParameters.PrimaryScreenHeight - 1;
            this.Left = SystemConfig.DesktopWorkingArea.Right - this.Width - 12;
            this.Top = SystemConfig.DesktopWorkingArea.Top + 1;
            MouseLeave += Window_MouseLeave;

            /// hiding window
            HideWindow();

            /// checking for cursor location
            this.Loaded += delegate { CheckCursorLocation(); };
        }
        private void CheckCursorLocation()
        {
            SetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE, (GetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += delegate
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    Point cursorPosition = GetMouseLocation.GetMousePosition();

                    /* Debug */
                    //Text2.Content = $"{desktopWorkingArea.Right}, {desktopWorkingArea.Top}";
                    //Text3.Content = $"{cursorPosition.X}, {cursorPosition.Y}";

                    if (cursorPosition.X + 1 == SystemConfig.DesktopWorkingArea.Right && cursorPosition.Y == SystemConfig.DesktopWorkingArea.Top && GlobalConfig.IsEnabled)
                    {
                        var bc = new BrushConverter();
                        this.Background = GlobalConfig.GetConfig("Transparent");
                        CharmsGrid.Visibility = Visibility.Visible;
                        this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 1;
                        this.Top = SystemConfig.DesktopWorkingArea.Top + 1;
                    }
                }));

            };
            timer.Interval = 1;
            timer.Start();
        }
        private void OnButtonClick(object sender, MouseButtonEventArgs e)
        {
            Grid button = sender as Grid;
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
            this.Background = GlobalConfig.GetConfig("bg");
            charmsClock.Update();
        }
        public void HideWindow()
        {
            this.Background = GlobalConfig.GetConfig("Hide");
            charmsClock.Hide();
            CharmsGrid.Visibility = Visibility.Collapsed;
        }
    }
}
