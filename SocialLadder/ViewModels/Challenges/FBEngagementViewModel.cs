using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class FBEngagementViewModel : BaseChallengesViewModel
    {
        #region Fields
        private readonly IFacebookShareService _facebookService;
        #endregion
        public FBEngagementViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IFacebookShareService facebookService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _facebookService = facebookService;
            IsBusy = true;
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            await base.ChallengeSubmit();
            _facebookService.VerifyPermissions(null, (ChallengesFacebookShareResponseType result) => { Success(result); }, param);
        }

        private async void Success(ChallengesFacebookShareResponseType result)
        {
            SubmitButtonAnimated = true;
            if (result != ChallengesFacebookShareResponseType.Successed)
            {
                if (result == ChallengesFacebookShareResponseType.Error)
                {
                    _alertService.ShowToast("Facebook engagement error.");
                }
                if (result == ChallengesFacebookShareResponseType.Canceled)
                {
                    _alertService.ShowToast("Facebook engagement was canceled.");
                }
                if (result == ChallengesFacebookShareResponseType.NativeUninstallApp)
                {
                    _alertService.ShowToast("Facebook permissions failed.");
                }
                SubmitButtonAnimated = false;
                return;
            }
            await SL.Manager.PostSubmitEngagement(Challenge.ID, (ShareResponseModel response) =>
            {
                SubmitChallengeComplete(true, response);
                SubmitButtonAnimated = false;
            });
        }
    }
}
