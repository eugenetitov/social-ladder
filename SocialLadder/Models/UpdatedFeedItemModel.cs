using SocialLadder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models
{
    public class UpdatedFeedItemModel
    {
        public FeedItemModel UpdatedFeedItem;
        public FeedItemModel OldFeedItem;
        public FeedLoadingIndicatorMode LoaderMode;
    }
}
