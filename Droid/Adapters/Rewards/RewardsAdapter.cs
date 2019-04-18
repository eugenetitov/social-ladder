using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using SocialLadder.Droid.Views.Holders;

namespace SocialLadder.Droid.Adapters.Rewards
{
    public class RewardsAdapter : MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerAdapter
    {
        public RewardsAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
          
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = base.OnCreateViewHolder(parent, viewType);
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, this.BindingContext.LayoutInflaterHolder);
            var view = this.InflateViewForHolder(parent, viewType, itemBindingContext);
            var holder = new RewardsCellHolder(view, itemBindingContext)
            {
                Click = ItemClick
            };
            return holder;
        }
    }
}