using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CharmsBarReloaded
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
    class GlobalConfig
    {
        /// Constants. Only changed manually
        public const string VersionString = "a4.1";
        public const int Build = 16;

        /// other vars
        public static bool IsEnabled {  get; set; }
        public static bool HideWindowAfterClick {  get; set; }
        public static string BackgroundColor {  get; set; }
        public static string TextColor {  get; set; }
        public static string HoverColor {  get; set; }
        public static bool UseLightTheme {  get; set; }
        public static bool ShowChargingOnDesktop { get; set; }
        public static bool UseNetworkCaching {  get; set; }
        public static bool EnableKeyboardShortcut {  get; set; }
        public static void LoadConfig()
        {
            IsEnabled = true;
            UseNetworkCaching = true; /// This WILL be an option later
            try
            {
                HideWindowAfterClick = CharmsBarReloaded.Properties.Settings.Default.HideWindowAfterClick;
                BackgroundColor = CharmsBarReloaded.Properties.Settings.Default.BackgroundColor;
                TextColor = CharmsBarReloaded.Properties.Settings.Default.TextColor;
                HoverColor = CharmsBarReloaded.Properties.Settings.Default.HoverColor;
                UseLightTheme = CharmsBarReloaded.Properties.Settings.Default.UseLightTheme;
                ShowChargingOnDesktop = CharmsBarReloaded.Properties.Settings.Default.ShowChargingOnDesktop;
                EnableKeyboardShortcut = CharmsBarReloaded.Properties.Settings.Default.EnableKeyboardShortcut;
            }
            catch
            {
                /// use default config
                HideWindowAfterClick = true;
                BackgroundColor = "000000";
                TextColor = "d3d3d3";
                HoverColor = "4c4c4c";
                UseLightTheme = false;
                ShowChargingOnDesktop = false;
                EnableKeyboardShortcut = true;
            }
        }
        public static void ResetConfig()
        {
            /// use default config
            CharmsBarReloaded.Properties.Settings.Default.HideWindowAfterClick = true;
            CharmsBarReloaded.Properties.Settings.Default.BackgroundColor = "000000";
            CharmsBarReloaded.Properties.Settings.Default.TextColor = "d3d3d3";
            CharmsBarReloaded.Properties.Settings.Default.HoverColor = "4c4c4c";
            CharmsBarReloaded.Properties.Settings.Default.UseLightTheme = false;
            CharmsBarReloaded.Properties.Settings.Default.ShowChargingOnDesktop = false;
            CharmsBarReloaded.Properties.Settings.Default.EnableKeyboardShortcut = true;
            CharmsBarReloaded.Properties.Settings.Default.Save();
            LoadConfig();
        }
        public static void SaveConfig()
        {
            /// save config
            CharmsBarReloaded.Properties.Settings.Default.HideWindowAfterClick = HideWindowAfterClick;
            CharmsBarReloaded.Properties.Settings.Default.BackgroundColor = BackgroundColor;
            CharmsBarReloaded.Properties.Settings.Default.TextColor = TextColor;
            CharmsBarReloaded.Properties.Settings.Default.HoverColor = HoverColor;
            CharmsBarReloaded.Properties.Settings.Default.UseLightTheme = UseLightTheme;
            CharmsBarReloaded.Properties.Settings.Default.ShowChargingOnDesktop = ShowChargingOnDesktop;
            CharmsBarReloaded.Properties.Settings.Default.EnableKeyboardShortcut = EnableKeyboardShortcut;
            CharmsBarReloaded.Properties.Settings.Default.Save();
            /// fade in animation
            CharmsBar.fadeIn = new ColorAnimation
            {
                To = (Color)ColorConverter.ConvertFromString($"#FF{BackgroundColor.ToUpper()}"),
                Duration = TimeSpan.FromMilliseconds(100),
            };
        }
        public static Brush GetConfig(string name, string tryHexColor = null)
        {
            var bc = new BrushConverter();
            Brush result;

            if (tryHexColor != null)
                return (Brush)bc.ConvertFrom($"#FF{tryHexColor.ToUpper()}");
            
            switch (name)
            {
                case "Hide":
                    result = (Brush)bc.ConvertFrom("#00000000");
                    return result;
                case "bg":
                    result = (Brush)bc.ConvertFrom($"#FF{GlobalConfig.BackgroundColor.ToUpper()}");
                    return result;
                case "text":
                    result = (Brush)bc.ConvertFrom($"#FF{GlobalConfig.TextColor.ToUpper()}");
                    return result;
                case "hover":
                    result = (Brush)bc.ConvertFrom($"#FF{GlobalConfig.HoverColor.ToUpper()}");
                    return result;
                case "Transparent":
                    result = (Brush)bc.ConvertFrom("#01000000");
                    return result;
                case "White":
                    result = (Brush)bc.ConvertFrom("#FFFFFFFF");
                    return result;
            }
            return (Brush)bc.ConvertFrom("#FF000000"); //returning black when unknown
        }
    }
    class ButtonConfig
    {
        static string Button1_Action;
        static string Button2_Action;
        static string Button3_Action;
        static string Button4_Action;
        static string Button5_Action;
        public static string GetButtonConfig(int buttonId)
        {
            switch (buttonId)
            {
                case 1:
                    return Button1_Action;
                case 2:
                    return Button2_Action;
                case 3:
                    return Button3_Action;
                case 4:
                    return Button4_Action;
                case 5:
                    return Button5_Action;
                case -1:
                    return "Settings";
                case -2:
                    return "OldSettings";
                case -3:
                    return "OsSettings";
                case -4:
                    return "ControlPanel";
                case -5:
                    return "FocusSettings";
                default:
                    return "null";
            }
        }
        public static void SetVars()
        {
            Button1_Action = "Search";
            Button2_Action = "Share";
            Button3_Action = "Start";
            Button4_Action = "Devices";
            Button5_Action = "Settings";
        }
    }
}
