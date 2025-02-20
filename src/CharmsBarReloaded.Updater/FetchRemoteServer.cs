using System.Text.Json;

namespace CharmsBarReloaded.Updater
{
    public partial class UpdaterForm
    {
        private async void FetchRemoteUpdates(bool isCustomUrl)
        {
            installButton.Enabled = false;
            cancelButton.Enabled = false;
            UpdateStatus("Connecting to a remote server", "info", false);
            string remoteUrl = @""; //will be released later
            if (isCustomUrl)
                remoteUrl = customServerTextBox.Text;
            if (!isCustomUrl)
            {
                MessageBox.Show("Update list server is not up yet! Stay tuned for later updates.");
                return;
            }

            try
            {
                UpdateStatus("Connecting to a remote server", "info", false);

                var updatesList = await RemoteServer.FetchUpdates(isCustomUrl, customServerTextBox.Text);

                if (string.IsNullOrWhiteSpace(updatesList))
                {
                    UpdateStatus("Failed to fetch updates!", "error", false);
                    return;
                }

                updates = JsonSerializer.Deserialize<List<UpdateItem>>(updatesList, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                updates.Sort((a, b) => b.build.CompareTo(a.build));
                versionSelector.Items.Clear();
                RepopulateUpdatesList();
            }
            catch (UriFormatException)
            {
                UpdateStatus("Failed to connect to a server", "error", false);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == null)
            {
                UpdateStatus("Failed to connect to a server", "error", false);
            }
            catch
            {
                UpdateStatus("Failed to fetch updates!", "error", false);
            }
        }
    }
}
