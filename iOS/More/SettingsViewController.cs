using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Helpers;
using SocialLadder.iOS.Services;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using UIKit;
using UserNotifications;

namespace SocialLadder.iOS.More
{
    public partial class SettingsViewController : UIViewController
    {
        #region Fields & Properties
        private NSObject keyboardShowObj;
        private NSObject keyboardHideObj;

        List<string> ListOfCities { get; set; }
        string NameOfCity { get; set; }
        #endregion

        #region ctor
        public SettingsViewController(IntPtr handle) : base(handle)
        {

        }
        #endregion

        #region Lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            DisplayNameText.Text = StringWithEmojiConverter.ConvertEmojiFromServer(SL.Profile.UserName);//need to anderstand difference from NameText
            CityText.Text = SL.Profile.City ?? "";
            NameOfCity = SL.Profile.City ?? "";
            NameText.Text = SL.Profile.UserName;

            CityText.EditingChanged += (s, e) =>
            {
                CityText.Text = CheckText(CityText.Text);
            };

            EmailText.EditingDidEnd += (s, e) =>
            {
                var profile = SL.Profile;
                profile.EmailAddress = EmailText.Text;
                SL.Manager.SaveProfileAsync(profile);
                SL.Profile = profile;
            };

            NameText.ShouldReturn = delegate
            {
                NameText.EndEditing(true);
                return true;
            };

            DisplayNameText.ShouldReturn = delegate
            {
                DisplayNameText.EndEditing(true);
                return true;
            };

            CityText.ShouldReturn = delegate
            {
                CityText.EndEditing(true);
                return true;
            };

            EmailText.ShouldReturn = delegate
            {
                EmailText.EndEditing(true);
                return true;
            };

            var tap = new UITapGestureRecognizer(() =>
            {
                foreach (var view in View.Subviews[0].Subviews[0].Subviews)
                {
                    if (view is UITextView text)
                        text.ResignFirstResponder();
                    if (view is UITextField text1)
                        text1.ResignFirstResponder();
                }
            });
            View.AddGestureRecognizer(tap);

            EmailText.Text = SL.Profile.EmailAddress;
            
            var version = NSBundle.MainBundle.InfoDictionary["CFBundleVersion"];//NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
            VersionNumber.Text = $"v{version.Description}";

            #region v2
            //swFromElectricAdvinture.On = SL.AppSettings.GetValueOrDefault("FromElectricAdvintureEnabled", false);
            //swFromElectricAdvinture.ValueChanged += SwFromElectricAdvinture_ValueChanged;

            //Need remove to v.2
            Name_View.TranslatesAutoresizingMaskIntoConstraints = false;
            Name_Divider.Hidden = true;
            Name_View.RemoveConstraint(Name_View_Aspect);
            Name_View.AddConstraint(NSLayoutConstraint.Create(Name_View, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 0, 0));
            Name_View.UpdateConstraints();
            #endregion

            swComments.On = SL.AppSettings.GetValueOrDefault("CommentsEnabled", false);
            swComments.ValueChanged += SwComments_ValueChanged;

            swLikes.On = SL.AppSettings.GetValueOrDefault("LikesEnabled", false);
            swLikes.ValueChanged += SwLikes_ValueChanged;

            swDiscoverTeams.On = SL.AppSettings.GetValueOrDefault("DiscoverTeamsEnabled", false);
            swDiscoverTeams.ValueChanged += SwDiscoverTeams_ValueChanged;

            swHideSocialNetworks.On = SL.AppSettings.GetValueOrDefault("HideSocialNetworksEnabled", false);
            swHideSocialNetworks.ValueChanged += SwHideSocialNetworks_ValueChanged;

            #region v1
            var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
            swNotifications.On = notificationStatus.Equals(Enums.NotififcationStatus.Enabled.ToString()) ? true : false;
            swNotifications.ValueChanged += SwNotifications_ValueChanged;

            //SetupNotificationSwitch();

