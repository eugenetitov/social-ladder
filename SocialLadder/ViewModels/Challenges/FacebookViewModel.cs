using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;
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
    public class FacebookViewModel : BaseChallengesViewModel
    {
        private readonly IFacebookShareService _facebookService;
        #region Properties

        #endregion

        public FacebookViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IFacebookShareService facebookService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _facebookService = facebookService;
            SubmitButtonImage = _assetService.ChallengesFacebookButton;
            TopMarginToCompleteView = true;
            IsBusy = true;
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            await base.ChallengeSubmit();
            // await _facebookService.SendOpenGraph(param, Challenge, null, ChallengeImage, Success);
            await _facebookService.ShareFacebookChallenge(param, Challenge, null, ChallengeImage, Success);
        }

        //public string GetSubmitButtonImageNormalName()
        //{
        //    return _assetService.ChallengesFacebookButton;
        //}

        private async void Success(ChallengesFacebookShareResponseType result)
        {
            SubmitButtonAnimated = true;
            SubmitButtonImage = _assetService.NetworkConnectingLoaderImage;
            if (result != ChallengesFacebookShareResponseType.Successed)
            {
                if (result == ChallengesFacebookShareResponseType.Error)
                {
                    _alertService.ShowToast("Facebook share error. Try relogin to native facebook app.");
                }
                if (result == ChallengesFacebookShareResponseType.Canceled)
                {
                    _alertService.ShowToast("Facebook share was canceled.");
                }
                if (result == ChallengesFacebookShareResponseType.NativeUninstallApp)
                {
                    _alertService.ShowToast("Facebook share failed. Check native facebook app and try again.");
                }               
                SubmitButtonAnimated = false;
                SubmitButtonImage = _assetService.ChallengesFacebookButton;
                return;
            }
            await SL.Manager.SubmitAnswerAsync(Challenge.ID, null, null, (ChallengeResponseModel response) =>
            {
                SubmitChallengeComplete(true, response);
                SubmitButtonAnimated = false;
                SubmitButtonImage = _assetService.ChallengesFacebookButton;
            });
        }
    }
}
