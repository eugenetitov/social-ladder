using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Webkit;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Helpers.WebViewHelpers;
using SocialLadder.Enums;
using SocialLadder.Extensions;
using SocialLadder.Helpers;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Intro;
using SocialLadder.ViewModels.Main;
using SocialLadder.ViewModels.Web;

namespace SocialLadder.Droid.Fragments.Web
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class WebViewFragment : BaseFragment<WebViewModel>, IMvxOverridePresentationAttribute
    {
        public MvxBasePresentationAttribute PresentationAttribute()
        {
            MvxFragmentPresentationAttribute attr = new MvxFragmentPresentationAttribute();
            if (!NavigationHelper.ShowWebViewFromIntroVM)
            {
                attr.ActivityHostViewModelType = typeof(MainViewModel);
                attr.FragmentContentId = Resource.Id.content_frame_full;
                attr.AddToBackStack = true;
            }
            if (NavigationHelper.ShowWebViewFromIntroVM)
            {
                attr.ActivityHostViewModelType = typeof(IntroContainerViewModel);
                attr.FragmentContentId = Resource.Id.content_frame_full;
                attr.AddToBackStack = true;
            }
            return attr;
        }

        protected override int FragmentId => Resource.Layout.web_view_layout;
        protected override bool HasBackButton => true;

        private IMvxInteraction<WebViewContentType> _webViewInteraction;
        public IMvxInteraction<WebViewContentType> WebViewInteraction
        {
            get => _webViewInteraction;
            set
            {
                if (_webViewInteraction != null)
                    _webViewInteraction.Requested -= _webViewInteraction_Requested;

                _webViewInteraction = value;
                _webViewInteraction.Requested += _webViewInteraction_Requested; ;
            }
        }

        private WebView _webView;
        private ImageView _loader;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _loader = view.FindViewById<ImageView>(Resource.Id.web_view_loader);
            _webViewInteraction = new MvxInteraction<WebViewContentType>();
            _webView = view.FindViewById<WebView>(Resource.Id.webView);
            _webView.Settings.SetPluginState(WebSettings.PluginState.On);
            _webView.Settings.AllowFileAccess = true;
            _webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            _webView.Settings.JavaScriptEnabled = true;
            var set = this.CreateBindingSet<WebViewFragment, WebViewModel>();
            set.Bind(this).For(v => v.WebViewInteraction).To(viewModel => viewModel.LoadWebPageInteraction).OneWay(); 
            set.Apply();

            AnimateLoader();
            return view;
        }

        private void _webViewInteraction_Requested(object sender, MvvmCross.Platform.Core.MvxValueEventArgs<WebViewContentType> e)
        {
            if (e.Value == WebViewContentType.Data)
            {

            }
            if (e.Value == WebViewContentType.GoogleDrive)
            {
                _webView.SetWebChromeClient(new CustomWebChromeClient(StopAnimate));
            }
            if (e.Value == WebViewContentType.Url)
            {
                _webView.LoadUrl(ViewModel.WebViewData.Url);
                var helper = new WebHelper(() => { AnimateLoader(); }, () => { StopAnimate(); });
                _webView.SetWebViewClient(helper);
            }
        }

        private void AnimateLoader()
        {
            AnimateImage(_loader);
        }

        private void StopAnimate()
        {
            _loader.Visibility = ViewStates.Gone;
            _loader.ClearAnimation();
        }

    }
}