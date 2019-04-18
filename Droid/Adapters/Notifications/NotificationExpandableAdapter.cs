using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.ExtensionMethods;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid.Adapters.Notifications
{
    public class NotificationMvxExpandableListAdapter : MvxExpandableListAdapter, IExpandableListAdapter
    {
        public NotificationMvxExpandableListAdapter(Context context)
            : base(context)
        {
        }

        public NotificationMvxExpandableListAdapter(Context context, IMvxAndroidBindingContext bindingContext)
            : base(context, bindingContext)
        {
        }

        protected NotificationMvxExpandableListAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public Action<int> ColapseItemHandler;

        private int _expandedItemIndex = 0;
        public override void OnGroupExpanded(int groupPosition)
        {
            if (_expandedItemIndex != groupPosition)
            {
                ColapseItemHandler?.Invoke(_expandedItemIndex);
            }
            _expandedItemIndex = groupPosition;
            base.OnGroupExpanded(groupPosition);
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var item = GetRawGroup(groupPosition);
            var view = GetBindableView(convertView, item, parent, Resource.Layout.notification_group_cell);
            SetupFontsForGroupCell(view); 
            return view;
        }
  
        public override int GetChildrenCount(int groupPosition)
        {
            return base.GetChildrenCount(groupPosition);
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var item = GetRawItem(groupPosition, childPosition);

            return GetBindableView(convertView, item, parent, Resource.Layout.notification_item_cell);
        }

        public void SetupFontsForGroupCell(View groupView)
        {
            FontHelper.UpdateFont(groupView.FindViewById<TextView>(Resource.Id.area_name_text), FontsConstants.PN_B, (float)0.06);
            FontHelper.UpdateFont(groupView.FindViewById<TextView>(Resource.Id.notification_count_text), FontsConstants.PN_R, (float)0.025);
        }
    }
}