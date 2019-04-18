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
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views.Attributes;

namespace SocialLadder.Droid.Helpers
{
    public class CustomPresenter : MvxAppCompatViewPresenter
    {
        public CustomPresenter(IEnumerable<Assembly> androidViewAssemblies) : base(androidViewAssemblies)
        {
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);
        }

        protected override void ShowActivity(Type view, MvxActivityPresentationAttribute attribute, MvxViewModelRequest request)
        {          
            base.ShowActivity(view, attribute, request);
        }

        protected override void ShowFragment(Type view, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            base.ShowFragment(view, attribute, request);
        }

        protected override bool CloseActivity(IMvxViewModel viewModel, MvxActivityPresentationAttribute attribute)
        {
            return base.CloseActivity(viewModel, attribute);
        }

        protected override bool CloseFragment(IMvxViewModel viewModel, MvxFragmentPresentationAttribute attribute)
        {
            return base.CloseFragment(viewModel, attribute);
        }
    }
}