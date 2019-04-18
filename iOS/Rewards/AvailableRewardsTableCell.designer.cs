// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.Rewards
{
    [Register ("AvailableRewardsTableCell")]
    partial class AvailableRewardsTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView IconStatus { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RewardImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView RewardName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RewardTime { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IconStatus != null) {
                IconStatus.Dispose ();
                IconStatus = null;
            }

            if (RewardImage != null) {
                RewardImage.Dispose ();
                RewardImage = null;
            }

            if (RewardName != null) {
                RewardName.Dispose ();
                RewardName = null;
            }

            if (RewardTime != null) {
                RewardTime.Dispose ();
                RewardTime = null;
            }
        }
    }
}