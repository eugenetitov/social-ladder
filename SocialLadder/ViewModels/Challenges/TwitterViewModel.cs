using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges.ChallengesDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class TwitterViewModel : BaseChallengesViewModel
    {
        #region Propertyes

        #endregion

        public TwitterViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger = null) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            SubmitButtonImage = _assetService.ChallengesShareButton;
            IsBusy = true;
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            //await base.ChallengeSubmit();
            await _navigationService.Navigate<TwitterDetailsViewModel>();
            _messenger.Publish(new MessangerChallengeModel(this, Challenge));
        }
    }
}
