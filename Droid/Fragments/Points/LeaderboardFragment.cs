using System;
using System.ComponentModel;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Adapters.Points;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Interfaces;
using SocialLadder.Droid.Views;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Points;

namespace SocialLadder.Droid.Fragments.Points
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainActivity), FragmentContentId = Resource.Id.content_frame)]
    public class LeaderboardFragment : BaseFragment<LeaderboardViewModel>, IPointsTabFragment
    {
        protected override int FragmentId => Resource.Layout.LeaderboardLayout;
        protected override bool HasBackButton => false;
        private readonly double topLayoutConstantAspect = 0.465948;
        private MvxListView leaderboardList;
        private TextView tvOverall, tvRanked;
        private View view, collectionBackgroundView;
        private MvxAppCompatImageView userPhoto;
        private LeaderboardAdapter adapter;
        private LockableScrollView scrollView;
        private PointsScrollHelper scrollHelper;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            leaderboardList = view.FindViewById<MvxListView>(Resource.Id.leaderboard_list);
            tvOverall = view.FindViewById<TextView>(Resource.Id.tv_overall);
            tvRanked = view.FindViewById<TextView>(Resource.Id.tv_ranked);
            userPhoto = view.FindViewById<MvxAppCompatImageView>(Resource.Id.user_photo);
            collectionBackgroundView = view.FindViewById<View>(Resource.Id.collection_background_view);
            view.FindViewById<Button>(Resource.Id.earn_button).Click += (s, e) => {
                (Activity as MainActivity).SetRequiredCurrentTab(3);
            };
            adapter = new LeaderboardAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext, leaderboardList);
            leaderboardList.Adapter = adapter;
            UpdateControls();
            scrollView = view.FindViewById<LockableScrollView>(Resource.Id.scrollView);
            scrollHelper = new PointsScrollHelper(scrollView, false, GetType());
            scrollView.ScrollChange += (s, e) =>
            {
                OnScrollViewChanged(s, e);
            };

            return view;
        }

        public override void OnStop()
        {
            base.OnStop();
            scrollHelper.RemoveEvents();
        }

        public override void OnStart()
        {
            base.OnStart();
            scrollHelper.AddEvents();
        }

        public override void OnResume()
        {
            base.OnResume();
            adapter.SetTableHeight(GetHeightForList());
            adapter.ItemsSource = ViewModel.LeaderboardItems;
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.IsBusy))
            {
                if (ViewModel.IsBusy)
                {
                    ViewModel.SetRefreshImage();
                    userPhoto.ImageUrl = string.Empty;
                    AnimateImage(userPhoto);
                }
                if (!ViewModel.IsBusy)
                {
                    userPhoto.ImageUrl = ViewModel.Profile.ProfilePictureURL;
                    userPhoto.ClearAnimation();
                }
            }
            //if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.ItemSelected))
            //{
            //    if (!ViewModel.ItemSelected)
            //    {
            //        adapter.SetUserPhotoUrl();
            //    }
            //}                    
        }

        private int GetHeightForList()
        {
            var aspectSpace = Math.Round(topLayoutConstantAspect * DimensHelper.GetScreenWidth());
            var margins = (DimensHelper.GetDimensById(Resource.Dimension.leaderboard_image_margin) + DimensHelper.GetDimensById(Resource.Dimension.points_margin));
            tvOverall.Measure(0, 0);
            tvRanked.Measure(0, 0);
            var stageHeight = DimensHelper.GetScreenHeight() - (DimensHelper.GetDimensById(Resource.Dimension.toolbar_height) + DimensHelper.GetDimensById(Resource.Dimension.divider_height) * 2 +
                DimensHelper.GetDimensById(Resource.Dimension.tabbar_height) + DimensHelper.GetDimensById(Resource.Dimension.points_tabs_height));
            var textHeight = tvOverall.MeasuredHeight + tvRanked.MeasuredHeight;
            var topHeight = aspectSpace + margins + textHeight;
            var listHeight = stageHeight - topHeight;
            return (int)listHeight;
        }

        private void UpdateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<Button>(Resource.Id.earn_button), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_overall), FontsConstants.PN_B, (float)0.045);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_ranked), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_placeholder), FontsConstants.PN_R, (float)0.06);          
            ChangeBackgroundCollectionHeight((Activity as MainActivity).AreasViewIsShown);
        }

        public void ChangeBackgroundCollectionHeight(bool isShowed)
        {
            var aspectSpace = Math.Round(topLayoutConstantAspect * DimensHelper.GetScreenWidth());
            var margins = (DimensHelper.GetDimensById(Resource.Dimension.points_margin));
            var topHeight = aspectSpace + margins;
            PointsHeightHelper.UpdateBackgroundCollectionViewHeight(collectionBackgroundView, isShowed, topHeight);
        }

        protected override void AreasCollectionShow(bool isShowed)
        {
            ChangeBackgroundCollectionHeight(isShowed);
        }
    }
}