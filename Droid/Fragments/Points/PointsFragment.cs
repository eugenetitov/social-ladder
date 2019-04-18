using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Core;
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
    public class PointsFragment : BaseFragment<PointsViewModel>, IPointsTabFragment
    {
        protected override int FragmentId => Resource.Layout.PointsLayout;
        protected override bool HasBackButton => false;
        private readonly double topLayoutConstantAspect = 0.73107;
        private View view, collectionBackgroundView;
        private double ScoreFill { get; set; }
        private ImageView scoreImage;
        private LockableScrollView scrollView;
        private PointsScrollHelper scrollHelper;
        private PointsAdapter pointsAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            scoreImage = view.FindViewById<ImageView>(Resource.Id.score_image);
            var pointsList = view.FindViewById<MvxListView>(Resource.Id.points_list);
            collectionBackgroundView = view.FindViewById<View>(Resource.Id.collection_background_view);
            pointsAdapter = new PointsAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext, pointsList);
            pointsList.Adapter = pointsAdapter;
            GetScoreFill();
            UpdateControls();
            scrollView = view.FindViewById<LockableScrollView>(Resource.Id.scrollView);
            scrollHelper = new PointsScrollHelper(scrollView, false, GetType());
            scrollView.ScrollChange += (s, e) =>
            {
                OnScrollViewChanged(s, e);
            };
            return view;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
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
            var triangle = view.FindViewById<TriangePointsView>(Resource.Id.triangle);
            var popupTriangle = view.FindViewById<TriangePointsView>(Resource.Id.popup_Triangle);
            triangle.SetParameters(ScoreFill, GetChartLayoutWidth(), triangle.LayoutParameters.Height);
            popupTriangle.SetParameters(ScoreFill, GetChartPopupLayoutWidth(0, 2), triangle.LayoutParameters.Height);
            triangle.Invalidate();
            popupTriangle.Invalidate();
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.IsBusy))
            {
                if (ViewModel.IsBusy)
                {
                    ViewModel.SetRefreshImage();
                    AnimateImage(scoreImage);
                }
                if (!ViewModel.IsBusy)
                {
                    scoreImage.ClearAnimation();
                }
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.DetailsViewHidden))
            {
                (Activity as MainActivity).ChangeShieldViewsVisibility(ViewModel.DetailsViewHidden);
            }
        }

        private void GetScoreFill()
        {
            ScoreFill = ViewModel.Profile != null ? (float)ViewModel.Profile.OverallSLScore / 100.0f : 0.0f;
        }

        private float GetChartLayoutWidth()
        {
            var roundImage = view.FindViewById<ImageView>(Resource.Id.cricle_image);
            roundImage.Measure(0, 0);
            var imageWidth = roundImage.MeasuredWidth;
            return GetChartPopupLayoutWidth(imageWidth, 1);
        }

        private float GetChartPopupLayoutWidth(int imageWidth, int count)
        {
            var margins = (DimensHelper.GetDimensById(Resource.Dimension.points_margin) * count);
            var width = Android.Content.Res.Resources.System.DisplayMetrics.WidthPixels - (imageWidth + margins);
            return width;
        }

        private void UpdateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.popup_title_text), FontsConstants.PN_B, (float)0.07);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.popup_description_text), FontsConstants.PN_R, (float)0.05);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_superfan), FontsConstants.PN_B, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.score_count), FontsConstants.PN_B, (float)0.06);
            ChangeBackgroundCollectionHeight((Activity as MainActivity).AreasViewIsShown);
        }

        public void ChangeBackgroundCollectionHeight(bool isShowed)
        {
            var aspectSpace = Math.Round(topLayoutConstantAspect * DimensHelper.GetScreenWidth());
            var margins = (DimensHelper.GetDimensById(Resource.Dimension.points_margin));
            var topHeight = aspectSpace + margins;
            if (collectionBackgroundView == null)
            {
                view = this.BindingInflate(FragmentId, null, true);
                collectionBackgroundView = view.FindViewById<View>(Resource.Id.collection_background_view);
            }
            PointsHeightHelper.UpdateBackgroundCollectionViewHeight(collectionBackgroundView, isShowed, topHeight);
        }

        protected override void AreasCollectionShow(bool isShowed)
        {
            ChangeBackgroundCollectionHeight(isShowed);
        }
    }
}