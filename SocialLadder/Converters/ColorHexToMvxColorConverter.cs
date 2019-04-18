using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Converters
{
    public class ColorHexToMvxColorConverter : MvxValueConverter<string, MvxColor>
    {
        protected override MvxColor Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            string hex = value;
            int r = System.Convert.ToInt32(hex.Substring(1, 2), 16);
            int g = System.Convert.ToInt32(hex.Substring(3, 2), 16);
            int b = System.Convert.ToInt32(hex.Substring(5, 2), 16);
            var mvxColor = new MvxColor(r, g, b);
            return mvxColor;
        }
    }
}
