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

namespace SocialLadder.iOS.UserInfo
{
    [Register ("UserInfoViewController")]
    partial class UserInfoViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnAddRemoveFriend { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnClose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivScoreImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ivUserImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblFriendsInfo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblRankCategory { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblScoreValue { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblUserName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Views.FeedTableView tvFeedItems { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vBasis { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vUserInfoHeader { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnAddRemoveFriend != null) {
                btnAddRemoveFriend.Dispose ();
                btnAddRemoveFriend = null;
            }

            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (ivScoreImage != null) {
                ivScoreImage.Dispose ();
                ivScoreImage = null;
            }

            if (ivUserImage != null) {
                ivUserImage.Dispose ();
                ivUserImage = null;
            }

            if (lblFriendsInfo != null) {
                lblFriendsInfo.Dispose ();
                lblFriendsInfo = null;
            }

            if (lblRankCategory != null) {
                lblRankCategory.Dispose ();
                lblRankCategory = null;
            }

            if (lblScoreValue != null) {
                lblScoreValue.Dispose ();
                lblScoreValue = null;
            }

            if (lblUserName != null) {
                lblUserName.Dispose ();
                lblUserName = null;
            }

            if (tvFeedItems != null) {
                tvFeedItems.Dispose ();
                tvFeedItems = null;
            }

            if (vBasis != null) {
                vBasis.Dispose ();
                vBasis = null;
            }

            if (vUserInfoHeader != null) {
                vUserInfoHeader.Dispose ();
                vUserInfoHeader = null;
            }
        }
    }
}