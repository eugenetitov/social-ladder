using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Adapters.More;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Fragments.Main;
using SocialLadder.Droid.Helpers;
using SocialLadder.ViewModels.Main;
using SocialLadder.ViewModels.More;

namespace SocialLadder.Droid.More
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class SettingsFragment : BaseFragment<SettingsViewModel>, AdapterView.IOnItemSelectedListener, IDialogInterfaceOnClickListener
    {
        protected override int FragmentId => Resource.Layout.SettingsLayout;
        protected override bool HasBackButton => true;

        private bool _isFirstEnter = true;

        private TextView profileText, accSettingsText, infoText, notificationText, locationText, notificationWithSwitchText, versionText;
        private TextView displayNameText, cityText, emailText;
        private EditText DisplayName, Email;
        private Spinner City;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            profileText = view.FindViewById<TextView>(Resource.Id.Profile);
            accSettingsText = view.FindViewById<TextView>(Resource.Id.AccountSettings);
            infoText = view.FindViewById<TextView>(Resource.Id.Info);
            notificationText = view.FindViewById<TextView>(Resource.Id.Notifications);
            locationText = view.FindViewById<TextView>(Resource.Id.Location);
            notificationWithSwitchText = view.FindViewById<TextView>(Resource.Id.txtNotification);
            versionText = view.FindViewById<TextView>(Resource.Id.txtVersion);

            displayNameText = view.FindViewById<TextView>(Resource.Id.txtDisplayName);
            cityText = view.FindViewById<TextView>(Resource.Id.txtCity);
            emailText = view.FindViewById<TextView>(Resource.Id.txtEmail);

            DisplayName = view.FindViewById<EditText>(Resource.Id.txbDisplayName);
            City = view.FindViewById<Spinner>(Resource.Id.txbCity);
            Email = view.FindViewById<EditText>(Resource.Id.txbEmail);

            DisplayName.KeyPress += DisplayNameKeyOkCloseKeyboard;

            var Switch = view.FindViewById<Switch>(Resource.Id.Switch);
            Switch.Click += (sender, e) =>
            {
                if(Switch.Checked)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(Context);
                    builder.SetMessage("To enable notifications, please navigate to Settings > Notifications > SocialLadder, and make sure Allow Notifications is set to ON.");
                    builder.SetCancelable(false);
                    builder.SetNegativeButton("Ok", this);
                    AlertDialog alert = builder.Create();
                    alert.Show();
                }
                ViewModel.OnNotifEnabledChangedCommand.Execute();
            };

            try
            {
                PackageInfo pInfo = Context.PackageManager.GetPackageInfo(Context.PackageName, 0);
                versionText.Text = $"v{pInfo.VersionName}";
            }
            catch (PackageManager.NameNotFoundException e)
            {
                e.PrintStackTrace();
            }

            UpdateFonts();

            return view;
        }
        

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ArrayAdapter<String> adapter = new ArrayAdapter<String>(Activity, Resource.Layout.support_simple_spinner_dropdown_item, ViewModel.CityList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleDropDownItem1Line);
            SettingsAdapter adapter1 = new SettingsAdapter(Context, ViewModel.CityList);
            City.Adapter = adapter1;
            City.OnItemSelectedListener = this;
            City.SetSelection(ViewModel.CityList.IndexOf(ViewModel.City));
        }


        public override void OnResume()
        {
            base.OnResume();

            
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();

            ViewModel.OnSaveProfile.Execute();
            DisplayName.KeyPress -= DisplayNameKeyOkCloseKeyboard;
        }

        private void UpdateFonts()
        {
            FontHelper.UpdateFont(profileText, FontsConstants.PN_B, (float)0.037);
            FontHelper.UpdateFont(accSettingsText, FontsConstants.PN_B, (float)0.037);
            FontHelper.UpdateFont(locationText, FontsConstants.PN_B, (float)0.037);
            FontHelper.UpdateFont(notificationWithSwitchText, FontsConstants.PN_B, (float)0.039);
            FontHelper.UpdateFont(DisplayName, FontsConstants.PN_B, (float)0.039);
            FontHelper.UpdateFont(Email, FontsConstants.PN_B, (float)0.039);

            FontHelper.UpdateFont(displayNameText, FontsConstants.PN_R, (float)0.033);
            FontHelper.UpdateFont(cityText, FontsConstants.PN_R, (float)0.033);
            FontHelper.UpdateFont(emailText, FontsConstants.PN_R, (float)0.033);

            FontHelper.UpdateFont(infoText, FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(notificationText, FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(versionText, FontsConstants.PN_R, (float)0.037);
        }

        public void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            if (_isFirstEnter)
            {
                _isFirstEnter = false;
                return;
            }
            ViewModel.City = ViewModel.CityList[position];
        }

        public void OnNothingSelected(AdapterView parent)
        {
            
        }

        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Cancel();
        }

        private void DisplayNameKeyOkCloseKeyboard(object sender, View.KeyEventArgs e)
        {
            e.Handled = false;
            if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
            {
                e.Handled = true;
            }
        }
    }
}