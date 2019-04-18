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
    [Register ("TermsServiceViewController")]
    partial class TermsServiceViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TermsHeightToolBarConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TermsToolBarContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        WebKit.WKWebView WebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TermsHeightToolBarConstraint != null) {
                TermsHeightToolBarConstraint.Dispose ();
                TermsHeightToolBarConstraint = null;
            }

            if (TermsToolBarContainer != null) {
                TermsToolBarContainer.Dispose ();
                TermsToolBarContainer = null;
            }

            if (WebView != null) {
                WebView.Dispose ();
                WebView = null;
            }
        }
    }
}