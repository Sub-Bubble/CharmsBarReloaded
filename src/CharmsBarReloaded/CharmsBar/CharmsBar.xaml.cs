﻿using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.CharmsBar
{
    /// <summary>
    /// Interaction logic for CharmsBar.xaml
    /// </summary>
    public partial class CharmsBar : Window
    {
        public CharmsBar()
        {
            InitializeComponent();
        }
        public void HideWindow()
        {
            BeginAnimation(UIElement.OpacityProperty, fadeOut);
            foreach (Grid grid in charmsStack.Children)
            {
                foreach (var item in grid.Children)
                {
                    if (item.GetType() == typeof(Image))
                    {
                        string source = ((Image)item).Source.ToString();
                        if (!source.Contains("Preview")) ((Image)item).Source = new BitmapImage(new Uri(source.Insert(source.LastIndexOf(".png"), "Preview")));
                    }
                    if (item.GetType() == typeof(Label))
                        ((Label)item).Visibility = Visibility.Collapsed;
                    if (item.GetType() == typeof(Grid))
                        ((Grid)item).Background = GetBrush.GetSpecialBrush("White");
                }
            }
        }
        public bool windowVisible;
        public bool isAnimating = false;
        public int windowWidth;
        public void Window_Reload()
        {
            InitializeAnimations();
            SetupButtons();

            Log.Info("Loaded Charms Bar successfully!");
            this.Background = GetBrush.GetBrushFromHex(App.charmsConfig.charmsBarConfig.BackgroundColor);
            PrepareButtons(App.charmsConfig.EnableAnimations);
        }
        private void FadeOut_Completed(object? sender, EventArgs e)
        {
            PrepareButtons(false);
            isAnimating = false;
            windowVisible = false;
            this.Background = GetBrush.GetSpecialBrush("Hide");
            charmsStack.Visibility = Visibility.Collapsed;
            BeginAnimation(OpacityProperty, backTo1Opacity);
        }
    }
}
