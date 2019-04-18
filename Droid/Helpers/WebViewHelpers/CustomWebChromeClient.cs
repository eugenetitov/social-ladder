using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace SocialLadder.Droid.Helpers.WebViewHelpers
{
    public class CustomWebChromeClient : WebChromeClient
    {
        public Action PageFinished;
        public CustomWebChromeClient(Action actionStop)
        {
            PageFinished = actionStop;
        }
        public override void OnProgressChanged(WebView view, int newProgress)
        {
            base.OnProgressChanged(view, newProgress);
            if (newProgress == 100)
            {
                PageFinished?.Invoke();
            }
        }
    }
}