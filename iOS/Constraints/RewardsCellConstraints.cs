using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Constraints
{
    public static class RewardsCellConstraints
    {
        public static NSLayoutConstraint RewardImageRightLeading(NSObject view1, NSObject view2, nfloat offset)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Leading, 1.0f, offset);
        }

        public static NSLayoutConstraint RewardImageRightHeight(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Width, 0.39f, 0);
        }

        public static NSLayoutConstraint DescriptionViewRightLeading(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Trailing, 1.0f, 0);
        }

        public static NSLayoutConstraint RewardImageLeftTrailing(NSObject view1, NSObject view2, nfloat offset)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Trailing, 1.0f, offset);
        }

        public static NSLayoutConstraint RewardImageLeftHeight(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Width, 0.39f, 0);
        }

        public static NSLayoutConstraint DescriptionViewLeftLeading(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Leading, 1.0f, 0);
        }

        public static NSLayoutConstraint DescriptionViewLeftTrailing(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Leading, 1.0f, 0);
        }

    }
}