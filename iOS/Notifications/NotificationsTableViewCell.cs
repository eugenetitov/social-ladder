using System;

using Foundation;
using UIKit;
using SocialLadder.Models;
using SocialLadder.iOS.Constraints;

namespace SocialLadder.iOS.Notifications
{
    public partial class NotificationsTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "NotificationsTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static NotificationsTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected NotificationsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            NotificationText.Text = String.Empty;
            AreaImage.Image = null;
            ArrowRight.Image = null;
        }

        public void UpdateCellData(NotificationItemModel item)
        {
            NotificationText.Text = item.Message;
            AreaImage.Image = item.Icon;
        }

        public void SetupView()
        {
            NotificationText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);
            //BottomLine.Hidden = false;
            ArrowRight.Image = UIImage.FromBundle("arrow-right");
        }
    }
}
