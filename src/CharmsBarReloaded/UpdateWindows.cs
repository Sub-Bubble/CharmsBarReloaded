namespace CharmsBarReloaded
{
    public partial class App
    {
        private int reloadCooldown = 0;
        private void CharmsBarReloaded_Update(object? sender, System.Timers.ElapsedEventArgs e)
        {
            charmsBar.Dispatcher.Invoke(() => { charmsBar.CharmsBar_Update(ref charmsClock); });
            charmsClock.Dispatcher.Invoke(() => { charmsClock.Update(); });
            if (reloadCooldown == 20)
            {
                settingsHome.Dispatcher.Invoke(() => { settingsHome.Home_Reload(); });
                reloadCooldown = 0;
            }
            reloadCooldown++;
        }
        private void CharmsBarReloaded_WifiUpdate(object? sender, System.Timers.ElapsedEventArgs e)
        {
            charmsClock.Dispatcher.Invoke(charmsClock.UpdateInternetStatus);
            settingsHome.Dispatcher.Invoke(settingsHome.UpdateInternetStatus);
        }
    }
}
