using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using SocialLadder.iOS.ViewControllers.Base;
using System;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    public class IntroBaseViewController :  UIViewController//<T> : BaseViewController<T> where T : class, IMvxViewModel
    {
        public int PageIndex { get; set; }
        public IntroContainerViewController IntroContainerViewController { get; set; }

        public IntroBaseViewController(IntPtr handle) : base(handle)
        {
            ApplyStyle();
        }

        void ApplyStyle()
        {
            View.BackgroundColor = UIColor.Clear;
        }



        public virtual void PrepareContainer()
        {
            if (IntroContainerViewController != null)
                IntroContainerViewController.PrepareForIntro();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            PrepareContainer();
            if (IntroContainerViewController != null)
                IntroContainerViewController.ChangeProgressViewValue(this.PageIndex);
        }
    }
}