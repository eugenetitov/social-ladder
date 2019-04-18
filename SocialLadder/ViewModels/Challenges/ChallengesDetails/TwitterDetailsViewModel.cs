using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using SocialLadder.Logger;
using SocialLadder.Models;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges.ChallengesDetails
{
    public class TwitterDetailsViewModel : BaseChallengesViewModel
    {
        #region Fields
        private readonly int _maxSymbolsCount = 280;
        #endregion
        #region Properties
        private ShareTemplateModel _shareTemplate;
        public ShareTemplateModel ShareTemplate
        {
            get => _shareTemplate;
            set => SetProperty(ref _shareTemplate, value);
        }

        private int _symbolsCount;
        public int SymbolsCount
        {
            get => _symbolsCount;
            set => SetProperty(ref _symbolsCount, value);
        }

        private string _enteredText;
        public string EnteredText
        {
            get
            {
                return SymbolsCount > 0 ? _enteredText : _enteredText.Substring(0, _maxSymbolsCount);
            }
            set
            {
                string newValue = value;
                var remainderSymbolsCount = _maxSymbolsCount - value.Length;
                SymbolsCount = remainderSymbolsCount >= 0 ? remainderSymbolsCount : 0;
                if (remainderSymbolsCount <= 0)
                {
                    newValue = value.Substring(0, _maxSymbolsCount);
                }
                SetProperty(ref _enteredText, value);
            } 
        }

        private string _linkText;
        public string LinkText
        {
            get => _linkText;
            set => SetProperty(ref _linkText, value);
        }
        #endregion
        public TwitterDetailsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger = null) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            SymbolsCount = _maxSymbolsCount;
            SubmitButtonImage = _assetService.ChallengesShareButton;
        }

        public override void SetChallengeModel(ChallengeResponseModel response)
        {
            base.SetChallengeModel(response);
            SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, ShareTemplateRefreshed);
        }

        public void ShareTemplateRefreshed(ShareResponseModel shareResponse)
        {
            IsBusy = true;
            if (shareResponse != null && shareResponse.ShareTemplate != null)
            {
                ShareTemplate = shareResponse.ShareTemplate;
                LinkText = (ShareTemplate.ActionLink == null) ? "(No Link Provided)" : ShareTemplate.ActionLink;
                EnteredText = ShareTemplate.PostTitle;
                IsBusy = false;
                return;
            }
            var response = _alertService.ShowOkCancelMessage(string.Empty, ChallengesConstants.ChallengesTwitterDetailsMessageText, () => _navigationService.Close(this), null, false);
            IsBusy = false;
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            await base.ChallengeSubmit(param);
            IsBusy = false;
            if (ShareTemplate != null)
            {
                SubmitButtonImage = _assetService.NetworkConnectingLoaderImage;
                LogHelper.LogChallengeSubmition(Challenge.ID.ToString(), Challenge.Name);

                ShareModel share = new ShareModel();
                share.ShareTransactionId = ShareTemplate.ShareTransactionID;
                share.BodyMessage = LinkText;
                share.ShareEntryList = System.Linq.Enumerable.ToList(ShareTemplate.NetworkShareList); ;//new List<ShareNetworkStatus>() { new ShareNetworkStatus { NetworkName = "Twitter", AllowSharing = true } }
                await SL.Manager.PostShare(share, (ShareResponseModel response) =>
                {
                    if (response.ResponseCode > 0 && String.IsNullOrEmpty(response.ResponseMessage))
                    {
                        response.ResponseMessage = "Congratulations!\r\nChallenge Completed!";
                    }
                    SubmitChallengeComplete(true, response);
                });
                //await Task.Delay(3000);
                SubmitButtonImage = _assetService.ChallengesShareButton;
            }
        }

        public string GetSubmitButtonImageNormalName()
        {
            return _assetService.ChallengesShareButton;
        }

        //public void SetRefreshImage()
        //{
        //    SubmitButtonImage = _assetService.NetworkConnectingLoaderImage;
        //}
    }
}
