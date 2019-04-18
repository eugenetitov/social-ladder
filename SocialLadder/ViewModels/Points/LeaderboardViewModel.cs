using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Mappers;
using SocialLadder.Models.LocalModels.Points;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Feed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Points
{
    public class LeaderboardViewModel : BasePointsViewModel, IMainTabViewModel
    {

        #region Properties
        private MvxObservableCollection<LocalFriendModel> _leaderboardItems;
        public MvxObservableCollection<LocalFriendModel> LeaderboardItems
        {
            get => _leaderboardItems;
            set
            {
                SetProperty(ref _leaderboardItems, value);
            }
        }

        private bool _placeholderHidden;
        public bool PlaceholderHidden
        {
            get => _placeholderHidden;
            set
            {
                SetProperty(ref _placeholderHidden, value);
            }
        }

        private string _placeholderText;
        public string PlaceholderText
        {
            get => _placeholderText;
            set => SetProperty(ref _placeholderText, value);
        }

        private string _profileLeaderboardPosition;
        public string ProfileLeaderboardPosition
        {
            get => _profileLeaderboardPosition;
            set => SetProperty(ref _profileLeaderboardPosition, value);
        }

        //private bool _itemSelected;
        //public bool ItemSelected
        //{
        //    get => _itemSelected;
        //    set => SetProperty(ref _itemSelected, value);
        //}

        public int ScoreFactor { get; set; } = 100;         

        #endregion

        public LeaderboardViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            IsBusy = true;
            LeaderboardItems = new MvxObservableCollection<LocalFriendModel>();
        }

        public async override Task InitVM()
        {
            string rank = CountSuffixGenerator.GenerateSuffixedStringFromString(Profile.FriendRank);
            ProfileLeaderboardPosition = String.Format("Ranked {0} among friends", rank);
            string nameArea = string.IsNullOrEmpty(SL.AreaName) ? string.Empty : " in " + SL.AreaName;
            PlaceholderText = "We don't see any of your friend" + nameArea;

            int? friendsCount = Profile?.FriendPreviewList?.Count;
            double? maxScore = 0;
            if (friendsCount != null)
            {
                maxScore = friendsCount > 0 ? Profile.FriendPreviewList[0].Score : 0;
                var MaxScoreValue = maxScore.HasValue == true ? (float)(maxScore.Value) : 0;
            }

            LeaderboardItems.Clear();
            var source = SL.FriendList;
            PlaceholderHidden = source == null || source.Count == 0 ? false : true;
            if (source == null)
            {
                return;
            }
            foreach (var item in source)
            {
                var friend = FriendMapper.ItemToLocalItem(item);
                friend.CurrentScoreValue = (double)(item.Score != null && maxScore != null ? ((item.Score * ScoreFactor) / maxScore) : 0);
                LeaderboardItems.Add(friend);
            }
            await base.InitVM();
        }

        public async override Task RefreshOther()
        {
            await base.RefreshOther();
            await Task.Run(async () =>
            {
                await SL.Manager.GetChallengesAsync((response) => { });
                await SL.Manager.GetRewardsAsync((response) => { });
            });
        }

        #region Commands
        public MvxAsyncCommand EarnMoreCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate<ChallengeViewModel>();
                });
            }
        }

        public MvxAsyncCommand<LocalFriendModel> LeaderboardItemClicked
        {
            get
            {
                return new MvxAsyncCommand<LocalFriendModel>(async (param) =>
                {
                    //ItemSelected = true;                 
                    //LeaderboardItems.Where(x => x.IsSelected).All(x => x.IsSelected = false);
                    //var selectedItem = param;
                    //selectedItem.IsSelected = true;
                    //var index = LeaderboardItems.IndexOf(param);
                    //LeaderboardItems[index] = selectedItem;
                    IsBusy = true;
                    await _navigationService.Navigate<FeedDetailsViewModel>();
                    _messenger.Publish(new MessangerFeedUrlModel(this, string.Empty, string.Empty, param.SCSUserProfileID, true));
                    IsBusy = false;
                    //ItemSelected = false;
                });
            }
        }
        #endregion
    }
}
