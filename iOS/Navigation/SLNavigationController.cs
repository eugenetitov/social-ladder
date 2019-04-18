using Foundation;
using SocialLadder.iOS.ViewControllers;
using System;
using UIKit;
using FFImageLoading;
using CoreGraphics;
using SocialLadder.iOS.Views.Shared;
using System.Linq;
using SocialLadder.iOS.Models.Enums;

namespace SocialLadder.iOS.Navigation
{
    public partial class SLNavigationController : UINavigationController
    {
        private bool _isNavBarHidden;
        private nfloat _actionBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height; //22f;
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;
        //UISwipeGestureRecognizer SwipeDown { get; set; }
        public NavigationTitleView NavTitle { get; set; }

        public SLNavigationController(IntPtr handle) : base(handle)
        {

        }

        public ENavigationTabs PreselectedTab
        {
            get;
            set;
        }

        public void SetTitle(string title)
        {
            try
            {
                NavTitle?.SetTitle(title);

                if (NavigationBar != null)
                {
                    //UINavigationItem navItem = NavigationBar.Items != null ? NavigationBar.Items[0] : null;
                    //if (navItem != null)
                    //{
                    //    NavTitle = navItem.TitleView as NavigationTitleView;
                    //    if (NavTitle != null)
                    //    {
                    //        NavTitle.SetTitle(title);
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
            }

        }

        public void Update()
        {
            try
            {
                if (NavTitle == null)
                    return;

                var area = SL.Area;

                if (area != null)
                    NavTitle.BackgroundColor = area.areaPrimaryColor.ToUIColor();

                NavTitle.Update();

                if (NavigationBar != null)
                {
                    //var area = SL.Area;
                    //if (area != null)
                    //    NavigationBar.BarTintColor = area.areaPrimaryColor.ToUIColor();

                    //UINavigationItem navItem = NavigationBar.Items != null ? NavigationBar.Items[0] : null;
                    //if (navItem != null)
                    //{
                    //    NavTitle = navItem.TitleView as NavigationTitleView;
                    //    if (NavTitle != null)
                    //    {
                    //        NavTitle.Update();
                    //    }
                    //}
                }
            }
            catch (Exception)
            {
            }
        }
        /*
        private void HandleDrag(UISwipeGestureRecognizer recognizer)
        {
			UIViewController visibleViewController = VisibleViewController;
			if (visibleViewController != null)
			{
				SLViewController sLViewController = visibleViewController as SLViewController;
				if (sLViewController != null)
				{
					sLViewController.Refresh();
				}
			}
        }
        */
        public override void UpdateViewConstraints()
        {
            base.UpdateViewConstraints();

        }

        public void LoadNotifications()
        {
            try
            {
                UIStoryboard board = UIStoryboard.FromName("Notifications", null);
                UIViewController controller = (UIViewController)board.InstantiateViewController("NotificationsViewController");
                PushViewController(controller, true);
            }
            catch (Exception)
            {
            }
        }

        void NavNotificationClicked()
        {
            LoadNotifications();
        }

        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                base.SetNavigationBarHidden(true, false);

                NavTitle = NavigationTitleView.Create();
                NavTitle.NotificationButtonOutlet.TouchUpInside += (object sender, EventArgs e) => NavNotificationClicked();
                NavTitle.BtnBackOutlet.TouchUpInside += (object sender, EventArgs e) => PopViewController(false);

                View.AddSubview(NavTitle);

                NavTitle.TranslatesAutoresizingMaskIntoConstraints = false;
                NavTitle.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
                NavTitle.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
                NavTitle.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
                NavTitle.HeightAnchor.ConstraintEqualTo(View.WidthAnchor, 66f / 414f, _actionBarHeight).Active = true;

                if (NavigationBar != null)
                {
                    //UINavigationItem navItem = NavigationBar.Items != null ? NavigationBar.Items[0] : null;
                    //if (navItem != null)
                    //{
                    //    NavTitle = NavigationTitleView.Create();

                    //    if (NavTitle != null)
                    //    {
                    //        if (NavTitle.NotificationButtonOutlet != null)
                    //            NavTitle.NotificationButtonOutlet.TouchUpInside += (object sender, EventArgs e) => NavNotificationClicked();
                    //        navItem.TitleView = NavTitle;
                    //    }

                    //}
                }
            }
            catch (Exception)
            {
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            TuggleCustomNavigationBar(true);
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);

            if (ChildViewControllers.Count() > 1)
                NavTitle.BackMode();
        }

        public override UIViewController PopViewController(bool animated)
        {
            UIViewController controller = base.PopViewController(animated);

            if (ChildViewControllers.Count() == 1)
                NavTitle.RootMode();

                return controller;
        }

        public override void SetNavigationBarHidden(bool hidden, bool animated)
        {
            //base.SetNavigationBarHidden(hidden, animated);

            _isNavBarHidden = hidden;

            if (NavTitle == null)
                return;

            TuggleCustomNavigationBar(animated);
        }

        private void TuggleCustomNavigationBar(bool animated)
        {
            NSLayoutConstraint top = View.Constraints.Where(cns => cns.FirstItem == NavTitle && cns.FirstAttribute == NSLayoutAttribute.Top ||
            cns.SecondItem == NavTitle && cns.SecondAttribute == NSLayoutAttribute.Top).FirstOrDefault();

            if (top == null)
                return;

            nfloat topInset = _isNavBarHidden ? 0 : NavTitle.Frame.Height - _actionBarHeight;
            nfloat topConstant = _isNavBarHidden ? -NavTitle.Frame.Height : 0;

            if (top.Constant == topConstant && AdditionalSafeAreaInsets.Top == topInset)
                return;

            UIView.BeginAnimations("slideAnimation");
            UIView.SetAnimationBeginsFromCurrentState(true);
            UIView.SetAnimationDuration(animated? 0.2 : 0);
            UIView.SetAnimationCurve(UIViewAnimationCurve.Linear);

            top.Constant = topConstant;
            AdditionalSafeAreaInsets = new UIEdgeInsets(topInset, 0, 0, 0);

            View.LayoutIfNeeded();

            UIView.CommitAnimations();
        }
    }
}