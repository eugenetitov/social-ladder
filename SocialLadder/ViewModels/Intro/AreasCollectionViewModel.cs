using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Mappers;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Intro
{
    public class AreasCollectionViewModel : BaseViewModel, IBaseChildViewModel
    {
        #region Properties

        public string PredefinedCode { get; set; } = string.Empty;
        private bool _detailsViewHidden;
        private bool _newAreaAdded { get; set; }
        public bool DetailsViewHidden
        {
            get => _detailsViewHidden;
            set => SetProperty(ref _detailsViewHidden, value);
        }

        private bool _areasToolbarHidden;
        public bool AreasToolbarHidden
        {
            get => _areasToolbarHidden;
            set => SetProperty(ref _areasToolbarHidden, value);
        }

        private bool _areasSubscribedTextHidden;
        public bool AreasSubscribedTextHidden
        {
            get => _areasSubscribedTextHidden;
            set => SetProperty(ref _areasSubscribedTextHidden, value);
        }

        private bool _areasToolbarBackShowed;
        public bool AreasToolbarBackShowed
        {
            get => _areasToolbarBackShowed;
            set => SetProperty(ref _areasToolbarBackShowed, value);
        }

        private MvxObservableCollection<LocalAreaModel> _areas;
        public MvxObservableCollection<LocalAreaModel> Areas
        {
            get => _areas;
            set => SetProperty(ref _areas, value);
        }

        private LocalAreaModel _selectedArea;
        public LocalAreaModel SelectedArea
        {
            get => _selectedArea;
            set
            {
                var item = value;
                if (item == null)
                {
                    return;
                }               
                item.areaDescription = !string.IsNullOrEmpty(value.areaDescription) ? value.areaDescription : "This is " + value.areaName + " area";
                AreasSubscribedTextHidden = SL.Profile.AreaSubsList != null && SL.Profile.AreaSubsList.Where(x => x.areaID == item.areaID).FirstOrDefault() != null ? false : true;
                SetProperty(ref _selectedArea, item);
            }
        }
        #endregion
        public AreasCollectionViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService) : base(navigationService, alertService, assetService, localNotificationService)
        {
            Areas = new MvxObservableCollection<LocalAreaModel>();
            DetailsViewHidden = false;
            // set PredefinedCode from invite code
            AreasToolbarHidden = NavigationHelper.ShowAreasCollectionPageFromMainVM ? false : true;
            ToolbarDoneButtonHidden = NavigationHelper.ShowAreasCollectionPageAfterLogout ? false : true;
            ToolbarBackButtonHidden = NavigationHelper.ShowAreasCollectionNeedShowBackButton ? false : true;
            AreasToolbarBackShowed = (NavigationHelper.ShowAreasCollectionPageFromMainVM && !NavigationHelper.ShowAreasCollectionPageAfterLogout) ? true : false;
            AreasSubscribedTextHidden = true;
            _newAreaAdded = false;
        }

        public async override Task Initialize()
        {
            await Refresh();
            //if (NavigationHelper.ShowAreasCollectionPage)
            //{
            //    InitAreas();
            //}
            await InitAreas();
            ScoreCount = Profile.Score.ToString();
            await base.Initialize();
        }

        public override Task RefreshOther()
        {
            if (SL.HasAreas)
                SL.RefreshAll();
            return base.RefreshOther();
        }

        #region Commands
        public MvxAsyncCommand AddInviteCodeCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    var areaCode = await _alertService.ShowAlertWithInput(string.Empty, "Add Invite Code");
                    if (!string.IsNullOrEmpty(areaCode))
                    {
                        await ProcessAreaInviteCode(areaCode);
                    }
                    else
                    {
                        _alertService.ShowSingle("Didn't get your code. Make sure to enter a code.");
                    }
                });
            }
        }

        public MvxAsyncCommand RefreshCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    IsRefreshing = true;
                    //Refresh logic will be there
                    await InitAreas();
                    IsRefreshing = false;
                });
            }
        }

        public MvxCommand CloseDetailsViewCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    DetailsViewHidden = false;
                    AreasSubscribedTextHidden = true;
                });
            }
        }

        public MvxCommand<LocalAreaModel> AreaClickedCommand
        {
            get
            {
                return new MvxCommand<LocalAreaModel>((param) =>
                {
                    if (param == null)
                    {
                        DetailsViewHidden = false;
                        AreasSubscribedTextHidden = true;
                        return;
                    }
                    SelectedArea = param;
                    DetailsViewHidden = true;
                });
            }
        }

        public MvxAsyncCommand EnterInviteCodeCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    if (SelectedArea == null)
                    {
                        return;
                    }
                    DetailsViewHidden = false;
                    if (!string.IsNullOrEmpty(SelectedArea.JoinCode))
                    {
                        await ProcessAreaInviteCode(SelectedArea.JoinCode);
                    }
                    else
                    {
                        string code = string.Empty;
                        code = await _alertService.ShowAlertWithInput("Join", "Enter your code to join", PredefinedCode, "Ok", "Cancel");
                        if (!string.IsNullOrEmpty(code))
                        {
                            await ProcessAreaInviteCode(code);
                        }
                        else
                        {
                            _alertService.ShowSingle("Didn't get your code. Make sure to enter a code.");
                        }
                    }
                });
            }
        }
        #endregion
        #region Methods
        //public void InitAreas()
        //{
        //    Areas.Clear();
        //    var areas = SL.GetAreas();
        //    var suggestedAreas = SL.GetSuggestedAreas();
        //    if ((areas == null || areas.Count == 0) && (suggestedAreas == null || suggestedAreas.Count == 0))
        //    {
        //        _alertService.ShowToast("Areas not found");
        //        return;
        //    }
        //    foreach (var area in areas)
        //    {
        //        var item = AreasViewMapper.ItemToLocalItem(area, this, true);
        //        Areas.Add(item);
        //    }
        //    foreach (var area in suggestedAreas)
        //    {
        //        var item = AreasViewMapper.ItemToLocalItem(area, this, true, true);
        //        Areas.Add(item);
        //    }
        //    if (areas != null && SL.HasAreas)
        //    {
        //        SL.Profile.SetDefaultAreaIfNeeded();
        //    }
        //    ToolbarDoneButtonHidden = true;
        //}

        public async Task InitAreas()
        {
            Areas.Clear();
            var areas = SL.GetAreas();
            var suggestedAreas = SL.GetSuggestedAreas();
            if ((areas == null || areas.Count == 0) && (suggestedAreas == null || suggestedAreas.Count == 0))
            {
                _alertService.ShowToast("Areas not found");
                return;
            }
            if (areas != null && areas.Count != 0)
            {
                foreach (var area in areas)
                {
                    var item = AreasViewMapper.ItemToLocalItem(area, this, true);
                    Areas.Add(item);
                }
            }
            if (suggestedAreas != null && suggestedAreas.Count != 0)
            {
                foreach (var area in suggestedAreas)
                {
                    var item = AreasViewMapper.ItemToLocalItem(area, this, true, true);
                    Areas.Add(item);
                }
            }
            if (areas != null && SL.HasAreas)
            {
                SL.Profile.SetDefaultAreaIfNeeded();
            }
            ToolbarDoneButtonHidden = true;
            if ((areas == null || areas.Count == 0) && (suggestedAreas != null || suggestedAreas.Count != 0))
            {
                var result = await _alertService.ShowAlertConfirmation("Were you provided with a unique invite code?", "Yes", "Nope");
                if (result)
                {
                    AddInviteCodeCommand.Execute();
                }
            }
        }

        private async Task ProcessAreaInviteCode(string code)
        {
            IsBusy = true;
            ActionResponseModel action = await SL.Manager.AreaInviteAsync(code);
            if (action.ResponseCode > 0)
            {
                await SL.Manager.GetProfileAsync();
                _newAreaAdded = true;
            }
            await InitAreas();
            DetailsViewHidden = false;
            _alertService.ShowMessage("Join", string.IsNullOrEmpty(action.ResponseMessage) ? "Connection error occurred" : action.ResponseMessage);
            IsBusy = false;
        }

        public async override Task InvokeToolbarDoneCommand()
        {
            await base.InvokeToolbarDoneCommand();
            NavigationHelper.AreasCollectionPageAfterLogoutDoneClicked = NavigationHelper.ShowAreasCollectionPageAfterLogout = true;
            if (!SL.HasAreas)
            {
                var result = await _alertService.ShowAlertConfirmation("There are no areas. Do you want to login to another account?");
                if (result)
                {
                    IsBusy = true;
                    NavigationHelper.ShowNetworksPageFromMainVM = true;
                    NavigationHelper.AreasCollectionPageAfterLogoutDoneClicked = false;
                    SL.Logout();
                    var _locatioService = Mvx.Resolve<ILocationService>();
                    var location = _locatioService.CurrentLocation == null ? await _locatioService.GetCurrentLocation() : _locatioService.CurrentLocation;
                    await SL.Manager.GetCityListWithLatitude(location.Lat, location.Long);
                    IsBusy = false;
                    await _navigationService.Close(this);
                    await _navigationService.Navigate<NetworksViewModel>();
                }
                return;
            }
            await BackCommandInvoke();
        }

        public async override Task BackCommandInvoke()
        {
            if (!NavigationHelper.ShowAreasCollectionNeedShowBackButton)
            {
                return;
            }

            Mvx.Resolve<IPlatformNavigationService>().NavigateToMainWithSwitchArea(null);
            //await _navigationService.Close(this);
            await base.BackCommandInvoke();
        }

        public async override Task ItemActionInvoke(object param)
        {
            await base.ItemActionInvoke(param);
            var areaId = param as string;
            var area = Areas.Where(x => x.areaID.ToString() == areaId).FirstOrDefault();
            var _platformNavigationService = Mvx.Resolve<IPlatformNavigationService>();
            if (area == null || _platformNavigationService == null || AreasToolbarHidden || area.IsSuggestedArea)
            {
                AreaClickedCommand.Execute(area);
                return;
            }
            _platformNavigationService.NavigateToMainWithSwitchArea(area, _newAreaAdded);
            await _navigationService.Close(this);
        }
        #endregion
    }
}
