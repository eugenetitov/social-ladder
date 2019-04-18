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
    [Register ("MultipleChoiceViewController")]
    partial class MultipleChoiceViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnTopNotify { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChallengeImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ChallengeText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsTableViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsWebViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint CollectionViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Content { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ImageHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ImageOverlayView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imgTopLock { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTopUnlocksIn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MCCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView PointsImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PointsText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SelectAllThatApply { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubmitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Challenges.MultipleChoiceTableView TableView { get; set; }

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
            if (btnTopNotify != null) {
                btnTopNotify.Dispose ();
                btnTopNotify = null;
            }

            if (ChallengeImage != null) {
                ChallengeImage.Dispose ();
                ChallengeImage = null;
            }

            if (ChallengeText != null) {
                ChallengeText.Dispose ();
                ChallengeText = null;
            }

            if (cnsTableViewHeight != null) {
                cnsTableViewHeight.Dispose ();
                cnsTableViewHeight = null;
            }

            if (cnsWebViewHeight != null) {
                cnsWebViewHeight.Dispose ();
                cnsWebViewHeight = null;
            }

            if (CollectionViewHeightConstraint != null) {
                CollectionViewHeightConstraint.Dispose ();
                CollectionViewHeightConstraint = null;
            }

            if (Content != null) {
                Content.Dispose ();
                Content = null;
            }

            if (ImageHeightConstraint != null) {
                ImageHeightConstraint.Dispose ();
                ImageHeightConstraint = null;
            }

            if (ImageOverlayView != null) {
                ImageOverlayView.Dispose ();
                ImageOverlayView = null;
            }

            if (imgTopLock != null) {
                imgTopLock.Dispose ();
                imgTopLock = null;
            }

            if (lblTopTime != null) {
                lblTopTime.Dispose ();
                lblTopTime = null;
            }

            if (lblTopUnlocksIn != null) {
                lblTopUnlocksIn.Dispose ();
                lblTopUnlocksIn = null;
            }

            if (MCCollectionView != null) {
                MCCollectionView.Dispose ();
                MCCollectionView = null;
            }

            if (PointsImage != null) {
                PointsImage.Dispose ();
                PointsImage = null;
            }

            if (PointsText != null) {
                PointsText.Dispose ();
                PointsText = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (SelectAllThatApply != null) {
                SelectAllThatApply.Dispose ();
                SelectAllThatApply = null;
            }

            if (SubmitButton != null) {
                SubmitButton.Dispose ();
                SubmitButton = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
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