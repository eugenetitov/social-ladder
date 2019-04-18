using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;
using SocialLadder.Enums;

namespace SocialLadder.Models
{
    public class NotificationModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(ResponseModel))]
        public int ResponseModelID
        {
            get; set;
        }

        [OneToMany("NotificationModelID", CascadeOperations = CascadeOperation.All)]
        public FeedActionModel Action
        {
            get; set;
        }

        public string Message
        {
            get; set;
        }
        public string AreaGUID
        {
            get; set;
        }

        public string NotificationUID
        {
            get; set;
        }

        public string AreaName
        {
            get; set;
        }
        public int Count
        {
            get; set;
        }
        public DateTime CreationDate
        {
            get; set;
        }

        public NotificationType NotificationType
        {
            get; set;
        }

        //ToDo Add Enum for types
        public string NotificationAction;



    }

    public class NotificationResponseModel : ResponseModel
    {
        public NotificationModel NotificationObject
        {
            get; set;
        }
        public string NextPage
        {
            get; set;
        }
        public string AcknowledgedPage
        {
            get; set;
        }
    }
}
