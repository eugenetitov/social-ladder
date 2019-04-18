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

namespace SocialLadder.Droid.Adapters.AggregateFeed
{
    public class AggFeedAdapter : MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerAdapter
    {
        public AggFeedAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {
           
        }


        public override int ItemCount
        {
            get
            {
                if (base.ItemCount > 8)
                {
                    return 8;
                }
                return base.ItemCount;
            }
        }      

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            string item = (string)GetItem(position);
            AggFeedCellHolder aggItemHolder = holder as AggFeedCellHolder;
            if ((base.ItemCount>8) && (position==7))
            {
                //aggItemHolder.UpdateCountLabel(base.ItemCount);
            }
            base.OnBindViewHolder(holder, position);
        } 

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        { 
            var itemView = base.OnCreateViewHolder(parent, viewType); 
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, this.BindingContext.LayoutInflaterHolder);
            var view = this.InflateViewForHolder(parent, viewType, itemBindingContext); 
            return new AggFeedCellHolder(view, itemBindingContext);
        } 
    }
}