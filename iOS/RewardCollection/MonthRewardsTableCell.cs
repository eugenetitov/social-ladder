using System;
using CoreGraphics;
using FFImageLoading;
using Foundation;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.RewardCollection
{
    public partial class MonthRewardsTableCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MonthRewardsTableCell");
        public static readonly UINib Nib;

        static MonthRewardsTableCell()
        {
            Nib = UINib.FromName("MonthRewardsTableCell", NSBundle.MainBundle);
        }

        protected MonthRewardsTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void Update(RewardModel model)
        {
            RewardCategoryName.Text = model.Name;
            RewardItemBackground.Image = UIImage.FromBundle("rewardsDetails-placeholder");
            ImageService.Instance.LoadUrl(model.MainImageURL).Into(RewardItemBackground);
            RewardsCountLabel.Text = model.ChildList?.Count.ToString();
            RewardIcon.Image = UIImage.FromBundle("reward-icon_white");
            SetFonts();
        }

        public override void LayoutSubviews()
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            this.LayoutMargins = new UIEdgeInsets(50, 50, 50, 50);
            base.LayoutSubviews();
        }
        
        private void SetFonts()
        {
            CGRect screenRect = UIScreen.MainScreen.Bounds;
            nfloat screenWidth = screenRect.Size.Width;
            float rewardNameFontMultiplier = 0.06f;
            float rewardCountFontMultiplier = 0.045f;
            RewardCategoryName.Font = UIFont.FromName("ProximaNova-Bold", screenWidth * rewardNameFontMultiplier);
            RewardsCountLabel.Font = UIFont.FromName("ProximaNova-Regular", screenWidth * rewardCountFontMultiplier);
        }
    }
}
