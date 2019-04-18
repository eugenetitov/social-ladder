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
    [Register ("MoreTableViewCell")]
    partial class MoreTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MenuImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MenuText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MenuImage != null) {
                MenuImage.Dispose ();
                MenuImage = null;
            }

            if (MenuText != null) {
                MenuText.Dispose ();
                MenuText = null;
            }
        }
    }
}