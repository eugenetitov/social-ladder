using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Gms.Maps;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Cross;
using FFImageLoading.Views;
using Java.Lang;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using SocialLadder.Droid.Delegates;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Views.Holders;
using SocialLadder.Enums;
using SocialLadder.Models;

namespace SocialLadder.Droid.Adapters
{  
    public class FeedAdapter : MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerAdapter, IFeedCellActionsDelegate
    {
        bool IsLoading { get; set; }


        public IFeedSourceActionsDelegate Delegate {
            get; set;
        }

        public FeedAdapter( IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
           
        }

        public int GetItemPosition(FeedItemModel feedItemModel)
        { 
            var position = base.GetViewPosition(feedItemModel);
            return position;
        }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            int lastItemPosition = ItemCount - 1;
            if (position == lastItemPosition)
            {                
                Delegate.LoadNextPage();
            }

            FeedCellHolder feedCellHolder = holder as FeedCellHolder;
            FeedItemModel item = (FeedItemModel)GetItem(position);
            if (feedCellHolder != null)
            { 
                feedCellHolder.UpdateLayoutSections(item.LayoutSections, item.BaseContent, item.ChallengeTypeDisplayName);

                feedCellHolder.OnShowAllCommentsButtonClicked = new Action(() =>
                {
                    feedCellHolder.ShowCommentsWithEmoji(item.FilteredEngagementList.Where(x => x.EngagementType == "COMMENT").ToList());
                });
                GoogleMap map = feedCellHolder.CurrentMap;
                FeedContentMapModel mapContent = item.BaseContent as FeedContentMapModel;
                if ((mapContent != null) && (map != null))
                {
                    //GoogleMapHelper.UpdateMapZoom(map, mapContent.Lat, mapContent.Long, 5);

                    //map.MoveCamera(CameraUpdateFactory.NewLatLng(new Android.Gms.Maps.Model.LatLng(mapContent.Lat, mapContent.Long))); 
                    //CameraUpdate zoom = CameraUpdateFactory.ZoomTo(15);
                    //map.AnimateCamera(zoom);
                }
                //ChallengeName, OfferName  ChallengeTypeDisplayName
                if (item.AggregateProfileImageUrls != null)
                {
                    var textEnd = item.ChallengeTypeDisplayName == null || item.ChallengeTypeDisplayName == string.Empty ? "reward" : "challenge";
                    var aggText = item.AggregateProfileImageUrls.Count.ToString() + (item.AggregateProfileImageUrls.Count > 1 ? " people" : " person") + " completed this " + textEnd;
                    feedCellHolder.SetAggregateViewText(aggText);
                }
                if (item.Header != null)
                {
                    string emojiString;
                    try
                    {
                        if (!string.IsNullOrEmpty(item.Header.Actor))
                        {
                            var convertStr = string.Join("-", System.Text.RegularExpressions.Regex.Matches(item.Header.Actor, @"..").Cast<System.Text.RegularExpressions.Match>().ToList());
                            string[] tempArr = convertStr.Split('-');
                            byte[] decBytes = new byte[tempArr.Length];
                            for (int i = 0; i < tempArr.Length; i++)
                            {
                                decBytes[i] = System.Convert.ToByte(tempArr[i], 16);
                            }
                            string strWithEmoji = Encoding.BigEndianUnicode.GetString(decBytes, 0, decBytes.Length);
                            emojiString = strWithEmoji;
                        }
                        else
                            emojiString = item.Header.Actor;
                    }
                    catch (FormatException)
                    {
                        emojiString = item.Header.Actor;
                    }

                    var titleText = string.IsNullOrEmpty(item.Header.Actor) ? item.Header.ActionText : emojiString + " " + item.Header.ActionText;
                    feedCellHolder.SetTitleViewText(titleText);
                }
                if(item.HasComments)
                {
                    //var engagementList = item.FilteredEngagementList.Where(x => x.EngagementType == "COMMENT").ToList();
                    //feedCellHolder.ShowComments(engagementList.GetRange(0, 3));
                }
            }
            base.OnBindViewHolder(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = base.OnCreateViewHolder(parent, viewType);
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, this.BindingContext.LayoutInflaterHolder);
            var view = this.InflateViewForHolder(parent, viewType, itemBindingContext);
            var holder = new FeedCellHolder(view, itemBindingContext)
            {
                Click = ItemClick,
                LongClick = ItemLongClick
            };
            holder.Delegate = this;
            return holder;
        }

        public void PostLike(int position)
        {
            FeedItemModel item = (FeedItemModel)GetItem(position);  
            Delegate.PostLike(item);
        }

        public void PostComment(int position)
        {
            FeedItemModel item = (FeedItemModel)GetItem(position);
            Delegate.PostComment(item);
        }

        public void LoadProfileDetails(int position, MvxCachedImageView image)
        {
            FeedItemModel item = (FeedItemModel)GetItem(position);
            Delegate.LoadProfileDetails(item, image);
        }

        public void PostReportIt(int position)
        {
            Delegate.PostReportIt((FeedItemModel)GetItem(position));
        }

        public void InviteToBuyClick(int itemPosition)
        {
            Delegate.InviteToBuyClick((FeedItemModel)GetItem(itemPosition));
        }

        public void InviteToJoinClick(int itemPosition)
        {
            Delegate.InviteToJoinClick((FeedItemModel)GetItem(itemPosition));
        }
    }
}