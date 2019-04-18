using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models;
using SocialLadder.ViewModels.Base;

namespace SocialLadder.Droid.Delegates
{
    public class FeedDelegate : IFeedSourceActionsDelegate
    {
        private BaseFeedViewModel ViewModel { get; }
        public MvxCachedImageView AnimatedUserImage { get; set; }
        public string AnimatedUserImagePath { get; set; }
        public bool Animated { get; set; }

        public FeedDelegate(BaseFeedViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void LoadNextPage()
        {
            ViewModel.LoadNextPageCommand.Execute();
        }

        public void PostLike(FeedItemModel item)
        {
            ViewModel.LikeFeedCommand.Execute(item);
        }

        public void PostComment(FeedItemModel item)
        {
            ViewModel.CommentFeedCommand.Execute(item);
        }

        public void LoadProfileDetails(FeedItemModel feedItem, MvxCachedImageView image)
        {
            AnimatedUserImage = image;
            AnimatedUserImagePath = AnimatedUserImage.ImagePath;
            ViewModel.LoadFeedByUrlCommand.Execute(feedItem);
        }

        public void PostReportIt(FeedItemModel feedItem)
        {
            ViewModel.PostReportItCommand.Execute(feedItem);
        }

        public void InviteToBuyClick(FeedItemModel feedItem)
        {
            ViewModel.InviteToBuyCommand.Execute(feedItem);
        }

        public void InviteToJoinClick(FeedItemModel feedItem)
        {
            ViewModel.InviteToJoinCommand.Execute(feedItem);
        }

        public void SetAnimatedImage()
        {
            if (!string.IsNullOrEmpty(AnimatedUserImagePath) && !Animated)
            {
                AnimatedUserImage.ImagePath = string.Empty;
                int resourceId = (int)typeof(Resource.Drawable).GetField("nerwork_loader").GetValue(null);
                Drawable drawable = ContextCompat.GetDrawable(Android.App.Application.Context, resourceId);
                AnimatedUserImage.SetImageDrawable(drawable);
                AnimationHelper.AnimateImage(AnimatedUserImage);
                Animated = true;
                return;
            }
            if (!string.IsNullOrEmpty(AnimatedUserImagePath) && Animated)
            {
                AnimatedUserImage.ClearAnimation();
                RemoveAnimatedImage();
                Animated = false;
            }
        }

        public void RemoveAnimatedImage()
        {
            AnimatedUserImage.ImagePath = AnimatedUserImagePath;
            AnimatedUserImage = null;
            AnimatedUserImagePath = string.Empty;
        }
    }
}