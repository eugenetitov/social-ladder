using SocialLadder.iOS.Points.Models;
using System;
using System.Collections.Generic;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public class PointsPageViewControllerDataSource : UIPageViewControllerDataSource
    {
        private PointsContainerViewController _parentViewController;
        public int CurrentIndex { get; set; } = -1;
        public int SelectedIndex { get; set; } = -1;
        public event EventHandler ChangePage;

        public PointsPageViewControllerDataSource(PointsContainerViewController viewController)
        {
            _parentViewController = viewController;
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var vc = referenceViewController as PointsBaseViewController;
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
            index--;
            //if (index < 0)
            //    index = PageCount - 1;
            if (SelectedIndex != -1)
            {
                CurrentIndex = index = SelectedIndex;
            }

            ChangePage?.Invoke(CurrentIndex, null);
            if (index < 0)
            {
                index = 0;
                return null;
            }
            return _parentViewController.ViewControllerAtIndex(index);
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var vc = referenceViewController as PointsBaseViewController;
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
            //if (index > (PageCount - 1))
            //    index = 0;
            if (SelectedIndex != -1)
            {
                CurrentIndex = index = SelectedIndex;

            }
            ChangePage?.Invoke(CurrentIndex, null);
            if (index > (PageCount - 1))
            {
                index = PageCount - 1;
                return null;
            }
            return _parentViewController.ViewControllerAtIndex(index);
        }

        public LocalPointsModel SetSelectedPage(int pageIndex, UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var model = new LocalPointsModel();
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

        //public override nint GetPresentationCount(UIPageViewController pageViewController)
        //{
        //    return PageCount;
        //}

        //public override nint GetPresentationIndex(UIPageViewController pageViewController)
        //{
        //    return 0;
        //}

    }
}