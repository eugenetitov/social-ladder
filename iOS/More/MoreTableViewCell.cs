using System;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.More
{
    public partial class MoreTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "MoreTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;
        private CoreGraphics.CGRect MainFrame = UIScreen.MainScreen.Bounds; 

        static MoreTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected MoreTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
            MenuText.TextColor = UIColor.Black;            
        }

        public void UpdateCellData(MenuOption menuOption)
        {
            MenuText.Text = menuOption.Text;
            if (MenuText.Text == "Logout")
                MenuText.TextColor = UIColor.Gray;
            MenuImage.Image = UIImage.FromBundle(menuOption.Image);
            MenuText.Font = UIFont.FromName("ProximaNova-Regular", 13.5f * MainFrame.Width / 414f);
            SeparatorInset = new UIEdgeInsets(0, MainFrame.Width * 0.135f, 0, 0);
        }
    }
}
