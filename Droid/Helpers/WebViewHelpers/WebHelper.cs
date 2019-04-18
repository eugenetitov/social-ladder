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
using Android.Views.Animations;
using Android.Webkit;
using Android.Widget;

namespace SocialLadder.Droid.Helpers
{
    public class WebHelper : WebViewClient
    {
        public Action PageStarted;
        public Action PageFinished;
        private bool loadingFinished = true;
        private bool redirect = false;

        public WebHelper()
        {

        }

        public WebHelper(Action pageStarted, Action pageFinished)
        {
            PageStarted = pageStarted;
            PageFinished = pageFinished;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            if (!loadingFinished)
            {
                redirect = true;
            }

            loadingFinished = false;

            view.LoadUrl(url);
            return true;
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            loadingFinished = false;

            PageStarted?.Invoke();
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, String url)
        {
            base.OnPageFinished(view, url);
            if (!redirect)
            {
                loadingFinished = true;
            }

            if (loadingFinished && !redirect)
            {
                PageFinished?.Invoke();
            }
            else
            {
                redirect = false;
            }

        }
    }
}