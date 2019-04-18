using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Helpers
{
    public static class FontHelper
    {
        private static float metrics = Resources.System.DisplayMetrics.WidthPixels / Resources.System.DisplayMetrics.ScaledDensity;

        public static void UpdateFont(TextView view, string font, float textSizeWithScreenSize)
        {
            if (view != null)
            {
                view.Typeface = GetTypeFace(font);
                view.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)(metrics * textSizeWithScreenSize));
            }
        }

        public static void UpdateFont(Button view, string font, float textSizeWithScreenSize)
        {
            if (view != null)
            {
                view.SetTypeface(GetTypeFace(font), TypefaceStyle.Normal);
                view.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)(metrics * textSizeWithScreenSize));
            }
        }

        public static void UpdateFont(EditText view, string font, float textSizeWithScreenSize)
        {
            if (view != null)
            {
                view.SetTypeface(GetTypeFace(font), TypefaceStyle.Normal);
                view.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)(metrics * textSizeWithScreenSize));
            }
        }

        private static Typeface GetTypeFace(string font)
        {
            return Typeface.CreateFromAsset(Application.Context.Assets, font);
        }
    }
}