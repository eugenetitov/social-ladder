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
    public  class RewardScoreToStatusImageConverter : MvxValueConverter<int, string>
    {
        protected override string Convert(int value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string resource = "res:claim_btn_background";
            if (value > SL.Profile.Score)
                resource = "res:reward_more_score";
            return resource;
        }
    
    }
}