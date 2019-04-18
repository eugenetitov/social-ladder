// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.RewardCollection
{
    [Register ("RewardCollectionViewCell")]
    partial class RewardCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RewardImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RewardImageOverlay { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RewardName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RewardsCountLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (RewardImage != null) {
                RewardImage.Dispose ();
                RewardImage = null;
            }

            if (RewardImageOverlay != null) {
                RewardImageOverlay.Dispose ();
                RewardImageOverlay = null;
            }

            if (RewardName != null) {
                RewardName.Dispose ();
                RewardName = null;
            }

            if (RewardsCountLabel != null) {
                RewardsCountLabel.Dispose ();
                RewardsCountLabel = null;
            }
        }
    }
}