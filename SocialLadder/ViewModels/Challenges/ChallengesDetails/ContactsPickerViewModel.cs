using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges.ChallengesDetails
{
    public class ContactsPickerViewModel : BaseChallengesViewModel
    {
        private readonly ISmsService _smsService;
        private readonly int _maxselectedContacts = 20;

        #region Properties
        public MvxObservableCollection<LocalContactModel> InitializContacts { get; set; }
        private int SelectedContactsCount { get; set; } = 0;

        private MvxObservableCollection<LocalContactModel> _contacts;
        public MvxObservableCollection<LocalContactModel> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }

        private string _pickerCountText;
        public string PickerCountText
        {
            get => _pickerCountText;
            set => SetProperty(ref _pickerCountText, value);
        }

        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            set
            {
                SetProperty(ref _searchString, value);
                SearchContact();
            }
        }

        private bool _needGetContacts;
        public bool NeedGetContacts
        {
            get => _needGetContacts;
            set => SetProperty(ref _needGetContacts, value);
        }
        private ShareTemplateModel ShareTemplateModel { get; set; }
        #endregion

        public ContactsPickerViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, ISmsService smsService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _smsService = smsService;
            ToolbarDoneButtonHidden = false;
            NeedGetContacts = false;
            Contacts = new MvxObservableCollection<LocalContactModel>();
            UpdatePickerCountText();
            _token = _messenger.Subscribe<MessengerActionHandlerModel>(OnLocationActionHandlerMessage);
        }

        #region ActionHandler
        private void OnLocationActionHandlerMessage(MessengerActionHandlerModel locationMessage)
        {
            if (locationMessage != null)
            {
                ShareTemplateModel = new ShareTemplateModel { ActionLink = locationMessage.Model.ShareTemplate.InviteText, InviteText = locationMessage.Model.ShareTemplate.ActionLink };
                NeedGetContacts = true;
            }
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            _messenger.Unsubscribe<MessengerActionHandlerModel>(_token);
        }
        #endregion

        public async override void SetChallengeModel(ChallengeResponseModel response)
        {
            IsBusy = true;
            base.SetChallengeModel(response);
            await SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, SendInviteMessage);
            IsBusy = false;
        }

        private void SendInviteMessage(ShareResponseModel model)
        {
            if (model?.ShareTemplate == null)
            {
                return;
            }
            ShareTemplateModel = model.ShareTemplate;
            NeedGetContacts = true;          
        }

        public async void SetContactsCollectionFromPlatform(List<LocalContactModel> contacts)
        {
            IsBusy = true;
            if (contacts == null)
            {
                var result = await _alertService.ShowAlertConfirmation("You don't have enough permissions. Please add contact permissions in application settings.", "Ok", string.Empty);
                if (result)
                {
                    await BackCommandInvoke();
                    return;
                }
            }
            var sortContacts = contacts.OrderBy(o => o.Name).ToList();
            if (Contacts == null)
            {
                Contacts = new MvxObservableCollection<LocalContactModel>();
            }
            foreach (var item in sortContacts)
            {
                item.IsSelected = false;
                Contacts.Add(item);
            }
            InitializContacts = Contacts;
            IsBusy = false;
        }

        public async override Task InvokeToolbarDoneCommand()
        {
            await base.InvokeToolbarDoneCommand();
            var numbersList = Contacts.Where(x => x.IsSelected).Select(l => l.Number).ToList();
            _smsService.SendSmsToNumbers(numbersList, $"{ShareTemplateModel.InviteText} {ShareTemplateModel.ActionLink}");
            await _navigationService.Close(this);
        }

        private void SearchContact()
        {
            IsBusy = true;
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                Contacts = InitializContacts;
            }
            else
            {
                var findResult = InitializContacts.Where(i => i.Name.ToLower().Contains(SearchString.ToLower()));
                Contacts = new MvxObservableCollection<LocalContactModel>(findResult.Cast<LocalContactModel>());
            }
            IsBusy = false;
        }

        #region Commands
        public MvxAsyncCommand<LocalContactModel> ContactSelectedCommand
        {
            get
            {
                return new MvxAsyncCommand<LocalContactModel>(async (item) =>
                {
                    IsBusy = true;
                    if (SelectedContactsCount > _maxselectedContacts)
                    {
                        _alertService.ShowToast("Maximum contacts allowed");
                    }
                    item.IsSelected = SelectedContactsCount < _maxselectedContacts ? !item.IsSelected : false;
                    var selectedItem = Contacts.Where(x => x.Number == item.Number).FirstOrDefault();
                    var selectedIndex = Contacts.IndexOf(selectedItem);
                    Contacts[selectedIndex] = item;
                    UpdatePickerCountText();
                    IsBusy = false;
                });
            }
        }

        private void UpdatePickerCountText()
        {
            SelectedContactsCount = Contacts.Where(x => x.IsSelected == true).ToList().Count();
            if (SelectedContactsCount == 0)
            {
                PickerCountText = $"Pick {SelectedContactsCount} people to invite";
            }
            if (SelectedContactsCount < _maxselectedContacts)
            {
                PickerCountText = $"{SelectedContactsCount} selected, pick {_maxselectedContacts - SelectedContactsCount} more";
            }
            if (SelectedContactsCount >= _maxselectedContacts)
            {
                PickerCountText = $"{SelectedContactsCount} selected - maximum allowed";
            }
        }
        #endregion
    }
}
