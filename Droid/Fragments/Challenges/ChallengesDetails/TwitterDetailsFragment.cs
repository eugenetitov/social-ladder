using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SocialLadder.Droid.Views;
using SocialLadder.Extensions;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges.ChallengesDetails;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges.Details
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class TwitterDetailsFragment : BaseFragment<TwitterDetailsViewModel>
    {
        protected override int FragmentId => Resource.Layout.ChallengesTwitterDetailsLayout;
        protected override bool HasBackButton => true;

        private View view;
        private ImageButton submitButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            submitButton = view.FindViewById<ImageButton>(Resource.Id.submit_button);
            UndateControls();
            return view;
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.SubmitButtonImage))
            {
                if (ViewModel.GetSubmitButtonImageNormalName() != ViewModel.SubmitButtonImage)
                {
                    AnimateImage(submitButton);
                }
                if (ViewModel.GetSubmitButtonImageNormalName() == ViewModel.SubmitButtonImage)
                {
                    submitButton.ClearAnimation();
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
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.top_lable), FontsConstants.PN_R, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.count_lable), FontsConstants.PN_R, (float)0.045);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.middle_lable), FontsConstants.PN_B, (float)0.04);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.input_text), FontsConstants.PN_R, (float)0.05);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.link_lable), FontsConstants.PN_R, (float)0.05);
        }
    }
}