using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;

namespace SocialLadder.Droid.Converters
{
    public class ColorHexToDrawableValueConverter : MvxValueConverter<string, ColorDrawable>
    {
        protected override ColorDrawable Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            string hex = value;
            int r = System.Convert.ToInt32(hex.Substring(1, 2), 16);
            int g = System.Convert.ToInt32(hex.Substring(3, 2), 16);
            int b = System.Convert.ToInt32(hex.Substring(5, 2), 16);
            Android.Graphics.Color color = Android.Graphics.Color.Rgb(r, g, b);
            ColorDrawable draw = new ColorDrawable(color);
            return draw;
        }
    }
}