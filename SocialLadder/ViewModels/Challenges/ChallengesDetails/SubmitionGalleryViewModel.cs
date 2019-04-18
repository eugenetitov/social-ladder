using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Main;

namespace SocialLadder.ViewModels.Challenges.ChallengesDetails
{
    public class SubmitionGalleryViewModel : BasePosteringViewModel
    {
        #region Fields
        private ICloudinaryService _cloudinaryService;
        private readonly IPlatformNavigationService _platformNavigationService;
        private readonly ILocationService _locationService;
        private bool _isUnlimitedImages;
        private bool _allowUserCompletion;
        private const int MaxImages = 1000000000;
        #endregion
        #region Properties
        private List<LocalPosterModel> _posters;
        public List<LocalPosterModel> Posters
        {
            get
            {
                return _posters;
            }
            set
            {
                _posters = value;
                RaisePropertyChanged(() => Posters);
            }
        }

        private string _challengeName;
        public string ChallengeName
        {
            get
            {
                return _challengeName;
            }
            set
            {
                _challengeName = value;
                RaisePropertyChanged(() => ChallengeName);
            }
        }

        private bool _isSubmitionEnabled;
        public bool IsSubmitionEnabled
        {
            get => _isSubmitionEnabled;
            set => SetProperty(ref _isSubmitionEnabled, value);
        }
        #endregion
        #region Commands
        private MvxAsyncCommand _takePictureCommand;
        public IMvxAsyncCommand TakePictureCommand
        {
            get
            {
                if (_takePictureCommand == null)
                {
                    _takePictureCommand = new MvxAsyncCommand(TakePicture);
                }
                return  _takePictureCommand;
            }
        }

        private MvxAsyncCommand _completeChallengeCommand;
        public IMvxAsyncCommand CompleteChallengeCommand
        {
            get
            {
                if (_completeChallengeCommand == null)
                {
                    _completeChallengeCommand = new MvxAsyncCommand(CompleteChallenge);
                }
                return _completeChallengeCommand;
            }
        }
        #endregion
        #region Interactions
        private MvxInteraction<Action<List<LocalPosterModel>>> _takePhotoFromLibraryInteraction = new MvxInteraction<Action<List<LocalPosterModel>>>();
        public IMvxInteraction<Action<List<LocalPosterModel>>> TakePhotoFromLibraryInteraction => _takePhotoFromLibraryInteraction;
        private MvxInteraction<Action<List<LocalPosterModel>>> _takePhotoFromCameraInteraction = new MvxInteraction<Action<List<LocalPosterModel>>>();
        public IMvxInteraction<Action<List<LocalPosterModel>>> TakePhotoFromCameraInteraction => _takePhotoFromCameraInteraction;
        #endregion

        public SubmitionGalleryViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, ICloudinaryService cloudinaryService, IMvxMessenger messanger, IPlatformNavigationService platformNavigationService, ILocationService locationService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _token = _messenger.Subscribe<MessangerImageGalleryModel>(OnPostersReceived);
            _cloudinaryService = cloudinaryService;
            _locationService = locationService;
            _platformNavigationService = platformNavigationService;
            IsSubmitionEnabled = false;
        }

        #region UpdateChallengeMethods
        private async Task CompleteChallenge()
        {
            if (Challenge == null || Posters == null)
            {

                return;
            }
            string message = $"You have submitted {Challenge.CompletedCount}/{(_isUnlimitedImages ? "∞" : Challenge.TargetCount.ToString())} pieces of content - would you like to complete this challenge now, or leave it open to submit more later?";
            List<ConfigurableAlertAction> actions = new List<ConfigurableAlertAction>();
            actions.Add(new ConfigurableAlertAction() { OnClickHandler = SubmitChallenge, Text = "COMPLETE NOW" });
            actions.Add(new ConfigurableAlertAction() { OnClickHandler = NavigateToMain, Text = "LEAVE OPEN" });

            _alertService.ShowAlertMessage(message, actions);

        }
        private void SubmitChallenge()
        {
            SL.Manager.PostSubmitCollateral(Challenge.ID, (resp) =>
            {
                if (Posters.Count + Challenge.CompletedCount == Challenge.TargetCount || (Posters.Count > 0 && Challenge.AllowUserCompletion == true))
                    SubmitChallengeComplete(false, resp);
                else
                {
                    List<ConfigurableAlertAction> actions = new List<ConfigurableAlertAction>();
                    actions.Add(new ConfigurableAlertAction() { OnClickHandler = () => { NavigateToMain(); }, Text = "OK" });
                    _alertService.ShowAlertMessage("Your content was accepted", actions);
                }
            });
        }
        #endregion
        #region OtherMethods
        private void NavigateToMain()
        {
            _platformNavigationService.NavigateToTab(Enums.ENavigationTabs.CHALLENGES);
        }

        private async Task TakePicture()
        {
            List<ConfigurableAlertAction> actions = new List<ConfigurableAlertAction>();
            actions.Add(new ConfigurableAlertAction()
            {
                Text = "Camera",
                OnClickHandler = () =>
                {
                    _takePhotoFromCameraInteraction.Raise(OnImagesReceivedAsync);
                }
            });
            actions.Add(new ConfigurableAlertAction()
            {
                Text = "Library",
                OnClickHandler = () =>
                {
                    _takePhotoFromLibraryInteraction.Raise(OnImagesReceivedAsync);
                }
            });
            _alertService.ShowAlertMessage("Take picture", actions);
        }
        #endregion
        #region ReceivedMethods
        private void OnImagesReceivedAsync(List<LocalPosterModel> posters)
        {
            Task.Run(async () => {
                var currentLocation = await _locationService.GetCurrentLocation();
                foreach (var image in posters)
                {
                    image.Lat = (image.Lat == 0 ? currentLocation.Lat : image.Lat);
                    image.Lon = (image.Lon == 0 ? currentLocation.Long : image.Lon);
                    if (image.CreatedFromCamera && !image.HasLocation && image.Lat != 0 && image.Lon != 0)
                    {
                        image.HasLocation = true;
                    }
                }
                IsBusy = true;
                await _navigationService.Navigate<PhotoGalleryViewModel>();
                _messenger.Publish<MessangerImageGalleryModel>(new MessangerImageGalleryModel(this, posters, Challenge));
            });
        }

        private async void OnPostersReceived(MessangerImageGalleryModel model)
        {
            if (model == null || model.Challenge == null)
            {
                _alertService.ShowToast("An error was occurred.");
                return;
            }
            Challenge = model.Challenge;
            ChallengeName = model.Challenge?.Name;
            Posters = model.Posters;
            var targetCount = Challenge.TargetCount == null ? 0 : Challenge.TargetCount.Value;
            RefereshImageCount();

            if (Challenge.TargetCount.HasValue)
            {
                _isUnlimitedImages = targetCount >= MaxImages;
            }
            PosteringTargetPhotoCount = Challenge.CompletedCount == null ? 0 : (int)Challenge.CompletedCount;
            if (Challenge.AllowUserCompletion.HasValue)
            {
                _allowUserCompletion = Challenge.AllowUserCompletion.Value;
            }
            if (_isUnlimitedImages || (_allowUserCompletion && PosteringTargetPhotoCount > 0) || (!_allowUserCompletion && PosteringTargetPhotoCount == targetCount))
            {
                IsSubmitionEnabled = true;
            }
            _messenger.Unsubscribe<MessangerImageGalleryModel>(_token);
        }
        #endregion
    }
}
