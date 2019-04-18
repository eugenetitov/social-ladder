// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.Areas
{
    [Register ("AreaCollectionViewCell")]
    partial class AreaCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView AreaImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AreaName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Score { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ScoreImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AreaImage != null) {
                AreaImage.Dispose ();
                AreaImage = null;
            }

            if (AreaName != null) {
                AreaName.Dispose ();
                AreaName = null;
            }

            if (Score != null) {
                Score.Dispose ();
                Score = null;
            }

            if (ScoreImage != null) {
                ScoreImage.Dispose ();
                ScoreImage = null;
            }
        }
    }
}