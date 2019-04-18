using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System;
using UIKit;
using WebKit;
using SocialLadder.Models;
using SafariServices;
using SocialLadder.Authentication;
using SocialLadder.iOS.Constraints;

namespace SocialLadder.iOS.ViewControllers
{
    public partial class WebViewController : UIViewController
    {
        private int _statusBarHeight = (int)UIApplication.SharedApplication.StatusBarFrame.Height;

        private InstagramAuthTopBar _instagramAuthTopBar;

        public string Url { get; set; }

        public IInstagramAuthenticationDelegate InstagramDelegate { get; set; }

        public WKWebView WebView { get; set; }

        public WebViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            SetupToolBar();
            SetupWebView();
            SetupConstraints();
            View.BringSubviewToFront(_instagramAuthTopBar);
            WebView.LoadRequest(new NSUrlRequest(new NSUrl(Url)));
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _instagramAuthTopBar.BtnCancel.TouchUpInside += OnClickCancelBarButton;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _instagramAuthTopBar.BtnCancel.TouchUpInside -= OnClickCancelBarButton;
        }

        void OnClickCancelBarButton(object sender, EventArgs e)
        {
            InstagramDelegate.OnInstagramAuthenticationCanceled();
        }

        public async void Complete(string token)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            string responce = await client.GetStringAsync(string.Format("https://api.instagram.com/v1/users/self/?access_token={0}", token));

            var networkResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<InstagramUserResponce>(responce);
            var socialNetworkModel = new SocialNetworkModel();
            socialNetworkModel.AccessToken = token;
            socialNetworkModel.UserID = networkResponse?.data?.id;
            socialNetworkModel.NetworkName = "Instagram";
            //socialNetworkModel.NUserName = networkResponse?.data?.full_name;
            //socialNetworkModel.NetworkName = "Instagram";
            //socialNetworkModel.Country = "test";
            //socialNetworkModel.Data = "test";
            //socialNetworkModel.DOB = DateTime.Now;
            //socialNetworkModel.EmailAddress = "test@test.com";
            //socialNetworkModel.FirstName = "test";
            //socialNetworkModel.ID = networkResponse?.data?.id;
            //socialNetworkModel.LastName = "test";
            //socialNetworkModel.MiddleName = "test";
            //socialNetworkModel.networkID = 0;
            //socialNetworkModel.NLocation = "test";
            //socialNetworkModel.ProfileID = 0;
            //socialNetworkModel.ScoreComponentA = 0;
            //socialNetworkModel.ScoreComponentB = 0;
            //socialNetworkModel.TokenExpirationDate = DateTime.Now;


            InstagramDelegate.OnInstagramAuthenticationCompleted(socialNetworkModel/*new SocialNetworkModel() { AccessToken = token, UserID = networkResponse?.data?.id, NUserName = networkResponse?.data?.full_name, NetworkName = "Instagram" }*/);
        }

        private void SetupToolBar()
        {
            _instagramAuthTopBar = InstagramAuthTopBar.Create();
            var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);
            UIVisualEffectView blurView = new UIVisualEffectView(blurEffect);
            blurView.Frame = new CGRect(_instagramAuthTopBar.Frame.X, _instagramAuthTopBar.Frame.Y, _instagramAuthTopBar.Frame.Width, 63f);

            _instagramAuthTopBar.BackgroundColor = UIColor.FromRGBA(1, 1, 1, 0f);
            _instagramAuthTopBar.AddSubview(blurView);
            _instagramAuthTopBar.SendSubviewToBack(blurView);
            _instagramAuthTopBar.BtnCancel.Font = UIFont.FromName(_instagramAuthTopBar.BtnCancel.Font.Name, SizeConstants.ScreenMultiplier * 16);
            _instagramAuthTopBar.BtnCancel.Font = UIFont.FromName(_instagramAuthTopBar.BtnCancel.Font.Name, SizeConstants.ScreenMultiplier * 16);

            View.AddSubview(_instagramAuthTopBar);
        }

        private void SetupWebView()
        {
            WebView = new WKWebView(View.Frame, new WKWebViewConfiguration());
            WebView.UIDelegate = new WebDelegate();
            WebView.NavigationDelegate = new NavDelegate(this);

            WebView.ScrollView.ContentInset = new UIEdgeInsets(63f, 0, 0, 0);// / 414f * _screenWidth
            View.AddSubview(WebView);
            View.BackgroundColor = UIColor.Gray;
        }

        private void SetupConstraints()
        {
            _instagramAuthTopBar.TranslatesAutoresizingMaskIntoConstraints = false;
            _instagramAuthTopBar.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            _instagramAuthTopBar.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            _instagramAuthTopBar.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            _instagramAuthTopBar.HeightAnchor.ConstraintEqualTo(63f).Active = true;//_instagramAuthTopBar.WidthAnchor, 63f / 414f

            WebView.TranslatesAutoresizingMaskIntoConstraints = false;
            WebView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            WebView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            WebView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            WebView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        }

        private class InstagramUserResponce
        {
            public UserData data
            {
                get; set;
            }
        }

        private class UserData
        {
            public string id
            {
                get; set;
            }

            public string full_name
            {
                get;set;
            }
        }
    }

    public class WebDelegate : WKUIDelegate
    {
    }

    public class NavDelegate : WKNavigationDelegate
    {
        WebViewController WebViewController
        {
            get; set;
        }

        public NavDelegate(WebViewController webViewController)
        {
            WebViewController = webViewController;
        }

        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            var url = navigationAction.Request.Url;

            string urlString = url.ToString().ToLower();

            if (urlString.Contains("socialladderapp") && urlString.Contains("access_token"))
            {
                string[] comps = urlString.Split('=');
                if (comps != null && comps.Length == 2)
                {

                    WebViewController.Complete(comps[1]);

                }
            }

            decisionHandler(WKNavigationActionPolicy.Allow);
        }
    }
}