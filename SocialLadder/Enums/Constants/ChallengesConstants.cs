using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Enums.Constants
{
    public static class ChallengesConstants
    {
        #region TypeCodes
        public static readonly string ChallengeCheckin = "CHECKIN";
        public static readonly string ChallengeCollateralTracking = "COLLATERAL TRACKING";
        public static readonly string ChallengeFBEngagement = "FB ENGAGEMENT";
        public static readonly string ChallengeInsta = "INSTA";
        public static readonly string ChallengeInvite = "INVITE";
        public static readonly string ChallengeShare = "SHARE";
        public static readonly string ChallengePostering = "POSTERING";
        public static readonly string ChallengeFlyering = "FLYERING";
        public static readonly string ChallengeManual = "MANUAL";
        public static readonly string ChallengeMC = "MC";
        public static readonly string ChallengeSignUp = "SIGNUP";
        public static readonly string ChallengeSubmit = "SUBMIT";
        public static readonly string ChallengeVerify = "VERIFY";
        public static readonly string ChallengeFacebook = "FACEBOOK";
        public static readonly string ChallengeDocsubmit = "DOCSUBMIT";
        public static readonly string ChallengeItb = "ITB";
        #endregion
        #region DisplayNames        
        public static readonly string ChallengeTwitterDisplayNames = "Twitter";
        public static readonly string ChallengeFacebookDisplayNames = "Facebook";
        public static readonly string ChallengeInviteToBuyDisplayNames = "Invite to Buy";
        public static readonly string ChallengeInviteToJoinDisplayNames = "Invite to Join";
        #endregion
        #region PhotoLibrary
        public static string Camera = "Camera";
        public static string Library = "Library";
        public static string SendToInstagram = "Send to Instagram";
        public static string Cancel = "Cancel";
        #endregion
        #region InviteMenu
        public static string WhatsApp = "WhatsApp";
        public static string SMS = "SMS";
        #endregion
        #region ChallengeStatus
        public static string ChallengeStatusPending = "Pending";
        public static string ChallengeStatusComplete = "Complete";
        #endregion
        #region ChallengesDetailsViewBackground
        public static string ChallengesDetailsMCBackground = "#24D1B4";
        public static string ChallengesDetailsInstaBackground = "#000000";
        public static string ChallengesDetailsFailBackground = "#F07275";
        #endregion
        #region DetailsText
        public static string ChallengesTwitterDetailsMessageText = "It looks like you don't have a Twitter account connected - please head to the More tab and connect your account!";
        #endregion
        #region Other
        public static string ChallengesChackNotificationsMessageText = "Challenges expire quickly - allow this app to send you push notifications to be notified about special opportunities";
        public static string ChallengesCompleteSuccessText = "Congratulations! Challenge Complete!";
        public static string ChallengesCompleteUnsuccessText = "Challenge uncomplete!";
        public static string ChallengesCheckinUnsuccessText = "Hey are you really there? Doesn't seem like it...";
        public static string ChallengesCompletePointsText = " pts added to your account";
        public static string PosteringImageHasNotLocationText = "This image doesn't contain location data, therefore it can't be submitted.";
        #endregion
    }
}
