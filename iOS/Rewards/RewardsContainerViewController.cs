using Foundation;
using SocialLadder.iOS.Rewards;
using System;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class RewardsContainerViewController : UIViewController
    {
        UIPageViewController pageViewController;
        RewardsPageViewControllerDataSource pageSource;
        public nfloat PageYOffset
        {
            get; set;
        }

        bool Glitch
        {
            get; set;
        }   //first time we try to open the areacollection (this layout uses a scrollview and pagecontroller), the constraints do not seem to update and the areacollection appears covered by the below view (constraint from areacollection.bottom to view.top). this glitch loads the second vc and switches to the first on viewwillappear because during testing, it was found that after switching pages, the it does not have this problem (this glitch simulates those steps)

        public RewardsContainerViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            tabBarView.Hidden = true;
            UIStoryboard board = UIStoryboard.FromName("Main", null);
            pageViewController = board.InstantiateViewController("PointsPageViewController") as UIPageViewController;
            pageSource = new RewardsPageViewControllerDataSource(this);

            pageViewController.DataSource = pageSource;

            var startVC = this.ViewControllerAtIndex(1) as RewardsBaseViewController;    //start on second page for glitch (should start on page index 0 without glitch)
            var viewControllers = new UIViewController[] { startVC };

            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
            //pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height - 50);
            pageViewController.View.Frame = mainView.Frame;
           
            AddChildViewController(this.pageViewController);
            mainView.AddSubview(this.pageViewController.View);
            pageViewController.DidMoveToParentViewController(this);

            //startVC.ShowAreaCollection();
            //startVC.HideAreaCollection();
            //PrevPage();

            pageSource.ChangePage += (s, e) =>
            {
                var index = (int)s;
                ChangeSelectedButton(index);
            };
            AllRewardsButton.SetTitle("All Rewards");
            AviableRewardsButton.SetTitle("Aviable");
            ClaimedButton.SetTitle("Claimed");
            AllRewardsButton.SetSelected();
        }

        public override void ViewWillAppear(bool animated)
        {
          
            base.ViewWillAppear(animated);
         
            if (!Glitch)
            {
                PrevPage();
                Glitch = true;
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

        }

        public void SetSelectedPage(int pageIndex)
        {
            RewardsBaseViewController current = pageViewController.ViewControllers[0] as RewardsBaseViewController;
            var model = pageSource.SetSelectedPage(pageIndex, pageViewController, current);
            pageSource.SelectedIndex = -1;
            if (model == null)
            {
                return;
            }
            var viewControllers = new UIViewController[] { model.Controller };
            pageViewController.SetViewControllers(viewControllers, model.Direction, true, null);
        }

        public void NextPage()
        {
            RewardsBaseViewController current = pageViewController.ViewControllers[0] as RewardsBaseViewController;

            var next = pageSource.GetNextViewController(pageViewController, current);
            var viewControllers = new UIViewController[] { next };
            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);
        }

        public void PrevPage()
        {
            RewardsBaseViewController current = pageViewController.ViewControllers[0] as RewardsBaseViewController;
            var prev = pageViewController.DataSource.GetPreviousViewController(pageViewController, current);
            var viewControllers = new UIViewController[] { prev };
            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Reverse, true, null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            RewardsBaseViewController vc;
            UIStoryboard board = UIStoryboard.FromName("Rewards", null);
            switch (index)
            {
                case 0:
                    vc = board.InstantiateViewController("RewardsViewController") as RewardsBaseViewController;
                    break;
                case 1:
                    vc = board.InstantiateViewController("AviableRewardsViewController") as RewardsBaseViewController;
                    break;
                default:
                    vc = board.InstantiateViewController("ClaimedRewardsViewController") as RewardsBaseViewController;
                    break;
            }
            vc.PageIndex = index;
            vc.RewardsContainerViewController = this;
            return vc;
        }

        private void UpdateTabButtons()
        {
            var buttons = new RewardsTabButton[] { AllRewardsButton, AviableRewardsButton, ClaimedButton };
            foreach (var button in buttons)
            {
                button.SetUnselected();
            }
        }


        partial void AllRewardsButton_TouchUpInside(RewardsTabButton sender)
        {
            SetSelectedPage(0);
        }

        partial void AviableButton_TouchUpInside(RewardsTabButton sender)
        {
            SetSelectedPage(1);
        }

        partial void TransactionsButton_TouchUpInside(RewardsTabButton sender)
        {
            SetSelectedPage(2);
        }

        private void ChangeSelectedButton(int page)
        {
            UpdateTabButtons();
            if (page == 0)
            {
                AllRewardsButton.SetSelected();
            }
            if (page == 1)
            {
                AviableRewardsButton.SetSelected();
            }
            if (page == 2)
            {
                ClaimedButton.SetSelected();
            }
        }

    }
}