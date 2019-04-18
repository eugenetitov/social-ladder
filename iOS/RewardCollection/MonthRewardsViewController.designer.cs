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

namespace SocialLadder.iOS.RewardCollection
{
    [Register ("MonthRewardsViewController")]
    partial class MonthRewardsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardsTabButton AllRewardsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Areas.AreaCollectionView AreaCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AreaCollectionHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardsTabButton AviableRewardsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardsTabButton ClaimedButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Rewards.RewardsTableView RewardsTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView tabBarView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AllRewardsButton != null) {
                AllRewardsButton.Dispose ();
                AllRewardsButton = null;
            }

            if (AreaCollection != null) {
                AreaCollection.Dispose ();
                AreaCollection = null;
            }

            if (AreaCollectionHeight != null) {
                AreaCollectionHeight.Dispose ();
                AreaCollectionHeight = null;
            }

            if (AviableRewardsButton != null) {
                AviableRewardsButton.Dispose ();
                AviableRewardsButton = null;
            }

            if (ClaimedButton != null) {
                ClaimedButton.Dispose ();
                ClaimedButton = null;
            }

            if (RewardsTableView != null) {
                RewardsTableView.Dispose ();
                RewardsTableView = null;
            }

            if (tabBarView != null) {
                tabBarView.Dispose ();
                tabBarView = null;
            }
        }
    }
}