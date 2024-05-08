using Microsoft.Win32;
//using NativeWifi;
using NETWORKLIST;
using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;
//using static NativeWifi.Wlan;

namespace CharmsBarReloaded
{
    public static class Networking
    {
        /// airplane mode
        static RegistryKey airplaneModeRegistry = Registry.LocalMachine.OpenSubKey(@"SYSTEM\ControlSet001\Control\RadioManagement\SystemRadioState");
        private static bool airplaneModeOn() => Convert.ToBoolean(airplaneModeRegistry.GetValue("", "")); //returns true if airplane mode is on
        static NetworkInterfaceType connectionType;

        private static string getConnectionType()
        {
            if (!GlobalConfig.UseNetworkCaching || connectionType == null)
                connectionType = getActiveNetworkInterface();
            switch (connectionType)
            {
                case NetworkInterfaceType.Ethernet:
                    return "Ethernet";
                case NetworkInterfaceType.Wireless80211:
                    return "Wifi";
                default:
                    return "Unknown";

            }
        }
        public static void UpdateNetworkCache()
        {
            connectionType = getActiveNetworkInterface();
        }

        /// active network
        private static NetworkInterfaceType getActiveNetworkInterface()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                if (networkInterface.OperationalStatus == OperationalStatus.Up && !(networkInterface.Description.ToLower().Contains("virtual") || networkInterface.Name.ToLower().Contains("virtual")
                    && networkInterface.Description.ToLower().Contains("VPN") || networkInterface.Name.ToLower().Contains("VPN")))
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

        /// actual endpoint
        public static string NetworkStatus() 
        {
            if (airplaneModeOn()) return "Airplane";
            if (getConnectionType() == "Unknown") return "NoInternet";
            if (getConnectionType() == "Wifi")
            {
                return $"Wifi{getNetworkConnectivityStatus()}{Utilities.WifiStatus.GetWifiLinkQuality()}";
            }
            return $"{getConnectionType()}{getNetworkConnectivityStatus()}";
        }
    }
}
