using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace CharmsBarReloaded.CharmsBar
{
    public partial class CharmsBar
    {
        DoubleAnimation fadeOut = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromMilliseconds(100)
        };
        DoubleAnimation backTo1Opacity = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.Zero
        };
        public Storyboard fadeIn = new Storyboard();
        private void InitializeAnimations()
        {
            TimeSpan timeSpan = TimeSpan.Zero;
            if (App.charmsConfig.EnableAnimations) timeSpan = TimeSpan.FromMilliseconds(100);
            fadeIn.Children.Clear(); //just in case
            fadeIn.Children.Add(new ColorAnimation
            {
                To = (Color)ColorConverter.ConvertFromString($"#FF{App.charmsConfig.charmsBarConfig.BackgroundColor}"),
                Duration = timeSpan
            });
            Storyboard.SetTargetProperty(fadeIn, new PropertyPath("(Window.Background).(SolidColorBrush.Color)"));
            fadeIn.Completed += (sender, e) => { isAnimating = false; };
            fadeOut.Completed += FadeOut_Completed;
        }
        public void PrepareButtons(bool doSlideIn = true)
        {
            isAnimating = true;
            var storyboard = new Storyboard();
            int numberOfGrids = charmsStack.Children.Count;

            for (int i = 0; i < numberOfGrids; i++)
            {
               var grid = charmsStack.Children[i] as Grid;
               var slideInAnimation = new DoubleAnimation
                    {
                    From = 0,
                    To = charmsStack.ActualWidth,
                    Duration = TimeSpan.Zero,
                    BeginTime = TimeSpan.FromSeconds(i * 0.01),
               };
                Storyboard.SetTarget(slideInAnimation, grid);
                Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                storyboard.Children.Add(slideInAnimation);
                storyboard.Completed += delegate { grid.Opacity = 1.0; if (doSlideIn) SlideInButtons(); };
            }
            BeginStoryboard(storyboard);
        }

        public void SlideInButtons()
        {
            var storyboard = new Storyboard();
            var animationOrder = CalculateAnimationOrder(charmsStack.Children.Count);

            for (int i = 0; i < animationOrder.Count; i++)
            {
                var gridsToAnimate = animationOrder[i];
                foreach (var index in gridsToAnimate)
                {
                    var grid = charmsStack.Children[index] as Grid;
                    if (grid != null)
                    {
                        TimeSpan animationDuration;
                        if (App.charmsConfig.EnableAnimations)
                            animationDuration = TimeSpan.FromSeconds(0.5);
                        else
                            animationDuration = TimeSpan.Zero;
                        var slideInAnimation = new DoubleAnimation
                        {
                            From = charmsStack.ActualWidth,
                            To = 0,
                            Duration = animationDuration,
                            BeginTime = TimeSpan.FromSeconds(i * 0.05),
                            EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseOut }
                        };

                        Storyboard.SetTarget(slideInAnimation, grid);
                        Storyboard.SetTargetProperty(slideInAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                        storyboard.Children.Add(slideInAnimation);
                    }
                }
            }
            storyboard.Completed += (sender, e) => isAnimating = false;
            storyboard.Begin(this);
        }
        private List<List<int>> CalculateAnimationOrder(int buttonsCount)
        {
            var animationOrder = new List<List<int>>();
            int center = (buttonsCount - 1) / 2;

            for (int i = 0; i <= center; i++)
            {
                var step = new List<int>();
                if (buttonsCount % 2 != 0 && i == 0)
                {
                    step.Add(center);
                }
                else
                {
                    if (center - i > 0) step.Add(center - i);
                    if (center + i < buttonsCount) step.Add(center + i);
                }
                animationOrder.Add(step);
            }
            return animationOrder;
        }
    }
}
