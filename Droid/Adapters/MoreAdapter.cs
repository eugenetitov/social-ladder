using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using SocialLadder.Models;

namespace SocialLadder.Droid.Adapters
{
    public class MoreAdapter : BaseAdapter<string>
    {
        Activity context;
        List<string> data;

        public MoreAdapter(Activity context) : base()
        {
            this.context = context;

            data = new List<string>() { "FAQ", "Settings", "Privacy and Terms of Service", "Logout" };
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]
        {
            get
            {

                return data[position];
            }
        }

        public override int Count
        {
            get
            {
                return data.Count;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Resource.Layout.list_item, null);

            TextView textView;

            textView = (TextView)view;


            textView.Text = data[position];

            return view;
        }
    }
}