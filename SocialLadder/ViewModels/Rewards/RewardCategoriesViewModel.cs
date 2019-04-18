using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Droid.Interfaces;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Rewards
{
    public class RewardCategoriesViewModel : BaseViewModel, IMainTabViewModel
    {
        public RewardCategoriesViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            BackButtonHidden = false; 
            _refreshCommand = new MvxAsyncCommand(Refresh);
            _onRewardCategorySelectedCommand = new MvxAsyncCommand<RewardItemModel>(OnRewardCategorySelected);
            PlaceholderModel = LocalPlaceholderHelper.GetRewardsPlacehplder();
        }

        private MvxObservableCollection<RewardItemModel> _rewardsCategories;
        public MvxObservableCollection<RewardItemModel> RewardsCategories
        {
            get
            {
                return _rewardsCategories;
            }
            set
            {
                _rewardsCategories = value;
                RaisePropertyChanged(() => RewardsCategories);
            }
        }

        private IMvxAsyncCommand<RewardItemModel> _onRewardCategorySelectedCommand;
        public IMvxAsyncCommand<RewardItemModel> OnRewardCategorySelectedCommand
        {
            get
            {
                return _onRewardCategorySelectedCommand;
            }
        }

        private IMvxAsyncCommand _refreshCommand;
        public IMvxAsyncCommand RefreshCommand
        {
            get
            {
                return _refreshCommand;
            }
        }

        private async Task RefreshRewards()
        {
            var rewardResponse = await SL.Manager.GetRewardsAsync(null);
            if (rewardResponse.RewardList == null || rewardResponse.RewardList.Count == 0)
            {
                PlaceholderHidden = true;
                return;
            }
            PlaceholderHidden = false;
            if (rewardResponse.RewardList != null)
            {
                RewardsCategories = new MvxObservableCollection<RewardItemModel>(rewardResponse.RewardList);
            }
        }

        private async Task RefreshProfile()
        { 
            await SL.Manager.GetProfileAsync();  
        }

        private async Task OnRewardCategorySelected(RewardItemModel reward)
        {
            await _navigationService.Navigate<RewardsViewModel>();
            MessangerRewardModel rewardMessage = new MessangerRewardModel(this, reward);
            _messenger.Publish<MessangerRewardModel>(rewardMessage);
        }

        public override async Task Refresh()
        {
            IsBusy = IsRefreshing = true;
            await RefreshRewards();
            await RefreshProfile();
            IsBusy = IsRefreshing = false;
        }

        public void RefreshNotificationSettings()
        {
            var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
            if ((notificationStatus != Enums.NotififcationStatus.Discarded.ToString()) && (notificationStatus != Enums.NotififcationStatus.Enabled.ToString()))
            {
                string message = "Challenges expire quickly - allow this app to send you push notifications to be notified about special opportunities";
                if (Constants.NeedFirebaseImpl)
                {
                    var _notificationService = Mvx.Resolve<IFirebaseService>();

                    if (_notificationService == null || !_notificationService.IsPushNotificationServiceAlailable())
                    {
                        _alertService.ShowToast("Push notification service isn't alailable");
                        return;
                    }
                    var notificationOkResponse = new ConfigurableAlertAction
                    {
                        Text = "Yes",
                        OnClickHandler = new Action(() =>
                        {
                            _notificationService.RegisterPushNotificationService();
                            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Enabled.ToString());
                        })
                    };
                    var notificationCancelResponse = new ConfigurableAlertAction
                    {
                        Text = "No",
                        OnClickHandler = new Action(() =>
                        {
                            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Disabled.ToString());
                        })
                    };
                    var notificationNeverResponse = new ConfigurableAlertAction
                    {
                        Text = "Never",
                        OnClickHandler = new Action(() =>
                        {
                            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Discarded.ToString());
                        })
                    };
                    _alertService.ShowAlertMessage(message, notificationOkResponse, notificationCancelResponse, notificationNeverResponse);
                }
                else
                {
                    var _notificationService = Mvx.Resolve<IPushNotificationService>();
                    if (_notificationService == null || !_notificationService.IsPushNotificationServiceAlailable())
                    {
                        _alertService.ShowToast("Push notification service isn't alailable");
                        return;
                    }
                    var notificationOkResponse = new ConfigurableAlertAction
                    {
                        Text = "Yes",
                        OnClickHandler = new Action(() => {
                            _notificationService.RegisterPushNotificationService();
                            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Enabled.ToString());
                        })
                    };
                    var notificationCancelResponse = new ConfigurableAlertAction
                    {
                        Text = "No",
                        OnClickHandler = new Action(() => {
                            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Disabled.ToString());
                        })
                    };
                    var notificationNeverResponse = new ConfigurableAlertAction
                    {
                        Text = "Never",
                        OnClickHandler = new Action(() => {
                            SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Discarded.ToString());
                        })
                    };
                    _alertService.ShowAlertMessage(message, notificationOkResponse, notificationCancelResponse, notificationNeverResponse);
                }
            }
        }
    }
}

