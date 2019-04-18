using System;
using System.Collections.Generic;
using SocialLadder.iOS.Areas;
using SocialLadder.iOS.Models;
using SocialLadder.iOS.ViewControllers.Intro;
using UIKit;

namespace SocialLadder.iOS.Sources.Intro
{

    public class IntroPageViewControllerDataSource : UIPageViewControllerDataSource
    {
        private IntroContainerViewController _parentViewController;
        public int CurrentIndex { get; set; }
        public int NextIndex { get; set; }
        public event EventHandler ChangePage;
        public List<PageModel> PageList { get; set; }

        public IntroPageViewControllerDataSource(IntroContainerViewController viewController)
        {
            _parentViewController = viewController;

            PageList = new List<PageModel>();
            PageList.Add(new PageModel("Intro", "Intro1a"));
            PageList.Add(new PageModel("Intro", "Intro2a"));
            PageList.Add(new PageModel("Intro", "Intro3a"));
            PageList.Add(new PageModel("Intro", "Intro4a"));
            PageList.Add(new PageModel("Networks", "Networks"));
            PageList.Add(new PageModel("Areas", "Areas"));
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var vc = referenceViewController as IntroBaseViewController;
            CurrentIndex = vc.PageIndex;
            var index = vc.PageIndex;
            //index--;
            //if (index < 0)
            //    index = PageCount - 1;
            index--;
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
            var vc = referenceViewController as IntroBaseViewController;
            CurrentIndex = vc.PageIndex;
            NextIndex = vc.PageIndex;
            NextIndex++;
            ChangePage(CurrentIndex, null);
            if (NextIndex == PageCount - 1)
            {
                if (!SL.HasNetworks)
                {
                    return null;
                }
            }

            //if (NextIndex > (PageCount - 1))
            //    NextIndex = 0;
            if (NextIndex > (PageCount - 1))
            {
                NextIndex = PageCount - 1;
                return null;
            }


            return _parentViewController.ViewControllerAtIndex(NextIndex);
        }

        public int PageCount
        {
            get
            {
                return PageList.Count;
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