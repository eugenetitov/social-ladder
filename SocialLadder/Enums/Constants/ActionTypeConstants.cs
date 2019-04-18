using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Enums.Constants
{

    public static class ActionTypeConstants
    {
        #region ActionTypes
        public static string ChallengeActionType = "CHALLENGE";
        public static string ReferralAcceptedActionType = "REFERRAL_ACCEPTED";
        public static string ItbCompletedActionType = "ITB_COMPLETE";
        public static string CtacActionType = "CTAC";
        public static string DialogActionType = "DIALOG";
        public static string RewardActionType = "REWARD";
        public static string RewardListActionType = "REWARDLIST";
        public static string ChallengeListActionType = "CHALLENGELIST";
        public static string FeedActionType = "FEED";
        public static string AggChalcompActionType = "AGG_CHALCOMP";
        public static string AggCommitActionType = "AGG_COMMIT";
        public static string WebActionType = "WEB";
        public static string SettingsActionType = "SETTINGS";
        public static string ShareActionType = "SHARE";
        public static string SwitchAreaActionType = "SwitchArea";
        public static string InviteActionType = "INVITE";
        public static string IWebActionType = "IWEB";
        public static string CtaiwActionType = "CTAIW";
        public static string ScoreActionType = "SCORE";
        public static string FriendListActionType = "FRIENDLIST";
        //public static string ActionType = "";
        #endregion
        #region ActionParamDict
        public static string DialogStyleActionParamDict = "DialogStyle";
        public static string ChallengeDetailURLActionParamDict = "ChallengeDetailURL";
        public static string WebRequestURLActionParamDict = "WebRequestURL";
        public static string FeedURLActionParamDict = "FeedURL";
        public static string RewardDetailURLActionParamDict = "RewardDetailURL";
        public static string SubmissionURLActionParamDict = "SubmissionURL";
        public static string CaptionActionParamDict = "Caption";
        public static string ShareTemplateURLActionParamDict = "ShareTemplateURL";
        #endregion
        #region DialogStyles
        public static string NoneDialogStyle = "NONE";
        public static string EntryDialogStyle = "ENTRY";
        #endregion
        #region RewardTypes
        public static string RewardRewardType = "REWARD";
        public static string CategoryRewardType = "CATEGORY";
        #endregion
    }
}
