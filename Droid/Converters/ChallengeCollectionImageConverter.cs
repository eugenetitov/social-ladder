using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
using SocialLadder.Models.LocalModels.Challenges;

namespace SocialLadder.Droid.Converters
{
    public class ChallengeCollectionImageConverter : MvxValueConverter<ChallengeIcon, Drawable>
    {
        protected override Drawable Convert(ChallengeIcon value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.Icon))
            {
                using (var webClient = new WebClient())
                {
                    var imageBytes = webClient.DownloadData(value.IconUrl);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        Drawable image = new BitmapDrawable(Application.Context.Resources, BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length));
                        return image;
                    }
                }
            }
            int resourceId = (int)typeof(Resource.Drawable).GetField(value.Icon).GetValue(null);
            Drawable drawable = ContextCompat.GetDrawable(Android.App.Application.Context, resourceId);
            return drawable;
        }
    }
}