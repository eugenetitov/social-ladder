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
    [Register ("LeaderboardFilterTableCell")]
    partial class LeaderboardFilterTableCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblFilterName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vBasis { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblFilterName != null) {
                lblFilterName.Dispose ();
                lblFilterName = null;
            }

            if (vBasis != null) {
                vBasis.Dispose ();
                vBasis = null;
            }
        }
    }
}