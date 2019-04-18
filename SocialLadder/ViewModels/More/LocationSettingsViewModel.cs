using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.More
{
    public class LocationSettingsViewModel : BaseViewModel, IBaseChildViewModel
    {
        public LocationSettingsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            CurrentVM = this;
        }
    }
}
