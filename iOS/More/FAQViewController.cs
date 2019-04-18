using CoreGraphics;
using Foundation;
using ObjCRuntime;
using SocialLadder.iOS.ViewControllers;
using System;
using System.Diagnostics;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.More
{
    public partial class FAQViewController : UIViewController
    {
        public static UIView Overlay;

        public FAQViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //Overlay = Platform.AddOverlay(new CGRect(View.Frame.X, NavigationController.NavigationBar.Frame.GetMaxY(), View.Frame.Width, View.Frame.Height), UIColor.White, true, true, 0.1f);
            AddOverlay();
            WebView.NavigationDelegate = new NavDelegate2(this);
            // Need deviceUUID and AreaGUID as a parameter

            //http://socialladderapidev.cloudapp.net/SL/HelpDesk/AskAQuestion?deviceUUID=54D9521BD5404540AE099ACAB8C54611&AreaGUID=0CD0151C-D9AF-4852-88DD-AE4D27843103
            //var url = new NSUrl("http://socialladderapidev.cloudapp.net/SL/HelpDesk/AskAQuestion?deviceUUID="+SL.DeviceUUID+"&AreaGUID="+SL.AreaGUID);
            //var url = new NSUrl("https://socialladder.rkiapps.com/");
            //http://socialladderapidev.cloudapp.net/SL/helpdesk/askaquestion?deviceuuid=8275313D9D17455A888801D100DF07EE&areaguid=0CD0151C-D9AF-4852-88DD-AE4D27843103
            var url = new NSUrl(Constants.ServerUrl + "/SL/helpdesk/askaquestion?deviceuuid=" + SL.DeviceUUID+ "&areaguid=" + SL.AreaGUID);
            //var url = new NSUrl(Constants.ServerUrl+"/SL/helpdesk/askaquestion?deviceUUID="+SL.DeviceUUID+"&AreaGUID="+SL.AreaGUID);
            var request = new NSUrlRequest(url);
            WebView.LoadRequest(request);
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

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public class NavDelegate2 : WKNavigationDelegate
        {
            FAQViewController FaqViewController { get; set; }

            public NavDelegate2(FAQViewController viewController)
            {
                FaqViewController = viewController;
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