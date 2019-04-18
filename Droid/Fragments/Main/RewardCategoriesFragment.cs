using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Felipecsl.GifImageViewLib;
using Java.Lang;
using Java.Lang.Reflect;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Adapters;
using SocialLadder.Droid.Adapters.Rewards;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Rewards;
using System.ComponentModel;

namespace SocialLadder.Droid.Fragments.Main
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainActivity), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame)]
    public class RewardCategoriesFragment : BaseFragment<RewardCategoriesViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_reward_categories;
        protected override bool HasBackButton => false;

        private MvxRecyclerView _rewardRecyclerView;
        private GifImageView gifImageView;
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.EnsureBindingContextIsSet(inflater);
            view = this.BindingInflate(FragmentId, container, false);

            _rewardRecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.reward_category_collection);
            _rewardRecyclerView.SetLayoutManager(new GridLayoutManager(Activity, 2, LinearLayoutManager.Vertical, false));
            _rewardRecyclerView.Adapter = new RewardsCategoryAdapter((IMvxAndroidBindingContext)BindingContext);
            UpdateControls();
            SetPlaceholder();
            var _refreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            _refreshLayout.Refresh += (s, e) => { ViewModel.RefreshCommand.Execute(); };
            var set = this.CreateBindingSet<RewardCategoriesFragment, RewardCategoriesViewModel>();
            set.Bind(_refreshLayout).For(v => v.Refreshing).To(viewModel => viewModel.IsRefreshing);
            set.Apply();
            CustomSwipeRefreshLayoutHelper.Customize(_refreshLayout);
            _rewardRecyclerView.ScrollChange += (s, e) =>
            {
                OnScrollViewChanged(s, e, _rewardRecyclerView.ScrollState);
            };
            return view;
        }

        private void SetPlaceholder()
        {
            gifImageView = view.FindViewById<GifImageView>(Resource.Id.gif_View);
            var bytes = GifViewHelper.ReadAllBytes(Resources.OpenRawResource(Resource.Drawable.rewards_placeholder));
            gifImageView.SetBytes(bytes);
        }

        private void UpdateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.ph_title), FontsConstants.PN_B, (float)0.09);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.ph_text), FontsConstants.PN_R, (float)0.04);
        }

        public override void OnStart()
        {
            base.OnStart();
            gifImageView.StartAnimation();
        }

        public override void OnStop()
        {
            base.OnStop();
            gifImageView.StopAnimation();
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.PlaceholderHidden))
            {
                if (ViewModel.PlaceholderHidden)
                {
                    gifImageView.StartAnimation();
                }
                if (!ViewModel.PlaceholderHidden)
                {
                    gifImageView.StopAnimation();
                }
            }
        }
    }
}