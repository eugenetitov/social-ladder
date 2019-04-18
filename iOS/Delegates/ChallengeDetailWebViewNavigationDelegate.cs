using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ObjCRuntime;
using SocialLadder.Logger;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.Delegates
{
    public class ChallengeDetailWebViewNavigationDelegate : WKNavigationDelegate
    {

        public event Action NavigationFinished;

        string GetLogInfo
        {
            get
            {
                return SL.Device.Model + " " + SL.Device.SystemVersion + " " + Constants.PlatformVersion + " " + SL.DeviceUUID;
            }
        }

        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            NavigationFinished();

            webView.EvaluateJavaScript("document.body.getBoundingClientRect().bottom", completionHandler: (heightHeight, heightError) =>
            {
                if (heightError != null)
                    LogHelper.LogUserMessage("WKNavigationDelegate:DidFinishNavigation", GetLogInfo + " " + heightError.Description);
            });
        }

        public override void DecidePolicy(WKWebView view, WKNavigationAction navAction, Action<WKNavigationActionPolicy> handler)
        {
            if (navAction.NavigationType == WKNavigationType.LinkActivated)
            {
                handler?.Invoke(WKNavigationActionPolicy.Cancel);
                view.BeginInvokeOnMainThread(() => UIApplication.SharedApplication.OpenUrl(navAction.Request.Url));
            }
            else
                handler?.Invoke(WKNavigationActionPolicy.Allow);
        }

        public override void ContentProcessDidTerminate(WKWebView webView)
        {
            LogHelper.LogUserMessage("WKNavigationDelegate:ContentProcessDidTerminate", GetLogInfo);
        }

        public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            LogHelper.LogUserMessage("WKNavigationDelegate:DidFailNavigation", GetLogInfo + " " + error.Description);
        }
        /*
        public override void DidCommitNavigation(WKWebView webView, WKNavigation navigation)
        {

        }
        */
        public override void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
            LogHelper.LogUserMessage("WKNavigationDelegate:DidFailProvisionalNavigation", GetLogInfo + " " + error.Description);
        }
        /*
        public override void DidReceiveAuthenticationChallenge(WKWebView webView, NSUrlAuthenticationChallenge challenge, [BlockProxy(typeof(Trampolines.NIDActionArity2V2))] Action<NSUrlSessionAuthChallengeDisposition, NSUrlCredential> completionHandler)
        {

        }

        public override void DidReceiveServerRedirectForProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {

        }

        public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
        {

        }
        */
    }
}