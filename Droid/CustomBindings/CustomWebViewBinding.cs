using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Platform.Platform;
using SocialLadder.Droid.Views.BindableViews;

namespace SocialLadder.Droid.CustomBindings
{
    public class CustomWebViewBinding : MvxPropertyInfoTargetBinding<BindableWebView>
    {
        private bool subscribed;

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public CustomWebViewBinding(object target, PropertyInfo targetPropertyInfo) : base(target, targetPropertyInfo)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = target as BindableWebView;
            if (view == null) return;

            view.Data = (string)value;
            view.LoadData((string)value, "text/html", "utf-8");
        }

        public override void SubscribeToEvents()
        {
            var myView = View;
            if (myView == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - MyView is null in MyViewMyPropertyTargetBinding");
                return;
            }

            subscribed = true;
            myView.CustomPropertyChanged += HandleMyPropertyChanged;
        }

        private void HandleMyPropertyChanged(object sender, EventArgs e)
        {
            var myView = View;
            if (myView == null) return;

            FireValueChanged(myView.Data);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (isDisposing)
            {
                var myView = View;
                if (myView != null && subscribed)
                {
                    myView.CustomPropertyChanged -= HandleMyPropertyChanged;
                    subscribed = false;
                }
            }
        }

    }
}