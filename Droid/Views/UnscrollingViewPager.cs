using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Views
{
    [Register("socialLadder.droid.views.UnscrollingViewPager")]
    public class UnscrollingViewPager : ViewPager
    {
        public UnscrollingViewPager(Context context) : base(context)
        {
        }

        public UnscrollingViewPager(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public override bool CanScrollHorizontally(int direction)
        {
            return false;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (this.Enabled)
            {
                return base.OnTouchEvent(e);
            }
            return false;

        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (this.Enabled)
            {
                return base.OnInterceptTouchEvent(ev);
            }
            return false;
        }

        public void SetPagingEnabled(bool enabled)
        {
            this.Enabled = enabled;
        }
    }
}