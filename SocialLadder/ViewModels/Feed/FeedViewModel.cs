using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Feed
{
    public class FeedViewModel : BaseFeedViewModel, IMainTabViewModel
    {
        public FeedViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            LastTabVM = this;
        }
    }
}
