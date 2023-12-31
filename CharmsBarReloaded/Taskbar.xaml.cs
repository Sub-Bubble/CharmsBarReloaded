﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CharmsBarReloaded
{
    public partial class Taskbar
    {
        internal void OpenSettings(object sender, RoutedEventArgs e)
        {
            Worker.ClickHandler.Do(-1);
        }
        internal void ExitApp(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ToggleCharmsBar(object sender, RoutedEventArgs e)
        {
            Checkmark.IsChecked = !GlobalConfig.IsEnabled;
            GlobalConfig.IsEnabled = !GlobalConfig.IsEnabled;
        }
    }
}
