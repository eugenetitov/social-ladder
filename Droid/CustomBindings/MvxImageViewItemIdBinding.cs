using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Platform.Platform;
using SocialLadder.Droid.Views.BindableViews;

namespace SocialLadder.Droid.CustomBindings
{
    public class MvxImageViewItemIdBinding : MvvmCross.Binding.Droid.Target.MvxAndroidTargetBinding
    {
        private readonly MvxParamCommandImageView _imageView;

        public MvxImageViewItemIdBinding(MvxParamCommandImageView imageView) : base(imageView)
        {
            _imageView = imageView;
        }

        public static void Register(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxCustomBindingFactory<MvxParamCommandImageView>("ItemId", (imageView) => new MvxImageViewItemIdBinding(imageView)));
        }

        #region implemented abstract members of MvxTargetBinding

        public override Type TargetType
        {
            get { return typeof(string); }
        }

        #endregion

        #region implemented abstract members of MvxConvertingTargetBinding

        protected override void SetValueImpl(object target, object value)
        {
            var itemId = (string)value;
            if (!string.IsNullOrEmpty(itemId))
            {
                var imageView = (MvxParamCommandImageView)target;
                try
                {
                    imageView.ItemId = itemId;
                }
                catch (Exception ex)
                {
                    MvxTrace.Error(ex.Message);
                    throw;
                }
            }
            else
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value was not a valid data");
            }
        }
        #endregion
    }
}