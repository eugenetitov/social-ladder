using CoreGraphics;
using Foundation;
using ObjCRuntime;
using System;
using UIKit;
using WebKit;
using SocialLadder.Models;
using SafariServices;
using SocialLadder.Authentication;

namespace SocialLadder.iOS.ViewControllers
{
    public partial class GenericWebViewController : UIViewController
    {
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;
        //private int _statusBarHeight = (int)UIApplication.SharedApplication.StatusBarFrame.Height;

        //private InstagramAuthTopBar _instagramAuthTopBar;
        private UIButton _btnBack;

        public string Url { get; set; }

        //public IInstagramAuthenticationDelegate InstagramDelegate { get; set; }

        public WKWebView WebView { get; set; }

        public GenericWebViewController() : base()
        {
            ;
        }

        public GenericWebViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Gray;

            SetupToolBar();
            SetupWebViewContent();
            SetupConstraints();
            //View.BringSubviewToFront(_instagramAuthTopBar);
            //new NSUrlRequest(new NSUrl(Url));
            WebView.LoadRequest(new NSMutableUrlRequest(new NSUrl(Url)));
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //_instagramAuthTopBar.BtnCancel.TouchUpInside += OnClickCancelBarButton;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //_instagramAuthTopBar.BtnCancel.TouchUpInside -= OnClickCancelBarButton;
        }

        //void OnClickCancelBarButton(object sender, EventArgs e)
        //{
        //    InstagramDelegate.OnInstagramAuthenticationCanceled();
        //}

        //public async void Complete(string token)
        //{
        //System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        //string responce = await client.GetStringAsync(string.Format("https://api.instagram.com/v1/users/self/?access_token={0}", token));

        //var networkResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<InstagramUserResponce>(responce);
        //var socialNetworkModel = new SocialNetworkModel();
        //socialNetworkModel.AccessToken = token;
        //socialNetworkModel.UserID = networkResponse?.data?.id;
        //socialNetworkModel.NetworkName = "Instagram";

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


        //InstagramDelegate.OnInstagramAuthenticationCompleted(socialNetworkModel/*new SocialNetworkModel() { AccessToken = token, UserID = networkResponse?.data?.id, NUserName = networkResponse?.data?.full_name, NetworkName = "Instagram" }*/);
        //}

        private void SetupToolBar()
        {
            _btnBack = new UIButton();
            _btnBack.SetTitle("Back", UIControlState.Normal);
            _btnBack.TouchUpInside += ((object sender, EventArgs args) => { this.DismissViewController(true, null); });
            View.AddSubview(_btnBack);

            //_instagramAuthTopBar = InstagramAuthTopBar.Create();
            //var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);
            //UIVisualEffectView blurView = new UIVisualEffectView(blurEffect);
            //blurView.Frame = new CGRect(_instagramAuthTopBar.Frame.X, _instagramAuthTopBar.Frame.Y, _instagramAuthTopBar.Frame.Width, 63f);

            //_instagramAuthTopBar.BackgroundColor = UIColor.FromRGBA(1, 1, 1, 0f);
            //_instagramAuthTopBar.AddSubview(blurView);
            //_instagramAuthTopBar.SendSubviewToBack(blurView);
            //_instagramAuthTopBar.BtnCancel.Font = UIFont.FromName(_instagramAuthTopBar.BtnCancel.Font.Name, CodeBehindUIConstants.BaseFontSize16Proprotion * _screenWidth);
            //_instagramAuthTopBar.BtnCancel.Font = UIFont.FromName(_instagramAuthTopBar.BtnCancel.Font.Name, CodeBehindUIConstants.BaseFontSize16Proprotion * _screenWidth);

            //View.AddSubview(_instagramAuthTopBar);
        }

