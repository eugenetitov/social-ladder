using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using SocialLadder.iOS.Challenges;
using SocialLadder.iOS.Connectivity;
using SocialLadder.iOS.Models.Enums;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Points;
using SocialLadder.iOS.RewardCollection;
using SocialLadder.iOS.ViewControllers.Base;
using SocialLadder.ViewModels.Feed;
using SocialLadder.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UIKit;
using Xamarin.Essentials;

namespace SocialLadder.iOS.ViewControllers.Main
{
    [MvxFromStoryboard("Main")]
    [MvxRootPresentation]
    public partial class MainViewController : MvxTabBarViewController<MainViewModel>
    {
        public MainViewController (IntPtr handle) : base (handle)
        {
            
        }

        private bool _firstTimePresented = true;
        private ConnectionMessageView _connectionMessageView;

        public Dictionary<ENavigationTabs, int> TabLookUp
        {
            get; set;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = e.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                // Connection to internet is available
                _connectionMessageView?.RemoveFromSuperview();
                return;
            }
            if (_connectionMessageView == null)
            {
                var messageView = ConnectionMessageView.Create();
                messageView.Frame = new CGRect(0, this.TabBar.Frame.Y - messageView.Frame.Height, messageView.Frame.Width, messageView.Frame.Height);
                _connectionMessageView = messageView;
                _connectionMessageView.ClipsToBounds = true;
                this.View.AddSubview(_connectionMessageView);
                return;
            }
            this.View.AddSubview(_connectionMessageView);

        }

        public override void ViewWillAppear(bool animated)
        {
            //if (this.ViewControllers[2].GetType() != typeof(RewardCollectionViewController))
            //{
            //    UIViewController parentController = Platform.RootViewController;
            //    if (parentController?.GetType() == typeof(SLNavigationController))
            //    {
            //        var child = parentController.ChildViewControllers[0];
            //        (child as MainViewController).SwapInViewController("Rewards", "RewardCollectionViewController", ENavigationTabs.REWARDS);
            //    }
            //}
            var viewModel = this.ViewModel;
            SetupTapBar();

            var current = Xamarin.Essentials.Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                var messageView = ConnectionMessageView.Create();
                messageView.Frame = new CGRect(0, this.TabBar.Frame.Y - ConnectionMessageView.CollectionViewHeight, messageView.Frame.Width, ConnectionMessageView.CollectionViewHeight);
                _connectionMessageView = messageView;
                this.View.AddSubview(_connectionMessageView);
            }

            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            base.ViewWillAppear(animated);
        }

        public override void ViewDidLayoutSubviews()
        {
            //areacollection show workaround
            if (_connectionMessageView != null)
            {
                _connectionMessageView.Frame = new CGRect(0, this.TabBar.Frame.Y - ConnectionMessageView.CollectionViewHeight, _connectionMessageView.Frame.Width, ConnectionMessageView.CollectionViewHeight);
            }

            base.ViewDidLayoutSubviews();
        }

        public override void ViewWillDisappear(bool animated)
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
           
            CrashlyticsKit.Crashlytics.Instance.SetStringValue("UserName", SL.Profile?.UserName);


        }


        public void SetupTapBar()
        {
            if (_firstTimePresented)
            {
                _firstTimePresented = false;
                ViewModel.ShowFeedViewModelCommand.Execute(null);
                ViewModel.ShowRewardsViewModelCommand.Execute(null);
                ViewModel.ShowChallengesViewModelCommand.Execute(null);
                ViewModel.ShowMoreViewModelCommand.Execute(null);
            }
        }

        private UIViewController CreateTabFor(string title, string imageName, string selectedImageName, IMvxViewModel viewModel)
        {
            var controller = new UINavigationController();
            var screen = this.CreateViewControllerFor(viewModel) as UIViewController;
            screen.TabBarItem = new UITabBarItem(title, UIImage.FromBundle(imageName), UIImage.FromBundle(selectedImageName));

            //Just for example, use the previous line
           // screen.TabBarItem = new UITabBarItem(UITabBarSystemItem.Search, index);
            controller.PushViewController(screen, false);
            return controller;
        }


        public async void RefreshAreas()
        {
            var viewController = (SelectedViewController as SLViewController) ?? (SelectedViewController?.ParentViewController as SLViewController) ?? (SelectedViewController?.ParentViewController?.ParentViewController as SLViewController);

            if (viewController == null)
            {
                Debug.WriteLine($"Cannot cast SelectedViewController{SelectedViewController} as SLViewController to RefreshAreas()");
            }
            await viewController?.RefreshAreas();
        }

        //public void SwapInViewController(string storyboard, string viewController, ENavigationTabs tab, bool animated = true)
        //{
        //    if (TabLookUp != null)
        //    {
        //        int index = TabLookUp[tab];
        //        SwapInViewController(storyboard, viewController, index, animated);
        //    }
        //    else
        //        Debug.WriteLine(@"Attempted to swapin tab by enum before tablookup built, make sure to build before calling or swap in by index if cant build before");


        //}

        //public override void ItemSelected(UITabBar tabbar, UITabBarItem item)
        //{
        //    if (this.ViewControllers[2].GetType() != typeof(RewardCollectionViewController))
        //    {
        //        UIViewController parentController = Platform.RootViewController;
        //        if (parentController?.GetType() == typeof(SLNavigationController))
        //        {
        //            var child = parentController.ChildViewControllers[0];
        //            (child as MainViewController).SwapInViewController("Rewards", "RewardCollectionViewController", ENavigationTabs.REWARDS);
        //            return;
        //        }
        //    }
        //}

        //void SwapInViewController(string storyboard, string viewController, int index, bool animated = true)
        //{
        //    UIViewController[] controllers = this.ViewControllers;
        //    if (0 <= index && index < controllers.Length)
        //    {
        //        UIStoryboard board = UIStoryboard.FromName(storyboard, null);
        //        if (board != null)
        //        {
        //            UIViewController controller = (UIViewController)board.InstantiateViewController(viewController);
        //            if (controller != null)
        //            {
        //                Debug.WriteLine(@"Starting Swap in " + storyboard + " " + viewController + " at " + index);

        //                controller.TabBarItem = controllers[index].TabBarItem;
        //                controller.Title = controllers[index].Title;
        //                controllers[index] = controller;
        //                this.SetViewControllers(controllers, animated);

        //                Debug.WriteLine(@"Completed Swap in " + storyboard + " " + viewController + " at " + index);
        //            }
        //        }
        //    }
        //}

        //public void SwapInViewController(UIViewController controller, int index, bool animated = true)
        //{
        //    UIViewController[] controllers = this.ViewControllers;
        //    if (0 <= index && index < controllers.Length)
        //    {
        //        if (controller != null)
        //        {
        //            Debug.WriteLine(@"Starting Swap in " + controller.Title + " at " + index);

        //            controller.TabBarItem = controllers[index].TabBarItem;
        //            controller.Title = controllers[index].Title;
        //            controllers[index] = controller;
        //            this.SetViewControllers(controllers, animated);

        //            Debug.WriteLine(@"Completed Swap in " + controller.Title + " at " + index);
        //        }
        //    }
        //}

        public void SelectTab(ENavigationTabs tab)
        {
            if (TabLookUp != null)
                this.SelectedIndex = TabLookUp[tab];
            else
                Debug.WriteLine(@"Attempted to select tab by type befre tablookup table exists");
        }
    }
}