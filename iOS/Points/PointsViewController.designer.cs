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
    [Register ("PointsViewController")]
    partial class PointsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Areas.AreaCollectionView AreaCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AreaCollectionHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BottomTabView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnBgCenterX { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsAreaCollectionTop { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsTableViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivProfileImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivTriangle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblScoreGroup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LeaderboardButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.SLButton PointsAndStatsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Score { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ScoreImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.SLScoreView ScoreView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.PointsTableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TransactionsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vBgImgOverlay { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContentBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vScoreimageInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vScoreLabelInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vScoreViewAlignment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vScoreViewFooterAlignment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vScoreViewSplitter { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTriangleBorder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTriangleBottomAlign { get; set; }

        [Action ("LeaderboardButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LeaderboardButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("LeaderboardButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LeaderboardButton_TouchUpInside (SocialLadder.iOS.Points.CustomViews.PointsTabButton sender);

        [Action ("PointsAndStatsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PointsAndStatsButton_TouchUpInside (SocialLadder.iOS.SLButton sender);

        [Action ("PointsAndStatsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PointsAndStatsButton_TouchUpInside (SocialLadder.iOS.Points.CustomViews.PointsTabButton sender);

        [Action ("TransactionsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TransactionsButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TransactionsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TransactionsButton_TouchUpInside (SocialLadder.iOS.Points.CustomViews.PointsTabButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AreaCollection != null) {
                AreaCollection.Dispose ();
                AreaCollection = null;
            }

            if (AreaCollectionHeight != null) {
                AreaCollectionHeight.Dispose ();
                AreaCollectionHeight = null;
            }

            if (BottomTabView != null) {
                BottomTabView.Dispose ();
                BottomTabView = null;
            }

            if (btnInfo != null) {
                btnInfo.Dispose ();
                btnInfo = null;
            }

            if (cnBgCenterX != null) {
                cnBgCenterX.Dispose ();
                cnBgCenterX = null;
            }

            if (cnsAreaCollectionTop != null) {
                cnsAreaCollectionTop.Dispose ();
                cnsAreaCollectionTop = null;
            }

            if (cnsTableViewHeight != null) {
                cnsTableViewHeight.Dispose ();
                cnsTableViewHeight = null;
            }

            if (ivBackground != null) {
                ivBackground.Dispose ();
                ivBackground = null;
            }

            if (ivProfileImage != null) {
                ivProfileImage.Dispose ();
                ivProfileImage = null;
            }

            if (ivTriangle != null) {
                ivTriangle.Dispose ();
                ivTriangle = null;
            }

            if (lblScoreGroup != null) {
                lblScoreGroup.Dispose ();
                lblScoreGroup = null;
            }

            if (LeaderboardButton != null) {
                LeaderboardButton.Dispose ();
                LeaderboardButton = null;
            }

            if (PointsAndStatsButton != null) {
                PointsAndStatsButton.Dispose ();
                PointsAndStatsButton = null;
            }

            if (Score != null) {
                Score.Dispose ();
                Score = null;
            }

            if (ScoreImage != null) {
                ScoreImage.Dispose ();
                ScoreImage = null;
            }

            if (ScoreView != null) {
                ScoreView.Dispose ();
                ScoreView = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }

            if (TransactionsButton != null) {
                TransactionsButton.Dispose ();
                TransactionsButton = null;
            }

            if (vBgImgOverlay != null) {
                vBgImgOverlay.Dispose ();
                vBgImgOverlay = null;
            }

            if (vContent != null) {
                vContent.Dispose ();
                vContent = null;
            }

            if (vContentBackground != null) {
                vContentBackground.Dispose ();
                vContentBackground = null;
            }

            if (vScoreimageInset != null) {
                vScoreimageInset.Dispose ();
                vScoreimageInset = null;
            }

            if (vScoreLabelInset != null) {
                vScoreLabelInset.Dispose ();
                vScoreLabelInset = null;
            }

            if (vScoreViewAlignment != null) {
                vScoreViewAlignment.Dispose ();
                vScoreViewAlignment = null;
            }

            if (vScoreViewFooterAlignment != null) {
                vScoreViewFooterAlignment.Dispose ();
                vScoreViewFooterAlignment = null;
            }

            if (vScoreViewSplitter != null) {
                vScoreViewSplitter.Dispose ();
                vScoreViewSplitter = null;
            }

            if (vTriangleBorder != null) {
                vTriangleBorder.Dispose ();
                vTriangleBorder = null;
            }

            if (vTriangleBottomAlign != null) {
                vTriangleBottomAlign.Dispose ();
                vTriangleBottomAlign = null;
            }
        }
    }
}