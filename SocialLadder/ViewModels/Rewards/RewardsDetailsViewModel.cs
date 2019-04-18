using AdvancedTimer.Forms.Plugin.Abstractions;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Rewards
{
    public class RewardsDetailsViewModel : BaseViewModel, IBaseChildViewModel
    {
        #region Properties
        private string _rewardImageUrl;
        public string RewardImageUrl
        {
            get
            {
                return _rewardImageUrl;
            }
            set
            {
                _rewardImageUrl = value;
                RaisePropertyChanged(() => RewardImageUrl);
            }
        }

        private string _rewardName;
        public string RewardName
        {
            get
            {
                return _rewardName;
            }
            set
            {
                _rewardName = value;
                RaisePropertyChanged(() => RewardName);
            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get
            {
                return _categoryName;
            }
            set
            {
                _categoryName = value;
                RaisePropertyChanged(() => CategoryName);
            }
        }

        private int _rewardScore;
        public int RewardScore
        {
            get
            {
                return _rewardScore;
            }
            set
            {
                _rewardScore = value;
                RaisePropertyChanged(() => RewardScore);
            }
        }

        private bool _isRewardBlocked;
        public bool IsRewardBlocked
        {
            get
            {
                return _isRewardBlocked;
            }
            set
            {
                _isRewardBlocked = value;
                RaisePropertyChanged(() => IsRewardBlocked);
            }
        }

        private bool _isCompleteViewVisible;
        public bool IsCompleteViewVisible
        {
            get
            {
                return _isCompleteViewVisible;
            }
            set
            {
                _isCompleteViewVisible = value;
                RaisePropertyChanged(() => IsCompleteViewVisible);
            }
        }

        private string _rewardUnlockTime;
        public string RewardUnlockTime
        {
            get
            {
                return _rewardUnlockTime;
            }
            set
            {
                _rewardUnlockTime = value;
                RaisePropertyChanged(() => RewardUnlockTime);
            }
        }

        private string _rewardOfferTime;
        public string RewardOfferTime
        {
            get
            {
                return _rewardOfferTime;
            }
            set
            {
                _rewardOfferTime = value;
                RaisePropertyChanged(() => RewardOfferTime);
            }
        }

        private bool _isRewardButtonEnabled;
        public bool IsRewardButtonEnabled
        {
             get
            {
                return _isRewardButtonEnabled;
            }
            set
            {
                _isRewardButtonEnabled = value;
                RaisePropertyChanged(() => IsRewardButtonEnabled);
            }
        }

        private bool _isEnoughtPoints;
        public bool IsEnoughtPoints
        {
            get
            {
                return _isEnoughtPoints;
            }
            set
            {

                _isEnoughtPoints = value;
                RaisePropertyChanged(() => IsEnoughtPoints);
            }
        }

        private string _tapCounterText;
        public string TapCounterText
        {
            get => _tapCounterText;
            set => SetProperty(ref _tapCounterText, value);
        }

        private int _tapCounter;
        private int TapCounter
        {
            get { return _tapCounter; }
            set {
                _tapCounter = value;
                TapCounterText = _tapCounter + " Tap";
                if (_tapCounter != 1)
                {
                    TapCounterText += "s";
                }
                
            }
        }

        private RewardItemModel _rewardItemModel;
        public RewardItemModel RewardItem
        {
            get
            {
                return _rewardItemModel;
            }
            set
            {
                _rewardItemModel = value;
                RewardImageUrl = _rewardItemModel.MainImageURL;
                HtmlDescription = _rewardItemModel.Description;
                RewardScore = _rewardItemModel.MinScore;
                if (_rewardItemModel.AutoUnlockDate.HasValue)
                {

                    IsRewardBlocked = _rewardItemModel.AutoUnlockDate.Value.ToUniversalTime() > DateTime.Now.ToUniversalTime();
                }
                else
                {
                    IsRewardBlocked = false;
                }
                IsEnoughtPoints = _rewardItemModel.MinScore < SL.Profile.Score;
            }
        }

        private bool _isSubmitionEnabled;
        public bool IsSubmitionEnabled
        {
            get
            {
                return _isSubmitionEnabled;
            }
            set
            {
                _isSubmitionEnabled = value;
                RaisePropertyChanged(() => IsSubmitionEnabled);
            }
        }

        #endregion

        private MvxCommand _claimRewardCommand;

        public IMvxCommand ClaimRewardCommand
        {
            get
            {
                return _claimRewardCommand;
            }
        }

        private MvxAsyncCommand _browseRewardsCommand;
        public MvxAsyncCommand BrowseRewadsCommand
        {
            get
            {
                return _browseRewardsCommand;
            }
        }


        private IMvxAsyncCommand _closeClaimRewardViewCommand;

        public IMvxAsyncCommand CloseClaimRewardViewCommand
        {
            get
            {
                return _closeClaimRewardViewCommand;
            }
        }

        private MvxInteraction<RewardResponseModel> _claimRewardInteraction;
        public IMvxInteraction<RewardResponseModel> ClaimRewardInteraction => _claimRewardInteraction;

        public string HtmlDescription { get; set; }
        public bool IsClaimed { get; set; }

        private IAdvancedTimer _timer;

        public RewardsDetailsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IAdvancedTimer advancedTimer) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            TapCounter = 0;
            IsSubmitionEnabled = false;
            _timer = advancedTimer;
            _timer.initTimer(1000, OnTimerHandler, true);
            _claimRewardInteraction = new MvxInteraction<RewardResponseModel>();
            _token = _messenger.Subscribe<MessangerRewardModel>(OnRewardMessage);
            _claimRewardCommand = new MvxCommand(ClaimReward);

            _browseRewardsCommand = new MvxAsyncCommand(BrowseRewards);
            _closeClaimRewardViewCommand = new MvxAsyncCommand(async () =>
            {
                if (IsClaimed)
                {
                    Close(this);
                    IsCompleteViewVisible = false;
                }
                else
                {
                    IsCompleteViewVisible = false;
                }
            });
        }
        
        public override void ViewAppearing()
        {
            base.ViewAppeared();
            WebViewData = new LocalWebDataModel { Data = HtmlDescription };
        }

        public override void ViewDisappearing()
        {
            _messenger.Unsubscribe<MessangerRewardModel>(_token);
            base.ViewDisappearing();
        }

        public void OnRewardMessage(MessangerRewardModel parameter)
        {
            RewardItem = parameter.RewardItem; 
            CategoryName = parameter.CategoryName;
            RewardName = parameter.RewardItem.Name;
            IsSubmitionEnabled = true;
            IsCompleteViewVisible = false;
            _timer.startTimer(); 
            UpdateTimer();
        }
         

        private void ClaimReward()
        {
            _alertService.ShowOkCancelMessage("", "Are you sure? You can only get one reward in this group, so make sure this is one you want!", async ()=>
            {
                //OnRewadClaimed(new RewardResponseModel() { ResponseMessage = "teste", ResponseCode = 1, UpdatedReward = RewardItem });
                //return;

                if (!IsEnoughtPoints)
                {
                    return;
                }

                IsBusy = true;
                await SL.Manager.PostCommitReward(RewardItem.ID, OnRewadClaimed);
            }, null); 
        } 

        private void OnRewadClaimed(RewardResponseModel response)
        {
            IsBusy = false;
            IsClaimed = (response.ResponseCode > 0) ? true : false;
            if (response.UpdatedReward == null)
            {
                response.UpdatedReward = RewardItem;
            }
            TapCounter++;
            _claimRewardInteraction.Raise(response);
        }

        private void OnTimerHandler(object s, EventArgs e)
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        { 
            if (RewardItem.AutoUnlockDate != null)
            {
                var timeSpan = (RewardItem.AutoUnlockDate?.ToLocalTime() ?? DateTime.Now) - DateTime.Now;
                if (timeSpan != null)
                {
                    RewardUnlockTime = string.Format(timeSpan.Days > 0 ? "{0:dd}d:{0:hh}h:{0:mm}m" : "{0:hh}h:{0:mm}m:{0:ss}s", timeSpan);
                }
            }
            if (RewardItem.OfferExpirationDate != null)
            {
                var offerTimeSpan = (RewardItem.OfferExpirationDate?.ToLocalTime() ?? DateTime.Now) - DateTime.Now;
                if (offerTimeSpan != null)
                {
                    RewardOfferTime = string.Format(offerTimeSpan.Days > 0 ? "{0:dd}d:{0:hh}h:{0:mm}m left to claim this reward!" : "{0:hh}h:{0:mm}m:{0:ss}s left to claim this reward!", offerTimeSpan);
                }
            }
        }

        public async Task BrowseRewards()
        {
            if (IsRewardBlocked)
            {
                IsCompleteViewVisible = false;
                Close(this);
            }
            else
            {
                await _navigationService.Navigate<WebViewModel>();
                _messenger.Publish<MessangerWebModel>(new MessangerWebModel(this, string.Format("https://socialladder.rkiapps.com/SL/HelpDesk/RewardStatus?deviceUUID={0}&AreaGUID={1}", SL.DeviceUUID, SL.AreaGUID), true));
                Close(this);
            }
        }
    }
}
