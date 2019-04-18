using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.Interfaces;
using UIKit;

namespace SocialLadder.iOS.PlatformServices
{
    public class PlatformAssetService : IPlatformAssetService
    {
        public string SmallScoreIcon => "small-score-icon";
        public string NetworksScoreImage => "SLCircleLogo";
        public string NetworksScoreImageBold => "SLCircleLogo";
        public string FBUnconnectedIcon => "social-connect_fb-unconnected";
        public string FBConnectedIcon => "social-connect_fb-connected";
        public string TwitterUnconnectedIcon => "social-connect_twitter-unconnected";
        public string TwitterConnectedIcon => "social-connect_twitter-connected";
        public string InstaUnconnectedIcon => "social-connect_insta-connected";
        public string InstaConnectedIcon => "social-connect_insta-connected";
        public string NetworkConnectedLoaderImage => "check-icon_green";
        public string NetworkConnectingLoaderImage => "loading-indicator";
        public string NetworksScore33Per_Image => "on-board-33";
        public string NetworksScore66Per_Image => "on-board-66";
        public string NetworksScore100Per_Image => "on-board-100";
        public string IconScoreTransactions => "IconScoreTransactions";

        public string PinIconWhite => "location-icon_large";
        public string FBLogoIconWhite => "fb-logo";
        public string InstaIconWhite => "insta-icon_white";
        public string TicketIconWhite => "ticket-icon_white";
        public string InviteIconWhite => "invite-icon_white";
        public string BulhornIconWhite => "bullhorn_icon_white";
        public string PosteringIconWhite => "postering-icon_large";
        public string FlyeringIconWhite => "flyering-icon_large";
        public string ManualIconWhite => "manual-icon_large";
        public string QuizIconWhite => "quiz-icon_white";
        public string SignupIconWhite => "signup-icon_large";

        public string ChallengesShareButton => "challenge-btn_share";
        public string ChallengesFacebookButton => "challenge-btn_fb";
        public string ChallengesInviteButton => "challenge-btn_itb";
        public string DocsubmitIconWhite => "docsubmit";
        public string TwitterIconWhite => "twitter-logo_white";
    }
}