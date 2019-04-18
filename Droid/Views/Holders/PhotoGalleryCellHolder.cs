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
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;

namespace SocialLadder.Droid.Views.Holders
{
    public class PhotoGalleryCellHolder : MvxRecyclerViewHolder
    {
        public Action<int> OnAddDescriptionHandler;
        public Action<int> OnDeletePhotoHandler;
        private TextView counter;

        public PhotoGalleryCellHolder(View itemView, IMvxAndroidBindingContext context) : base(itemView, context)
        {
            var button = itemView.FindViewById<Button>(Resource.Id.btnAddCaption);
            var editButton = itemView.FindViewById<Button>(Resource.Id.btnEditCaption);
            var deleteButton = itemView.FindViewById<ImageButton>(Resource.Id.btnDelete);
            counter = itemView.FindViewById<TextView>(Resource.Id.txtNumber);

            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.txtCaption), FontsConstants.PN_R, (float)0.038);
            FontHelper.UpdateFont(counter, FontsConstants.PN_R, (float)0.035);
            FontHelper.UpdateFont(button, FontsConstants.PN_R, (float)0.038);
            FontHelper.UpdateFont(editButton, FontsConstants.PN_R, (float)0.03);
            FontHelper.UpdateFont(itemView.FindViewById<TextView>(Resource.Id.error_location_text), FontsConstants.PN_R, (float)0.035);

            button.Click += (sender, e) => OnAddDescriptionHandler(AdapterPosition);
            editButton.Click += (sender, e) => OnAddDescriptionHandler(AdapterPosition);
            deleteButton.Click += (sender, e) => OnDeletePhotoHandler(AdapterPosition);
        }
    }
}