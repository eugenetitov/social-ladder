using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using FFImageLoading;
using Foundation;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;
using SocialLadder.Authentication;
using SocialLadder.iOS.Helpers;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.ViewControllers;
using SocialLadder.iOS.ViewControllers.Intro;
using SocialLadder.iOS.ViewControllers.Main;
using SocialLadder.Models;
using SocialLadder.ViewModels.More;
using SocialLadder.ViewModels.Points;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.More
{
    [MvxFromStoryboard("More")]
    //[MvxTabPresentation(WrapInNavigationController = true, TabName = "More", TabIconName = "feed-icon_on", TabSelectedIconName = "feed-icon_off")]
    [MvxRootPresentation]
    public partial class MoreViewController :  BaseTabViewController<MoreViewModel> //SLViewController, IFacebookAuthenticationDelegate, ITwitterAuthenticationDelegate, IInstagramAuthenticationDelegate
    {
        #region Fields
        private CoreGraphics.CGRect MainFrame = UIScreen.MainScreen.Bounds;
        #endregion

        #region ctors
        public MoreViewController(IntPtr handle) : base(handle)
        {
            var ViewModel = this.ViewModel;
        }
        #endregion

        //#region Lifecycle
        //public override void DidReceiveMemoryWarning()
        //{
        //    base.DidReceiveMemoryWarning();
        //    // Release any cached data, images, etc that aren't in use.
        //}

        //public override void ViewDidLoad()
        //{
        //    base.AreaCollectionOutlet = AreaCollection;
        //    base.AreaCollectionHeightOutlet = AreaCollectionHeight;
        //    base.AreaCollectionTopOutlet = cnsAreaCollectionTop;

        //    base.ViewDidLoad();

        //    TableView.RegisterNibForCellReuse(MoreTableViewCell.Nib, MoreTableViewCell.ClassName);

        //    RefreshAsync();

        //    BkgdImage.Image = UIImage.FromBundle("more-bg");

        //    BtnPendingRequest.Layer.BorderColor = BtnPendingRequest.TitleLabel.TextColor.CGColor;
        //    BtnPendingRequest.Layer.BorderWidth = 1f;

        //    BtnPendingRequest.TouchUpInside += (e, ev) =>
        //    {
        //        UIStoryboard board = UIStoryboard.FromName("More", null);
        //        UIViewController controller = board.InstantiateViewController("FriendRequestsViewController");
        //        NavigationController.PushViewController(controller, true);
        //    };

        //    var labelString = new NSMutableAttributedString(InfoLabel.Text);
        //    var paragraphStyle = new NSMutableParagraphStyle { LineSpacing = 2f };
        //    var style = UIStringAttributeKey.ParagraphStyle;
        //    var range = new NSRange(0, labelString.Length);
        //    labelString.AddAttribute(style, paragraphStyle, range);
        //    InfoLabel.AttributedText = labelString;

        //    scrlMainContent.Scrolled += (s, e) =>
        //    {
        //        if (!AreaCollectionHidden)
        //        {
        //            HideAreaCollection();
        //        }
        //    };

        //    UpdateView();

        //    Platform.ClearBrowserCache();
        //}

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);

        //    RefreshAsync();

        //    ImageService.Instance.LoadUrl(SL.Profile.ProfilePictureURL).Into(ProfileImage);
        //    TableView.Source = new MoreTableSource(this);

        //    NameLabel.Text = StringWithEmojiConverter.ConvertEmojiFromServer(SL.Profile.UserName);
        //    CityLabel.Text = SL.Profile.City;
        //}

        //public override void ViewDidLayoutSubviews()
        //{
        //    base.ViewDidLayoutSubviews();
        //    nfloat fontMultiplier = UIScreen.MainScreen.Bounds.Width / 414;

        //    NameLabel.Font = UIFont.FromName("ProximaNova-Bold", 16.5f * fontMultiplier);
        //    PositionLabel.Font = UIFont.FromName("ProximaNova-Bold", 14 * fontMultiplier);
        //    CityLabel.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
        //    InfoLabel.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
        //    BtnPendingRequest.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
        //    FriendsCountText.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
        //    ConnectedNetworkText.Font = UIFont.FromName("ProximaNova-Regular", 15.5f * fontMultiplier);
        //    AddPointsText.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
        //    FacebookLabel.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
        //    TwitterLabel.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
        //    InstagramLabel.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);

        //    InstaLoginButton.Layer.CornerRadius = InstaLoginButton.Frame.Width / 2f;
        //    TwitterLoginButton.Layer.CornerRadius = InstaLoginButton.Frame.Width / 2f;
        //    ProfileImage.Layer.CornerRadius = ProfileImage.Frame.Width / 2;
        //    ScoreImage.Layer.CornerRadius = ScoreImage.Frame.Width / 2;

        //    TableViewHeight.Constant = TableView.ContentSize.Height;

        //    scrlMainContent.ContentSize = new CGSize(scrlMainContent.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);

        //    InvokeOnMainThread(() =>
        //    {
        //        View.LayoutIfNeeded();
        //    });
        //    TableViewHeight.Constant = TableView.ContentSize.Height;
        //    scrlMainContent.ContentSize = new CGSize(scrlMainContent.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);
        //}
        //#endregion

        //#region Refresh methods
        //public async override Task Refresh()
        //{
        //    await RefreshProfile();
        //    await base.Refresh();
        //}

        //private async void RefreshAsync()
        //{
        //    await Refresh();
        //}

        //void UpdateUserData(ProfileResponseModel response)
        //{
        //    UpdateProfileData();
        //    UpdateNetworksConnection();
        //}

        //async Task RefreshProfile()
        //{
        //    await SL.Manager.GetProfileAsync(UpdateUserData);
        //    AreaCollection.ReloadData();
        //    UpdateNavBar();
        //}

        //async void RefreshChallenges()
        //{
        //    await SL.Manager.GetChallengesAsync();
        //}

        //async void RefreshRewards()
        //{
        //    await SL.Manager.GetRewardsAsync();
        //}

        //async void RefreshFeed()
        //{
        //    await SL.Manager.GetFeedAsync();
        //}
        //#endregion

        //#region EventHandlers
        //partial void FacebookLoginButton_TouchUpInside(UIButton sender)
        //{
        //    //animate indicator
        //    FacebookConnectedImage.Image = UIImage.FromBundle("loading-indicator");
        //    Platform.AnimateRotation(FacebookConnectedImage);
        //    //push fb login
        //    var auth = new FacebookAuthenticator(Configuration.ClientId, Configuration.Scope, this);
        //    var authenticator = auth.GetAuthenticator();
        //    var viewController = authenticator.GetUI();
        //    PresentViewController(viewController, true, null);
        //}

        //partial void TwitterLoginButton_TouchUpInside(UIButton sender)
        //{
        //    TwitterConnectedImage.Image = UIImage.FromBundle("loading-indicator");
        //    Platform.AnimateRotation(TwitterConnectedImage);

        //    var auth = new TwitterAuthentificator("QkS2cTMjtUD1F8N7NvA9w", "NVaB0TL4CmyWlFl0mXoTv9S5LncEdQx2uh2wkP7DsOo", Configuration.Scope, this);
        //    var authenticator = auth.GetAuthenticator();
        //    var viewController = authenticator.GetUI();
        //    PresentViewController(viewController, true, null);
        //}

        //partial void InstaLoginButton_TouchUpInside(UIButton sender)
        //{
        //    InstagramConnectedImage.Image = UIImage.FromBundle("loading-indicator");
        //    Platform.AnimateRotation(InstagramConnectedImage);

        //    UIStoryboard board = UIStoryboard.FromName("Web", null);
        //    WebViewController ctrl = (WebViewController)board.InstantiateViewController("WebViewController");
        //    const string ClientID = "cf88ac6682e24ffe83441b6950e3134a";
        //    const string RedirectUrl = "http://socialladderapp.com";
        //    ctrl.Url = "https://api.instagram.com/oauth/authorize/?client_id=" + ClientID + "&redirect_uri=" + RedirectUrl + "&response_type=token";
        //    ctrl.InstagramDelegate = this;
        //    this.PresentViewController(ctrl, false, null);
        //}
        //#endregion

        //#region Events
        //async public void OnFacebookAuthenticationCompleted(SocialNetworkModel network)
        //{
        //    DismissViewController(true, null);

        //    Platform.ClearBrowserCache();
        //    //Update SL server
        //    var response = await SL.CheckInNetwork(network, Platform.Lat, Platform.Lon);

        //    Platform.AnimateRotationComplete(FacebookConnectedImage);

        //    UpdateView();

        //    if (response.ResponseCode > 0)
        //    {
        //        SL.RefreshProfile();
        //    }
        //    else
        //    {
        //        await Platform.ShowAlert(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", "OK");
        //    }

        //    UpdateNetworksConnection();
        //}

        //public void OnFacebookAuthenticationFailed(string message, Exception exception)
        //{
        //    Platform.AnimateRotationComplete(FacebookConnectedImage);
        //    UpdateView();
        //}

        //public void OnFacebookAuthenticationCanceled()
        //{
        //    Platform.AnimateRotationComplete(FacebookConnectedImage);
        //    UpdateView();
        //}

        //public async void OnTwitterAuthenticationCompleted(SocialNetworkModel network)
        //{
        //    DismissViewController(true, null);

        //    Platform.ClearBrowserCache();
        //    //Update SL server
        //    var response = await SL.CheckInNetwork(network, Platform.Lat, Platform.Lon);

        //    Platform.AnimateRotationComplete(TwitterConnectedImage);

        //    UpdateView();

        //    if (response.ResponseCode > 0)
        //    {
        //        SL.RefreshProfile();
        //    }
        //    else
        //    {
        //        await Platform.ShowAlert(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", "OK");
        //    }

        //    UpdateNetworksConnection();
        //}

        //public void OnTwitterAuthenticationFailed(string message, Exception exception)
        //{
        //    Platform.AnimateRotationComplete(TwitterConnectedImage);
        //    UpdateView();
        //}

        //public void OnTwitterAuthenticationCanceled()
        //{
        //    Platform.AnimateRotationComplete(TwitterConnectedImage);
        //    UpdateView();
        //}

        //public async void OnInstagramAuthenticationCompleted(SocialNetworkModel network)
        //{
        //    DismissViewController(true, null);
        //    Platform.ClearBrowserCache();
        //    var response = await SL.CheckInNetwork(network, Platform.Lat, Platform.Lon);

        //    Platform.AnimateRotationComplete(InstagramConnectedImage);

        //    UpdateView();

        //    if (response.ResponseCode > 0)
        //    {
        //        SL.RefreshProfile();
        //    }
        //    else
        //    {
        //        await Platform.ShowAlert(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", "OK");
        //    }

        //    UpdateNetworksConnection();
        //}

        //public void OnInstagramAuthenticationFailed(string message, Exception exception)
        //{
        //    DismissViewController(true, null);
        //    Platform.AnimateRotationComplete(InstagramConnectedImage);
        //    InstagramConnectedImage.Image = null;
        //}

        //public void OnInstagramAuthenticationCanceled()
        //{
        //    DismissViewController(true, null);
        //    Platform.AnimateRotationComplete(InstagramConnectedImage);
        //    InstagramConnectedImage.Image = null;
        //}


        //#endregion

        //#region Methods
        //void UpdateProfileData()
        //{
        //    var profile = SL.Profile;
        //    if (profile != null)
        //    {
        //        NameLabel.Text = StringWithEmojiConverter.ConvertEmojiFromServer(profile.UserName);
        //        CityLabel.Text = profile.City;
        //        Score.Text = ((int)profile.Score).ToString();

        //        FriendsCountText.Text = (SL.FriendList != null) ?
        //            (SL.FriendList.Count.ToString() + " Friends") : "0 Friends";
        //    }
        //}

        //void UpdateNetworksConnection()
        //{
        //    var profile = SL.Profile;
        //    var networkList = profile.NetworkList;

        //    if (profile.NetworkList != null)
        //    {
        //        if (networkList.Exists(x => x.NetworkName == "Facebook"))
        //        {
        //            FacebookLabel.TextColor = UIColor.Black;
        //            FacebookLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_fb-connected"), UIControlState.Normal);
        //        }
        //        if (!networkList.Exists(x => x.NetworkName == "Facebook"))
        //        {
        //            FacebookLabel.TextColor = UIColor.Gray;
        //            FacebookLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_fb-unconnected"), UIControlState.Normal);
        //        }
        //        if (networkList.Exists(x => x.NetworkName == "Twitter"))
        //        {
        //            TwitterLabel.TextColor = UIColor.Black;
        //            TwitterLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_twitter-connected"), UIControlState.Normal);
        //        }
        //        if (!networkList.Exists(x => x.NetworkName == "Twitter"))
        //        {
        //            TwitterLabel.TextColor = UIColor.Gray;
        //            TwitterLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_twitter-unconnected"), UIControlState.Normal);
        //        }
        //        if (networkList.Exists(x => x.NetworkName == "Instagram"))
        //        {
        //            InstagramLabel.TextColor = UIColor.Black;
        //            InstaLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_insta-connected"), UIControlState.Normal);
        //        }
        //        if (!networkList.Exists(x => x.NetworkName == "Instagram"))
        //        {
        //            InstagramLabel.TextColor = UIColor.Gray;
        //            InstaLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_insta-unconnected"), UIControlState.Normal);
        //        }

        //        ConnectedNetworkText.Text = networkList.Count + " of 3 Networks Connected";
        //        if (networkList.Count == 3)
        //        {
        //            AddPointsText.Hidden = true;
        //            return;
        //        }

        //        AddPointsText.Hidden = false;
        //        return;
        //    }
        //}

        //void UpdateView()
        //{
        //    FacebookConnectedImage.Image = null;
        //    TwitterConnectedImage.Image = null;
        //    InstagramConnectedImage.Image = null;
        //}

        //public void Logout()
        //{
        //    SL.Logout();
        //    SL.Manager.GetCityListWithLatitude(Platform.Lat, Platform.Lon);
        //    Platform.ClearBrowserCache();

        //    var storyboard = UIStoryboard.FromName("Networks", null);
        //    UIViewController viewController = storyboard.InstantiateViewController("Networks");
        //    (viewController as NetworksViewController).UnHideViews();

        //    Platform.AddVideo(viewController.View);
        //    Platform.AddCover(viewController.View);

        //    PresentViewController(viewController, true, null);
        //}
        //#endregion
    }
}

