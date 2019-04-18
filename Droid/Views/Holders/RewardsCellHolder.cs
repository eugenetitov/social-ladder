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
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid.Views.Holders
{
    public class RewardsCellHolder : MvxRecyclerViewHolder
    {
        public override void OnViewRecycled()
        {

            base.OnViewRecycled();
        }

        public RewardsCellHolder(View itemView, IMvxAndroidBindingContext bindingContext) : base(itemView, bindingContext)
        {
            //subcategory fonts
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.rewards_category_count_text), FontsConstants.PN_R, 0.03f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.reward_category_text), FontsConstants.PN_B, 0.055f);

            //reward fonts
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.reward_name_text), FontsConstants.PN_B, 0.05f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.point_price_text), FontsConstants.PN_R, 0.03f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.unit_count_text), FontsConstants.PN_R, 0.03f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.text_view1), FontsConstants.PN_R, 0.031f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.count_down_text), FontsConstants.PN_R, 0.031f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.points_text), FontsConstants.PN_B, 0.03f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.more_text), FontsConstants.PN_B, 0.03f);

        }
    }
}