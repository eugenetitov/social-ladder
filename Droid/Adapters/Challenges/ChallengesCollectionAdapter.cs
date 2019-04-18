using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Views.Holders;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;

namespace SocialLadder.Droid.Adapters.Challenges
{
    public class ChallengesCollectionAdapter : MvxRecyclerAdapter
    {
        MvxRecyclerView CollectionView { get; set; }

        public ChallengesCollectionAdapter(IMvxAndroidBindingContext bindingContext, MvxRecyclerView recyclerView) : base(bindingContext)
        {
            CollectionView = recyclerView;
            Source = new List<LocalChallengeTypeModel>();
        }

        private List<LocalChallengeTypeModel> Source { get; set; }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = base.OnCreateViewHolder(parent, viewType);
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, this.BindingContext.LayoutInflaterHolder);
            var view = this.InflateViewForHolder(parent, viewType, itemBindingContext);
            return new ChallengesCollectionViewHolder(view, itemBindingContext)
            {
                Click = ItemClick,
                LongClick = ItemLongClick
            };
        }

        public void SetListSource()
        {
            Source = ItemsSource.Cast<LocalChallengeTypeModel>().ToList();           
        }

        protected override void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsSourceCollectionChanged(sender, e);
            SetListSource();
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (Source.Count == 0)
            {
                SetListSource();
            }
            var item = Source[position];
            var challengesHolder = holder as ChallengesCollectionViewHolder;
            if (position == 0)
            {
                challengesHolder.SetFirstItemMargin();
            }
            if (position > 0)
            {
                challengesHolder.ResetLeftMargin();
            }
            if (item.ItemState == Enums.ChallengesCollectionItemState.Unselected || item.ItemState == Enums.ChallengesCollectionItemState.Default)
            {
                challengesHolder.SetItemUnselected();
            }
            if (item.ItemState == Enums.ChallengesCollectionItemState.Selected)
            {
                challengesHolder.SetItemSelected();
                ScrollToCenter(position);
            }
            challengesHolder.SetColor(item.Color);
            base.OnBindViewHolder(holder, position);
        }

        private void ScrollToCenter(int position)
        {
            CollectionView.ScrollToPosition(position);
        }
    }
}