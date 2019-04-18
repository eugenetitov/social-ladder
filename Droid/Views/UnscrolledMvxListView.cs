using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;

namespace SocialLadder.Droid.Views
{
    [Register("socialLadder.droid.views.UnscrolledMvxListView")]
    public class UnscrolledMvxListView : MvxListView
    {

        private bool _scrollable = true;

        public UnscrolledMvxListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetScrollingEnabled(false);
        }

        public UnscrolledMvxListView(Context context, IAttributeSet attrs, IMvxAdapter adapter) : base(context, attrs, adapter)
        {
            SetScrollingEnabled(false);
        }

        protected UnscrolledMvxListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            SetScrollingEnabled(false);
        }

        private void SetScrollingEnabled(bool enabled)
        {
            _scrollable = enabled;
        }

        public bool IsScrollable()
        {
            return _scrollable;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                if (_scrollable)
                {
                    return base.OnTouchEvent(e);
                }
                return _scrollable;
            }
            return base.OnTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Down)
            {
                if (_scrollable)
                {
                    return base.OnInterceptTouchEvent(ev);
                }
                return _scrollable;
            }
            return base.OnInterceptTouchEvent(ev);
        }

        //protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        //{
        //    //base.OnScrollChanged(l, t, oldl, oldt);
        //}

        //public override void ScrollTo(int x, int y)
        //{
        //    //base.ScrollTo(x, y);
        //}
    }
}