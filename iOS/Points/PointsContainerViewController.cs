using CoreGraphics;
using FFImageLoading;
using Foundation;
using MvvmCross.iOS.Views;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Points.CustomViews;
using SocialLadder.ViewModels.Points;
using System;
using System.Collections.Generic;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public partial class PointsContainerViewController : MvxViewController<PointsContainerViewModel> //UIViewController//SLViewController
    {
        private string _currentAreaGUID;
        UIPageViewController pageViewController;
        PointsPageViewControllerDataSource pageSource;
        public nfloat PageYOffset { get; set; }
        bool Glitch { get; set; }   //first time we try to open the areacollection (this layout uses a scrollview and pagecontroller), the constraints do not seem to update and the areacollection appears covered by the below view (constraint from areacollection.bottom to view.top). this glitch loads the second vc and switches to the first on viewwillappear because during testing, it was found that after switching pages, the it does not have this problem (this glitch simulates those steps)

        PointsViewController PointsVcInstance;
        LeaderboardViewController LeaderboardVcInstance;
        TransactionsViewController TransactionsVcInstance;

        public List<bool> SeparatelyShouldRefreshEndpoints { get; set; }
        //public bool ShouldRefreshAllEndpoints { get; set; }

        public PointsContainerViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();

            SeparatelyShouldRefreshEndpoints = new List<bool> { true, true, true };

            PointsAndStatsButton.SetTitle("Points & Stats");
            LeaderboardButton.SetTitle("Leaderboard");
            TransactionsButton.SetTitle("Transactions");
            PointsAndStatsButton.SetSelected();

            UIStoryboard board = UIStoryboard.FromName("Main", null);
            pageViewController = board.InstantiateViewController("PointsPageViewController") as UIPageViewController;
            pageSource = new PointsPageViewControllerDataSource(this);

            pageViewController.DataSource = pageSource;

            var startVC = this.ViewControllerAtIndex(1) as PointsBaseViewController;    //start on second page for glitch (should start on page index 0 without glitch)
            var viewControllers = new UIViewController[] { startVC };

            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
            pageViewController.View.Frame = mainView.Bounds;
            AddChildViewController(this.pageViewController);
            mainView.AddSubview(this.pageViewController.View);
            pageViewController.DidMoveToParentViewController(this);

            CGRect frame = View.Frame;
        }

        void CheckColor(UIColor color)
        {
            var components = color.CGColor.Components;
            var result = (components[0] * 0.299 + components[1] * 0.587 + components[2] * 0.114) * 255;

            if (result > 186)
            {
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
            }
            else
            {
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            SeparatelyShouldRefreshEndpoints[0] = true;
            SeparatelyShouldRefreshEndpoints[1] = true;
            SeparatelyShouldRefreshEndpoints[2] = true;

            //PointsBaseViewController currentController = (pageViewController.ViewControllers[0] as PointsBaseViewController);
            //if (currentController != null)
            //{
            //    currentController.ShouldRefreshEndpoint = true;
            //}


            //ShouldRefreshAllEndpoints = true;

            foreach (UIView view in pageViewController.View.Subviews)
            {
                if (view is UIScrollView)
                    (view as UIScrollView).Scrolled -= MainScroll_Scrolled;
            }
        }

        private void MainScroll_Scrolled(object sender, EventArgs e)
        {
            var currentController = pageViewController.ViewControllers[0];//needs to check is top VC
            if (currentController is LeaderboardViewController)
            {
                LeaderboardVcInstance = currentController as LeaderboardViewController;
                LeaderboardVcInstance.HideAreaCollection();
            }
            if (currentController is PointsViewController)
            {
                PointsVcInstance = currentController as PointsViewController;
                PointsVcInstance.HideAreaCollection();
            }
            if (currentController is TransactionsViewController)
            {
                TransactionsVcInstance = currentController as TransactionsViewController;
                TransactionsVcInstance.HideAreaCollection();
            }

            AllocatePointsVcImg();
            AllocateTransactionsVcImg();
            CheckColor(SL.Area.areaPrimaryColor.ToUIColor());
        }

        private void AllocatePointsVcImg()
        {
            var bg = PointsVcInstance?.IvBackground;
            if (bg == null)
                return;

            var globalPosition = bg.Superview.ConvertRectToView(PointsVcInstance.View.Frame, UIApplication.SharedApplication.KeyWindow);

            if (globalPosition.GetMidX() >= UIScreen.MainScreen.Bounds.GetMidX())
            {
                PointsVcInstance.CnBgCenterX.Constant = -globalPosition.GetMinX();
            }
            else
            {
                PointsVcInstance.CnBgCenterX.Constant = 0;
            }
        }

        private void AllocateTransactionsVcImg()
        {
            var bg = TransactionsVcInstance?.IvBackground;
            if (bg == null)
                return;

            var globalPosition = bg.Superview.ConvertRectToView(TransactionsVcInstance.View.Frame, UIApplication.SharedApplication.KeyWindow);

            if (globalPosition.GetMidX() <= UIScreen.MainScreen.Bounds.GetMidX())
            {
                TransactionsVcInstance.CnBgCenterX.Constant = -globalPosition.GetMinX();
            }
            else
            {
                TransactionsVcInstance.CnBgCenterX.Constant = 0;
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            pageViewController.View.Frame = mainView.Bounds;
            pageViewController.View.LayoutSubviews();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _currentAreaGUID = SL.AreaGUID;

            CheckColor(SL.Area.areaPrimaryColor.ToUIColor());

            foreach (UIView view in pageViewController.View.Subviews)
            {
                if (view is UIScrollView)
                    (view as UIScrollView).Scrolled += MainScroll_Scrolled;
            }

            if (!Glitch)
            {
                Glitch = true;
                PrevPage();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            //PointsBaseViewController currentController = (pageViewController.ViewControllers[0] as PointsBaseViewController);
            //if (currentController != null)
            //{
            //    currentController.ShouldRefreshEndpoint = false;
            //    SeparatelyShouldRefreshEndpoints[currentController.PageIndex] = false;
            //}
            //if (ShouldRefreshAllEndpoints)
            //{
            //    ShouldRefreshAllEndpoints = false;
            //}
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void PointsAndStats_TouchUpInside(PointsTabButton sender)
        {
            SetSelectedPage(0);
        }

        partial void LeaderboardButton_TouchUpInside(PointsTabButton sender)
        {
            SetSelectedPage(1);
        }

        partial void TransactionsButton_TouchUpInside(PointsTabButton sender)
        {
            SetSelectedPage(2);
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            PointsBaseViewController vc;
            
            UIStoryboard board = UIStoryboard.FromName("Main", null);
            //switch (index)
            //{
            //    //case 0:
            //    //break;
            //    case 1:
            //        vc = LeaderboardVcInstance ?? (LeaderboardVcInstance = board.InstantiateViewController("LeaderboardViewController") as LeaderboardViewController);
            //        break;
            //    case 2:
            //        vc = TransactionsVcInstance ?? (TransactionsVcInstance = board.InstantiateViewController("TransactionsViewController") as TransactionsViewController);
            //        break;
            //    default:
            //        vc = PointsVcInstance ?? (PointsVcInstance = board.InstantiateViewController("PointsViewController") as PointsViewController);
            //        break;
            //}

            //CGRect frame = vc.View.Frame;



            //vc.PageIndex = index;
            //vc.PointsContainerViewController = this;
            //return vc;
            return new UIViewController();
        }

        //public void EndpointWillAppear(PointsBaseViewController currentEndpoint)
        //{
        //    currentEndpoint.ShouldRefreshEndpoint = SeparatelyShouldRefreshEndpoints[currentEndpoint.PageIndex];
        //    SeparatelyShouldRefreshEndpoints[currentEndpoint.PageIndex] = !Glitch ? true : false;
        //}

        public bool ShouldRefreshEndpoint(int pageIndex)
        {
            return SeparatelyShouldRefreshEndpoints[pageIndex];
        }

        public void EndpointRefreshDidBegin(int pageInex)
        {
            if (_currentAreaGUID != SL.AreaGUID)
            {
                SeparatelyShouldRefreshEndpoints[0] = true;
                SeparatelyShouldRefreshEndpoints[1] = true;
                SeparatelyShouldRefreshEndpoints[2] = true;

                _currentAreaGUID = SL.AreaGUID;
            }
            SeparatelyShouldRefreshEndpoints[pageInex] = !Glitch ? true : false;
        }

        public void NextPage()
        {
            PointsBaseViewController current = pageViewController.ViewControllers[0] as PointsBaseViewController;

            var next = pageViewController.DataSource.GetNextViewController(pageViewController, current);

            var viewControllers = new UIViewController[] { next };
            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);
            next.View.LayoutSubviews();
        }

        public void PrevPage()
        {
            PointsBaseViewController current = pageViewController.ViewControllers[0] as PointsBaseViewController;

            var prev = pageViewController.DataSource.GetPreviousViewController(pageViewController, current);

            var viewControllers = new UIViewController[] { prev };
            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Reverse, true, null);
        }

        public void SetSelectedPage(int pageIndex)
        {
            PointsBaseViewController current = pageViewController.ViewControllers[0] as PointsBaseViewController;
            var model = pageSource.SetSelectedPage(pageIndex, pageViewController, current);
            pageSource.SelectedIndex = -1;
            if (model == null)
            {
                return;
            }
            var viewControllers = new UIViewController[] { model.Controller };
            pageViewController.SetViewControllers(viewControllers, model.Direction, true, null);
        }

        public void ChangeSelectedButton(int page)
        {
            UpdateTabButtons();
            if (page == 0)
            {
                PointsAndStatsButton.SetSelected();
            }
            if (page == 1)
            {
                LeaderboardButton.SetSelected();
            }
            if (page == 2)
            {
                TransactionsButton.SetSelected();
            }
        }

        private void UpdateTabButtons()
        {
            var buttons = new PointsTabButton[] { PointsAndStatsButton, LeaderboardButton, TransactionsButton };
            foreach (var button in buttons)
            {
                button.SetUnselected();
            }
        }
    }
}