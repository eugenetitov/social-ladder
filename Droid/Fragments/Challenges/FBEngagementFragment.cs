﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.PlatformService;
using SocialLadder.Droid.Views;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class FBEngagementFragment : BaseFragment<FBEngagementViewModel>, View.IOnScrollChangeListener
    {
        protected override int FragmentId => Resource.Layout.ChallengesFBEngagementLayout;
        protected override bool HasBackButton => true;

        private ScrollView parallaxScrollView;
        private ImageView parallaxImage;
        private ImageButton button;
        //private WebView _webView;
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            parallaxScrollView = view.FindViewById<ScrollView>(Resource.Id.parallax_scroll_view);
            parallaxImage = view.FindViewById<ImageView>(Resource.Id.parallax_image);
            parallaxScrollView.SetOnScrollChangeListener(this);
            UndateControls();
            button = view.FindViewById<ImageButton>(Resource.Id.photo_button);
            view.FindViewById<ImageButton>(Resource.Id.photo_button).Click += (s, e) =>
            {
                ViewModel.ChallengeSubmit(this.Activity);
            };

            //_webView = view.FindViewById<WebView>(Resource.Id.web_view);
            //WebSettings webSettings = _webView.Settings;
            //webSettings.TextZoom = 85;

            return view;
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.SubmitButtonAnimated))
            {
                if (ViewModel.SubmitButtonAnimated)
                {
                    AnimateButton(button, Resource.Drawable.ic_loadingIndicator);
                }
                if (!ViewModel.SubmitButtonAnimated)
                {
                    button.SetImageResource(Resource.Drawable.challenges_fb_btn);
                    button.ClearAnimation();
                }
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            var challengeTriangle = view.FindViewById<TriangleChallengeView>(Resource.Id.challenge_popup_triangle);
            challengeTriangle.SetParameters(0, 0, ViewModel as BaseChallengesViewModel);
            challengeTriangle.Invalidate();
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            parallaxImage.SetY(parallaxImage.Top - scrollY / 3);
            var imageParams = parallaxImage.LayoutParameters;
            imageParams.Width = DimensHelper.GetScreenWidth() + scrollY / 4;
            parallaxImage.LayoutParameters = imageParams;
        }

        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.title), FontsConstants.PN_R, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.time), FontsConstants.PN_R, (float)0.04);         
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_title_text), FontsConstants.PN_B, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_peopleCount_text), FontsConstants.PN_R, (float)0.045);
        }
    }
}