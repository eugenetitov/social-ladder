using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;

namespace SocialLadder.Droid.Converters
{
    public class ChallengeTypeToActorBackgroundConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string resource = "res:ic_agg_background";
            if (value != String.Empty && value != null)
            {
                resource = "res:ic_agg_challenge_background";
            }

            //string resource = "res:ic_heart_icon_off";
            //if (value != String.Empty && value != null)
            //{
            //    resource = "res:ic_heart_icon_on";
            //} 

            return resource;
        }
    }
}