using CharmsBarReloaded.Settings;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CharmsBarReloaded
{
    class ClickHandler
    {
        #region keyboard keys constants
        const byte keyWin = 0x5B;
        const byte keyS = 0x53;
        const byte keyK = 0x4B;
        const byte keyI = 0x49;
        const byte keyControl = 0x11;
        const byte keyEscape = 0x1B;
        const byte keyPrntScr = 0x2C;
        const uint Key_Unpress = 0x02;
        #endregion keyboard keys constants

        #region keyboard simulation
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
           UIntPtr dwExtraInfo);
        #endregion keyboard simulation
        SettingsBeta oldSettings = new SettingsBeta();
        static CharmsSettings settings = new CharmsSettings();
        int button_id;
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
                    keybd_event(keyWin, 0, 0, UIntPtr.Zero);
                    keybd_event(keyS, 0, 0, UIntPtr.Zero);
                    keybd_event(keyS, 0, Key_Unpress, UIntPtr.Zero);
                    keybd_event(keyWin, 0, Key_Unpress, UIntPtr.Zero);
                    break;
                case "Share":
                    keybd_event(keyPrntScr, 0, 0, UIntPtr.Zero);
                    keybd_event(keyPrntScr, 0, Key_Unpress, UIntPtr.Zero);
                    break;
                case "Start":
                    keybd_event(keyControl, 0, 0, UIntPtr.Zero);
                    keybd_event(keyEscape, 0, 0, UIntPtr.Zero);
                    keybd_event(keyControl, 0, Key_Unpress, UIntPtr.Zero);
                    keybd_event(keyEscape, 0, Key_Unpress, UIntPtr.Zero);
                    break;
                case "Devices":
                    keybd_event(keyWin, 0, 0, UIntPtr.Zero);
                    keybd_event(keyK, 0, 0, UIntPtr.Zero);
                    keybd_event(keyK, 0, Key_Unpress, UIntPtr.Zero);
                    keybd_event(keyWin, 0, Key_Unpress, UIntPtr.Zero);
                    break;
                case "Settings":
                    clickHandler.OpenSettings();
                    break;
                case "OldSettings":
                    clickHandler.OpenOldSettings();
                    break;
                case "OsSettings":
                    keybd_event(keyWin, 0, 0, UIntPtr.Zero);
                    keybd_event(keyI, 0, 0, UIntPtr.Zero);
                    keybd_event(keyI, 0, Key_Unpress, UIntPtr.Zero);
                    keybd_event(keyWin, 0, Key_Unpress, UIntPtr.Zero);
                    break;
                case "ControlPanel":
                    Process.Start("control.exe");
                    break;
            }
        }
        public void OpenSettings()
        {
            settings.Show();
            settings.Deactivated += (sender, args) => { settings.HideWindow(); };
            settings.Focus();
            settings.Animate();
        }
        public void OpenOldSettings()
        {
            oldSettings.Show();
            oldSettings.Focus();
        }
        public static void SwitchSettingsPage(int pageId = 0)
        {
            switch (pageId)
            {
                case 0:
                    settings.SettingsFrame.Content = new SettingsHome();
                    break;
                case 1:
                    settings.SettingsFrame.Content = new General();
                    break;
                case 2:
                    settings.SettingsFrame.Content = new Customization();
                    break;
                case 3:
                    settings.SettingsFrame.Content = new About();
                    break;
            }
        }
    }
}
