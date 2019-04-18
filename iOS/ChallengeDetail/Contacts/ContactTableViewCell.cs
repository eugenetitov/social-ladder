using System;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class ContactTableViewCell : UITableViewCell
    {
        public static readonly NSString ClassName = new NSString("ContactTableViewCell");
        public static readonly UINib Nib;

        static ContactTableViewCell()
        {
            Nib = UINib.FromName("ContactTableViewCell", NSBundle.MainBundle);
        }

        protected ContactTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void UpdateCell(PersonContact person)
        {
            lblName.Text = $"{person.GivenName} {person.FamilyName}";
            UpdateStatus(person);
            //imgStatus.BackgroundColor = UIColor.Black;
        }
        public void UpdateStatus(PersonContact person)
        {
            if (person.SelectedContact)
            {
                //imgStatus.BackgroundColor = UIColor.Green;
                imgStatus.Image = UIImage.FromBundle("check-mark-black");
            }

            if (!person.SelectedContact)
            {
                //imgStatus.BackgroundColor = UIColor.Blue;
                imgStatus.Image = null;
            }

        }

    }
}
