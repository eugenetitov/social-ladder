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
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace SocialLadder.Droid.Views.Holders
{
    public class AggFeedCellHolder : MvxRecyclerViewHolder
    {
        private readonly MvxCachedImageView _actorIcon;
        private readonly TextView _countText;
 

        public AggFeedCellHolder(View itemView, IMvxAndroidBindingContext context, string imageUrl = null) : base(itemView, context)
        {
            _actorIcon = itemView.FindViewById<MvxCachedImageView>(Resource.Id.agg_actor_iconImageView);
            _countText = itemView.FindViewById<TextView>(Resource.Id.count_more_text);
        }


        public override void OnViewRecycled()
        { 
            _countText.Visibility = ViewStates.Invisible;
            base.OnViewRecycled();
        }

        public void UpdateCountLabel(int itemCount)
        {
            _countText.Visibility = ViewStates.Visible;
            _countText.Text = string.Format("+{0}", itemCount);
            _actorIcon.SetImageBitmap(null); 
        }
    }
}