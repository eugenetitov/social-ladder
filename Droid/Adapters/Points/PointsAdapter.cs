using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Points;
using BaseAdapter = SocialLadder.Droid.Adapters.Base.BaseAdapter<SocialLadder.Models.LocalModels.Points.PointsSummaryLocalModel>;

namespace SocialLadder.Droid.Adapters.Points
{
    public class PointsAdapter : BaseAdapter
    {
        #region Fields
        private Context context;
        #endregion

        public PointsAdapter(Context context, IMvxAndroidBindingContext bindingContext, MvxListView tableView) : base(context, bindingContext)
        {
            this.context = context;
            this.tableView = tableView;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);
            var cellsHeight = (int)Math.Round((double)DimensHelper.GetDimensById(Resource.Dimension.leaderboard_item_height) + DimensHelper.GetDimensById(Resource.Dimension.divider_height)) * ItemsSource.Cast<PointsSummaryLocalModel>().Count();
            UpdateTableHeight(cellsHeight);
            UpdateControls(view);
            return view;
        }

        public void ResetTableViewHeight()
        {
            //UpdateTableHeight(0);
        }

        //protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        //{
        //    base.BindBindableView(source, viewToUse);
        //    (context as Activity).RunOnUiThread(() => {
        //        var cellsHeight = (int)Math.Round((60 + 1) * Android.Content.Res.Resources.System.DisplayMetrics.Density) * ItemsSource.Cast<PointsSummaryLocalModel>().Count();
        //        UpdateTableHeight(cellsHeight);
        //    });
        //    UpdateControls(viewToUse.Content);
        //}

        private void UpdateControls(View view)
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.top_text), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.bottom_text), FontsConstants.PN_R, (float)0.04);
        }
    }
}