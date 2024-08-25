using System.Diagnostics;
using System.Runtime.InteropServices;
using MessageBox = System.Windows.MessageBox;

namespace CharmsBarReloaded
{
    public partial class App : System.Windows.Application
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public static void ClickHandler(string action)
        {
            switch (action)
            {
                case "Search":
                    keybd_event(0x5B, 0, 0, UIntPtr.Zero);
                    keybd_event(0x53, 0, 0, UIntPtr.Zero);
                    keybd_event(0x53, 0, 0x02, UIntPtr.Zero);
                    keybd_event(0x5B, 0, 0x02, UIntPtr.Zero);
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
                    MessageBox.Show("Settings coming back in future beta builds");
                    break;
                case "FocusSettings":
                    MessageBox.Show("Settings coming back in future beta builds");
                    break;

                //settings navigation
                case "SettingsHome":
                    MessageBox.Show("Settings coming back in future beta builds");
                    break;
                case "SettingsGeneral":
                    MessageBox.Show("General settings are on a rewrite, will come back soon!");
                    break;
                case "SettingsPersonalization":
                    MessageBox.Show("Personalization settings are on a rewrite, will come back soon!");
                    break;
                case "SettingsAbout":
                    MessageBox.Show("About page is on a rewrite, will come back soon!");
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
