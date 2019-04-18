using System;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class ContactHeaderView : UITableViewHeaderFooterView
    {
        public static readonly NSString ClassName = new NSString("ContactHeaderView");
        public static readonly UINib Nib;

        static ContactHeaderView()
        {
            Nib = UINib.FromName("ContactHeaderView", NSBundle.MainBundle);
        }

        protected ContactHeaderView(IntPtr handle) : base(handle)
        {
        }

        public void UpdateHeader(string Latter)
        {
            lblHeader.Text = Latter;
        }
    }
}
