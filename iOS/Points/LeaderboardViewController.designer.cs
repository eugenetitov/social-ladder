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

namespace SocialLadder.iOS.Points
{
    [Register ("LeaderboardViewController")]
    partial class LeaderboardViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Areas.AreaCollectionView AreaCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AreaCollectionHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDropDown { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChallengesButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnBgCenterX { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsAreaCollectionTop { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView EmptyView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivLeaderboardDropdown { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivTriangle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLedaerboardCollectionTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblProfileLeaderboardPosition { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.SLButton LeaderboardButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView leaderboardTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PointsAndStatsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProfileImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TableViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TransactionsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView tvLeaderboardFilterTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.LeaderboardTableView tvLeaderboardTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vChallengeButtonInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContentBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vDropDownButtonInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vLeaderboardDropdown { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vLeaderboardFilterTableVInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vLeaderboardTableInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vProfileImageInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vProfileLeaderboardPositionInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTriangleAlignment { get; set; }

        [Action ("ButtonDropDown_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ButtonDropDown_TouchUpInside (UIKit.UIButton sender);

        [Action ("ChallengesButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ChallengesButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("LeaderboardButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LeaderboardButton_TouchUpInside (SocialLadder.iOS.SLButton sender);

        [Action ("PointsAndStatsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PointsAndStatsButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TransactionsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TransactionsButton_TouchUpInside (UIKit.UIButton sender);

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

            if (btnDropDown != null) {
                btnDropDown.Dispose ();
                btnDropDown = null;
            }

            if (ChallengesButton != null) {
                ChallengesButton.Dispose ();
                ChallengesButton = null;
            }

            if (cnBgCenterX != null) {
                cnBgCenterX.Dispose ();
                cnBgCenterX = null;
            }

            if (cnsAreaCollectionTop != null) {
                cnsAreaCollectionTop.Dispose ();
                cnsAreaCollectionTop = null;
            }

            if (EmptyView != null) {
                EmptyView.Dispose ();
                EmptyView = null;
            }

            if (ivBackground != null) {
                ivBackground.Dispose ();
                ivBackground = null;
            }

            if (ivLeaderboardDropdown != null) {
                ivLeaderboardDropdown.Dispose ();
                ivLeaderboardDropdown = null;
            }

            if (ivTriangle != null) {
                ivTriangle.Dispose ();
                ivTriangle = null;
            }

            if (lblLedaerboardCollectionTitle != null) {
                lblLedaerboardCollectionTitle.Dispose ();
                lblLedaerboardCollectionTitle = null;
            }

            if (lblProfileLeaderboardPosition != null) {
                lblProfileLeaderboardPosition.Dispose ();
                lblProfileLeaderboardPosition = null;
            }

            if (LeaderboardButton != null) {
                LeaderboardButton.Dispose ();
                LeaderboardButton = null;
            }

            if (leaderboardTextView != null) {
                leaderboardTextView.Dispose ();
                leaderboardTextView = null;
            }

            if (PointsAndStatsButton != null) {
                PointsAndStatsButton.Dispose ();
                PointsAndStatsButton = null;
            }

            if (ProfileImage != null) {
                ProfileImage.Dispose ();
                ProfileImage = null;
            }

            if (ScrollView != null) {
                ScrollView.Dispose ();
                ScrollView = null;
            }

            if (TableViewHeight != null) {
                TableViewHeight.Dispose ();
                TableViewHeight = null;
            }

            if (TransactionsButton != null) {
                TransactionsButton.Dispose ();
                TransactionsButton = null;
            }

            if (tvLeaderboardFilterTable != null) {
                tvLeaderboardFilterTable.Dispose ();
                tvLeaderboardFilterTable = null;
            }

            if (tvLeaderboardTable != null) {
                tvLeaderboardTable.Dispose ();
                tvLeaderboardTable = null;
            }

            if (vChallengeButtonInset != null) {
                vChallengeButtonInset.Dispose ();
                vChallengeButtonInset = null;
            }

            if (vContent != null) {
                vContent.Dispose ();
                vContent = null;
            }

            if (vContentBackground != null) {
                vContentBackground.Dispose ();
                vContentBackground = null;
            }

            if (vDropDownButtonInset != null) {
                vDropDownButtonInset.Dispose ();
                vDropDownButtonInset = null;
            }

            if (vLeaderboardDropdown != null) {
                vLeaderboardDropdown.Dispose ();
                vLeaderboardDropdown = null;
            }

            if (vLeaderboardFilterTableVInset != null) {
                vLeaderboardFilterTableVInset.Dispose ();
                vLeaderboardFilterTableVInset = null;
            }

            if (vLeaderboardTableInset != null) {
                vLeaderboardTableInset.Dispose ();
                vLeaderboardTableInset = null;
            }

            if (vProfileImageInset != null) {
                vProfileImageInset.Dispose ();
                vProfileImageInset = null;
            }

            if (vProfileLeaderboardPositionInset != null) {
                vProfileLeaderboardPositionInset.Dispose ();
                vProfileLeaderboardPositionInset = null;
            }

            if (vTriangleAlignment != null) {
                vTriangleAlignment.Dispose ();
                vTriangleAlignment = null;
            }
        }
    }
}