using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.Interfaces;
using SocialLadder.iOS.Helpers;
using UIKit;

namespace SocialLadder.iOS.PlatformServices
{
    public class PushNotificationService : IPushNotificationService
    {
        public bool IsPushNotificationServiceAlailable()
        {
            int isNotificationEnabled;
            if (UIApplication.SharedApplication.RespondsToSelector(new ObjCRuntime.Selector("IsRegisteredForRemoteNotifications")))
            {
                bool isNotifications = UIApplication.SharedApplication.IsRegisteredForRemoteNotifications;
                isNotificationEnabled = isNotifications ? 1 : 0;
            }
            else
            {
                UIRemoteNotificationType types = UIApplication.SharedApplication.EnabledRemoteNotificationTypes;
                isNotificationEnabled = (types == UIRemoteNotificationType.Alert) ? 1 : 0;
            }
            return isNotificationEnabled > 0 ? true : false;
        }

        public void RegisterPushNotificationService()
        {
            RegistrationNotification.Register(null);
        }
    }
}