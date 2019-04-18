using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Challenges.ChallengesDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class FlyeringViewModel : BasePosteringViewModel
    {
        #region Properties
        private readonly ILocationService _locationService;
        public LocationModel CurrentLocation { get; set; }
        #endregion

        public FlyeringViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, ILocationService locationService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _locationService = locationService;
        }

        public async override Task Initialize()
        {
            await base.Initialize();
            IsBusy = true;
            CurrentLocation = await _locationService.GetCurrentLocation();
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            SubmitButtonAnimated = true;
            await base.ChallengeSubmit();
            _messenger.Unsubscribe<MessangerChallengeModel>(_token);
            if (Challenge.CompletedCount > 0)
            {
                await _navigationService.Navigate<SubmitionGalleryViewModel>();
                _messenger.Publish<MessangerImageGalleryModel>(new MessangerImageGalleryModel(this, new List<Models.LocalModels.Challenges.LocalPosterModel>(), Challenge));
                return;
            }
            if (Challenge.CompletedCount <= 0)
            {
                await _navigationService.Navigate<PhotoPickerViewModel>();
            }
            _messenger.Publish<MessangerChallengeModel>(new MessangerChallengeModel(this, Challenge));
            SubmitButtonAnimated = true;
        }
    }
}
