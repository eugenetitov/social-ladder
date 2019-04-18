using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Challenges
{
    public class BasePosteringViewModel : BaseChallengesViewModel
    {
        public readonly int _maxImages = 1000000000;
        

        #region Properties
        private bool _imageCountViewHidden;
        public bool ImageCountViewHidden
        {
            get => _imageCountViewHidden;
            set => SetProperty(ref _imageCountViewHidden, value);
        }

        private int _posteringTargetPhotoCount;
        public int PosteringTargetPhotoCount
        {
            get => _posteringTargetPhotoCount;
            set => SetProperty(ref _posteringTargetPhotoCount, value);
        }

        private int _posteringCurentPhotoCount;
        public int PosteringCurentPhotoCount
        {
            get => _posteringCurentPhotoCount;
            set => SetProperty(ref _posteringCurentPhotoCount, value);
        }

        private string _posteringTotalPhotoCount;
        public string PosteringTotalPhotoCount
        {
            get => _posteringTotalPhotoCount;
            set
            {
                var totalCount = " / " + value;
                SetProperty(ref _posteringTotalPhotoCount, totalCount);
            }
        }

        private bool _requiredLocation;
        public bool RequiredLocation
        {
            get => _requiredLocation;
            set
            {
                SetProperty(ref _requiredLocation, value);
            }
        }

        #endregion



        public BasePosteringViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger = null) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
        }

        public async override void ViewAppeared()
        {
            if (Challenge != null && !string.IsNullOrEmpty(Challenge.ChallengeDetailsURL))
            {
                IsBusy = true;
                await SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, (model) =>
                {
                    if (model.Challenge == null)
                    {
                        return;
                    }
                    Challenge = model.Challenge;
                    PosteringTargetPhotoCount = (int)model.Challenge.CompletedCount;
                });
                IsBusy = false;
            }
            base.ViewAppeared();
            if (ChallengeWasFinished)
            {
                await Task.Delay(1000);
                await BackCommandInvoke();
            }
        }

        public override void SetChallengeModel(ChallengeResponseModel response)
        {
            base.SetChallengeModel(response);
            //SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, OnShareTemplateRefereshed);
            RequiredLocation = response.Challenge != null && response.Challenge.LocationLat != null && response.Challenge.LocationLong != null ? true : false;
            RefereshImageCount();


        }

        public void RefereshImageCount()
        {
            if ((Challenge.TargetCount ?? 0) == 1 || Challenge.Status == ChallengesConstants.ChallengeStatusPending || Challenge.Status == ChallengesConstants.ChallengeStatusComplete)
            {
                ImageCountViewHidden = true;
            }
            else
            {
                ImageCountViewHidden = false;
                PosteringTargetPhotoCount = (Challenge.CompletedCount ?? 0);
                PosteringTotalPhotoCount = ((Challenge.TargetCount ?? 0) == 0 ? "∞" : Challenge.TargetCount.ToString());
            }
            if (Challenge == null)
            {
                return;
            }
            if (Challenge.Status == ChallengesConstants.ChallengeStatusPending || Challenge.Status == ChallengesConstants.ChallengeStatusComplete ||
                ((Challenge.CompletedCount ?? 0) >= ((Challenge.TargetCount ?? 0) == 0 ? _maxImages : Challenge.TargetCount.Value) && (Challenge.CollateralReview ?? false) && !(Challenge.AllowUserCompletion ?? false)))
            {
                SubmitButtonHidden = true;
                CollateralDisplayCountIfNeed();
                return;
            }
        }

        private void CollateralDisplayCountIfNeed()
        {
            //if ((Challenge.TargetCount ?? 0) == 1 || Challenge.Status == "Pending" || Challenge.Status == "Complete")
            //{
            //    vImagesCount.Hidden = true;
            //    cnImagesCountHeight.Constant = 0;
            //    cnMarginCount.Constant = 0;
            //}
            //else
            //{
            //    vImagesCount.Hidden = false;
            //    vImagesCount.ClipsToBounds = false;

            //    cnImagesCountHeight.Constant = SizeConstants.ScreenWidth / 13;
            //    cnMarginCount.Constant = SizeConstants.ScreenWidth / 15;

            //    vImagesCount.Layer.BorderColor = UIColor.FromRGB(238, 238, 238).CGColor;
            //    vImagesCount.Layer.BorderWidth = 1;
            //    vImagesCount.Layer.CornerRadius = 2;

            //    UploadedCountText1.Text = (Challenge.CompletedCount ?? 0).ToString();
            //    NeedUploadCountText1.Text = "/ " + ((Challenge.TargetCount ?? 0) == 0 ? "∞" : Challenge.TargetCount.ToString());
            //}
        }
    }
}
