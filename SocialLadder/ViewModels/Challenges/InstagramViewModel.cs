using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums.Constants;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class InstagramViewModel : BaseChallengesViewModel, IChallengeShareViewModel
    {
        private readonly IShareService _shareService;
        private readonly IClipboardService _clipboardService;
        #region Propertyes
        private bool _showCopyToast;
        public bool ShowCopyToast
        {
            get => _showCopyToast;
            set => SetProperty(ref _showCopyToast, value);
        }
        #endregion

        public InstagramViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IShareService shareService, IClipboardService clipboardService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _shareService = shareService;
            _clipboardService = clipboardService;
            NavigationHelper.NeedSubmitInstagramChallenge = false;
            Init();
        }

        private async Task Init()
        {
            TopMarginToCompleteView = false;
            ShowCopyToast = false;
            IsBusy = true;
        }

        #region Cpommands      
        public MvxAsyncCommand CopyHashCommand
        {
            get
            {
                return new MvxAsyncCommand(async() =>
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
            await SL.Manager.UpdateChallenge(Challenge, Challenge.ID, (bool response) =>
            {
                SubmitChallengeComplete(response);
                _shareService.CleanFile();
                SubmitButtonAnimated = false;
            });
        }

        #endregion
        #region Methods
        public async override Task RefreshOther()
        {
            await base.RefreshOther();
            await Init();
        }

        #endregion
        #region LibraryMethods
        public bool CheckNetworkConnected()
        {
            if (SL.IsNetworkConnected("Instagram"))
            {
                return true;
            }
            //Insta Share
            return false;
        }

        public void AddPhotoFromCameraOrLibrary(byte[] image)
        {
            if (image != null)
            {
                ChallengeImage = image;
                ChallengesPhotoHelper.ChallengeImage = image;
            }
            else
            {
                _alertService.ShowToast("Unable to add image");
            }
        }

        public async Task ShareToSocialNetwork()
        {
            IsBusy = true;
            await _shareService.SharePhotoToInsta("Instagram share", "Share this challenge to instagram", Challenge.ShareImage, ChallengeImage);
            IsBusy = false;
        }

        public void ShowToastIfPermissionsDenided()
        {
            _alertService.ShowToast("You don't have enough permissions");
        }
        #endregion
    }
}
