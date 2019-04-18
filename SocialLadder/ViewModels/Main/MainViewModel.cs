using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels;
using SocialLadder.Models.LocalModels.Areas;
using SocialLadder.Models.LocalModels.Mappers;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Feed;
using SocialLadder.ViewModels.Intro;
using SocialLadder.ViewModels.More;
using SocialLadder.ViewModels.Points;
using SocialLadder.ViewModels.Rewards;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Main
{
    public class MainViewModel : BaseViewModel
    {
        public event EventHandler AreaChanged;
        #region Properties
        private MvxObservableCollection<AreaModel> _areasCollection;
        public MvxObservableCollection<AreaModel> AreasCollection
        {
            get => _areasCollection;
            set => SetProperty(ref _areasCollection, value);
        }

        private bool _detailsViewHidden;
        public bool DetailsViewHidden
        {
            get => _detailsViewHidden;
            set => SetProperty(ref _detailsViewHidden, value);
        }

        private bool _changeCurrentArea;
        public bool ChangeCurrentArea
        {
            get => _changeCurrentArea;
            set => SetProperty(ref _changeCurrentArea, value);
        }

        private LocalWebDataModel _wizardWebViewData;
        public LocalWebDataModel WizardWebViewData
        {
            get => _wizardWebViewData;
            set => SetProperty(ref _wizardWebViewData, value);
        }

        private bool _wizardViewHidden;
        public bool WizardViewHidden
        {
            get => _wizardViewHidden;
            set => SetProperty(ref _wizardViewHidden, value);
        }

        private bool _internetConnectionViewHidden;
        public bool InternetConnectionViewHidden
        {
            get => _internetConnectionViewHidden;
            set => SetProperty(ref _internetConnectionViewHidden, value);
        }
        #endregion

        #region Interactions
        private MvxInteraction<LocalChangedMainViewModel> _mainViewModelChanged;
        public IMvxInteraction<LocalChangedMainViewModel> MainViewModelChanged
        {
            get
            {
                if (_mainViewModelChanged == null)
                {
                    _mainViewModelChanged = new MvxInteraction<LocalChangedMainViewModel>();
                }
                return _mainViewModelChanged;
            }
        }
        #endregion

       
        #region Fields
        private readonly IPlatformAssetService _assetService;
        private readonly ILocationService _locationservice;
        #endregion

        #region Constructors
        public MainViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, ILocationService locationservice) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            InternetConnectionViewHidden = true;
            _assetService = assetService;
            _locationservice = locationservice;
            ShowFeedViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<FeedViewModel>());
            ShowChallengesViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<ChallengeViewModel>());
            ShowRewardsViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<RewardCategoriesViewModel>());
            ShowMoreViewModelCommand = new MvxAsyncCommand(async () => await _navigationService.Navigate<MoreViewModel>());
            AreasCollection = new MvxObservableCollection<AreaModel>();
            NavigationHelper.ShowWebViewFromIntroVM = false;
        }
        #endregion

        #region Lifecycle
        public override void Start()
        {
            base.Start();
            CrossConnectivity.Current.ConnectivityChanged += (s, e) =>
            {
                if (!e.IsConnected)
                {
                    //_alertService.ShowToast("Internet connection is lost");
                    InternetConnectionViewHidden = false;
                }
                if (e.IsConnected)
                {
                    //_alertService.ShowToast("Internet connection restored");
                    InternetConnectionViewHidden = true;
                    Task.Run(async () => { await LastTabVM.Refresh(); });                  
                }
            };
            BackButtonHidden = false;
            NotificationDotHidden = true;
            WizardViewHidden = true;
        }

        public async override Task Initialize()
        {
            InitAreasCollection();
            await base.Initialize();
            await CheckProfileAreas();
        }
        #endregion

        #region Commands
        private MvxCommand _refreshAreasCommand;
        public MvxCommand RefreshAreasCommand
        {
            get
            {
                return _refreshAreasCommand = _refreshAreasCommand ?? new MvxCommand(InitAreasCollection);
            }
        }

        public IMvxAsyncCommand ShowFeedViewModelCommand
        {
            get; private set;
        }

        public IMvxAsyncCommand ShowChallengesViewModelCommand
        {
            get; private set;
        }

        public IMvxAsyncCommand ShowRewardsViewModelCommand
        {
            get; private set;
        }

        public IMvxAsyncCommand ShowMoreViewModelCommand
        {
            get; private set;
        }

        public virtual MvxAsyncCommand CloseWizardViewCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    WizardViewHidden = true;
                    NavigationHelper.ShowAreasCollectionPageFromMainVM = NavigationHelper.ShowAreasCollectionNeedShowBackButton = true;
                    await _navigationService.Navigate<AreasCollectionViewModel>();
                });
            }
        }

        public MvxAsyncCommand<AreaModel> AreaClickCommand
        {
            get
            {
                return new MvxAsyncCommand<AreaModel>(async (param) =>
                {
                    IsBusy = true;
                    ChangeCurrentVMIsBusyProperty(true);
                    if (param is LocalCurrentAreaModel)
                    {
                        IsBusy = false;
                        ChangeCurrentVMIsBusyProperty(false);
                        return;
                    }
                    if (param is LocalAreasModel && (param as LocalAreasModel).IsAddAreaButton)
                    {
                        //Navigate to AreasCollectionFragment
                        NavigationHelper.ShowAreasCollectionPageFromMainVM = true;
                        await _navigationService.Navigate<AreasCollectionViewModel>();
                        IsBusy = false;
                        ChangeCurrentVMIsBusyProperty(false);
                        return;
                    }
                    await AreaClicked(param.areaID);
                });
            }
        }
        #endregion;           

        #region Override
        public async override Task Refresh()
        {
            NavigationHelper.ShowAreasCollectionNeedShowBackButton = true;
            if (SL.HasAreas)
                SL.RefreshAll();
            await base.Refresh();
            InitAreasCollection();
            Image = SL.Area.areaDefaultImageURL;
            _mainViewModelChanged.Raise(new LocalChangedMainViewModel { StatusBarColor = HexToolbarColor });
        }
        #endregion

        #region Methods
        private void InitAreasCollection()
        {
            AreasCollection.Clear();
            var areas = SL.Profile.AreaSubsList as IEnumerable<AreaModel>;
            if (areas == null || areas.Count() == 0)
            {
                return;
            }
            var currentItem = areas.Where(x => x.areaGUID == SL.AreaGUID).FirstOrDefault();
            AreasCollection.Add(new LocalAreasModel { IsAddAreaButton = true, areaName = "Find a new area" });
            foreach (var item in areas)
            {
                if (currentItem == null || (currentItem != null && item.areaID == currentItem.areaID))
                {
                    AreasCollection.Add(AreaMapper.ItemToCurrentLocalItem(item as AreaModel, _assetService.SmallScoreIcon, ScoreCount));
                    //HexToolbarColor = currentItem.areaPrimaryColor;
                    //_mainViewModelChanged.Raise(new LocalChangedMainViewModel { StatusBarColor = HexToolbarColor });
                    currentItem = item;
                    continue;
                }
                AreasCollection.Add(AreaMapper.ItemToLocalItem(item as AreaModel));
            }
        }

       

        public async Task AreaClicked(int areaId)
        {
            IsBusy = true;
            var list = AreasCollection;
            ChangeCurrentArea = true;
            var clickedItem = list.Where(x => x.areaID == areaId).FirstOrDefault();
            var currentArea = list.Where(x => x.GetType() == typeof(LocalCurrentAreaModel)).FirstOrDefault();
            await AreaSelected(clickedItem);
            if (clickedItem is LocalAreasModel)
            {
                var newCurrentItem = AreaMapper.ItemToCurrentLocalItem(clickedItem as AreaModel, _assetService.SmallScoreIcon, ScoreCount);
                //var index = list.IndexOf(clickedItem);
                int currentIndex = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].areaID == clickedItem.areaID)
                    {
                        currentIndex = i;
                        break;
                    }
                }
                if (currentIndex != -1)
                {
                    list[currentIndex] = newCurrentItem;
                }
                //list[list.IndexOf(clickedItem)] = newCurrentItem;
            }
            if (currentArea is LocalCurrentAreaModel)
            {
                var oldcurrentItem = AreaMapper.ItemToLocalItem(currentArea as AreaModel);
                int currentIndex = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].areaID == currentArea.areaID)
                    {
                        currentIndex = i;
                        break;
                    }
                }
                if (currentIndex != -1)
                {
                    list[currentIndex] = oldcurrentItem;
                }
                //list[list.IndexOf(currentArea)] = oldcurrentItem;
            }
            //await AreaSelected(clickedItem);
            AreasCollection = list;
            ChangeCurrentArea = false;
            ChangeCurrentVMIsBusyProperty(false);
            IsBusy = false;
        }

        private async Task AreaSelected(AreaModel area)
        {
            if (area != null)
            {
                AreaChanged.Invoke(null, null);
                ScoreImage = _assetService.NetworkConnectingLoaderImage;
                SL.AreaGUID = area.areaGUID;
                try
                {
                    SL.ChallengeList = null;
                    SL.RewardList = null;
                    SL.Feed = null;
                }
                catch (Exception ex) { Debug.WriteLine("Can't clear SL collections"); }
                var location = await _locationservice.GetCurrentLocation();
                ProfileResponseModel response = await SL.Manager.FinalCheckInAsync(location.Lat, location.Long);

                Title = area.areaName;
                HexToolbarColor = area.areaPrimaryColor;
                _mainViewModelChanged.Raise(new LocalChangedMainViewModel { StatusBarColor = HexToolbarColor });
                if (SL.HasProfile)
                    ScoreCount = SL.Profile.Score.ToString();
                if (LastTabVM != null)
                {
                    await LastTabVM.Refresh();
                }
                Image = area.areaDefaultImageURL;
                ScoreImage = _assetService.SmallScoreIcon;
            }
        }

        private void ChangeCurrentVMIsBusyProperty(bool param)
        {
            if (CurrentVM != null)
            {
                CurrentVM.IsBusy = CurrentVM.IsRefreshing = param;
            }
        }

        public void RefreshScoreView()
        {
            if (Profile != null)
            {
                ScoreCount = Profile.Score.ToString();
            }
        }

        private async Task CheckProfileAreas()
        {
            if (!SL.HasAreas/* && string.IsNullOrEmpty(SL.AreaGUID)*/)
            {
                NavigationHelper.ShowAreasCollectionPageFromMainVM = NavigationHelper.ShowAreasCollectionPageAfterLogout = true;
                NavigationHelper.ShowAreasCollectionNeedShowBackButton = false;
                await _navigationService.Navigate<AreasCollectionViewModel>();
                //_alertService.ShowToast("There are no areas");
                return;
            }
        }

        public async void ChangeCurrentAreaFromAreaVM(AreaModel param, bool newAreaWasAdded)
        {
            if (newAreaWasAdded)
            {
                await Refresh();
            }
            if (param == null)
            {
                await LastTabVM.Refresh();
                return;
            }
            if (param.areaID == SL.Area.areaID)
            {
                var item = AreaMapper.ItemToCurrentLocalItem(param, string.Empty, string.Empty);
                AreaClickCommand.Execute(item);
            }
            if (param.areaID != SL.Area.areaID)
            {
                var item = AreaMapper.ItemToLocalItem(param);
                AreaClickCommand.Execute(item);
            }
            //Refresh();
        }

        public void ShowWizardWebViewWithUrl(string url)
        {
            WizardWebViewData = new LocalWebDataModel { Url = url };
            WizardViewHidden = false;
        }
        #endregion
    }
}
