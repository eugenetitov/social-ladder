// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.Points
{
    [Register ("LeaderboardTableViewCell")]
    partial class LeaderboardTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivMarker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NameText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProfileImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView ProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ProgressBarWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RankText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TopSeparator { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContentInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vInsetProportion { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vProfileImageInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vProgressNameInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vRankTextInset { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ivMarker != null) {
                ivMarker.Dispose ();
                ivMarker = null;
            }

            if (NameText != null) {
                NameText.Dispose ();
                NameText = null;
            }

            if (ProfileImage != null) {
                ProfileImage.Dispose ();
                ProfileImage = null;
            }

            if (ProgressBar != null) {
                ProgressBar.Dispose ();
                ProgressBar = null;
            }

            if (ProgressBarWidthConstraint != null) {
                ProgressBarWidthConstraint.Dispose ();
                ProgressBarWidthConstraint = null;
            }

            if (RankText != null) {
                RankText.Dispose ();
                RankText = null;
            }

            if (TopSeparator != null) {
                TopSeparator.Dispose ();
                TopSeparator = null;
            }

            if (vContentInset != null) {
                vContentInset.Dispose ();
                vContentInset = null;
            }

            if (vInsetProportion != null) {
                vInsetProportion.Dispose ();
                vInsetProportion = null;
            }

            if (vProfileImageInset != null) {
                vProfileImageInset.Dispose ();
                vProfileImageInset = null;
            }

            if (vProgressNameInset != null) {
                vProgressNameInset.Dispose ();
                vProgressNameInset = null;
            }

            if (vRankTextInset != null) {
                vRankTextInset.Dispose ();
                vRankTextInset = null;
            }
        }
    }
}