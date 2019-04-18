using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.PlatformService
{
    public class PushNotificationService : IPushNotificationService
    {
        public static Activity Activity { get; set; }

        public bool IsPushNotificationServiceAlailable()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(Activity);
            if (resultCode == ConnectionResult.Success)
            {
                return true;
            }
            return false;
        }

        public void RegisterPushNotificationService()
        {
            var intent = new Intent(Activity, typeof(RegistrationIntentService));
            Activity.StartService(intent);
        }
    }
}