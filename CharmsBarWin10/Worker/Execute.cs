using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CharmsBarWin10.Worker
{
    class Execute
    {
        const byte keyWin = 0x5B;
        const byte keyS = 0x53;
        const byte keyK = 0x4B;
        const byte keyControl = 0x11;
        const byte keyEscape = 0x1B;
        const byte keyPrntScr = 0x2C;
        const uint Key_Unpress = 0x02;
        public static void ShowStartMenu()
        {
            keybd_event(keyControl, 0, 0, UIntPtr.Zero);
            keybd_event(keyEscape, 0, 0, UIntPtr.Zero);
            keybd_event(keyControl, 0, Key_Unpress, UIntPtr.Zero);
            keybd_event(keyEscape, 0, Key_Unpress, UIntPtr.Zero);
        }
        public static void ShowSearch()
        {
            keybd_event(keyWin, 0, 0, UIntPtr.Zero);
            keybd_event(keyS, 0, 0, UIntPtr.Zero);
            keybd_event(keyS, 0, Key_Unpress, UIntPtr.Zero);
            keybd_event(keyWin, 0, Key_Unpress, UIntPtr.Zero);
        }
        public static void Share()
        {
            keybd_event(keyPrntScr, 0, 0, UIntPtr.Zero);
            keybd_event(keyPrntScr, 0, Key_Unpress, UIntPtr.Zero);
        }
        public static void ShowDevices()
        {
            keybd_event(keyWin, 0, 0, UIntPtr.Zero);
            keybd_event(keyK, 0, 0, UIntPtr.Zero);
            keybd_event(keyK, 0, Key_Unpress, UIntPtr.Zero);
            keybd_event(keyWin, 0, Key_Unpress, UIntPtr.Zero);
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags,
           UIntPtr dwExtraInfo);
    }
}
