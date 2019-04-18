using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Lang.Reflect;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Adapters;
using SocialLadder.Droid.Adapters.Rewards;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.ViewModels.Main;
using SocialLadder.ViewModels.Rewards;
using static Android.Support.V4.Widget.SwipeRefreshLayout;

namespace SocialLadder.Droid.Fragments.Rewards
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class RewardsFragment : BaseFragment<RewardsViewModel>
    {  
        protected override int FragmentId => Resource.Layout.rewards_activity_layout;
        protected override bool HasBackButton => true;

        private SwipeRefreshLayout _refreshLayout;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        { 
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.category_text), FontsConstants.PN_R, 0.035f);

            var recyvlerView = view.FindViewById<MvxRecyclerView>(Resource.Id.reward_collection);
            recyvlerView.Adapter = (new RewardsAdapter((IMvxAndroidBindingContext)BindingContext));
            _refreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);

            var set = this.CreateBindingSet<RewardsFragment, RewardsViewModel>();
            set.Bind(_refreshLayout).For(v => v.Refreshing).To(viewModel => viewModel.IsRefreshing);
            set.Apply();
            _refreshLayout.Refresh += (sender, e) => ViewModel.RefreshCommand.Execute();
            
            if (_refreshLayout != null)
            {
                _refreshLayout.SetColorSchemeResources(Resource.Color.color_transparent);
                _refreshLayout.SetProgressBackgroundColor(Resource.Color.color_transparent);
                _refreshLayout.SetSize(0);
                try
                {
                    var field = _refreshLayout.Class.GetDeclaredField("mCircleView");
                    AccessibleObject[] array = new AccessibleObject[1];
                    array[0] = field;
                    AccessibleObject.SetAccessible(array, true);
                    var _refreshImageView = (ImageView)field.Get(_refreshLayout);
                    _refreshImageView.Elevation = 0;
                    return view;
                }
                catch (NoSuchFieldException e)
                {
                    e.PrintStackTrace();
                }
                catch (IllegalAccessException e)
                {
                    e.PrintStackTrace();
                }
            }
            return view;
        }
    }
}