using System;
using FFImageLoading;
using Foundation;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class RewardCategoryTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "RewardCategoryTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static RewardCategoryTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected RewardCategoryTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            CategoryName.Text = null;
            CategoryImage.Image = null;
        }

        public void UpdateCellData(RewardModel reward)
        {
            CategoryName.Text = reward.Name;
            CategoryCountLabel.Text = (reward.NumChildren ?? reward.ChildList?.Count ?? 0).ToString();

            ImageService.Instance.LoadUrl(reward.MainImageURL).Into(CategoryImage);

            CategoryImage.Layer.CornerRadius = CategoryImageOverlay.Layer.CornerRadius = UIScreen.MainScreen.Bounds.Width / 100 * 0.48f;

            SetFonts();
        }

        private void SetFonts()
        {
            var multiplier = UIScreen.MainScreen.Bounds.Width / 100;
            CategoryName.Font = UIFont.FromName("ProximaNova-Bold", multiplier * 5.8f);
            CategoryCountLabel.Font = UIFont.FromName("ProximaNova-Regular", multiplier * 3.86f);
        }
    }
}
