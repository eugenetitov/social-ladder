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
    public class ActionsToFooterFeedCellVisibilityConverter : MvxValueConverter<Dictionary<string, FeedActionModel>, ViewStates>
    {
        protected override ViewStates Convert(Dictionary<string, FeedActionModel> actionDictionary, Type targetType, object parameter, CultureInfo culture)
        {
            if ((actionDictionary==null) || ((!actionDictionary.ContainsKey("Comment")) && ((!actionDictionary.ContainsKey("Boost")||(!actionDictionary.ContainsKey("Like"))))))
            {
                return ViewStates.Gone;
            }
            return ViewStates.Visible;
        }
    }
}