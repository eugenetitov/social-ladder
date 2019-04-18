using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    public partial class Intro3ViewController : IntroBaseViewController
    {
        public Intro3ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            try
            {
                lbl_Bottom3.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.052f);
                lbl_Title1.Font = lbl_Title2.Font = lbl_Title3.Font = lbl_Title4.Font = lbl_Title5.Font = lbl_Title6.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.03f);


                img_IconCenter.Image = UIImage.FromBundle("Images1");
                //img_IconCenter.BackgroundColor = UIColor.Blue;
                img_Icon1.Image = UIImage.FromBundle("check-icon_white");
                img_Icon2.Image = UIImage.FromBundle("invite-icon_off");
                img_Icon3.Image = UIImage.FromBundle("ticket-icon_white");
                img_Icon4.Image = UIImage.FromBundle("quiz-icon_white");
                img_Icon5.Image = UIImage.FromBundle("share-icon_large");
                img_Icon6.Image = UIImage.FromBundle("insta-icon_white");
            }
            catch { }
        }
    }
}