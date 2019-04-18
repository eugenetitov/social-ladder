using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using SocialLadder.Droid.Activities.BaseActivity;
using SocialLadder.ViewModels.Main;
using Android.Content.PM;
using SocialLadder.Droid.Services;
using MvvmCross.Droid.Support.V4;
using System.Collections.Generic;
using SocialLadder.Droid.Fragments.Main;
using SocialLadder.ViewModels.Feed;
using SocialLadder.ViewModels.Points;
using SocialLadder.ViewModels.Rewards;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.More;
using Android.Support.V4.Content;
using Android.Graphics;
using static Android.Views.View;
using System;
using SocialLadder.Droid.Views;
using MvvmCross.Droid.Support.V7.RecyclerView;
using SocialLadder.Droid.Adapters.Areas;
using MvvmCross.Binding.Droid.BindingContext;
using Android.Support.V7.Widget;
using SocialLadder.Droid.CustomListeners;
using Android.Views.Animations;
using SocialLadder.Droid.Helpers;
using System.ComponentModel;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Base;
using MvvmCross.Platform;
using MvvmCross.Core.Navigation;
using SocialLadder.Interfaces;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Droid.Assets;
using static Android.Widget.ImageView;
using Xamarin.Facebook;
using Android.Content;
using SocialLadder.Droid.PlatformService;
using Com.Nguyenhoanglam.Imagepicker.Model;
using SocialLadder.Droid.Models.MvxMessanger;
using SocialLadder.Interfaces.Base;
using System.Threading.Tasks;
using SocialLadder.Models;
using Android.Webkit;
using SocialLadder.Droid.Helpers.WebViewHelpers;
using MvvmCross.Core.ViewModels;
using SocialLadder.Models.LocalModels;
using MvvmCross.Platform.Core;
using MvvmCross.Binding.BindingContext;
using CrashlyticsKit;
using FabricSdk;
using Android.Gms.Common;
using Firebase;

namespace SocialLadder.Droid.Activities.Main
{
    [Activity(Label = "MainActivity", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation)]
    public class MainActivity : BaseAppCompatActivity<MainViewModel>, TabLayout.IOnTabSelectedListener
    {
        #region Variables
        //internal static readonly string CHANNEL_ID = "fcm_default_channel";
        protected override int LayoutResource => Resource.Layout.main;
        private readonly string[] _tabTitles = new string[] { "Feed", "Points", "Rewards", "Challenges", "More" };
        private int[] _imageResId = { Resource.Drawable.feed_icon, Resource.Drawable.points_icon, Resource.Drawable.rewards_icon, Resource.Drawable.challenges_icon, Resource.Drawable.more_icon };
        private MvxRecyclerView _areasView;
        private Android.Support.V7.Widget.Toolbar _toolbar;
        private UnscrollingViewPager _viewPager;
        private ImageView _scoreImage;
        private MvxCachingFragmentStatePagerAdapter _tabAdapter;
        private int _lastSelectedTab { get; set; } = 0;
        private WebView _wizardWebView;
        private ImageView _wizardLoader;
        private RelativeLayout _maimToolbarLayout;
        private ControlTouchListener _areaListener;
        private ControlTouchListener _toolbarListener;
        public event EventHandler AreasCollectionShow;
        public bool AreasViewIsShown { get; set; } = false;
        public bool AreasViewWasAnimated { get; set; } = true;
        public static event EventHandler<ScrollChangeEventArgs> ScrollHandler;
        public static int ScrollY { get; set; } = -1;
        public static Type ScrolledViewType { get; set; }
        public PointsViewModelHelper vmHelper;
        public ICallbackManager CallBackManager;
        #endregion

        #region Properties
        private IMvxInteraction<LocalChangedMainViewModel> _mainViewModelChanged;
        public IMvxInteraction<LocalChangedMainViewModel> MainViewModelChanged
        {
            get => _mainViewModelChanged;
            set
            {
                if (_mainViewModelChanged != null)
                    _mainViewModelChanged.Requested -= OnMainViewModelChanged;

                _mainViewModelChanged = value;
                _mainViewModelChanged.Requested += OnMainViewModelChanged;
            }
        }
        #endregion

        #region Lifecycle
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            FacebookSdk.SdkInitialize(ApplicationContext);
            CallBackManager = CallbackManagerFactory.Create();
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            _areasView = FindViewById<MvxRecyclerView>(Resource.Id.areas_collection);
            _scoreImage = FindViewById<ImageView>(Resource.Id.score_image);
            _wizardWebView = FindViewById<WebView>(Resource.Id.wizard_web_view);
            _maimToolbarLayout = FindViewById<RelativeLayout>(Resource.Id.main_toolbar_layout);

