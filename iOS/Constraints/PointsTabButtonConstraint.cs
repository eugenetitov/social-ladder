using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Constraints
{
    public static class PointsTabButtonConstraint
    {
        public static NSLayoutConstraint PointsTabButtonTopTextCenterX(NSObject topText, NSObject view)
        {
            return NSLayoutConstraint.Create(topText, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1.0f, 0f);
        }

        public static NSLayoutConstraint PointsTabButtonTopTextTop(NSObject topText, NSObject view)
        {
            return NSLayoutConstraint.Create(topText, NSLayoutAttribute.Top, NSLayoutRelation.Equal, view, NSLayoutAttribute.Top, 1.0f, 0f);
        }

        public static NSLayoutConstraint PointsTabButtonTopTextHeight(NSObject topText, NSObject view)
        {
            return NSLayoutConstraint.Create(topText, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0.9f, 4f);
        }

        public static NSLayoutConstraint PointsTabButtonTopTextWidth(NSObject topText, NSObject view)
        {
            return NSLayoutConstraint.Create(topText, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, 1.0f, 0f);
        }


        public static NSLayoutConstraint PointsTabButtonBottomLineCenterX(NSObject bottomLine, NSObject view)
        {
            return NSLayoutConstraint.Create(bottomLine, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1.0f, 0f);
        }

        public static NSLayoutConstraint PointsTabButtonBottomLineBottom(NSObject bottomLine, NSObject view)
        {
            return NSLayoutConstraint.Create(bottomLine, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, view, NSLayoutAttribute.Bottom, 1.0f, -1f);
            //return NSLayoutConstraint.Create(topText, NSLayoutAttribute.Top, NSLayoutRelation.Equal, view, NSLayoutAttribute.Top, 1.0f, 0f);
        }

        public static NSLayoutConstraint PointsTabButtonBottomLineHeight(NSObject bottomLine, NSObject view)
        {
            return NSLayoutConstraint.Create(bottomLine, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0.05f, 0f);
            //return NSLayoutConstraint.Create(topText, NSLayoutAttribute.Height, NSLayoutRelation.Equal, view, NSLayoutAttribute.Height, 0.9f, 0f);
        }

        public static NSLayoutConstraint PointsTabButtonBottomLineWidth(NSObject bottomLine, NSObject view)
        {
            return NSLayoutConstraint.Create(bottomLine, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, 1.0f, 6f);
            //return NSLayoutConstraint.Create(topText, NSLayoutAttribute.Width, NSLayoutRelation.Equal, view, NSLayoutAttribute.Width, 1.0f, 0f);
        }

    }
}