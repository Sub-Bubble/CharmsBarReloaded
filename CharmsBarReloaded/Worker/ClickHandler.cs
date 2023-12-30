using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CharmsBarReloaded.Worker
{
    class ClickHandler
    {
        private SettingsBeta settings = new SettingsBeta();
        private int button_id;
        public ClickHandler(int clicked_id)
        {
            button_id = clicked_id;

        }
        public static void Do(int button_id)
        {
            var clickHandler = new ClickHandler(button_id);
            switch (ButtonConfig.GetButtonConfig(button_id))
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
                    clickHandler.OpenSettings();
                    break;
            }
        }
        public void OpenSettings()
        {
            settings.Show();
            settings.Focus();
        }
    }
}
