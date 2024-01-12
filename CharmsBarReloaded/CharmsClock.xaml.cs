using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for CharmsClock.xaml
    /// </summary>
    public partial class CharmsClock : Window
    {
        // no alt tab
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;

        public CharmsClock()
        {            
            InitializeComponent();
            this.Loaded += delegate 
            {
                // hide from alttab
                SetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE, (GetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
                System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
                timer.Tick += new EventHandler(AlwaysUpdate);
                timer.Start();
                timer.Interval = TimeSpan.FromSeconds(1);
            };
        }
        private void AlwaysUpdate(object sender, EventArgs e)
        {
            // time logic
            if (DateTime.Now.Hour.ToString().Length == 1)
            {
                ClockHours.Margin = new Thickness(94, 3, 0, -106);
                ClockSeparator.Margin = new Thickness(138, -24.99, -190, -98);
                ClockMinutes.Margin = new Thickness(157, -17, -190, -198);
                Week.Margin = new Thickness(267, 2, 0, -18);
                Date.Margin = new Thickness(269, 3, 0, -24);
            }
            else
            {
                ClockHours.Margin = new Thickness(95, 3, 0, -106);
                ClockSeparator.Margin = new Thickness(169, -24.99, -190, -98);
                ClockMinutes.Margin = new Thickness(188, -17, -190, -198);
                Week.Margin = new Thickness(298, 2, 0, -18);
                Date.Margin = new Thickness(300, 4, 0, -24);
            }
            ClockHours.Content = DateTime.Now.Hour.ToString();
            ClockMinutes.Content = DateTime.Now.ToString("mm");
            Date.Content = DateTime.Today.ToString("MMMM d");
            Week.Content = DateTime.Today.ToString("dddd");
            
            // Battery icon logic
            if (SystemInformation.PowerStatus.BatteryChargeStatus.ToString() == "NoSystemBattery")
                BatteryLife.Visibility = Visibility.Hidden;
            else
            {
                switch (SystemConfig.BatteryPercentage())
                {
                    case 0:
                        BatteryLife.Source = new BitmapImage(new Uri(@"/Assets/CharmsClockIcons/Battery0.png", UriKind.Relative)); break;
                    case 1: case 2: case 3: case 4:
                        BatteryLife.Source = new BitmapImage(new Uri(@"/Assets/CharmsClockIcons/Battery1.png", UriKind.Relative)); break;
                    case 5: case 6: case 7: case 8: case 9:
                        BatteryLife.Source = new BitmapImage(new Uri(@"/Assets/CharmsClockIcons/Battery5.png", UriKind.Relative)); break;
                    default:
                        BatteryLife.Source = new BitmapImage(new Uri(@$"/Assets/CharmsClockIcons/Battery{(SystemConfig.BatteryPercentage()/10)}0.png", UriKind.Relative)); break;
                        
                }
            }
            var ChargeStatus = SystemInformation.PowerStatus.PowerLineStatus;
            switch (ChargeStatus)
            {
                case System.Windows.Forms.PowerLineStatus.Online:
                    if (!GlobalConfig.ShowChargingOnDesktop && SystemInformation.PowerStatus.BatteryChargeStatus.ToString() == "NoSystemBattery")
                        IsCharging.Visibility = Visibility.Collapsed;
                    else
                        IsCharging.Visibility = Visibility.Visible;
                    break;
                case System.Windows.Forms.PowerLineStatus.Offline:
                    IsCharging.Visibility = Visibility.Collapsed; break;
            }

            // Internet icon logic
            switch (Networking.NetworkStatus())
            {
                case "Wifi":
                    InternetStatus.Source = new BitmapImage(new Uri(@$"/Assets/CharmsClockIcons/WifiInternetMax.png", UriKind.Relative)); break;
                case "Ethernet":
                    InternetStatus.Source = new BitmapImage(new Uri(@$"/Assets/CharmsClockIcons/EthernetInternet.png", UriKind.Relative)); break;
                case "Unknown":
                    InternetStatus.Source = new BitmapImage(new Uri(@$"/Assets/CharmsClockIcons/NoInternet.png", UriKind.Relative)); break;
            }
        }
        public void Update()
        {
            this.Left = 51;
            this.Top = SystemConfig.DesktopWorkingArea.Bottom - 188;
            this.Show();
        }
    }
}