using System;
using UIKit;
using CoreGraphics;
using SocialLadder.iOS.Navigation;
using SocialLadder.Models;
using SocialLadder.Interfaces;
using SocialLadder.iOS.Services;
using Facebook.ShareKit;
using Foundation;
using Social;
using System.Diagnostics;
using SocialLadder.iOS.Constraints;
using System.Collections.Generic;
using MessageUI;
using System.Threading.Tasks;
using SocialLadder.iOS.Helpers;
using CrashlyticsKit;
using SocialLadder.iOS.ViewControllers.Main;
using SocialLadder.ViewModels.Points;
using SocialLadder.ViewModels.Challenges;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace SocialLadder.iOS.Challenges
{
    [MvxFromStoryboard("Main")]

    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Challenges", TabIconName = "challenges-icon_on", TabSelectedIconName = "challenges-icon_off")]
    public partial class ChallengesViewController : BaseTabViewController<ChallengeViewModel>//, Facebook.ShareKit.ISharingDelegate
    {
        public ChallengesViewController() : base("ChallengesViewController", null)
        {
        }

        public ChallengesViewController(IntPtr handle) : base(handle)
        {
        }

        //string _areaName;
        //string _challengeTypeFilter;
        //public string ChallengeTypeFilter
        //{
        //    get
        //    {
        //        if (_areaName != SL.AreaName && ((((ChallengeCollectionSource)CollectionView?.Source)?.ChallengesTypes?.Count ?? 0) > 0))
        //        {
        //            _areaName = SL.AreaName;
        //            ChallengeTypeFilter = null;
        //            CollectionView.SelectedItem = -1;
        //            CollectionView.ReloadData();
        //            CollectionView.ScrollRectToVisible(new CGRect(0, 0, 1, 1), false);
        //        }
        //        return _challengeTypeFilter;
        //    }
        //    set
        //    {
        //        var previous = _challengeTypeFilter;
        //        _challengeTypeFilter = value;
        //        if (previous != value)
        //        {
        //            TableView.ReloadData();
        //            if ((((ChallengesTableSource)TableView?.Source)?.ItemsSource?.Count ?? 0) > 0)
        //            {
        //                TableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, true);
        //            }
        //        }
        //    }
        //}

        ////private UIView _overlay;
        ////private bool _isContentLoaded;
        //private ChallengeCollectionSource _challengeCollectionSource;

        //void ApplyStyle()
        //{
        //    View.InsertSubview(new UIImageView(UIImage.FromBundle("more-bg")), 0);
        //}

        //private void CheckToRegisterAppForRemoteNotifications()
        //{

        //    var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
        //    if (!RegistrationNotification.CheckIfPushNotificationsEnabled() && (notificationStatus != Enums.NotififcationStatus.Discarded.ToString()) && (notificationStatus != Enums.NotififcationStatus.Enabled.ToString()))
        //    {
        //        var alertController = UIAlertController.Create("Challenges expire quickly - allow this app to send you push notifications to be notified about special opportunities", string.Empty, UIAlertControllerStyle.Alert);

        //        // Add Actions
        //        alertController.AddAction(UIAlertAction.Create("Yes", UIAlertActionStyle.Default, /*async*/ (a) =>
        //        {
        //            //SL.Profile.isNotificationEnabled = true;
        //            // await SaveProfile(true);
        //            //await SL.Manager.SaveProfileAsync(SL.Profile, SaveProfileComplete);
        //            RegistrationNotification.Register(null);
        //            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Enabled.ToString());
        //            //Platform.ShowAlert("Go into your IOS Settings>SocialLadder>Notifications and make sure Allow Notifications is enabled.",string.Empty);
        //        }));
        //        alertController.AddAction(UIAlertAction.Create("No", UIAlertActionStyle.Default, (a) =>
        //        {
        //            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Disabled.ToString());
        //        }));
        //        alertController.AddAction(UIAlertAction.Create("Never", UIAlertActionStyle.Default, (a) =>
        //        {
        //            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Discarded.ToString());
        //        }));

        //        // Show the alert
        //        NavigationController.PresentViewController(alertController, true, null);
        //    }
        //}

        //private async Task SaveProfile(bool isNotificationEnabled)
        //{
        //    var profile = SL.Profile;
        //    var newProfile = new ProfileUpdateModel()
        //    {
        //        UserName = profile.UserName,
        //        EmailAddress = profile.EmailAddress,
        //        isNotificationEnabled = isNotificationEnabled,
        //        isPhoneBookEnabled = false,
        //        isGeoEnabled = profile.isGeoEnabled,
        //        LocationLat = profile.LocationLat,
        //        LocationLon = profile.LocationLon,
        //        City = profile.City,
        //        AppVersion = profile.AppVersion
        //    };

        //    var result = await SL.Manager.UpdateProfileAsync(newProfile);
        //    if (result.ResponseCode > 0)
        //    {
        //        profile.isNotificationEnabled = isNotificationEnabled;
        //        SL.Profile = profile;
        //    }
        //}

        //public void SaveProfileComplete(ProfileResponseModel response)
        //{
        //    SL.Profile = response.Profile;
        //}

        //public override void ViewDidLoad()
        //{
        //    Crashlytics.Instance.Log("Challenges.ChallengesViewController.ViewDidLoad()");
        //    base.AreaCollectionOutlet = AreaCollection;
        //    base.AreaCollectionHeightOutlet = AreaCollectionHeight;
        //    base.AreaCollectionTopOutlet = cnsAreaCollectionTop;

        //    SetupEmptyCollectionView();

        //    TableView.AddRefreshControl();
        //    TableView.RegisterNibForCellReuse(ChallengesTableViewCell.Nib, ChallengesTableViewCell.ClassName);
        //    TableView.ViewController = this;

        //    CollectionView.RegisterNibForCell(ChallengeCollectionViewCell.Nib, ChallengeCollectionViewCell.ClassName);
        //    CollectionView.ViewController = this;

        //    CheckToRegisterAppForRemoteNotifications();
        //    ApplyStyle();

        //    SetupTableView();
        //    SetupCollectionView();

        //    base.ViewDidLoad();
        //}

        //UISwipeGestureRecognizer Swipe;
        //UITapGestureRecognizer Tap;

        //private void SetupEmptyCollectionView()
        //{
        //    EmptyCollectionView.Hidden = true;

        //    Swipe = new UISwipeGestureRecognizer();
        //    Swipe.AddTarget(() => HideAreaCollection());
        //    Swipe.Direction = UISwipeGestureRecognizerDirection.Up | UISwipeGestureRecognizerDirection.Down;
        //    EmptyCollectionView.AddGestureRecognizer(Swipe);

        //    Tap = new UITapGestureRecognizer();
        //    Tap.AddTarget(() => HideAreaCollection());
        //    EmptyCollectionView.AddGestureRecognizer(Tap);

        //    var data = NSData.FromFile("Images/image.gif");
        //    var image = AnimatedImageView.GetAnimatedImageView(data, ViewForImage);
        //    ViewForImage.Image = image.Image;
        //}

        //public override void ViewDidLayoutSubviews()
        //{
        //    base.ViewDidLayoutSubviews();

        //    //SetupEmptyCollectionView();
        //}

        //private static UIImage FromUrl(string uri)
        //{
        //    using (var url = new NSUrl(uri))
        //    using (var data = NSData.FromUrl(url))
        //        return UIImage.LoadFromData(data);
        //}

        //public override void DidReceiveMemoryWarning()
        //{
        //    base.DidReceiveMemoryWarning();
        //    // Release any cached data, images, etc that aren't in use.
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    Crashlytics.Instance.Log("Challenges.ChallengesViewController.ViewWillAppear()");

        //    base.ViewWillAppear(animated);
        //    RefreshAsync();
        //    // LoadChallengeDetail(new ChallengeModel { TypeCode = "FB ENGAGEMENT" });
        //}


        //private void SetupTableView()
        //{
        //    Crashlytics.Instance.Log("Challenges.ChallengesViewController.SetupTableView()");
        //    TableView.Source = new ChallengesTableSource(TableView, this);
        //    TableView.RowHeight = UITableView.AutomaticDimension;
        //    TableView.EstimatedRowHeight = SizeConstants.Screen.Width * 0.4f;
        //    TableView.SetNeedsLayout();
        //    TableView.LayoutIfNeeded();
        //    TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        //    TableView.ReloadData();
        //    TableView.CustomRefreshControl.ValueChanged += async (s, e) =>
        //    {
        //        if (Platform.IsInternetConnectionAvailable() == true)
        //        {

        //            await SL.Manager.GetChallengesAsync();
        //            TableView.ReloadData();
        //        }
        //        TableView.HideLoader();
        //    };
        //}

        //private void SetupCollectionView()
        //{
        //    Crashlytics.Instance.Log("Challenges.ChallengesViewController.SetupCollectionView()");
        //    cnCollectionViewHeight.Constant = SizeConstants.ScreenWidth * 0.535f;

        //    nfloat inset = SizeConstants.Screen.Width * 0.045f;
        //    nfloat size = SizeConstants.Screen.Width * 0.39f;
        //    if (_challengeCollectionSource == null)
        //    {
        //        _challengeCollectionSource = new ChallengeCollectionSource();
        //    }
        //    _challengeCollectionSource.ChallengeItemSelected += OnChallengeItemSelected;
        //    CollectionView.Source = _challengeCollectionSource;
        //    UIImageView backgroundImage = new UIImageView(UIImage.FromBundle("more-bg"));
        //    backgroundImage.Alpha = 0.5f;
        //    CollectionView.BackgroundView = backgroundImage;
        //    CollectionView.CollectionViewLayout = new UICollectionViewFlowLayout
        //    {
        //        ItemSize = new CGSize(size, size),
        //        ScrollDirection = UICollectionViewScrollDirection.Horizontal,
        //        SectionInset = new UIEdgeInsets(inset, inset, inset, inset),
        //        //MinimumInteritemSpacing = UIScreen.MainScreen.Bounds.Width * 0.05f,
        //        MinimumLineSpacing = SizeConstants.Screen.Width * 0.055f
        //    };
        //    CollectionView.ReloadData();
        //}

        //public override void ViewWillDisappear(bool animated)
        //{
        //    base.ViewWillDisappear(animated);
        //    _challengeCollectionSource.ChallengeItemSelected -= OnChallengeItemSelected;
        //}

        //public void LoadChallengeDetail(ChallengeModel challenge)
        //{
        //    // challenge.TypeCode = "FB ENGAGEMENT";
        //    UIStoryboard board = UIStoryboard.FromName("Challenges", null);
        //    ChallengeDetailBaseViewController controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("MultipleChoiceViewController"); ;
        //    if (challenge.TypeCode == "MC" || challenge.TypeCode == "SIGNUP")
        //    {
        //        controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("MultipleChoiceViewController");
        //    }
        //    else if (challenge.TypeCode == "INSTA")
        //    {
        //        controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("InstagramViewController");
        //    }
        //    else if (challenge.TypeCode == "CHECKIN")
        //    {
        //        controller = (CheckInViewController)board.InstantiateViewController("CheckInViewController");
        //    }
        //    else if (challenge.TypeCode == "SHARE" && challenge.TypeCodeDisplayName == "Facebook")
        //    {
        //        controller = (FacebookDetailViewController)board.InstantiateViewController("FacebookDetailViewController");
        //    }
        //    //else if (challenge.TypeCode == "FB ENGAGEMENT")
        //    //{
        //    //    controller = (FacebookDetailViewController)board.InstantiateViewController("FacebookDetailViewController");
        //    //}
        //    else
        //    {
        //        controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("ChallengeDetailViewController");
        //    }
        //    controller.Challenge = challenge;
        //    NavigationController.PushViewController(controller, true);
        //}

        //private async Task RefreshChallenges()
        //{
        //    Crashlytics.Instance.Log("Challenges.ChallengesViewController.RefreshChallenges()");
        //    TableView.ShowLoader();
        //    await SL.Manager.GetChallengesAsync();
        //    TableView.ReloadData();
        //    CollectionView.ReloadData();
        //    TableView.HideLoader();
        //}

        //private async Task RefreshProfile()
        //{
        //    Crashlytics.Instance.Log("Challenges.ChallengesViewController.RefreshProfile()");
        //    await SL.Manager.GetProfileAsync();
        //    AreaCollection.ReloadData();
        //    UpdateNavBar();
        //}

        //public async override Task Refresh()
        //{
        //    Crashlytics.Instance.Log("Challenges.ChallengesViewController.Refresh()");
        //    SL.IsBusy = true;
        //    if (Platform.IsInternetConnectionAvailable() == false)
        //    {
        //        SL.IsBusy = false;
        //        return;
        //    }
        //    await RefreshChallenges();
        //    await RefreshProfile();
        //    await base.Refresh();

        //    var itemsCount = SL.ChallengeList.Count;

        //    if (itemsCount == 0)
        //    {
        //        EmptyCollectionView.Hidden = false;
        //        ViewForImage.StartAnimating();
        //        SL.IsBusy = false;
        //        return;
        //    }
        //    EmptyCollectionView.Hidden = true;
        //    ViewForImage.StopAnimating();
        //    SL.IsBusy = false;
        //}

        //public async void RefreshAsync()
        //{
        //    await Refresh();
        //}

        //public void DidComplete(ISharing sharer, NSDictionary results)
        //{
        //    Debug.WriteLine(@"message posted");
        //}

        //public void DidFail(ISharing sharer, NSError error)
        //{
        //    Debug.WriteLine(@"message failed: " + error.DebugDescription);
        //}

        //public void DidCancel(ISharing sharer)
        //{
        //    Debug.WriteLine(@"message canceled");
        //}

        ////private void ShowLoader()
        ////{
        ////    if (_isContentLoaded)
        ////    {
        ////        TableView.ShowLoader();
        ////        return;
        ////    }
        ////    _overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
        ////}

        ////private void HideLoader()
        ////{
        ////    if (_isContentLoaded)
        ////    {
        ////        TableView.HideLoader();
        ////        return;
        ////    }
        ////    _overlay.RemoveFromSuperview();
        ////}

        //public void OnChallengeItemSelected()
        //{
        //    if (!AreaCollectionHidden)
        //    {
        //        HideAreaCollection();
        //    }
        //}
    }
}
