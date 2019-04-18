using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Mocks
{
   public static class MockGenerator
    {
        public static List<SummaryModel> GetSummaryModels()
        {
           var result = new List<SummaryModel>();

            result.Add(new ChallengeSummaryModel(1, 2)
            {
            });
            result.Add(new ChallengeTypeSummaryModel(3, 4, "new code type", "display name")
            {
            });
            result.Add(new RewardSummaryModel(5, 6, 7)
            {
            });

            return result;
        }
    }
}
