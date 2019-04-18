using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Authentication;
using SocialLadder.Enums;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace SocialLadder.ViewModels.Intro
{
    public class NetworksViewModel : BaseViewModel, INetworkViewModel, IFacebookAuthenticationDelegate, ITwitterAuthenticationDelegate, IInstagramAuthenticationDelegate
    {
        ILocationService _locatioService;
        #region Properties
        private MvxInteraction<LocalNetworkAction> _actionInteraction;
        public IMvxInteraction<LocalNetworkAction> ActionInteraction
        {
            get
            {
                if (_actionInteraction == null)
                {
                    _actionInteraction = new MvxInteraction<LocalNetworkAction>();
                }
                return _actionInteraction;
            }
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private int _socialCount = 0;
        public int SocialCount
        {
            get { return _socialCount; }
            set
            {
                if (SocialCount < 3)
                {
                    _socialCount = value;
                }
                if (SocialCount > 3)
                {
                    _socialCount = 3;
                }
            }
        }

        private string _fBImage;
        public string FBImage
        {
            get => _fBImage;
            set => SetProperty(ref _fBImage, value);
        }

        private string _twitterImage;
        public string TwitterImage
        {
            get => _twitterImage;
            set => SetProperty(ref _twitterImage, value);
        }

        private string _instaImage;
        public string InstaImage
        {
            get => _instaImage;
            set => SetProperty(ref _instaImage, value);
        }

        private string _fBLoaderImage;
        public string FBLoaderImage
        {
            get => _fBLoaderImage;
            set => SetProperty(ref _fBLoaderImage, value);
        }

        private string _twitterLoaderImage;
        public string TwitterLoaderImage
        {
            get => _twitterLoaderImage;
            set => SetProperty(ref _twitterLoaderImage, value);
        }

        private string _instaLoaderImage;
        public string InstaLoaderImage
        {
            get => _instaLoaderImage;
            set => SetProperty(ref _instaLoaderImage, value);
        }

        private bool _privacyPolicyViewHidden;
        public bool PrivacyPolicyViewHidden
        {
            get => _privacyPolicyViewHidden;
            set => SetProperty(ref _privacyPolicyViewHidden, value);
        }

        private bool _backgroundHidden;
        public bool BackgroundHidden
        {
            get => _backgroundHidden;
            set => SetProperty(ref _backgroundHidden, value);
        }

        private string _webViewLink;
        public string WebViewLink
        {
            get => _webViewLink;
            set => SetProperty(ref _webViewLink, value);
        }
        private bool _logoutButtonHidden;
        public bool LogoutButtonHidden
        {
            get => _logoutButtonHidden;
            set => SetProperty(ref _logoutButtonHidden, value);
        }
        #endregion

        #region Constructors
        public NetworksViewModel(IMvxNavigationService navigationService, ILocationService loationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messenger) : base(navigationService, alertService, assetService, localNotificationService, messenger)
        {
            _locatioService = loationService;
            FBImage = _assetService.FBUnconnectedIcon;
            InstaImage = _assetService.InstaUnconnectedIcon;
            TwitterImage = _assetService.TwitterUnconnectedIcon;

            //PrivacyPolicyViewHidden = true;
            WebViewLink = Constants.WebViewLink;
            BackgroundHidden = NavigationHelper.ShowNetworksPageFromMainVM ? false : true;
            PrivacyPolicyViewHidden = !BackgroundHidden;
            LogoutButtonHidden = true;
            SetScoreImage(true);
        }
        #endregion

        #region Lifecycle
        public override void Start()
        {
            base.Start();
            ScoreImage = _assetService.NetworksScoreImageBold;
        }

        public async override void ViewAppeared()
        {
            base.ViewAppeared();
            if (NavigationHelper.ShowNetworksPageFromMainVM && NavigationHelper.ShowAreasCollectionPageAfterLogout && NavigationHelper.AreasCollectionPageAfterLogoutDoneClicked)
            {
                await Task.Run(async () =>
                {
                    NavigationHelper.ShowAreasCollectionPageAfterLogout = NavigationHelper.AreasCollectionPageAfterLogoutDoneClicked = false;
                    await _navigationService.Close(this);
                });
            }
        }
        #endregion

        #region Methods
        private void SetScoreImage(bool loginCanceled = false)
        {
            int count = 0;
            if (SL.IsNetworkConnected("Facebook"))
            {
                FBImage = _assetService.FBConnectedIcon;
                FBLoaderImage = _assetService.NetworkConnectedLoaderImage;
                count += 1;
            }
            if (SL.IsNetworkConnected("Twitter"))
            {
                TwitterImage = _assetService.TwitterConnectedIcon;
                TwitterLoaderImage = _assetService.NetworkConnectedLoaderImage;
                count += 1;
            }
            if (SL.IsNetworkConnected("Instagram"))
            {
                InstaImage = _assetService.InstaConnectedIcon;
                InstaLoaderImage = _assetService.NetworkConnectedLoaderImage;
                count += 1;
            }
            if (count == 0)
            {
                LogoutButtonHidden = true;
                FBImage = _assetService.FBUnconnectedIcon;
                TwitterImage = _assetService.TwitterUnconnectedIcon;
                InstaImage = _assetService.InstaUnconnectedIcon;
                ScoreImage = _assetService.NetworksScoreImageBold;
                FBLoaderImage = TwitterLoaderImage = InstaLoaderImage = string.Empty;
            }
            if (count == 1)
            {
                ScoreImage = _assetService.NetworksScore33Per_Image;
            }
            if (count == 2)
            {
                ScoreImage = _assetService.NetworksScore66Per_Image;
            }
            if (count == 3)
            {
                ScoreImage = _assetService.NetworksScore100Per_Image;
            }
            if (count > 0)
            {
                LogoutButtonHidden = false;
            }
        }

        //private void SetScoreImage(bool loginCanceled = false)
        //{
        //    if (!loginCanceled)
        //    {
        //        SocialCount += 1;
        //    }
        //    if (SocialCount == 0)
        //    {
        //        ScoreImage = _assetService.NetworksScoreImageBold;
        //        return;
        //    }
        //    if (SocialCount == 1)
        //    {
        //        ScoreImage = _assetService.NetworksScore33Per_Image;
        //    }
        //    if (SocialCount == 2)
        //    {
        //        ScoreImage = _assetService.NetworksScore66Per_Image;
        //    }
        //    if (SocialCount == 3)
        //    {
        //        ScoreImage = _assetService.NetworksScore100Per_Image;
        //    }
        //}

        private async Task CheckInNetwork(SocialNetworkModel network)
        {
            IsBusy = true;
            var location = await _locatioService.GetCurrentLocation();
            if (!location.IsAvailable)
            {
                _alertService.ShowToast("Location service not available");
            }
            var response = await SL.CheckInNetwork(network, location.Lat, location.Long);
            if (response.ResponseCode > 0)
            {
                if (SL.HasAreas)
                    SL.RefreshAll();
            }
            else
            {
                _alertService.ShowOkCancelMessage(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", null, null);
            }
            IsBusy = false;
            SetScoreImage();
        }

        public string GetScoreImageLoadName()
        {
            return _assetService.NetworkConnectingLoaderImage;
        }
        #endregion

        #region FBDelegate
        public async Task OnFacebookAuthenticationCompleted(SocialNetworkModel network)
        {
            IsBusy = false;
            FBLoaderImage = _assetService.NetworkConnectedLoaderImage;
            FBImage = _assetService.FBConnectedIcon;
            //SetScoreImage();
            Mvx.Resolve<IFacebookShareService>().SetAccessToken(network.AccessToken, network.UserID);
            await CheckInNetwork(network);
        }

        public void OnFacebookAuthenticationFailed(string message, Exception exception)
        {
            IsBusy = false;
            FBLoaderImage = string.Empty;
            SetScoreImage(true);
        }

        public void OnFacebookAuthenticationCanceled()
        {
            IsBusy = false;
            FBLoaderImage = string.Empty;
            SetScoreImage(true);
        }
        #endregion

        #region TwitterDelegate
        public async Task OnTwitterAuthenticationCompleted(SocialNetworkModel network)
        {
            IsBusy = false;
            TwitterLoaderImage = _assetService.NetworkConnectedLoaderImage;
            TwitterImage = _assetService.TwitterConnectedIcon;
            //SetScoreImage();
            await CheckInNetwork(network);
        }

        public void OnTwitterAuthenticationFailed(string message, Exception exception)
        {
            IsBusy = false;
            TwitterLoaderImage = string.Empty;
            SetScoreImage(true);
        }

        public void OnTwitterAuthenticationCanceled()
        {
            IsBusy = false;
            TwitterLoaderImage = string.Empty;
            SetScoreImage(true);
        }
        #endregion

        #region InstaDelegate
        public void OnInstagramAuthenticationFailed(string message, Exception exception)
        {
            IsBusy = false;
            InstaLoaderImage = string.Empty;
            SetScoreImage(true);
        }

        public void OnInstagramAuthenticationCanceled()
        {
            IsBusy = false;
            InstaLoaderImage = string.Empty;
            SetScoreImage(true);
        }

        public async Task OnInstagramAuthenticationCompleted(SocialNetworkModel network)
        {
            IsBusy = false;
            InstaImage = _assetService.InstaConnectedIcon;
            InstaLoaderImage = _assetService.NetworkConnectedLoaderImage;
            //SetScoreImage();
            await CheckInNetwork(network);
        }
        #endregion

        #region Commands
        public MvxCommand FbConnectCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsBusy = true;
                    FBLoaderImage = _assetService.NetworkConnectingLoaderImage;
                    ScoreImage = _assetService.NetworkConnectingLoaderImage;
                });
            }
        }

        public MvxCommand TwitterConnectCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsBusy = true;
                    TwitterLoaderImage = _assetService.NetworkConnectingLoaderImage;
                    ScoreImage = _assetService.NetworkConnectingLoaderImage;
                });
            }
        }

        public MvxAsyncCommand InstaConnectCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await Task.Run(async () =>
                    {
                        IsBusy = true;
                        InstaLoaderImage = _assetService.NetworkConnectingLoaderImage;
                        ScoreImage = _assetService.NetworkConnectingLoaderImage;
                    });
                });
            }
        }

        public MvxAsyncCommand LogoutCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    ScoreImage = GetScoreImageLoadName();
                    await LogoutHelper.Logout();
                    SetScoreImage();
                });
            }
        }

        public MvxCommand LoadPrivacyPolicy
        {
            get
            {
                return new MvxCommand(() =>
                {
                    WebViewLink = "https://socialladder.rkiapps.com/privacy-policy-mobileonly.html";
                    PrivacyPolicyViewHidden = false;
                    //ButtonOnPrivacyTouchInside(this, EventArgs.Empty);
                    _actionInteraction.Raise(LocalNetworkAction.OnPrivacyAction);
                });
            }
        }

        public MvxAsyncCommand LoadPrivacyPolicyWebView
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await InitWebView("http://socialladder.rkiapps.com/PrivacyPolicy.aspx?deviceUUID=");
                });
            }
        }

        public MvxCommand LoadTermsService
        {
            get
            {
                return new MvxCommand(() =>
                {


                    WebViewLink = "https://socialladder.rkiapps.com/terms-and-conditions-mobileonly.html";
                    PrivacyPolicyViewHidden = false;
                    //if (!NavigationHelper.ShowNetworksPageFromMainVM)
                    //{
                    //    //ButtonOnPrivacyTouchInside(this, EventArgs.Empty);
                    //    _actionInteraction.Raise(LocalNetworkAction.OnPrivacyAction);
                    //}
                    _actionInteraction.Raise(LocalNetworkAction.OnPrivacyAction);
                });
            }
        }

        public MvxAsyncCommand LoadTermsServiceWebView
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await InitWebView("http://socialladder.rkiapps.com/TermOfService.aspx?deviceUUID=");
                });
            }
        }

        private async Task InitWebView(string url)
        {
            await _navigationService.Navigate<WebViewModel>();
            if (!string.IsNullOrEmpty(SL.AreaGUID))
            {
                var _url = url + SL.DeviceUUID + "&AreaGUID=" + SL.AreaGUID;
                _messenger.Publish(new MessangerWebModel(this, _url, false));
            }
            else
            {
                var _url = url + SL.DeviceUUID;
                _messenger.Publish(new MessangerWebModel(this, _url, false));
            }
            _actionInteraction.Raise(LocalNetworkAction.OnPrivacyAction);
        }

        public MvxAsyncCommand DoneCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    if (!NavigationHelper.ShowNetworksPageFromMainVM)
                    {
                        return;
                    }
                    if (!SL.IsNetworkConnected("Facebook") && !SL.IsNetworkConnected("Twitter") && !SL.IsNetworkConnected("Instagram"))
                    {
                        _alertService.ShowToast("Please, connect social network");
                        return;
                    }
                    var platformNavigationService = Mvx.Resolve<IPlatformNavigationService>();
                    if (platformNavigationService != null)
                    {
                        platformNavigationService.NavigateToTab(ENavigationTabs.FEED, true);
                    }
                    var areas = SL.GetAreas();
                    //var suggestedAreas = SL.GetSuggestedAreas();
                    //if ((areas == null || areas.Count == 0) && (suggestedAreas == null || suggestedAreas.Count == 0))
                    if (areas == null || areas.Count == 0)
                    {
                        NavigationHelper.ShowAreasCollectionPageFromMainVM = true;
                        await _navigationService.Navigate<AreasCollectionViewModel>();
                    }
                    else
                    {
                        await _navigationService.Close(this);
                    }
                    //NavigationHelper.ShowAreasCollectionPageFromMainVM = NavigationHelper.ShowAreasCollectionPageAfterLogout = true;
                    //await _navigationService.Navigate<AreasCollectionViewModel>();
                });
            }
        }

        public MvxCommand ClosePrivacyPolicyViewCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    PrivacyPolicyViewHidden = false;
                    if (!NavigationHelper.ShowNetworksPageFromMainVM)
                    {
                        //CloseButtonTouchInside(this, EventArgs.Empty);
                        _actionInteraction.Raise(LocalNetworkAction.CloseAction);
                    }
                });
            }
        }

        public MvxCommand AgreePrivacyPolicyViewCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    PrivacyPolicyViewHidden = true;
                });
            }
        }
        #endregion
    }
}
