using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.Models.MessangerModels;

namespace SocialLadder.ViewModels.Challenges.ChallengesDetails
{
    public class PhotoPickerViewModel : BasePosteringViewModel
    {
        private readonly ILocationService _locationService;

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

        public MvxCommand _pickPhotoCommand;
        public MvxCommand PickPhotoCommand
        {
            get
            {
                if (_pickPhotoCommand == null)
                {
                    _pickPhotoCommand = new MvxCommand(PickImage);
                }
                return _pickPhotoCommand;
            }
        }

        private MvxInteraction<Action<List<LocalPosterModel>>> _takePhotoFromLibraryInteraction =  new MvxInteraction<Action<List<LocalPosterModel>>>();        
        public IMvxInteraction<Action<List<LocalPosterModel>>> TakePhotoFromLibraryInteraction => _takePhotoFromLibraryInteraction;
        private MvxInteraction<Action<List<LocalPosterModel>>> _takePhotoFromCameraInteraction = new MvxInteraction<Action<List<LocalPosterModel>>>();
        public IMvxInteraction<Action<List<LocalPosterModel>>> TakePhotoFromCameraInteraction => _takePhotoFromCameraInteraction;

        public PhotoPickerViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, ILocationService locationService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
           _token = _messenger.Subscribe<MessangerChallengeModel>(OnChallengeReceived);
            _locationService = locationService;
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _messenger.Unsubscribe<MessangerChallengeModel>(_token);
            base.ViewDestroy(viewFinishing);
        }

        private void PickImage()
        {
            List<ConfigurableAlertAction> actions = new List<ConfigurableAlertAction>();
            actions.Add(new ConfigurableAlertAction()
            {
                Text = ChallengesConstants.Camera,
                OnClickHandler = () =>
                {
                    _takePhotoFromCameraInteraction.Raise(OnImagesReceived);
                }
            });
            actions.Add(new ConfigurableAlertAction()
            {
                Text = ChallengesConstants.Library,
                OnClickHandler = () =>
                {
                    _takePhotoFromLibraryInteraction.Raise(OnImagesReceived);
                }
            });
            _alertService.ShowAlertMessage("Take pickture", actions); 
        } 

        private void OnChallengeReceived(MessangerChallengeModel challengeModel)
        {
            Challenge = challengeModel.Model;
            ChallengeName = Challenge.Name;
            RefereshImageCount();
        }

        private void OnImagesReceived(List<LocalPosterModel> posters)
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
    }
}
