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
using SocialLadder.Enums;
using SocialLadder.Models;

namespace SocialLadder.Droid.Converters
{
    public class InverseVisibilityValueConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? ViewStates.Gone : ViewStates.Visible;
        }
    }
    public class DirectVisibilityValueConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }
    }
    public class DirectHiddenValueConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? ViewStates.Visible : ViewStates.Invisible;
        }
    }
    public class FeedBackgroundVisibilityConverter : MvxValueConverter<FeedContentType[], ViewStates>
    {
        protected override ViewStates Convert(FeedContentType[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.Contains(Enums.FeedContentType.RewardClaimed))
            {
                return ViewStates.Visible;
            }
            if (value.Contains(FeedContentType.Aggregate))
            {
                return ViewStates.Visible;
            }
            return ViewStates.Gone;
        }
    }

    public class PointsToVisibilityConverter : MvxValueConverter<int?, ViewStates>
    {
        protected override ViewStates Convert(int? value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value == 0)
            {
                return ViewStates.Invisible;
            }
            return ViewStates.Visible;
            //return base.Convert(value, targetType, parameter, culture);
        }
    }

    public class ActionDictionaryToReportVisibilityConverter : MvxValueConverter<Dictionary<string, FeedActionModel>, ViewStates>
    {
        protected override ViewStates Convert(Dictionary<string, FeedActionModel> value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return ViewStates.Invisible;
            }
            if ((value.ContainsKey("Report This")) || (value.ContainsKey("Flag It")))
            {
                return ViewStates.Visible;
            }
            return ViewStates.Invisible;
        }
    }

    public class RewardScoreToVisibilityConverter : MvxValueConverter<int, ViewStates>
    {
        protected override ViewStates Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            string rewardType = parameter as string;
            if (rewardType.ToLower() == "blocked")
            {
                if (value > SL.Profile.Score)
                {
                    return ViewStates.Visible;
                }
                return ViewStates.Invisible;
            }
            if (value <= SL.Profile.Score)
            {
                return ViewStates.Visible;
            }
            return ViewStates.Invisible;

        }
    }

    public class InverseBoolVisibilityConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            if (false == value)
            {
                return ViewStates.Visible;
            }
            return ViewStates.Gone;

        }
    }

    public class ActionsToCommentsVisibilityConverter : MvxValueConverter<Dictionary<string, FeedActionModel>, ViewStates>
    {
        protected override ViewStates Convert(Dictionary<string, FeedActionModel> actionDictionary, Type targetType, object parameter, CultureInfo culture)
        {
            if ((actionDictionary == null) || (!actionDictionary.ContainsKey("Comment")))
            {
                return ViewStates.Invisible;
            }
            return ViewStates.Visible;
        }
    }

    public class ActionsToLikeVisibilityConverter : MvxValueConverter<Dictionary<string, FeedActionModel>, ViewStates>
    {
        protected override ViewStates Convert(Dictionary<string, FeedActionModel> actionDictionary, Type targetType, object parameter, CultureInfo culture)
        {
            if ((actionDictionary == null) || ((!actionDictionary.ContainsKey("Like")) && ((!actionDictionary.ContainsKey("Boost")))))
            {
                return ViewStates.Invisible;
            }
            return ViewStates.Visible;
        }
    }

    public class DescriptionTextItemVisibilityConverter : MvxValueConverter<string, ViewStates>
    {
        protected override ViewStates Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ViewStates.Gone;
            }
            else
            {
                return ViewStates.Visible;
            }
        }
    }

    public class DescriptionButtonItemVisibilityConverter : MvxValueConverter<string, ViewStates>
    {
        protected override ViewStates Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ViewStates.Visible;
            }
            else
            {
                return ViewStates.Gone;
            }
        }
    }

    public class MapItemVisibilityValueConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? ViewStates.Visible : ViewStates.Gone;
        }
    }

    public class PhotoItemVisibilityValueConverter : MvxValueConverter<bool, ViewStates>
    {
        protected override ViewStates Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ? ViewStates.Gone : ViewStates.Visible;
        }
    }

    public class LabelItemVisibilityValueConverter : MvxValueConverter<int, ViewStates>
    {
        protected override ViewStates Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == 3)
                return ViewStates.Invisible;
            else
                return ViewStates.Visible;
        }
    }
}