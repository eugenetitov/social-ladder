using SocialLadder.Models.LocalModels.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Mappers
{
    public static class NotificationsMapper
    {
        public static LocalNotificationModel ItemToLocalItem(NotificationModel model)
        {
            LocalNotificationModel item = new LocalNotificationModel()
            {
                ID = model.ID,
                ResponseModelID = model.ResponseModelID,
                Action = model.Action,
                Message = model.Message,
                AreaGUID = model.AreaGUID,
                NotificationUID = model.NotificationUID,
                AreaName = model.AreaName,
                Count = model.Count,
                CreationDate = model.CreationDate,
                NotificationType = model.NotificationType,
                NotificationAction = model.NotificationAction
            };
            return item;
        }
    }
}
