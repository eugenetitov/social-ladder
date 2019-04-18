using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
using SocialLadder.Models;

namespace SocialLadder.Droid.Converters
{
    public class EngagementListToCommentCountConverter : MvxValueConverter<List<FeedEngagementModel>, int>
    {
        protected override int Convert(List<FeedEngagementModel> value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Where(x => x.EngagementType == "COMMENT").Count();
        }
    }
}