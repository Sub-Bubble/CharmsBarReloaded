using CharmsBarReloaded.Properties;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Management;
using CoreAudio;

namespace CharmsBarReloaded.Settings
{
    /// <summary>
    /// Interaction logic for SettingsHome.xaml
    /// </summary>
    public partial class SettingsHome : Page
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        BrightnessControl brightnessControl = new BrightnessControl();
        public SettingsHome()
        {
            InitializeComponent();
            this.Loaded += SettingsHome_Loaded;
        }

        private void SettingsHome_Loaded(object sender, RoutedEventArgs e)
        {
            //making hitboxes solid
            Separator1.Width = this.Width;
            Separator1.Height = this.Height;
            Separator2.Width = this.Width;
            Separator2.Height = this.Height;
            Separator3.Width = this.Width;
            Separator3.Height = this.Height;

            managementObjCollection = new ManagementObjectSearcher(new ManagementScope("root\\WMI"), new SelectQuery("WmiMonitorBrightness")).Get();
            networkImage.Source = new BitmapImage(new Uri(@$"../Assets/CharmsClockIcons/{Networking.NetworkStatus()}.png", UriKind.Relative));
            keyboardLayout.Text = InputLanguage.CurrentInputLanguage.Culture.ThreeLetterISOLanguageName.ToUpper();
            brightnessText.Text = getcurentBrightness();
            timer.Elapsed += delegate
            {
                string brightness = getcurentBrightness();
                //getting default audio device and its volume
                MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator(Guid.Empty);
                MMDevice device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                int volume = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                this.Dispatcher.Invoke(() =>
                {
                    keyboardLayout.Text = InputLanguage.CurrentInputLanguage.Culture.ThreeLetterISOLanguageName.ToUpper();
                    networkImage.Source = new BitmapImage(new Uri(@$"../Assets/CharmsClockIcons/{Networking.NetworkStatus()}.png", UriKind.Relative));
                    volumeText.Text = $"{volume}";
                    brightnessText.Text = getcurentBrightness();
                    if (volume == 0 || device.AudioEndpointVolume.Mute)
                        volumeImage.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/VolumeMute.png", UriKind.Relative));
                    else volumeImage.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/Volume.png", UriKind.Relative));
                });
            };
            timer.Interval = 100;
            timer.Start();
        }

        ManagementObjectCollection managementObjCollection;
        private string getcurentBrightness()
        {
            try
            {
                foreach (ManagementObject managementObject in managementObjCollection)
                {
                    return $"{managementObject["CurrentBrightness"]}";
                }

            }
            catch
            {
                return "Unavailable";
            }
            return "Unavailable";
        }

        private void General_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(1);
        }

        private void Custiomization_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(2);
        }

        private void About_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(3);
        }
        private void OldSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.Do(-2);
        }

        private void OsSettings_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.Do(-3);
        }

        private void ControlPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.Do(-4);
        }

        private void Network_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = "ms-availablenetworks:", UseShellExecute = true});
        }


        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        private void KeyboardLayout_MouseDown(object sender, MouseButtonEventArgs e)
        {

            // windows + space
            keybd_event(0x5B, 0, 0, UIntPtr.Zero);
            keybd_event(0x20, 0, 0, UIntPtr.Zero);
            keybd_event(0x20, 0, 0x0002, UIntPtr.Zero);
            keybd_event(0x5B, 0, 0x0002, UIntPtr.Zero);

            keyboardLayout.Text = InputLanguage.CurrentInputLanguage.Culture.ThreeLetterISOLanguageName.ToUpper();
        }

        private void volume_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point cursorPosition = GetMouseLocation.GetMousePosition();
            Process.Start(new ProcessStartInfo { FileName = "sndvol.exe", Arguments = $"-f {cursorPosition.Y*65536+cursorPosition.X}", UseShellExecute = true });
            CharmsSettings.windowBusy = true;
        }

        private void Notifications_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"ms-actioncenter:", UseShellExecute = true });
        }

        private void Brightness_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CharmsSettings.windowBusy = true;
            brightnessControl.Show();
            brightnessControl.Top = ClickHandler.GetWindowLocation("Top") + ClickHandler.GetWindowLocation("Height") - 250;
            brightnessControl.Left = ClickHandler.GetWindowLocation("Left") + ClickHandler.GetWindowLocation("Width") - 105;
            brightnessControl.Focus();
            brightnessControl.Deactivated += (sender, args) => { CharmsSettings.windowBusy = false; ClickHandler.Do(-5); brightnessControl.Hide(); };
            brightnessControl.brightnessText.Text = getcurentBrightness();
            if (getcurentBrightness() == "Unavailable")
                brightnessControl.IsEnabled = false;
            else 
                brightnessControl.brightnessSlider.Value = double.Parse(getcurentBrightness());
        }

        private void Power_MouseDown(object sender, MouseButtonEventArgs e)
        {
            shutdownPopup.IsOpen = !shutdownPopup.IsOpen;
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!powerGrid.IsMouseOver && !shutdownPopup.IsMouseOver)
                shutdownPopup.IsOpen = false;
            if (shutdownPopup.IsMouseOver)
                shutdownPopup.IsOpen = true;
        }

        private void Sleep_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = Environment.ExpandEnvironmentVariables(@"%WINDIR%\System32\rundll32.exe"), Arguments = "powrprof.dll, SetSuspendState 0, 1, 0", UseShellExecute = true});
        }

        private void Shutdown_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "shutdown.exe", Arguments = "-s -t 0", UseShellExecute = true});
        }

        private void Restart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = "shutdown.exe", Arguments = "-r -t 0", UseShellExecute = true });
        }
    }
}