using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content.Res;
using Android.Gms.Maps;
using Android.Support.Constraints;
using Android.Support.Text.Emoji.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Cross;
using FFImageLoading.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Adapters.AggregateFeed;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Converters;
using SocialLadder.Droid.Delegates;
using SocialLadder.Droid.Helpers;
using SocialLadder.Enums;
using SocialLadder.Models;
using static Android.Views.View;

namespace SocialLadder.Droid.Views.Holders
{
    internal class FeedCellHolder : MvxRecyclerViewHolder, IOnMapReadyCallback
    {
        #region Properties
        private readonly RelativeLayout _invieToJoinLayout;
        private readonly RelativeLayout _mediaContent;
        private readonly LinearLayout _aggContentLayout;
        private readonly RelativeLayout _headerLayout;
        private readonly RelativeLayout _rewardClaimed;
        private readonly ImageViewAsync _actorImageView;
        private readonly TextView _eventNameTextView;
        private readonly MvxCachedImageView _mvxImageView;
        private readonly RelativeLayout _itbFeedContentLayout;
        private readonly ConstraintLayout _mapLayout;
        private readonly Button _readAllCommentsButton, _inviteToBuyButton;
        private readonly TextView _commentsTextView;
        private readonly MvxCachedImageView _likesIcon;
        private readonly MvxCachedImageView _commentIcon;
        private readonly MvxRecyclerView _aggregateRecyclerView;
        private readonly RelativeLayout _footerRelativelayout;
        private readonly ImageView _feedImageOverlay;
        private readonly ImageView _reportImageButton;
        private readonly MvxCachedImageView _agg_background_image;
        private readonly ImageView _agg_subIcon;
        private readonly TextView _likeTextView, _aggregate_text, _actorNameTextView;
        private readonly MapView _mapView;
        private readonly Android.Content.Context _context;

        #endregion

        public IFeedCellActionsDelegate Delegate
        {
            get;
            set;
        }

        public Action OnShowAllCommentsButtonClicked { get; set; }

        public GoogleMap CurrentMap
        {
            get;set;
        }

        private double _latitude;
        private double _longitude;

