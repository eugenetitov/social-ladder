using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Logger
{
    public static class LogHelper
    {
        public static void LogChallengeSubmition(string challengeID, string challengeName)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"Action","CLICKED" },
                {"ChallengeID",challengeID },
                {"ChallengeName", challengeName }
            };
            SL.Manager.LogUserEvent("CHALLENGE", parameters);
        }

        public static void LogRewardSubmition(string rewardId, string rewardName)
        {
            var parameters = new Dictionary<string, string>()
            {
                {"Action","CLICKED" },
                {"RewardID",rewardId },
                {"RewardName", rewardName }
            };
            SL.Manager.LogUserEvent("OFFER", parameters);
        }

        public static void LogUserMessage(string type, string message)
        {
            var paramenters = new Dictionary<string, string>()
            {
                { "Error", message }
            };


            SL.Manager.LogUserEvent(type, paramenters);
        }
    }
}
