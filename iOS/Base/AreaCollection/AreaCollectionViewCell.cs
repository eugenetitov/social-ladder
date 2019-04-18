using System;

using Foundation;
using UIKit;
using SocialLadder.Models;
using FFImageLoading;

namespace SocialLadder.iOS.Areas
{
    public partial class AreaCollectionViewCell : UICollectionViewCell
    {
        public static readonly string ClassName = "AreaCollectionViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static AreaCollectionViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected AreaCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();

            AreaImage.Image = null;
            AreaName.Text = null;
            ScoreImage.Hidden = true;
            Score.Hidden = true;
        }

        public void UpdateCellData(AreaModel item)
        {
            AreaName.Text = item.areaName;
            ImageService.Instance.LoadUrl(item.areaDefaultImageURL).Into(AreaImage);
            if (SL.HasProfile)
                Score.Text = SL.Profile.Score.ToString();
            ScoreImage.Hidden = item.areaGUID != SL.AreaGUID;
            Score.Hidden = ScoreImage.Hidden;
        }

        public void UpdateToAddArea()
        {
             AreaImage.Image = UIImage.FromBundle("add-area_large");
            AreaName.Text = "Find a new area";
            ScoreImage.Hidden = true;
            Score.Hidden = true;
        }
    }
}
