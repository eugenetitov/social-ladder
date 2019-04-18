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
    [Register ("MonthRewardsTableCell")]
    partial class MonthRewardsTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RewardCategoryName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RewardIcon { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RewardItemBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RewardsCountLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (RewardCategoryName != null) {
                RewardCategoryName.Dispose ();
                RewardCategoryName = null;
            }

            if (RewardIcon != null) {
                RewardIcon.Dispose ();
                RewardIcon = null;
            }

            if (RewardItemBackground != null) {
                RewardItemBackground.Dispose ();
                RewardItemBackground = null;
            }

            if (RewardsCountLabel != null) {
                RewardsCountLabel.Dispose ();
                RewardsCountLabel = null;
            }
        }
    }
}