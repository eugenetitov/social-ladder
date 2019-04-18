using Foundation;
using SocialLadder.ViewModels.Intro;
using System;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    public partial class Intro1ViewController : IntroBaseViewController
    {
        public Intro1ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            try
            {
                //img_Logo.Image = UIImage.FromBundle("SLCircleLogo");
                lbl_SocialLadder.Font = UIFont.FromName("HoratioD-Medi", UIScreen.MainScreen.Bounds.Width * 0.105f);
                lbl_Welcome.Font = UIFont.FromName("ProximaNova-Bold", UIScreen.MainScreen.Bounds.Width * 0.075f);

                img_Logo.Image = UIImage.FromBundle("SLCircleLogo");
            }
            catch { }
        }
    }
}