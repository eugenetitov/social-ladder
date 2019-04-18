using System;

using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class MultipleChoiceTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "MultipleChoiceTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static MultipleChoiceTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        void ApplyStyles()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        protected MultipleChoiceTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            ApplyStyles();
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
        }

        public void UpdateCellData(ChallengeAnswerModel item, bool isSelected)
        {
            SelectedImage.Image = UIImage.FromBundle("selected-mc_white");
            ImgBackground.BackgroundColor = isSelected ? UIColor.FromRGB(189, 76, 217) : UIColor.Clear;
            ChoiceName.Text = item.AnswerName;
            SelectedImage.ClipsToBounds = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            ChoiceName.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.Screen.Width * 0.04f);
        }
    }
}
