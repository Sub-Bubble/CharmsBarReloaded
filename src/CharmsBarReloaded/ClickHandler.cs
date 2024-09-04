using CharmsBarReloaded.CharmsSettings.Pages;
using CharmsBarReloaded.Config;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace CharmsBarReloaded
{
    public partial class App : Application
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public static void ClickHandler(string action)
        {
            switch (action)
            {
                //main charms bar actions
                case "Search":
                    Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"shell:::{{2559a1f8-21d7-11d4-bdaf-00c04f60b9f0}}", CreateNoWindow = true });
                    break;
                case "Share":
                    keybd_event(0x2C, 0, 0, UIntPtr.Zero);
                    keybd_event(0x2C, 0, 0x02, UIntPtr.Zero);
                    break;
                case "Devices":
                    Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"ms-settings-connectabledevices:devicediscovery", CreateNoWindow = true });
                    break;
                case "Start":
                    keybd_event(0x11, 0, 0, UIntPtr.Zero);
                    keybd_event(0x1B, 0, 0, UIntPtr.Zero);
                    keybd_event(0x11, 0, 0x02, UIntPtr.Zero);
                    keybd_event(0x1B, 0, 0x02, UIntPtr.Zero);
                    break;
                case "Settings":
                    charmsSettings.isBusy = true;

                    Current.Dispatcher.BeginInvoke(new Action(() => {
                        charmsSettings.Show();
                       charmsSettings.Focus();
                    }), System.Windows.Threading.DispatcherPriority.Input);
                    break;
                case "FocusSettings":
                    MessageBox.Show("Settings coming back in future beta builds");
                    break;

                //settings navigation
                case "SettingsHome":
                    charmsSettings.frame.Content = settingsHome;
                    charmsConfig.Save();
                    translationManager = new TranslationManager().Load(charmsConfig.CurrentLocale);
                    charmsBar.Window_Reload(charmsConfig, translationManager);
                    charmsBar.HideWindow();
                    settingsHome.ReloadStrings();
                    settingsGeneral.ReloadStrings();
                    settingsPersonalization.ReloadStrings();
                    settingsAbout.ReloadStrings();
                    break;
                case "SettingsGeneral":
                    charmsSettings.frame.Content = settingsGeneral;
                    break;
                case "SettingsPersonalization":
                    charmsSettings.frame.Content = settingsPersonalization;
                    break;
                case "SettingsAbout":
                    charmsSettings.frame.Content = settingsAbout;
                    break;

                //Windows Actions
                case "OsSettings":
                    Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"ms-settings:", CreateNoWindow = true });
                    break;
                case "ControlPanel":
                    Process.Start(new ProcessStartInfo { FileName = "control.exe", CreateNoWindow = true });
                    break;
                case "Network":
                    Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"ms-availablenetworks:", CreateNoWindow = true });
                    break;
                case "VolumeSettings":
                    var process = Process.GetProcessesByName("sndvol");
                    if (process.Any())
                    {
                        foreach (var current in process)
                            current.Kill();
                        break;
                    }
                    Process.Start(new ProcessStartInfo { FileName = "sndvol.exe", Arguments = $"-f {SystemConfig.GetMouseLocation.Y * 65536 + SystemConfig.GetMouseLocation.X}", UseShellExecute = true });
                    break;
                case "ChangeKeyboardLayout":
                    keybd_event(0x5B, 0, 0, UIntPtr.Zero);
                    keybd_event(0x20, 0, 0, UIntPtr.Zero);
                    keybd_event(0x20, 0, 0x0002, UIntPtr.Zero);
                    keybd_event(0x5B, 0, 0x0002, UIntPtr.Zero);
                    break;
                case "Notifications":
                    Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"ms-actioncenter:", CreateNoWindow = true });
                    break;
                case "Shutdown":
                    Process.Start(new ProcessStartInfo { FileName = "shutdown.exe", Arguments = $"-s -t 0", CreateNoWindow = true });
                    break;
                case "Sleep":
                    Process.Start(new ProcessStartInfo { FileName = Environment.ExpandEnvironmentVariables(@"%WINDIR%\System32\rundll32.exe"), Arguments = "powrprof.dll, SetSuspendState 0, 1, 0", CreateNoWindow = true });
                    break;
                case "Restart":
                    Process.Start(new ProcessStartInfo { FileName = "shutdown.exe", Arguments = $"-r -t 0", CreateNoWindow = true });
                    break;
            }
        }
    }
}
