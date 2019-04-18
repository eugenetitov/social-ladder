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
    [Register ("ChallengeCollectionViewCell")]
    partial class ChallengeCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Background { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BadgeView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChallengeImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ChallengeName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIProgressView ChallengeProgressBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ChallengeProgressText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Background != null) {
                Background.Dispose ();
                Background = null;
            }

            if (BadgeView != null) {
                BadgeView.Dispose ();
                BadgeView = null;
            }

            if (ChallengeImage != null) {
                ChallengeImage.Dispose ();
                ChallengeImage = null;
            }

            if (ChallengeName != null) {
                ChallengeName.Dispose ();
                ChallengeName = null;
            }

            if (ChallengeProgressBar != null) {
                ChallengeProgressBar.Dispose ();
                ChallengeProgressBar = null;
            }

            if (ChallengeProgressText != null) {
                ChallengeProgressText.Dispose ();
                ChallengeProgressText = null;
            }
        }
    }
}