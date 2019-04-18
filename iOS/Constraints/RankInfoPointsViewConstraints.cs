using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Constraints
{
   public static class RankInfoPointsViewConstraints
    {
        public static NSLayoutConstraint RankInfoPointsViewCenterX(NSObject vRankInfo, NSObject view)
        {
            return NSLayoutConstraint.Create(vRankInfo, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1.0f, 0f);
        }

        public static NSLayoutConstraint RankInfoPointsViewCenterY(NSObject vRankInfo, NSObject view)
        {
            return NSLayoutConstraint.Create(vRankInfo, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1.0f, 0f);
        }

        public static NSLayoutConstraint RankInfoPointsViewWidth(NSObject vRankInfo, NSObject view)
        {
            return NSLayoutConstraint.Create(vRankInfo, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, 382f / CodeBehindUIConstants.BaseMarkupWidth, 0f);
        }

        public static NSLayoutConstraint RankInfoPointsViewAspectRatio(NSObject vRankInfo)
        {
            return NSLayoutConstraint.Create(vRankInfo, NSLayoutAttribute.Width, NSLayoutRelation.Equal, vRankInfo, NSLayoutAttribute.Height, 435 / CodeBehindUIConstants.BaseMarkupWidth, 0f);
        }
    }
}