using CharmsBarReloaded.Config;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CharmsBarReloaded.CharmsBar
{
    public partial class CharmsBar
    {
        private void SetupButtons()
        {
            Grid[] buttons = new Grid[App.charmsConfig.charmsBarConfig.ButtonActions.Length + 2];
            #region filler
            buttons[0] = new Grid();
            buttons[0].Children.Add(new TextBlock { Height = 100, Text = "", HorizontalAlignment = HorizontalAlignment.Stretch });
            #endregion filler
            for (int i = 1; i < App.charmsConfig.charmsBarConfig.ButtonActions.Length + 2; i++)
            {
                TextBlock hitboxFiller = new TextBlock { Height = 100, HorizontalAlignment = HorizontalAlignment.Stretch, Text = "" };
                #region filler
                if (i == App.charmsConfig.charmsBarConfig.ButtonActions.Length + 1)
                {
                    buttons[i] = new Grid();
                    buttons[i].Children.Add(hitboxFiller);
                    break;
                }
                #endregion filler
                if (!CharmsConfig.CharmsBarConfig.ValidActions.Contains(App.charmsConfig.charmsBarConfig.ButtonActions[i - 1]))
                {
                    MessageBox.Show("Invalid Config!");
                    Log.Error($"Invalid action: {App.charmsConfig.charmsBarConfig.ButtonActions[i - 1]}");
                    throw new InvalidDataException($"Invalid action: {App.charmsConfig.charmsBarConfig.ButtonActions[i - 1]}");
                }

                string action = App.charmsConfig.charmsBarConfig.ButtonActions[i - 1];

                buttons[i] = new Grid { Height = 100, RenderTransform = new TranslateTransform() };
                buttons[i].MouseDown += (o, e) => { App.ClickHandler(action); if (App.charmsConfig.charmsBarConfig.HideWindowAfterClick) HideWindow(); };
                buttons[i].Opacity = 0.01;
                buttons[i].Style = new Style
                {
                    TargetType = typeof(Grid),
                    Triggers = { new Trigger { Property = IsMouseOverProperty, Value = true,
                        Setters = { new Setter { Property = BackgroundProperty, Value = GetBrush.GetBrushFromHex(App.charmsConfig.charmsBarConfig.HoverColor) }}}}
                };
                buttons[i].Children.Add(hitboxFiller);

                //image source
                BitmapImage image = new BitmapImage(new Uri($"pack://application:,,,/Assets/CharmsBar/{action}.png"));
                if (App.charmsConfig.charmsBarConfig.UsesDynamicColor[i - 1])
                    buttons[i].Children.Add(new Grid
                    {
                        Height = 48,
                        Width = 48,
                        Margin = new Thickness(0, 18.01, 0, 0),
                        VerticalAlignment = VerticalAlignment.Top,
                        OpacityMask = new ImageBrush(image),
                        Background = SystemConfig.GetAccentColor
                    });
                else
                    buttons[i].Children.Add(new Image
                    {
                        Height = 48,
                        Margin = new Thickness(0, 18, 0, 0),
                        VerticalAlignment = VerticalAlignment.Top,
                        Source = image
                    });
                buttons[i].Children.Add(new Label
                {
                    Foreground = GetBrush.GetBrushFromHex(App.charmsConfig.charmsBarConfig.TextColor),
                    Height = 26,
                    Content = $"{App.translationManager.GetTranslation($"CharmsBar.{action}")}",
                    Margin = new Thickness(0, 0, 0, 11),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                Log.Info($"Loading button: {action}");
            }
            charmsStack.Children.Clear();
            foreach (var button in buttons)
            {
                charmsStack.Children.Add(button);
            }
            charmsStack.InvalidateMeasure();
            charmsStack.UpdateLayout();
            windowWidth = (int)charmsStack.ActualWidth;
        }
    }
}
