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

namespace SocialLadder.Droid.Converters
{
    public class DateTimeToTimeAgoStringConverter : MvxValueConverter<DateTime?, string>
    {
        protected override string Convert(DateTime? value, Type targetType, object parameter, CultureInfo culture)
        {
            string display;
            if (value != null)
            {
                TimeSpan diff =  DateTime.UtcNow  - value.Value;
                if (diff.TotalDays >= 1)
                    display = (int)diff.TotalDays + " day" + ((int)diff.TotalDays > 1 ? "s" : "") + " ago";
                else if (diff.TotalHours >= 1)
                    display = (int)diff.TotalHours + " hour" + ((int)diff.TotalHours > 1 ? "s" : "") + " ago";
                else if (diff.TotalMinutes >= 1)
                    display = (int)diff.TotalMinutes + " minute" + ((int)diff.TotalMinutes > 1 ? "s" : "") + " ago";
                else if (diff.TotalSeconds >= 1)
                    display = (int)diff.TotalSeconds + " second" + ((int)diff.TotalSeconds > 1 ? "s" : "") + " ago";
                else
                    display = "Just Now";
            }
            else
                display = "";
            return display;
        }
    }
}