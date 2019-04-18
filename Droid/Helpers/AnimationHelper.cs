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

namespace SocialLadder.Droid.Helpers
{
    public static class AnimationHelper
    {
        public static void AnimateImage(ImageView image, int resource = 0)
        {
            try
            {
                if (resource != 0)
                {
                    image.SetImageResource(resource);
                }
                RotateAnimation rotateAnimation = new RotateAnimation(0, 359, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
                rotateAnimation.RepeatCount = Animation.Infinite;
                rotateAnimation.RepeatMode = RepeatMode.Restart;
                rotateAnimation.Duration = 700;
                rotateAnimation.Interpolator = new LinearInterpolator();
                image.StartAnimation(rotateAnimation);

            }
            catch { }
        }

        public static void AnimateButton(ImageButton button, int resource = 0)
        {
            try
            {
                if (resource != 0)
                {
                    button.SetImageResource(resource);
                }
                RotateAnimation rotateAnimation = new RotateAnimation(0, 359, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
                rotateAnimation.RepeatCount = Animation.Infinite;
                rotateAnimation.RepeatMode = RepeatMode.Restart;
                rotateAnimation.Duration = 700;
                rotateAnimation.Interpolator = new LinearInterpolator();
                button.StartAnimation(rotateAnimation);

            }
            catch { }
        }
    }
}