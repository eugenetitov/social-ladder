using System;

using Foundation;
using UIKit;
using SocialLadder.iOS.Camera;
using SocialLadder.iOS.Constraints;

namespace SocialLadder.iOS.Challenges
{
    public partial class CollateralCollectionViewCell : UICollectionViewCell
    {
        public static readonly string ClassName = "CollateralCollectionViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static CollateralCollectionViewCell()
        {
            Nib = UINib.FromName("CollateralCollectionViewCell", NSBundle.MainBundle);
        }

        protected CollateralCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
        }

        public void UpdateCellData(CameraPicture picture)
        {
            ImageView.Image = picture.Image;
            ImageView.Layer.CornerRadius = SizeConstants.ScreenMultiplier * 8;
            ImageView.ClipsToBounds = true;
        }
    }
}
