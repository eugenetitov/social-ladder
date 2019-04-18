// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.Points
{
    [Register ("PointsTableViewCell")]
    partial class PointsTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView ProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Text { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Text2 { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vContentInsets { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vLabelsInset { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vTextLabelInset { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Image != null) {
                Image.Dispose ();
                Image = null;
            }

            if (ProgressBar != null) {
                ProgressBar.Dispose ();
                ProgressBar = null;
            }

            if (Text != null) {
                Text.Dispose ();
                Text = null;
            }

            if (Text2 != null) {
                Text2.Dispose ();
                Text2 = null;
            }

            if (vContent != null) {
                vContent.Dispose ();
                vContent = null;
            }

            if (vContentInsets != null) {
                vContentInsets.Dispose ();
                vContentInsets = null;
            }

            if (vLabelsInset != null) {
                vLabelsInset.Dispose ();
                vLabelsInset = null;
            }

            if (vTextLabelInset != null) {
                vTextLabelInset.Dispose ();
                vTextLabelInset = null;
            }
        }
    }
}