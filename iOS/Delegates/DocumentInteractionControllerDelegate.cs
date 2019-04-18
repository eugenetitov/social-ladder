using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.iOS.Challenges;
using UIKit;

namespace SocialLadder.iOS.Delegates
{
    public class DocumentInteractionControllerDelegate : UIDocumentInteractionControllerDelegate
    {
        InstagramViewController InstagramViewController
        {
            get; set;
        }
        public DocumentInteractionControllerDelegate(InstagramViewController instagramViewController)
        {
            InstagramViewController = instagramViewController;
        }

        public override void DidEndSendingToApplication(UIDocumentInteractionController controller, string application)
        {
            //ChallengeModel challenge = InstagramViewController.Challenge;
            //challenge.Status = "Pending";
            //SL.Manager.UpdateChallenge(challenge, challenge.ID);
            InstagramViewController.SendToInstagramComplete();
        }
    }
}