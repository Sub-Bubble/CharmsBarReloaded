namespace CharmsBarReloaded.Config
{
    public partial class CharmsConfig
    {
        public class CharmsBarConfig
        {
            public bool IsEnabled { get; set; } = true;
            public bool HideWindowAfterClick { get; set; } = true;
            public string TextColor { get; set; } = "d3d3d3";
            public string HoverColor { get; set; } = "4c4c4c";
            public string BackgroundColor { get; set; } = "000000";
            public bool EnableKeyboardShortcut { get; set; } = true;
            public bool KeyboardShortcutOverridesOffSetting { get; set; } = false;
            public string[] ButtonActions { get; set; } = ["Search", "Share", "Start", "Devices", "Settings"];
            public bool[] UsesDynamicColor { get; set; } = [false, false, true, false, false];
            public static HashSet<string> ValidActions { get { return new HashSet<string> { "Search", "Share", "Start", "Devices", "Settings" }; } }
        }
    }
}
