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

namespace SocialLadder.iOS.ViewControllers.Intro
{
    [Register ("Intro1ViewController")]
    partial class Intro1ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img_Logo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbl_SocialLadder { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbl_Welcome { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (img_Logo != null) {
                img_Logo.Dispose ();
                img_Logo = null;
            }

            if (lbl_SocialLadder != null) {
                lbl_SocialLadder.Dispose ();
                lbl_SocialLadder = null;
            }

            if (lbl_Welcome != null) {
                lbl_Welcome.Dispose ();
                lbl_Welcome = null;
            }
        }
    }
}