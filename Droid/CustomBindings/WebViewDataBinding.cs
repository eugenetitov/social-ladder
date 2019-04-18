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
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platform.Platform;
using SocialLadder.Models.LocalModels;

namespace SocialLadder.Droid.CustomBindings
{
    public class WebViewDataBinding : MvvmCross.Binding.Droid.Target.MvxAndroidTargetBinding
    {
        private readonly WebView _webView;

        public WebViewDataBinding(WebView webView) : base(webView)
        {
            _webView = webView;
        }

        public static void Register(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxCustomBindingFactory<WebView>("Data", (webView) => new WebViewDataBinding(webView)));
        }

        public override Type TargetType
        {
            get { return typeof(LocalWebDataModel); }
        }

        protected override void SetValueImpl(object target, object value)
        {
            var webView = (WebView)target;
            try
            {
                if (value == null)
                {
                    return;
                }
                if (value.GetType() == typeof(LocalWebDataModel))
                {
                    var webData = value as LocalWebDataModel;
                    if (!string.IsNullOrEmpty(webData.Url))
                    {
                        webView.LoadUrl(webData.Url);
                        return;
                    }
                    if (!string.IsNullOrEmpty(webData.Data))
                    {
                        webView.LoadDataWithBaseURL(null, webData.Data, "text/html", "UTF-8", "about:blank");
                        return;
                    }
                }
                if (value.GetType() == typeof(string))
                {
                    var data = (string)value;
                    if (!string.IsNullOrEmpty(data))
                    {
                        //webView.LoadData(data, "text/html", "utf-8");
                        webView.LoadDataWithBaseURL(null, data, "text/html", "UTF-8", "about:blank");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MvxTrace.Error(ex.Message);
                //throw;
                return;
            }   
            MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value was not a valid data");
        }

    }
}
