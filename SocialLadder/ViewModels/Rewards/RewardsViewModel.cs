using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Plugin.SecureStorage;
using SocialLadder.Enums.Constants;
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

namespace SocialLadder.ViewModels.Rewards
{
    public class RewardsViewModel : BaseViewModel, IBaseChildViewModel
    {
        private string _categoryName;
        private string _subCategoryName;

        public string CategoryName
        {
            get
            {
                if ((_subCategoryName != null) && (_categoryName != null))
                {
                    return string.Format("{0}      |      {1}", _categoryName, _subCategoryName);
                }
                if (_categoryName != null)
                {
                    return _categoryName;
                }
                if (_subCategoryName != null)
                {
                    return _subCategoryName;
                }
                return null;
            }
        }

        private MvxObservableCollection<RewardItemModel> _rewardItems;
        public MvxObservableCollection<RewardItemModel> RewardItems
        {
            get => _rewardItems;
            set => SetProperty(ref _rewardItems, value);
        }

        private IMvxAsyncCommand<RewardItemModel> _onRewardClickCommand;
        public IMvxAsyncCommand<RewardItemModel> OnRewardClickCommand
        {
            get
            {
                return _onRewardClickCommand;
            }
        }



        public RewardsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _onRewardClickCommand = new MvxAsyncCommand<RewardItemModel>(DrillDown);
            _token = _messenger.Subscribe<MessangerRewardModel>(OnRewardMessage);
            IsRefreshing = true;
        }

        public override void ViewDisappearing()
        {
            _messenger.Unsubscribe<MessangerRewardModel>(_token);
            base.ViewDisappearing();
        }


        public override async Task BackCommandInvoke()
        {
            var backCategoryName = CrossSecureStorage.Current.GetValue("parentCategory", string.Empty);
            if (CategoryName == backCategoryName || (CategoryName != backCategoryName && !CategoryName.Contains(backCategoryName)))
            {
                await base.BackCommandInvoke();
                return;
            }

            var parent = CrossSecureStorage.Current.GetValue("parentRewards", string.Empty);
            var rewardsParentList = Newtonsoft.Json.JsonConvert.DeserializeObject<MvxObservableCollection<RewardItemModel>>(parent);

            if(rewardsParentList != null)
            {
                var parameter = new MessangerRewardModel(this, rewardsParentList, backCategoryName);
                await _navigationService.Navigate<RewardsViewModel>();
                _messenger.Unsubscribe<MessangerRewardModel>(_token);
                _messenger.Publish<MessangerRewardModel>(parameter);
            }
            else
            {
                await base.BackCommandInvoke();
                return;
            }
            
        }

        public void OnRewardMessage(MessangerRewardModel parameter)
        {
            if (parameter.RewardItem == null && parameter.ParentRewardItems.Any())
            {
                _categoryName = parameter.CategoryName;
                RaisePropertyChanged(() => CategoryName);
                RewardItems = new MvxObservableCollection<RewardItemModel>(parameter.ParentRewardItems);
                IsRefreshing = false;
                return;
            }

            if (parameter.RewardItem.ChildList != null)
            {
                _categoryName = parameter.CategoryName;
                _subCategoryName = parameter.RewardItem.Name;
                RaisePropertyChanged(() => CategoryName);
                RewardItems = new MvxObservableCollection<RewardItemModel>(parameter.RewardItem.ChildList);

                IsRefreshing = false;
                return;
            }
            _categoryName = parameter.RewardItem.Name;
            SL.Manager.GetRewardsDrilldownAsync(parameter.RewardItem.ID, OnRewardDrillDownCompleted);
            RaisePropertyChanged(() => CategoryName);
        }

        private void OnRewardDrillDownCompleted(RewardResponseModel model)
        {
            RewardItems = new MvxObservableCollection<RewardItemModel>(model.RewardList);
            for (int item = 0; item < RewardItems.Count; item++)
            {
                if (RewardItems[item].NumChildren == null && RewardItems[item].ChildList != null)
                {
                    RewardItems[item].NumChildren = RewardItems[item].ChildList.Count;
                }
            }
            IsRefreshing = false;
            //RaisePropertyChanged(() => IsRefreshing);
        }

        private async Task DrillDown(RewardItemModel itemModel)
        {
            var parameter = new MessangerRewardModel(this, itemModel, CategoryName);
            if (itemModel.Type == ActionTypeConstants.CategoryRewardType)
            {
                var parentList = Newtonsoft.Json.JsonConvert.SerializeObject(RewardItems);
                CrossSecureStorage.Current.SetValue("parentRewards", parentList);
                CrossSecureStorage.Current.SetValue("parentCategory", CategoryName);

                await _navigationService.Navigate<RewardsViewModel>();
                _messenger.Unsubscribe<MessangerRewardModel>(_token);
                _messenger.Publish<MessangerRewardModel>(parameter);
                return;

            }
            await _navigationService.Navigate<RewardsDetailsViewModel>();
            _messenger.Unsubscribe<MessangerRewardModel>(_token);
            _messenger.Publish<MessangerRewardModel>(parameter);
        }
    }

    public class RewardsNavigationParameterModel
    {
        public RewardItemModel RewardItem
        {
            get; set;
        }

        public string CategoryName
        {
            get; set;
        }

        public string SubcategoryName
        {
            get; set;
        }
    }
}



