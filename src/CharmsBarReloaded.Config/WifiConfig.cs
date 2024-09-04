using System.Net.NetworkInformation;
using System.Net.Sockets;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;

namespace CharmsBarReloaded.Config;

public class WifiConfig
{
    private record WiFiInformation
    {
        public required string AdapterName { get; set; }
        public required string Status { get; set; }
        public required WiFiNetwork ConnectedNetwork { get; set; }
        public required List<WiFiNetwork> AvailableNetworks { get; set; }
    }

    private record WiFiNetwork
    {
        public required string Ssid { get; set; }
        public required int SignalStrength { get; set; }
        public required NetworkSecuritySettings SecuritySettings { get; set; }
    }

    private static NetworkInterface? GetMainNetworkAdapter()
    {
        foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (adapter.OperationalStatus != OperationalStatus.Up) continue;
            if (!adapter.GetIPProperties().UnicastAddresses.Any(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork || ip.Address.AddressFamily == AddressFamily.InterNetworkV6)) continue;
            if (adapter.Description.Contains("VPN") || adapter.Description.Contains("Virtual") || adapter.Description.Contains("VM") || adapter.Description.Contains("Software") || adapter.Description.Contains("TAP-Windows") ) continue;

            return adapter;
        }

        return null;
    }

    private static async Task<WiFiInformation?> GetWiFiStatusAsync()
    {
        var wifiAdapters = await WiFiAdapter.FindAllAdaptersAsync();
        if (wifiAdapters.Count == 0) return null;

        var adapter = wifiAdapters[0];
        var connectedProfile = await adapter.NetworkAdapter.GetConnectedProfileAsync();
        var availableNetworks = adapter.NetworkReport.AvailableNetworks;
        WiFiNetwork? connectedNetwork = null;
        if (connectedProfile != null)
        {
            foreach (var network in availableNetworks)
            {
                if (network.Ssid != connectedProfile.ProfileName) continue;

                connectedNetwork = new WiFiNetwork
                {
                    Ssid = network.Ssid,
                    SignalStrength = network.SignalBars * 25,
                    SecuritySettings = network.SecuritySettings
                };

                break;
            }
        }

        var availableWiFiNetworks = new List<WiFiNetwork>();
        foreach (var network in availableNetworks)
        {
            availableWiFiNetworks.Add(new WiFiNetwork
            {
                Ssid = network.Ssid,
                SignalStrength = network.SignalBars * 25,
                SecuritySettings = network.SecuritySettings,
            });
        }

        return new WiFiInformation
        {
            AdapterName = adapter.NetworkAdapter.NetworkAdapterId.ToString(),
            Status = connectedProfile != null ? "Connected" : "Searching",
            ConnectedNetwork = connectedNetwork!,
            AvailableNetworks = availableWiFiNetworks,
        };
    }

    public async static Task<string> GetStatus()
    {
        var mainAdapter = GetMainNetworkAdapter();
        if (mainAdapter is null)
            return "NoInternet";

        if (mainAdapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
            return "EthernetInternet";

        var wifi = await GetWiFiStatusAsync();

        if (wifi is null) return "WifiNoInternet";
        if (wifi.Status != "Connected") return "WifiNoInternet";

        int strength = wifi.ConnectedNetwork.SignalStrength;
        if (strength <= 20) return "WifiInternetWeakest";
        if (strength <= 40) return "WifiInternetWeak";
        if (strength <= 60) return "WifiInternetMedium";
        if (strength <= 80) return "WifiInternetStrong";
        
        return "WifiInternetMax";
    }
}
