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

namespace SocialLadder.iOS.More
{
    [Register ("PrivacyPolicyViewController")]
    partial class PrivacyPolicyViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint PrivacyHeightToolBarConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView PrivacyToolBarContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        WebKit.WKWebView WebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PrivacyHeightToolBarConstraint != null) {
                PrivacyHeightToolBarConstraint.Dispose ();
                PrivacyHeightToolBarConstraint = null;
            }

            if (PrivacyToolBarContainer != null) {
                PrivacyToolBarContainer.Dispose ();
                PrivacyToolBarContainer = null;
            }

            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }
        }
    }
}