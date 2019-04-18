using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Views.Holders;
using SocialLadder.Models.LocalModels.Challenges;

namespace SocialLadder.Droid.Adapters.Challenges
{
    public class PhotoGalleryCollectionAdapter : MvxRecyclerAdapter
    {
        public Action<LocalPosterModel> OnAddDescriptionToPosterHandler;
        public Action<LocalPosterModel> OnDeletePosterHandler;

        private MvxObservableCollection<LocalPosterModel> Posters { get; set; }

        private int posterPosition = 0;

        public PhotoGalleryCollectionAdapter(IMvxAndroidBindingContext bindingContext, MvxObservableCollection<LocalPosterModel> posters) : base(bindingContext)
        {
            Posters = posters;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, this.BindingContext.LayoutInflaterHolder);
            var view = this.InflateViewForHolder(parent, viewType, itemBindingContext);
            int width = (int)(parent.MeasuredWidth / 1.5);
            var layotParamethers = view.LayoutParameters;
            layotParamethers.Width = width;
            view.SetMinimumWidth(width);
            view.LayoutParameters = layotParamethers;

            if (posterPosition >= Posters.Count)
            {
                posterPosition = 0;
            }
            posterPosition++;

            var holder = new PhotoGalleryCellHolder(view, itemBindingContext);
            holder.OnAddDescriptionHandler = AddDescription;
            holder.OnDeletePhotoHandler = DeletePhoto;
            return holder;
        }

        private void AddDescription(int position)
        {
            LocalPosterModel item = (LocalPosterModel)GetItem(position);
            OnAddDescriptionToPosterHandler(item);
        }

        private void DeletePhoto(int position)
        {
            LocalPosterModel item = (LocalPosterModel)GetItem(position);
            OnDeletePosterHandler(item);
        }
    }
}