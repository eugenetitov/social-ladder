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
    [Register ("InstagramViewController")]
    partial class InstagramViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AspectHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChallengeImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ChallengeText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsWebViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView CollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CollectionViewDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HashBottomText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HashtagCopiedText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField HashText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HashTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView InstaCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint InstaCollectionViewAspect { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView MainScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MainSrollBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PointsImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PointsText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubmitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TagCopiedView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        WebKit.WKWebView WebView { get; set; }

        [Action ("SubmitButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SubmitButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AspectHeight != null) {
                AspectHeight.Dispose ();
                AspectHeight = null;
            }

            if (ChallengeImage != null) {
                ChallengeImage.Dispose ();
                ChallengeImage = null;
            }

            if (ChallengeText != null) {
                ChallengeText.Dispose ();
                ChallengeText = null;
            }

            if (cnsWebViewHeight != null) {
                cnsWebViewHeight.Dispose ();
                cnsWebViewHeight = null;
            }

            if (CollectionView != null) {
                CollectionView.Dispose ();
                CollectionView = null;
            }

            if (CollectionViewDescription != null) {
                CollectionViewDescription.Dispose ();
                CollectionViewDescription = null;
            }

            if (HashBottomText != null) {
                HashBottomText.Dispose ();
                HashBottomText = null;
            }

            if (HashtagCopiedText != null) {
                HashtagCopiedText.Dispose ();
                HashtagCopiedText = null;
            }

            if (HashText != null) {
                HashText.Dispose ();
                HashText = null;
            }

            if (HashTitle != null) {
                HashTitle.Dispose ();
                HashTitle = null;
            }

            if (InstaCollectionView != null) {
                InstaCollectionView.Dispose ();
                InstaCollectionView = null;
            }

            if (InstaCollectionViewAspect != null) {
                InstaCollectionViewAspect.Dispose ();
                InstaCollectionViewAspect = null;
            }

            if (MainScroll != null) {
                MainScroll.Dispose ();
                MainScroll = null;
            }

            if (MainSrollBottomConstraint != null) {
                MainSrollBottomConstraint.Dispose ();
                MainSrollBottomConstraint = null;
            }

            if (PointsImage != null) {
                PointsImage.Dispose ();
                PointsImage = null;
            }

            if (PointsText != null) {
                PointsText.Dispose ();
                PointsText = null;
            }

            if (SubmitButton != null) {
                SubmitButton.Dispose ();
                SubmitButton = null;
            }

            if (TagCopiedView != null) {
                TagCopiedView.Dispose ();
                TagCopiedView = null;
            }

            if (TimeText != null) {
                TimeText.Dispose ();
                TimeText = null;
            }

            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }
        }
    }
}