using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Media;
using System;
using Android.Content.PM;
using SocialLadder.ViewModels.Intro;
using SocialLadder.Droid.Activities.BaseActivity;
using System.Collections.Generic;
using MvvmCross.Droid.Support.V4;
using Android.Support.V4.View;
using MvvmCross.Platform;
using SocialLadder.Droid.Fragments.Intro;
using MvvmCross.Core.Navigation;
using SocialLadder.Droid.Assets;
using SocialLadder.Interfaces;
using SocialLadder.Droid.Services;
using SocialLadder.Droid.Helpers;
using Android;
using SocialLadder.Droid.PlatformService;
using MvvmCross.Plugins.Messenger;

namespace SocialLadder.Droid.Activities.Intro
{
    public class VideoLoop : Java.Lang.Object, Android.Media.MediaPlayer.IOnPreparedListener
    {
        public void OnPrepared(MediaPlayer mp)
        {
            mp.Looping = true;
        }
    }

    [Activity(Label = "IntroContainerActivity", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation)]
    public class IntroContainerActivity : BaseAppCompatActivity<IntroContainerViewModel>, ViewPager.IOnPageChangeListener
    {
        #region Variables
        protected override int LayoutResource => Resource.Layout.intro;       
        private LinearLayout _bottomView;
        private LinearLayout _topView;
        private IMvxNavigationService _navigationService;
        private IPlatformAssetService _assetService;
        private IAlertService _alertService;
        private IMvxMessenger _messenger;
        private ILocalNotificationService _localNotificationService;
        private MvxCachingFragmentStatePagerAdapter _adapter;
        private ProgressBar _progressBar;
        private Button _buttonNext;
        private Button _buttonLogin;
        public ViewPager viewPager;
        public List<MvxViewPagerFragmentInfo> fragments;
        public int CurrentFragmentIndex { get; set; } = 0;
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.SetStatusBarColor(Android.Graphics.Color.Black);
            PermissionHelper.AddPermission(this, new List<string> { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation });
            _buttonNext = FindViewById<Button>(Resource.Id.btn_next);
            FontHelper.UpdateFont(_buttonNext, FontsConstants.PN_R, (float)0.045);
            _buttonLogin = FindViewById<Button>(Resource.Id.intro_login_button);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.customProgress);
            _progressBar.Progress = 5;
            viewPager = FindViewById<ViewPager>(Resource.Id.view_pager_frame);
            SetUpViewPager();
            _buttonNext.Click += ButtonNextClick;
            _buttonLogin.Click += ButoonLoginClick;
            viewPager.Click += ViewPagerClick;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _buttonNext.Click -= ButtonNextClick;
            _buttonLogin.Click -= ButoonLoginClick;
            viewPager.Click -= ViewPagerClick;
        }

        protected override void OnStart()
        {
            base.OnStart();
            LoopVideo();
            AlertService.Activity = PlatformNavigationService.Activity = this;
        }
        #endregion

        #region Methods
        private void SetUpViewPager()
        {
            if (viewPager == null)
            {
                return;
            }
            _navigationService = Mvx.Resolve<IMvxNavigationService>();
            var _locationService = Mvx.Resolve<ILocationService>();
            _assetService = Mvx.Resolve<IPlatformAssetService>();
            _alertService = Mvx.Resolve<IAlertService>();
            _messenger = Mvx.Resolve<IMvxMessenger>();
            _localNotificationService = Mvx.Resolve<ILocalNotificationService>();
            fragments = new List<MvxViewPagerFragmentInfo>();
            fragments.Add(new MvxViewPagerFragmentInfo("", typeof(Intro1Fragment), new Intro1ViewModel(_navigationService, _alertService, _assetService, _localNotificationService)));
            fragments.Add(new MvxViewPagerFragmentInfo("", typeof(Intro2Fragment), new Intro2ViewModel(_navigationService, _alertService, _assetService, _localNotificationService)));
            fragments.Add(new MvxViewPagerFragmentInfo("", typeof(Intro3Fragment), new Intro3ViewModel(_navigationService, _alertService, _assetService, _localNotificationService)));
            fragments.Add(new MvxViewPagerFragmentInfo("", typeof(Intro4Fragment), new Intro4ViewModel(_navigationService, _alertService, _assetService, _localNotificationService)));
            fragments.Add(new MvxViewPagerFragmentInfo("", typeof(NetworksFragment), new NetworksViewModel(_navigationService, _locationService, _alertService, _assetService, _localNotificationService, _messenger)));
            viewPager.AddOnPageChangeListener(this);
            _adapter = new MvxCachingFragmentStatePagerAdapter(this, SupportFragmentManager, fragments);
            viewPager.Adapter = _adapter;
        }

        private void ViewPagerClick(object sender, EventArgs e)
        {
            HideSoftKeyboard();
        }

        private void ButoonLoginClick(object sender, EventArgs e)
        {
            viewPager.SetCurrentItem(4, true);
        }

        private void ButtonNextClick(object sender, EventArgs e)
        {
            if (CurrentFragmentIndex == 5)
            {
                ViewModel.NavigateToMainView.Execute();
                return;
            }
            if (CurrentFragmentIndex == 4 && ViewModel.IsAuthorized)
            {
                AddAreasFragment();
            }
            viewPager.SetCurrentItem(CurrentFragmentIndex + 1, true);
        }

        private void AddAreasFragment()
        {
            if (fragments.Count == 5)
            {
                fragments.Add(new MvxViewPagerFragmentInfo("", typeof(AreasFragment), new AreasCollectionViewModel(_navigationService, _alertService, _assetService, _localNotificationService)));
                viewPager.Adapter = _adapter;
            }
        }

        private void LoopVideo()
        {
            var videoView = FindViewById<VideoView>(Resource.Id.intro_video_view);
            videoView.SetOnPreparedListener(new VideoLoop());
            var uri = Android.Net.Uri.Parse("android.resource://" + PackageName + "/" + Resource.Raw.cropedBackgroundVideo);
            videoView.SetVideoURI(uri);
            videoView.Start();
        }

        #region IOnPageChangeListener
        public void OnPageScrollStateChanged(int state)
        {
            HideSoftKeyboard();
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            //if (CurrentFragmentIndex >= 4)
            //{
            //    viewPager.SetCurrentItem(4, true);
            //}
        }

        public void OnPageSelected(int position)
        {
            var viewModel = (_adapter.FragmentsInfo[position].ViewModel);
            _buttonLogin.Visibility = _buttonNext.Visibility = ViewStates.Visible;
            _buttonNext.Text = "Next";
            CurrentFragmentIndex = position;
            if (position == 0)
            {
                _progressBar.Progress = 5;
            }
            if (position == 1)
            {
                _progressBar.Progress = 20;
            }
            if (position == 2)
            {
                _progressBar.Progress = 40;
            }
            if (position == 3)
            {
                _progressBar.Progress = 60;
            }
            if (position == 4)
            {
                _progressBar.Progress = 80;
                _buttonLogin.Visibility = ViewStates.Invisible;
            }
            if (position == 5)
            {
                _progressBar.Progress = 100;
                _buttonNext.Text = "Done";
                _buttonLogin.Visibility = ViewStates.Invisible;
                (viewModel as AreasCollectionViewModel).InitAreas();

            }
        }
        #endregion      
        #endregion
    }
}
