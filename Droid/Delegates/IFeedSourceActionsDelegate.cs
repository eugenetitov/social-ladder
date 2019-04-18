using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using SocialLadder.Models;

namespace SocialLadder.Droid.Delegates
{
    public interface IFeedSourceActionsDelegate
    {
        void LoadNextPage();
        void PostLike(FeedItemModel feedItem);
        void PostComment(FeedItemModel feedItem); 
        void LoadProfileDetails(FeedItemModel feedItem, MvxCachedImageView image);
        void PostReportIt(FeedItemModel feedItem);
        void InviteToBuyClick(FeedItemModel feedItem);
        void InviteToJoinClick(FeedItemModel feedItem);
    }
}