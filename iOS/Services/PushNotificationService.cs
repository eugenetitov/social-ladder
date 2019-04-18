using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Foundation;
using Newtonsoft.Json;
using SocialLadder.Enums;
using SocialLadder.iOS.Helpers;
using SocialLadder.Models;
using UIKit;
using UserNotifications;

namespace SocialLadder.iOS.Services
{
    public class PushNotificationService
    {
        public static void AddLocalNotification(NotificationModel model)
        {
            model.NotificationType = Enums.NotificationType.LocalNotification;
            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            var requestID = Guid.NewGuid().ToString();
            var jsonData = JsonConvert.SerializeObject(model);
            NSError error = new NSError();
            //NSDictionary jsonDict = (NSDictionary)NSJsonSerialization.Deserialize("{ \"Action\": {\"ActionScreen\":\"FEED\",\"ActionParamDict\":{\"FeedID\":\"11376388\",\"FeedURL\":\"https://socialladder.rkiapps.com/SocialLadderAPI/api/v1/feed?deviceUUID=3C98514894414A379F2A73C916A63FB7&ForFeedID=11376388&AreaGUID=5CFAF4F1-5DEF-407F-A35A-561C1E5F5AD0\",\"FeedItemPosition\":\"1\"}},\"Message\":\"xxx123 commented on your feed\",\"AreaGUID\":\"5CFAF4F1-5DEF-407F-A35A-561C1E5F5AD0\",\"NotificationType\":1 }",0, out error);
            NSDictionary jsonDict = (NSDictionary)NSJsonSerialization.Deserialize(jsonData, 0, out error);
            jsonDict = jsonDict.RemoveNullValues();
            var content = new UNMutableNotificationContent { Title = string.Empty, Subtitle = string.Empty, Body = model.Message, Badge = 1, UserInfo = jsonDict };
            var request = UNNotificationRequest.FromIdentifier(requestID, content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    System.Diagnostics.Debug.WriteLine(err);
                }
            });
        }

        public async void ProcesRemoteNotificationAction(NotificationModel notificaton)
        {
            var notificationActionHandler = new ActionHandlerService();
            var notificationModel = await SL.Manager.AcknowledgeNotification(notificaton.NotificationUID, null);
            if (notificationModel == null)
            {
                return;
            }
            SwitchAreaIfNeeded(notificaton.AreaGUID);
            notificationActionHandler.HandleActionAsync(notificationModel.Action);
        }

        public void ProcesLocalNotificationAction(NotificationModel notification)
        {
            var notificationActionHandler = new ActionHandlerService();
            SwitchAreaIfNeeded(notification.AreaGUID);
            notificationActionHandler.HandleActionAsync(notification.Action);
        }

        public void SwitchAreaIfNeeded(string areaGuid)
        {
            ActionHandlerService.SwitchAreaIfNeeded(areaGuid);
        }

        public NotificationModel ParceFromUNNotification(UNNotification notification)
        {
            NotificationModel notificationModel = new NotificationModel();
            if (notification.Request.Content.UserInfo.Keys.Count() != 0)
            {
                NSDictionary userInfo = notification.Request.Content.UserInfo;

                if (null != userInfo && userInfo.ContainsKey(new NSString("aps")))
                {
                    //Get the aps dictionary
                    NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
                    NSDictionary alert = aps.ObjectForKey(new NSString("aps")) as NSDictionary;

                    notificationModel.AreaGUID = (userInfo.ObjectForKey(new NSString("AreaGUID") as NSString))?.ToString();
                    notificationModel.NotificationUID = (userInfo.ObjectForKey(new NSString("NotificationUID") as NSString))?.ToString();
                    return notificationModel;
                }

                if (userInfo.ContainsKey(new NSString("NotificationType")) && ((int.Parse((userInfo.ObjectForKey(new NSString("NotificationType") as NSString)).ToString())) == (int)NotificationType.LocalNotification))
                {
                    FeedActionModel actionModel = new FeedActionModel();

                    var action = userInfo.ObjectForKey(new NSString("Action")) as NSDictionary;

                    var actionParamDict = action.ObjectForKey(new NSString("ActionParamDict")) as NSDictionary;

                    actionModel.ActionScreen = ((NSString)action["ActionScreen"]);
                    actionModel.ActionParamDict = new System.Collections.Generic.Dictionary<string, string>();
                    actionModel.WebRequestURL = ((NSString)action["WebRequestUrl"]);

					if(actionParamDict!=null)
                    foreach (var item in actionParamDict)
                    {
                        actionModel.ActionParamDict.Add((NSString)item.Key, (NSString)item.Value);
                    }
                    notificationModel.Message = notification.Request.Content.Body; // (alert[new NSString("NotificationUID")] as NSString)?.ToString();
                    notificationModel.NotificationType = NotificationType.LocalNotification;
                    notificationModel.Action = actionModel;
                    notificationModel.AreaGUID = (userInfo.ObjectForKey(new NSString("AreaGUID") as NSString))?.ToString();
                    //notificationModel = JsonConvert.DeserializeObject<NotificationModel>(userInfo.ToString());
                    return notificationModel;
                }


            }
            return notificationModel;
        }

    }
}