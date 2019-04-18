using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.CurrentConstants
{
    public static class Colors
    {
        public static UIColor PointsTabButtonColor { get => UIColor.FromRGB(173, 173, 173); }
        public static UIColor PointsTabButtonSelectedColor { get => UIColor.FromRGB(0, 122, 194); }
        public static UIColor FeedCellLikeButtonSelectedColor { get => UIColor.Red; }
        public static UIColor FeedCellLikeButtonUnselectedColor { get => UIColor.LightGray; }
        public static UIColor FeedCellButtonLinkColor { get => UIColor.FromRGB(0, 122, 194); }
        public static UIColor FeedCellFBButtonBorderColor { get => UIColor.FromRGB(245, 245, 245); }
        public static UIColor RewardsRedAnimationCircleColor { get => UIColor.FromRGBA(241, 114, 117, 240); }
        public static UIColor RewardsGreenAnimationCircleColor { get => UIColor.FromRGBA(36, 209, 180, 153); }
    }
}