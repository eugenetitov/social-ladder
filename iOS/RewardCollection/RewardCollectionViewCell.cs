using System;
using CoreGraphics;
using FFImageLoading;
using Foundation;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.RewardCollection
{
    public partial class RewardCollectionViewCell : UICollectionViewCell
    {
        public static readonly string ClassName = "RewardCollectionViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;
        private nfloat ScreenWidth = UIScreen.MainScreen.Bounds.Size.Width;

        static RewardCollectionViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected RewardCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            RewardImage.Image = null;
        }

        public void UpdateCellData(RewardModel item)
        {
            RewardName.Text = item.Name;
            RewardsCountLabel.Text = (item.NumChildren ?? item.ChildList?.Count ?? 0).ToString();
            ImageService.Instance.LoadUrl(item.MainImageURL).Into(RewardImage);

            RewardImage.Layer.CornerRadius = RewardImageOverlay.Layer.CornerRadius = UIScreen.MainScreen.Bounds.Width / 100 * 0.48f;

            SetFonts();
        }

        private void SetFonts()
        {
            RewardsCountLabel.Font = UIFont.FromName("ProximaNova-Regular", ScreenWidth * 0.035f);

            var rewardNameFont = UIFont.FromName("ProximaNova-Bold", ScreenWidth * 0.06f);
            RewardName.Font = GetOptimalFontForWidth(rewardNameFont, ScreenWidth / 3.1f);
        }

        private UIFont GetOptimalFontForWidth(UIFont baseFont, nfloat widthInside)
        {
            var words = RewardName.Text.Split(' ');
            nfloat maxWordWidth = 0;
            foreach (NSString item in words)
            {
                nfloat width = item.GetSizeUsingAttributes(new UIStringAttributes { Font = baseFont }).Width;
                if (width > maxWordWidth)
                    maxWordWidth = width;
            }
            return maxWordWidth > widthInside ? baseFont.WithSize(baseFont.PointSize * (widthInside / maxWordWidth)) : baseFont;
        }
    }
}
