using System;
using FFImageLoading;
using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public partial class LeaderboardTableViewCell : UITableViewCell
    {
        private static nfloat _progressMaxProportion = 250f / 336f;

        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;

        public static readonly string ClassName = "LeaderboardTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        public UIView TopSeparatorView => TopSeparator;
        public nfloat _progressValue;

        static LeaderboardTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
            
        }

        protected LeaderboardTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }
        
        public override void PrepareForReuse()
        {
            TopSeparator.Hidden = true;
            base.PrepareForReuse();
        }


        public void UpdateCellData(FriendModel friend, float progressBarValue)
        {
            UpdateCellData(friend, progressBarValue, false);
        }

        public void UpdateCellData(FriendModel friend, float progressBarValue, bool isRequeredSeparator)
        {
            NameText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);
            RankText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);

            NameText.Text = friend.Name;
            RankText.Text = friend.Rank.ToString();

            ImageService.Instance.LoadUrl(friend.ProfilePicURL).Into(ProfileImage);
            ProgressBarWidthConstraint.Constant = NameText.Frame.Width * (float.IsNaN(progressBarValue) ? 0 : progressBarValue);
            if (isRequeredSeparator)
            {
                TopSeparator.Hidden = false;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            ProfileImage.Layer.CornerRadius = ProfileImage.Frame.Width / 2;
            ProgressBar.UpdateConstraints();
        }
    }
}
