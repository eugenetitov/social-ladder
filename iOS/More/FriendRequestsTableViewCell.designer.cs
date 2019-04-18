// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.More
{
    [Register ("FriendRequestsTableViewCell")]
    partial class FriendRequestsTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ActionText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ActorImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnConfirm { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnDelete { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CreationDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView separatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActionText != null) {
                ActionText.Dispose ();
                ActionText = null;
            }

            if (ActorImage != null) {
                ActorImage.Dispose ();
                ActorImage = null;
            }

            if (btnConfirm != null) {
                btnConfirm.Dispose ();
                btnConfirm = null;
            }

            if (btnDelete != null) {
                btnDelete.Dispose ();
                btnDelete = null;
            }

            if (CreationDate != null) {
                CreationDate.Dispose ();
                CreationDate = null;
            }

            if (separatorView != null) {
                separatorView.Dispose ();
                separatorView = null;
            }
        }
    }
}