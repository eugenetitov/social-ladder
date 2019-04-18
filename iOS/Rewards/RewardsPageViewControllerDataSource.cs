using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.iOS.Rewards.Models;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public class RewardsPageViewControllerDataSource : UIPageViewControllerDataSource
    {
        private RewardsContainerViewController _parentViewController;
        public int CurrentIndex { get; set; } = -1;
        public int SelectedIndex { get; set; } = -1;
       
        public event EventHandler ChangePage;

        public RewardsPageViewControllerDataSource(RewardsContainerViewController viewController)
        {
            _parentViewController = viewController;
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var vc = referenceViewController as RewardsBaseViewController;
            var index = vc.PageIndex; 
            if (CurrentIndex == -1)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex = vc.PageIndex;
            }
            index--; 
            if (SelectedIndex != -1)
            {
                CurrentIndex = index = SelectedIndex;
            }

            ChangePage(CurrentIndex, null);
            if (index < 0)
            {
                index = 0;
                return null;
            }
            return _parentViewController.ViewControllerAtIndex(index);
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var vc = referenceViewController as RewardsBaseViewController;
            var index = vc.PageIndex;
            //CurrentIndex = vc.PageIndex;
            if (CurrentIndex == -1)
            {
                CurrentIndex = 0;
            }
            else
            {
                CurrentIndex = vc.PageIndex;
            }
            index++; 
            if (SelectedIndex != -1)
            {
                CurrentIndex = index = SelectedIndex;

            }
            ChangePage(CurrentIndex, null);
            if (index > (PageCount - 1))
            {
                index = PageCount - 1;
                return null;
            }
            return _parentViewController.ViewControllerAtIndex(index);
        }

        public LocalRewardsModel SetSelectedPage(int pageIndex, UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var model = new LocalRewardsModel();
            SelectedIndex = pageIndex;
            if (SelectedIndex != -1 && SelectedIndex > CurrentIndex)
            {
                model.Controller = GetNextViewController(pageViewController, referenceViewController);
                model.Direction = UIPageViewControllerNavigationDirection.Forward;
                return model;
            }
            if (SelectedIndex != -1 && SelectedIndex < CurrentIndex)
            {
                model.Controller = GetPreviousViewController(pageViewController, referenceViewController);
                model.Direction = UIPageViewControllerNavigationDirection.Reverse;
                return model;
            }
            return null;
        }

        int PageCount
        {
            get
            {
                return 3;
            }
        }

    }
}