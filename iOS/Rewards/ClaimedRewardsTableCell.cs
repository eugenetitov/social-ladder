using System;
using FFImageLoading;
using Foundation;
using SocialLadder.iOS.Rewards.Models;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class ClaimedRewardsTableCell : UITableViewCell, IRewardTableCell
    {
        public static readonly string ClassName = "ClaimedRewardsTableCell";
        public static readonly NSString Key = new NSString("ClaimedRewardsTableCell");
        public static readonly UINib Nib;

        static ClaimedRewardsTableCell()
        {
            Nib = UINib.FromName("ClaimedRewardsTableCell", NSBundle.MainBundle);
        }

        protected ClaimedRewardsTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            RewardImage.Image = null;
            RewardName.Text = null;
            //if (((RewardName.ContentSize.Height - RewardName.TextContainerInset.Top - RewardName.TextContainerInset.Bottom) / RewardName.Font.LineHeight) < 2)
            //{
            //    var frame = RewardName.Frame;
            //    frame.Y -= (nfloat)(RewardName.Font.LineHeight * 0.6);
            //    RewardName.Frame = frame;
            //}
            base.PrepareForReuse();
        }

        public void UpdateCellData(RewardItemModel item)
        {
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            UIStringAttributes stringAttributes = new UIStringAttributes
            {
                Font = RewardName.Font,
                ForegroundColor = UIColor.Black,
                ParagraphStyle = new NSMutableParagraphStyle() { LineSpacing = 6.0f }
            };
            var AttributedText = new NSMutableAttributedString(item.Name);
            AttributedText.AddAttributes(stringAttributes, new NSRange(0, item.Name.Length));
            RewardName.AttributedText = AttributedText;
            RewardName.SizeToFit();
            //if (((RewardName.ContentSize.Height - RewardName.TextContainerInset.Top - RewardName.TextContainerInset.Bottom) / RewardName.Font.LineHeight) < 2)
            //{
            //    var frame = RewardName.Frame;
            //    frame.Y += (nfloat)(RewardName.Font.LineHeight * 0.6);
            //    RewardName.Frame = frame;
            //}
            RewardImage.Image = UIImage.FromBundle("CellImagePlaceholder");
        }

        public void UpdateCellData(RewardItemModel item, nfloat offset, bool isRightOrientation = true)
        {
            throw new NotImplementedException();
        }

        public void UpdateCellData(RewardItemModel item, nfloat offset, RewardStatus status, bool isRightOrientation = true)
        {
            if (status == RewardStatus.Aviable)
            {
                IconStatus.Image = UIImage.FromBundle("circle_green");
            }
            if (status == RewardStatus.Claimed)
            {
                IconStatus.Image = UIImage.FromBundle("gift_icon");
            }
            UpdateCellData(item);
        }
    }
}
