using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Binding.ExtensionMethods;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Points;
using BaseAdapter = SocialLadder.Droid.Adapters.Base.BaseAdapter<SocialLadder.Models.LocalModels.Points.TransactionsLocalModel>;

namespace SocialLadder.Droid.Adapters.Points
{
    public class TransactionsAdapter : BaseAdapter
    {
        #region Fields
        private Context context;
        LinearLayout bottomBackgroundView;
        #endregion

        public TransactionsAdapter(Context context, IMvxAndroidBindingContext bindingContext, MvxListView tableView, LinearLayout bottomBackgroundView) : base(context, bindingContext)
        {
            this.context = context;
            this.tableView = tableView;
            this.bottomBackgroundView = bottomBackgroundView;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);

            (context as Activity).RunOnUiThread(() =>
            {
                var cellsHeight = (int)Math.Round((double)DimensHelper.GetDimensById(Resource.Dimension.leaderboard_item_height)) * ItemsSource.Cast<TransactionsLocalModel>().Count();
                UpdateTableHeight(cellsHeight);
                UpdateBackgroundViewHeight(cellsHeight, bottomBackgroundView, 5);
            });

            view.FindViewById<ImageView>(Resource.Id.icon).SetColorFilter(new Color(ContextCompat.GetColor(context, Resource.Color.areas_description_tex_color)), PorterDuff.Mode.SrcIn);

            UpdateControls(view);
            return view;
        }

        private void UpdateControls(View view)
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_date), FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.points_text), FontsConstants.PN_B, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.transaction_text), FontsConstants.PN_R, (float)0.03);
        }
    }
}