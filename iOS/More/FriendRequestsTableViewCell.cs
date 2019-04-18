using System;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.More
{
    public partial class FriendRequestsTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "FriendRequestsTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;
        private CoreGraphics.CGRect MainFrame = UIScreen.MainScreen.Bounds;

        public UIView SeparatorView
        {
            get => this.separatorView;
        }

        static FriendRequestsTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected FriendRequestsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            ActionText.TextColor = UIColor.FromRGB(38, 38, 38);
            SeparatorView.BackgroundColor = UIColor.FromRGB(216, 216, 216);
        }

        public void UpdateCellData(TableOption menuOption)
        {
            ActionText.Text = menuOption.Text;
            CreationDate.Text = menuOption.Date;
            ActorImage.Image = UIImage.FromBundle(menuOption.Image);

            nfloat multiplier = UIScreen.MainScreen.Bounds.Width / 414;

            btnConfirm.Layer.BorderWidth = 1 * multiplier;
            btnConfirm.Layer.BorderColor = UIColor.FromRGB(0, 122, 194).CGColor;
            btnConfirm.Layer.CornerRadius = 2 * multiplier;
            btnDelete.Layer.BorderWidth = 1 * multiplier;
            btnDelete.Layer.BorderColor = UIColor.FromRGB(173, 173, 173).CGColor;
            btnDelete.Layer.CornerRadius = 2 * multiplier;

            ActionText.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);
            CreationDate.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);
            btnConfirm.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);
            btnDelete.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);
        }
    }
}
