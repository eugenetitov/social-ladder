using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Helpers
{
    public static class DimensHelper
    {
        public static int GetDimensById(int deminId)
        {
            return (int)Application.Context.Resources.GetDimension(deminId);
        }

        public static int GetScreenWidth()
        {
            return Android.Content.Res.Resources.System.DisplayMetrics.WidthPixels;
        }

        public static int GetScreenHeight()
        {
            return Android.Content.Res.Resources.System.DisplayMetrics.HeightPixels;
        }

        public static float GetScreenDensity()
        {
            return Android.Content.Res.Resources.System.DisplayMetrics.Density;
        }

        public static int GetStatusBarHeight()
        {
            int result = 0;
            int resourceId = Application.Context.Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                result = Application.Context.Resources.GetDimensionPixelSize(resourceId);
            }
            return result;
        }
    }
}