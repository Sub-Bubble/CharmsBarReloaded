using CoreAudio;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Point = System.Windows.Point;
using PowerLineStatus = System.Windows.Forms.PowerLineStatus;

namespace CharmsBarReloaded.Config
{
    public class SystemConfig
    {
        /// <summary>
        /// Class that returns and sets system settings 
        /// used by application
        /// </summary>
        public static Rect GetDesktopWorkingArea { get { return SystemParameters.WorkArea; } }
        public static int GetBatteryPercentage { get { return Convert.ToInt32(SystemInformation.PowerStatus.BatteryLifePercent * 100); } }
        public static string IsCharging
        {
            get
            {
                if (SystemInformation.PowerStatus.BatteryChargeStatus == BatteryChargeStatus.NoSystemBattery)
                    return "NoBattery";
                switch (SystemInformation.PowerStatus.PowerLineStatus)
                {
                    case PowerLineStatus.Online:
                        return "Charging";
                    case PowerLineStatus.Offline:
                        return "NotCharging";
                    //code below should be impossible to reach
                    default:
                        return string.Empty;
                }
            }
        }

        public static bool IsLightTheme
        {
            get
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
                var value = key?.GetValue("AppsUseLightTheme");
                return value is int i && i > 0;
            }
        }
        public static System.Windows.Media.Brush GetAccentColor
        {
            get
            {
                int colorValue = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Accent", "AccentColorMenu", null);
                var color = System.Windows.Media.Color.FromArgb(255, (byte)(colorValue & 0xFF), (byte)((colorValue >> 8) & 0xFF), (byte)((colorValue >> 16) & 0xFF));
                return new SolidColorBrush(color);
            }
        }
        public static bool StartupKeyExists
        {
            get
            {
                RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                var value = startupKey.GetValue("CharmsBarReloaded");
                if (value == null)
                    return false;
                else if (value != AppContext.BaseDirectory)
                    startupKey.SetValue("CharmsBarReloaded", AppContext.BaseDirectory);
                return true;
            }
        }
        public static string GetKeyboardLayout
        {
            get
            {
                return InputLanguage.CurrentInputLanguage.Culture.ThreeLetterISOLanguageName.ToUpper();
            }
        }

        private static ManagementObjectCollection brightnessValues = new ManagementObjectSearcher(new ManagementScope("root\\WMI"), new SelectQuery("WmiMonitorBrightness")).Get();
        private static ManagementObjectCollection brightnessMethods = new ManagementObjectSearcher(new ManagementScope("root\\WMI"), new SelectQuery("WmiMonitorBrightnessMethods")).Get();
        public static int DeviceBrightness
        {
            get //will return 0-100 if there is a display, and -1 if there is none
            {
                try
                {
                    foreach (ManagementObject managementObject in brightnessValues)
                    {
                        return (int)managementObject["CurrentBrightness"];
                    }

                }
                catch
                {
                    return -1;
                }
                return -1;
            }
            set
            {
                try
                {
                    foreach (ManagementObject managementObject in brightnessMethods)
                    {
                        managementObject.InvokeMethod("WmiSetBrightness", new Object[] { UInt32.MaxValue, (byte)value });
                        break;
                    }
                }
                catch { Log.Error("Cannot set display brightness!"); }
            }
        }

        private static MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator(Guid.Empty);
        private static MMDevice device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        public static int GetDeviceVolume
        {
            get
            {
                return (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
            }
        }
        public static bool IsVolumeMuted
        {
            get
            {
                return device.AudioEndpointVolume.Mute;
            }
        }

        private static string cachedInternetStatus = "NoInternet";
        private static bool isCachingInternetStatus = false;
        private async static Task UpdateCachedInternetStatus()
        {
            if (isCachingInternetStatus) return;

            isCachingInternetStatus = true;
            cachedInternetStatus = await WifiConfig.GetStatus();

            isCachingInternetStatus = false;
        }

        public static string GetInternetStatus
        {
            get
            {
                UpdateCachedInternetStatus();
                return cachedInternetStatus;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point { public Int32 X; public Int32 Y; };
        public static Point GetMouseLocation
        {
            get
            {
                var w32Mouse = new Win32Point();
                GetCursorPos(ref w32Mouse);
                return new System.Windows.Point(w32Mouse.X, w32Mouse.Y);
            }
        }

        public static void SetupStartupKey(object value)
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            switch (value)
            {
                case true:
                    startupKey.SetValue("CharmsBarReloaded", AppContext.BaseDirectory);
                    break;
                case false:
                    startupKey.DeleteValue("CharmsBarReloaded");
                    break;
            }
        }
    }
}