            SetUpToolbar();
            SetUpPointsViewModelHelper();         
            SetUpViewPager();
            SetUpAreasViewAdapter();
            SetUpCrashlyticsKit();
            SetUpNotification();  
            _maimToolbarLayout.Click += MainToolbarLayoutClick;

            var set = this.CreateBindingSet<MainActivity, MainViewModel>();
            set.Bind(this).For(v => v.MainViewModelChanged).To(viewModel => viewModel.MainViewModelChanged).OneWay();
            set.Apply();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _maimToolbarLayout.Click -= MainToolbarLayoutClick;
            ViewModel.AreaChanged -= ViewModelAreaChanged;
            _toolbarListener.SwipeDown -= ToolbarListenerSwipeDown;
        }

        protected override void OnResume()
        {
            base.OnResume();
            var color = SL.Area == null || string.IsNullOrEmpty(SL.Area.areaPrimaryColor) ? "#000000" : SL.Area.areaPrimaryColor;
            SetStatusBarColor(color);
        }

        protected override void OnStart()
        {
            base.OnStart();
            AlertService.Activity = PlatformNavigationService.Activity = PushNotificationService.Activity = FirebaseService.Activity = LocalNotificationService.Activity = this;
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            var messanger = Mvx.Resolve<IMvxMessenger>();
            if (resultCode == Result.Ok && requestCode == (int)Com.Nguyenhoanglam.Imagepicker.Model.Config.RcPickImages)
            {
                var pickedImages = data.GetParcelableArrayListExtra(Config.ExtraImages);
                messanger.Publish<ImagePickerMessangerModel>(new ImagePickerMessangerModel(this, pickedImages));
                return;
            }
            CallBackManager.OnActivityResult(requestCode, (int)resultCode, data);
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.ChangeCurrentArea))
            {
                CloseAreaViewWithAnim();
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.IsBusy))
            {
                if (ViewModel.IsBusy)
                {
                    AnimateImage(_scoreImage);
                }
                if (!ViewModel.IsBusy)
                {
                    _scoreImage.ClearAnimation();
                }
            }
        }
        #endregion

        #region Override
        public override void OnBackPressed()
        {
            if (!CloseBaseChildViewModels())
            {
                base.OnBackPressed();
            }
        }
        #endregion

        #region Methods
        private void OnMainViewModelChanged(object sender, MvxValueEventArgs<LocalChangedMainViewModel> eventArgs)
        {
            if (eventArgs.Value != null && !string.IsNullOrEmpty(eventArgs.Value.StatusBarColor))
            {
                SetStatusBarColor(eventArgs.Value.StatusBarColor);
            }
        }

        public void SetStatusBarColor(string color)
        {
            int r = System.Convert.ToInt32(color.Substring(1, 2), 16);
            int g = System.Convert.ToInt32(color.Substring(3, 2), 16);
            int b = System.Convert.ToInt32(color.Substring(5, 2), 16);
            var _color = Android.Graphics.Color.Rgb(r, g, b);
            Window.SetStatusBarColor(_color);
        }

        private void SetUpAreasViewAdapter()
        {
            _areasView.Adapter = new AreasCollectionAdapter((IMvxAndroidBindingContext)BindingContext);
            _areasView.SetLayoutManager(new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false));
            _areasView.HasFixedSize = true;
            _areaListener = new ControlTouchListener();
            _areasView.SetOnTouchListener(_areaListener);
        }

        private void SetUpPointsViewModelHelper()
        {
            vmHelper = new PointsViewModelHelper();
            vmHelper.CreateVM();
            ViewModel.AreaChanged += ViewModelAreaChanged;
        }

        private void SetUpViewPager()
        {
            _viewPager = FindViewById<UnscrollingViewPager>(Resource.Id.view_pager_frame);
            _viewPager.SetPagingEnabled(false);
            if (_viewPager != null)
            {
                var _navigationService = Mvx.Resolve<IMvxNavigationService>();
                var _locationService = Mvx.Resolve<ILocationService>();
                var _assetService = Mvx.Resolve<IPlatformAssetService>();
                var _alertService = Mvx.Resolve<IAlertService>();
                var _messamger = Mvx.Resolve<IMvxMessenger>();
                var _encodingService = Mvx.Resolve<IEncodingService>();
                var _platformNavigationService = Mvx.Resolve<IPlatformNavigationService>();
                var _localNotificationService = Mvx.Resolve<ILocalNotificationService>();
                var fragments = new List<MvxViewPagerFragmentInfo>
                {
                    new MvxViewPagerFragmentInfo(_tabTitles[0], typeof(FeedFragment), new FeedViewModel(_navigationService, _alertService, _assetService,_localNotificationService, _messamger)),
                    new MvxViewPagerFragmentInfo(_tabTitles[1], typeof(PointsContainerFragment), new PointsContainerViewModel(_navigationService, _alertService, _assetService,_localNotificationService, _messamger)),
                    new MvxViewPagerFragmentInfo(_tabTitles[2], typeof(RewardCategoriesFragment), new RewardCategoriesViewModel(_navigationService, _alertService, _assetService,_localNotificationService, _messamger)),
                    new MvxViewPagerFragmentInfo(_tabTitles[3], typeof(ChallengesFragment), new ChallengeViewModel(_navigationService, _alertService, _assetService,_localNotificationService, _messamger, _platformNavigationService, _locationService)),
                    new MvxViewPagerFragmentInfo(_tabTitles[4], typeof(MoreFragment), new MoreViewModel(_navigationService, _locationService, _alertService, _assetService,_localNotificationService, _messamger,_encodingService)),
                };
                _tabAdapter = new MvxCachingFragmentStatePagerAdapter(this, this.SupportFragmentManager, fragments);
                _viewPager.Adapter = _tabAdapter;
                var tabLayout = FindViewById<TabLayout>(Resource.Id.bottom_navigation);
                tabLayout.SetupWithViewPager(_viewPager);
                tabLayout.SetBackgroundColor(new Android.Graphics.Color(ContextCompat.GetColor(this.ApplicationContext, Resource.Color.main_toolbar_background)));
                for (int i = 0; i < tabLayout.TabCount; i++)
                {
                    TabLayout.Tab tab = tabLayout.GetTabAt(i);
                    tab.SetCustomView(GetTabView(i));
                }
                tabLayout.AddOnTabSelectedListener(this);
                TabHighlited(tabLayout.GetTabAt(0));
                (fragments[0].ViewModel as BaseViewModel).Refresh();
            }
        }
        
        private void SetUpToolbar()
        {
            if (_toolbar != null)
            {
                SetSupportActionBar(_toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                SupportActionBar.SetHomeButtonEnabled(false);

                _toolbarListener = new ControlTouchListener();
                _toolbar.SetOnTouchListener(_toolbarListener);
                _toolbarListener.SwipeDown += ToolbarListenerSwipeDown;

                FontHelper.UpdateFont(_toolbar.FindViewById<TextView>(Resource.Id.area_name_text), FontsConstants.PN_B, (float)0.032);
                FontHelper.UpdateFont(_toolbar.FindViewById<TextView>(Resource.Id.score_count), FontsConstants.PN_B, (float)0.019);
            }
        }

        private void SetUpCrashlyticsKit()
        {
            // Initiate Fabric
            CrashlyticsKit.Crashlytics.Instance.Initialize();
            FabricSdk.Fabric.Instance.Initialize(ApplicationContext);
            CrashlyticsKit.Crashlytics.Instance.SetStringValue("DeviceUUID", SL.DeviceUUID);
            CrashlyticsKit.Crashlytics.Instance.SetStringValue("UserName", SL.Profile?.UserName);
        }

        private void SetUpNotification()
        {
            //[START: FIREBASE]
            var notificationStatus = SL.AppSettings.GetValueOrDefault("NotificationStatus", string.Empty);

            if (notificationStatus == Enums.NotififcationStatus.Enabled.ToString())
            {
                if (Constants.NeedFirebaseImpl)
                {
                    if (Build.VERSION.SdkInt < BuildVersionCodes.O)
                    {
                        // Notification channels are new in API 26 (and not a part of the
                        // support library). There is no need to create a notification 
                        // channel on older versions of Android.
                        return;
                    }

                    var channel = new NotificationChannel(LocalConstants.NotificationCannelId, "SOCIALLADDER", NotificationImportance.Default)
                    {
                        Description = "Firebase Cloud Messages appear in this channel"
                    };

                    var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                    notificationManager.CreateNotificationChannel(channel);
                }
                else
                {
                    var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
                    if (resultCode == ConnectionResult.Success)
                    {
                        var intent = new Intent(this, typeof(RegistrationIntentService));
                        StartService(intent);
                    }
                }
            }
            //[END: FIREBASE]
        }

        private bool CloseBaseChildViewModels()
        {
            foreach (var fragment in this.SupportFragmentManager.Fragments)
            {
                if ((fragment as MvxFragment).ViewModel is IBaseChildViewModel)
                {
                    ((fragment as MvxFragment).ViewModel as BaseViewModel).BackCommand.Execute();
                    return true;
                }
            }
            return false;
        }

        public static void PointsScrolling(object sender, View.ScrollChangeEventArgs e)
        {
            try
            {
                if (sender != null && e != null)
                {
                    ScrollY = e.ScrollY;
                    ScrolledViewType = sender as Type;
                }
                ScrollHandler.Invoke(ScrolledViewType, new ScrollChangeEventArgs(null, 0, ScrollY, 0, 0));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Scroll Exception: " + ex.Message);
            }
        }

        private void ViewModelAreaChanged(object sender, EventArgs e)
        {
            vmHelper.SetFirstInitialise();
        }

        private void MainToolbarLayoutClick(object sender, EventArgs e)
        {
            ShowAreaViewWithAnim();
        }

        private void ToolbarListenerSwipeDown(object sender, EventArgs e)
        {
            ShowAreaViewWithAnim();
            ViewModel.AreaCollectionHidden = true;
        }

        public void ChangeShieldViewsVisibility(bool isVisible)
        {
            FindViewById<LinearLayout>(Resource.Id.main_bottom_shield).Visibility = (isVisible ? ViewStates.Visible : ViewStates.Gone);
            FindViewById<LinearLayout>(Resource.Id.main_top_shield).Visibility = (isVisible ? ViewStates.Visible : ViewStates.Gone);
        }

        #region PlatformNavigation
        public void SetFeedCurrentTabWithRefresh()
        {
            _viewPager.SetCurrentItem(0, true);
            Task.Run(async () => { await ViewModel.Refresh(); });
        }

        public void ChangeNotificationIndicatorStatus(bool status)
        {
            ViewModel.NotificationDotHidden = status;
        }

        public void OpenMainViewWithSwitchArea(AreaModel param, bool newAreaWasAdded)
        {
            if (param != null)
            {
                ViewModel.ChangeCurrentAreaFromAreaVM(param, newAreaWasAdded);
            }
            //
            ViewModel.RefreshAreasCommand.Execute();
            //
            CloseAreaViewWithAnim();
        }

        public void SetRequiredCurrentTab(int currentTabIndex)
        {
            CloseBaseChildViewModels();
            _viewPager.SetCurrentItem(currentTabIndex, true);
        }

        public void RefreshScoreView()
        {
            ViewModel.RefreshScoreView();
        }

        public void ShowWizardWebView(string url)
        {
            ViewModel.ShowWizardWebViewWithUrl(url);
            InitWizardWebView();
        }
        #endregion

        #region AreasAnimation
        private void ShowAreaViewWithAnim()
        {
            try
            {
                AreasViewIsShown = true;
                AreasCollectionShow.Invoke(true, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            AreasViewWasAnimated = false;
            var toolbarParams = _toolbar.LayoutParameters;
            toolbarParams.Height = 0;
            _toolbar.LayoutParameters = toolbarParams;
            _toolbar.Invalidate();
            ControlAnimation(_areasView, -DimensHelper.GetDimensById(Resource.Dimension.areas_view_height), 0, () => { }, () => { });
            ViewModel.AreaCollectionHidden = true;
        }

        public void CloseAreaViewWithAnim()
        {
            try
            {
                AreasViewIsShown = false;
                AreasCollectionShow.Invoke(false, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
            ControlAnimation(_toolbar, 0, 0, () => { }, () =>
            {
                var toolbarParams = _toolbar.LayoutParameters;
                toolbarParams.Height = DimensHelper.GetDimensById(Resource.Dimension.toolbar_height);
                _toolbar.LayoutParameters = toolbarParams;
                _toolbar.Invalidate();
            });
            ControlAnimation(_areasView, 0, -DimensHelper.GetDimensById(Resource.Dimension.areas_view_height), () => { }, () => { ViewModel.AreaCollectionHidden = false; });
        }

        private void ControlAnimation(View view, float fromY, float toY, Action actionStart, Action actionEnd, long duration = 250)
        {
            TranslateAnimation anim = new TranslateAnimation(0, 0, fromY, toY);
            anim.Duration = duration;
            anim.FillAfter = true;
            anim.AnimationStart += (s, e) =>
            {
                actionStart.Invoke();
            };
            anim.AnimationEnd += (s, e) =>
            {
                actionEnd.Invoke();
                AreasViewWasAnimated = true;
            };
            view.StartAnimation(anim);
        }
        #endregion

        #region TabsChanges
        public View GetTabView(int position)
        {
            View view = LayoutInflater.From(this).Inflate(Resource.Layout.custom_tab_layout, null);
            TextView tv = (TextView)view.FindViewById<TextView>(Resource.Id.textView);
            var item = _tabTitles[position];
            FontHelper.UpdateFont(tv, FontsConstants.PN_B, 0.027f);
            tv.SetText(item, TextView.BufferType.Normal);
            ImageView img = (ImageView)view.FindViewById(Resource.Id.imgView);
            img.SetImageResource(_imageResId[position]);
            img.SetScaleType(ScaleType.CenterInside);
            view.LayoutParameters = new LinearLayout.LayoutParams((int)(Android.Content.Res.Resources.System.DisplayMetrics.WidthPixels * 0.26f), ViewGroup.LayoutParams.MatchParent);
            return view;
        }

        public void OnTabReselected(TabLayout.Tab tab)
        {

        }

        public async void OnTabSelected(TabLayout.Tab tab)
        {
            TabHighlited(tab);
            if (tab.Position == 1)
            {
                vmHelper.SetFirstInitialise();
                (_tabAdapter.FragmentsInfo[tab.Position].ViewModel as PointsContainerViewModel).SetDefaultCurrentVM(_lastSelectedTab);
                _lastSelectedTab = tab.Position;
                return;
            }
            if (tab.Position == 2)
            {
                (_tabAdapter.FragmentsInfo[tab.Position].ViewModel as RewardCategoriesViewModel).RefreshNotificationSettings();
            }
            _lastSelectedTab = tab.Position;
            ViewModel.SetCurrentVM(_tabAdapter.FragmentsInfo[tab.Position].ViewModel);
            await (_tabAdapter.FragmentsInfo[tab.Position].ViewModel as BaseViewModel).Refresh();
            CloseAreaViewWithAnim();
        }

        private void TabHighlited(TabLayout.Tab tab)
        {
            RunOnUiThread(() =>
            {
                var textview = tab.CustomView.FindViewById<TextView>(Resource.Id.textView);
                ImageView imgView = tab.CustomView.FindViewById<ImageView>(Resource.Id.imgView);

                textview.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.tab_selected_color));
                imgView.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.tab_selected_color)), PorterDuff.Mode.SrcIn);
            });
        }

        public void OnTabUnselected(TabLayout.Tab tab)
        {
            RunOnUiThread(() =>
            {
                var textview = tab.CustomView.FindViewById<TextView>(Resource.Id.textView);
                ImageView imgView = tab.CustomView.FindViewById<ImageView>(Resource.Id.imgView);

                textview.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.areas_description_tex_color));
                imgView.SetColorFilter(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.areas_description_tex_color)), PorterDuff.Mode.SrcIn);
            });
        }
        #endregion

        #region WizardView
        private void InitWizardWebView()
        {
            if (_wizardWebView != null)
            {
                if (_wizardLoader == null)
                {
                    _wizardLoader = FindViewById<ImageView>(Resource.Id.wizard_loading_indicator);
                }
                _wizardWebView.Settings.JavaScriptEnabled = true;
                _wizardWebView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                _wizardWebView.SetWebViewClient(new WizardWebViewClient(StartLoaderingWizardWebView, FinishLoaderingWizardWebView, CloseWizardWebView));
            }
        }

        private void StartLoaderingWizardWebView()
        {
            AnimateImage(_wizardLoader);
            _wizardLoader.Visibility = ViewStates.Visible;
        }

        private void FinishLoaderingWizardWebView()
        {
            _wizardLoader.ClearAnimation();
            _wizardLoader.Visibility = ViewStates.Gone;
            _wizardWebView.SetBackgroundColor(Android.Graphics.Color.Transparent);
            _wizardWebView.SetWebChromeClient(null);
        }

        private void CloseWizardWebView()
        {
            FinishLoaderingWizardWebView();
            _wizardWebView.LoadUrl(null);
            ViewModel.CloseWizardViewCommand.Execute();
        }
        #endregion

        #endregion
    }
}

