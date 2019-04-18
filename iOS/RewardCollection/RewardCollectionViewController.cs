using System;
using CoreGraphics;
using UIKit;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Rewards;
using SocialLadder.iOS.ViewControllers;
using SocialLadder.Models;
using System.Collections.Generic;
using Foundation;
using System.IO;
using SDWebImage;
using SocialLadder.iOS.CustomControlls;
using System.Threading.Tasks;
using SocialLadder.iOS.Helpers;

namespace SocialLadder.iOS.RewardCollection
{
    public partial class RewardCollectionViewController : SLViewController
    {
        #region properties
        public bool ShouldDrillDownImmediately { get; set; }
        public bool IsContentShouldBeCleared
        {
            get; set;
        }
        public static List<RewardCollectionItemModel> RewardList
        {
            get; set;
        }

        public int CurrentPage
        {
            get; set;
        }
        #endregion

        #region fields

        private bool _isContentLoaded;
        private UISwipeGestureRecognizer _swipe;
        private UITapGestureRecognizer _tap;
        //private UIView _overlay;
        #endregion

        #region ctors
        public RewardCollectionViewController() : base("RewardCollectionViewController", null)
        {
        }

        public RewardCollectionViewController(IntPtr handle) : base(handle)
        {
        }

        #endregion



        public override void ViewDidLoad()
        {
            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;
            base.AreaCollectionTopOutlet = cnsAreaCollectionTop;

            base.ViewDidLoad();

            ShouldDrillDownImmediately = true;

            var iv = new UIImageView(UIImage.FromBundle("more-bg.jpg"));
            iv.AddSubview(new UIView(iv.Bounds) { BackgroundColor = UIColor.FromRGBA(255, 255, 255, 128) });
            iv.ContentMode = UIViewContentMode.ScaleToFill;
            CollectionView.BackgroundView = iv;
            CollectionView.AddRefreshControl();
            AllRewardsButton.SetTitle("All Rewards");
            AvailableRewardsButton.SetTitle("Aviable");
            ClaimedButton.SetTitle("Claimed");
            
            // Perform any additional setup after loading the view, typically from a nib.
            CollectionView.RegisterNibForCell(RewardCollectionViewCell.Nib, RewardCollectionViewCell.ClassName);

            CheckToRegisterAppForRemoteNotifications();
        }


