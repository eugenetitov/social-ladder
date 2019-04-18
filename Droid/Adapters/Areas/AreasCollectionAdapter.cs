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
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Views.Holders;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Areas;

namespace SocialLadder.Droid.Adapters.Areas
{
    public class AreasCollectionAdapter : MvxRecyclerAdapter
    {
        public AreasCollectionAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
        {

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = base.OnCreateViewHolder(parent, viewType);
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, this.BindingContext.LayoutInflaterHolder);
            var view = this.InflateViewForHolder(parent, viewType, itemBindingContext);
            return new AreasCollectionViewHolder(view, itemBindingContext)
            {
                Click = ItemClick,
                LongClick = ItemLongClick
            };
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var item = (AreaModel)GetItem(position);
            var areaHolder = holder as AreasCollectionViewHolder;
            if (item is LocalAreasModel && (item as LocalAreasModel).IsAddAreaButton)
            {
                areaHolder.SetDefaultImage();
            }
            base.OnBindViewHolder(holder, position);
        }
    }
}