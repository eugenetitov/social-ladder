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
    public class CommentsToVisibilityConverter : MvxValueConverter<List<FeedEngagementModel>, ViewStates>
    {
        protected override ViewStates Convert(List<FeedEngagementModel> value, Type targetType, object parameter, CultureInfo culture)
        {
            long minCountOfComments = (Int64)parameter; 
            if (value == null)
            {
                return ViewStates.Gone;
            }
            var countOfComments = value.Where(x => x.EngagementType == "COMMENT").Count<FeedEngagementModel>();
            if (countOfComments > minCountOfComments)
            {
                return ViewStates.Visible;
            }
            return ViewStates.Gone;
        }
    }
}