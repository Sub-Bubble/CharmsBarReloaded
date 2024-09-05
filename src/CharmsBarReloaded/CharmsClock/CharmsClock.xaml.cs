using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.CharmsClock
{
    /// <summary>
    /// Interaction logic for CharmsClock.xaml
    /// </summary>
    public partial class CharmsClock : Window
    {
        public CharmsClock()
        {
            InitializeComponent();
        }
        public void Update(CharmsConfig.CharmsClockConfig config)
        {
            switch (SystemConfig.IsCharging)
            {
                case "NoBattery":
                    if (config.ShowChargingOnDesktop)
                        IsCharging.Visibility = Visibility.Visible;
                    else
                        IsCharging.Visibility = Visibility.Collapsed;
                    BatteryLife.Visibility = Visibility.Collapsed;
                    break;
                case "Charging":
                    BatteryLife.Visibility = Visibility.Visible;
                    IsCharging.Visibility = Visibility.Visible;
                    UpdateBatteryLife();
                    break;
                case "NotCharging":
                    BatteryLife.Visibility = Visibility.Visible;
                    IsCharging.Visibility = Visibility.Collapsed;
                    UpdateBatteryLife();
                    break;
            }

            Hours.Content = DateTime.Now.Hour;
            Minutes.Content = DateTime.Now.ToString("mm");
            Date.Content = $"{DateTime.Today.ToString("dddd")}\n{DateTime.Today.ToString("M")}";
        }
        private void UpdateBatteryLife()
        {
            switch (SystemConfig.GetBatteryPercentage)
            {
                case 0:
                    BatteryLife.Source = new BitmapImage(new Uri(@"../Assets/CharmsClock/Battery0.png", UriKind.Relative)); break;
                case 1:
                case 2:
                case 3:
                case 4:
                    BatteryLife.Source = new BitmapImage(new Uri(@"../Assets/CharmsClock/Battery1.png", UriKind.Relative)); break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    BatteryLife.Source = new BitmapImage(new Uri(@"../Assets/CharmsClock/Battery5.png", UriKind.Relative)); break;
                case 100:
                    BatteryLife.Source = new BitmapImage(new Uri(@"../Assets/CharmsClock/BatteryFull.png", UriKind.Relative)); break;
                default:
                    BatteryLife.Source = new BitmapImage(new Uri(@$"../Assets/CharmsClock/Battery{(SystemConfig.GetBatteryPercentage / 10)}0.png", UriKind.Relative)); break;
            }
        }

        public void UpdateInternetStatus()
        {
            InternetStatus.Source = new BitmapImage(new Uri($"pack://application:,,,/Assets/CharmsClock/{SystemConfig.GetInternetStatus}.png"));
        }
    }
}
