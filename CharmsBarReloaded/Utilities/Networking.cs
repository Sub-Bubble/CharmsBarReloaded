using Microsoft.Win32;
using NativeWifi;
using NETWORKLIST;
using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static NativeWifi.Wlan;

namespace CharmsBarReloaded
{
    public static class Networking
    {
        /// airplane mode
        static RegistryKey airplaneModeRegistry = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\Control\RadioManagement\SystemRadioState");
        private static bool airplaneModeOn() => Convert.ToBoolean(airplaneModeRegistry.GetValue("", "")); //returns true if airplane mode is on

        private static string getConnectionType()
        {
            switch (getActiveNetworkInterface())
            {
                case NetworkInterfaceType.Ethernet:
                    return "Ethernet";
                case NetworkInterfaceType.Wireless80211:
                    return "Wifi";
                default:
                    return "Unknown";

            }
        }

        /// active network
        private static NetworkInterfaceType getActiveNetworkInterface()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                if (networkInterface.OperationalStatus == OperationalStatus.Up && !(networkInterface.Description.ToLower().Contains("virtual") || networkInterface.Name.ToLower().Contains("virtual")))
                    return networkInterface.NetworkInterfaceType;
            return NetworkInterfaceType.Unknown;
        }

        /// connection to the internet
        static INetworkListManager nlm = new NetworkListManager();
        static string getNetworkConnectivityStatus()
        {
            if (nlm.IsConnectedToInternet) return "Internet";
            else return "NoInternet";
        }

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
                MessageBox.Show($"Either you have no WiFi module or my code has errored out. Error code: {ex.Message}");
                return string.Empty;
            }
        }

        /// actual endpoint
        public static string NetworkStatus() 
        {
            if (airplaneModeOn()) return "Airplane";
            if (getConnectionType() == "Unknown") return "NoInternet";
            if (getConnectionType() == "Wifi")
            {
                return $"Wifi{getNetworkConnectivityStatus()}{GetWifiLinkQuality()}";
            }
            return $"{getConnectionType()}{getNetworkConnectivityStatus()}";
        }
    }
}
