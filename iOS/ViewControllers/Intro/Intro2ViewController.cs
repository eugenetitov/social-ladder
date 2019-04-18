using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    public partial class Intro2ViewController : IntroBaseViewController
    {
        public Intro2ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            try
            {
                img_Right.Image = UIImage.FromBundle("gift_icon_white");
                lbl_Right.Font = lbl_Left.Font = UIFont.FromName("SFProText-Regular", UIScreen.MainScreen.Bounds.Width * 0.045f);
            }
            catch { }
        }
    }
}