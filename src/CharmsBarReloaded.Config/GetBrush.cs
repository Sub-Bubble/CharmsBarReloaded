using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace CharmsBarReloaded.Config
{
    //suppressing as there will be error handling in other classes
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
    public static class GetBrush
    {
        private static BrushConverter converter = new BrushConverter();
        public static Brush GetBrushFromHex(string hex)
        {
            return (Brush)converter.ConvertFrom($"#FF{hex.ToUpper()}");
        }
        public static Brush GetSpecialBrush(string id)
        {
            switch (id)
            {
                case "Hide":
                    return (Brush)converter.ConvertFrom("#00000000");
                case "Transparent":
                    return (Brush)converter.ConvertFrom("#01000000");
                case "White":
                    return (Brush)converter.ConvertFrom("#00000000");
                default:
                    return (Brush)converter.ConvertFrom("#FF000000");
            }
        }
    }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
}