        public FeedCellHolder(View itemView, IMvxAndroidBindingContext bindingContext) : base(itemView, bindingContext)
        {
            _headerLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.headerLayout);
            _mediaContent = itemView.FindViewById<RelativeLayout>(Resource.Id.mediaContentLayout);
            _invieToJoinLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.itjFeedContentLayout);
            _eventNameTextView = itemView.FindViewById<TextView>(Resource.Id.titleTextView);
            _mvxImageView = itemView.FindViewById<MvxCachedImageView>(Resource.Id.contentImageView);
            _itbFeedContentLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.itbFeedContentLayout);
            _mapLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.mapContentLayout);
            _readAllCommentsButton = itemView.FindViewById<Button>(Resource.Id.readCommentsButton);
            _commentsTextView = itemView.FindViewById<TextView>(Resource.Id.commentsTextView);
            _aggContentLayout = itemView.FindViewById<LinearLayout>(Resource.Id.aggFeedContentLayout);
            _rewardClaimed = itemView.FindViewById<RelativeLayout>(Resource.Id.rewardClaimedContent);
            _likesIcon = itemView.FindViewById<MvxCachedImageView>(Resource.Id.likesIcon);
            _commentIcon = itemView.FindViewById<MvxCachedImageView>(Resource.Id.commentsIcon);
            _aggregateRecyclerView = itemView.FindViewById<MvxRecyclerView>(Resource.Id.aggCollection);
            _footerRelativelayout = itemView.FindViewById<RelativeLayout>(Resource.Id.footerContentLayout);
            _feedImageOverlay = itemView.FindViewById<ImageView>(Resource.Id.gradient_overlay_image);
            _reportImageButton = itemView.FindViewById<ImageView>(Resource.Id.report_button);
            _actorImageView = itemView.FindViewById<MvxCachedImageView>(Resource.Id.ActorImage);
            _agg_background_image = itemView.FindViewById<MvxCachedImageView>(Resource.Id.agg_background_image);
            _agg_subIcon = itemView.FindViewById<ImageView>(Resource.Id.agg_sub_icon_image);
            _likeTextView = itemView.FindViewById<TextView>(Resource.Id.likesCountText);
            _mapView = itemView.FindViewById<MapView>(Resource.Id.map_view);
            _aggregate_text = itemView.FindViewById<TextView>(Resource.Id.aggregate_text);
            _actorNameTextView = itemView.FindViewById<TextView>(Resource.Id.actorNameTextView);

            _context = itemView.Context;


            _aggregateRecyclerView.Adapter = new AggFeedAdapter((IMvxAndroidBindingContext)BindingContext);
            var layoutManager = new LinearLayoutManager(_context);
            layoutManager.Orientation = LinearLayoutManager.Horizontal;
            _aggregateRecyclerView.SetLayoutManager(layoutManager);

            this.DelayBind(() =>
            {
                var set = this.CreateBindingSet<FeedCellHolder, FeedItemModel>();
                set.Bind(_commentsTextView).For(x => x.TextFormatted).To(x => x.FilteredEngagementList).OneTime().WithConversion(new ListEngagementToCommentsConverter(), 3);
                set.Bind(_aggregateRecyclerView).For(x => x.ItemsSource).To(x => x.AggregateProfileImageUrls);
                set.Apply();
            });

            //_readAllCommentsButton.Click += OnReadCommentButtonClick;
            //_likesIcon.Click += OnLikeButtonClick;
            //_commentIcon.Click += OnCommentButtonClick;
            //_actorImageView.Click += OnLoadFriendFeedButtonClick;
            AddEvents();

            _reportImageButton.Click += ((sender, e) =>
             {
                 Delegate.PostReportIt(AdapterPosition);
             });

            var tnnITB = itemView.FindViewById<Button>(Resource.Id.inviteToBuyBtn);
            tnnITB.Click += (s, e) =>
            {
                Delegate.InviteToBuyClick(AdapterPosition);
            };
            var btnITJ = itemView.FindViewById<Button>(Resource.Id.inviteToJoinBtn);
            btnITJ.Click += (s, e) =>
            {
                Delegate.InviteToJoinClick(AdapterPosition);
            };
            UpdateFonts();
        }

        private void AddEvents()
        {
            _readAllCommentsButton.Click += (s, e) => {
                OnShowAllCommentsButtonClicked();
                _readAllCommentsButton.Visibility = ViewStates.Gone;
            };
            _likesIcon.Click += (s, e) => {
                Delegate.PostLike(AdapterPosition);
            };
            _commentIcon.Click += (s, e) => {
                Delegate.PostComment(AdapterPosition);
            };
            _actorImageView.Click += (s, e) => {
                Delegate.LoadProfileDetails(AdapterPosition, (MvxCachedImageView)s);
            };
        }

        //~FeedCellHolder() 
        //{
        //    _readAllCommentsButton.Click -= OnReadCommentButtonClick;
        //    _likesIcon.Click -= OnLikeButtonClick;
        //    _commentIcon.Click -= OnCommentButtonClick;
        //    _actorImageView.Click -= OnLoadFriendFeedButtonClick;
        //} 

        public void ShowComments(List<FeedEngagementModel> engagements)
        { 
            _readAllCommentsButton.Visibility = ViewStates.Gone;
            _commentsTextView.TextFormatted = CommentsHelper.BuildComments(engagements);
        }

        public void ShowCommentsWithEmoji(List<FeedEngagementModel> engagements)
        {
            _readAllCommentsButton.Visibility = ViewStates.Gone;
            _commentsTextView.TextFormatted = CommentsHelper.BuildCommentsWithEmoji(engagements);
        }

        public void UpdateLike(FeedItemModel feedItem)
        {
            if (feedItem.Likes > 1)
            {
                _likeTextView.Text = string.Format("{0} likes", feedItem.Likes);
            }
            else
            {
                _likeTextView.Text = string.Format("{0} like", feedItem.Likes);
            }

            if (feedItem.DidLike == true)
            {
                _likesIcon.ImagePath = "res:ic_heart_icon_on";
                _likesIcon.Reload();
                return;
            }
            _likesIcon.ImagePath = "res:ic_heart_icon_off";
            _likesIcon.Reload();

        }


        public override void OnViewRecycled()
        {
            if (CurrentMap != null)
            {
                CurrentMap.Clear();
                CurrentMap.MapType  = GoogleMap.MapTypeNone;
            }
            _agg_subIcon.SetImageBitmap(null);
            base.OnViewRecycled();
        }

        private void UpdateFonts()
        {
            //header
            float headerFontSize = (float)0.035;
            float timeFontSize = (float)0.03;
            FontHelper.UpdateFont(_actorNameTextView, FontsConstants.PN_R, headerFontSize);
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.titleTextView), FontsConstants.PN_R, headerFontSize);
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.timeTextView), FontsConstants.PN_R, timeFontSize);

            //footer
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.likesCountText), FontsConstants.PN_R, 0.04f);
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.pointsCountText), FontsConstants.PN_R, 0.04f);
            

            //itj
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.invite_text), FontsConstants.PN_R, timeFontSize);
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.actor_friend_text), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.welcome_Text), FontsConstants.PN_B, headerFontSize);

            //media
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.media_overlay_text), FontsConstants.PN_R, (float)0.04);

            //itb
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.unit_sold_text), FontsConstants.PN_R, (float)0.08);

            //reward claimed
            FontHelper.UpdateFont(ItemView.FindViewById<TextView>(Resource.Id.reward_title_text), FontsConstants.PN_B, 0.06f);
            //FontHelper.UpdateTextViewFont(view.FindViewById<TextView>(Resource.Id.tv_ranked), FontsConstants.PN_R, (float)0.04);

            FontHelper.UpdateFont(_aggregate_text, FontsConstants.PN_R, 0.041f);
        }

        public void UpdateFeedItem(FeedItemModel feedItemModel)
        {
            ShowComments(feedItemModel.FilteredEngagementList);
        }

        public void UpdateLayoutSections(FeedContentType[] content, FeedContentModelBase feedContent =null, string challengeTypeDisplayName = null)
        { 
            
            if (content == null)
            {
                content = new Enums.FeedContentType[] { };
            }

            if (content.Contains(Enums.FeedContentType.Header))
            {
                _headerLayout.Visibility = ViewStates.Visible;
            }
            else
            {
                _headerLayout.Visibility = ViewStates.Gone;
            }

            if (!content.Contains(Enums.FeedContentType.Text))
            {

            }
            else
            {

            }

            if (!content.Contains(Enums.FeedContentType.HtmlText))
            {

            }
            else
            {

            }
            if (content.Contains(Enums.FeedContentType.Image))
            {
                _mediaContent.Visibility = ViewStates.Visible;
                var imageContent = feedContent as FeedContentImageModel;
                float width = 1f;
                float height = 1f;

                if (imageContent != null)
                {
                    width = imageContent.ImageWidth;
                    height = imageContent.ImageHeight;
                }

                var multiplier = height/ width;
                var screenWidth = Resources.System.DisplayMetrics.WidthPixels;
                _mvxImageView.LayoutParameters.Height = (int)(screenWidth * multiplier);
                _feedImageOverlay.LayoutParameters.Height = (int)(screenWidth * multiplier);
            }
            else
            {
                _mediaContent.Visibility = ViewStates.Gone;

            }

            if (!content.Contains(Enums.FeedContentType.Video))
            {

            }
            else 
            {

            }
            if (!content.Contains(Enums.FeedContentType.Map))
            {
                _mapLayout.Visibility = ViewStates.Gone;
            }
            else 
            {
                _mapLayout.Visibility = ViewStates.Visible;
                var mapContent = feedContent as FeedContentMapModel;


                if (mapContent != null)
                {
                    _latitude = mapContent.Lat;
                    _longitude = mapContent.Long;
                }

                if (_mapView != null)
                {
                    _mapView.OnCreate(null);
                    _mapView.OnResume();
                    _mapView.GetMapAsync(this);
                }
            }

            if (!content.Contains(Enums.FeedContentType.CollectionCheckIn))
            {

            }
            else 
            {

            }

            if (!content.Contains(Enums.FeedContentType.Aggregate))
            {
                _aggContentLayout.Visibility = ViewStates.Gone;
                _eventNameTextView.Visibility = ViewStates.Gone;
                _agg_background_image.Visibility = ViewStates.Gone;
                _agg_subIcon.Visibility = ViewStates.Gone;
            }
            else
            {
                _aggContentLayout.Visibility = ViewStates.Visible;
                _eventNameTextView.Visibility = ViewStates.Visible; 
                _agg_background_image.Visibility = ViewStates.Visible; 
                _agg_subIcon.Visibility = ViewStates.Visible;

                if (challengeTypeDisplayName != null)
                { 
                    SetupAggregateActorChallengeSubviews(challengeTypeDisplayName);
                }
            }
            if (!content.Contains(Enums.FeedContentType.ProductSold))
            {
                _itbFeedContentLayout.Visibility = ViewStates.Gone;
            }
            else 
            { 
                _itbFeedContentLayout.Visibility = ViewStates.Visible;
            }

            if (!content.Contains(Enums.FeedContentType.FriendJoined))
            {
                _invieToJoinLayout.Visibility = ViewStates.Gone;
            }
            else 
            {
                _invieToJoinLayout.Visibility = ViewStates.Visible;
            }

            if (!content.Contains(Enums.FeedContentType.RewardClaimed))
            {

                _rewardClaimed.Visibility = ViewStates.Gone;
            }
            else 
            {

                _rewardClaimed.Visibility = ViewStates.Visible;
            }

            if (!content.Contains(Enums.FeedContentType.Share))
            {

            }
            else 
            {

            }

            if (!content.Contains(FeedContentType.Engagement))
            {
                _footerRelativelayout.Visibility = ViewStates.Gone;
            }
            else 
            {
                _footerRelativelayout.Visibility = ViewStates.Visible;
            }
        }

        private void SetupAggregateActorChallengeSubviews(string challengeType)
        {
            if (string.IsNullOrEmpty(challengeType))
            {
                return;
            }
            switch (challengeType)
            {
                case ("Survey"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_quiz_icon_white);// UIImage.FromBundle("quiz-icon_white"));
                    break;
                case ("Checkin"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_quiz_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("location-icon_white");
                    break;
                case ("CollateralTracking"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_pin_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("location-icon_white");
                    break;
                case ("Postering"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_pin_icon_white); 
                    //_agg_subIcon.Image = UIImage.FromBundle("location-icon_white");
                    break;
                case ("FacebookEngagement"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.fb_logo_white);
                   // _agg_subIcon.Image = UIImage.FromBundle("fb-logo_white");
                    break;
                case ("Instagram"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_insta_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("insta-icon_white");
                    break;
                case ("InviteToBuy"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.ticket_icon);
                    //_agg_subIcon.Image = UIImage.FromBundle("ticket-icon_white");
                    break;
                case ("InviteToJoin"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_invite_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("invite-icon_white");
                    break;
                case ("Share"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_bullhorn_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("bullhorn_icon_white");
                    break;
                case ("Flyering"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.flyering_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("flyering-icon_large");
                    break;
                case ("Manual"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.manual_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("manual-icon_large");
                    break;
                case ("Quiz"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_quiz_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("quiz-icon_white");
                    break;
                case ("Signup"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.signup_icon_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("signup-icon_large");
                    break;
                case ("Reward"): 
                    _agg_subIcon.SetImageBitmap(null);
                    //_agg_subIcon.Image = UIImage.FromBundle("reward-icon_white");
                    break;
                case ("Facebook Engagement"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.fb_logo_white);
                    //_agg_subIcon.Image = UIImage.FromBundle("fb-logo");
                    break;
                case ("Twitter"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.icons_twitter_white);
                    break;
                case ("Document Submission"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.docsubmit_icon_white);
                    break;
                case ("Facebook"):
                    _agg_subIcon.SetImageResource(Resource.Drawable.fb_logo_white);
                    break;
            }

        }

        public void SetAggregateViewText(string text)
        {
            _aggregate_text.Text = string.IsNullOrEmpty(text) ? string.Empty : text;
        }

        public void SetTitleViewText(string text)
        {
            _actorNameTextView.Text = string.IsNullOrEmpty(text) ? string.Empty : text;
        }



        //private void OnCommentButtonClick(object sender, EventArgs e)
        //{
        //    Delegate.PostComment(AdapterPosition);
        //}

        //private void OnLikeButtonClick(object sender, EventArgs e)
        //{  
        //    Delegate.PostLike(AdapterPosition);
        //}

        //private void OnReadCommentButtonClick(object sender, EventArgs e)
        //{ 
        //    OnShowAllCommentsButtonClicked();
        //    _readAllCommentsButton.Visibility = ViewStates.Gone;
        //}

        //private void OnLoadFriendFeedButtonClick(object sender, EventArgs e)
        //{
        //    Delegate.LoadProfileDetails(AdapterPosition, (MvxCachedImageView)sender);
        //}

        public void OnMapReady(GoogleMap googleMap)
        {
            GoogleMapHelper.UpdateMapZoom(googleMap, _latitude, _longitude, 5);
            CurrentMap = googleMap; 
        }
    }
}