using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using Plugin.Connectivity;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Intro
{
    public class IntroContainerViewModel : BaseViewModel
    {
        public bool IsAuthorized
        {
            get { return SL.HasNetworks; }
        }

        public bool HasAreas
        {
            get { return SL.HasAreas; }
        }

        public IntroContainerViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService) : base(navigationService, alertService, assetService, localNotificationService)
        {
            NavigationHelper.ShowWebViewFromIntroVM = true;
        }

        public override void Start()
        {
            base.Start();
            CrossConnectivity.Current.ConnectivityChanged += (s, e) =>
            {
                if (!e.IsConnected)
                {
                    _alertService.ShowToast("Internet connection is lost");
                }
                if (e.IsConnected)
                {
                    _alertService.ShowToast("Internet connection restored");
                }
            };
        }

        #region Commands
        public MvxAsyncCommand NavigateToMainView
        {
            get
            {
                return new MvxAsyncCommand(async() =>
                {
                    if (IsAuthorized && HasAreas)
                    {
                        await _navigationService.Navigate<MainViewModel>();
                        await _navigationService.Close(this);
                    }
                    else
                    {
                        _alertService.ShowToast("You have no areas");
                    }
                });
            }
        }
        #endregion


    }
}

