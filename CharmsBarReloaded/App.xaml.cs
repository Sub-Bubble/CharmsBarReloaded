using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CharmsBarReloaded
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An error just happened. {e.Exception.Message}", "ErrorMessage", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            int count = Process.GetProcesses().Where(current => current.ProcessName == Process.GetCurrentProcess().ProcessName).Count();
            if (count > 1)
            {
                MessageBox.Show( "You're already running this app! Hover on top right corner to open Charms Bar!", "Already running!", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            TaskbarIcon? tray = (TaskbarIcon)FindResource("TaskbarIcon");
        }
    }
}
