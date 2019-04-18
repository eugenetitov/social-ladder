using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Areas;

namespace SocialLadder.Droid.Views.Holders
{
    public class AreasCollectionViewHolder : MvxRecyclerViewHolder
    {
        private readonly MvvmCross.Droid.Support.V7.AppCompat.Widget.MvxAppCompatImageView image;
        public AreasCollectionViewHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            image = itemView.FindViewById<MvvmCross.Droid.Support.V7.AppCompat.Widget.MvxAppCompatImageView>(Resource.Id.imageView);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.title), FontsConstants.PN_R, (float)0.03);
        }

        public void SetDefaultImage()
        {
            image.SetImageResource(Resource.Drawable.add_area_image);
        }
    }
}