using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.PlatformService
{
    public class PlatformAssetService : IPlatformAssetService
    {
        public string SmallScoreIcon => "small_score_icon";
        public string NetworksScoreImage => "SL_cricle_icon";
        public string NetworksScoreImageBold => "SL_cricle_icon_boldWhite";
        public string FBUnconnectedIcon => "fb_unconnected";
        public string FBConnectedIcon => "fb_connected";
        public string NetworkConnectedLoaderImage => "connect_social";
        public string NetworkConnectingLoaderImage => "ic_loadingIndicator";
        public string TwitterUnconnectedIcon => "twitter_unconnected";
        public string TwitterConnectedIcon => "twitter_connected";
        public string InstaUnconnectedIcon => "insta_unconnected";
        public string InstaConnectedIcon => "insta_connected";
        public string NetworksScore33Per_Image => "score_33";
        public string NetworksScore66Per_Image => "score_66";
        public string NetworksScore100Per_Image => "score_100";
        public string IconScoreTransactions => "IconScoreTransactions";

        public string PinIconWhite => "icons_pin_icon_white";
        public string FBLogoIconWhite => "fb_logo_white";
        public string InstaIconWhite => "icons_insta_icon_white";
        public string TicketIconWhite => "Icons_ticket_icon_off";
        public string InviteIconWhite => "icons_invite_icon_white";
        public string BulhornIconWhite => "icons_bullhorn_icon_white";
        public string PosteringIconWhite => "postering_icon_white";
        public string FlyeringIconWhite => "flyering_icon_white";
        public string ManualIconWhite => "manual_icon_white";
        public string QuizIconWhite => "icons_quiz_icon_white";
        public string SignupIconWhite => "signup_icon_white";
        public string DocsubmitIconWhite => "docsubmit_icon_white";
        public string TwitterIconWhite => "icons_twitter_white";
        

        public string ChallengesShareButton => "challenge_share_btn";
        public string ChallengesFacebookButton => "challenges_fb_btn";
        public string ChallengesInviteButton => "challenge_invite_btn";

        
    }
}