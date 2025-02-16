using System.Net.Http.Headers;
using System.Text.Json;


namespace CharmsBarReloaded.Updater
{
    public class RemoteServer
    {
        private static string remoteUrl = @""; //will be released later
        public static async Task CheckForUpdates(bool includeBetas, bool useCustomUrl, string CustomUrl)
        {
            if (!InstallDetector.IsInstalled())
            {
                MessageBox.Show($"{Program.AppName} is not installed, cannot check for updates!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<UpdateItem> updates = new List<UpdateItem>();
            string updatesList = string.Empty;
            try
            {
                updatesList = await FetchUpdates(useCustomUrl, CustomUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to update server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(updatesList))
            {
                MessageBox.Show("Failed to fetch updates!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            updates = JsonSerializer.Deserialize<List<UpdateItem>>(updatesList, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            updates.Sort((a, b) => b.build.CompareTo(a.build));

            bool canUpdate = false;
            foreach (var update in updates)
            {
                if (includeBetas && (update.build > InstallDetector.BuildNumber))
                {
                    MessageBox.Show($"A new update is available!\n[{InstallDetector.VersionString}] -> [{update.versionName}]\nDo you want to update?"
                        , $"{ (update.isBeta ? "[BETA] " : "") }Update available!", MessageBoxButtons.YesNo);
                    canUpdate = true;
                    break;
                }
                else if (!includeBetas && !update.isBeta && (update.build > InstallDetector.BuildNumber))
                {
                    MessageBox.Show($"A new update is available!\n[{InstallDetector.VersionString}] -> [{update.versionName}]\nDo you want to update?"
                        , "Update available!", MessageBoxButtons.YesNo);
                    canUpdate = true;
                    break;
                }
            }
            if (!canUpdate)
                MessageBox.Show($"You are running the latest version of {Program.AppName}", "No updates available", MessageBoxButtons.OK);
        }
        public static async Task<string> FetchUpdates(bool isCustomUrl, string customUrl = "")
        {
            if (isCustomUrl)
                remoteUrl = customUrl;
            if (!isCustomUrl)
            {
                MessageBox.Show("Update list server is not up yet! Stay tuned for later updates.");
                return null;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(remoteUrl);
                client.DefaultRequestHeaders.Add($"{Program.AppName}-Updater", "v1.0");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                return await FindRemoteJson(client, remoteUrl);

            }
        }
        private static async Task<string> FindRemoteJson(HttpClient client, string url)
        {
            if (url.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                return await GetRemoteJson(client, url);
            else
            {
                try
                {
                    string json = await GetRemoteJson(client, url);
                    if (!string.IsNullOrWhiteSpace(json))
                        return json;
                }
                catch { }
                return await GetRemoteJson(client, $"{url}/updates.json");
            }
        }
        private static async Task<string> GetRemoteJson(HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
