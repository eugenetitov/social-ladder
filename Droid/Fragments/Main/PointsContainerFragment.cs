using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.Navigation;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Fragments.Points;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Views;
using SocialLadder.Interfaces;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Points;
using System.Collections.Generic;

namespace SocialLadder.Droid.Fragments.Main
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainActivity), FragmentContentId = Resource.Id.content_frame)]
    public class PointsContainerFragment : BaseFragment<PointsContainerViewModel>, ViewPager.IOnPageChangeListener, TabLayout.IOnTabSelectedListener
    {
        protected override int FragmentId => Resource.Layout.fragment_points;
        protected override bool HasBackButton => false;
        private UnscrollingViewPager viewPager;
        private List<MvxViewPagerFragmentInfo> fragments;
        private TabLayout tabLayout;
        private MvxCachingFragmentStatePagerAdapter pagerAdapter;
        private readonly string[] tabTitles = new string[] { "Points & Stats", "Leaderboard", "Transactions" };

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            viewPager = view.FindViewById<UnscrollingViewPager>(Resource.Id.points_view_pager);
            viewPager.SetPagingEnabled(true);
            PointsSmoothViewPagerHelper.Customize(viewPager);
            if (viewPager != null)
            {
                fragments = new List<MvxViewPagerFragmentInfo>
                {
                     new MvxViewPagerFragmentInfo(tabTitles[0], typeof(PointsFragment), (Activity as MainActivity).vmHelper.PointsVM),
                     new MvxViewPagerFragmentInfo(tabTitles[1], typeof(LeaderboardFragment), (Activity as MainActivity).vmHelper.LeaderboardVM),
                     new MvxViewPagerFragmentInfo(tabTitles[2], typeof(TransactionsFragment), (Activity as MainActivity).vmHelper.TransactionsVM),
                };
                pagerAdapter = new MvxCachingFragmentStatePagerAdapter(Activity, this.ChildFragmentManager, fragments);

                viewPager.Adapter = pagerAdapter;
                tabLayout = view.FindViewById<TabLayout>(Resource.Id.points_tabs);
                tabLayout.SetupWithViewPager(viewPager);
                viewPager.AddOnPageChangeListener(this);

                tabLayout.SetupWithViewPager(viewPager);
                for (int i = 0; i < tabLayout.TabCount; i++)
                {
                    TabLayout.Tab tab = tabLayout.GetTabAt(i);
                    tab.SetCustomView(GetTabView(i));
                }
                tabLayout.AddOnTabSelectedListener(this);
                TabHighlited(tabLayout.GetTabAt(0));
            }
            ViewModel.AddCurrentVM += ViewModel_AddCurrentVM;

            (Activity as MainActivity).vmHelper.PointsVM.DetailsViewOpened += (s, e) => {
                viewPager.SetPagingEnabled(!e);
            };
            return view;
        }

        private void ViewModel_AddCurrentVM(object sender, System.EventArgs e)
        {
            ViewModel.SetCurrentVM(pagerAdapter.FragmentsInfo[viewPager.CurrentItem].ViewModel);
            (pagerAdapter.FragmentsInfo[viewPager.CurrentItem].ViewModel as BasePointsViewModel).UpdateFromView(true);
            if ((int)sender > 2)
            {
                ViewModel.AddCurrentVM -= ViewModel_AddCurrentVM;
            }
        }

        #region TabsChanges
        public View GetTabView(int position)
        {
            View view = LayoutInflater.From(this.Activity).Inflate(Resource.Layout.custom_points_tab_layout, null);
            TextView tabText = (TextView)view.FindViewById<TextView>(Resource.Id.textView);
            var item = tabTitles[position];
            tabText.SetText(item, TextView.BufferType.Normal);
            FontHelper.UpdateFont(tabText, FontsConstants.PN_R, (float)0.04);
            view.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            return view;
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {

        }

        public void OnTabSelected(TabLayout.Tab tab)
        {
            TabHighlited(tab);
        }

        private void TabHighlited(TabLayout.Tab tab)
        {
            Activity.RunOnUiThread(() =>
            {
                var textview = tab.CustomView.FindViewById<TextView>(Resource.Id.textView);
                var btmLine = tab.CustomView.FindViewById<LinearLayout>(Resource.Id.bottom_line);

                textview.SetTextColor(ContextCompat.GetColorStateList(Activity, Resource.Color.tab_selected_color));
                btmLine.SetBackgroundColor(new Color(ContextCompat.GetColor(Activity, Resource.Color.tab_selected_color)));
                btmLine.Visibility = ViewStates.Visible;
            });
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
            Activity.RunOnUiThread(() =>
            {
                var textview = tab.CustomView.FindViewById<TextView>(Resource.Id.textView);
                var btmLine = tab.CustomView.FindViewById<LinearLayout>(Resource.Id.bottom_line);

                textview.SetTextColor(ContextCompat.GetColorStateList(Activity, Resource.Color.areas_description_tex_color));
                btmLine.SetBackgroundColor(new Color(ContextCompat.GetColor(Activity, Resource.Color.areas_description_tex_color)));
                btmLine.Visibility = ViewStates.Invisible;
            });
        }
        #endregion

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public async void OnPageSelected(int position)
        {
            if ((Activity as MainActivity).ViewModel.AreaCollectionHidden)
            {
                MainActivity.PointsScrolling(null, null);
            }
            ViewModel.SetCurrentVM(pagerAdapter.FragmentsInfo[position].ViewModel);
            (pagerAdapter.FragmentsInfo[position].ViewModel as BasePointsViewModel).UpdateFromView();
            (Activity as MainActivity).CloseAreaViewWithAnim();
        }
    }
}