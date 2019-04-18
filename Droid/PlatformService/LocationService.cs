using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using SocialLadder.Droid.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models.LocalModels;

namespace SocialLadder.Droid.PlatformService
{
    public class LocationService : ILocationService
    {
        private FusedLocationProviderClient fusedLocationProviderClient;
        private Context _context => Application.Context;

        //public double Lat => throw new NotImplementedException();
        //public double Long => throw new NotImplementedException();

        public LocationModel CurrentLocation { get; set; }

        private bool IsGooglePlayServicesInstalled()
        {
            var queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(_context);
            if (queryResult == ConnectionResult.Success)
            {
                ConnectService();
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                var errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
            }

            return false;
        }

        private void ConnectService()
        {
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(_context);
        }

        public void OnLocationChanged(Location location)
        {
            throw new NotImplementedException();
        }

        public async Task<LocationModel> GetCurrentLocation()
        {
            try
            {
                IsGooglePlayServicesInstalled();
                if (CurrentLocation != null)
                {
                    return CurrentLocation;
                }
                ConnectService();
                Location location = await fusedLocationProviderClient.GetLastLocationAsync();
                CurrentLocation = new LocationModel { Lat = location.Latitude, Long = location.Longitude, IsAvailable = true };
            }
            catch (System.Exception)
            {
                CurrentLocation = new LocationModel { Lat = 0, Long = 0, IsAvailable = false };
            }
            
            return CurrentLocation;
        }
    }
}