using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Reflection.Metadata;
using System.Net.NetworkInformation;
using Microsoft.Win32;

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

        private static NetworkInterfaceType getActiveNetworkInterface()
        {
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                if (networkInterface.OperationalStatus == OperationalStatus.Up && !(networkInterface.Description.ToLower().Contains("virtual") || networkInterface.Name.ToLower().Contains("virtual")))
                    return networkInterface.NetworkInterfaceType;
            return NetworkInterfaceType.Unknown;
        }

        /*static bool GetNetworkConnectivityStatus()
        {
            //will add code in future
        }*/

        public static string NetworkStatus()
        {
            if (!airplaneModeOn())
                return $"{getConnectionType()}";
            return "Airplane";
        }
    }
}
