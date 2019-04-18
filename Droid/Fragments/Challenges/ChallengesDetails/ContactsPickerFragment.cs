using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Adapters.Challenges;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Challenges.ChallengesDetails;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges.ChallengesDetails
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class ContactsPickerFragment : BaseFragment<ContactsPickerViewModel>
    {
        protected override int FragmentId => Resource.Layout.ContactsPickerLayout;
        protected override bool HasBackButton => true;
        private InviteService inviteService;
        private ContactsListAdapter contactsAdapter;
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            var contactList = view.FindViewById<MvxListView>(Resource.Id.contacts_list);
            
            contactsAdapter = new ContactsListAdapter(Activity, (IMvxAndroidBindingContext)BindingContext);
            contactList.Adapter = contactsAdapter;
            UndateControls();
            inviteService = new InviteService(this);
            return view;
        }

        public async override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.NeedGetContacts))
            {
                if (ViewModel.NeedGetContacts)
                {
                    var contacts = await inviteService.GetContacts();
                    ViewModel.SetContactsCollectionFromPlatform(contacts);
                    //contactsAdapter.ItemsSource = ViewModel.Contacts;
                    ViewModel.NeedGetContacts = false;
                }
            }
        }

        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.back_text), FontsConstants.PN_B, (float)0.048);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.done_text), FontsConstants.PN_B, (float)0.048);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.count_text), FontsConstants.PN_R, (float)0.04);
            //FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.input_text), FontsConstants.PN_R, (float)0.04);
            //FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.link_lable), FontsConstants.PN_R, (float)0.05);
        }


    }
}