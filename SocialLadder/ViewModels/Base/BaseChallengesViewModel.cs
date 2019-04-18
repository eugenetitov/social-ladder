using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using Plugin.Connectivity;
using SocialLadder.Converters;
using SocialLadder.Enums.Constants;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels;
using SocialLadder.Models.MessangerModels;

namespace SocialLadder.ViewModels.Base
{
    public class BaseChallengesViewModel : BaseViewModel, IBaseChildViewModel
    {
        public static bool ChallengeWasFinished { get; set; } = false;
        #region Propertyes
        private ChallengeModel _challenge;
        public ChallengeModel Challenge
        {
            get => _challenge;
            set
            {
                SetProperty(ref _challenge, value);
                if (value != null)
                {
                    //Task.Run(async () => { await ReadImageUrlToBytesArray(); });
                    ReadImageUrlToBytesArray();
                }
            }
        }

        private MvxObservableCollection<FeedItemModel> _feedItems;
        public MvxObservableCollection<FeedItemModel> FeedItems
        {
            get => _feedItems;
            set => SetProperty(ref _feedItems, value);
        }

        private byte[] _challengeImage;
        public byte[] ChallengeImage
        {
            get => _challengeImage;
            set => SetProperty(ref _challengeImage, value);
        }

        private bool _topMarginToCompleteView;
        public bool TopMarginToCompleteView
        {
            get => _topMarginToCompleteView;
            set => SetProperty(ref _topMarginToCompleteView, value);
        }

        private bool _feedItemCompleteViewHidden;
        public bool FeedItemCompleteViewHidden
        {
            get => _feedItemCompleteViewHidden;
            set => SetProperty(ref _feedItemCompleteViewHidden, value);
        }

        private bool _detailsViewHidden;
        public bool DetailsViewHidden
        {
            get => _detailsViewHidden;
            set => SetProperty(ref _detailsViewHidden, value);
        }

        private string _challengePointsText;
        public string ChallengePointsText
        {
            get => _challengePointsText;
            set => SetProperty(ref _challengePointsText, value);
        }

        private string _challengeMessageText;
        public string ChallengeMessageText
        {
            get => _challengeMessageText;
            set => SetProperty(ref _challengeMessageText, value);
        }

        private string _hexCompleteViewColor;
        public string HexCompleteViewColor
        {
            get => _hexCompleteViewColor;
            set => SetProperty(ref _hexCompleteViewColor, value);
        }

        private bool _submitButtonHidden;
        public bool SubmitButtonHidden
        {
            get => _submitButtonHidden;
            set => SetProperty(ref _submitButtonHidden, value);
        }

        private bool _submitButtonAnimated;
        public bool SubmitButtonAnimated
        {
            get => _submitButtonAnimated;
            set => SetProperty(ref _submitButtonAnimated, value);
        }

