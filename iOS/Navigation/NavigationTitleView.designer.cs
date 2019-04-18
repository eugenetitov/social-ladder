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

namespace SocialLadder.iOS.Navigation
{
    [Register ("NavigationTitleView")]
    partial class NavigationTitleView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnBack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsVActionBarAlignmentBasisHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView HandleImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NotificationButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView NotificationButtonImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Score { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ScoreImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Title { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vActionBarAlignmentBasis { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vHandleImageLeftBottomBasis { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vLeftBottomBasis { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnBack != null) {
                btnBack.Dispose ();
                btnBack = null;
            }

            if (btnDone != null) {
                btnDone.Dispose ();
                btnDone = null;
            }

            if (cnsVActionBarAlignmentBasisHeight != null) {
                cnsVActionBarAlignmentBasisHeight.Dispose ();
                cnsVActionBarAlignmentBasisHeight = null;
            }

            if (HandleImage != null) {
                HandleImage.Dispose ();
                HandleImage = null;
            }

            if (Image != null) {
                Image.Dispose ();
                Image = null;
            }

            if (NotificationButton != null) {
                NotificationButton.Dispose ();
                NotificationButton = null;
            }

            if (NotificationButtonImage != null) {
                NotificationButtonImage.Dispose ();
                NotificationButtonImage = null;
            }

            if (Score != null) {
                Score.Dispose ();
                Score = null;
            }

            if (ScoreImage != null) {
                ScoreImage.Dispose ();
                ScoreImage = null;
            }

            if (Title != null) {
                Title.Dispose ();
                Title = null;
            }

            if (vActionBarAlignmentBasis != null) {
                vActionBarAlignmentBasis.Dispose ();
                vActionBarAlignmentBasis = null;
            }

            if (vHandleImageLeftBottomBasis != null) {
                vHandleImageLeftBottomBasis.Dispose ();
                vHandleImageLeftBottomBasis = null;
            }

            if (vLeftBottomBasis != null) {
                vLeftBottomBasis.Dispose ();
                vLeftBottomBasis = null;
            }
        }
    }
}