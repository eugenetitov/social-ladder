using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreLocation;
using Foundation;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Delegates
{
    public class CustomLocationManagerDelegate : CLLocationManagerDelegate
    {
        LocationManager LocMgr;

        public CustomLocationManagerDelegate(LocationManager manager)
        {
            LocMgr = manager;
        }

        public override void UpdatedLocation(CLLocationManager manager, CLLocation newLocation, CLLocation oldLocation)
        {
            double lat = newLocation.Coordinate.Latitude;
            double lon = newLocation.Coordinate.Longitude;
            SL.Manager.GetCityListWithLatitude(lat, lon, GetCityResponse);


        }

        private void GetCityResponse(CityResponseModel response)
        {
            if (response != null)
            {
                var profile = SL.Profile;
                if (profile == null)
                {
                    return;
                }
                ProfileUpdateModel profileUpdateModel = new ProfileUpdateModel();
                profileUpdateModel.AppVersion = profile.AppVersion;
                profileUpdateModel.City = response.result;
                profileUpdateModel.EmailAddress = profile.EmailAddress;
                profileUpdateModel.isGeoEnabled = true;
                profileUpdateModel.isNotificationEnabled = profile.isNotificationEnabled;
                profileUpdateModel.isPhoneBookEnabled = false;
                profileUpdateModel.LocationLat = Platform.Lat;
                profileUpdateModel.LocationLon = Platform.Lon;
                profileUpdateModel.UserName = profile.UserName;
                SL.Manager.UpdateProfileAsync(profileUpdateModel, null);
                //var profile = SL.Profile;
                //profile.City = response.result;
                //SL.Manager.SaveProfileAsync(profile);
            }
        }

        public override void AuthorizationChanged(CLLocationManager manager, CLAuthorizationStatus status)
        {
            if (status == CLAuthorizationStatus.Denied)
            {
                SL.Manager.GetCityListWithLatitude(Platform.Lat, Platform.Lon, null);
            }
        }

    }
}