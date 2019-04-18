using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Helpers
{
    public static class PointsHeightHelper
    {
        public static void UpdateBackgroundCollectionViewHeight(View view, bool isShowed, double topHeight)
        {
            var backgroundHeight = GetFreeSpaceHeight(isShowed) - topHeight;
            var backgroundParams = view.LayoutParameters;
            backgroundParams.Height = (int)backgroundHeight;
            view.LayoutParameters = backgroundParams;
            view.Invalidate();
        }

        private static int GetFreeSpaceHeight(bool isShowed)
        {
            var topViewHeight = !isShowed ? DimensHelper.GetDimensById(Resource.Dimension.toolbar_height) : DimensHelper.GetDimensById(Resource.Dimension.areas_view_height);
            var stageHeight = DimensHelper.GetScreenHeight() - (DimensHelper.GetStatusBarHeight() + DimensHelper.GetDimensById(Resource.Dimension.divider_height) * 2 + topViewHeight +
                DimensHelper.GetDimensById(Resource.Dimension.tabbar_height) + DimensHelper.GetDimensById(Resource.Dimension.points_tabs_height));
            return stageHeight;
        }
    }
}