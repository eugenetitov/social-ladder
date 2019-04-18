using System;
using SocialLadder.Models;
using Foundation;
using UIKit;
using SocialLadder.iOS.Constraints;

namespace SocialLadder.iOS.Points
{
    public partial class PointsTableViewCell : UITableViewCell
    {
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;

        public static readonly string ClassName = "PointsTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static PointsTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected PointsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
        }

        public void UpdateCellData(SummaryModel summary)
        {
            var frame = Frame;

            Text.Font = UIFont.FromName(Text.Font.Name, SizeConstants.ScreenMultiplier * 14);
            Text2.Font = UIFont.FromName(Text.Font.Name, SizeConstants.ScreenMultiplier * 14);
            Text.SizeToFit();
            Text2.SizeToFit();

            if ((summary as ChallengeTypeSummaryModel) != null)
                UpdateCellData(summary as ChallengeTypeSummaryModel);
            else if ((summary as ChallengeSummaryModel) != null)
                UpdateCellData(summary as ChallengeSummaryModel);
            else if ((summary as RewardSummaryModel) != null)
                UpdateCellData(summary as RewardSummaryModel);
        }

        public void UpdateCellData(ChallengeSummaryModel summary)
        {
            Image.Image = UIImage.FromBundle("challenges-icon_off");
            Text.Text = summary.ChallengeCompCount + " of " + summary.Total + " Challenges completed";
            Text2.Text = "";// string.Format("{0} reward purchased", summary.Total);
            if (summary.ChallengeCompCount == 0)
            {
                summary.ChallengeCompCount = 1;
            }
            ProgressBar.Progress = summary.Total > 0 ? (float)summary.ChallengeCompCount / (float)summary.Total : 0;
        }

        public void UpdateCellData(ChallengeTypeSummaryModel summary)
        {
            Image.Image = UIImage.FromBundle("challenges-icon_off");
            Text.Text = summary.ChallengeCompCount + " of " + summary.Total + " " + summary.DisplayName + " Challenges completed";
            Text2.Text = "";
            ProgressBar.Progress = summary.Total > 0 ? (float)summary.ChallengeCompCount / (float)summary.Total : 0;
        }

        public void UpdateCellData(RewardSummaryModel summary)
        {
            Image.Image = UIImage.FromBundle("challenges-icon_off");
            Text.Text = summary.UnlockedRewardsCount + " of " + summary.Total + " Rewards unlocked";
            Text2.Text = summary.PurchasedRewardsCount + " reward"  +  (summary.PurchasedRewardsCount != 1 ? "s" : "") + " purchased";
            ProgressBar.Progress = summary.Total > 0 ? (float)summary.UnlockedRewardsCount / (float)summary.Total : 0;
        }
    }
}
