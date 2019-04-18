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
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.ViewModels.Intro;

namespace SocialLadder.Droid.Fragments.Intro
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(IntroContainerViewModel), FragmentContentId = Resource.Id.content_frame)]
    public class Intro4Fragment : BaseFragment<Intro4ViewModel>
    {
        protected override int FragmentId => Resource.Layout.Intro4Layout;
        protected override bool HasBackButton => false;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.textViewBottom), FontsConstants.PN_R, (float)0.055);
            return view;
        }
    }
}