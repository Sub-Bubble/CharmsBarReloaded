using System.Windows.Media.Animation;

namespace CharmsBarReloaded.CharmsClock
{
    public partial class CharmsClock
    {
        public DoubleAnimation fadeIn = new DoubleAnimation
        {
            From = 0.0,
            To = 1.0,
            Duration = TimeSpan.FromMilliseconds(100)
        };
        public DoubleAnimation fadeOut = new DoubleAnimation
        {
            From = 1.0,
            To = 0.0,
            Duration = TimeSpan.FromMilliseconds(100)
        };
        public DoubleAnimation noAnimationIn = new DoubleAnimation
        {
            From = 0.0,
            To = 1.0,
            Duration = TimeSpan.Zero
        };
        public DoubleAnimation noAnimationOut = new DoubleAnimation
        {
            From = 1.0,
            To = 0.0,
            Duration = TimeSpan.Zero
        };
    }
}
