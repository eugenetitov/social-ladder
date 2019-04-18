using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.Models;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Feed
{
    public class FeedDetailsViewModel : BaseFeedViewModel, IBaseChildViewModel
    {
        #region Properties
        private bool _toolbarHidden;
        public bool ToolbarHidden
        {
            get => _toolbarHidden;
            set => SetProperty(ref _toolbarHidden, value);
        }

        private ProfileModel _userProfile;
        public ProfileModel UserProfile
        {
            get => _userProfile;
            set => SetProperty(ref _userProfile, value);
        }

        public override string ScoreCount
        {
            get
            {
                if (UserProfile != null)
                {
                    return UserProfile.Score.ToString();
                }
                return base.ScoreCount;
            }
            set => base.ScoreCount = value;
        }
        #endregion
        public FeedDetailsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            ToolbarHidden = false;
            FeedItems = new MvxObservableCollection<FeedItemModel>();
            _token = _messenger.Subscribe<MessangerFeedUrlModel>(RefreshFeedByUrl);
        }

        public override async Task Refresh()
        {
            IsRefreshing = IsBusy = true;
            await RefreshOther();
            await SL.Manager.GetProfileAsync();
            await FinishRefresh();
        }

        #region LifeCycle
        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            _messenger.Unsubscribe<MessangerFeedUrlModel>(_token);
        }
        #endregion
        #region Commands
        //public MvxCommand CloseUserProfileViewCommand
        //{
        //    get
        //    {
        //        return new MvxCommand(() =>
        //        {
        //            IsBusy = true;
        //            ToolbarHiden = false;
        //            IsBusy = false;
        //        });
        //    }
        //}
        #endregion
        #region Methods
        private async void RefreshFeedByUrl(MessangerFeedUrlModel model)
        {
            
            ToolbarHidden = model.HasToolbar;
            if (ToolbarHidden && model.FriendId != 0)
            {
                await SL.Manager.GetProfileByFriendIdAsync(model.FriendId, GetProfileComplete);
                await SL.Manager.GetFeedByFriendIdAsync(model.FriendId, GetFeedComplete);
            }
            if (model.FriendId == 0)
            {
                if (!string.IsNullOrEmpty(model.ProfileUrl))
                {
                    await SL.Manager.GetProfileByUrlAsync(model.ProfileUrl, GetProfileComplete);
                }
                await SL.Manager.GetFeedByUrlAsync(model.FeedUrl, GetFeedComplete);
            }
            
        }

        private void GetFeedComplete(FeedResponseModel feedResponse)
        {
            var list = SL.FeedList;
            if (list == null)
            {
                _alertService.ShowToast("An error occurred while loading the page. Try to refresh this page.");
                base.BackCommand.Execute();
                return;
            }
            FeedItems = new MvxObservableCollection<FeedItemModel>(list);
        }

        private void GetProfileComplete(ProfileResponseModel profileResponse)
        {
            if (profileResponse.Profile != null)
            {
                var profile = profileResponse.Profile;
                profile.UserName = ConvertFromByteToString(profile.UserName);
                UserProfile = profileResponse.Profile;
                ScoreCount = UserProfile.Score.ToString();
            }
        }

        private string ConvertFromByteToString(string strInByte)
        {
            try
            {
                var convertStr = string.Join("-", System.Text.RegularExpressions.Regex.Matches(strInByte, @"..").Cast<System.Text.RegularExpressions.Match>().ToList());
                string[] tempArr = convertStr.Split('-');
                byte[] decBytes = new byte[tempArr.Length];
                for (int i = 0; i < tempArr.Length; i++)
                {
                    decBytes[i] = System.Convert.ToByte(tempArr[i], 16);
                }
                string strWithEmoji = Encoding.BigEndianUnicode.GetString(decBytes, 0, decBytes.Length);
                return strWithEmoji;
            }
            catch (FormatException)
            {
                return strInByte;
            }
        }
        #endregion
    }
}
