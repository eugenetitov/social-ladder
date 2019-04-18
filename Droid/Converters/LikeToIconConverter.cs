using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace SocialLadder.Droid.Converters
{
    public class LikeToIconConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { 
            string resource = "res:ic_heart_icon_off"; 
            if (value)
                resource = "res:ic_heart_icon_on";
            return resource;
        }
    }
}