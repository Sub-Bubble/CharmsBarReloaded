using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Reflection.Metadata;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using NETWORKLIST;
using NativeWifi;
using static NativeWifi.Wlan;
using static NativeWifi.WlanClient;

namespace CharmsBarReloaded
{
    public static class GetMouseLocation
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point { public Int32 X; public Int32 Y; };
        public static System.Windows.Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new System.Windows.Point(w32Mouse.X, w32Mouse.Y);
        }
    }
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
            Wlan.WlanBssEntry[] wlanBssEntries = wlanClient.Interfaces[0].GetNetworkBssList();
            //MessageBox.Show($"Number of wlan clients: {wlanClient.Interfaces.Length}"); //use for debugging
            WlanBssEntry wifiEntry = wlanBssEntries[0];
            int linkQuality = Convert.ToInt32(wifiEntry.linkQuality)/20 + 1;
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
                case 5: case 6:
                    return "Max";
                default:
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
            return $"{getConnectionType()}{getNetworkConnectivityStatus().ToString()}";
        }
    }
}
