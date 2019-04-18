using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;
using SocialLadder.Models.MessangerModels;

namespace SocialLadder.ViewModels.Challenges.ChallengesDetails
{
    public class PhotoGalleryViewModel : BasePosteringViewModel
    {
        #region Fields
        private ICloudinaryService _cloudinaryService;
        private IPlatformNavigationService _platformNavigationService;
        #endregion
        #region properties
        private List<LocalPosterModel> UploadedPosters { get; set; }

        private MvxObservableCollection<LocalPosterModel> _posters;
        public MvxObservableCollection<LocalPosterModel> Posters
        {
            get => _posters;
            set => SetProperty(ref _posters, value);
        }

        private LocalPosterModel _poster;
        public LocalPosterModel Poster
        {
            get => _poster;
            set => SetProperty(ref _poster, value);
        }

        private bool _needAddCaption;
        public bool NeedAddCaption
        {
            get => _needAddCaption;
            set => SetProperty(ref _needAddCaption, value);
        }

        private string _caption;
        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value);
        }

        private string _percent;
        public string Percent
        {
            get => _percent;
            set => SetProperty(ref _percent, value);
        }

        private int _progress;
        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }
        #endregion properties
        #region Commands
        private MvxAsyncCommand _submitCommand;
        public MvxAsyncCommand SubmitCommand
        {
            get
            {
                if (_submitCommand == null)
                {
                    _submitCommand = new MvxAsyncCommand(SubmitImages);

                }
                return _submitCommand;
            }
        }

        public MvxCommand<string> OpenDescriptionCommand
        {
            get
            {
                return new MvxCommand<string>((descr) =>
                {
                    if (!string.IsNullOrEmpty(descr))
                    {
                        Caption = descr;
                    }
                    NeedAddCaption = true;
                });
            }
        }

        public MvxCommand CloseDescriptionCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    NeedAddCaption = false;
                });
            }
        }

        public MvxCommand SaveDescriptionCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    NeedAddCaption = false;
                    int index = Posters.IndexOf(Poster);
                    Posters[index].Title = $"{Caption}";
                    Caption = null;
                });
            }
        }

        public MvxAsyncCommand DeletePosterCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    NeedAddCaption = false;
                    Posters.Remove(Poster);
                    UpdateProgressBar();
                    if (Posters.Count == 0)
                    {
                        await this.BackCommandInvoke();
                    }
                });
            }
        }
        #endregion Commands

        public PhotoGalleryViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, ICloudinaryService cloudinaryService, IMvxMessenger messanger, IPlatformNavigationService platformNavigationService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _token = _messenger.Subscribe<MessangerImageGalleryModel>(SetupImageGallery);
            _cloudinaryService = cloudinaryService;
            _platformNavigationService = platformNavigationService;
            NeedAddCaption = false;
            UploadedPosters = new List<LocalPosterModel>();
        }

        private void SetupImageGallery(MessangerImageGalleryModel model)
        {
            Challenge = model.Challenge;
            int countOfImages = model.Posters.Count + (int)Challenge.CompletedCount;
            int needToLeave = (int)Challenge.TargetCount - (int)Challenge.CompletedCount;
            if (countOfImages > Challenge.TargetCount)
            {
                model.Posters.RemoveRange(needToLeave, model.Posters.Count - needToLeave);
            }
            int _index = 1;
            foreach (var post in model.Posters)
            {
                post.Position = $"{model.Challenge.CompletedCount + _index} of {model.Challenge.TargetCount}";
                _index += 1;
            }
            Posters = new MvxObservableCollection<LocalPosterModel>(model.Posters);

            UpdateProgressBar();
            RefereshImageCount();
            _messenger.Unsubscribe<MessangerImageGalleryModel>(_token);
        }

        #region SubmitImages
        private async Task SubmitImages()
        {
            IsBusy = true;
            List<LocalPosterModel> photoUploadedPositions = new List<LocalPosterModel>();
            ChallengeResponseModel completedChallengeResponse = null;
            for (int i = 0; i < Posters.Count; i++)
            {
                if (!Posters[i].HasLocation || (RequiredLocation && Posters[i].Lat == 0 && Posters[i].Lon == 0))
                {
                    completedChallengeResponse = null;
                    continue;
                }
                var imagePublicId = await _cloudinaryService.Upload(Posters[i].Image);
                if (!string.IsNullOrEmpty(imagePublicId))
                {
                    completedChallengeResponse = await SL.Manager.PostSubmitContent(Challenge.ID, imagePublicId, Posters[i].Title, Posters[i].Lat, Posters[i].Lon, null);
                    PosteringCurentPhotoCount += 1;
                    photoUploadedPositions.Add(Posters[i]);
                }
                if (completedChallengeResponse.ResponseCode != 0)
                {
                    UploadedPosters.Add(Posters[i]);
                }
            }
            var unsubmitedPhotoCount = Posters.Count - photoUploadedPositions.Count;
            if (unsubmitedPhotoCount > 0)
            {
                var posters = Posters;
                foreach (var item in photoUploadedPositions)
                {
                    if (item != null)
                    {
                        posters.Remove(item);
                        Posters = posters;
                    }
                }
                _alertService.ShowSingle($"Unable to upload {unsubmitedPhotoCount} images. Press Submit to reload.");
                IsBusy = false;
                return;
            }
            await FinishSubmit(completedChallengeResponse);
            IsBusy = false;
        }

        private async Task FinishSubmit(ChallengeResponseModel completedChallengeResponse)
        {
            var isSubmited = SubmitContentResponse(completedChallengeResponse);
            if (!isSubmited)
            {
                if (completedChallengeResponse != null)
                {
                    _alertService.ShowToast(completedChallengeResponse.ResponseMessage);
                    await SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, (challenge) =>
                    {
                        Challenge = challenge.Challenge;
                    });
                }
                await _navigationService.Close(this);
                await _navigationService.Navigate<SubmitionGalleryViewModel>();
                _messenger.Publish<MessangerImageGalleryModel>(new MessangerImageGalleryModel(this, UploadedPosters, Challenge));
            }
            IsBusy = false;
        }

        public bool SubmitContentResponse(ChallengeResponseModel challengeResponse = null)
        {
            if (challengeResponse == null)
            {
                _alertService.ShowToast("Can't submit this content to server!");
                return false;
            }
            if (challengeResponse.ResponseCode == 0)
            {
                SubmitChallengeComplete(false, challengeResponse);
                return false;
            }
            if (PosteringCurentPhotoCount < (Challenge.TargetCount - PosteringTargetPhotoCount) && challengeResponse.ResponseMessage != "You completed that challenge!")
            {
                return false;
            }
            if (PosteringCurentPhotoCount >= (Challenge.TargetCount - PosteringTargetPhotoCount))
            {
                SubmitChallengeComplete(true, challengeResponse);
                return true;
            }
            return true;
        }
        #endregion

        public override async Task CloseDetailsView()
        {
            DetailsViewHidden = true;
            ChallengeWasFinished = true;

            if (PosteringCurentPhotoCount >= (Challenge.TargetCount - PosteringTargetPhotoCount))
            {
                _platformNavigationService.NavigateToTab(Enums.ENavigationTabs.CHALLENGES);
                return;
            }

            await _navigationService.Close(this);
            await _navigationService.Navigate<SubmitionGalleryViewModel>();
            _messenger.Publish<MessangerImageGalleryModel>(new MessangerImageGalleryModel(this, Posters.ToList(), Challenge));
        }

        private void UpdateProgressBar()
        {
            float addedCount = (Challenge.CompletedCount ?? 0);
            float needCount = (Challenge.TargetCount ?? 0);
            float currentCount = Posters.Count;

            if (needCount == currentCount)
            {
                Progress = 100;
                Percent = "100%";
            }
            else
            {
                Percent = $"{(int)((currentCount + addedCount) / needCount * 100)}%";
                Progress = (int)((currentCount + addedCount) / needCount * 100);
            }
        }
    }
}