        private void CheckToRegisterAppForRemoteNotifications()
        {
            var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
            if (!RegistrationNotification.CheckIfPushNotificationsEnabled() && (notificationStatus != Enums.NotififcationStatus.Discarded.ToString()) && (notificationStatus != Enums.NotififcationStatus.Enabled.ToString()))
            {
                var alertController = UIAlertController.Create("Challenges expire quickly - allow this app to send you push notifications to be notified about special opportunities", string.Empty, UIAlertControllerStyle.Alert);

                // Add Actions
                alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, /*async*/ (a) =>
                {
                    //SL.Profile.isNotificationEnabled = true;
                    // await SaveProfile(true);
                    //await SL.Manager.SaveProfileAsync(SL.Profile, SaveProfileComplete);
                    RegistrationNotification.Register(null);
                    SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Enabled.ToString());
                    //Platform.ShowAlert("Go into your IOS Settings>SocialLadder>Notifications and make sure Allow Notifications is enabled.",string.Empty);
                }));
                alertController.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Default, (a) =>
                {
                    SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Disabled.ToString());
                }));
                alertController.AddAction(UIAlertAction.Create("Never", UIAlertActionStyle.Default, (a) =>
                {
                    SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Discarded.ToString());
                }));

                // Show the alert
                NavigationController.PresentViewController(alertController, true, null);
            }
        }

        private async Task SaveProfile(bool isNotificationEnabled)
        {
            var profile = SL.Profile;
            var newProfile = new ProfileUpdateModel()
            {
                UserName = profile.UserName,
                EmailAddress = profile.EmailAddress,
                isNotificationEnabled = isNotificationEnabled,
                isPhoneBookEnabled = false,
                isGeoEnabled = profile.isGeoEnabled,
                LocationLat = profile.LocationLat,
                LocationLon = profile.LocationLon,
                City = profile.City,
                AppVersion = profile.AppVersion
            };

            var result = await SL.Manager.UpdateProfileAsync(newProfile);
            if (result.ResponseCode > 0)
            {
                profile.isNotificationEnabled = isNotificationEnabled;
                SL.Profile = profile;
            }
        }


        public void IsRewardCollectionEmpty()
        {
            var itemsCount = CollectionView.Source.GetItemsCount(CollectionView, 0);

            if (itemsCount == 0)
            {
                EmptyCollectionView.Hidden = false;

                _swipe = new UISwipeGestureRecognizer();
                _swipe.AddTarget(() => HideAreaCollection());
                _swipe.Direction = UISwipeGestureRecognizerDirection.Up | UISwipeGestureRecognizerDirection.Down;
                EmptyCollectionView.AddGestureRecognizer(_swipe);

                _tap = new UITapGestureRecognizer();
                _tap.AddTarget(() => HideAreaCollection());
                EmptyCollectionView.AddGestureRecognizer(_tap);

                var data = NSData.FromFile("Images/panda-ohno.gif");
                var image = AnimatedImageView.GetAnimatedImageView(data, ViewForImage);
                ViewForImage.Image = image.Image;
                ViewForImage.StartAnimating();
            }

            if (itemsCount != 0)
            {
                EmptyCollectionView.Hidden = true;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            tabBarView.Hidden = true;
            float inset = (float)(this.CollectionView.ViewForBaselineLayout.Frame.Width / 16); //30.0f;

            CGRect screenRect = UIScreen.MainScreen.Bounds;
            nfloat screenWidth = screenRect.Size.Width;
            nfloat cellWidth = (screenWidth - inset * 3.0f) / 2.0f;
            CollectionView.Source = new RewardCollectionSource(this);

            CollectionView.CollectionViewLayout = new UICollectionViewFlowLayout
            {
                ItemSize = new CGSize(cellWidth, cellWidth),
                ScrollDirection = UICollectionViewScrollDirection.Vertical,
                SectionInset = new UIEdgeInsets(inset, inset, inset, inset),
                MinimumInteritemSpacing = inset,
                MinimumLineSpacing = inset
            };

            ChangeSelectedButton(0);
            CollectionView.CustomRefreshControl.ValueChanged += async (s, e) =>
            {
                if (Platform.IsInternetConnectionAvailable() == true)
                {
                    await SL.Manager.GetRewardsAsync();
                    CollectionView.ReloadData();
                    IsRewardCollectionEmpty();
                }
                CollectionView.HideLoader();
            };

            RefreshAsync();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        async void RefreshChallenges()
        {
            await SL.Manager.GetChallengesAsync();
        }

        async Task RefreshProfile()
        {
            await SL.Manager.GetProfileAsync();
            AreaCollection.ReloadData();
            UpdateNavBar();
        }

        private void UpdateTabButtons()
        {
            var buttons = new RewardsTabButton[] { AllRewardsButton, AvailableRewardsButton, ClaimedButton };
            foreach (var button in buttons)
            {
                button.SetUnselected();
            }
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
                AvailableRewardsButton.SetSelected();
            }
            if (page == 2)
            {
                ClaimedButton.SetSelected();
            }
            CurrentPage = page;
        }

        private async Task RefreshRewards()
        {
            ShowLoader();
            await SL.Manager.GetRewardsAsync(RewardsRefreshCompleted);
        }

        private void ShowLoader()
        {
            if (!_isContentLoaded)
            {
                CollectionView.ShowLoader();
                return;
            }
            //_overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
        }

        private void HideLoader()
        {
            if (_isContentLoaded)
            {
                CollectionView.HideLoader();
                return;
            }
            //_overlay.RemoveFromSuperview();
        }

        async void RefreshFeed()
        {
            await SL.Manager.GetFeedAsync();
        }

        public async override Task Refresh()
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }
            await RefreshRewards();
            //RefreshChallenges();
            //RefreshFeed();
            await RefreshProfile();
            await base.Refresh();
        }

        private async void RefreshAsync()
        {
            await Refresh();
        }

        partial void AllRewardsButton_TouchUpInside(RewardsTabButton sender)
        {
            ChangeSelectedButton(0);
        }

        partial void AviableRewardsButton_TouchUpInside(RewardsTabButton sender)
        {
            ChangeSelectedButton(1);
        }

        partial void ClaimedButton_TouchUpInside(RewardsTabButton sender)
        {
            ChangeSelectedButton(2);
        }

        private void RewardsRefreshCompleted(RewardResponseModel rewardResponse)
        {
            //if (ShouldDrillDownImmediately && SL.RewardList?.Count == 1 && SL.RewardList[0].Type == "CATEGORY")
            //{
            //    var reward = SL.RewardList[0];
            //    IsContentShouldBeCleared = true;
            //    UIStoryboard board = UIStoryboard.FromName("Rewards", null);
            //    RewardsViewController ctrl = (RewardsViewController)board.InstantiateViewController("RewardsViewController") as RewardsViewController;
            //    ctrl.RewardsCategoryName = reward.Name;
            //    if (reward.ChildList != null)
            //        ctrl.RewardList = reward.ChildList;
            //    else
            //    {
            //        ctrl.CategoryID = reward.ID;
            //        SL.RewardList = null;
            //    }
            //    ShouldDrillDownImmediately = false;



            //    UIViewController parentController = Platform.RootViewController;
            //    if (parentController?.GetType() == typeof(SLNavigationController))
            //    {
            //        ctrl.IsChangingAreasDisabled = false;
            //        ctrl.IsCategoryPageSkiped = true;
            //        var child = parentController.ChildViewControllers[0];
            //        (child as MainViewController).SwapInViewController(ctrl, (int)ENavigationTabs.REWARDS);
            //        return;
            //    }

            //    parentController = Platform.TopViewController;
            //    if (parentController?.GetType() == typeof(SLNavigationController))
            //    {
            //        var child = parentController.ChildViewControllers[0];
            //        ctrl.IsChangingAreasDisabled = false;
            //        ctrl.IsCategoryPageSkiped = true;
            //        (child as MainViewController).SwapInViewController(ctrl, (int)ENavigationTabs.REWARDS);
            //        return;
            //    }


            //    //if (parentController?.GetType() == typeof(SLNavigationController))
            //    //{
            //    //    var child = parentController.ChildViewControllers[0];
            //    //    (child as MainViewController).SelectedIndex = (int)2;
            //    //    return;
            //    //} 

            //    //parentController = Platform.TopViewController;
            //    //if (parentController?.GetType() == typeof(SLNavigationController))
            //    //{
            //    //    var child = parentController.ChildViewControllers[0];
            //    //    (child as MainViewController).SelectedIndex = (int)tabToNavigate;
            //    //    return;
            //    //}

            //    //SLNavigationController viewController = storyboard.InstantiateViewController(viewControllerName) as SLNavigationController;
            //    //parentController.PresentViewController(viewController, false, null);
            //    //NavigationController.PushViewController(ctrl, true);
            //}

            IsContentShouldBeCleared = false;
            ShouldDrillDownImmediately = true;

            CollectionView.ReloadData();
            IsRewardCollectionEmpty();

            
            _isContentLoaded = true;
            HideLoader();
        }
    }
}

