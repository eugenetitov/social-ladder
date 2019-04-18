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

namespace SocialLadder.Droid.Delegates
{
    public interface IFeedCellActionsDelegate
    {
        void PostLike(int itemPosition);

        void PostComment(int itemPosition);

        void LoadProfileDetails(int itemPosition, MvxCachedImageView image);

        void PostReportIt(int itemPosition);

        void InviteToBuyClick(int itemPosition);
        void InviteToJoinClick(int itemPosition);
    }
}