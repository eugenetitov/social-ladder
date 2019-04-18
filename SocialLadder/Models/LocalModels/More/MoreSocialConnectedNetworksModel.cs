using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.More
{
    public class MoreSocialConnectedNetworksModel
    {
        public bool FacebookConnected { get; set; }
        public bool TwitterkConnected { get; set; }
        public bool InstagramConnected { get; set; }

        public MoreSocialConnectedNetworksModel(bool fbConnected, bool twitterConnected, bool instaConnected)
        {
            FacebookConnected = fbConnected;
            TwitterkConnected = twitterConnected;
            InstagramConnected = instaConnected;
        }
    }
}
