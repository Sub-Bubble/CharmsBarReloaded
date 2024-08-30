using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CharmsBarReloaded.CharmsSettings.Pages
{
    /// <summary>
    /// Interaction logic for Personalization.xaml
    /// </summary>
    public partial class Personalization : Page
    {
        public Personalization()
        {
            InitializeComponent();

            personalizationTitle.Text = App.translationManager.GetTranslation("CharmsSettings.Home.Personalization");
        }
        #region back button
        private void BackButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButtonClicked.png", UriKind.Relative));
        }
        private void BackButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
            App.ClickHandler("SettingsHome");
        }
        private void BackButton_MouseEnter(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButtonMouseOver.png", UriKind.Relative));
        }
        private void BackButton_MouseLeave(object sender, MouseEventArgs e)
        {
            BackButton.Source = new BitmapImage(new Uri(@"../../Assets/CharmsSettings/BackButton.png", UriKind.Relative));
        }
        #endregion back button
    }
}