            ViewHideBlockDiscover.AddConstraint(NSLayoutConstraint.Create(ViewHideBlockDiscover, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 0));
            ViewHideBlockNotification.AddConstraint(NSLayoutConstraint.Create(ViewHideBlockNotification, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 0));
            View.UpdateConstraints();
            #endregion
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            nfloat fontMultiplier = SizeConstants.ScreenMultiplier;

            lblProfile.Font = UIFont.FromName("ProximaNova-Bold", 14 * fontMultiplier);
            first_label_account_settings_title.Font = UIFont.FromName("ProximaNova-Bold", 14 * fontMultiplier);
            lblNotifications.Font = UIFont.FromName("ProximaNova-Bold", 14 * fontMultiplier);
            lblPrivacy.Font = UIFont.FromName("ProximaNova-Bold", 14 * fontMultiplier);
            lblLocation.Font = UIFont.FromName("ProximaNova-Bold", 14 * fontMultiplier);

            lblCount.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);

            second_label_account_settings_title.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);
            VersionNumber.Font = UIFont.FromName("ProximaNova-Regular", 14 * fontMultiplier);

            lblDisplayName.Font = UIFont.FromName("ProximaNova-Bold", 12 * fontMultiplier);
            lblCity.Font = UIFont.FromName("ProximaNova-Bold", 12 * fontMultiplier);
            lblBio.Font = UIFont.FromName("ProximaNova-Bold", 12 * fontMultiplier);
            lblName.Font = UIFont.FromName("ProximaNova-Bold", 12 * fontMultiplier);
            lblEmail.Font = UIFont.FromName("ProximaNova-Bold", 12 * fontMultiplier);

            DisplayNameText.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);
            CityText.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);
            BioText.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);
            NameText.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);
            EmailText.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);
            lblNotifications.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);
            lblComments.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);
            lblLikes.Font = UIFont.FromName("ProximaNova-Bold", 16 * fontMultiplier);

            lblHideSocialNetworks.Font = UIFont.FromName("SFProText-Medium", 16 * fontMultiplier);
            BtnLocationSettings.Font = UIFont.FromName("SFProText-Medium", 16 * fontMultiplier);

            lblDiscoverTeams.Font = UIFont.FromName("SFProText-Medium", 18 * fontMultiplier);

            lblDiscoverDescription.Font = UIFont.FromName("SFProText-Regular", 13 * fontMultiplier);


            //DisplayNameText.TextContainerInset = new UIEdgeInsets(0, -lblDiscoverDescription.TextContainer.LineFragmentPadding, 0, -lblDiscoverDescription.TextContainer.LineFragmentPadding);
            //CityText.TextContainerInset = new UIEdgeInsets(0, -lblDiscoverDescription.TextContainer.LineFragmentPadding, 0, -lblDiscoverDescription.TextContainer.LineFragmentPadding);
            //BioText.TextContainerInset = new UIEdgeInsets(0, -lblDiscoverDescription.TextContainer.LineFragmentPadding, 0, -lblDiscoverDescription.TextContainer.LineFragmentPadding);
            //NameText.TextContainerInset = new UIEdgeInsets(0, -lblDiscoverDescription.TextContainer.LineFragmentPadding, 0, -lblDiscoverDescription.TextContainer.LineFragmentPadding);
            //EmailText.TextContainerInset = new UIEdgeInsets(0, -lblDiscoverDescription.TextContainer.LineFragmentPadding, 0, -lblDiscoverDescription.TextContainer.LineFragmentPadding);
            lblDiscoverDescription.TextContainerInset = new UIEdgeInsets(0, -lblDiscoverDescription.TextContainer.LineFragmentPadding, 0, -lblDiscoverDescription.TextContainer.LineFragmentPadding);

            //TableViewHeight.Constant = TableView.ContentSize.Height;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            LoadCityList();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            RegisterForKeyboardNotifications();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (CheckChanges())
            {
                SaveProfile();
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            RemoveForKeyboardNotifications();
        }
        #endregion

        #region Methods

        void SaveProfile()
        {
            SL.Profile.UserName = SocialLadder.iOS.Helpers.StringWithEmojiConverter.ConvertEmojiToServer(DisplayNameText.Text);
            SL.Profile.City = CityText.Text;
            SL.Profile.EmailAddress = EmailText.Text;
            SL.Profile.isNotificationEnabled = swNotifications.On;

            var newProfile = new ProfileUpdateModel()
            {
                UserName = StringWithEmojiConverter.ConvertEmojiToServer(DisplayNameText.Text),
                EmailAddress = EmailText.Text,
                isNotificationEnabled = swNotifications.On,
                isPhoneBookEnabled = false,
                isGeoEnabled = SL.Profile.isGeoEnabled,
                LocationLat = SL.Profile.LocationLat,
                LocationLon = SL.Profile.LocationLon,
                City = CityText.Text,
                AppVersion = SL.Profile.AppVersion
            };

            SL.Manager.UpdateProfileAsync(newProfile);
        }

        //based on topic https://stackoverflow.com/questions/40531103/swift-ios-check-if-remote-push-notifications-are-enabled-in-ios9-and-ios10
        //private async void SetupNotificationSwitch()
        //{
        //    var currentNoificationCenter = UNUserNotificationCenter.Current;
        //    var settings = await currentNoificationCenter.GetNotificationSettingsAsync();
        //    if (settings.AuthorizationStatus == UNAuthorizationStatus.Authorized)
        //    {
        //        swNotifications.On = true;
        //        return;
        //    }
        //    swNotifications.On = false;           
        //}


        private bool CheckChanges()
        {
            var profile = SL.Profile;
            if (!profile.UserName.Equals(DisplayNameText.Text))
            {
                return true;
            }
            if (!profile.EmailAddress.Equals(EmailText.Text))
            {
                return true;
            }
            if (!profile.City.Equals(CityText.Text))
            {
                return true;
            }
            if (profile.isNotificationEnabled != swNotifications.On)
            {
                return true;
            }
            return false;
        }

        string CheckText(string text)
        {
            if (!ListOfCities.Contains(text))
            {
                return NameOfCity;
            }
            return text;
        }

        private async void LoadCityList()
        {
            var CitiesModel = new CitiesModel(CityText, this);

            CitiesResponseModel cities = await SL.Manager.GetCityList();
            if (cities.result == null)
            {
                CitiesModel.Items = new List<string> { "Philadelphia", "New York City", "Chicago", "Washington D.C.", "London", "Jakarta", "Ibiza", "Las Vegas", "Amsterdam", "Miami", "Istanbul", "Rio de Janeiro", "Austin", "Bucharest", "Tallahassee", "Ft. Lauderdale", "Atlanta", "Sheffield", "Leeds", "Birmingham", "Manchester", "Antwerp", "Cochabamba", "Buenos Aires", "La Paz", "Santa Cruz", "Los Angeles", "Johannesburg", "Sydney", "Brighton", "Reading", "Charleston", "Buffalo", "Portland", "Raleigh" };
            }

            if (cities.result != null)
            {
                CitiesModel.Items = cities.result;
                CitiesModel.Items.Sort();
            }

            ListOfCities = CitiesModel.Items;

            var PickerView = new UIPickerView { DataSource = CitiesModel, Delegate = CitiesModel };
            CityText.InputView = PickerView;
        }

        void LoadLocationSettings()
        {
            UIStoryboard board = UIStoryboard.FromName("More", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("LocationViewController");
            NavigationController.PushViewController(ctrl, true);
        }

        public class CitiesModel : UIPickerViewModel
        {
            public List<string> Items { get; set; }
            public SettingsViewController ViewController { get; set; }

            private UITextField cityTextView;

            public CitiesModel(UITextField cityTextView, SettingsViewController settingsViewController)
            {
                this.cityTextView = cityTextView;
                ViewController = settingsViewController;
            }

            public override nint GetComponentCount(UIPickerView pickerView)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
            {
                return Items.Count;
            }

            public override string GetTitle(UIPickerView pickerView, nint row, nint component)
            {
                return Items[(int)row];
            }

            public override void Selected(UIPickerView pickerView, nint row, nint component)
            {
                cityTextView.Text = Items[(int)pickerView.SelectedRowInComponent(0)];
                ViewController.NameOfCity = cityTextView.Text;
            }
        }

        protected virtual void RegisterForKeyboardNotifications()
        {
            if (keyboardShowObj == null)
            {
                keyboardShowObj = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardDidShowNotification);
                keyboardHideObj = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardWillHideNotification);
            }
        }

        protected virtual void RemoveForKeyboardNotifications()
        {
            if (keyboardShowObj != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardShowObj);
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardHideObj);
                keyboardShowObj = null;
                keyboardHideObj = null;
            }
        }

        private void OnKeyboardDidShowNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
            {
                return;
            }
            var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            MainScrollBottomConstraint.Constant = keyboardFrame.Height;
            View.UpdateConstraints();

        }

        private void OnKeyboardWillHideNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
            {
                return;
            }
            MainScrollBottomConstraint.Constant = 0;
            View.UpdateConstraints();
        }
        #endregion

        #region EventHandler
        void SwComments_ValueChanged(object sender, EventArgs e)
        {
            UISwitch control = sender as UISwitch;
            if (control != null)
            {
                SL.AppSettings.AddOrUpdateValue("CommentsEnabled", control.On);

                if (control.On)
                {
                }
                else
                {
                }
            }
        }

        void SwLikes_ValueChanged(object sender, EventArgs e)
        {
            UISwitch control = sender as UISwitch;
            if (control != null)
            {
                SL.AppSettings.AddOrUpdateValue("LikesEnabled", control.On);

                if (control.On)
                {
                }
                else
                {
                }
            }
        }

        void SwDiscoverTeams_ValueChanged(object sender, EventArgs e)
        {
            UISwitch control = sender as UISwitch;
            if (control != null)
            {
                SL.AppSettings.AddOrUpdateValue("DiscoverTeamsEnabled", control.On);

                if (control.On)
                {
                }
                else
                {
                }
            }
        }

        void SwHideSocialNetworks_ValueChanged(object sender, EventArgs e)
        {
            UISwitch control = sender as UISwitch;
            if (control != null)
            {
                SL.AppSettings.AddOrUpdateValue("HideSocialNetworksEnabled", control.On);

                if (control.On)
                {
                }
                else
                {
                }
            }
        }

        partial void BtnLocationSettings_UpInside(UIButton sender)
        {
            LoadLocationSettings();
        }
        #endregion

        #region v2
        //void SwFromElectricAdvinture_ValueChanged(object sender, EventArgs e)
        //{
        //    UISwitch control = sender as UISwitch;
        //    if (control != null)
        //    {
        //        SL.AppSettings.AddOrUpdateValue("FromElectricAdvintureEnabled", control.On);

        //        if (control.On)
        //        {
        //            /*
        //            UNUserNotificationCenter.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
        //            {
        //            // You can add further code here to handle approval
        //                SL.AppSettings.AddOrUpdateValue("FromElectricAdvintureEnabled", approved);
        //            });
        //            */
        //        }
        //        else
        //        {
        //        }
        //    }
        //}
        #endregion

        #region v1
        void SwNotifications_ValueChanged(object sender, EventArgs e)
        {
            UISwitch control = sender as UISwitch;
            if (control != null)
            {
                var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
                if (control.On)
                {

                    Platform.ShowAlert(string.Empty, "To enable notifications, please navigate to Settings > Notifications > SocialLadder, and make sure Allow Notifications is set to ON.");
                    SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Enabled.ToString());
                }
                else
                {
                    //RegistrationNotification.Unregister();
                    SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Disabled.ToString());
                }
            }
        }
        #endregion
    }
}