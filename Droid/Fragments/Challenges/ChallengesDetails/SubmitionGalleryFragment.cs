using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Droid.Assets;
using SocialLadder.Droid.Fragments.BaseFragment;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Models;
using SocialLadder.Droid.Models.MvxMessanger;
using SocialLadder.Droid.Views;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Challenges.ChallengesDetails;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.Droid.Fragments.Challenges.ChallengesDetails
{
    [MvxFragmentPresentation(ActivityHostViewModelType = typeof(MainViewModel), AddToBackStack = true, FragmentContentId = Resource.Id.content_frame_full)]
    public class SubmitionGalleryFragment : BaseFragment<SubmitionGalleryViewModel>
    {
        protected override int FragmentId => Resource.Layout.ChallengesSubmitionGalleryView;

        protected override bool HasBackButton => true;

        private MvxRecyclerView _recyclerView;
        private View view;
        private Action<List<LocalPosterModel>> _onTakePhotohandler;
        private IMvxMessenger _messanger;
        private MvxSubscriptionToken _token;
        private PhotoGalleryService _photoGalleryService;

        private IMvxInteraction<Action<List<LocalPosterModel>>> _libraryInteraction;
        public IMvxInteraction<Action<List<LocalPosterModel>>> LibraryInteraction
        {
            get => _libraryInteraction;
            set
            {
                if (_libraryInteraction != null)
                    _libraryInteraction.Requested -= TakePhotoFromLibrary;

                _libraryInteraction = value;
                _libraryInteraction.Requested += TakePhotoFromLibrary;
            }
        }


        private IMvxInteraction<Action<List<LocalPosterModel>>> _cameraInteraction;
        public IMvxInteraction<Action<List<LocalPosterModel>>> CameraInteraction
        {
            get => _cameraInteraction;
            set
            {
                if (_cameraInteraction != null)
                    _cameraInteraction.Requested -= TakePhotoFromCamera;

                _cameraInteraction = value;
                _cameraInteraction.Requested += TakePhotoFromCamera;
            }
        }

        private View _view;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            view = base.OnCreateView(inflater, container, savedInstanceState);
            _photoGalleryService = new PhotoGalleryService(this, null);
            _recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.image_collection);

            _messanger = Mvx.Resolve<IMvxMessenger>();

            GridLayoutManager layoutManager = new GridLayoutManager(this.Context, 3);
            _recyclerView.SetLayoutManager(layoutManager);

            _view = view;

            var set = this.CreateBindingSet<SubmitionGalleryFragment, SubmitionGalleryViewModel>();
            set.Bind(this).For(v => v.LibraryInteraction).To(viewModel => viewModel.TakePhotoFromLibraryInteraction).OneWay();
            set.Bind(this).For(v => v.CameraInteraction).To(viewModel => viewModel.TakePhotoFromCameraInteraction).OneWay();
            set.Apply();

            UndateControls();
            return view;
        }


        private void UndateControls()
        {
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.ChallengeName), FontsConstants.PN_B, (float)0.057);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.current_count), FontsConstants.PN_B, (float)0.042);
            FontHelper.UpdateFont(view.FindViewById<TextView>(Resource.Id.total_count), FontsConstants.PN_B, (float)0.042);
        }

        public override void OnResume()
        {
            base.OnResume();
            
            var challengeTriangle = _view.FindViewById<TriangleChallengeView>(Resource.Id.challenge_popup_triangle);
            challengeTriangle.SetParameters(0, 0, ViewModel as BaseChallengesViewModel);
            challengeTriangle.Invalidate();
        }

        private void TakePhotoFromLibrary(object sender, MvxValueEventArgs<Action<List<LocalPosterModel>>> eventArgs)
        {
            _onTakePhotohandler = eventArgs.Value;

            _token = _messanger.Subscribe<ImagePickerMessangerModel>(OnImagePicked);

            Com.Nguyenhoanglam.Imagepicker.UI.Imagepicker.ImagePicker
                .With(this.Activity)
                .SetShowCamera(false)
                .SetMultipleMode(true)
                .Start();
        }

        private void TakePhotoFromCamera(object sender, MvxValueEventArgs<Action<List<LocalPosterModel>>> eventArgs)
        {
            _onTakePhotohandler = eventArgs.Value;

            _token = _messanger.Subscribe<ImagePickerMessangerModel>(OnImagePicked);
            
            Activity.RunOnUiThread(() => 
            {
                if (!PermissionHelper.CheckPermissions(Activity, new List<string> { Manifest.Permission.Camera }))
                {
                    var _alertService = Mvx.Resolve<IAlertService>();
                    _alertService.ShowToast("Please, go to settings and turn on access to camera for this app");
                }
                else
                    _photoGalleryService.GetImageFromCamera();
            });
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Activity.RunOnUiThread(() => {
                var result = _photoGalleryService.OnActivityResult(requestCode, resultCode, data);
                if (result != null)
                {
                    var hasLocation = _photoGalleryService.imageLat != 0 && _photoGalleryService.imageLong != 0 ? true : false;
                    _onTakePhotohandler(new List<LocalPosterModel> { new LocalPosterModel {
                        Image = result,
                        Lat = _photoGalleryService.imageLat,
                        Lon = _photoGalleryService.imageLong,
                        CreatedFromCamera = true,
                        HasLocation = (ViewModel.RequiredLocation ? hasLocation : true) }
                    });
                }
            });
        }

        private async void OnImagePicked(ImagePickerMessangerModel result)
        {
            _messanger.Unsubscribe<ImagePickerMessangerModel>(_token);
            List<LocalPosterModel> imageBytes = new List<LocalPosterModel>();
            List<Com.Nguyenhoanglam.Imagepicker.Model.Image> images = result.Images.Cast<Com.Nguyenhoanglam.Imagepicker.Model.Image>().ToList();
            var locationService = Mvx.Resolve<ILocationService>();
            var curentLocation = await locationService.GetCurrentLocation();
            foreach (var image in images)
            {
                if (image.Path.StartsWith("file:"))
                {
                    image.Path = image.Path.Remove(0, 5);
                }

                _photoGalleryService.SetImageUriFromFragment(image.Path);
                var photo = _photoGalleryService.OnActivityResult(AndroidRequestCode.CameraRequestCode, -1, null);
                var hasLocation = _photoGalleryService.imageLat == 0 && _photoGalleryService.imageLat == 0 ? false : true;
                imageBytes.Add(new LocalPosterModel()
                {
                    Image = photo,
                    Lat = _photoGalleryService.imageLat,
                    Lon = _photoGalleryService.imageLong,
                    HasLocation = (ViewModel.RequiredLocation ? hasLocation : true)
                });

            }
            _onTakePhotohandler(imageBytes);
        }
        
    }
}