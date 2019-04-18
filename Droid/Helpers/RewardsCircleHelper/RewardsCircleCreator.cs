using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace SocialLadder.Droid.Helpers.RewardsCircleHelper
{
    public class RewardsCircleCreator
    {
        public static void Create(Activity activity, View parentView, Action completeAnimation = null)
        {
            var container = parentView.FindViewById<RelativeLayout>(Resource.Id.circle_view_container);

            var rewardsCircleView = new ImageView(activity);
            rewardsCircleView.Id = new ViewIdGenerator(activity).FindFreeId();
            var animSize = DimensHelper.GetScreenHeight() * 2;

            rewardsCircleView.SetImageResource(Resource.Drawable.reward_circle_background);

            RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(animSize, animSize);
            param.AddRule(LayoutRules.AlignParentBottom);
            param.AddRule(LayoutRules.AlignParentRight);
            param.RightMargin = (int)Math.Round(-animSize * 0.333f);
            param.BottomMargin = (int)Math.Round(-animSize * 0.445f);
            param.TopMargin = (int)Math.Round(-animSize * 0.168f);
            param.LeftMargin = (int)Math.Round(-animSize * 0.16f);
            rewardsCircleView.LayoutParameters = param;
            container.AddView(rewardsCircleView);

            AnimateCircle(activity, rewardsCircleView, completeAnimation);
        }

        private static void AnimateCircle(Activity activity, ImageView image, Action completeAnimation)
        {
            Animation animation = AnimationUtils.LoadAnimation(activity, Resource.Animation.rewards_details_circle_anim);
            animation.RepeatCount = 0;
            animation.FillAfter = true;
            animation.FillBefore = true;
            animation.AnimationEnd += (s, e) => {
                if (completeAnimation != null)
                {
                    completeAnimation.Invoke();
                }
            };
            image.StartAnimation(animation);
        }
    }
}