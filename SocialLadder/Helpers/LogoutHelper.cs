using MvvmCross.Platform;
using SocialLadder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Helpers
{
    public static class LogoutHelper
    {
        public static async Task Logout()
        {
            if (SL.IsNetworkConnected("Facebook"))
            {
                Mvx.Resolve<IFacebookShareService>().SetAccessToken(string.Empty, string.Empty, true);
            }
            SL.Logout();
            var _locatioService = Mvx.Resolve<ILocationService>();
            var location = _locatioService.CurrentLocation == null ? await _locatioService.GetCurrentLocation() : _locatioService.CurrentLocation;
            await SL.Manager.GetCityListWithLatitude(location.Lat, location.Long);
        }
    }
}
