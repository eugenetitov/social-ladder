using System;
using UIKit;
using UserNotifications;
using SocialLadder.iOS.Services;
using SocialLadder.iOS.Delegates;

namespace SocialLadder.iOS.Helpers
{
    public class RegistrationNotification
    {
        public RegistrationNotification()
        {
            
        }

        public  static void Register(UIApplication application = null)
        {
            if (application == null)
            {
                application = UIApplication.SharedApplication;
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Sound |
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge, null);

                application.RegisterUserNotificationSettings(settings);
                application.RegisterForRemoteNotifications();
            }
            //else
            //{
            //    application.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Badge |
            //        UIRemoteNotificationType.Sound | UIRemoteNotificationType.Alert);
            //}
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(error?.Description);
                });

                UNUserNotificationCenter.Current.Delegate = new UserPushNotificationCenterDelegate();
            }
            else
            {
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
        }

        //public static void Unregister(UIApplication application = null)
        //{
        //    if (application == null)
        //    {
        //        application = UIApplication.SharedApplication;
        //    }
        //    application.UnregisterForRemoteNotifications();
        //}

        public static bool CheckIfPushNotificationsEnabled()
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
            //bool isNotifications = UIApplication.SharedApplication.IsRegisteredForRemoteNotifications;
            //isNotificationEnabled = isNotifications ? 1 : 0;
            return isNotificationEnabled > 0 ? true : false;
        }
    }
}