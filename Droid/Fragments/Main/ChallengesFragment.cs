using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Felipecsl.GifImageViewLib;
using Java.Lang;
using Java.Lang.Reflect;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Adapters;
using SocialLadder.Droid.Adapters.Challenges;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Helpers.ChallengesCollectionScrollHelper;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Challenges;
using System;
using System.ComponentModel;
using System.IO;

namespace SocialLadder.Droid.Fragments.Main
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainActivity), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class ChallengesFragment : BaseFragment<ChallengeViewModel>
    {
        protected override int FragmentId => Resource.Layout.fragment_challenges;
        protected override bool HasBackButton => false;
        private MvxRecyclerView collectionView;
        private ChallengesCollectionAdapter collectionViewAdapter;
        private MvxListView tableView;
        private ChallengesTableViewAdapter tableAdapter;
        private GifImageView gifImageView;
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            collectionView = view.FindViewById<MvxRecyclerView>(Resource.Id.challenges_collection);
            collectionViewAdapter = new ChallengesCollectionAdapter((IMvxAndroidBindingContext)BindingContext, collectionView);
            collectionView.Adapter = collectionViewAdapter;
            collectionView.SetLayoutManager(new ChallengesLinearLayoutManager(this.Activity, LinearLayoutManager.Horizontal, false));
            collectionView.HasFixedSize = true;
            collectionView.ScrollChange += (s, e) =>
            {
                OnScrollViewChanged(s, e, collectionView.ScrollState);
            };

            tableView = view.FindViewById<MvxListView>(Resource.Id.challenges_list);
            tableAdapter = new ChallengesTableViewAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext);
            tableView.Adapter = tableAdapter;
            tableView.ScrollStateChanged += (s, e) =>
            {
                if (e.ScrollState == ScrollState.TouchScroll)
                {
                    OnScrollViewChanged(s, e, 1);
                }
            };
            //tableView.ScrollChange += (s, e) =>
            //{
            //    OnScrollViewChanged(s, e);
            //};
            SetPlaceholder();
            UpdateControls();
            var _refreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            _refreshLayout.Refresh += (s, e) => 
            {
                ViewModel.RefreshCommand.Execute();
            };
            var set = this.CreateBindingSet<ChallengesFragment, ChallengeViewModel>();
            set.Bind(_refreshLayout).For(v => v.Refreshing).To(viewModel => viewModel.IsRefreshing);
            set.Apply();
            CustomSwipeRefreshLayoutHelper.Customize(_refreshLayout);
            return view;
        }

        private void SetPlaceholder()
        {
             gifImageView = view.FindViewById<GifImageView>(Resource.Id.gif_View);
            var bytes = GifViewHelper.ReadAllBytes(Resources.OpenRawResource(Resource.Drawable.challenges_placeholder));
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