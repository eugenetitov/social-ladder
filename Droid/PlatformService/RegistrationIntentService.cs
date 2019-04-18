using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Gcm;
using Android.Gms.Iid;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.PlatformService
{
    [Service(Exported = false)]
    class RegistrationIntentService : IntentService
    {
        static readonly string[] Topics = {
        "global"
    };
        public RegistrationIntentService() : base("RegistrationIntentService")
        {
        }
        protected override void OnHandleIntent(Intent intent)
        {
            try
            {
                Log.Info("RegistrationIntentService", "Calling InstanceID.GetToken");
                lock (this)
                {
                    var instanceID = InstanceID.GetInstance(this);
                    var token = instanceID.GetToken(
                        "592518889540", GoogleCloudMessaging.InstanceIdScope, null);
                    Log.Info("RegistrationIntentService", "GCM Registration Token: " + token);
                    SendRegistrationToAppServer(token);
                    SubscribeToTopics(token, Topics);
                }
            }
            catch (Exception e)
            {
                Log.Debug("RegistrationIntentService", "Failed to get a registration token");
                return;
            }
        }
        void SendRegistrationToAppServer(string token)
        {
            SL.Manager.UpdateAPNToken(token, null);
        }
        void SubscribeToTopics(string token, string[] topics)
        {
            foreach (var topic in topics)
            {
                var pubSub = GcmPubSub.GetInstance(this);
                pubSub.Subscribe(token, "/topics/" + topic, null);
            }
        }
    }
}