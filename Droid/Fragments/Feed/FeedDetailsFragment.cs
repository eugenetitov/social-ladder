using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Core;
using SocialLadder.Droid.Adapters;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Delegates;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Views.Holders;
using SocialLadder.Models;
using SocialLadder.ViewModels.Feed;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Feed
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class FeedDetailsFragment : BaseFragment<FeedDetailsViewModel>
    {
        protected override int FragmentId => Resource.Layout.FeedDetailsLayout;
        protected override bool HasBackButton => true;
        private FeedAdapter _feedAdapter;
        private FeedDelegate _adapterDelegate;
        private View view;
        private ImageView _feedLoader;

        private IMvxInteraction<UpdatedFeedItemModel> _feedUpdateInteraction;
        public IMvxInteraction<UpdatedFeedItemModel> FeedUpdateInteraction
        {
            get => _feedUpdateInteraction;
            set
            {
                if (_feedUpdateInteraction != null)
                    _feedUpdateInteraction.Requested -= OnFeedInteractionRequested;

                _feedUpdateInteraction = value;
                _feedUpdateInteraction.Requested += OnFeedInteractionRequested;
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            _feedLoader = view.FindViewById<ImageView>(Resource.Id.feed_loading_indicator_image);
            _feedAdapter = new FeedAdapter((IMvxAndroidBindingContext)BindingContext);
            _adapterDelegate = new FeedDelegate(ViewModel);
            _feedAdapter.Delegate = _adapterDelegate;
            var list = view.FindViewById<MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView>(Resource.Id.feedRecycler);
            list.SetLayoutManager(new LinearLayoutManager(Context));
            list.Adapter = _feedAdapter;
            var _refreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            _refreshLayout.Refresh += (s, e) => { ViewModel.RefreshCommand.Execute(); };
            this.EnsureBindingContextIsSet(inflater);
            var set = this.CreateBindingSet<FeedDetailsFragment, FeedDetailsViewModel>();
            set.Bind(this).For(v => v.FeedUpdateInteraction).To(viewModel => viewModel.FeedItemUpdateInteraction).OneWay();
            set.Bind(_refreshLayout).For(v => v.Refreshing).To(viewModel => viewModel.IsRefreshing);
            set.Apply();
            UndateControls();
            CustomSwipeRefreshLayoutHelper.Customize(_refreshLayout);
            return view;
        }

        private void OnFeedInteractionRequested(object sender, MvxValueEventArgs<UpdatedFeedItemModel> eventArgs)
        {
            if (eventArgs.Value == null)
            {
                _adapterDelegate.SetAnimatedImage();
                return;
            }
            if (eventArgs.Value != null && eventArgs.Value.LoaderMode != Enums.FeedLoadingIndicatorMode.Default)
            {
                CheckLoaderIndicatorMode(eventArgs.Value.LoaderMode);
                return;
            }
            var updatedFeedModel = eventArgs.Value;
            var updatedItem = updatedFeedModel.UpdatedFeedItem;
            var oldItem = updatedFeedModel.OldFeedItem;
            var list = this.View.FindViewById<MvvmCross.Droid.Support.V7.RecyclerView.MvxRecyclerView>(Resource.Id.feedRecycler);
            var itemIndex = _feedAdapter.GetItemPosition(updatedFeedModel.OldFeedItem);
            var holder = list.FindViewHolderForPosition(itemIndex) as FeedCellHolder;
            if (holder != null)
            {
                holder.UpdateLike(updatedItem);
                if ((updatedItem.FilteredEngagementList != null) && (!updatedItem.FilteredEngagementList.Equals(oldItem.FilteredEngagementList)))
                {
                    holder.ShowComments(updatedItem.FilteredEngagementList);
                    return;
                }

                //holder.UpdateLike(updatedItem);
            }
        }

        public void CheckLoaderIndicatorMode(Enums.FeedLoadingIndicatorMode loaderMode)
        {
            if (loaderMode == Enums.FeedLoadingIndicatorMode.NeedShow)
            {
                AnimateImage(_feedLoader);
            }
            if (loaderMode == Enums.FeedLoadingIndicatorMode.NeedHide)
            {
                _feedLoader.ClearAnimation();
            }
        }

        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.profile_name), FontsConstants.PN_B, (float)0.0395);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.profile_info), FontsConstants.PN_B, (float)0.037);
        }
    }
}