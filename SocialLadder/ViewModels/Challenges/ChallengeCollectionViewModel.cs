using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Droid.Interfaces;
using SocialLadder.Enums.Constants;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.Models.LocalModels.Mappers;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class ChallengeViewModel : BaseViewModel, IMainTabViewModel
    {
        private readonly IPlatformAssetService _assetService;
        private readonly IPlatformNavigationService _platformNavigationService;
        private readonly ILocationService _locationService;
        public static bool IsFirstInitialize { get; set; } = true;
        #region Properties
        private int ChallengeListCount { get; set; }

        private MvxObservableCollection<LocalChallengeTypeModel> _challengesCollection;
        public MvxObservableCollection<LocalChallengeTypeModel> ChallengesCollection
        {
            get => _challengesCollection;
            set => SetProperty(ref _challengesCollection, value);
        }

        private MvxObservableCollection<LocalChallengeModel> _challengesList;
        public MvxObservableCollection<LocalChallengeModel> ChallengesList
        {
            get => _challengesList;
            set => SetProperty(ref _challengesList, value);
        }

        private int SelectedIndex { get; set; } = -1;

        private List<string> OrderTitles = new List<string>() { "Expiring", "Recently Added", "More", "Pending Approval", "Completed" };
        #endregion

        public ChallengeViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IPlatformNavigationService platformNavigationService, ILocationService locationService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _assetService = assetService;
            _platformNavigationService = platformNavigationService;
            _locationService = locationService;
            PlaceholderModel = LocalPlaceholderHelper.GetChallengesPlacehplder();
        }

        public async override Task Initialize()
        {
            await base.Initialize();
            await RefreshChallenges();
        }

        private async Task InitTable()
        {
            IsRefreshing = true;
            ChallengesList = new MvxObservableCollection<LocalChallengeModel>();
            var challengesList = SL.ChallengeList.OrderBy(x => x.EffectiveEndDate).ToList();
            if (challengesList == null || challengesList.Count == 0)
            {
                IsRefreshing = false;
                return;
            }
            if (challengesList != null || challengesList.Count > 0)
            {
                FillUpChallengesList(challengesList.OrderBy(o => o.Group).ToList());
                ChallengeListCount = ChallengesList.Count;
            }
            IsRefreshing = false;
        }

        private List<string> InitSections()
        {
            return OrderTitles.Where(x => ChallengesList.FirstOrDefault(y => x.Equals(y.Group)) != null).ToList();
        }

        #region Commands
        public MvxCommand<LocalChallengeTypeModel> ChallengesCollectionClickCommand
        {
            get
            {
                return new MvxCommand<LocalChallengeTypeModel>((param) =>
                {
                    IsBusy = true;

                    var collection = ChallengesCollection;
                    for (int i = 0; i < collection.Count; i++)
                    {
                        if (collection[i] != param as LocalChallengeTypeModel)
                        {
                            var item = ChallengesCollectionMapper.ItemToLocalItem(collection[i], _assetService);
                            item.ItemState = Enums.ChallengesCollectionItemState.Unselected;
                            collection[i] = item;
                        }
                    }
                    SelectedIndex = ChallengesCollection.IndexOf(param);
                    if (SelectedIndex < 0)
                    {
                        return;
                    }
                    param.ItemState = (param.ItemState == Enums.ChallengesCollectionItemState.Selected ? Enums.ChallengesCollectionItemState.Unselected : Enums.ChallengesCollectionItemState.Selected);
                    if (param.ItemState == Enums.ChallengesCollectionItemState.Selected)
                    {
                        SetFilterToChallengesList(param.TypeCode, param.DisplayName);
                    }
                    if (param.ItemState == Enums.ChallengesCollectionItemState.Unselected)
                    {
                        InitTable();
                    }
                    collection[SelectedIndex] = param;
                    ChallengesCollection = collection;
                    if (ChallengesCollection[SelectedIndex].ItemState != Enums.ChallengesCollectionItemState.Selected)
                    {
                        SelectedIndex = -1;
                    }
                    IsBusy = false;
                });
            }
        }

        public MvxAsyncCommand<ChallengeModel> ChallengesListClickCommand
        {
            get
            {
                return new MvxAsyncCommand<ChallengeModel>(async (param) =>
                {
                    IsBusy = true;

                    if (param.TypeCode == ChallengesConstants.ChallengeMC || param.TypeCode == ChallengesConstants.ChallengeSignUp)
                    {
                        await _navigationService.Navigate<MultipleChoiceViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeInsta)
                    {
                        await _navigationService.Navigate<InstagramViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeCheckin)
                    {
                        await _navigationService.Navigate<CheckInViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeShare && param.TypeCodeDisplayName == ChallengesConstants.ChallengeTwitterDisplayNames)
                    {
                        await _navigationService.Navigate<TwitterViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeShare && param.TypeCodeDisplayName == ChallengesConstants.ChallengeFacebookDisplayNames)
                    {
                        await _navigationService.Navigate<FacebookViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeFBEngagement)
                    {                     
                        await _navigationService.Navigate<FBEngagementViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengePostering)
                    {
                        await _navigationService.Navigate<PosteringViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeFlyering)
                    {
                        await _navigationService.Navigate<FlyeringViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeCollateralTracking)
                    {
                        await _navigationService.Navigate<CollateralTrackingViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeManual)
                    {
                        await _navigationService.Navigate<ManualViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeInvite && (param.TypeCodeDisplayName == ChallengesConstants.ChallengeInviteToBuyDisplayNames || param.TypeCodeDisplayName == ChallengesConstants.ChallengeInviteToJoinDisplayNames))
                    {
                        await _navigationService.Navigate<InviteViewModel>();
                    }
                    if (param.TypeCode == ChallengesConstants.ChallengeDocsubmit)
                    {
                        await _navigationService.Navigate<DocSubmitViewModel>();
                    }
                    else
                    {
                        //controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("ChallengeDetailViewController");
                    }
                    _messenger.Publish(new MessangerChallengeModel(this, param));
                    IsBusy = false;
                });
            }
        }
        #endregion

        #region UpdateCollection
        private void FillUpChallengesList(List<ChallengeModel> list)
        {
            ChallengesList = new MvxObservableCollection<LocalChallengeModel>();
            var sectionList = OrderTitles.Where(x => list.FirstOrDefault(y => x.Equals(y.Group)) != null).ToList();

            var challenges = new List<LocalChallengeModel>();
            foreach (var item in list)
            {
                var sectionName = string.Empty;
                bool sectionHidden = true;

                if (sectionList.Contains(item.Group))
                {
                    var count = list.Where(x => x.Group == item.Group).Count();
                    var num = count == 1 ? " Challenge " : " Challenges ";
                    sectionName = count + " " + item.Group + num;
                    sectionList.Remove(item.Group);
                    sectionHidden = false;
                }
                challenges.Add(ChallengesListMapper.ItemToLocalItem(item, sectionName, sectionHidden, _assetService));
            }
            var orderedSections = OrderTitles.Where(x => list.FirstOrDefault(y => x.Equals(y.Group)) != null).ToList();
            foreach (var sectionitem in orderedSections)
            {
                ChallengesList.AddRange(challenges.Where(x => x.Group == sectionitem));
            }
            ChallengeListCount = ChallengesList.Count;
        }

        private void SetFilterToChallengesList(string typeCode, string displayName)
        {
            if (ChallengeListCount > ChallengesList.Count)
            {
                InitTable();
            }
            //OldSort
            //var sortedItemsList = ChallengesList.Where(x => x.TypeCode == typeCode && x.TypeCodeDisplayName == displayName).ToList();
            //ChallengesList.RemoveItems(sortedItemsList);

            //NewSort
            var sortedItemsList = ChallengesList.Where(x => x.TypeCodeDisplayName == displayName).ToList();
            ChallengesList.Clear();
            ChallengesList.AddRange(sortedItemsList);


            var sectionList = OrderTitles.Where(x => ChallengesList.FirstOrDefault(y => x.Equals(y.Group)) != null).ToList();
            var challenges = new List<LocalChallengeModel>();
            foreach (var item in ChallengesList)
            {
                var sectionName = "name";
                bool sectionHidden = true;

                if (sectionList.Contains(item.Group))
                {
                    var count = ChallengesList.Where(x => x.Group == item.Group).Count();
                    var num = count == 1 ? " Challenge " : " Challenges ";
                    sectionName = count + " " + item.Group + num;
                    sectionList.Remove(item.Group);
                    sectionHidden = false;
                }
                challenges.Add(ChallengesListMapper.ItemToLocalItem(item, sectionName, sectionHidden, _assetService));
            }
            var orderedSections = OrderTitles.Where(x => ChallengesList.FirstOrDefault(y => x.Equals(y.Group)) != null).ToList();

            ChallengesList = new MvxObservableCollection<LocalChallengeModel>();
            foreach (var sectionitem in orderedSections)
            {
                ChallengesList.AddRange(challenges.Where(x => x.Group == sectionitem));
            }
        }
        #endregion


        private async Task RefreshChallenges()
        {
            await Task.Run(async () =>
            {
                var challenges = await SL.Manager.GetChallengesAsync();
                ChallengesCollection = new MvxObservableCollection<LocalChallengeTypeModel>();
                if (challenges.ChallengeList == null || challenges.ChallengeTypeList == null || challenges.ChallengeTypeList.Count == 0)
                {
                    ChallengesList = new MvxObservableCollection<LocalChallengeModel>();
                    PlaceholderHidden = true;
                    return;
                }
                PlaceholderHidden = false;
                InvokeOnMainThread(() =>
                {
                    if (challenges.ChallengeList != null)
                    {
                        var sortedList = challenges.ChallengeList.OrderBy(x => x.EffectiveEndDate).ToList();
                        FillUpChallengesList(sortedList);
                    }
                    if (challenges.ChallengeTypeList != null)
                    {
                        for (int i = 0; i < challenges.ChallengeTypeList.Count; i++)
                        {
                            bool isSelected = SelectedIndex >= 0 && SelectedIndex == i ? true : false;
                            ChallengesCollection.Add(ChallengesCollectionMapper.ItemToLocalItem(challenges.ChallengeTypeList[i], _assetService, isSelected));
                        }
                    }
                    if (SelectedIndex >= 0)
                    {
                        SetFilterToChallengesList(ChallengesCollection[SelectedIndex].TypeCode, ChallengesCollection[SelectedIndex].DisplayName);
                    }
                });
            });
        }

        public async override Task FinishRefresh()
        {
            await RefreshChallenges();
            CheckToRegisterAppForRemoteNotifications();
            _platformNavigationService.RefreshScoreView();
            await base.FinishRefresh();
        }

        private void CheckToRegisterAppForRemoteNotifications()
        {
            if (!IsFirstInitialize)
            {
                return;
            }
            var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
            if ((notificationStatus != Enums.NotififcationStatus.Discarded.ToString()) && (notificationStatus != Enums.NotififcationStatus.Enabled.ToString()))
            {
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
                    _alertService.ShowAlertMessage(ChallengesConstants.ChallengesChackNotificationsMessageText, notificationOkResponse, notificationCancelResponse, notificationNeverResponse);
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
                    _alertService.ShowAlertMessage(ChallengesConstants.ChallengesChackNotificationsMessageText, notificationOkResponse, notificationCancelResponse, notificationNeverResponse);
                }
                IsFirstInitialize = false;
            }
        }
    }
}
