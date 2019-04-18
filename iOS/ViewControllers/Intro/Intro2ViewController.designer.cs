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
    [Register ("Intro2ViewController")]
    partial class Intro2ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img_left { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView img_Right { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbl_Left { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lbl_Right { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (img_left != null) {
                img_left.Dispose ();
                img_left = null;
            }

            if (img_Right != null) {
                img_Right.Dispose ();
                img_Right = null;
            }

            if (lbl_Left != null) {
                lbl_Left.Dispose ();
                lbl_Left = null;
            }

            if (lbl_Right != null) {
                lbl_Right.Dispose ();
                lbl_Right = null;
            }
        }
    }
}