using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace SocialLadder.Models
{
    public class ResponseModel
    {
        public int ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string NoDataMessage { get; set; }
        public string NoDataImageURL { get; set; }
        public string NoDataBackground { get; set; }

        [OneToMany ("ResponseModelID", CascadeOperations = CascadeOperation.All)]
        public List<NotificationModel> ResponseNotificationList { get; set; }
    }

    public class NetworkResponseModel : ResponseModel
    {
        
    }

    public class GuideResponseModel : ResponseModel
    {
        public string GuideUrl
        {
            get; set;
        }
    }
}
