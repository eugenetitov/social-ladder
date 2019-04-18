using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;

namespace SocialLadder.Droid.Converters
{
    public class StringToDrawableConverter : MvxValueConverter<string, Drawable>
    {
        protected override Drawable Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            int resourceId = (int)typeof(Resource.Drawable).GetField(value).GetValue(null);
            Drawable drawable = ContextCompat.GetDrawable(Android.App.Application.Context, resourceId);
            return drawable;
        }
    }
}