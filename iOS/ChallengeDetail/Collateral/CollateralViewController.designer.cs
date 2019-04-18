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
    [Register ("CollateralViewController")]
    partial class CollateralViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddPosterButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CameraButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CheckBoxsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CollectionFullView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Challenges.CollateralCollectionView CollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView DescriptionText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PreviewImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView StepOneImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel StepOneText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView StepTwoImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel StepTwoText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UploadCountText { get; set; }

        [Action ("AddPosterButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void AddPosterButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("CameraButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CameraButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("DeleteButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void DeleteButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddPosterButton != null) {
                AddPosterButton.Dispose ();
                AddPosterButton = null;
            }

            if (CameraButton != null) {
                CameraButton.Dispose ();
                CameraButton = null;
            }

            if (CheckBoxsView != null) {
                CheckBoxsView.Dispose ();
                CheckBoxsView = null;
            }

            if (CollectionFullView != null) {
                CollectionFullView.Dispose ();
                CollectionFullView = null;
            }

            if (CollectionView != null) {
                CollectionView.Dispose ();
                CollectionView = null;
            }

            if (DeleteButton != null) {
                DeleteButton.Dispose ();
                DeleteButton = null;
            }

            if (DescriptionText != null) {
                DescriptionText.Dispose ();
                DescriptionText = null;
            }

            if (PreviewImage != null) {
                PreviewImage.Dispose ();
                PreviewImage = null;
            }

            if (StepOneImage != null) {
                StepOneImage.Dispose ();
                StepOneImage = null;
            }

            if (StepOneText != null) {
                StepOneText.Dispose ();
                StepOneText = null;
            }

            if (StepTwoImage != null) {
                StepTwoImage.Dispose ();
                StepTwoImage = null;
            }

            if (StepTwoText != null) {
                StepTwoText.Dispose ();
                StepTwoText = null;
            }

            if (UploadCountText != null) {
                UploadCountText.Dispose ();
                UploadCountText = null;
            }
        }
    }
}