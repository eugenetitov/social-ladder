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

namespace SocialLadder.iOS.Rewards
{
    [Register ("RewardsContainerViewController")]
    partial class RewardsContainerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardsTabButton AllRewardsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardsTabButton AviableRewardsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardsTabButton ClaimedButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView mainView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView tabBarView { get; set; }

        [Action ("AllRewardsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AllRewardsButton_TouchUpInside (SocialLadder.iOS.RewardsTabButton sender);

        [Action ("AviableButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AviableButton_TouchUpInside (SocialLadder.iOS.RewardsTabButton sender);

        [Action ("TransactionsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TransactionsButton_TouchUpInside (SocialLadder.iOS.RewardsTabButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AllRewardsButton != null) {
                AllRewardsButton.Dispose ();
                AllRewardsButton = null;
            }

            if (AviableRewardsButton != null) {
                AviableRewardsButton.Dispose ();
                AviableRewardsButton = null;
            }

            if (ClaimedButton != null) {
                ClaimedButton.Dispose ();
                ClaimedButton = null;
            }

            if (mainView != null) {
                mainView.Dispose ();
                mainView = null;
            }

            if (tabBarView != null) {
                tabBarView.Dispose ();
                tabBarView = null;
            }
        }
    }
}