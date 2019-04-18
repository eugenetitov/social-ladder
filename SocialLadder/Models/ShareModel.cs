using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace SocialLadder.Models
{
    public class ShareNetworkStatus
    {
        public string NetworkName { get; set; }
        public bool AllowSharing { get; set; }
    }

    public class ShareModel
    {
        public List<ShareNetworkStatus> ShareEntryList { get; set; }
        public string ShareTransactionId { get; set; }
        public string BodyMessage { get; set; }
        public string UserMessage { get; set; }
    }

    public class ShareTemplateModel
    {
        public int? NumFriends { get; set; }
        public int? NumPoints { get; set; }
        public string PostHref { get; set; }
        public string PostTitle { get; set; }
        public string PostCaption { get; set; }
        public string PostDescription { get; set; }
        public string CommentsXid { get; set; }
        public string ImageURL { get; set; }
        public string ImageURLLowRes { get; set; }
        public string ActionLink { get; set; }
        public string ActionLinkDesc { get; set; }
        public string ReferralString { get; set; }
        public string ShareTransactionID { get; set; }
        public ShareNetworkStatus[] NetworkShareList { get; set; }
        public string InviteTitle { get; set; }
        public string InviteText { get; set; }
        public string ButtonName { get; set; }
        public string ShadowText { get; set; }
        public string InviteType { get; set; }
        public string ShareStyle { get; set; }
        public int InviteTarget { get; set; }
        public bool RestrictInvites { get; set; }
        public string RestrictionReason { get; set; }
        public bool AllowEmail { get; set; }
        public bool AllowSMS { get; set; }
        public Dictionary<string, string> InviteParamsDict { get; set; }
        public bool isEditable { get; set; }
        public bool AllowWhatsApp { get; set; }
    }

    public class ShareResponseModel : ResponseModel
    {
        public ShareTemplateModel ShareTemplate { get; set; }
    }
}