using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.Models;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.More
{
    public class SettingsViewModel : BaseViewModel, IBaseChildViewModel
    {
        private IEncodingService _encodingService;

        #region Properties
        private List<string> _cityList;
        public List<string> CityList
        {
            get => _cityList;
            set
            {
                SetProperty(ref _cityList, value);
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                SetProperty(ref _displayName, value);
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                SetProperty(ref _city, value);
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
            }
        }

        private bool _isNotifEnabled;
        public bool IsNotifEnabled
        {
            get => _isNotifEnabled;
            set
            {
                SetProperty(ref _isNotifEnabled, value);
            }
        }
        #endregion

        #region Constructors
        public SettingsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IEncodingService encodingService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _encodingService = encodingService;
            CurrentVM = this;
            var task = Task.Run(async () => { return await SL.Manager.GetProfileAsync(UpdateUserData); }).Result; 
            LoadCityListAsync();
        }
        #endregion

        #region Commands
        public MvxAsyncCommand OnSaveProfile
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    if(CheckChanges())
                    {
                        DisplayName = _encodingService.EncodeToNonLossyAscii(DisplayName); 

                        SL.Profile.UserName = DisplayName;
                        SL.Profile.City = City;
                        SL.Profile.EmailAddress = Email;
                        SL.Profile.isNotificationEnabled = IsNotifEnabled;

                        var newProfile = new ProfileUpdateModel()
                        {
                            UserName = DisplayName,
                            EmailAddress = Email,
                            isNotificationEnabled = IsNotifEnabled,
                            isPhoneBookEnabled = false,
                            isGeoEnabled = SL.Profile.isGeoEnabled,
                            LocationLat = SL.Profile.LocationLat,
                            LocationLon = SL.Profile.LocationLon,
                            City = City,
                            AppVersion = SL.Profile.AppVersion
                        };

                        await SL.Manager.UpdateProfileAsync(newProfile);
                    }
                });
            }
        }

        public MvxCommand OnNotifEnabledChangedCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var notifStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
                    if(IsNotifEnabled)
                    {
                        SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Enabled.ToString());
                    }
                    else
                    {
                        SL.AppSettings.AddOrUpdateValue("NotificationStatus", Enums.NotififcationStatus.Disabled.ToString());
                    }
                });
            }
        }

        public MvxAsyncCommand LocationCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await _navigationService.Navigate<LocationSettingsViewModel>();
                });
            }
        }
        #endregion

        #region Methods
        private async Task LoadProfile()
        {
            await SL.Manager.GetProfileAsync(UpdateUserData);
        }

        private void LoadCityListAsync()
        {
            CitiesResponseModel cities = Task.Run(async () => await SL.Manager.GetCityList()).Result;
            
            var citiesList = new List<string>();
            if(cities.result == null)
            {
                citiesList = new List<string> { "Philadelphia", "New York City", "Chicago", "Washington D.C.", "London", "Jakarta", "Ibiza", "Las Vegas", "Amsterdam", "Miami", "Istanbul", "Rio de Janeiro", "Austin", "Bucharest", "Tallahassee", "Ft. Lauderdale", "Atlanta", "Sheffield", "Leeds", "Birmingham", "Manchester", "Antwerp", "Cochabamba", "Buenos Aires", "La Paz", "Santa Cruz", "Los Angeles", "Johannesburg", "Sydney", "Brighton", "Reading", "Charleston", "Buffalo", "Portland", "Raleigh" };

            }
            if (cities.result != null)
            {
                citiesList = cities.result;
                citiesList.Sort();
            }
            CityList = citiesList;
        }

        private void UpdateUserData(ProfileResponseModel response)
        {
            var profile = SL.Profile;
            var notifStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);

            if (profile != null && notifStatus != null)
            {
                DisplayName = _encodingService.DecodeFromNonLossyAscii(profile.UserName);
                SL.Profile.UserName = DisplayName;
                City = profile.City;
                Email = profile.EmailAddress;
                IsNotifEnabled = notifStatus.Equals(Enums.NotififcationStatus.Enabled.ToString()) ? true : false;
            }
        }

        private bool CheckChanges()
        {
            var profile = SL.Profile;
            if (!profile.UserName.Equals(DisplayName))
            {
                return true;
            }
            if (!profile.EmailAddress.Equals(Email))
            {
                return true;
            }
            if (!profile.City.Equals(City))
            {
                return true;
            }
            if (profile.isNotificationEnabled != IsNotifEnabled)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
