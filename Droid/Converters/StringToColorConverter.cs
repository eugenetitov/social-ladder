using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Color;

namespace SocialLadder.Droid.Converters
{
    public class StringToColorConverter: MvxColorValueConverter<string>
    {  
        protected override MvxColor Convert(string value, object parameter, CultureInfo culture)
        {
            if (value == "Logout")
            {
                return new MvxColor(Color.Argb(255, 173, 173, 173));
            }
            else
            {
                return new MvxColor(Color.Black);
            }
        }
    }

    public class NetworkStringToColorConverter : MvxColorValueConverter<string>
    {
        protected override MvxColor Convert(string value, object parameter, CultureInfo culture)
        {
            if (value == "fb_unconnected" || value == "twitter_unconnected" || value == "insta_unconnected")
            {
                return new MvxColor(Color.Argb(255, 173, 173, 173));
            }
            else
            {
                return new MvxColor(Color.Black);
            }
        }
    }
}