using System;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Lang.Reflect;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Core;
using SocialLadder.Droid.Adapters.Notifications;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.ViewModels.Main;
using SocialLadder.ViewModels.Notification;

namespace SocialLadder.Droid.Fragments.Norifications
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class NotificationFragment : BaseFragment<NotificationsViewModel>
    {
        protected override int FragmentId => Resource.Layout.NotificationView;

        protected override bool HasBackButton => true;

        private IMvxInteraction<int> _expandNotificationGroup;
        public IMvxInteraction<int> ExpandNotificationGroup
        {
            get => _expandNotificationGroup;
            set
            {
                if (_expandNotificationGroup != null)
                    _expandNotificationGroup.Requested -= OnExpandNotificationGroup;

                _expandNotificationGroup = value;
                _expandNotificationGroup.Requested += OnExpandNotificationGroup;
            }
        }

        private MvxExpandableListView _listView;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            var adapter = new NotificationMvxExpandableListAdapter(this.Context, (IMvxAndroidBindingContext)BindingContext);
            adapter.ColapseItemHandler = (position) =>
            {
                _listView.CollapseGroup(position);
            };
            _listView = view.FindViewById<MvxExpandableListView>(Resource.Id.mvxExpandableListView); 
            _listView.SetAdapter(adapter);
            var _refreshLayout = view.FindViewById<SwipeRefreshLayout>(Resource.Id.swipe_refresh_layout);
            _refreshLayout.Refresh += (s, e) => { ViewModel.RefreshCommand.Execute(); };
            var set = this.CreateBindingSet<NotificationFragment, NotificationsViewModel>();
            set.Bind(this).For(v => v.ExpandNotificationGroup).To(viewModel => viewModel.GroupSelectedInteractions).OneWay();
            set.Bind(_refreshLayout).For(v => v.Refreshing).To(viewModel => viewModel.IsRefreshing);
            set.Apply();
            UpdateFonts(view);
            CustomSwipeRefreshLayoutHelper.Customize(_refreshLayout);
            return view;
        }

        private void UpdateFonts(View view)
        {
            //FontHelper.UpdateFont(view.FindViewById<Button>(Resource.Id.close_button), FontsConstants.PN_B, (float)0.03);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.title_text), FontsConstants.PN_B, (float)0.03);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.close_button_text), FontsConstants.PN_B, (float)0.03);
        }

        public void OnExpandNotificationGroup(object sender, MvxValueEventArgs<int> position)
        {
            _listView.ExpandGroup(position.Value);
        }

    }
}
