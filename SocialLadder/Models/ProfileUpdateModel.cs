using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models
{
    public class ProfileUpdateModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public bool isNotificationEnabled { get; set; }
        public bool isPhoneBookEnabled { get; set; }
        public bool isGeoEnabled { get; set; }
        public double LocationLat { get; set; }
        public double LocationLon { get; set; }
        public string City { get; set; }
        public int AppVersion { get; set; }
    }
}