        private string _submitButtonImage;
        public string SubmitButtonImage
        {
            get => _submitButtonImage;
            set => SetProperty(ref _submitButtonImage, value);
        }
        #endregion
        public BaseChallengesViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger = null) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _token = _messenger.Subscribe<MessangerChallengeModel>(OnLocationMessage);
            DetailsViewHidden = true;
            TopMarginToCompleteView = true;
            FeedItemCompleteViewHidden = true;
            ChallengeWasFinished = false;
            HexCompleteViewColor = ChallengesConstants.ChallengesDetailsInstaBackground;
        }

        public override void ViewDisappeared()
        {
            base.ViewDisappeared();
            _messenger.Unsubscribe<MessangerChallengeModel>(_token);
        }

        private void OnLocationMessage(MessangerChallengeModel locationMessage)
        {
            if (locationMessage != null)
            {
                SL.Manager.RefreshChallengeDetail(locationMessage.Model.ChallengeDetailsURL, (ChallengeResponseModel response) =>
                {
                    SetChallengeModel(response);
                });
            }

        }

        public void SetRefreshImage()
        {
            ScoreImage = _assetService.NetworkConnectingLoaderImage;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            CurrentVM = this;
        }

        private async Task ReadImageUrlToBytesArray()
        {
            IsBusy = true;
            if (!string.IsNullOrEmpty(Challenge.Image))
            {
                await Task.Run(async () =>
                {
                    ChallengeImage = await ImageUrlToByteArrayLocalConverter.ReadImageUrlToBytesArray(Challenge.Image);
                });
            }
            IsBusy = false;
        }



        public virtual async void SubmitChallengeComplete(bool didSucceed, ResponseModel response = null)
        {
            ChallengesPhotoHelper.ChallengeImage = null;
            if(response == null)
            {
                await base.BackCommandInvoke();
                return;
            }
            ChallengeWasFinished = true;
            if (response.ResponseCode > 0)
            {
                HexCompleteViewColor = ChallengesConstants.ChallengesDetailsMCBackground;
                ChallengeMessageText = string.IsNullOrEmpty(response.ResponseMessage) ? ChallengesConstants.ChallengesCompleteSuccessText : response.ResponseMessage;
            }
            if(response.ResponseCode <= 0)
            {
                HexCompleteViewColor = ChallengesConstants.ChallengesDetailsFailBackground;
                ChallengeMessageText = string.IsNullOrEmpty(response.ResponseMessage) ? ChallengesConstants.ChallengesCompleteUnsuccessText : response.ResponseMessage; 
            }
            if(response is ChallengeResponseModel)
            {                
                if (Challenge.CollateralReview ?? false)
                {
                    ChallengePointsText = string.Empty;
                }
                else
                {
                    if(Challenge.PointsPerInstance != 0 || Challenge.CompletedCount != null)
                        ChallengePointsText = response.ResponseCode > 0 && Challenge.PointValue > 0 ? (Challenge.CompPointValue + (Challenge.PointsPerInstance * Challenge.CompletedCount)) + ChallengesConstants.ChallengesCompletePointsText : "";
                    else
                        ChallengePointsText = response.ResponseCode > 0 && Challenge.PointValue > 0 ? Challenge.CompPointValue + ChallengesConstants.ChallengesCompletePointsText : "";
                }
            }
            if(response is ShareResponseModel)
            {
                ChallengeMessageText = string.IsNullOrEmpty((response as ShareResponseModel).ResponseMessage) ? ChallengesConstants.ChallengesCompleteSuccessText : (response as ShareResponseModel).ResponseMessage;
                ChallengePointsText = (response as ShareResponseModel).ResponseCode > 0 && Challenge.PointValue > 0 ? Challenge.PointValue + ChallengesConstants.ChallengesCompletePointsText : "";
            }
            DetailsViewHidden = false;
        }

        public virtual byte[] GetArrayImage()
        {
            return ChallengeImage;
        }

        public virtual void SetChallengeModel(ChallengeResponseModel response)
        {
            Challenge = response.Challenge;
            WebViewData = new LocalWebDataModel { Data = Challenge.Desc };
            CheckStatus();
        }

        private void CheckStatus()
        {
            if (Challenge != null && !string.IsNullOrEmpty(Challenge.Status) && Challenge.Status == ChallengesConstants.ChallengeStatusPending || Challenge.Status == ChallengesConstants.ChallengeStatusComplete)
            {
                SubmitButtonHidden = true;
            }
            else
            {
                SubmitButtonHidden = false;
            }
        }

        public virtual async Task ChallengeSubmit(object param = null)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return;
            }
        }

        public async virtual Task CloseDetailsView()
        {
            DetailsViewHidden = true;

            if (ChallengeWasFinished)
            {
                Mvx.Resolve<IPlatformNavigationService>().NavigateToTab(Enums.ENavigationTabs.CHALLENGES);
            }           
        }

        #region Commands       
        public MvxAsyncCommand CloseDetailsViewCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    await CloseDetailsView();
                });
            }
        }

        public MvxAsyncCommand SubmitButtonCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    SubmitButtonAnimated = true;
                    await ChallengeSubmit();
                    SubmitButtonAnimated = false;
                });
            }
        }
        #endregion
    }
}
