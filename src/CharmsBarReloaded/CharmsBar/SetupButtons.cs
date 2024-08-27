using CharmsBarReloaded.Config;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace CharmsBarReloaded.CharmsBar
{
    public partial class CharmsBar
    {
        private void SetupButtons(CharmsConfig.CharmsBarConfig charmsBarConfig, TranslationManager translation)
        {
            Grid[] buttons = new Grid[charmsBarConfig.ButtonsAmount+2];
            #region filler
            buttons[0] = new Grid();
            buttons[0].Children.Add(new TextBlock { Height = 100, Text = "", HorizontalAlignment = HorizontalAlignment.Stretch });
            #endregion filler
            for (int i = 1; i < charmsBarConfig.ButtonsAmount+2; i++)
            {
                TextBlock hitboxFiller = new TextBlock { Height = 100, HorizontalAlignment = HorizontalAlignment.Stretch, Text = "" };
                #region filler
                if (i == charmsBarConfig.ButtonsAmount + 1)
                {
                    buttons[i] = new Grid();
                    buttons[i].Children.Add(hitboxFiller);
                    break;
                }
                #endregion filler
                if (!CharmsConfig.CharmsBarConfig.ValidActions.Contains(charmsBarConfig.ButtonActions[i - 1]))
                {
                    MessageBox.Show("Invalid Config!");
                    Log.Error($"Invalid action: {charmsBarConfig.ButtonActions[i - 1]}");
                    throw new InvalidDataException($"Invalid action: {charmsBarConfig.ButtonActions[i - 1]}");
                }

                string action = charmsBarConfig.ButtonActions[i - 1];

                buttons[i] = new Grid { Height = 100,/* Width = 86, */RenderTransform = new TranslateTransform() };
                buttons[i].MouseDown += (o, e) => { App.ClickHandler(action); if (charmsBarConfig.HideWindowAfterClick) HideWindow(); };
                buttons[i].Opacity = 0.01;
                buttons[i].Style = new Style
                {
                    TargetType = typeof(Grid),
                    Triggers = { new Trigger { Property = IsMouseOverProperty, Value = true,
                        Setters = { new Setter { Property = BackgroundProperty, Value = GetBrush.GetBrushFromHex(charmsBarConfig.HoverColor) }}}}
                };
                buttons[i].Children.Add(hitboxFiller);

                //image source
                BitmapImage image = new BitmapImage(new Uri($"pack://application:,,,/Assets/CharmsBar/{action}.png"));
                 if (action == "Start" /*&& {here should be theme parameter that decides if start button will use accent color}*/)
                    buttons[i].Children.Add(new Grid
                    {
                        Height = 48, Width = 48,
                        Margin = new Thickness(0, 18.01, 0, 0), VerticalAlignment = VerticalAlignment.Top,
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
                    Foreground = GetBrush.GetBrushFromHex(charmsBarConfig.TextColor),
                    Height = 26,
                    Content = $"{translation.GetTranslation($"CharmsBar.{action}")}",
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
