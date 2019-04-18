using System;
using System.ComponentModel;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
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
    public class TransactionsFragment : BaseFragment<TransactionsViewModel>, IPointsTabFragment
    {
        protected override int FragmentId => Resource.Layout.TransactionsLayout;
        protected override bool HasBackButton => false;
        private readonly double topLayoutConstantAspect = 0.73206;
        ImageView scoreImage;
        private View view, collectionBackgroundView;
        private LockableScrollView scrollView;
        private PointsScrollHelper scrollHelper;
        private MvxListView transactionsList;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            transactionsList = view.FindViewById<MvxListView>(Resource.Id.transactions_list);
            scoreImage = view.FindViewById<ImageView>(Resource.Id.score_image);
            collectionBackgroundView = view.FindViewById<View>(Resource.Id.collection_background_view);
            view.FindViewById<Button>(Resource.Id.see_em_button).Click += (s, e) => { (Activity as MainActivity).SetRequiredCurrentTab(2); };
            view.FindViewById<Button>(Resource.Id.do_more_button).Click += (s, e) => { (Activity as MainActivity).SetRequiredCurrentTab(3); };
            var bottomBackgroundLayout = view.FindViewById<LinearLayout>(Resource.Id.bottom_white_background);
            transactionsList.Adapter = new TransactionsAdapter(this.Activity, (IMvxAndroidBindingContext)BindingContext, transactionsList, bottomBackgroundLayout);
            scrollView = view.FindViewById<LockableScrollView>(Resource.Id.scrollView);
            scrollHelper = new PointsScrollHelper(scrollView, false, GetType());
            UpdateControls();

            scrollView.ScrollChange += async (s, e) =>
            {
                OnScrollViewChanged(s, e);
                if (scrollView.GetChildAt(0).Height - scrollView.Height <= scrollView.ScrollY)
                {
                    if (!ViewModel.IsLoadingMore && !ViewModel.IsBusy)
                    {
                        await ViewModel.LoadMoreTransactions();
                    }
                }
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
        }

        private void UpdateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.tv_superfan), FontsConstants.PN_B, (float)0.045);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.rewards_count), FontsConstants.PN_R, (float)0.032);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.challenges_count), FontsConstants.PN_R, (float)0.032);

            FontHelper.UpdateFont(view.FindViewById<Button>(Resource.Id.see_em_button), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<Button>(Resource.Id.do_more_button), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.score_count), FontsConstants.PN_B, (float)0.06);

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