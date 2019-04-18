using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Helpers.ChallengesCollectionScrollHelper
{
    public class CustomLinearSmoothScroller : LinearSmoothScroller
    {
        private Context _context;

        public CustomLinearSmoothScroller(Context context) : base(context)
        {
            _context = context;
        }

        public override int CalculateDtToFit(int viewStart, int viewEnd, int boxStart, int boxEnd, int snapPreference)
        {
            //return base.CalculateDtToFit(viewStart, viewEnd, boxStart, boxEnd, snapPreference);
            int dimen = (int)System.Math.Round(DimensHelper.GetDimensById(Resource.Dimension.challenges_collection_unselected_side) / DimensHelper.GetScreenDensity());
            var positionX = (boxStart + (boxEnd - boxStart) / 2) - (viewStart + (viewEnd - viewStart) / 2) + dimen;
            return positionX;
        }
    }
}