using System;
using System.Collections.Generic;

namespace SocialLadder.Models.LocalModels.Notifications
{
    public class ExpandableNotificationModel : List<LocalNotificationModel>
    { 
        public string AreaTitle { get; set; }
        public string AreaGuid { get; set; } 
        public string AreaImage { get; set; }       
        public string NotificationCount { get; set; }
        public string AcknowledgedNotificationUrl { get; set; }

        public ExpandableNotificationModel(ICollection<LocalNotificationModel> notifications) : base(notifications)
        {

        }
    }
}
