using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;

namespace SocialLadder.Droid.Converters
{
    public class ColorValueConverter : MvxValueConverter<MvxColor, ColorDrawable>
    {
        protected override ColorDrawable Convert(MvxColor value, Type targetType, object parameter, CultureInfo culture)
        {
            //Color color = Color.Rgb(value.R, value.G, value.B);
            //return color;
            Color color = Color.Rgb(value.R, value.G, value.B);
            ColorDrawable draw = new ColorDrawable(color);
            return draw;
        }
    }
}