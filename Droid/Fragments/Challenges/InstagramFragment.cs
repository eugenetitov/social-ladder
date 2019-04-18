using Android.Content;
using Android.OS;
using Android.Support.Constraints;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Models;
using SocialLadder.Droid.Views;
using SocialLadder.Enums.Constants;
using SocialLadder.Extensions;
using SocialLadder.Helpers;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Main;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SocialLadder.Droid.Fragments.Challenges
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class InstagramFragment : BaseFragment<InstagramViewModel>, View.IOnScrollChangeListener
    {
        protected override int FragmentId => Resource.Layout.ChallengesInstagramLayout;
        protected override bool HasBackButton => true;

        private ScrollView parallaxScrollView;
        private ImageView parallaxImage;
        private ImageButton button;
        private View view;
        private PhotoGalleryService photoGalleryService;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            parallaxScrollView = view.FindViewById<ScrollView>(Resource.Id.parallax_scroll_view);
            parallaxImage = view.FindViewById<ImageView>(Resource.Id.parallax_image);
            parallaxScrollView.SetOnScrollChangeListener(this);
            UndateControls();
            button = view.FindViewById<ImageButton>(Resource.Id.photo_button);
            photoGalleryService = new PhotoGalleryService(this, view.FindViewById<ImageButton>(Resource.Id.photo_button));
            return view;
        }

        public override void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewModelChanged(sender, e);
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.IsBusy))
            {
                if (ViewModel.Challenge.IsFixedContent && !ViewModel.IsBusy)
                {
                    photoGalleryService.UserImageWasAdded = true;
                }
            }
            if (e.PropertyName == PropertiesExtension.GetPropertyName(() => ViewModel.SubmitButtonAnimated))
            {
                if (ViewModel.SubmitButtonAnimated)
                {
                    AnimateButton(button, Resource.Drawable.ic_loadingIndicator);
                }
                if (!ViewModel.SubmitButtonAnimated)
                {
                    button.SetImageResource(Resource.Drawable.challenges_photo_btn);
                    button.ClearAnimation();
                }
            }
        }

        public async override void OnResume()
        {
            base.OnResume();
            var challengeTriangle = view.FindViewById<TriangleChallengeView>(Resource.Id.challenge_popup_triangle);
            challengeTriangle.SetParameters(0, 0, ViewModel as BaseChallengesViewModel);
            challengeTriangle.Invalidate();
            if (NavigationHelper.NeedSubmitInstagramChallenge)
            {
                await ViewModel.ChallengeSubmit();
                NavigationHelper.NeedSubmitInstagramChallenge = false;
            }
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
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.time), FontsConstants.PN_R, (float)0.045);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.hash_title), FontsConstants.SFP_R, (float)0.03);
            FontHelper.UpdateFont(view.FindViewById<EditText>(Resource.Id.hash_input), FontsConstants.SFP_R, (float)0.045);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.hash_hint), FontsConstants.SFP_R, (float)0.03);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.toast_text), FontsConstants.SFP_M, (float)0.031);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_title_text), FontsConstants.PN_B, (float)0.06);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.complete_peopleCount_text), FontsConstants.PN_R, (float)0.045);
        }

        public async override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == AndroidRequestCode.CameraRequestCode || requestCode == AndroidRequestCode.GalleryRequestCode)
            {
                var result = photoGalleryService.OnActivityResult(requestCode, resultCode, data);
                if (result != null)
                {
                    ViewModel.AddPhotoFromCameraOrLibrary(result);
                    await ViewModel.ShareToSocialNetwork();
                }
            }
            if (requestCode == AndroidRequestCode.InstagramRequestCode)
            {
                //Share console was open
            }
        }
    }
}