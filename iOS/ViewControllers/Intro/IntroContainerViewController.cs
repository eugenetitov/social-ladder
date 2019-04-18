using AVFoundation;
using CoreGraphics;
using Foundation;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using SocialLadder.iOS.Areas;
using SocialLadder.iOS.Models;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Services;
using SocialLadder.iOS.Sources.Intro;
using SocialLadder.iOS.ViewControllers;
using SocialLadder.iOS.ViewControllers.Base;
using SocialLadder.iOS.ViewControllers.Intro;
using SocialLadder.ViewModels.Intro;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    [MvxFromStoryboard("Intro")]
    [MvxRootPresentation]
    public partial class IntroContainerViewController : BaseViewController<IntroContainerViewModel>
    {
        UIPageViewController pageViewController;
        IntroPageViewControllerDataSource dataSource;
        bool Glitch { get; set; }   //first time we try to open the areacollection (this layout uses a scrollview and pagecontroller), the constraints do not seem to update and the areacollection appears covered by the below view (constraint from areacollection.bottom to view.top). this glitch loads the second vc and switches to the first on viewwillappear because during testing, it was found that after switching pages, the it does not have this problem (this glitch simulates those steps)

        public IntroContainerViewController(IntPtr handle) : base(handle)
        {
            //SL.Manager.LogUserEvent("test");
        }

        void ApplyStyle()
        {
            Platform.AddVideo(View);
            Platform.AddCover(View);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AVAudioSession.SharedInstance().SetCategory(AVAudioSessionCategory.Ambient);
            AVAudioSession.SharedInstance().SetActive(true);
            UIApplication.SharedApplication.BeginReceivingRemoteControlEvents();

            ApplyStyle();

            UIStoryboard board = UIStoryboard.FromName("Intro", null);
            pageViewController = board.InstantiateViewController("IntroPageViewController") as UIPageViewController;
            dataSource = new IntroPageViewControllerDataSource(this);
            pageViewController.DataSource = dataSource;

            var startVC = this.ViewControllerAtIndex(0) as IntroBaseViewController;    //start on second page for glitch (should start on page index 0 without glitch)
            var viewControllers = new UIViewController[] { startVC };

            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, false, null);
            pageViewController.View.Frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Size.Height);
            AddChildViewController(this.pageViewController);
            View.AddSubview(this.pageViewController.View);
            pageViewController.DidMoveToParentViewController(this);

            UpdateViews();
            View.BringSubviewToFront(btn_Next);
            btn_Next.TouchUpInside += (s, e) =>
            {
                Traverse();
            };

            dataSource.ChangePage += (s, e) =>
            {
                var index = (int)s;
                ChangeProgressViewValue(index);
            };

            View.BringSubviewToFront(LoginButton);


        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public void ChangeProgressViewValue(int index)
        {
            float progress = 0;
            if (index > 0)
            {
                if (index == 5 && !SL.HasNetworks)
                {
                    return;
                }
                progress = (float)index / (float)(dataSource.PageList.Count - 1);
            }
            else if (index == 0)
            {
                progress = 0.05f;
            }
            InvokeOnMainThread(() => { prb_ProgressValue.Progress = progress; });
        }

        void Traverse()
        {
            try
            {
                AreasViewController areas = pageViewController.ViewControllers[0] as AreasViewController;
                if (areas != null && SL.HasAreas)
                {
                    SL.Profile.SetDefaultAreaIfNeeded();

                    List<string> areaImageUrlList = SL.AreaList.Select(a => a.areaDefaultImageURL).ToList();
                    FileCachingService.PreloadImagesToDiskFromUrl(areaImageUrlList);

                    LoadMain();
                }
                else
                    NextPage();
            }
            catch (Exception)
            {
            }
        }

        public void LoadMain()
        {
            List<string> areaImageUrlList = SL.AreaList.Select(a => a.areaDefaultImageURL).ToList();
            FileCachingService.PreloadImagesToDiskFromUrl(areaImageUrlList);

            ((IntroContainerViewModel)ViewModel).NavigateToMainView.Execute();
            //UIStoryboard board = UIStoryboard.FromName("Main", null);
            //UIViewController ctrl = (UIViewController)board.InstantiateViewController("MainViewController");
            //this.PresentViewController(ctrl, false, null);
        }

        public void NextPage()
        {
            IntroBaseViewController current = pageViewController.ViewControllers[0] as IntroBaseViewController;
            var next = dataSource.GetNextViewController(pageViewController, current);
            var viewControllers = new UIViewController[] { next };
            ChangeProgressViewValue(dataSource.NextIndex);
            if (next == null && dataSource.CurrentIndex > 0)
            {
                //LoginCommand();
                return;
            }
            pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Forward, true, null);
        }

        public void PrevPage()
        {
            IntroBaseViewController current = pageViewController.ViewControllers[0] as IntroBaseViewController;
            IntroBaseViewController prev = dataSource.GetPreviousViewController(pageViewController, current) as IntroBaseViewController;
            if (prev != null)
            {
                var viewControllers = new UIViewController[] { prev };
                pageViewController.SetViewControllers(viewControllers, UIPageViewControllerNavigationDirection.Reverse, true, null);
            }
        }

        public void GoToPage(int index, UIPageViewControllerNavigationDirection direction)
        {
            IntroBaseViewController vc = ViewControllerAtIndex(index) as IntroBaseViewController;

            var viewControllers = new UIViewController[] { vc };
            pageViewController.SetViewControllers(viewControllers, direction, true, null);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            IntroBaseViewController vc;
            if (0 <= index && index < dataSource.PageList.Count)
            {
                PageModel page = dataSource.PageList[index];
                UIStoryboard board = UIStoryboard.FromName(page.Storyboard, null);
                vc = board.InstantiateViewController(page.ViewController) as IntroBaseViewController;
                vc.PageIndex = index;
                vc.IntroContainerViewController = this;
            }
            else
                vc = null;
            return vc;
        }

        partial void LoginButton_TouchUpInside(UIButton sender)
        {
            int networksPageIndex = 4;
            GoToPage(networksPageIndex, UIPageViewControllerNavigationDirection.Forward);
        }

        private void UpdateViews()
        {
            LoginButton.Font = btn_Next.Font = UIFont.FromName("ProximaNova-Regular", btn_Next.Frame.Height * 0.85f);

            prb_ProgressValue.Layer.CornerRadius = prb_ProgressValue.Layer.Sublayers[1].CornerRadius = 6f;
            prb_ProgressValue.ClipsToBounds = prb_ProgressValue.Subviews[1].ClipsToBounds = true;

            prb_ProgressBackground.Layer.CornerRadius = prb_ProgressBackground.Layer.Sublayers[1].CornerRadius = 8f;
            prb_ProgressBackground.ClipsToBounds = prb_ProgressValue.Subviews[1].ClipsToBounds = true;
        }

        public void PrepareForIntro()
        {
            btn_Next.SetTitle("Next", UIControlState.Normal);
            LoginButton.Hidden = false;
            btn_Next.TouchUpInside -= DoneButtonNoAreasAction;
        }

        public void PrepareForAreas()
        {
            btn_Next.SetTitle("Done", UIControlState.Normal);
            LoginButton.Hidden = true;
            if (!SL.HasAreas)
                btn_Next.TouchUpInside += DoneButtonNoAreasAction;
        }

        public void PrepareForNetworks()
        {
            btn_Next.SetTitle("Next", UIControlState.Normal);
            LoginButton.Hidden = true;
            btn_Next.TouchUpInside -= DoneButtonNoAreasAction;
        }

        public void DoneButtonNoAreasAction(object sender, EventArgs e)
        {
            if (!SL.HasAreas)
                Platform.ShowAlert(null, "You must join an area to complete setup!");
        }
    }
}