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
using SocialLadder.Helpers;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class InviteFragment : BaseFragment<InviteViewModel>, View.IOnScrollChangeListener
    {
        protected override int FragmentId => Resource.Layout.ChallengesDetailsLayout;
        protected override bool HasBackButton => true;

        private ScrollView parallaxScrollView;
        private ImageView parallaxImage;
        private View view;
        private InviteService inviteService;
        private ImageButton imageButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            parallaxScrollView = view.FindViewById<ScrollView>(Resource.Id.parallax_scroll_view);
            parallaxImage = view.FindViewById<ImageView>(Resource.Id.parallax_image);
            parallaxScrollView.SetOnScrollChangeListener(this);
            UndateControls();
            view.FindViewById<ImageButton>(Resource.Id.submit_button).Click += (s, e) =>
            {
                ViewModel.ChallengeSubmit();
            };
            imageButton = view.FindViewById<ImageButton>(Resource.Id.submit_button);
            inviteService = new InviteService(this);



            return view;
        }

        public async override void OnResume()
        {
            base.OnResume();
            var challengeTriangle = view.FindViewById<TriangleChallengeView>(Resource.Id.challenge_popup_triangle);
            challengeTriangle.SetParameters(0, 0, ViewModel as BaseChallengesViewModel);
            challengeTriangle.Invalidate();
            if(NavigationHelper.NeedSubmitInviteChallenge)
            {
                await ViewModel.ChallengeSubmit();
                NavigationHelper.NeedSubmitInviteChallenge = false;
            }
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            parallaxImage.SetY(parallaxImage.Top - scrollY / 3);
            var imageParams = parallaxImage.LayoutParameters;
            imageParams.Width = DimensHelper.GetScreenWidth() + scrollY / 4;
            parallaxImage.LayoutParameters = imageParams;
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.NeedShowSendMenu))
            {
                if (ViewModel.NeedShowSendMenu)
                {
                    inviteService.ShowMenu(imageButton);
                    //inviteService.ShareLink("What kind of message?", ViewModel.GetInviteText);
                    ViewModel.NeedShowSendMenu = false;
                }
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.SubmitButtonAnimated))
            {
                if (ViewModel.SubmitButtonAnimated)
                {
                    AnimateButton(imageButton, Resource.Drawable.ic_loadingIndicator);
                }
                if (!ViewModel.SubmitButtonAnimated)
                {
                    imageButton.SetImageResource(Resource.Drawable.challenge_invite_btn);
                    imageButton.ClearAnimation();
                }
            }
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            inviteService.OnActivityResult(requestCode, resultCode, data);
        }

        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.title), FontsConstants.PN_R, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.time), FontsConstants.PN_R, (float)0.045);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_title_text), FontsConstants.PN_B, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_peopleCount_text), FontsConstants.PN_R, (float)0.045);
        }
    }
}