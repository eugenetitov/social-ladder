using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Web
{
    public class WebViewModel : BaseViewModel
    {
        private MvxInteraction<WebViewContentType> _loadWebPageInteraction;
        public IMvxInteraction<WebViewContentType> LoadWebPageInteraction => _loadWebPageInteraction;
      
        private bool _webToolbarBackShowed;
        public bool WebToolbarBackShowed
        {
            get => _webToolbarBackShowed;
            set => SetProperty(ref _webToolbarBackShowed, value);
        }

        public WebViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _token = _messenger.Subscribe<MessangerWebModel>(OnLoadPage);
            _loadWebPageInteraction = new MvxInteraction<WebViewContentType>();
            HexToolbarColor = "#000000";
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _messenger.Unsubscribe<MessangerWebModel>(_token);
            base.ViewDestroy(viewFinishing);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            if (WebViewData != null && (!string.IsNullOrEmpty(WebViewData.Url) || !string.IsNullOrEmpty(WebViewData.Data)))
            {
                RaiseWebViewData();
            }
        }

        public void OnLoadPage(MessangerWebModel parameter)
        {
            string url;
            if (parameter.URL.EndsWith(".pdf"))
                //url = "https://docs.google.com/gview?embedded=true&url=" + parameter.URL;
                url = "https://docs.google.com/viewer?url=" + parameter.URL + "&embedded=true";
            else
                url = parameter.URL;

            WebViewData = new Models.LocalModels.LocalWebDataModel { Url = url };
            WebToolbarBackShowed = parameter.ToolbarScoreViewHidden;
            RaiseWebViewData();
        }

        private void RaiseWebViewData()
        {
            if (!string.IsNullOrEmpty(WebViewData.Url) && (WebViewData.Url.Length >= 15 && (WebViewData.Url.Substring(0, 15).Contains("https://goo.gl/")) || (WebViewData.Url.Length >= 25 && WebViewData.Url.Substring(0, 25).Contains("https://drive.google.com"))))
            {
                _loadWebPageInteraction.Raise(WebViewContentType.GoogleDrive);
                return;
            }
            if (!string.IsNullOrEmpty(WebViewData.Url))
            {
                _loadWebPageInteraction.Raise(WebViewContentType.Url);
            }
            if (!string.IsNullOrEmpty(WebViewData.Data))
            {
                _loadWebPageInteraction.Raise(WebViewContentType.Data);
            }
        }
    }
}
