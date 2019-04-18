// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.Challenges
{
    [Register ("MultipleChoiceTableViewCell")]
    partial class MultipleChoiceTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ChoiceName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ImgBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView SelectedImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ChoiceName != null) {
                ChoiceName.Dispose ();
                ChoiceName = null;
            }

            if (ImgBackground != null) {
                ImgBackground.Dispose ();
                ImgBackground = null;
            }

            if (SelectedImage != null) {
                SelectedImage.Dispose ();
                SelectedImage = null;
            }
        }
    }
}