using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Java.Lang.Reflect;

namespace SocialLadder.Droid.Helpers
{
    public class PointsSmoothViewPagerHelper
    {
        public static void Customize(ViewPager viewPager)
        {
            try
            {
                var viewpager = viewPager.Class;
                Field scroller = viewpager.GetDeclaredField("mScroller");
                scroller.Accessible = true;
                scroller.Set(viewPager, new CustomScroller(viewPager.Context));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class CustomScroller : Scroller
    {
        public CustomScroller(Context context) : base(context, new DecelerateInterpolator())
        {
        }

        public CustomScroller(Context context, IInterpolator interpolator) : base(context, interpolator)
        {
        }

        public override void StartScroll(int startX, int startY, int dx, int dy, int duration)
        {
            base.StartScroll(startX, startY, dx, dy, 400);
        }
    }
}