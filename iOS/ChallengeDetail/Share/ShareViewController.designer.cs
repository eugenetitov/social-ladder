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

namespace SocialLadder.iOS.Challenges
{
    [Register ("ShareViewController")]
    partial class ShareViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnShare { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Count { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView FacebookImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch FacebookSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FacebookText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ReferralString { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SelectText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView ShareMessage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TwitterImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch TwitterSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TwitterText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vBottomRightBasis { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnShare != null) {
                btnShare.Dispose ();
                btnShare = null;
            }

            if (Count != null) {
                Count.Dispose ();
                Count = null;
            }

            if (FacebookImage != null) {
                FacebookImage.Dispose ();
                FacebookImage = null;
            }

            if (FacebookSwitch != null) {
                FacebookSwitch.Dispose ();
                FacebookSwitch = null;
            }

            if (FacebookText != null) {
                FacebookText.Dispose ();
                FacebookText = null;
            }

            if (ReferralString != null) {
                ReferralString.Dispose ();
                ReferralString = null;
            }

            if (SelectText != null) {
                SelectText.Dispose ();
                SelectText = null;
            }

            if (ShareMessage != null) {
                ShareMessage.Dispose ();
                ShareMessage = null;
            }

            if (TwitterImage != null) {
                TwitterImage.Dispose ();
                TwitterImage = null;
            }

            if (TwitterSwitch != null) {
                TwitterSwitch.Dispose ();
                TwitterSwitch = null;
            }

            if (TwitterText != null) {
                TwitterText.Dispose ();
                TwitterText = null;
            }

            if (vBottomRightBasis != null) {
                vBottomRightBasis.Dispose ();
                vBottomRightBasis = null;
            }
        }
    }
}