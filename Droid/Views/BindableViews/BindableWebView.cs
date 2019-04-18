using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace SocialLadder.Droid.Views.BindableViews
{
    public class BindableWebView : WebView
    {
        protected BindableWebView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public BindableWebView(Context context) : base(context)
        {
            Init();
        }

        public BindableWebView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public BindableWebView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init();
        }

        private void Init()
        {
            this.VerticalScrollBarEnabled = false;
            this.HorizontalScrollBarEnabled = false;
        }

        private string _data;
        public string Data
        {
            get { return _data; }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                _data = value;
            }
        }

        public Action<object, EventArgs> CustomPropertyChanged { get; internal set; }
    }
}