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



        string bgColorError;
        string textColorError;
        string hoverColorError;
        public Customization()
        {

            InitializeComponent();

            bgColorPreview.Background = GlobalConfig.GetConfig("bg");
            bgColorTextbox.Text = GlobalConfig.BackgroundColor;
            
            textColorPreview.Background = GlobalConfig.GetConfig("text");
            textColorTextbox.Text = GlobalConfig.TextColor;
            
            hoverColorPreview.Background = GlobalConfig.GetConfig("hover");
            hoverColorTextbox.Text = GlobalConfig.HoverColor;

            EnableAnimations.IsChecked = GlobalConfig.EnableAnimations;


            timer.Interval = 1;
            timer.Elapsed += delegate
            {
                this.Dispatcher.Invoke(new Action(delegate
                {
                    // doesnt one dare to touch something that just works.
                    // especially when the cost is really low.
                    bgColorTextbox.Text = Regex.Replace(bgColorTextbox.Text, "[^0-9A-Fa-f]", "");
                    if (bgColorError != bgColorTextbox.Text)
                        try
                        {
                            bgColorPreview.Background = GlobalConfig.GetConfig("", bgColorTextbox.Text);
                            if (GlobalConfig.BackgroundColor.ToUpper() != bgColorTextbox.Text.ToUpper())
                            {
                                GlobalConfig.BackgroundColor = bgColorTextbox.Text;
                                GlobalConfig.SaveConfig();
                            }
                        }
                        catch { bgColorError = bgColorTextbox.Text; }



                    textColorTextbox.Text = Regex.Replace(textColorTextbox.Text, "[^0-9A-Fa-f]", "");
                    if (textColorError != textColorTextbox.Text)
                        try
                        {
                            textColorPreview.Background = GlobalConfig.GetConfig("", textColorTextbox.Text);
                            if (GlobalConfig.TextColor.ToUpper() != textColorTextbox.Text.ToUpper())
                            {
                                GlobalConfig.TextColor = textColorTextbox.Text;
                                GlobalConfig.SaveConfig();
                            }
                        }
                        catch { textColorError = textColorTextbox.Text; }



                    hoverColorTextbox.Text = Regex.Replace(hoverColorTextbox.Text, "[^0-9A-Fa-f]", "");
                    if (hoverColorError != hoverColorTextbox.Text)
                        try
                        {
                            hoverColorPreview.Background = GlobalConfig.GetConfig("", hoverColorTextbox.Text);
                            if (GlobalConfig.HoverColor.ToUpper() != hoverColorTextbox.Text.ToUpper())
                            {
                                GlobalConfig.HoverColor = hoverColorTextbox.Text;
                                GlobalConfig.SaveConfig();
                            }
                        }
                        catch { hoverColorError = hoverColorTextbox.Text; }

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
            GlobalConfig.BackgroundColor = "000000";
            bgColorTextbox.Text = GlobalConfig.BackgroundColor;
            GlobalConfig.TextColor = "d3d3d3";
            textColorTextbox.Text = GlobalConfig.TextColor;
            GlobalConfig.HoverColor = "4c4c4c";
            hoverColorTextbox.Text = GlobalConfig.HoverColor;
            GlobalConfig.SaveConfig();
        }

        private void EnableAnimations_Click(object sender, RoutedEventArgs e)
        {
            GlobalConfig.EnableAnimations = (bool)EnableAnimations.IsChecked;
        }
    }
}
