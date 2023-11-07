using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CharmsBarWin10.Worker
{
    class ClickHandler
    {
        private int button_id;
        public ClickHandler(int clicked_id)
        {
            button_id = clicked_id;

        }
        public static void Do(int button_id)
        {
            switch (Config.ButtonConfig.GetButtonConfig(button_id))
            {
                case "Search":
                    Execute.ShowSearch();
                    break;
                case "Share":
                    Execute.Share();
                    break;
                case "Start":
                    Execute.ShowStartMenu();
                    break;
                case "Devices":
                    Execute.ShowDevices();
                    break;
                case "Settings":
                    MessageBox.Show("Settings coming soon!", "Coming Soon", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

    }
}
