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
    [Register ("TransactionsViewController")]
    partial class TransactionsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Areas.AreaCollectionView AreaCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AreaCollectionHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDoMoreChallenges { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSeeRewards { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnRightBgCenterX { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsAreaCollectionTop { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsTableViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Content { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivChallengesImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivRewardsImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivRightBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivScoreImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivTriangle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblChallengesTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblChallengesValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblRewardsTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblRewarsValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblScoreValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTransactionHistoryTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LeaderboardButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PointsAndStatsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.TransactionsTableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.SLButton TransactionsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vButtonsLeftAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vChallengesSectionAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContentBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vLabelsLeftAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vRewardsSectionVAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vScoreImageAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vScoreValueAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTableVAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTransactionsHistoryTitleVAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTriangleAlign { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTriangleBoard { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vVerticalScoreImageLine { get; set; }

        [Action ("btnChallenges_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnChallenges_TouchUpInside (UIKit.UIButton sender);

        [Action ("btnRewards_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnRewards_TouchUpInside (UIKit.UIButton sender);

        [Action ("LeaderboardButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void LeaderboardButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("PointsAndStatsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PointsAndStatsButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TransactionsButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TransactionsButton_TouchUpInside (SocialLadder.iOS.SLButton sender);

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

            if (btnDoMoreChallenges != null) {
                btnDoMoreChallenges.Dispose ();
                btnDoMoreChallenges = null;
            }

            if (btnSeeRewards != null) {
                btnSeeRewards.Dispose ();
                btnSeeRewards = null;
            }

            if (cnRightBgCenterX != null) {
                cnRightBgCenterX.Dispose ();
                cnRightBgCenterX = null;
            }

            if (cnsAreaCollectionTop != null) {
                cnsAreaCollectionTop.Dispose ();
                cnsAreaCollectionTop = null;
            }

            if (cnsTableViewHeight != null) {
                cnsTableViewHeight.Dispose ();
                cnsTableViewHeight = null;
            }

            if (Content != null) {
                Content.Dispose ();
                Content = null;
            }

            if (ivChallengesImage != null) {
                ivChallengesImage.Dispose ();
                ivChallengesImage = null;
            }

            if (ivRewardsImage != null) {
                ivRewardsImage.Dispose ();
                ivRewardsImage = null;
            }

            if (ivRightBackground != null) {
                ivRightBackground.Dispose ();
                ivRightBackground = null;
            }

            if (ivScoreImage != null) {
                ivScoreImage.Dispose ();
                ivScoreImage = null;
            }

            if (ivTriangle != null) {
                ivTriangle.Dispose ();
                ivTriangle = null;
            }

            if (lblChallengesTitle != null) {
                lblChallengesTitle.Dispose ();
                lblChallengesTitle = null;
            }

            if (lblChallengesValue != null) {
                lblChallengesValue.Dispose ();
                lblChallengesValue = null;
            }

            if (lblRewardsTitle != null) {
                lblRewardsTitle.Dispose ();
                lblRewardsTitle = null;
            }

            if (lblRewarsValue != null) {
                lblRewarsValue.Dispose ();
                lblRewarsValue = null;
            }

            if (lblScoreValue != null) {
                lblScoreValue.Dispose ();
                lblScoreValue = null;
            }

            if (lblTransactionHistoryTitle != null) {
                lblTransactionHistoryTitle.Dispose ();
                lblTransactionHistoryTitle = null;
            }

            if (LeaderboardButton != null) {
                LeaderboardButton.Dispose ();
                LeaderboardButton = null;
            }

            if (PointsAndStatsButton != null) {
                PointsAndStatsButton.Dispose ();
                PointsAndStatsButton = null;
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

            if (vButtonsLeftAlign != null) {
                vButtonsLeftAlign.Dispose ();
                vButtonsLeftAlign = null;
            }

            if (vChallengesSectionAlign != null) {
                vChallengesSectionAlign.Dispose ();
                vChallengesSectionAlign = null;
            }

            if (vContentBackground != null) {
                vContentBackground.Dispose ();
                vContentBackground = null;
            }

            if (vLabelsLeftAlign != null) {
                vLabelsLeftAlign.Dispose ();
                vLabelsLeftAlign = null;
            }

            if (vRewardsSectionVAlign != null) {
                vRewardsSectionVAlign.Dispose ();
                vRewardsSectionVAlign = null;
            }

            if (vScoreImageAlign != null) {
                vScoreImageAlign.Dispose ();
                vScoreImageAlign = null;
            }

            if (vScoreValueAlign != null) {
                vScoreValueAlign.Dispose ();
                vScoreValueAlign = null;
            }

            if (vTableVAlign != null) {
                vTableVAlign.Dispose ();
                vTableVAlign = null;
            }

            if (vTransactionsHistoryTitleVAlign != null) {
                vTransactionsHistoryTitleVAlign.Dispose ();
                vTransactionsHistoryTitleVAlign = null;
            }

            if (vTriangleAlign != null) {
                vTriangleAlign.Dispose ();
                vTriangleAlign = null;
            }

            if (vTriangleBoard != null) {
                vTriangleBoard.Dispose ();
                vTriangleBoard = null;
            }

            if (vVerticalScoreImageLine != null) {
                vVerticalScoreImageLine.Dispose ();
                vVerticalScoreImageLine = null;
            }
        }
    }
}