using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using SocialLadder.Droid.Views.BindableViews;

namespace SocialLadder.Droid.CustomBindings
{
    public class ImageViewParamClickBinding : MvvmCross.Binding.Droid.Target.MvxAndroidTargetBinding
    {
        private readonly MvxParamCommandImageView _imageView;
        private MvxAsyncCommand<object> ClickCommand { get; set; }

        public ImageViewParamClickBinding(MvxParamCommandImageView imageView) : base(imageView)
        {
            _imageView = imageView;
            if (_imageView != null)
            {
                _imageView.Click += (s, e) =>
                {
                    if (ClickCommand != null)
                    {
                        ClickCommand.Execute(_imageView.ItemId);
                    }
                };
            }
        }

        public static void Register(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterFactory(new MvxCustomBindingFactory<MvxParamCommandImageView>("ClickParamCommand", (imageView) => new ImageViewParamClickBinding(imageView)));
        }

        public override Type TargetType
        {
            get { return typeof(MvxAsyncCommand<object>); }
        }

        public override void SetValue(object value)
        {
            if (value != null)
            {
                ClickCommand = (MvxAsyncCommand<object>)value;
            }
        }

        protected override void SetValueImpl(object target, object value)
        {
        }
    }
}