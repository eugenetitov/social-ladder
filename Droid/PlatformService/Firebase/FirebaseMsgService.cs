using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using Newtonsoft.Json;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.CustomListeners;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid.PlatformService
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseMsgService : FirebaseMessagingService
    {
        const string TAG = "FirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            if (Constants.NeedFirebaseImpl)
            {

                Log.Debug(TAG, "From: " + message.From);

                var body = message.GetNotification()?.Body;
                Log.Debug(TAG, "Notification Message Body: " + body);
                SendNotification(body, message.Data);
            }
        }

        private void SendNotification(string messageBody, IDictionary<string, string> data)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            if (data != null)
                foreach (var key in data.Keys)
                {
                    intent.PutExtra(key, data[key]);
                }

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

            string channelId = "fcm_default_channel";
            Android.Net.Uri defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(this, channelId)
                                      .SetSmallIcon(Resource.Drawable.notification_icon)
                                      .SetStyle(new NotificationCompat.BigTextStyle()
                                      .BigText(messageBody))
                                      .SetColor(Color.Black.ToArgb())
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetSound(defaultSoundUri)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
            {
                NotificationChannel channel = new NotificationChannel(channelId, "Channel human readable title", NotificationManager.ImportanceDefault);
                notificationManager.CreateNotificationChannel(channel);
            }

            Random random = new Random();
            int notifId = random.Next(9999 - 1000) + 1000;

            notificationManager.Notify(notifId, notificationBuilder.Build());

        }
    }
}