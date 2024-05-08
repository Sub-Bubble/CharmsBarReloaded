using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeWifi;
using static NativeWifi.Wlan;

namespace CharmsBarReloaded.Utilities
{
    public static class WifiStatus
    {

        ///wifi connectivity status. 
        ///buggy, inaccurate (probably), but hey, at least it works (at least, should)!
        static WlanClient wlanClient = new WlanClient();
        public static string GetWifiLinkQuality()
        {
            try
            {
                Wlan.WlanBssEntry[] wlanBssEntries = wlanClient.Interfaces[0].GetNetworkBssList();
                //MessageBox.Show($"Number of wlan clients: {wlanClient.Interfaces.Length}"); //use for debugging
                WlanBssEntry wifiEntry = wlanBssEntries[0];
                int linkQuality = Convert.ToInt32(wifiEntry.linkQuality) / 20 + 1;
                switch (linkQuality)
                {
                    case 1:
                        return "Weakest";
                    case 2:
                        return "Weak";
                    case 3:
                        return "Medium";
                    case 4:
                        return "Strong";
                    case 5:
                    case 6:
                        return "Max";
                    default:
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Either you have no WiFi module or my code has errored out. Error code: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
