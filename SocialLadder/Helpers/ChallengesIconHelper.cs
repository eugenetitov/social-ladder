using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Helpers
{
    public static class ChallengesIconHelper
    {
        public static string LoadImages(string typeCode, string displayName, IPlatformAssetService assetService)
        {
            string icon = string.Empty;
            if (typeCode == ChallengesConstants.ChallengeCheckin || typeCode == ChallengesConstants.ChallengeCollateralTracking)
            {
                icon = assetService.PinIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeFacebook)
            {
                icon = assetService.FBLogoIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeFBEngagement)
            {
                icon = assetService.FBLogoIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeInsta)
            {
                icon = assetService.InstaIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeInvite && displayName == ChallengesConstants.ChallengeInviteToBuyDisplayNames)
            {
                icon = assetService.TicketIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeInvite && displayName == ChallengesConstants.ChallengeInviteToJoinDisplayNames)
            {
                icon = assetService.InviteIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeShare)
            {
                icon = assetService.BulhornIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengePostering)
            {
                icon = assetService.PosteringIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeFlyering)
            {
                icon = assetService.FlyeringIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeManual)
            {
                icon = assetService.ManualIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeMC)
            {
                icon = assetService.QuizIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeSignUp)
            {
                icon = assetService.SignupIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeDocsubmit)
            {
                icon = assetService.DocsubmitIconWhite;
            }
            if (typeCode == ChallengesConstants.ChallengeSubmit || typeCode == ChallengesConstants.ChallengeVerify)
            {
                icon = string.Empty;
            }
            return icon;
        }
    }
}
