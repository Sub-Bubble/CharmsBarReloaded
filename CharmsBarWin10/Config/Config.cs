using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharmsBarWin10.Config
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
        public const string VersionString = "b1.0";
        public const int Version = 1;
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
