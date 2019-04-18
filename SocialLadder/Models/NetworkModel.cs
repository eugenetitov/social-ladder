using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace SocialLadder.Models
{
    public class SocialNetworkModel
    {
        [JsonIgnore]
        public string ID { get; set; }

        [JsonIgnore, PrimaryKey]
        public int networkID { get; set; }

        [JsonIgnore, ForeignKey(typeof(ProfileModel))]
        public int ProfileID { get; set; }

        public string UserID { get; set; }
        public string NetworkName { get; set; }
        public string AccessToken { get; set; }
        public int ScoreComponentA { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public int ScoreComponentB { get; set; }
        public string NUserName { get; set; }
        public string NLocation { get; set; }
        public string Data { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? TokenExpirationDate { get; set; }
    }
}