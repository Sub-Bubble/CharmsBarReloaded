using System.Net.Http.Headers;
using System.Text.Json;


namespace CharmsBarReloaded.Updater
{
    public class RemoteServer
    {
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
                updatesList = await FetchUpdates(CustomUrl);
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
                        , $"{(update.isBeta ? "[BETA] " : "")}Update available!", MessageBoxButtons.YesNo);
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
        public static async Task<string> FetchUpdates(string remoteUrl)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(remoteUrl);
            client.DefaultRequestHeaders.Add($"{Program.AppName}-Updater", "v1.0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return await FindRemoteJson(client, remoteUrl);
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

        public static CancellationTokenSource cancelToken = new CancellationTokenSource();
        public static async Task DownloadPackage(string downloadLink, string resultPath, IProgress<int> progress, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            using HttpResponseMessage httpResponse = await client.GetAsync(downloadLink, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            httpResponse.EnsureSuccessStatusCode();
            long? totalBytes = httpResponse.Content.Headers.ContentLength;

            using Stream contentStream = await httpResponse.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(resultPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true);

            byte[] buffer = new byte[8192];
            long totalRead = 0;
            int bytesRead;

            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await fileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                totalRead += bytesRead;

                if (totalBytes.HasValue)
                {
                    int percentage = (int)((totalRead * 100) / totalBytes.Value);
                    progress.Report(percentage);
                }
                else
                    progress.Report(-1);
            }
        }
    }
}
