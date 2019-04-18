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
using Firebase.Iid;
using Firebase.Messaging;
using SocialLadder.Droid.Interfaces;

namespace SocialLadder.Droid.PlatformService
{
    public class FirebaseService : IFirebaseService
    {
        private const string TAG = "FirebaseService";
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
            FirebaseMessaging.Instance.SubscribeToTopic("News");
            Android.Util.Log.Debug(TAG, "Subscribed to remote notifications");
            Android.Util.Log.Debug(TAG, "InstanceID token: " + FirebaseInstanceId.Instance.Token);
        }
    }
}