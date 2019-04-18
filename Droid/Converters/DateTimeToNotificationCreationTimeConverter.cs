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
    public class DateTimeToNotificationCreationTimeConverter : MvxValueConverter<DateTime?, string>
    {
        protected override string Convert(DateTime? date, Type targetType, object parameter, CultureInfo culture)
        {
            if (date == null)
            {
                return string.Empty;
            }

            string display;
            if (date != null)
            {
                TimeSpan diff =  DateTime.UtcNow - date.Value;
                if (diff.TotalDays >= 1)
                    display = (int)diff.TotalDays + "d";
                else if (diff.TotalHours >= 1)
                    display = (int)diff.TotalHours + "h";
                else if (diff.TotalMinutes >= 1)
                    display = (int)diff.TotalMinutes + "m";
                else if (diff.TotalSeconds >= 1)
                    display = (int)diff.TotalSeconds + "s";
                else
                    display = "Just Now";
            }
            else
                display = "";
            return display;
        }
    }
}