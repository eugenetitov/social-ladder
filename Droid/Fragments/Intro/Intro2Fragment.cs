﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
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
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(IntroContainerViewModel), FragmentContentId = Resource.Id.content_frame)]
    public class Intro2Fragment : BaseFragment<Intro2ViewModel>
    {
        protected override int FragmentId => Resource.Layout.Intro2Layout;
        protected override bool HasBackButton => false;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.textView1), FontsConstants.PN_R, (float)0.055);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.textView2), FontsConstants.PN_R, (float)0.055);
            return view;
        }
    }
}