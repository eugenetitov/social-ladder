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
    [Register ("CheckInViewController")]
    partial class CheckInViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CheckInCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint CheckInCollectionViewAspect { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsMapTop { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnWebViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView CollectionPeople { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Content { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CountPeopleLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HeaderLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MainContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView MainScroll { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        MapKit.MKMapView MapView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView padding { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubmitButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeDisLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        WebKit.WKWebView WebView { get; set; }

        [Action ("SubmitButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SubmitButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CheckInCollectionView != null) {
                CheckInCollectionView.Dispose ();
                CheckInCollectionView = null;
            }

            if (CheckInCollectionViewAspect != null) {
                CheckInCollectionViewAspect.Dispose ();
                CheckInCollectionViewAspect = null;
            }

            if (cnsMapTop != null) {
                cnsMapTop.Dispose ();
                cnsMapTop = null;
            }

            if (cnWebViewHeight != null) {
                cnWebViewHeight.Dispose ();
                cnWebViewHeight = null;
            }

            if (CollectionPeople != null) {
                CollectionPeople.Dispose ();
                CollectionPeople = null;
            }

            if (Content != null) {
                Content.Dispose ();
                Content = null;
            }

            if (CountPeopleLbl != null) {
                CountPeopleLbl.Dispose ();
                CountPeopleLbl = null;
            }

            if (DescriptionCount != null) {
                DescriptionCount.Dispose ();
                DescriptionCount = null;
            }

            if (HeaderLbl != null) {
                HeaderLbl.Dispose ();
                HeaderLbl = null;
            }

            if (MainContent != null) {
                MainContent.Dispose ();
                MainContent = null;
            }

            if (MainScroll != null) {
                MainScroll.Dispose ();
                MainScroll = null;
            }

            if (MapView != null) {
                MapView.Dispose ();
                MapView = null;
            }

            if (padding != null) {
                padding.Dispose ();
                padding = null;
            }

            if (SubmitButton != null) {
                SubmitButton.Dispose ();
                SubmitButton = null;
            }

            if (TimeDisLbl != null) {
                TimeDisLbl.Dispose ();
                TimeDisLbl = null;
            }

            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }
        }
    }
}