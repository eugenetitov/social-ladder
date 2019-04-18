using System.ComponentModel;
using Android.Content;
using Android.OS;
using Android.Text.Method;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Core;
using SocialLadder.Authentication;
using SocialLadder.Droid.Activities.Intro;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Enums;
using SocialLadder.Extensions;
using SocialLadder.Helpers;
using SocialLadder.ViewModels.Intro;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Intro
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(NetworksViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame)]
    public class NetworksFragment : BaseFragment<NetworksViewModel>, IMvxOverridePresentationAttribute
    {
        #region Variables
        private LinearLayout _fbView, _twitterView, _instaView;
        private TextView _fbText, _twitterText, _instaText, _networkTextTop, _networkTextBottom, _linkedText;
        private Button _button_PrivacyPolici, _button_TermsOfService;
        private ImageView _fbNetworkImage, _twitterNetworkImage, _instaNetworkImage, _fbNetworkLoader, _twitterNetworkLoader, _instaNetworkLoader, _score_View;
        private WebView _webViewPrivacy;
        private IntroContainerActivity _introActivity;
        private View _view;
        private bool _fbConnecting, _twitterConnecting, _instaConnecting;

        protected override int FragmentId => Resource.Layout.NetworksLayout;
        protected override bool HasBackButton => false;
        #endregion
         
        #region Properties
        private IMvxInteraction<LocalNetworkAction> _actionFragmentInteraction;
        public IMvxInteraction<LocalNetworkAction> ActionFragmentInteraction
        {
            get => _actionFragmentInteraction;
            set
            {
                if (_actionFragmentInteraction != null)
                    _actionFragmentInteraction.Requested -= ActionFragmentInteractionRequested;

                _actionFragmentInteraction = value;
                _actionFragmentInteraction.Requested += ActionFragmentInteractionRequested;
            }
        }
        #endregion

        #region Lifecycle
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = base.OnCreateView(inflater, container, savedInstanceState);

            _introActivity = this.Activity as IntroContainerActivity;
            _fbView = _view.FindViewById<LinearLayout>(Resource.Id.fb_view);
            _twitterView = _view.FindViewById<LinearLayout>(Resource.Id.twitter_view);
            _instaView = _view.FindViewById<LinearLayout>(Resource.Id.insta_view);
            _fbText = _fbView.FindViewById<TextView>(Resource.Id.networkText);
            _twitterText = _twitterView.FindViewById<TextView>(Resource.Id.networkText);
            _instaText = _instaView.FindViewById<TextView>(Resource.Id.networkText);
            _fbNetworkImage = _fbView.FindViewById<ImageView>(Resource.Id.networkImage);
            _fbNetworkLoader = _fbView.FindViewById<ImageView>(Resource.Id.networkLoader);
            _twitterNetworkImage = _twitterView.FindViewById<ImageView>(Resource.Id.networkImage);
            _twitterNetworkLoader = _twitterView.FindViewById<ImageView>(Resource.Id.networkLoader);
            _instaNetworkImage = _instaView.FindViewById<ImageView>(Resource.Id.networkImage);
            _instaNetworkLoader = _instaView.FindViewById<ImageView>(Resource.Id.networkLoader);
            _score_View = _view.FindViewById<ImageView>(Resource.Id.score_View);

            _webViewPrivacy = _view.FindViewById<WebView>(Resource.Id.webPrivacy);
            _linkedText = _view.FindViewById<TextView>(Resource.Id.txtLinks);
            
            _networkTextTop = _view.FindViewById<TextView>(Resource.Id.networkText_Top);
            _networkTextBottom = _view.FindViewById<TextView>(Resource.Id.networkText_Bottom);
            _button_PrivacyPolici = _view.FindViewById<Button>(Resource.Id.btn_PrivacyPolici);
            _button_TermsOfService = _view.FindViewById<Button>(Resource.Id.btn_termsOfService);

            _linkedText.MovementMethod = LinkMovementMethod.Instance;
            _webViewPrivacy.SetWebViewClient(new WebHelper());
            _webViewPrivacy.LoadUrl(ViewModel.WebViewLink);

            UndateControls();
            CreateBindings();
            _fbConnecting = _twitterConnecting = _instaConnecting = false;
            return _view;
        }

        public override void OnResume()
        {
            base.OnResume();
            if (NavigationHelper.ShowNetworksPageFromMainVM)
            {
                Activity.Window.SetStatusBarColor(Android.Graphics.Color.Black);
                LoopVideo();
            }
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        #region Authentication
        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.FBLoaderImage))
            {
                if (ViewModel.IsBusy && !_fbConnecting)
                {
                    _fbConnecting = true;
                    var auth = new FacebookAuthenticator(Configuration.FbClientId, Configuration.Scope, ViewModel);
                    var authenticator = auth.GetAuthenticator();
                    var intent = authenticator.GetUI(this.Activity);
                    this.StartActivity(intent);
                    AnimateImage(_fbNetworkLoader);
                }
                if (!ViewModel.IsBusy)
                {
                    _fbNetworkLoader.ClearAnimation();
                    _fbConnecting = false;
                }
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.TwitterLoaderImage))
            {
                if (ViewModel.IsBusy && !_twitterConnecting)
                {
                    _twitterConnecting = true;
                    var auth = new TwitterAuthentificator(Configuration.ConsumerKeyTwitter, Configuration.ConsumerSecretTwitter, Configuration.Scope, ViewModel);
                    var authenticator = auth.GetAuthenticator();
                    var intent = authenticator.GetUI(this.Activity);
                    this.StartActivity(intent);
                    AnimateImage(_twitterNetworkLoader);
                }
                if (!ViewModel.IsBusy)
                {
                    _twitterConnecting = false;
                    _twitterNetworkLoader.ClearAnimation();
                }
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.InstaLoaderImage))
            {
                if (ViewModel.IsBusy && !_instaConnecting)
                {
                    _instaConnecting = true;
                    var auth = new InstagramAuthenticator(Configuration.ConsumerKeyInsta, string.Empty, Configuration.InstaScope, ViewModel);
                    var authenticator = auth.GetAuthenticator();
                    var intent = authenticator.GetUI(this.Activity);
                    this.StartActivity(intent);
                    AnimateImage(_instaNetworkLoader);
                }
                if (!ViewModel.IsBusy)
                {
                    _instaConnecting = false;
                    _instaNetworkLoader.ClearAnimation();
                }
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.ScoreImage))
            {
                if (ViewModel.GetScoreImageLoadName() == ViewModel.ScoreImage)
                {
                    AnimateImage(_score_View);
                }
                if (ViewModel.GetScoreImageLoadName() != ViewModel.ScoreImage)
                {
                    _score_View.ClearAnimation();
                }
            }
        }
        #endregion
        #endregion

        #region Methods
        private void UndateControls()
        {
            _fbText.Text = "Facebook";
            _twitterText.Text = "Twitter";
            _instaText.Text = "Instagram";

            FontHelper.UpdateFont(_fbText, FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(_twitterText, FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(_instaText, FontsConstants.PN_R, (float)0.037);
            FontHelper.UpdateFont(_networkTextTop, FontsConstants.PN_R, (float)0.052);
            FontHelper.UpdateFont(_networkTextBottom, FontsConstants.PN_R, (float)0.033);
            FontHelper.UpdateFont(_button_PrivacyPolici, FontsConstants.PN_R, (float)0.033);
            FontHelper.UpdateFont(_button_TermsOfService, FontsConstants.PN_R, (float)0.033);
            FontHelper.UpdateFont(_view.FindViewById<Button>(Resource.Id.done_button), FontsConstants.PN_R, (float)0.07);
        }

        public MvxBasePresentationAttribute PresentationAttribute()
        {
            MvxFragmentPresentationAttribute attr = new MvxFragmentPresentationAttribute();
            if (NavigationHelper.ShowNetworksPageFromMainVM)
            {
                attr.ActivityHostViewModelType = typeof(MainViewModel);
                attr.FragmentContentId = Resource.Id.content_frame_full;
                attr.AddToBackStack = true;
            }
            if (!NavigationHelper.ShowNetworksPageFromMainVM)
            {
                attr.ActivityHostViewModelType = typeof(IntroContainerViewModel);
                attr.FragmentContentId = Resource.Id.content_frame;
                attr.AddToBackStack = true;
            }
            return attr;
        }
         

        private void ActionFragmentInteractionRequested(object sender, MvxValueEventArgs<LocalNetworkAction> eventArgs)
        {
            if (eventArgs.Value == LocalNetworkAction.CloseAction)
            {
                if (_introActivity != null)
                {
                    _introActivity.viewPager.SetCurrentItem(_introActivity.CurrentFragmentIndex - 1, true);
                }
            }
            if (eventArgs.Value == LocalNetworkAction.OnPrivacyAction)
            {
                _webViewPrivacy.LoadUrl(ViewModel.WebViewLink);
            }
            if (eventArgs.Value == LocalNetworkAction.FeedOpenAction)
            {

            }
        }

        private void LoopVideo()
        {
            var videoView = _view.FindViewById<VideoView>(Resource.Id.video_view);
            videoView.SetOnPreparedListener(new VideoLoop());
            var uri = Android.Net.Uri.Parse("android.resource://" + Activity.PackageName + "/" + Resource.Raw.cropedBackgroundVideo);
            videoView.SetVideoURI(uri);
            videoView.Start();
        }

        private void CreateBindings()
        {
            var owner = this as IMvxBindingContextOwner;
            var set = owner.CreateBindingSet<IMvxBindingContextOwner, NetworksViewModel>();
            set.Bind(_fbNetworkImage).For(v => v.Drawable).To(vm => vm.FBImage).WithConversion("StringDrawableConverter");
            set.Bind(_twitterNetworkImage).For(v => v.Drawable).To(vm => vm.TwitterImage).WithConversion("StringDrawableConverter");
            set.Bind(_instaNetworkImage).For(v => v.Drawable).To(vm => vm.InstaImage).WithConversion("StringDrawableConverter");

            set.Bind(_fbNetworkLoader).For(v => v.Drawable).To(vm => vm.FBLoaderImage).WithConversion("StringDrawableConverter");
            set.Bind(_twitterNetworkLoader).For(v => v.Drawable).To(vm => vm.TwitterLoaderImage).WithConversion("StringDrawableConverter");
            set.Bind(_instaNetworkLoader).For(v => v.Drawable).To(vm => vm.InstaLoaderImage).WithConversion("StringDrawableConverter");

            set.Bind(this).For(v => v.ActionFragmentInteraction).To(viewModel => viewModel.ActionInteraction).OneWay();
            set.Apply();
        }
        #endregion
    }
}