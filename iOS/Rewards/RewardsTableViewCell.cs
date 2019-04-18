using System;
using FFImageLoading;
using Foundation;
using UIKit;
using SocialLadder.Models;
using SocialLadder.iOS.Rewards.Models;
using SocialLadder.iOS.Constraints;
using CoreGraphics;

namespace SocialLadder.iOS.Rewards
{
    public partial class RewardsTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "RewardsTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        private nfloat _rewardWidthConstant = 0.385f * UIScreen.MainScreen.Bounds.Width;

        static RewardsTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected RewardsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            RewardImage.Image = null;
            RewardName.Text = null;
            topStatusImage.Image = null;
            iconStatus.Image = null;
            needMoreStatus.Text = null;
            vPercentContainer.Hidden = true;
            lblStatusCountTop.Hidden = true;
            lblStatusCountBottom.Hidden = true;
            iconStatus.BackgroundColor = UIColor.Clear;
            iconStatus.Layer.BorderColor = UIColor.Clear.CGColor;
            iconStatus.Layer.BorderWidth = 0;
            vPercentContainerLeft.Hidden = true;
            lblStatusCountTopLeft.Hidden = true;
            lblStatusCountBottomLeft.Hidden = true;
            topStatusImage.BackgroundColor = UIColor.Clear;
            topStatusImage.Layer.BorderColor = UIColor.Clear.CGColor;
            topStatusImage.Layer.BorderWidth = 0;
            cnCenterStatusWidth.Constant = 0;
            cnStatusPercent.Constant = 0;
            cnStatusPercentLeft.Constant = 0;


