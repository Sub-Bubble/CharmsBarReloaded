using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Reflection.Metadata;

namespace CharmsBarReloaded
{
    public static class GetMouseLocation
    { 
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point { public Int32 X; public Int32 Y; };
        public static System.Windows.Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new System.Windows.Point(w32Mouse.X, w32Mouse.Y);
        }
    }
}
