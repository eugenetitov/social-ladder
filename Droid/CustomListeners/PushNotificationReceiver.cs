using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.Navigation;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Newtonsoft.Json;
using SocialLadder.Droid.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;

namespace SocialLadder.Droid.CustomListeners
{
    [BroadcastReceiver]
    public class PushNotificationReceiver : BroadcastReceiver
    {
        public PushNotificationReceiver()
        {
        }

        public override void OnReceive(Context context, Intent intent)
        {
            try
            {
                var notificationBundle = intent.Extras.GetBundle(LocalConstants.NotificationAction);
                var actionString = notificationBundle.GetString(LocalConstants.NotificationObject);
                var action = JsonConvert.DeserializeObject<FeedActionModel>(actionString);
                var _navigationService = Mvx.Resolve<IMvxNavigationService>();
                var _alertService = Mvx.Resolve<IAlertService>();
                var _messenger = Mvx.Resolve<IMvxMessenger>();
                
                Mvx.Resolve<IActionHandlerService>().HandleActionAsync(_navigationService, _alertService, _messenger, action);
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}