using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using SocialLadder.Models;

namespace SocialLadder.Droid.Adapters
{
    internal class ChallengeCellHolder : Java.Lang.Object
    {
        public ChallengeModel ChallengeItem { get; set; }

        public ImageViewAsync ChallengeImage { get; set; }
        public TextView ChallengeName { get; set; }

        void Reset()
        {
            ChallengeItem = null;
            ChallengeImage.SetImageBitmap(null);
            ChallengeName.Text = null;
        }

        public void UpdateCellData(ChallengeModel item)
        {
            Reset();

            ChallengeItem = item; //update item since this holder may have been recylced so that the correct feed item is used in click events

            if (item != null)
            {
                ChallengeName.Text = item.Name;
                ImageService.Instance.LoadUrl(item.IconImageURL).Into(ChallengeImage);
            }
        }
    }

    public class ChallengesAdapter : BaseAdapter<ChallengeModel>
    {
        Activity context;

        public ChallengesAdapter(Activity context) : base()
        {
            this.context = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override ChallengeModel this[int position]
        {
            get
            {
                var list = SL.ChallengeList;
                return list != null ? list[position] : null;
            }
        }

        public override int Count
        {
            get 
            {
                var list = SL.ChallengeList;
                return list != null ? list.Count : 0;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var list = SL.ChallengeList;
            ChallengeModel item = list != null ? list[position] : null;

            //recycle a feed cell if possible for performance
            ChallengeCellHolder holder;
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.challenge_cell, null);

                //use a holder so we only need to find views and wire up the events once (wiring up events more than once would cause problems unless unsub from event handlers) for performance
                holder = new ChallengeCellHolder();
                holder.ChallengeImage = view.FindViewById<ImageViewAsync>(Resource.Id.ChallengeImage);
                holder.ChallengeName = view.FindViewById<TextView>(Resource.Id.ChallengeName);

                view.Tag = holder;
            }
            else
                holder = view.Tag as ChallengeCellHolder;

            holder.UpdateCellData(item);

            return view;
        }
    }
}