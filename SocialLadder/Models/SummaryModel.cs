namespace SocialLadder.Models
{
    public class SummaryModel
    {
        public int Total { get; set; }
        public SummaryModel(int total)
        {
            Total = total;
        }
    }

    public class ChallengeSummaryModel : SummaryModel
    {
        public int ChallengeCompCount { get; set; }
        public ChallengeSummaryModel(int challengeCompCount, int total) : base(total)
        {
            ChallengeCompCount = challengeCompCount;
        }
    }

    public class ChallengeTypeSummaryModel : ChallengeSummaryModel
    {
        public string TypeCode { get; set; }
        public string DisplayName { get; set; }

        public ChallengeTypeSummaryModel(int challengeCompCount, int total, string typeCode, string displayName) : base(challengeCompCount, total)
        {
            TypeCode = typeCode;
            DisplayName = displayName;
        }
    }

    public class RewardSummaryModel : SummaryModel
    {
        public int UnlockedRewardsCount { get; set; }
        public int PurchasedRewardsCount { get; set; }
        public RewardSummaryModel(int unlockedRewardsCount, int purchasedRewardsCount, int total) : base(total)
        {
            UnlockedRewardsCount = unlockedRewardsCount;
            PurchasedRewardsCount = purchasedRewardsCount;
        }
    }
}