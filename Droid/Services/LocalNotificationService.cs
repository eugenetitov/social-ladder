using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using Newtonsoft.Json;
using SocialLadder.Droid.CustomListeners;
using SocialLadder.Droid.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;

namespace SocialLadder.Droid.Services
{
    public class LocalNotificationService : ILocalNotificationService
    {
        public static Activity Activity;
        public static List<string> Notifications { get; set; } = new List<string>();
        private IPlatformNavigationService _platformNavigationService = Mvx.Resolve<IPlatformNavigationService>();

        public void RefreshNotifications()
        {
            if (Activity == null || SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty) != Enums.NotififcationStatus.Enabled.ToString())
            {
                return;
            }
            if (Notifications.Count >= 10)
            {
                Notifications.Clear();
            }
            Task.Run(async () =>
            {
                await Task.Delay(40000);
                await SL.Manager.RefreshNotificationsAsync((notificationResponse) =>
                {
                    if (notificationResponse != null && notificationResponse.NotificationObject != null)
                    {
                        if (Notifications.Contains(notificationResponse.NotificationObject.Message))
                        {
                            return;
                        }
                        _platformNavigationService.ChangeNotificationIndicatorStatus(false);
                        Notifications.Add(notificationResponse.NotificationObject.Message);
                        var action = notificationResponse.NotificationObject.Action;
                        var bundle = new Bundle();
                        bundle.PutString(LocalConstants.NotificationObject, JsonConvert.SerializeObject(notificationResponse.NotificationObject.Action));
                        var _context = Application.Context;
                        Intent intent = new Intent(_context, new PushNotificationReceiver().Class);
                        intent.PutExtra(LocalConstants.NotificationAction, bundle);
                        PendingIntent pndIntent = PendingIntent.GetBroadcast(_context, 0, intent, PendingIntentFlags.UpdateCurrent);

                        Android.Net.Uri defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
                        var notificationBuilder = new NotificationCompat.Builder(Activity, LocalConstants.NotificationCannelId)
                                                  .SetSmallIcon(Resource.Drawable.notification_icon)
                                                  .SetStyle(new NotificationCompat.BigTextStyle()
                                                  .BigText(notificationResponse.NotificationObject.Message))
                                                  .SetColor(Android.Graphics.Color.Black.ToArgb())
                                                  .SetContentText(notificationResponse.NotificationObject.Message)
                                                  .SetAutoCancel(true)
                                                  .SetSound(defaultSoundUri)
                                                  .SetContentIntent(pndIntent);

                        var notificationManager = (NotificationManager)Activity.GetSystemService(Context.NotificationService);

                        if (Build.VERSION.SdkInt >= Build.VERSION_CODES.O)
                        {
                            NotificationChannel channel = new NotificationChannel(LocalConstants.NotificationCannelId, LocalConstants.NotificationCannelName, NotificationManager.ImportanceDefault);
                            notificationManager.CreateNotificationChannel(channel);
                        }

                        int notifId = new Random().Next(9999 - 1000) + 1000;
                        notificationManager.Notify(notifId, notificationBuilder.Build());
                    }
                });

            });
        }
    }
}