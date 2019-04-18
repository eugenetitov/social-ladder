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
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using BaseAdapter = SocialLadder.Droid.Adapters.Base.BaseAdapter<SocialLadder.Models.LocalModels.Challenges.LocalChallengeModel>;

namespace SocialLadder.Droid.Adapters.Challenges
{
    public class ChallengesTableViewAdapter : BaseAdapter
    {
        public ChallengesTableViewAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);
            var icon = view.FindViewById<ImageView>(Resource.Id.challenge_icon);
            if (icon != null)
            {
                icon.SetColorFilter(new Color(ContextCompat.GetColor(Application.Context, Resource.Color.areas_description_tex_color)), PorterDuff.Mode.SrcIn);
            }
            UpdateControls(view);
            return view;
        }

        private void UpdateControls(View view)
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.section_title), FontsConstants.PN_B, (float)0.033);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.top_text), FontsConstants.PN_B, (float)0.042);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.bottom_text), FontsConstants.PN_R, (float)0.033);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.points_text), FontsConstants.PN_R, (float)0.032);
        }
    }
}