        private void SetupWebViewContent()
        {
            WebView = new WKWebView(View.Frame, new WKWebViewConfiguration
            {

            });
            View.AddSubview(WebView);

            //WebView.UIDelegate = new WebDelegate();
            WebView.NavigationDelegate = new GenericNavDelegate(this);
            WebView.ScrollView.ContentInset = new UIEdgeInsets(63f, 0, 0, 0);// / 414f * _screenWidth

            var width = UIScreen.MainScreen.Bounds.Width;
            //device-width
            NSString viewportScriptString = (NSString)$@"var meta = document.createElement('meta');
                meta.setAttribute('name', 'viewport');
                meta.setAttribute('content', 'width={width} * 1');
                meta.setAttribute('initial-scale', '1.0');
                meta.setAttribute('maximum-scale', '1.0');
                meta.setAttribute('minimum-scale', '1.0');
                meta.setAttribute('user-scalable', 'no');
                document.getElementsByTagName('head')[0].appendChild(meta);";
            //NSString disableSelectionScriptString = (NSString)"document.documentElement.style.webkitUserSelect='none';";
            //NSString disableCalloutScriptString = (NSString)"document.documentElement.style.webkitTouchCallout='none';";

            // 1 - Make user scripts for injection
            var viewportScript = new WKUserScript(source: viewportScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true);
            //var disableSelectionScript = new WKUserScript(source: disableSelectionScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true);
            //var disableCalloutScript = new WKUserScript(source: disableCalloutScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true);

            // 2 - Initialize a user content controller, add scripts
            // From docs: "provides a way for JavaScript to post messages and inject user scripts to a web view."
            var controller = WebView.Configuration.UserContentController;
            //var controller = new WKUserContentController();
            controller.AddUserScript(viewportScript);
            //controller.AddUserScript(disableSelectionScript);
            //controller.AddUserScript(disableCalloutScript);

            // 6 - Webview options 
            WebView.ScrollView.ScrollEnabled = true;               // Make sure our view is interactable
            WebView.ScrollView.Bounces = false;                    // Things like this should be handled in web code
            WebView.AllowsBackForwardNavigationGestures = false;   // Disable swiping to navigate
            WebView.ContentMode = UIViewContentMode.ScaleToFill;   // Scale the page to fill the web view
        }

        private void SetupConstraints()
        {
            _btnBack.TranslatesAutoresizingMaskIntoConstraints = false;
            _btnBack.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor, 32f).Active = true;
            _btnBack.TopAnchor.ConstraintEqualTo(View.TopAnchor, 32f).Active = true;

            //_instagramAuthTopBar.TranslatesAutoresizingMaskIntoConstraints = false;
            //_instagramAuthTopBar.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            //_instagramAuthTopBar.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            //_instagramAuthTopBar.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            //_instagramAuthTopBar.HeightAnchor.ConstraintEqualTo(63f).Active = true;//_instagramAuthTopBar.WidthAnchor, 63f / 414f

            WebView.TranslatesAutoresizingMaskIntoConstraints = false;
            WebView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            WebView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            WebView.TopAnchor.ConstraintEqualTo(_btnBack.BottomAnchor, 16f).Active = true;
            WebView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        }

        //private class InstagramUserResponce
        //{
        //    public UserData data
        //    {
        //        get; set;
        //    }
        //}

        //private class UserData
        //{
        //    public string id
        //    {
        //        get; set;
        //    }

        //    public string full_name
        //    {
        //        get;set;
        //    }
        //}
    }

    //public class GenericWebDelegate : WKUIDelegate
    //{
    //}

    public class GenericNavDelegate : WKNavigationDelegate
    {
        GenericWebViewController GenericWebViewController
        {
            get; set;
        }

        public GenericNavDelegate(GenericWebViewController webViewController)
        {
            GenericWebViewController = webViewController;
        }

        public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
        {
            var url = navigationAction.Request.Url;

            string urlString = url.ToString().ToLower();

            //if (urlString.Contains("socialladderapp") && urlString.Contains("access_token"))
            //{
            //    string[] comps = urlString.Split('=');
            //    if (comps != null && comps.Length == 2)
            //    {

            //        GenericWebViewController.Complete(comps[1]);

            //    }
            //}

            decisionHandler(WKNavigationActionPolicy.Allow);
        }
    }
}