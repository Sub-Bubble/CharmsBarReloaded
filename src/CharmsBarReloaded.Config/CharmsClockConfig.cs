namespace CharmsBarReloaded.Config
{
    public partial class CharmsConfig
    {
        public class CharmsClockConfig
        {
            public bool IsEnabled { get; set; } = true;
            public string BackgroundColor { get; set; } = "000000";
            public string TextColor { get; set; } = "ffffff";
            public bool ShowChargingOnDesktop { get; set; } = false;
            public bool SyncClockSettings { get; set; } = true;
        }
    }
}
