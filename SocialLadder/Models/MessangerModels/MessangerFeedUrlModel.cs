using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.MessangerModels
{
    public class MessangerFeedUrlModel : MvxMessage
    {
        public string FeedUrl { get;set; }
        public string ProfileUrl { get; set; }
        public int FriendId { get; set; }
        public bool HasToolbar { get; set; }

        public MessangerFeedUrlModel(object sender, string feedUrl, string profileUrl, int friendId, bool hasToolbar) : base(sender)
        {
            FeedUrl = feedUrl;
            ProfileUrl = profileUrl;
            FriendId = friendId;
            HasToolbar = hasToolbar;
        }
    }
}
