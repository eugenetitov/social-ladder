using MvvmCross.Core.Navigation;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class CheckInViewModel : BaseChallengesViewModel
    {
        #region Properties
        private readonly ILocationService _locationService;
        public LocationModel CurrentLocation { get; set; }
        #endregion

        public CheckInViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, ILocationService locationService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _locationService = locationService;
            IsBusy = true;
        }

        public async override Task Initialize()
        {
            await base.Initialize();
            CurrentLocation = await _locationService.GetCurrentLocation();
        }

        public async override Task ChallengeSubmit(object param = null)
        {
            await base.ChallengeSubmit();
            await SL.Manager.PostCheckInAndVerify(Challenge.ID, CurrentLocation.Lat, CurrentLocation.Long, (ChallengeResponseModel response) =>
            {
                if (response.ResponseCode <= 0 && string.IsNullOrEmpty(response.ResponseMessage))
                {
                    response.ResponseMessage = ChallengesConstants.ChallengesCheckinUnsuccessText;
                }
                SubmitChallengeComplete(true, response);
            });
            
        }

        public override byte[] GetArrayImage()
        {
            return null;
        }
    }
}
