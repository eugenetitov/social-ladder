using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading.Cross;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid.Views.Holders
{
    public class RewardsCategoryCellHolder : MvxRecyclerViewHolder
    {
     
        public RewardsCategoryCellHolder(View itemView, IMvxAndroidBindingContext bindingContext) : base(itemView, bindingContext)
        {
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.rewards_category_count_text), FontsConstants.PN_R, 0.03f);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.reward_category_text), FontsConstants.PN_B, 0.05f);
        }
    }
}