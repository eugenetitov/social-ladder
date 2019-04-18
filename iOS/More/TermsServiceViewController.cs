using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Navigation;
using System;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.More
{
    public partial class TermsServiceViewController : UIViewController
    {
        public static UIView Overlay;
        private NavigationTitleView NavTitle;

        public TermsServiceViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Need deviceUUID and AreaGUID as a parameter
            var deviceUUID = SL.DeviceUUID;
            var areaGUID = SL.AreaGUID;

            WebView.ScrollView.Bounces = false;
            WebView.NavigationDelegate = new NavDelegate2(this);
            if (areaGUID != null)
            {
                var url = new NSUrl("http://socialladder.rkiapps.com/TermOfService.aspx?deviceUUID=" + SL.DeviceUUID + "&AreaGUID=" + SL.AreaGUID);
                var request = new NSUrlRequest(url);
                WebView.LoadRequest(request);
            }
            else
            {
                var url = new NSUrl("http://socialladder.rkiapps.com/TermOfService.aspx?deviceUUID=" + SL.DeviceUUID);
                var request = new NSUrlRequest(url);
                WebView.LoadRequest(request);
            }

            if (NavigationController == null)
            {
                UnHideToolbarView();
            }

            //Overlay = Platform.AddOverlay(new CGRect(View.Frame.X, WebView.Frame.Y, View.Frame.Width, View.Frame.Height), UIColor.White, true, true, 0.1f);
            AddOverlay();
        }

        public void AddOverlay()
        {
            var statusBarFrameHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
            var navBarHeihgt = this.NavigationController.NavigationBar.Frame.Size.Height;
            var overlayFrame = new CGRect(View.Frame.X, View.Frame.Y + statusBarFrameHeight + navBarHeihgt, View.Frame.Width, View.Frame.Height - statusBarFrameHeight - navBarHeihgt);
            Overlay = Platform.AddOverlay(this.View, overlayFrame, UIColor.White, true);
            if (Overlay != null)
            {
                nfloat w = Overlay.Frame.Width;//View.Frame.Width;//UIScreen.MainScreen.Bounds.Width;
                nfloat h = Overlay.Frame.Height;//View.Frame.Height;//UIScreen.MainScreen.Bounds.Height;
                nfloat s = w * 0.2f;
                UIImageView progress = new UIImageView(new CGRect((w - s) / 2.0f, ((h - s) / 2.0f) - s / 2, s, s));
                progress.Image = UIImage.FromBundle("loading-indicator");
                Overlay.AddSubview(progress);
                Platform.AnimateRotation(progress);
            }
        }

        private void UnHideToolbarView()
        {
            NavTitle = NavigationTitleView.Create();
            NavTitle.BtnBackOutlet.TouchUpInside += (object sender, EventArgs e) =>
            {
                DismissModalViewController(true);
            };
            NavTitle.BackMode();
            TermsToolBarContainer.AddSubview(NavTitle);

            NavTitle.TranslatesAutoresizingMaskIntoConstraints = false;
            NavTitle.RightAnchor.ConstraintEqualTo(TermsToolBarContainer.RightAnchor).Active = true;
            NavTitle.TopAnchor.ConstraintEqualTo(TermsToolBarContainer.TopAnchor).Active = true;
            NavTitle.BottomAnchor.ConstraintEqualTo(TermsToolBarContainer.BottomAnchor).Active = true;
            NavTitle.LeftAnchor.ConstraintEqualTo(TermsToolBarContainer.LeftAnchor).Active = true;
            nfloat screenWidth = UIScreen.MainScreen.Bounds.Width;
            nfloat toolbarHeight = (66f / 414f) * screenWidth;
            TermsHeightToolBarConstraint.Constant = toolbarHeight;
            View.UpdateConstraints();
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return NavigationController != null ? UIStatusBarStyle.Default : UIStatusBarStyle.LightContent;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public class NavDelegate2 : WKNavigationDelegate
        {
            TermsServiceViewController termsServiceViewController { get; set; }

            public NavDelegate2(TermsServiceViewController viewController)
            {
                termsServiceViewController = viewController;
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                Overlay.RemoveFromSuperview();
            }
        }
    }
}