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
    [Register ("FacebookEngagementViewController")]
    partial class FacebookEngagementViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionShareLinkLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView EngagementLink { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LinksView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView ShareText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DescriptionShareLinkLbl != null) {
                DescriptionShareLinkLbl.Dispose ();
                DescriptionShareLinkLbl = null;
            }

            if (EngagementLink != null) {
                EngagementLink.Dispose ();
                EngagementLink = null;
            }

            if (LinksView != null) {
                LinksView.Dispose ();
                LinksView = null;
            }

            if (ShareText != null) {
                ShareText.Dispose ();
                ShareText = null;
            }
        }
    }
}