using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums.Constants;
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
    public class DocSubmitViewModel : BaseChallengesViewModel
    {
        private readonly IClipboardService _clipboardService;
        #region Propertyes
        private bool _showCopyToast;
        public bool ShowCopyToast
        {
            get => _showCopyToast;
            set => SetProperty(ref _showCopyToast, value);
        }
        #endregion

        public DocSubmitViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IClipboardService clipboardService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _clipboardService = clipboardService;
            IsBusy = true;
            Init();
        }

        private async Task Init()
        {
            TopMarginToCompleteView = false;
            ShowCopyToast = false;
        }

        #region Cpommands      
        public MvxAsyncCommand CopyLinkCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    ShowCopyToast = true;
                    await Task.Delay(2000);
                    _clipboardService.SaveToClipboard(Challenge.InstaCaption);
                    ShowCopyToast = false;
                });
            }
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            SubmitButtonAnimated = true;
            Challenge.Status = ChallengesConstants.ChallengeStatusPending;
            await SL.Manager.PostSubmitDocContent(Challenge.ID, Challenge.DocPathURL, null, PopToChallengeList);
        }

        void PopToChallengeList(ChallengeResponseModel challengeResponse)
        {
            SubmitChallengeComplete(true, challengeResponse);
            SubmitButtonAnimated = false;
        }

        #endregion
        #region Methods

        public async override Task RefreshOther()
        {
            await base.RefreshOther();
            await Init();
        }

        #endregion
    }
}
