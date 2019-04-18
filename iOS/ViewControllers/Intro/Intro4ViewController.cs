using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    public partial class Intro4ViewController : IntroBaseViewController
    {
        public Intro4ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            try
            {
                lbl_Bottom4.Font = UIFont.FromName("ProximaNova-Regular", UIScreen.MainScreen.Bounds.Width * 0.052f);

                var progressViews = new List<UIProgressView>() { prb_User1, prb_User2, prb_User3, prb_User4, prb_User5 };
                foreach (var progressView in progressViews)
                {
                    progressView.Layer.CornerRadius = progressView.Layer.Sublayers[1].CornerRadius = 3f;
                    progressView.ClipsToBounds = progressView.Subviews[1].ClipsToBounds = true;
                    progressView.Layer.BorderColor = UIColor.White.CGColor;
                    progressView.Layer.BorderWidth = 1f;
                }
            }
            catch { }
        }

    }
}