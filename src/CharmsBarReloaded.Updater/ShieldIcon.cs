using System.Runtime.InteropServices;

namespace CharmsBarReloaded.Updater
{
    public static class ShieldIcon
    {
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        public static void AddToButton(Button button)
        {
            button.FlatStyle = FlatStyle.System;
            SendMessage(button.Handle, 0x160C, 0, 1);
        }
        public static void RemoveFromButton(Button button)
        {
            button.FlatStyle = FlatStyle.System;
            SendMessage(button.Handle, 0x160C, 0, 0);
        }
    }
}
