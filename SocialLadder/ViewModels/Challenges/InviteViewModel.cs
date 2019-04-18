using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Models;
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
    public class InviteViewModel : BaseChallengesViewModel, IChallengeInviteService
    {
        private readonly ISmsService _smsService;
        #region
        private ShareTemplateModel ShareTemplateModel { get; set; }

        public string GetInviteText => $"{ShareTemplateModel.InviteText} {ShareTemplateModel.ActionLink}";

        private bool _needShowSendMenu;
        public bool NeedShowSendMenu
        {
            get => _needShowSendMenu;
            set => SetProperty(ref _needShowSendMenu, value);
        }
        #endregion
        public InviteViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, ISmsService smsService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _smsService = smsService;
            SubmitButtonImage = _assetService.ChallengesInviteButton;
            TopMarginToCompleteView = true;
            IsBusy = true;
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            await base.ChallengeSubmit();
            SubmitButtonAnimated = true;
            await SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, SendInviteMessage);
        }

        private void SendInviteMessage(ShareResponseModel model)
        {
            if (model?.ShareTemplate == null)
            {
                return;
            }
            ShareTemplateModel = model.ShareTemplate;
            NeedShowSendMenu = true;
            SubmitButtonAnimated = false;
        }

        public string GetSubmitButtonImageNormalName()
        {
            return _assetService.ChallengesInviteButton;
        }

        public async Task UseSmsToSendInviteContact(List<string> numbers)
        {
            //_smsService.SendSmsToNumbers(numbers, $"{ShareTemplateModel.InviteText} {ShareTemplateModel.ActionLink}");
            await _navigationService.Navigate<ContactsPickerViewModel>();
            _messenger.Publish(new MessangerChallengeModel(this, Challenge));
        }

        public async Task UseWatsAppToSendInviteContact(string contact)
        {
            _smsService.SendSmsToWatsApp(string.Empty, $"{ShareTemplateModel.InviteText} {ShareTemplateModel.ActionLink}");
        }

        public void ShowToastIfPermissionsDenided()
        {
            _alertService.ShowToast("You don't have enough permissions");
        }

        public void CancelInviteAction()
        {
            _alertService.ShowToast("Contact action canceled");
        }
    }
}
