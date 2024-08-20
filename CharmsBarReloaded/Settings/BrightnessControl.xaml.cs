using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Formats.Asn1.AsnWriter;

namespace CharmsBarReloaded.Settings
{
    /// <summary>
    /// Interaction logic for BrightnessControl.xaml
    /// </summary>
    public partial class BrightnessControl : Window
    {
        #region hiding window from alttab
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;
        #endregion hiding window from alttab

        ManagementObjectCollection managementObjCollection;
        public BrightnessControl()
        {
            InitializeComponent();
            this.Loaded += delegate
            {
                SetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE, (GetWindowLong(new WindowInteropHelper(this).Handle, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            };
            try
            {
                managementObjCollection = new ManagementObjectSearcher(new ManagementScope("root\\WMI"), new SelectQuery("WmiMonitorBrightnessMethods")).Get();
            }
            catch { }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            brightnessSlider.Value = Math.Round(brightnessSlider.Value, 0);
            brightnessText.Text = brightnessSlider.Value.ToString();
            try
            {
                foreach (ManagementObject mObj in managementObjCollection)
                {
                    mObj.InvokeMethod("WmiSetBrightness",
                        new Object[] { UInt32.MaxValue, (byte)brightnessSlider.Value });
                    break;
                }
            }
            catch {}
        }
    }
}
