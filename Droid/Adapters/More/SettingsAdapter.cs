using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid.Adapters.More
{
    class SettingsAdapter: BaseAdapter<string>
    {
        Context mContext;
        readonly LayoutInflater inflater;
        List<string> itemList;
        public SettingsAdapter(Context context, List<string> list)
        {
            inflater = LayoutInflater.FromContext(context);
            mContext = context;
            itemList = list;
        }
        public override string this[int position]
        {
            get { return itemList[position]; }
        }
        public override int Count
        {
            get { return itemList.Count; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //View view = convertView ?? inflater.Inflate(Resource.Layout.support_simple_spinner_dropdown_item, parent, false);
            View view = inflater.Inflate(Resource.Layout.city_item_layout, parent, false);
            var item = itemList[position];

            TextView tv = view.FindViewById<TextView>(Resource.Id.city_text);
            // here to set your Typeface
            FontHelper.UpdateFont(tv, FontsConstants.PN_B, (float)0.039);
            //Typeface typeFace1 = Typeface.CreateFromAsset(mContext.Assets, FontsConstants.PN_B);
            //tv.SetTypeface(typeFace1, TypefaceStyle.Normal);
            tv.Text = itemList[position];
            return view;
        }
    }
}