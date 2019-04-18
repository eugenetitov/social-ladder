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
    [Register ("TransactionsTableViewCell")]
    partial class TransactionsTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblTransactionText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PointsText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView TransactionImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView vBasis { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblTransactionText != null) {
                lblTransactionText.Dispose ();
                lblTransactionText = null;
            }

            if (PointsText != null) {
                PointsText.Dispose ();
                PointsText = null;
            }

            if (TimeText != null) {
                TimeText.Dispose ();
                TimeText = null;
            }

            if (TransactionImage != null) {
                TransactionImage.Dispose ();
                TransactionImage = null;
            }

            if (vBasis != null) {
                vBasis.Dispose ();
                vBasis = null;
            }
        }
    }
}