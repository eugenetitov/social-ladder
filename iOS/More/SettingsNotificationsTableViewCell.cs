using System;

using Foundation;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.More
{
    public partial class SettingsNotificationsTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "SettingsNotificationsTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        static SettingsNotificationsTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected SettingsNotificationsTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCellData(AreaModel area)
        {
            AreaName.Text = area.areaName;
        }
    }
}
