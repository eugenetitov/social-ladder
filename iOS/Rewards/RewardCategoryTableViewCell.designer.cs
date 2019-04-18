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
    [Register ("RewardCategoryTableViewCell")]
    partial class RewardCategoryTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CategoryCountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CategoryImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CategoryImageOverlay { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CategoryName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CategoryCountLabel != null) {
                CategoryCountLabel.Dispose ();
                CategoryCountLabel = null;
            }

            if (CategoryImage != null) {
                CategoryImage.Dispose ();
                CategoryImage = null;
            }

            if (CategoryImageOverlay != null) {
                CategoryImageOverlay.Dispose ();
                CategoryImageOverlay = null;
            }

            if (CategoryName != null) {
                CategoryName.Dispose ();
                CategoryName = null;
            }
        }
    }
}