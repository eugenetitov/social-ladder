using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Gcm;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.PlatformService
{
    [Service(Exported = false), IntentFilter(new[]
    {
        "com.google.android.c2dm.intent.RECEIVE"
    })]
    public class GcmListenerService : Android.Gms.Gcm.GcmListenerService
    {
        string _chanelId;

        public GcmListenerService() : base()
        {
            CreateChanelIfNeeded();
        }


        public GcmListenerService(IntPtr javaReference, JniHandleOwnership transfer)
        {
            CreateChanelIfNeeded();
        }

        
        public override void OnMessageReceived(string from, Bundle data)
        {
            // Extract the message received from GCM:  
            var message = data.GetString("message");
            Log.Debug("MyGcmListenerService", "From: " + from);
            Log.Debug("MyGcmListenerService", "Message: " + message);
            // Forward the received message in a local notification:  
            SendNotification(message);
        }

        // Use Notification Builder to create and launch the notification:  
        private void SendNotification(string message)
        {
            var intent = new Intent(this, typeof(Activities.Main.MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var notificationBuilder = new Notification.Builder(this, _chanelId)
            .SetSmallIcon(Resource.Drawable.notification_icon) //Icon  
            .SetContentTitle("Social ladder") //Title  
            .SetContentText(message) //Message  
            .SetAutoCancel(true)
            .SetContentIntent(pendingIntent);
            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, notificationBuilder.Build());
        }

        private void CreateChanelIfNeeded()
        {
            if (_chanelId != null)
            {
                return;
            }
            _chanelId = Guid.NewGuid().ToString();
            string chanelName = "SocialLadder";
            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                // Support for Android Oreo: Notification Channels
                NotificationChannel channel = new NotificationChannel(
                            _chanelId,
                            chanelName,
                            Android.App.NotificationImportance.Default);
                notificationManager.CreateNotificationChannel(channel);
            }

        }
    }
}