using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace CharmsBarReloaded.Config
{
    class SystemConfig
    {
        static BrushConverter bc = new BrushConverter();
        public static readonly Rect DesktopWorkingArea = SystemParameters.WorkArea;
        public static bool IsLightTheme()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            return value is int i && i > 0;
        }
        public static int BatteryPercentage()
        {
            return Convert.ToInt32(System.Windows.Forms.SystemInformation.PowerStatus.BatteryLifePercent * 100);
        }
        public static Brush AccentColor()
        {

            int colorValue = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Accent", "AccentColorMenu", null);

            int red = (colorValue >> 16) & 0xFF;
            int green = (colorValue >> 8) & 0xFF;
            int blue = colorValue & 0xFF;
            var color = Color.FromArgb(255, (byte)blue, (byte)green, (byte)red);
            return new SolidColorBrush(color);
        }
        public static bool StartupKeyExists()
        {
            RegistryKey startupKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            var value = startupKey.GetValue("CharmsBarReloaded");
            if (value == null)
                return false;
            else if (value != AppContext.BaseDirectory)
                startupKey.SetValue("CharmsBarReloaded", AppContext.BaseDirectory);
            return true;
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
