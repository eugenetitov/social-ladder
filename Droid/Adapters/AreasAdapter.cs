using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using SocialLadder.Models;

namespace SocialLadder.Droid.Adapters
{
    internal class AreasCellHolder : Java.Lang.Object
    {
        public AreaModel AreaItem { get; set; }

        public ImageViewAsync AreaImage { get; set; }
        public TextView AreaName { get; set; }

        void Reset()
        {
            AreaItem = null;
            AreaImage.SetImageBitmap(null);
            AreaName.Text = null;
        }

        public void UpdateCellData(AreaModel item)
        {
            Reset();

            AreaItem = item; //update item since this holder may have been recylced so that the correct feed item is used in click events

            if (item != null)
            {
                AreaName.Text = item.areaName;
                ImageService.Instance.LoadUrl(item.areaDefaultImageURL).Into(AreaImage);
            }
        }
    }

    public class AreasAdapter : BaseAdapter<AreaModel>
    {
        Activity context;

        public AreasAdapter(Activity context) : base()
        {
            this.context = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override AreaModel this[int position]
        {
            get
            {
                return SL.GetArea(position);
            }
        }

        public override int Count
        {
            get
            {
                return SL.AreaCount;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            AreaModel area = SL.GetArea(position);

            //recycle a feed cell if possible for performance
            AreasCellHolder holder;
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.areas_cell, null);

                //use a holder so we only need to find views and wire up the events once (wiring up events more than once would cause problems unless unsub from event handlers) for performance
                holder = new AreasCellHolder();
                holder.AreaImage = view.FindViewById<ImageViewAsync>(Resource.Id.AreaImage);
                holder.AreaName = view.FindViewById<TextView>(Resource.Id.AreaName);

                view.Tag = holder;
            }
            else
                holder = view.Tag as AreasCellHolder;

            holder.UpdateCellData(area);

            return view;
        }
    }
}