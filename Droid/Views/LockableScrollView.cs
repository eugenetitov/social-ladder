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

namespace SocialLadder.Droid.Views
{
    [Register("socialLadder.droid.views.LockableScrollView")]
    public class LockableScrollView : ScrollView
    {
        private bool _scrollable = true;
        private bool _isRefreshing = false;
        private bool LockedScroll = false;

        #region Cstr
        public LockableScrollView(Context context) : base(context)
        {
        }

        public LockableScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public LockableScrollView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected LockableScrollView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        #endregion
        #region Methods
        public void ChangeRefreshStatus(bool isRefreshed)
        {
            _isRefreshing = isRefreshed;
        }

        public bool IsRefreshed()
        {
            return _isRefreshing;
        }

        public void SetScrollingEnabled(bool enabled)
        {
            _scrollable = enabled;
        }

        public bool IsScrollable()
        {
            return _scrollable;
        }
        #endregion
        #region Overrides
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

        public override void ScrollTo(int x, int y)
        {
            if (LockedScroll)
            {
                return;
            }
            base.ScrollTo(x, y);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            LockedScroll = true;
            base.OnLayout(changed, left, top, right, bottom);
            LockedScroll = false;
        }
        #endregion
    }
}