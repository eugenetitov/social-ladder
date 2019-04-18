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
    [Register ("FacebookDetailViewController")]
    partial class FacebookDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AspectHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChallengesImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsWebViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CountOthersLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CountPeopleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView FBCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint FBCollectionViewAspect { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HeaderTextLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView MainScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubmitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeLastLbl { get; set; }

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

            if (ChallengesImage != null) {
                ChallengesImage.Dispose ();
                ChallengesImage = null;
            }

            if (cnsWebViewHeight != null) {
                cnsWebViewHeight.Dispose ();
                cnsWebViewHeight = null;
            }

            if (CountOthersLbl != null) {
                CountOthersLbl.Dispose ();
                CountOthersLbl = null;
            }

            if (CountPeopleLbl != null) {
                CountPeopleLbl.Dispose ();
                CountPeopleLbl = null;
            }

            if (FBCollectionView != null) {
                FBCollectionView.Dispose ();
                FBCollectionView = null;
            }

            if (FBCollectionViewAspect != null) {
                FBCollectionViewAspect.Dispose ();
                FBCollectionViewAspect = null;
            }

            if (HeaderTextLbl != null) {
                HeaderTextLbl.Dispose ();
                HeaderTextLbl = null;
            }

            if (MainScroll != null) {
                MainScroll.Dispose ();
                MainScroll = null;
            }

            if (SubmitButton != null) {
                SubmitButton.Dispose ();
                SubmitButton = null;
            }

            if (TimeLastLbl != null) {
                TimeLastLbl.Dispose ();
                TimeLastLbl = null;
            }

            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }
        }
    }
}