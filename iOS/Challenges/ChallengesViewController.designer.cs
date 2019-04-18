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
    [Register ("ChallengesViewController")]
    partial class ChallengesViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Areas.AreaCollectionView AreaCollection { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AreaCollectionHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnCollectionViewHeight { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint cnsAreaCollectionTop { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Challenges.ChallengeCollectionView CollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView EmptyCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        SocialLadder.iOS.Challenges.ChallengesTableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ViewForImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AreaCollection != null) {
                AreaCollection.Dispose ();
                AreaCollection = null;
            }

            if (AreaCollectionHeight != null) {
                AreaCollectionHeight.Dispose ();
                AreaCollectionHeight = null;
            }

            if (cnCollectionViewHeight != null) {
                cnCollectionViewHeight.Dispose ();
                cnCollectionViewHeight = null;
            }

            if (cnsAreaCollectionTop != null) {
                cnsAreaCollectionTop.Dispose ();
                cnsAreaCollectionTop = null;
            }

            if (CollectionView != null) {
                CollectionView.Dispose ();
                CollectionView = null;
            }

            if (EmptyCollectionView != null) {
                EmptyCollectionView.Dispose ();
                EmptyCollectionView = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }

            if (ViewForImage != null) {
                ViewForImage.Dispose ();
                ViewForImage = null;
            }
        }
    }
}