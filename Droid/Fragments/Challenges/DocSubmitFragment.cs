using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Views;
using SocialLadder.Enums.Constants;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class DocSubmitFragment : BaseFragment<DocSubmitViewModel>, View.IOnScrollChangeListener
    {
        protected override int FragmentId => Resource.Layout.ChallengesDocSubmitLayout;

        protected override bool HasBackButton => true;

        private ScrollView parallaxScrollView;
        private ImageView parallaxImage;
        private ImageButton button;
        private View view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            parallaxScrollView = view.FindViewById<ScrollView>(Resource.Id.parallax_scroll_view);
            parallaxImage = view.FindViewById<ImageView>(Resource.Id.parallax_image);
            parallaxScrollView.SetOnScrollChangeListener(this);

            button = view.FindViewById<ImageButton>(Resource.Id.photo_button);

            var docPath = view.FindViewById<EditText>(Resource.Id.hash_input);
            docPath.Click += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(docPath.Text))
                {
                    if (!docPath.Text.StartsWith("http://") && !docPath.Text.StartsWith("https://"))
                        docPath.Text = "http://" + docPath.Text;

                    Android.Net.Uri uri = Android.Net.Uri.Parse(docPath.Text);
                    Intent browserIntent = new Intent(Intent.ActionView, uri);
                    StartActivity(browserIntent);
                }
            };

            UndateControls();
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
                    button.SetImageResource(Resource.Drawable.challenge_docsubmit_btn);
                    button.ClearAnimation();
                }
            }

            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.Challenge))
            {
                if (ViewModel.Challenge != null && !string.IsNullOrEmpty(ViewModel.Challenge.Status) && ViewModel.Challenge.Status == ChallengesConstants.ChallengeStatusPending || ViewModel.Challenge.Status == ChallengesConstants.ChallengeStatusComplete)
                {
                    ViewModel.SubmitButtonHidden = true;
                }
                else
                {
                    ViewModel.SubmitButtonHidden = false;
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

        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.title), FontsConstants.PN_R, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.time), FontsConstants.PN_R, (float)0.045);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.hash_title), FontsConstants.SFP_R, (float)0.03);
            FontHelper.UpdateFont(view.FindViewById<EditText>(Resource.Id.hash_input), FontsConstants.SFP_R, (float)0.03);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.hash_hint), FontsConstants.SFP_R, (float)0.03);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.toast_text), FontsConstants.SFP_M, (float)0.031);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_title_text), FontsConstants.PN_B, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_peopleCount_text), FontsConstants.PN_R, (float)0.045);
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            parallaxImage.SetY(parallaxImage.Top - scrollY / 3);
            var imageParams = parallaxImage.LayoutParameters;
            imageParams.Width = DimensHelper.GetScreenWidth() + scrollY / 4;
            parallaxImage.LayoutParameters = imageParams;
        }
    }
}