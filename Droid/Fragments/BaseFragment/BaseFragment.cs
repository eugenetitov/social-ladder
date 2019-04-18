using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Helpers;
using SocialLadder.Extensions;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.ViewModels.Base;
using static Android.Support.V4.Widget.NestedScrollView;

namespace SocialLadder.Droid.Fragments.BaseFragment
{
    public abstract class BaseFragment<TViewModel> : MvxFragment<TViewModel>, View.IOnTouchListener where TViewModel : MvxViewModel, IMvxViewModel, IMvxNotifyPropertyChanged
    {
        #region Properties
        protected ImageView cycle;
        protected abstract int FragmentId { get; }
        protected virtual bool GoogleLogin { get; } = false;
        protected MvxAsyncCommand<string> GoogleSigninCommand;
        private MvxSwipeRefreshLayout refresheLayout;
        protected ImageView loader;
        protected View BusyView;
        protected abstract bool HasBackButton { get; }
        protected Android.Support.V7.Widget.Toolbar Toolbar { get; set; }
        private ScrollView scrollView;

        public MvxAppCompatActivity ParentActivity
        {
            get
            {
                return (MvxAppCompatActivity)Activity;
            }
        }

        public new TViewModel ViewModel { get => base.ViewModel as TViewModel; }

        protected const short GoogleIntentId = 0x6673;
        #endregion

        #region Create view
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(FragmentId, null, true);
            refresheLayout = view.FindViewById<MvxSwipeRefreshLayout>(Resource.Id.refresher);
            view.Click += (s, e) =>
            {
                HideKeyboardView(false);
            };
            loader = view.FindViewById<ImageView>(Resource.Id.loading_indicator_image);
            Toolbar = view.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (Toolbar != null && Toolbar.Visibility != ViewStates.Gone)
            {
                //if ((ParentActivity as MainActivity) != null)
                //{
                //    (ParentActivity as MainActivity).ViewModel.BackButtonHidden = HasBackButton;
                //}
                (ViewModel as BaseViewModel).BackButtonHidden = HasBackButton;
                UpdateToolbarElements(view);
                //var backButton = Toolbar.FindViewById<ImageButton>(Resource.Id.backArrow_image);
                //if (backButton != null)
                //{
                //    backButton.Click += (s, e) =>
                //    {
                //        //if (ViewModel is IMainTabViewModel && ParentActivity != null)
                //        //{
                //        //    (ParentActivity as MainActivity).ViewModel.BackButtonHidden = false;
                //        //}
                //        if (((ViewModel as BaseViewModel).GetCurrentVM() is IMainTabViewModel) && ParentActivity != null)
                //        {
                //            (ParentActivity as MainActivity).ViewModel.BackButtonHidden = false;
                //        }
                //    };
                //}
            }
            //BusyView = view.FindViewById(Resource.Id.v_busy_overlay);
            //if (BusyView != null)
            //{
            //    BusyView.Clickable = true;
            //    BusyView.Focusable = true;
            //    var busyImage = BusyView.FindViewById<ImageView>(Resource.Id.iv_busy_overlay);
            //    if (busyImage != null)
            //        AnimateImage(busyImage, Resource.Drawable.loader);
            //}
            //if ((ViewModel as BaseViewModel) != null && (ViewModel as BaseViewModel).IsBusy)
            //{
            //    if (BusyView != null)
            //        BusyView.Visibility = (ViewModel as BaseViewModel)?.IsBusy == true ? ViewStates.Visible : ViewStates.Invisible;
            //}
            ViewModel.PropertyChanged += OnViewModelChanged;


            return view;
        }

        public virtual void OnScrollViewChanged(object sender, EventArgs e, int scrollState = -1)
        {
            if (((MainActivity)ParentActivity)?.AreasViewWasAnimated == false || ((MainActivity)ParentActivity).ViewModel.IsBusy)
            {
                return;
            }
            if (((MainActivity)ParentActivity)?.AreasViewIsShown == true)
            {
                if (scrollState == 1 || scrollState == -1)
                {
                    ParentActivity.RunOnUiThread(() => { ((MainActivity)ParentActivity)?.CloseAreaViewWithAnim(); });
                }
                return;
            }
        }

        protected virtual void AreasCollectionShow(bool isShowed)
        {

        }

        public void AnimateImage(ImageView image, int resource = 0)
        {
            AnimationHelper.AnimateImage(image, resource);
        }

        public void AnimateButton(ImageButton button, int resource = 0)
        {
            AnimationHelper.AnimateButton(button, resource);
        }

        public virtual void HideKeyboardView(bool init = false)
        {
            try
            {
                ((MainActivity)ParentActivity)?.HideSoftKeyboard();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {GetType().Name}.onHideKeyboardView: {ex.Message}");
            }
        }

        #endregion

        #region Lifecycle

        public override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel.PropertyChanged -= OnViewModelChanged;
            if (loader != null)
            {
                AnimateImage(loader);
            }
        }

        public override void OnResume()
        {
            base.OnResume();

            if ((ViewModel as BaseViewModel) != null)
            {
                if (BusyView != null)
                    BusyView.Visibility = (ViewModel as BaseViewModel)?.IsBusy == true ? ViewStates.Visible : ViewStates.Invisible;
            }
            HideKeyboardView(true);
        }

        public override void OnStart()
        {
            base.OnStart();
            if (ParentActivity is MainActivity)
            {
                (ParentActivity as MainActivity).AreasCollectionShow += BaseFragment_AreasCollectionShow;
            }         
            if (loader != null)
            {
                AnimateImage(loader);
            }
        }

        private void BaseFragment_AreasCollectionShow(object sender, EventArgs e)
        {
            AreasCollectionShow((bool)sender);
        }

        public override void OnStop()
        {
            base.OnStop();
            if (ParentActivity is MainActivity)
            {
                (ParentActivity as MainActivity).AreasCollectionShow -= BaseFragment_AreasCollectionShow;
            }
            HideKeyboardView(false);
            if (loader != null)
            {
                loader.ClearAnimation();
            }
        }

        #endregion

        #region ViewModel
        public virtual void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == PropertiesExtension.GetPropertyName(() => (ViewModel as IBaseViewModel).IsBusy))
                {
                    if (loader != null)
                    {
                        using (var h = new Handler(Looper.MainLooper))
                            h.Post(() => { loader.Visibility = (ViewModel as IBaseViewModel)?.IsBusy == true ? ViewStates.Visible : ViewStates.Invisible; });
                    }
                }
                if (e.PropertyName == PropertiesExtension.GetPropertyName(() => (ViewModel as IBaseViewModel).IsRefreshing))
                {
                    if (!(ViewModel as IBaseViewModel).IsRefreshing && refresheLayout != null)
                    {
                        refresheLayout.Refreshing = false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {GetType().Name}.onViewModelChanged: {ex.Message}");
            }
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                HideKeyboardView(false);
            }
            return false;
        }
        #endregion

        #region Methods
        private void UpdateToolbarElements(View view)
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.area_name_text), FontsConstants.PN_B, (float)0.032);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.score_count), FontsConstants.PN_B, (float)0.019);
        }
        #endregion
    }
}