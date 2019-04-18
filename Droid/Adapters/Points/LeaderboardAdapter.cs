using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Points;
using BaseAdapter = SocialLadder.Droid.Adapters.Base.BaseAdapter<SocialLadder.Models.LocalModels.Points.LocalFriendModel>;

namespace SocialLadder.Droid.Adapters.Points
{
    public class LeaderboardAdapter : BaseAdapter
    {
        #region Fields
        private Context context;
        private List<LocalFriendModel> listItems;
        private int UserPosition { get; set; }
        private int VisibleItemsCount { get; set; }
        //public MvvmCross.Droid.Support.V7.AppCompat.Widget.MvxAppCompatImageView UserPhoto { get; set; }
        //public string UserPhotoUrl { get; set; }
        #endregion

        public LeaderboardAdapter(Context context, IMvxAndroidBindingContext bindingContext, MvxListView tableView) : base(context, bindingContext)
        {
            this.context = context;
            this.tableView = tableView;
            listItems = new List<LocalFriendModel>();
        }

        protected override void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsSourceCollectionChanged(sender, e);
            SetListItems();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = base.GetView(position, convertView, parent);
            UpdateControls(view);
            if (position == VisibleItemsCount - 1 && position < UserPosition)
            {             
                view.FindViewById<LinearLayout>(Resource.Id.horizontal_line).Visibility = ViewStates.Visible;
                view.FindViewById<ImageView>(Resource.Id.leaderboard_item_triangle).Visibility = ViewStates.Visible;
            }
            if (position > VisibleItemsCount)
            {
                return null;
            }
            //view.FindViewById<LinearLayout>(Resource.Id.content_layout).Click += (s, e) => {
            //    var userPhoto = (s as LinearLayout).FindViewById<MvvmCross.Droid.Support.V7.AppCompat.Widget.MvxAppCompatImageView>(Resource.Id.user_photo);
            //    if (userPhoto != null)
            //    {
            //        UserPhoto = userPhoto;
            //    }
            //};
            return view;
        }

        protected override void BindBindableView(object source, IMvxListItemView viewToUse)
        {
            if (listItems.Count == 0)
            {
                SetListItems();
            }
            var currentIndex = listItems.IndexOf(source as LocalFriendModel);
            if (currentIndex == VisibleItemsCount-1 && currentIndex < UserPosition)
            {
                source = listItems[UserPosition];
            }
            //var userPhoto = viewToUse.Content.FindViewById<MvvmCross.Droid.Support.V7.AppCompat.Widget.MvxAppCompatImageView>(Resource.Id.user_photo);
            //if (userPhoto != null)
            //{
            //    SetUserAnimatedImage(userPhoto, source as LocalFriendModel);
            //    (source as LocalFriendModel).IsSelected = false;
            //}
            base.BindBindableView(source, viewToUse);
        }

        //public void SetUserAnimatedImage(MvvmCross.Droid.Support.V7.AppCompat.Widget.MvxAppCompatImageView userPhoto, LocalFriendModel item)
        //{
        //    if (item.IsSelected)
        //    {
        //        UserPhoto = userPhoto;
        //        UserPhotoUrl = userPhoto.ImageUrl;
        //        userPhoto.ImageUrl = string.Empty;
        //        int resourceId = (int)typeof(Resource.Drawable).GetField("nerwork_loader").GetValue(null);
        //        Drawable drawable = ContextCompat.GetDrawable(Android.App.Application.Context, resourceId);
        //        userPhoto.SetImageDrawable(drawable);
        //        AnimationHelper.AnimateImage(userPhoto);
        //    }
        //    if (!item.IsSelected)
        //    {
        //        userPhoto.ClearAnimation();
        //        //userPhoto.ImageUrl = (string.IsNullOrEmpty(userPhoto.ImageUrl) && !string.IsNullOrEmpty(UserPhotoUrl)) ? UserPhotoUrl : item.ProfilePicURL;
        //        //UserPhotoUrl = string.Empty;
        //    }
        //}

        public void SetListItems()
        {
            listItems = ItemsSource.Cast<LocalFriendModel>().ToList();
            UserPosition = listItems.IndexOf(listItems.FirstOrDefault(x => x.Name.Equals("Me")));
        }

        private void UpdateControls(View view)
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_count), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.user_name), FontsConstants.PN_R, (float)0.04);
        }

        public void SetTableHeight(int cellsHeight)
        {
            UpdateTableHeight(cellsHeight);
            VisibleItemsCount = (int)Math.Round(cellsHeight / (double)DimensHelper.GetDimensById(Resource.Dimension.leaderboard_item_height));
        }

        //public void SetUserPhotoUrl()
        //{
        //    if (UserPhoto != null)
        //    {
        //        UserPhoto.ClearAnimation();
        //        UserPhoto.ImageUrl = UserPhotoUrl;
        //        UserPhoto = null;
        //        UserPhotoUrl = string.Empty;              
        //    }
        //}
    }
}