// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SocialLadder.iOS.Points
{
    [Register ("PointsContainerViewController")]
    partial class PointsContainerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.CustomViews.PointsTabButton LeaderboardButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView mainView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.CustomViews.PointsTabButton PointsAndStatsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.CustomViews.PointsTabButton TransactionsButton { get; set; }

        [Action ("LeaderboardButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LeaderboardButton_TouchUpInside (SocialLadder.iOS.Points.CustomViews.PointsTabButton sender);

        [Action ("PointsAndStats_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PointsAndStats_TouchUpInside (SocialLadder.iOS.Points.CustomViews.PointsTabButton sender);

        [Action ("TransactionsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TransactionsButton_TouchUpInside (SocialLadder.iOS.Points.CustomViews.PointsTabButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (LeaderboardButton != null) {
                LeaderboardButton.Dispose ();
                LeaderboardButton = null;
            }

            if (mainView != null) {
                mainView.Dispose ();
                mainView = null;
            }

            if (PointsAndStatsButton != null) {
                PointsAndStatsButton.Dispose ();
                PointsAndStatsButton = null;
            }

            if (TransactionsButton != null) {
                TransactionsButton.Dispose ();
                TransactionsButton = null;
            }
        }
    }
}