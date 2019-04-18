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
using Android.Widget;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;

namespace SocialLadder.Droid.Views.BindableViews
{
    public class MvxParamCommandImageView : MvxImageView
    {
        public IMvxAsyncCommand<string> ItemActionCommand { get; set; }
        public string ItemId { get; set; }
        public Action<object, EventArgs> CustomPropertyChanged { get; internal set; }

        protected MvxParamCommandImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        public MvxParamCommandImageView(Context context) : base(context)
        {
            Init();
        }

        public MvxParamCommandImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public MvxParamCommandImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init();
        }

        private void Init()
        {
        }
    }
}