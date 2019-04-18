using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
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
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models.LocalModels.Challenges;
using BaseAdapter = SocialLadder.Droid.Adapters.Base.BaseAdapter<SocialLadder.Models.LocalModels.Challenges.LocalChallengeAnswerModel>;

namespace SocialLadder.Droid.Adapters.Challenges
{
    public class ChallengesMultipleChoiseAdapter : BaseAdapter
    {
        #region Fields
        private Context context;
        #endregion

        public ChallengesMultipleChoiseAdapter(Context context, IMvxAndroidBindingContext bindingContext, MvxListView tableView) : base(context, bindingContext)
        {
            this.context = context;
            this.tableView = tableView;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);
            (context as Activity).RunOnUiThread(() => {
                UpdateTableHeight((DimensHelper.GetDimensById(Resource.Dimension.points_tabs_height)) * ItemsSource.Cast<LocalChallengeAnswerModel>().Count()) ;
            });
            UpdateControls(view);
            return view;
        }

        protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        {
            if (viewToUse != null && viewToUse.Content != null && source != null)
            {
                viewToUse.Content.FindViewById<RelativeLayout>(Resource.Id.image_view).Visibility = ((source as LocalChallengeAnswerModel).IsSelected ? ViewStates.Visible : ViewStates.Gone);
            }           
            base.BindBindableView(source, viewToUse);
        }

        private void UpdateControls(View view)
        {
            view.FindViewById<ImageView>(Resource.Id.mc_icon).SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(context, Resource.Color.textIcon)), PorterDuff.Mode.SrcIn);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.title), FontsConstants.PN_B, (float)0.035);
        }
    }
}