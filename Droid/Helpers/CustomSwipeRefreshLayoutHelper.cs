using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Lang.Reflect;

namespace SocialLadder.Droid.Helpers
{
    public static class CustomSwipeRefreshLayoutHelper
    {
        public static void Customize(SwipeRefreshLayout _refreshLayout)
        {
            if (_refreshLayout != null)
            {
                _refreshLayout.SetColorSchemeResources(Resource.Color.color_refresh_loader);
                _refreshLayout.SetProgressBackgroundColor(Resource.Color.color_transparent);
                _refreshLayout.SetSize(0);
                try
                {
                    var field = _refreshLayout.Class.GetDeclaredField("mCircleView");
                    AccessibleObject[] array = new AccessibleObject[1];
                    array[0] = field;
                    AccessibleObject.SetAccessible(array, true);
                    var _refreshImageView = (ImageView)field.Get(_refreshLayout);
                    _refreshImageView.Elevation = 0;
                }
                catch (NoSuchFieldException e)
                {
                    e.PrintStackTrace();
                }
                catch (IllegalAccessException e)
                {
                    e.PrintStackTrace();
                }
            }
        }
    }
}