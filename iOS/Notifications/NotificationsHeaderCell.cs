using System;

using Foundation;
using UIKit;
using SocialLadder.Models;
using SocialLadder.iOS.Constraints;

namespace SocialLadder.iOS.Notifications
{
    public partial class NotificationsHeaderCell : UITableViewHeaderFooterView
    {
        public static readonly string ClassName = "NotificationsHeaderCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static NotificationsHeaderCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected NotificationsHeaderCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            NotificationText.Text = String.Empty;
            NotificationsCount.Text = String.Empty;
            AreaImage.Image = null;
        }

        public void UpdateCellData<T>(ExpandableTableModel<T> item)
        {
            NotificationText.Text = item.Title;
            NotificationsCount.Text = item.NotificationsCount;
            AreaImage.Image = item.Icon;
        }

        public void SetupView()
        {
            NotificationText.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 24);
            NotificationsCount.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 12);
            CircleView.Layer.CornerRadius = SizeConstants.ScreenMultiplier * 12;
            //BottomLine.Hidden = false;
        }
    }
}
