using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using SocialLadder.Droid.Adapters.Challenges;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Views;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges.ChallengesDetails;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges.ChallengesDetails
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class PhotoGalleryFragment : BaseFragment<PhotoGalleryViewModel>
    {
        protected override int FragmentId => Resource.Layout.ChallengesImageGalleryView;

        protected override bool HasBackButton => false;

        private MvxRecyclerView _galleryCollection;
        private PhotoGalleryCollectionAdapter adapter;
        private Button btnSubmit;
        private TextView statusText;
        private TextView percentComplete;
        private View _view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view =  base.OnCreateView(inflater, container, savedInstanceState);
            btnSubmit = view.FindViewById<Button>(Resource.Id.btnSubmit);
            statusText = view.FindViewById<TextView>(Resource.Id.StatusText);
            percentComplete = view.FindViewById<TextView>(Resource.Id.percent_complete);

            adapter = new PhotoGalleryCollectionAdapter((IMvxAndroidBindingContext)BindingContext, ViewModel.Posters);
            adapter.OnAddDescriptionToPosterHandler = AddDesriptionToPoster;
            adapter.OnDeletePosterHandler = DeletePoster;
            adapter.NotifyDataSetChanged();
            LinearLayoutManager layoutManager = new LinearLayoutManager(this.Context, LinearLayoutManager.Horizontal, false); 

            _galleryCollection = view.FindViewById<MvxRecyclerView>(Resource.Id.gallery_layout);
            _galleryCollection.SetLayoutManager(layoutManager);
            _galleryCollection.Adapter = adapter;

            var captionText = view.FindViewById<EditText>(Resource.Id.CaptionText);
            captionText.FocusChange += (sender, e) =>
            {
                InputMethodManager imm = (InputMethodManager)Context.GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(captionText.WindowToken, 0);
            };
            UpdateFonts();
            
            _view = view;
            return view;
        }

        private void UpdateFonts()
        {
            FontHelper.UpdateFont(btnSubmit, FontsConstants.PN_R, (float)0.045);
            FontHelper.UpdateFont(statusText, FontsConstants.PN_B, (float)0.035);
            FontHelper.UpdateFont(percentComplete, FontsConstants.PN_B, (float)0.035);
        }

        private void AddDesriptionToPoster(LocalPosterModel poster)
        {
            ViewModel.Poster = poster;
            ViewModel.OpenDescriptionCommand.Execute(poster.Title);
        }

        private void DeletePoster(LocalPosterModel poster)
        {
            ViewModel.Poster = poster;
            ViewModel.DeletePosterCommand.Execute();
            adapter.NotifyDataSetChanged();
            _galleryCollection.SetAdapter(adapter);
            if (ViewModel.Progress == 0)
            {
                statusText.Text = "Add Photo";
                btnSubmit.SetTextColor(Android.Graphics.Color.LightGray);
            }
        }

        public override void OnResume()
        {
            base.OnResume();
            var challengeTriangle = _view.FindViewById<TriangleChallengeView>(Resource.Id.challenge_popup_triangle);
            challengeTriangle.SetParameters(0, 0, ViewModel as BaseChallengesViewModel);
            challengeTriangle.Invalidate();
        }
    }
}