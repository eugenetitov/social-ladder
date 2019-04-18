using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Views
{
    [Register("socialLadder.droid.views.CustomMapView")]
    public class CustomMapView : Android.Gms.Maps.MapView
    {
        public CustomMapView(Context context) : base(context)
        {
        }

        public CustomMapView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CustomMapView(Context context, GoogleMapOptions options) : base(context, options)
        {
        }

        public CustomMapView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected CustomMapView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            switch (ev.Action) {
                case MotionEventActions.Down:
                    this.Parent.Parent.RequestDisallowInterceptTouchEvent(true);             
                    break;
                case MotionEventActions.Up:
                    this.Parent.Parent.RequestDisallowInterceptTouchEvent(false);
                    break;
                default:
                    break;
            }
            return base.OnInterceptTouchEvent(ev);
        }
    }
}