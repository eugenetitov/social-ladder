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
    [Register ("RewardCollectionViewController")]
    partial class RewardCollectionViewController
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
        SocialLadder.iOS.RewardsTabButton AvailableRewardsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardsTabButton ClaimedButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsAreaCollectionTop { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.RewardCollectionView CollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView EmptyCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView tabBarView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.AnimatedImageView ViewForImage { get; set; }

        [Action ("AllRewardsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AllRewardsButton_TouchUpInside (SocialLadder.iOS.RewardsTabButton sender);

        [Action ("AviableRewardsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AviableRewardsButton_TouchUpInside (SocialLadder.iOS.RewardsTabButton sender);

        [Action ("ClaimedButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ClaimedButton_TouchUpInside (SocialLadder.iOS.RewardsTabButton sender);

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

            if (AvailableRewardsButton != null) {
                AvailableRewardsButton.Dispose ();
                AvailableRewardsButton = null;
            }

            if (ClaimedButton != null) {
                ClaimedButton.Dispose ();
                ClaimedButton = null;
            }

            if (cnsAreaCollectionTop != null) {
                cnsAreaCollectionTop.Dispose ();
                cnsAreaCollectionTop = null;
            }

            if (CollectionView != null) {
                CollectionView.Dispose ();
                CollectionView = null;
            }

            if (EmptyCollectionView != null) {
                EmptyCollectionView.Dispose ();
                EmptyCollectionView = null;
            }

            if (tabBarView != null) {
                tabBarView.Dispose ();
                tabBarView = null;
            }

            if (ViewForImage != null) {
                ViewForImage.Dispose ();
                ViewForImage = null;
            }
        }
    }
}