using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Authentication;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.More;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Intro;
using SocialLadder.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.More
{
    public class MoreViewModel : BaseViewModel, IFacebookAuthenticationDelegate, ITwitterAuthenticationDelegate, IInstagramAuthenticationDelegate, IMainTabViewModel
    {
        private ILocationService _locatioService;
        private IEncodingService _encodingService;

        #region Properties

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                SetProperty(ref _userName, value);
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                SetProperty(ref _city, value);
            }
        }

        private string _friendsCount;
        public string FriendsCount
        {
            get => _friendsCount;
            set
            {
                SetProperty(ref _friendsCount, value);
            }
        }

        private string _countConnected;
        public string CountConnected
        {
            get => _countConnected;
            set
            {
                SetProperty(ref _countConnected, value);
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

        private int _countOfNetworks;
        public int CountOfNetworks
        {
            get => _countOfNetworks;
            set
            {
                SetProperty(ref _countOfNetworks, value);
            }
        }
        #endregion
        private MvxObservableCollection<MoreItemModel> _moreItems;

        public MvxObservableCollection<MoreItemModel> MoreItems
        {
            get
            {
                return _moreItems;
            }
            set
            {
                SetProperty(ref _moreItems, value);
            }
        }

        public MoreViewModel(IMvxNavigationService navigationService, ILocationService loationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IEncodingService encodingService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _locatioService = loationService;
            _encodingService = encodingService;
            _onItemClickCommand = new MvxAsyncCommand<MoreItemModel>(DrillDown);

            FBImage = _assetService.FBUnconnectedIcon;
            InstaImage = _assetService.InstaUnconnectedIcon;
            TwitterImage = _assetService.TwitterUnconnectedIcon;

            LoadProfile();
            LoadMoreItems();
            ScoreImage = "IconScoreTransactions";
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            if (SL.Profile != null && string.IsNullOrEmpty(SL.Profile.UserName))
            {
                UserName = GetUserNameWithEmoji(SL.Profile.UserName);
            }
        }

        #region Commands
        private IMvxAsyncCommand<MoreItemModel> _onItemClickCommand;
        public IMvxAsyncCommand<MoreItemModel> OnItemClickCommand
        {
            get
            {
                return _onItemClickCommand;
            }
        }

        public MvxCommand FbConnectCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    IsBusy = true;
                    FBLoaderImage = _assetService.NetworkConnectingLoaderImage;
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
                    });
                });
            }
        }
        #endregion
        #region Methods
        private async Task DrillDown(MoreItemModel itemModel)
        {
            switch(itemModel.Name)
            {
                case "FAQ":
                    {
                        await _navigationService.Navigate<WebViewModel>();
                        _messenger.Publish<MessangerWebModel>(new MessangerWebModel(this, Constants.ServerUrl + "/SL/helpdesk/askaquestion?deviceuuid=" + SL.DeviceUUID + "&areaguid=" + SL.AreaGUID, true));
                        break;
                    }
                case "Settings":
                    {
                        await _navigationService.Navigate<SettingsViewModel>();
                        break;
                    }
                case "Privacy Policy":
                    {
                        await _navigationService.Navigate<WebViewModel>();
                        _messenger.Publish<MessangerWebModel>(new MessangerWebModel(this, string.Format("https://socialladder.rkiapps.com/privacy-policy-mobileonly.html", SL.DeviceUUID, SL.AreaGUID), true));
                        break;
                    }
                case "Terms of Service":
                    {
                        await _navigationService.Navigate<WebViewModel>();
                        _messenger.Publish<MessangerWebModel>(new MessangerWebModel(this, string.Format("https://socialladder.rkiapps.com/terms-and-conditions-mobileonly.html", SL.DeviceUUID, SL.AreaGUID), true));
                        break;
                    }
                case "Logout":
                    {
                        await LogoutInvoke();
                        break;
                    }
            }
        }

        public MoreSocialConnectedNetworksModel CheckConnectedNetworks()
        {
            var connectedModel = new MoreSocialConnectedNetworksModel(false,false,false);
            if (SL.IsNetworkConnected("Facebook"))
            {
                connectedModel.FacebookConnected = true;
            }
            if (SL.IsNetworkConnected("Twitter"))
            {
                connectedModel.TwitterkConnected = true;
            }
            if (SL.IsNetworkConnected("Instagram"))
            {
                connectedModel.InstagramConnected = true;
            }
            return connectedModel;
        }

        private async Task LogoutInvoke()
        {
            var result = await _alertService.ShowAlertConfirmation("Are you sure you want to log out?");
            if (result)
            {
                NavigationHelper.ShowNetworksPageFromMainVM = true;
                await LogoutHelper.Logout();
                await _navigationService.Navigate<NetworksViewModel>();
            }
        }
        

        private async void LoadProfile()
        {
            try {
                await SL.Manager.GetProfileAsync(UpdateUserData);
            } catch (Exception ex) {
                Debug.WriteLine("Can't get profile! " + ex.Message);
            }          
        }

        private void LoadMoreItems()
        {
            MoreItems = new MvxObservableCollection<MoreItemModel>();

            List<string> names = new List<string>
            {
                "FAQ", "Settings", "Privacy Policy", "Terms of Service", "Logout"
            };
            List<string> images = new List<string>
            {
                "account_question_icon", "account_settings_icon", "account_docs_icon", "account_docs_icon", "account_logout_icon"
            };
            for (int count = 0; count < 5; count++)
            {
                var model = new MoreItemModel()
                {
                    Name = names[count],
                    MoreImage = images[count]
                };
                RaisePropertyChanged("MoreImage");
                MoreItems.Add(model);
            }
            RaisePropertyChanged("MoreItems");
        }

        private void UpdateUserData(ProfileResponseModel response)
        {
            UpdateProfileData();
        }

        public async override Task FinishRefresh()
        {
            await base.FinishRefresh();
            UpdateProfileData();
            LoadMoreItems();
        }

        private void UpdateProfileData()
        {
            var profile = SL.Profile;
            if(profile != null)
            {
                UserName = /*GetUserNameWithEmoji*/(profile.UserName);
                City = profile.City;
                ScoreCount = ((int)profile.Score).ToString();
                FriendsCount = (SL.FriendList != null) ? (SL.FriendList.Count.ToString() + " Friends") : "0 Friends";
                CountOfNetworks = profile.NetworkList.Count;
                CountConnected = $"{CountOfNetworks} of 3 Networks Connected";
                for (int network = 0; network < profile.NetworkList.Count; network++)
                {
                    if (profile.NetworkList[network].NetworkName == "Facebook")
                    {
                        FBImage = _assetService.FBConnectedIcon;
                    }
                    if (profile.NetworkList[network].NetworkName == "Twitter")
                    {
                        TwitterImage = _assetService.TwitterConnectedIcon;
                    }
                    if (profile.NetworkList[network].NetworkName == "Instagram")
                    {
                        InstaImage = _assetService.InstaConnectedIcon;
                    }
                }

                Image = profile.ProfilePictureURL;
                RaisePropertyChanged("Image");
            }
        }

        private string GetUserNameWithEmoji(string userNameInBytes)
        {
            if (string.IsNullOrEmpty(userNameInBytes) || string.IsNullOrWhiteSpace(userNameInBytes))
                return userNameInBytes;

            try
            {
                var decodingUserName = _encodingService.DecodeFromNonLossyAscii(userNameInBytes);
                return decodingUserName;
            }
            catch (FormatException)
            {
                return userNameInBytes;
            }
        }

        private async Task CheckInNetwork(SocialNetworkModel network)
        {
            IsBusy = true;
            var location = await _locatioService.GetCurrentLocation();
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
        }
        #endregion
        #region FBDelegate
        public async Task OnFacebookAuthenticationCompleted(SocialNetworkModel network)
        {
            IsBusy = false;
            FBLoaderImage = _assetService.NetworkConnectedLoaderImage;
            FBImage = _assetService.FBConnectedIcon;
            await CheckInNetwork(network);
        }

        public void OnFacebookAuthenticationFailed(string message, Exception exception)
        {
            IsBusy = false;
            FBLoaderImage = string.Empty;
        }

        public void OnFacebookAuthenticationCanceled()
        {
            IsBusy = false;
            FBLoaderImage = string.Empty;
        }
        #endregion
        #region TwitterDelegate
        public async Task OnTwitterAuthenticationCompleted(SocialNetworkModel network)
        {
            IsBusy = false;
            TwitterLoaderImage = _assetService.NetworkConnectedLoaderImage;
            TwitterImage = _assetService.TwitterConnectedIcon;
            await CheckInNetwork(network);
        }

        public void OnTwitterAuthenticationFailed(string message, Exception exception)
        {
            IsBusy = false;
            TwitterLoaderImage = string.Empty;
        }

        public void OnTwitterAuthenticationCanceled()
        {
            IsBusy = false;
            TwitterLoaderImage = string.Empty;
        }
        #endregion
        #region InstaDelegate
        public void OnInstagramAuthenticationFailed(string message, Exception exception)
        {
            IsBusy = false;
            InstaLoaderImage = string.Empty;
        }

        public void OnInstagramAuthenticationCanceled()
        {
            IsBusy = false;
            InstaLoaderImage = string.Empty;
        }

        public async Task OnInstagramAuthenticationCompleted(SocialNetworkModel network)
        {
            IsBusy = false;
            InstaImage = _assetService.InstaConnectedIcon;
            InstaLoaderImage = _assetService.NetworkConnectedLoaderImage;
            await CheckInNetwork(network);
        }
        #endregion
    }

    public class MoreItemModel
    {
        public string Name { get; set; }

        public string MoreImage { get; set; }
    }
}
