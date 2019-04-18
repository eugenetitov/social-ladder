using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Constraints
{
    public static class AreasConstraints
    {
        public static nfloat ScreenWidth = UIScreen.MainScreen.Bounds.Width;

        public static NSLayoutConstraint AreaInfoViewTop(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Top, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Top, 1.0f, 0f);
        }

        public static NSLayoutConstraint AreaInfoViewBottom(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Bottom, 1.0f, 0f);
        }

        public static NSLayoutConstraint AreaInfoViewLeading(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Leading, 1.0f, 0f);
        }

        public static NSLayoutConstraint AreaInfoViewTrailing(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Trailing, 1.0f, 0f);
        }

        public static NSLayoutConstraint AreasTableSourceOverlayCenterX(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view2, NSLayoutAttribute.CenterX, 1.0f, 0f);
        }

        public static NSLayoutConstraint AreasTableSourceOverlayCenterY(NSObject view1, NSObject view2)
        {
            return NSLayoutConstraint.Create(view1, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, view2, NSLayoutAttribute.CenterY, 1.0f, 0f);
        }

        public static NSLayoutConstraint AreasTableSourceOverlayTop(NSObject view1, NSObject view2)
        {
            NSLayoutConstraint constraint =  NSLayoutConstraint.Create(view1, NSLayoutAttribute.Top, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Top, 1.0f, 175f / 414f * ScreenWidth);
            return constraint;
        }

        public static NSLayoutConstraint AreasTableSourceOverlayWidth(NSObject view1, NSObject view2)
        {
            NSLayoutConstraint constraint = NSLayoutConstraint.Create(view1, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view2, NSLayoutAttribute.Width, 366f / 414f, 0f);
            return constraint;
        }

        public static NSLayoutConstraint AreasTableSourceOverlayHeight(NSObject view1, NSObject view2)
        {
            NSLayoutConstraint constraint = NSLayoutConstraint.Create(view1, NSLayoutAttribute.Height, NSLayoutRelation.GreaterThanOrEqual, view1, NSLayoutAttribute.Width, 386f / 366f, 0f);
            return constraint;
        }
    }
}