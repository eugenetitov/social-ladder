using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Helpers.ChallengesCollectionScrollHelper
{
    public class ChallengesLinearLayoutManager : LinearLayoutManager
    {
        private Context _context;
        private CustomLinearSmoothScroller customLinearSmoothScroller;

        public ChallengesLinearLayoutManager(Context context) : base(context)
        {
            Init(context);
        }

        public ChallengesLinearLayoutManager(Context context, int orientation, bool reverseLayout) : base(context, orientation, reverseLayout)
        {
            Init(context);
        }

        public ChallengesLinearLayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context);
        }

        protected ChallengesLinearLayoutManager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void Init(Context context)
        {
            _context = context;
            customLinearSmoothScroller = new CustomLinearSmoothScroller(context);
        }

        public override void ScrollToPosition(int position)
        {
            customLinearSmoothScroller.TargetPosition = position;
            StartSmoothScroll(customLinearSmoothScroller);
            base.ScrollToPosition(position);
        }
    }
}