using System;
using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using SocialLadder.Extensions;
using SocialLadder.Interfaces.Base;
using SocialLadder.ViewModels.Base;

namespace SocialLadder.Droid.Activities.BaseActivity
{
    public abstract class BaseAppCompatActivity<TViewModel> : MvxAppCompatActivity<TViewModel> where TViewModel : class, IMvxViewModel, IMvxNotifyPropertyChanged
    {
        #region Properties
        public RelativeLayout busyView { get; set; }
        protected abstract int LayoutResource { get; }
        public new TViewModel ViewModel { get => base.ViewModel as TViewModel; }
        public IBaseViewModel BaseViewModel { get => base.ViewModel as IBaseViewModel; }
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(LayoutResource);
            ViewModel.PropertyChanged += OnViewModelChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel.PropertyChanged -= OnViewModelChanged;
        }
        #endregion

        #region Override
        public override void OnBackPressed()
        {
            //(ViewModel as BaseViewModel).SetLastVMAsCurrent();
            (ViewModel as BaseViewModel).BackCommand.Execute();
            //base.OnBackPressed();
        }
        #endregion

        #region Methods
        public virtual void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == PropertiesExtension.GetPropertyName(() => BaseViewModel.IsBusy))
                {
                    if (busyView != null)
                    {
                        using (var h = new Handler(Looper.MainLooper))
                            h.Post(() =>
                            {
                                busyView.Visibility = BaseViewModel?.IsBusy == true ? ViewStates.Visible : ViewStates.Invisible;
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {GetType().Name}.onViewModelChanged: {ex.Message}");
            }
        }

        public void HideSoftKeyboardMenu()
        {
            CurrentFocus.ClearFocus();
        }

        public void HideSoftKeyboard()
        {
            if (CurrentFocus == null)
                return;
            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);

            CurrentFocus.ClearFocus();
        }

        public void AnimateImage(ImageView image, int resource = 0)
        {
            try
            {
                if (resource != 0)
                {
                    image.SetImageResource(resource);
                }
                RotateAnimation rotateAnimation = new RotateAnimation(0, 359, Dimension.RelativeToSelf, 0.5f, Dimension.RelativeToSelf, 0.5f);
                rotateAnimation.RepeatCount = Animation.Infinite;
                rotateAnimation.RepeatMode = RepeatMode.Restart;
                rotateAnimation.Duration = 700;
                rotateAnimation.Interpolator = new LinearInterpolator();
                image.StartAnimation(rotateAnimation);

            }
            catch { }
        }
        #endregion
    }
}