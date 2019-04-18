using CoreGraphics;
using Foundation;
using GMImagePicker;
using Photos;
using SocialLadder.iOS.Camera;
using SocialLadder.iOS.Constraints;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class PosteringViewController : CameraViewController
    {
        #region Fields & Properties
        private CameraPicture ImageToUpload { get; set; }
        private string placeholder = "Description goes here...";
        private CollateralCollectionSource _collectionSource;

        private List<CameraPicture> Images { get; set; }
        private List<CameraPicture> ImagesAttach { get; set; }
        private List<ChallengeResponseModel> Responses { get; set; }
        private bool IsFileSizeError;
        private int ImagesToAddCount;
        private int CallbacksCount;
        private int TargetCount;
        private int CompletedCount =>
            Challenge.CompletedCount ?? 0;
        private int AllImagesCount =>
            ImagesAttach.Count + Images.Count + CompletedCountInitial;
        private bool IsSingleImage;
        private bool IsUnlimitedImages;
        private bool AllowUserCompletion;
        private UIView PosteringOverlay { get; set; }
        private UIView Spinner;
        private int CompletedCountInitial;
        private const int MAX_IMAGES = 1000000000;
        #endregion

        #region ctor
        public PosteringViewController(IntPtr handle) : base(handle)
        {
            Responses = new List<ChallengeResponseModel>();
            Images = new List<CameraPicture>();
            ImagesAttach = new List<CameraPicture>();
        }
        #endregion

        #region Lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CompletedCountInitial = Challenge.CompletedCount ?? 0;
            TargetCount = (Challenge.TargetCount ?? 0) == 0 ? MAX_IMAGES : Challenge.TargetCount.Value;
            IsSingleImage = TargetCount == 1;
            IsUnlimitedImages = TargetCount == MAX_IMAGES;
            AllowUserCompletion = (Challenge.AllowUserCompletion ?? false) || IsUnlimitedImages;

            SetupFonts();

            if (CompletedCount == 0)
            {
                ShowNoImageView();
            }
            else if (CompletedCount > 0)
            {
                ShowImagesCollectionView();
            }

            CountContainer.Layer.BorderColor = UIColor.FromRGB(238, 238, 238).CGColor;
            CountContainer.Layer.BorderWidth = 1;
            CountContainer.Layer.CornerRadius = 2;

            CameraButtonCenter.TouchUpInside += (e, s) => TakePicture(true);

            btnSubmitPictureWithDescription.TouchUpInside += (e, s) =>
            {
                if (IsSingleImage)
                {
                    Spinner = new UIImageView(UIImage.FromBundle("loading-indicator"));
                    PreviewImage.AddSubview(Spinner);
                    Platform.AnimateRotation(Spinner);
                    PosteringOverlay = Platform.AddOverlay(View, View.Frame, UIColor.Clear, true);

                    CountContainer.AddConstraint(NSLayoutConstraint.Create(CountContainer, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, 0));
                }
                else
                {
                    ShowImagesCollectionView();
                }
                SubmitChallenge(btnSubmitChallenge);
            };

            CameraButton.TouchUpInside += (e, s) => TakePicture(true);
            btnSubmitChallenge.TouchUpInside += (e, s) =>
            {
                UIAlertController alertController = UIAlertController.Create(null,
                    $"You have submitted {CompletedCount}/{(IsUnlimitedImages ? "∞" : TargetCount.ToString())} pieces of content - would you like to complete this challenge now, or leave it open to submit more later?", UIAlertControllerStyle.ActionSheet);

                alertController.AddAction(UIAlertAction.Create("COMPLETE NOW", UIAlertActionStyle.Destructive, (a) => SL.Manager.PostSubmitCollateral(Challenge.ID, (resp) => SubmitChallengeComplete(btnSubmitChallenge, resp))));
                alertController.AddAction(UIAlertAction.Create("LEAVE OPEN", UIAlertActionStyle.Cancel, (a) => {
                    NavigationController.PopViewController(true);
                    NavigationController.PopViewController(false);
                }));
                NavigationController.PresentViewController(alertController, true, null);
            };

            DescriptionText.ReturnKeyType = UIReturnKeyType.Done;
            DescriptionText.ShouldChangeText += DescriptionText_ShouldChangeText;

            DescriptionText.ShouldBeginEditing += (UITextView textView) =>
            {
                if (textView.Text == placeholder)
                {
                    textView.Text = "";
                    textView.TextColor = UIColor.Black; // Text Color
                }
                return true;
            };

            DescriptionText.ShouldEndEditing += (UITextView textView) =>
            {
                if (textView.Text == "")
                {
                    textView.Text = placeholder;
                    textView.TextColor = UIColor.LightGray; // Placeholder Color
                }
                return true;
            };

            lblChallengeName.Text = Challenge.Name;
            NeedUploadCountText.Text = "/ " + (IsUnlimitedImages ? "∞" : TargetCount.ToString());

            CollectionView.RegisterNibForCell(CollateralCollectionViewCell.Nib, CollateralCollectionViewCell.ClassName);
            CollectionViewAttach.RegisterNibForCell(CollateralCollectionViewCell.Nib, CollateralCollectionViewCell.ClassName);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (_collectionSource == null)
            {
                var inset = SizeConstants.ScreenWidth / 25;
                var inset2 = SizeConstants.ScreenWidth / 17;

                _collectionSource = new CollateralCollectionSource(Images);
                CollectionView.Source = _collectionSource;
                CollectionView.CollectionViewLayout = new UICollectionViewFlowLayout
                {
                    ItemSize = new CGSize(SizeConstants.ScreenWidth / 3.75, SizeConstants.ScreenWidth / 3.75),
                    ScrollDirection = UICollectionViewScrollDirection.Vertical,
                    SectionInset = new UIEdgeInsets(inset2, inset2, inset2, inset2),
                    MinimumInteritemSpacing = inset,
                    MinimumLineSpacing = inset
                };

                CollectionViewAttach.Source = CollectionView.Source;
                CollectionViewAttach.CollectionViewLayout = CollectionView.CollectionViewLayout;

                RefreshCollections();
            }
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            if (Spinner != null)
            {
                var w = PreviewImage.Bounds.Width;
                var h = PreviewImage.Bounds.Height;
                Spinner.Frame = new CGRect((w - w * 0.25f) / 2, (h - w * 0.25f) / 2, w * 0.25f, w * 0.25f);
            }
        }
        #endregion

        #region Events
        public override void OnTakePictureComplete(CameraPicture picture)
        {
            if (picture == null)
                return;
            ImagesAttach.Clear();
            ImagesToAddCount = 1;
            CallbacksCount = 0;

            AddImagesIfRequired(picture);
        }

        public override void OnTakePicturesComplete(MultiAssetEventArgs pictures)
        {
            if (pictures == null)
                return;
            ImagesAttach.Clear();
            ImagesToAddCount = pictures.Assets.Length;
            CallbacksCount = 0;

            var imageManager = new PHImageManager();
            foreach (var asset in pictures.Assets)
            {
                imageManager.RequestImageData(asset,
                    new PHImageRequestOptions { DeliveryMode = PHImageRequestOptionsDeliveryMode.HighQualityFormat },
                    (NSData data, NSString dataUti, UIImageOrientation orientation, NSDictionary info) =>
                    {
                        double? latFinal = null;
                        double? lonFinal = null;
                        if (data != null)
                        {
                            GetLocationFromImageData(data, ref latFinal, ref lonFinal);
                        }

                        var picture = new CameraPicture { Image = UIImage.LoadFromData(data), Data = data, Lat = latFinal, Lon = lonFinal };
                        AddImagesIfRequired(picture);

                    });
            }

        }
        #endregion

        #region Refresh
        public void RefreshCollections()
        {
            CollectionView.ReloadData();
            CollectionViewAttach.ReloadData();
        }
        #endregion

        #region Methods
        bool DescriptionText_ShouldChangeText(UITextView textView, NSRange range, string text)
        {
            if (text.Length == 1 && text == Environment.NewLine)
            {
                textView.ResignFirstResponder();
                return false;
            }
            return true;
        }

        private void GetLocationFromImageData(NSData data, ref double? latFinal, ref double? lonFinal)
        {
            var imageProperties = ImageIO.CGImageSource.FromData(data)?.CopyProperties(new NSDictionary(), 0);
            if (imageProperties == null)
            {
                return;
            }

            var gps = imageProperties.ObjectForKey(ImageIO.CGImageProperties.GPSDictionary) as NSDictionary;
            var lat = gps?[ImageIO.CGImageProperties.GPSLatitude];
            var latref = gps?[ImageIO.CGImageProperties.GPSLatitudeRef];
            var lon = gps?[ImageIO.CGImageProperties.GPSLongitude];
            var lonref = gps?[ImageIO.CGImageProperties.GPSLongitudeRef];

            if (lat != null && latref != null && lon != null && lonref != null)
            {
                latFinal = double.Parse(lat.Description, CultureInfo.InvariantCulture.NumberFormat);
                lonFinal = double.Parse(lon.Description, CultureInfo.InvariantCulture.NumberFormat);
                if (latref.Description == "S")
                {
                    latFinal = -latFinal;
                }
                if (lonref.Description == "W")
                {
                    lonFinal = -lonFinal;
                }
            }
        }

        private void AlertImageSizeErrorIfFinished(bool isError = false)
        {
            if (CallbacksCount++ == 0)
                IsFileSizeError = false;

            if (isError)
                IsFileSizeError = isError;

            if (IsFileSizeError && CallbacksCount == ImagesToAddCount)
                new UIAlertView("Some images are not added", "Please upload an image smaller than 5MB", new UIAlertViewDelegate() as IUIAlertViewDelegate, "Ok").Show();
        }

        private void AddImagesIfRequired(CameraPicture picture)
        {
            if (AllImagesCount >= TargetCount)
            {
                AlertImageSizeErrorIfFinished();
                return;
            }
            if ((picture.Data != null) && (picture.Data.Length > 5 * 1024 * 1024))
            {
                AlertImageSizeErrorIfFinished(true);
                return;
            }
            PreviewImage.Image = picture.Image;
            ImagesAttach.Add(picture);
            ShowAddDescriptionView();
            AlertImageSizeErrorIfFinished();
        }

        async void UploadImages(List<CameraPicture> images)
        {
            foreach (var picture in images)
            {
                Stream imageStream;
                if (picture.Data != null)
                {
                    imageStream = picture.Data.AsStream();
                }
                else
                {
                    imageStream = picture.Image.AsJPEG().AsStream();
                }
                var imagePublicId = await Platform.CloudinarySDK.UploadGetId(imageStream);
                var description = String.IsNullOrEmpty(DescriptionText?.Text) || DescriptionText.Text == placeholder ? " " : DescriptionText.Text;
                await SL.Manager.PostSubmitContent(Challenge.ID, imagePublicId, picture.Description, picture.Lat ?? Platform.Lat, picture.Lon ?? Platform.Lon, SubmitContentResponse);
            }
        }

        public override void SubmitChallenge(UIButton button)//Attach Button
        {
            base.SubmitChallenge(button);

            UploadImages(ImagesAttach);
        }

        public void SubmitContentResponse(ChallengeResponseModel challengeResponse = null)
        {
            if (challengeResponse != null)
                Responses.Add(challengeResponse);

            Challenge.CompletedCount = CompletedCount + (challengeResponse.ResponseCode > 0 ? 1 : 0);

            if (Responses.Count < ImagesAttach.Count && challengeResponse.ResponseMessage != "You completed that challenge!")
            {
                return;
            }
            /*
            var errorResponces = Responses.FindAll((x) => x.ResponseCode <= 0);
            if (errorResponces?.Count > 0)
            {
                string message = String.Empty;
                int count = 0;
                foreach (var item in errorResponces)
                {
                    message += $"{(++count).ToString()}) {item.ResponseMessage} \n\n";
                }
                new UIAlertView("Some images are not added", message, new UIAlertViewDelegate() as IUIAlertViewDelegate, "Ok").Show();
            }
            */
            if (PosteringOverlay != null)
            {
                PosteringOverlay.RemoveFromSuperview();
                PosteringOverlay = null;
                Platform.AnimateRotationComplete(Spinner);
                Spinner.RemoveFromSuperview();
                Spinner = null;
            }

            if (CompletedCount >= TargetCount)//max responce
            {
                SubmitChallengeComplete(btnSubmitChallenge, challengeResponse);
            }
            else
            {
                SubmitChallengeComplete(btnSubmitChallenge, null);//remove overlay && animation
            }
            Responses.Clear();
        }

        private void ShowNoImageView()
        {
            vCameraCenter.Hidden = false;

            CollectionView.Hidden = true;
            vDescriptionContainer.Hidden = true;
            submitContainer.Hidden = true;
            bottomButtonsContainer.Hidden = true;
        }

        private void ShowAddDescriptionView()
        {
            vDescriptionContainer.Hidden = false;
            submitContainer.Hidden = false;

            vCameraCenter.Hidden = true;
            CollectionView.Hidden = true;
            bottomButtonsContainer.Hidden = true;

            DescriptionText.Text = placeholder;
            DescriptionText.TextColor = UIColor.LightGray;
            UploadedCountText.Text = AllImagesCount.ToString();

            CollectionViewAttach.Hidden = ImagesAttach.Count <= 1;
            PreviewImage.Hidden = ImagesAttach.Count > 1;

            if (AllImagesCount < TargetCount)
            {
                CameraButton.Alpha = 1;
                CameraButton.UserInteractionEnabled = true;
            }
            else
            {
                CameraButton.Alpha = 0.25f;
                CameraButton.UserInteractionEnabled = false;
            }

            _collectionSource.Pictures = ImagesAttach;

            RefreshCollections();
        }

        private void ShowImagesCollectionView()
        {
            if (AllowUserCompletion)
            {
                btnSubmitChallenge.UserInteractionEnabled = true;
                btnSubmitChallenge.SetBackgroundImage(UIImage.FromBundle("challenge-btn_submit"), UIControlState.Normal);//btnSubmitChallenge.Alpha = 1;
            }
            else
            {
                btnSubmitChallenge.UserInteractionEnabled = false;
                btnSubmitChallenge.SetBackgroundImage(new UIImage(), UIControlState.Normal);//btnSubmitChallenge.Alpha = 0.25f;
            }

            CollectionView.Hidden = false;
            bottomButtonsContainer.Hidden = false;

            vCameraCenter.Hidden = true;
            vDescriptionContainer.Hidden = true;
            submitContainer.Hidden = true;

            CameraButton.SetBackgroundImage(UIImage.FromBundle("challenge-btn_photo"), UIControlState.Normal);

            UploadedCountText.Text = AllImagesCount.ToString();

            if (_collectionSource != null)
            {
                ImagesAttach.ForEach(x => x.Description = DescriptionText.Text);
                Images.AddRange(ImagesAttach);
                _collectionSource.Pictures = Images;
                RefreshCollections();
            }
        }

        private void SetupFonts()
        {
            lblChallengeName.Font = UIFont.FromName("SFProText-Bold", SizeConstants.ScreenMultiplier * 24);
            DescriptionText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 16);
            lblUpload.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 16);
            NeedUploadCountText.Font = UIFont.FromName("SFProText-Bold", SizeConstants.ScreenMultiplier * 18);
            UploadedCountText.Font = UIFont.FromName("SFProText-Bold", SizeConstants.ScreenMultiplier * 18);
        }
        #endregion
    }
}