using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
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
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            this.Loaded += CharmsClock_Loaded;
        }
        private void CharmsClock_Loaded(object sender, RoutedEventArgs e)
        {
            this.Top = SystemConfig.GetDesktopWorkingArea.Bottom - 188;
            this.Left = 50;
            if (!App.charmsConfig.charmsClockConfig.SyncClockSettings)
            {
                this.Background = GetBrush.GetBrushFromHex(App.charmsConfig.charmsClockConfig.BackgroundColor);
                var brush = GetBrush.GetBrushFromHex(App.charmsConfig.charmsClockConfig.TextColor);
                this.Hours.Foreground = brush;
                this.Minutes.Foreground = brush;
                this.Separator.Foreground = brush;
                this.Date.Foreground = brush;
            }
            else
                //omitting using charms bar text color is not a mistake
                this.Background = GetBrush.GetBrushFromHex(App.charmsConfig.charmsBarConfig.BackgroundColor);
            Update();
            BeginAnimation(UIElement.OpacityProperty, noAnimationOut);
            
            this.Loaded -= CharmsClock_Loaded;
        }

        public void Update()
        {
            switch (SystemConfig.IsCharging)
            {
                case "NoBattery":
                    if (App.charmsConfig.charmsClockConfig.ShowChargingOnDesktop)
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
