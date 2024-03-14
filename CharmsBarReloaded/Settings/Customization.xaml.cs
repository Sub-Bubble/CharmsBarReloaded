using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.Settings
{
    /// <summary>
    /// Interaction logic for Customization.xaml
    /// </summary>
    public partial class Customization : Page
    {
        #region back button
        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/BackButtonClicked.png", UriKind.Relative));
        }
        private void BackButton_Click(object sender, MouseButtonEventArgs e)
        {
            ClickHandler.SwitchSettingsPage(0);
        }

        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/BackButtonMouseOver.png", UriKind.Relative));
        }

        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
        }
        #endregion back button
        System.Timers.Timer timer = new System.Timers.Timer();
        Regex regex = new Regex("^[0-9A-Fa-f]{1}$");
        public Customization()
        {

            InitializeComponent();

            bgColorPreview.Background = GlobalConfig.GetConfig("bg");
            bgColorTextbox.Text = GlobalConfig.BackgroundColor;
            
            textColorPreview.Background = GlobalConfig.GetConfig("text");
            textColorTextbox.Text = GlobalConfig.TextColor;
            
            hoverColorPreview.Background = GlobalConfig.GetConfig("hover");
            hoverColorTextbox.Text = GlobalConfig.HoverColor;


            timer.Interval = 1;
            timer.Elapsed += delegate
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    try
                    {
                        bgColorTextbox.Text = Regex.Replace(bgColorTextbox.Text, "[^0-9A-Fa-f]", "");
                        bgColorPreview.Background = GlobalConfig.GetConfig("", bgColorTextbox.Text);
                        GlobalConfig.BackgroundColor = bgColorTextbox.Text;
                    } catch {}
                    try
                    {
                        textColorTextbox.Text = Regex.Replace(textColorTextbox.Text, "[^0-9A-Fa-f]", "");
                        textColorPreview.Background = GlobalConfig.GetConfig("", textColorTextbox.Text);
                        GlobalConfig.TextColor = textColorTextbox.Text;
                    } catch { }
                    try
                    {
                        hoverColorTextbox.Text = Regex.Replace(hoverColorTextbox.Text, "[^0-9A-Fa-f]", "");
                        hoverColorPreview.Background = GlobalConfig.GetConfig("", hoverColorTextbox.Text);
                        GlobalConfig.HoverColor = hoverColorTextbox.Text;
                    } catch { }
                }));
            };
            timer.Start();
            showChargingOnDesktop.IsChecked = GlobalConfig.ShowChargingOnDesktop;
        }

        private void ShowChargingOnDesktop_Update(object sender, System.Windows.RoutedEventArgs e)
        {
            GlobalConfig.ShowChargingOnDesktop = (bool)showChargingOnDesktop.IsChecked;
            GlobalConfig.SaveConfig();
        }

        private void ValidHexCheck(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !regex.IsMatch(e.Text);
        }

        private void ResetColorConfig(object sender, RoutedEventArgs e)
        {

            bgColorTextbox.Text = "000000";
            textColorTextbox.Text = "d3d3d3";
            hoverColorTextbox.Text = "4c4c4c";
        }
    }
}
