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
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using SocialLadder.Droid.Activities.BaseActivity;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid.Adapters.Base
{
    public class BaseAdapter<T> : MvxAdapter where T : class
    {
        protected T Item;
        public MvxListView tableView;

        public BaseAdapter(Context context) : base(context)
        {

        }

        public BaseAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        protected BaseAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        {
            base.BindBindableView(source as T, viewToUse);
        }

        public void UpdateTableHeight(int height)
        {
            var paramList = tableView.LayoutParameters;
            paramList.Height = height;
            tableView.LayoutParameters = paramList;
            tableView.RequestLayout();
            this.NotifyDataSetChanged();
            tableView.Invalidate();
        }

        public void UpdateBackgroundViewHeight(int height, LinearLayout layout, int aspect = 1)
        {
            var paramList = layout.LayoutParameters;
            paramList.Height = height - GetUnderViewHeight(aspect); ;
            layout.LayoutParameters = paramList;
            layout.RequestLayout();
            layout.Invalidate();
        }

        private int GetUnderViewHeight(int aspect)
        {
            var width = Android.Content.Res.Resources.System.DisplayMetrics.WidthPixels;
            var height = (double)(width / aspect);
            return (int)Math.Round(height);
        }
    }
}