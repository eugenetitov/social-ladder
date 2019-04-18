using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Constraints
{
    public static class ChallengesConstraints
    {
        public static NSLayoutConstraint ChallengesCollectionCellCenterXConstraint(NSObject currentView, NSObject parentView)
        {
            return NSLayoutConstraint.Create(currentView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, parentView, NSLayoutAttribute.CenterX, 1.0f, 0f);
        }

        public static NSLayoutConstraint ChallengesCollectionCellCenterYConstraint(NSObject currentView, NSObject parentView)
        {
            return NSLayoutConstraint.Create(currentView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, parentView, NSLayoutAttribute.CenterY, 1.0f, 0f);
        }

        public static NSLayoutConstraint ChallengesCollectionCellWidthConstraint(NSObject currentView, NSObject parentView, nfloat multiplayer)
        {
            return NSLayoutConstraint.Create(currentView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, parentView, NSLayoutAttribute.Width, multiplayer, 0f);
        }

        public static NSLayoutConstraint ChallengesCollectionCellHeightConstraint(NSObject currentView, NSObject parentView, nfloat multiplayer)
        {
            return NSLayoutConstraint.Create(currentView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, parentView, NSLayoutAttribute.Height, multiplayer, 0f);
        }

        public static NSLayoutConstraint ChallengesConstantHeightConstraint(NSObject currentView, nfloat constant)
        {
            return NSLayoutConstraint.Create(currentView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1.0f, constant);
        }

        public static NSLayoutConstraint ChallengesConstantTopConstraint(NSObject currentView, NSObject parentView, nfloat constant)
        {
            return NSLayoutConstraint.Create(currentView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, parentView, NSLayoutAttribute.Top, 1.0f, constant);
        }
    }
}