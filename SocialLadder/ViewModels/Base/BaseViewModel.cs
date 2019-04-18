using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.UI;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels;
using SocialLadder.ViewModels.Intro;
using SocialLadder.ViewModels.More;
using SocialLadder.ViewModels.Notification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Base
{
    public class BaseViewModel : MvxViewModel, IBaseViewModel, IItemActionService
    {
        #region Fields
        public MvxSubscriptionToken _token;
        protected readonly IMvxNavigationService _navigationService;
        protected readonly IAlertService _alertService;
        protected readonly IMvxMessenger _messenger;
        protected readonly IPlatformAssetService _assetService;
        protected readonly ILocalNotificationService _localNotificationService;
        public static BaseViewModel CurrentVM { get; set; }
        public static int ChildStackViewModelCount { get; set; } = 0;
        public static BaseViewModel LastTabVM { get; set; }
        #endregion

        #region Properties
        private bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set

            { SetProperty(ref _isBusy, value); }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set

            { SetProperty(ref _isRefreshing, value); }
        }

        private bool _backButtonHidden = false;
        public bool BackButtonHidden
        {
            get => _backButtonHidden;
            set => SetProperty(ref _backButtonHidden, value);
        }

        private bool _challengeCompleteViewHidden;
        public bool ChallengeCompleteViewHidden
        {
            get => _challengeCompleteViewHidden;
            set => SetProperty(ref _challengeCompleteViewHidden, value);
        }

        private bool _notificationDotHidden = true;
        public bool NotificationDotHidden
        {
            get => _notificationDotHidden;
            set => SetProperty(ref _notificationDotHidden, value);
        }

        private string _scoreCount;
        public virtual string ScoreCount
        {
            get
            {
                _scoreCount = Profile.Score.ToString();
                return _scoreCount;
            }
            set => SetProperty(ref _scoreCount, value);
        }

        private string _scoreImage;
        public string ScoreImage
        {
            get => _scoreImage;
            set => SetProperty(ref _scoreImage, value);
        }

        private bool _areaCollectionHidden;
        public bool AreaCollectionHidden
        {
            get => _areaCollectionHidden;
            set => SetProperty(ref _areaCollectionHidden, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private MvxColor _toolbarColor;
        public MvxColor ToolbarColor
        {
            get => _toolbarColor;
            set => SetProperty(ref _toolbarColor, value);
        }

        private LocalWebDataModel _webViewData;
        public LocalWebDataModel WebViewData
        {
            get => _webViewData;
            set => SetProperty(ref _webViewData, value);
        }

        private string _hexToolbarColor;
        public string HexToolbarColor
        {
            get => _hexToolbarColor;
            set => SetProperty(ref _hexToolbarColor, value);
        }

        public ProfileModel Profile
        {
            get => SL.Profile;
        }

        private bool _toolbarDoneButtonHidden;
        public bool ToolbarDoneButtonHidden
        {
            get => _toolbarDoneButtonHidden;
            set => SetProperty(ref _toolbarDoneButtonHidden, value);
        }

        private bool _toolbarBackButtonHidden;
        public bool ToolbarBackButtonHidden
        {
            get => _toolbarBackButtonHidden;
            set => SetProperty(ref _toolbarBackButtonHidden, value);
        }


        private LocalPlaceholderModel _placeholderModel;
        public LocalPlaceholderModel PlaceholderModel
        {
            get => _placeholderModel;
            set => SetProperty(ref _placeholderModel, value);
        }

        private bool _placeholderHidden;
        public bool PlaceholderHidden
        {
            get => _placeholderHidden;
            set => SetProperty(ref _placeholderHidden, value);
        }
        #endregion

        #region Ctor
        public BaseViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger = null)
        {
            _navigationService = navigationService;
            _alertService = alertService;
            ToolbarColor = Constants.ToolbarDefaultColor;
            _messenger = messanger;
            _assetService = assetService;
            _localNotificationService = localNotificationService;
            AreaCollectionHidden = false;
            ToolbarDoneButtonHidden = true;
            WebViewData = new LocalWebDataModel();
        }
        #endregion

        #region Lifecycle
        public override void Start()
        {
            base.Start();
            ScoreImage = _assetService.SmallScoreIcon;
            try
            {
                var area = SL.Area;
                Title = area.areaName;
                //HexToolbarColor = area.areaPrimaryColor;
                Image = area.areaDefaultImageURL;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Can't get area! " + ex.Message);
            }

            //CrossConnectivity.Current.ConnectivityChanged += (s, e) =>
            //{
            //    if (!e.IsConnected)
            //    {
            //        _alertService.ShowToast("Internet connection is lost");
            //    }
            //    if (e.IsConnected)
            //    {
            //        _alertService.ShowToast("Internet connection restored");
            //    }
            //};
            try
            {
                HexToolbarColor = SL.Area.areaPrimaryColor;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async override Task Initialize()
        {
            //await Refresh();
             await base.Initialize();
            ChangeChildStackVMCount(true);

            if (string.IsNullOrEmpty(SL.AreaGUID))
            {
                return;
            }
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            if (!(this is IMainTabViewModel))
            {
                CurrentVM = this;
            }
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            if (!(this is IBaseChildViewModel))
            {
                CurrentVM = LastTabVM;
            }
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);
            ChangeChildStackVMCount();
        }
        #endregion

        #region Commands
        public virtual MvxAsyncCommand BackCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await BackCommandInvoke();
                });
            }
        }

        public MvxAsyncCommand<object> ItemActionCommand
        {
            get
            {
                return new MvxAsyncCommand<object>(async (param) =>
                {
                    await ItemActionInvoke(param);
                });
            }
        }

        public virtual MvxAsyncCommand DoneCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await InvokeToolbarDoneCommand();
                });
            }
        }



        public MvxAsyncCommand NotificationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate<NotificationsViewModel>();
                });
            }
        }

        public MvxCommand ShowAreasCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AreaCollectionHidden = true;
                });
            }
        }

        public virtual MvxAsyncCommand RefreshCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await Refresh();
                });
            }
        }

        #endregion

        #region Methods

        #region RefreshMethods
        public virtual void RefreshProfileComplete(ProfileResponseModel response)
        {
            IsBusy = false;
        }

        public virtual async Task Refresh()
        {
            IsRefreshing = IsBusy = true;
            await RefreshOther();
            await SL.Manager.GetProfileAsync();
            await FinishRefresh();
        }

        public virtual async Task RefreshWithoutBusyProp()
        {
            await SL.Manager.GetProfileAsync();
        }

        public virtual async Task RefreshOther()
        {

        }

        public virtual async Task FinishRefresh()
        {
            ScoreCount = Profile != null ? Profile.Score.ToString() : string.Empty;
            try
            {
                HexToolbarColor = SL.Area.areaPrimaryColor;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            await SL.Manager.GetGuideUrl(SL.AreaGUID, new Action<GuideResponseModel>((guideModel) =>
            {
                if (guideModel.GuideUrl != null && !(CurrentVM is AreasCollectionViewModel))
                {
                    Mvx.Resolve<IPlatformNavigationService>().ShowWizardWebView(guideModel.GuideUrl);
                }
            }));
            _localNotificationService.RefreshNotifications();
            IsRefreshing = IsBusy = false;
        }
        #endregion

        public async virtual Task InvokeToolbarDoneCommand()
        {

        }

        public async virtual Task ItemActionInvoke(object param)
        {

        }

        public async virtual Task BackCommandInvoke()
        {
            if (CurrentVM is INetworkViewModel && NavigationHelper.ShowNetworksPageFromMainVM)
            {
                return;
            }
            if (this is IBaseChildViewModel)
            {
                await _navigationService.Close(this);
                if (ChildStackViewModelCount == 0 && LastTabVM != null)
                {
                    await LastTabVM.Refresh();
                }
                //CurrentVM = LastTabVM;
                return;
            }
            if (CurrentVM == null)
            {
                return;
            }
            await _navigationService.Close(CurrentVM);
            CurrentVM = LastTabVM;
        }

        private void ChangeChildStackVMCount(bool needAdded = false, bool needRefreshCount = false)
        {
            if (needRefreshCount && this is IBaseChildViewModel)
            {
                ChildStackViewModelCount = 0;
                return;
            }
            if (needAdded && this is IBaseChildViewModel)
            {
                ChildStackViewModelCount += 1;
            }
            if (!needAdded && this is IBaseChildViewModel)
            {
                ChildStackViewModelCount -= 1;
            }
        }

        public void SetCurrentVM(IMvxViewModel vm)
        {
            if (vm != null)
            {
                CurrentVM = LastTabVM = vm as BaseViewModel;
                ChangeChildStackVMCount(false, true);
            }
        }

        public BaseViewModel GetCurrentVM()
        {
            return CurrentVM;
        }

        //public void SetLastVMAsCurrent()
        //{
        //    if (LastTabVM != null)
        //    {
        //        CurrentVM = LastTabVM;
        //    }
        //}
        #endregion
    }
}
