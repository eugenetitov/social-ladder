using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace SocialLadder.Droid.Helpers.WebViewHelpers
{
    public class WizardWebViewClient : WebViewClient
    {
        public Action ReceivedEscapeSequence;
        public Action LoadingFinished;
        public Action LoadingStarted;

        public WizardWebViewClient(Action loadingStarted, Action loadingFinished, Action receivedEscapeSequence)
        {
            LoadingStarted = loadingStarted;
            LoadingFinished = loadingFinished;
            ReceivedEscapeSequence = receivedEscapeSequence;
        }


        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            if (url.Contains("closebrowser"))
            {
                ReceivedEscapeSequence.Invoke();
            }
            view.SetBackgroundColor(Color.Transparent);
            return false;
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            view.SetBackgroundColor(Color.Transparent);
            base.OnPageStarted(view, url, favicon);
            LoadingStarted.Invoke();
        }

        public override void OnPageFinished(WebView view, string url)
        {
            view.SetBackgroundColor(Color.Transparent);
            base.OnPageFinished(view, url);
            LoadingFinished.Invoke();
        }
    }
}