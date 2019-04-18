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

namespace SocialLadder.iOS.Connectivity
{
    [Register ("ConnectionMessageView")]
    partial class ConnectionMessageView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DismissButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MessageLb { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DismissButton != null) {
                DismissButton.Dispose ();
                DismissButton = null;
            }

            if (MessageLb != null) {
                MessageLb.Dispose ();
                MessageLb = null;
            }
        }
    }
}