            base.PrepareForReuse();
        }

        public void UpdateCellData(RewardItemModel item)
        {
            var test = this.Frame.Width;
            lblPointsCount.Text = item.MinScore.ToString();
            lblAviableUnits.Text = string.Format("{0} Units Available", item.RemainingUnits);
            ImageService.Instance.LoadUrl(item.MainImageURL).Into(RewardImage);
            this.SelectionStyle = UITableViewCellSelectionStyle.None;
            LikeButton.SetImage(UIImage.FromBundle("points-icon_off"), UIControlState.Normal);
            UIStringAttributes stringAttributes = new UIStringAttributes
            {
                Font = UIFont.FromName("ProximaNova-Bold", UIScreen.MainScreen.Bounds.Width * 0.055f),
                ForegroundColor = UIColor.Black,
                ParagraphStyle = new NSMutableParagraphStyle() { LineSpacing = 6.0f }
            };
            var attributedText = new NSMutableAttributedString(item.Name);
            attributedText.AddAttributes(stringAttributes, new NSRange(0, item.Name.Length));
            RewardName.AttributedText = attributedText;

            RewardImage.Layer.CornerRadius = RewardImageOverlay.Layer.CornerRadius = UIScreen.MainScreen.Bounds.Width / 100 * 0.48f;

            bool noPoints = item.MinScore > SL.Profile.Score;
            bool Locked = (item.AutoUnlockDate?.ToLocalTime() ?? DateTime.Now) > DateTime.Now;
            //item.MinScore = (int)(SL.Profile.Score * 0.8f);//Hardcode

            if (noPoints && Locked)
            {
                lockStatus.Image = UIImage.FromBundle("lock-icon");
                unlockStatus.Text = item.NextEventCountDown; //string.Format("{0} {1:hh}h:{1:mm}m:{1:ss}s", "Unlocks in", (TimeSpan)((item?.AutoUnlockDate?? DateTime.Now) - DateTime.Now));//.ToString(@"hh h\:mm m\:ss s\"));
                float iconStatusWidth = (float)(Frame.Width / 7.009);
                float statusLabelWidth = (float)(Frame.Width / 8.8);


                topLeftIcon.BackgroundColor = UIColor.White;
                topLeftIcon.Layer.BorderColor = UIColor.FromRGB(229, 229, 229).CGColor;
                topLeftIcon.Layer.BorderWidth = iconStatusWidth * 0.037f;
                topLeftIcon.Layer.CornerRadius = iconStatusWidth / 2;
                vStatusBorderLeft.Layer.BorderWidth = iconStatusWidth * 0.037f;
                vStatusBorderLeft.Layer.CornerRadius = iconStatusWidth / 2;
                vStatusBorderLeft.Layer.BorderColor = UIColor.FromRGB(232, 31, 138).CGColor;
                vStatusCircleLeft.Layer.CornerRadius = statusLabelWidth / 2;
                //topLeftIcon.BackgroundColor = UIColor.White;
                //topLeftIcon.Layer.BorderColor = UIColor.FromRGB(229, 229, 229).CGColor;
                //topLeftIcon.Layer.BorderWidth = topLeftIcon.Bounds.Width * 0.037f;
                //topLeftIcon.Layer.CornerRadius = topLeftIcon.Bounds.Width / 2;
                //vStatusBorderLeft.Layer.BorderWidth = vStatusBorderLeft.Bounds.Width * 0.037f;
                //vStatusBorderLeft.Layer.CornerRadius = vStatusBorderLeft.Bounds.Width / 2;
                //vStatusBorderLeft.Layer.BorderColor = UIColor.FromRGB(232, 31, 138).CGColor;
                //vStatusCircleLeft.Layer.CornerRadius = vStatusCircleLeft.Bounds.Width / 2;

                //cnStatusPercentLeft.Constant = (nfloat)(vPercentContainerLeft.Bounds.Width * (SL.Profile.Score / item.MinScore) - vPercentContainerLeft.Bounds.Width / 2);

                vPercentContainerLeft.Hidden = false;

                //UILabel lblStatusCount;
                //if (SL.Profile.Score / item.MinScore > 0.7)
                //{
                //    lblStatusCount = lblStatusCountBottomLeft;
                //}
                //else
                //{
                //    lblStatusCount = lblStatusCountTopLeft;
                //}
                //lblStatusCount.Hidden = false;
                //lblStatusCount.Text = (item.MinScore - SL.Profile.Score).ToString();
                //topLeftIcon.Image = UIImage.FromBundle("small-score-icon");
                lblStatusCountTopLeft.Hidden = false;
                lblStatusCountTopLeft.Text = (item.MinScore - SL.Profile.Score).ToString();
            }
            else if (noPoints && !Locked)
            {
               //iconStatus.Image = UIImage.FromBundle("small-score-icon");
                iconStatus.BackgroundColor = UIColor.White;
                iconStatus.Layer.BorderColor = UIColor.FromRGB(229, 229, 229).CGColor;
                float iconStatusWidth = (float)(Frame.Width / 7.211);
                float statusLabelWidth = (float)(Frame.Width / 8.8);
                iconStatus.Layer.BorderWidth = iconStatusWidth * 0.037f;
                iconStatus.Layer.CornerRadius = iconStatusWidth / 2;
                vStatusBorder.Layer.BorderWidth = iconStatusWidth * 0.037f;
                vStatusBorder.Layer.CornerRadius = iconStatusWidth / 2;
                vStatusBorder.Layer.BorderColor = UIColor.FromRGB(232, 31, 138).CGColor;
                vStatusCircle.Layer.CornerRadius = statusLabelWidth / 2;
                /*
                iconStatus.Layer.BorderWidth = iconStatus.Bounds.Width * 0.037f;
                iconStatus.Layer.CornerRadius = iconStatus.Bounds.Width / 2;
                vStatusBorder.Layer.BorderWidth = vStatusBorder.Bounds.Width * 0.037f;
                vStatusBorder.Layer.CornerRadius = vStatusBorder.Bounds.Width / 2;
                vStatusBorder.Layer.BorderColor = UIColor.FromRGB(232, 31, 138).CGColor;
                vStatusCircle.Layer.CornerRadius = vStatusCircle.Bounds.Width / 2;
                */
                //cnStatusPercent.Constant = (nfloat)(vPercentContainer.Bounds.Width * (SL.Profile.Score / item.MinScore) - vPercentContainer.Bounds.Width / 2);

                needMoreStatus.Text = "MORE NEEDED";

                vPercentContainer.Hidden = false;

                //UILabel lblStatusCount;
                //if (SL.Profile.Score / item.MinScore > 0.7)
                //{
                //    lblStatusCount = lblStatusCountBottom;
                //}
                //else
                //{
                //  lblStatusCount = lblStatusCountTop;
                //}
                lblStatusCountTop.Text = (item.MinScore - SL.Profile.Score).ToString();
                lblStatusCountTop.Hidden = false;
                //lblStatusCount = lblStatusCountBottom;
                //lblStatusCount.Hidden = false;
                //lblStatusCount.Text = (item.MinScore - SL.Profile.Score).ToString();
            }
            else if (!noPoints && Locked)
            {
                lockStatus.Image = UIImage.FromBundle("lock-icon");
                unlockStatus.Text = item.NextEventCountDown; //string.Format("{0} {1:hh}h:{1:mm}m:{1:ss}s", "Unlocks in", (TimeSpan)((item?.AutoUnlockDate?? DateTime.Now) - DateTime.Now));//.ToString(@"hh h\:mm m\:ss s\"));
                topStatusImage.Image = UIImage.FromBundle("circle_green");
            }
            else if (!noPoints && !Locked)//Available
            {
                cnCenterStatusWidth.Constant = RewardImageOverlay.Bounds.Width * cnTopStatusImageWidth.Multiplier - RewardImageOverlay.Bounds.Width * cnCenterStatusWidth.Multiplier;
                iconStatus.Image = UIImage.FromBundle("circle_green");
            }
        }

        public void UpdateCellData(RewardItemModel item, nfloat offset, bool isRightOrientation = true)
        {
            PrepareForReuse();
            UpdateCellData(item);

            if (isRightOrientation)
            {
                //var test = RewardImage.Frame.Width;
                //var test1 = RewardImage.Bounds.Width;
                //LeadingSpaceImageConstraint.Constant = this.Frame.Width - RewardImage.Frame.Width + offset;
                LeadingSpaceImageConstraint.Constant = this.Frame.Width - _rewardWidthConstant + offset;
                LeadingSpaceDescriptionConstraint.Constant = 0;
                TrailingSpaceDescriptionConstraint.Constant = _rewardWidthConstant - offset;
                //TrailingSpaceDescriptionConstraint.Constant = RewardImage.Frame.Width - offset;
            }
            else
            {
                LeadingSpaceImageConstraint.Constant = offset;
                //LeadingSpaceDescriptionConstraint.Constant = RewardImage.Frame.Width + offset;
                LeadingSpaceDescriptionConstraint.Constant = _rewardWidthConstant + offset;
                TrailingSpaceDescriptionConstraint.Constant = 0;
            }
            this.UpdateConstraintsIfNeeded();
        }

        public void UpdateCellData(RewardItemModel item, nfloat offset, RewardStatus status = RewardStatus.None, bool isRightOrientation = true)
        {
            if (status == RewardStatus.Aviable)
            {
                topStatusImage.Image = UIImage.FromBundle("circle_green");
            }
            if (status == RewardStatus.Claimed)
            {
                iconStatus.Image = UIImage.FromBundle("gift_icon");
            }
            UpdateCellData(item, offset, isRightOrientation);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            RewardName.Font = UIFont.FromName("ProximaNova-Bold", UIScreen.MainScreen.Bounds.Width * 0.055f);
            lblPointsCount.Font = lblAviableUnits.Font = UIFont.FromName("SFProText-Medium", UIScreen.MainScreen.Bounds.Width * 0.029f);
            unlockStatus.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.034f);
            needMoreStatus.Font = UIFont.FromName("SFProText-Bold", UIScreen.MainScreen.Bounds.Width * 0.024f);

            var statusCountFont = UIFont.FromName("SFProText-Heavy", UIScreen.MainScreen.Bounds.Width * 0.029f);
            lblStatusCountBottom.Font = lblStatusCountBottomLeft.Font = lblStatusCountTop.Font = lblStatusCountTopLeft.Font = statusCountFont;
        }
    }
}
