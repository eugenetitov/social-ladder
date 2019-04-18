// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.Points.CustomViews
{
    [Register ("RankInfoView")]
    partial class RankInfoView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnClose { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblRankInfoTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView txtvRankInfoDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vBasis { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Points.SLScoreView vScoreInfo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnClose != null) {
                btnClose.Dispose ();
                btnClose = null;
            }

            if (lblRankInfoTitle != null) {
                lblRankInfoTitle.Dispose ();
                lblRankInfoTitle = null;
            }

            if (txtvRankInfoDescription != null) {
                txtvRankInfoDescription.Dispose ();
                txtvRankInfoDescription = null;
            }

            if (vBasis != null) {
                vBasis.Dispose ();
                vBasis = null;
            }

            if (vScoreInfo != null) {
                vScoreInfo.Dispose ();
                vScoreInfo = null;
            }
        }
    }
}