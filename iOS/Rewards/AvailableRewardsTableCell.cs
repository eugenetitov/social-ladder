using System;
using FFImageLoading;
using Foundation;
using SocialLadder.iOS.Rewards.Models;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class AvailableRewardsTableCell : UITableViewCell, IRewardTableCell
    {
        public static readonly string ClassName = "AviableRewardsTableCell";
        public static readonly NSString Key = new NSString("AviableRewardsTableCell");
        public static readonly UINib Nib;
        //private static bool _isTitleSizeDefault;
        
        static AvailableRewardsTableCell()
        {
            Nib = UINib.FromName("AviableRewardsTableCell", NSBundle.MainBundle);
        }

        protected AvailableRewardsTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
       
        public override void PrepareForReuse()
        {
            RewardImage.Image = null;
            RewardName.Text = null;
            //RewardName.AttributedText = null;
            
            //if ((((RewardName.ContentSize.Height - RewardName.TextContainerInset.Top - RewardName.TextContainerInset.Bottom) / RewardName.Font.LineHeight) < 2)&& !_isTitleSizeDefault)
            //{
            //    var frame = RewardName.Frame;
            //    frame.Y -= (nfloat)(RewardName.Font.LineHeight * 0.6);
            //    RewardName.Frame = frame;
            //    _isTitleSizeDefault = true;
            //}

            base.PrepareForReuse();
        }

        public void UpdateCellData(RewardItemModel item)
        {
            RewardImage.Image = UIImage.FromBundle("CellImagePlaceholder");
            UIStringAttributes stringAttributes = new UIStringAttributes
            {
                Font = RewardName.Font,
                ForegroundColor = UIColor.Black,
                ParagraphStyle = new NSMutableParagraphStyle() { LineSpacing = 6.0f }
            };
            this.SelectionStyle = UITableViewCellSelectionStyle.None;

            var AttributedText = new NSMutableAttributedString(item.Name);
            AttributedText.AddAttributes(stringAttributes, new NSRange(0, item.Name.Length));
            RewardName.AttributedText = AttributedText;
           
            //if (((RewardName.ContentSize.Height - RewardName.TextContainerInset.Top - RewardName.TextContainerInset.Bottom) / RewardName.Font.LineHeight) < 2)
            //{
            //    var frame = RewardName.Frame;
            //    frame.Y += (nfloat)(RewardName.Font.LineHeight * 0.6);
            //    RewardName.Frame = frame;
            //    _isTitleSizeDefault = false;
            //}
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
