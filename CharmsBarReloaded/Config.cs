using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharmsBarReloaded
{
    class SystemConfig
    {
        public static bool IsLightTheme()
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            var value = key?.GetValue("AppsUseLightTheme");
            return value is int i && i > 0;
        }
    }
    class GlobalConfig
    {
        /// Constants. Only changed manually
        public const string VersionString = "b1.0";
        public const int Version = 1;

        /// other vars
        public static bool HideWindowAfterClick {  get; set; }
        public static string BackgroundColor {  get; set; }
        public static string TextColor {  get; set; }
        public static string HoverColor {  get; set; }
        public static bool UseLightTheme {  get; set; }
        public static void LoadConfig()
        {
            try
            {
                HideWindowAfterClick = CharmsBarReloaded.Properties.Settings.Default.HideWindowAfterClick;
                BackgroundColor = CharmsBarReloaded.Properties.Settings.Default.BackgroundColor;
                TextColor = CharmsBarReloaded.Properties.Settings.Default.TextColor;
                HoverColor = CharmsBarReloaded.Properties.Settings.Default.HoverColor;
                UseLightTheme = CharmsBarReloaded.Properties.Settings.Default.UseLightTheme;
            }
            catch
            {
                /// use default config
                HideWindowAfterClick = true;
                BackgroundColor = "000000";
                TextColor = "d3d3d3";
                HoverColor = "4c4c4c";
                UseLightTheme = false;
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
