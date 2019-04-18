using System;
using Foundation;
using SocialLadder.Enums;
using SocialLadder.iOS.Helpers;
using SocialLadder.iOS.Notifications;
using SocialLadder.iOS.Services;
using SocialLadder.Logger;
using UIKit;
using UserNotifications;

namespace SocialLadder.iOS.Delegates
{
    public class UserPushNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        private PushNotificationService _notificationService;
        private ActionHandlerService _actionHandlerService;

        #region Constructors
        public UserPushNotificationCenterDelegate()
        {
            _notificationService = new PushNotificationService();
            _actionHandlerService = new ActionHandlerService();
        }
        #endregion

        #region Override Methods
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);
            var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);
            var push = new PushNotificationService().ParceFromUNNotification(notification);
            if (push.NotificationUID != null)
            {
                SL.Manager.AcknowledgeNotification(push.NotificationUID, null);
            }
            if (notificationStatus == NotififcationStatus.Enabled.ToString())
            {
                completionHandler(UNNotificationPresentationOptions.Alert);
            }
        }

        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            try
            {

                // Take action based on Action ID
                switch (response.ActionIdentifier)
                {
                    case "reply":
                        // Do something
                        Console.WriteLine("Received the REPLY custom action.");
                        break;
                    default:
                        // Take action based on identifier
                        ResetNotificationBadge();
                        ProcessDefaultNotificationAction(response);
                        break;
                }
            }
            catch (Exception e)
            {
                LogHelper.LogUserMessage("NOTIFICATIONPENDING_FAILED", e.Message);
            }
            // Inform caller it has been handled
            completionHandler();
        }

        private void ResetNotificationBadge()
        {
            //UIApplication.SharedApplication.ApplicationIconBadgeNumber -= (int)notificationBadge;
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        private void ProcessDefaultNotificationAction(UNNotificationResponse response)
        {
            if (response.IsDefaultAction)
            {
                var notification = _notificationService.ParceFromUNNotification(response.Notification);

                if (notification.NotificationType == Enums.NotificationType.RemoteNotification)
                {
                    _notificationService.ProcesRemoteNotificationAction(notification);
                }
                else
                {
                    _notificationService.ProcesLocalNotificationAction(notification);
                }
                // Handle default action...
                Console.WriteLine("Handling the default action.");
            }
            else if (response.IsDismissAction)
            {
                // Handle dismiss action
                Console.WriteLine("Handling a custom dismiss action.");
            }
        }
        #endregion
    }
}