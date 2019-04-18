using MvvmCross.Core.Navigation;
using SocialLadder.Interfaces;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Intro
{
    public class Intro3ViewModel : BaseViewModel
    {
        public Intro3ViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService) : base(navigationService, alertService, assetService, localNotificationService)
        {

        }
    }
}
