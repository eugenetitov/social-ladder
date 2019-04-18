using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Camera;
using SocialLadder.iOS.Constraints;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class CollateralViewController : CameraViewController
    {
        private CameraPicture ImageToUpload { get; set; }
        private string placeholder = "Description goes here...";
        private NSLayoutConstraint _constraintVisible;
        private bool isVisibleCollectionFullView
        {
            get
            {
                return _constraintVisible.Active;
            }
            set {
                CollectionFullView.Hidden = !value;
                UploadCountText.Hidden = !value;
                _constraintVisible.Active = value;
            }
        }


        public List<CameraPicture> Images { get; set; }

        public CollateralViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            AddPosterButton.Hidden = true;

            var constraints = CollectionFullView.Constraints;
            foreach (var item in constraints)
            {
                if (item.FirstAttribute == NSLayoutAttribute.Width && item.SecondAttribute == NSLayoutAttribute.Height)
                {
                    _constraintVisible = item;
                    isVisibleCollectionFullView = false;
                }
            }
           // isVisibleCollectionFullView = true;

            DescriptionText.ReturnKeyType = UIReturnKeyType.Done;
            DescriptionText.ShouldChangeText += DescriptionText_ShouldChangeText;
            StepOneImage.Layer.BorderWidth = 1;
            StepOneImage.Layer.BorderColor = UIColor.FromRGB(229, 229, 229).CGColor;
            StepTwoImage.Layer.BorderWidth = 1;
            StepTwoImage.Layer.BorderColor = UIColor.FromRGB(229, 229, 229).CGColor;
            AddPosterButton.Layer.BorderWidth = 1;
            AddPosterButton.Layer.BorderColor = UIColor.FromRGB(229, 229, 229).CGColor;

            DescriptionText.Text = placeholder;
            DescriptionText.TextColor = UIColor.LightGray;
            // MainHeaderLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.065f);
            Images = new List<CameraPicture>();

            PreviewImage.Image = UIImage.FromBundle("add-image-border");

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
            DeleteButton.Hidden = true;
            //AddPosterButton.Hidden = true;
            UpdateButton(true);

            CollectionView.RegisterNibForCell(CollateralCollectionViewCell.Nib, CollateralCollectionViewCell.ClassName);
        }

       

        bool DescriptionText_ShouldChangeText(UITextView textView, NSRange range, string text)
        {
            if (text.Length == 1 && text == Environment.NewLine)
            {
                if (DidAllStepsComplete())
                {
                    //AddPosterButton.Hidden = false;
                    UpdateButton(false);
                }
                if (DidStepComplete(2))
                    MarkStep(2, true);

                textView.ResignFirstResponder();
                return false;
            }
            return true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            float inset = 30.0f;
            float inset2 = 29.5f;
            CollectionView.Source = new CollateralCollectionSource(Images);
            CollectionView.CollectionViewLayout = new UICollectionViewFlowLayout
            {
                ItemSize = new CGSize(70, 70),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                SectionInset = new UIEdgeInsets(inset2, inset2, inset2, inset2),
                MinimumInteritemSpacing = inset,
                MinimumLineSpacing = inset
            };
            Refresh();
        }

        private bool TextFieldShouldReturn(UITextField textfield)
        {
            if (DidAllStepsComplete())
            {
                //AddPosterButton.Hidden = false;
                UpdateButton(false);
            }
            if (DidStepComplete(2))
                MarkStep(2, true);

            textfield.ResignFirstResponder();   //close keyboard
            return false; // We do not want UITextField to insert line-breaks.
        }

        partial void CameraButton_TouchUpInside(UIButton sender)
        {
            if (DidAllStepsComplete())
            {
                SubmitChallenge(CameraButton);
                return;
            }
            TakePicture();

        }
        
        public override void OnTakePictureComplete(CameraPicture picture)
        {
            ImageToUpload = picture;
            PreviewImage.Image = picture.Image;
            DeleteButton.Hidden = false;
            MarkStep(1, true);
            if (DidAllStepsComplete())
            {
                //AddPosterButton.Hidden = false;
                UpdateButton(false);
            }
        }

        async void Upload(CameraPicture picture)
        {
            var imageUrl = await Platform.CloudinarySDK.Upload(picture.Image.AsPNG().AsStream());
            await SL.Manager.PostSubmitContent(Challenge.ID, imageUrl, DescriptionText.Text, Platform.Lat, Platform.Lon, SubmitContentResponse);
            ResetUI();
        }

        public override void SubmitChallenge(UIButton button)
        {
            base.SubmitChallenge(button);
            Upload(ImageToUpload);
        }

        partial void AddPosterButton_TouchUpInside(UIButton sender)
        {
            //if (DidAllStepsComplete())
            //{
            //    SubmitChallenge(CameraButton);
            //}
        }

        void ResetUI()
        {
            Images.Add(ImageToUpload);

            ImageToUpload = null;
            DescriptionText.Text = null;
            PreviewImage.Image = UIImage.FromBundle("add-image-border");
            MarkStep(1, false);
            MarkStep(2, false);
            DeleteButton.Hidden = true;
            UpdateButton(true);
            //AddPosterButton.Hidden = true;



            if (Images.Count!= 0 )
            {
                isVisibleCollectionFullView = true;
            }
            else
            {
                isVisibleCollectionFullView = false;
            }
            
            Refresh();
        }

        public void Refresh()
        {
            //UploadCountText.Text = Images.Count > 0 ? Images.Count + " Image" + (Images.Count > 1 ? "s" : "") + " Uploaded" : "";
            CollectionView.ReloadData();
        }

        public void SubmitContentResponse(ChallengeResponseModel challengeResponse)
        {
            //ChallengeComplete(challengeResponse);
            SubmitChallengeComplete(CameraButton, challengeResponse);
        }
        /*
        void ChallengeComplete(ChallengeResponseModel challengeResponse)
        {
            
            UIView overlay = Platform.AddOverlay(UIColor.FromRGBA(36, 209, 180, 153));  //from spec
            if (overlay != null)
            {
                ChallengeCompleteView challengeComplete = ChallengeCompleteView.Create();
                overlay.AddSubview(challengeComplete);
                challengeComplete.Update(overlay, challengeResponse, Challenge);
            }

        }
        */
        bool DidStepComplete(int step)
        {
            bool didCompete;
            switch (step)
            {
                case 1:
                    didCompete = ImageToUpload != null;
                    break;
                case 2:
                    didCompete = DescriptionText.Text.Length > 0;
                    break;
                default:
                    didCompete = false;
                    break;
            }
            return didCompete;
        }

        bool DidAllStepsComplete()
        {
            return DidStepComplete(1) && DidStepComplete(2);
        }

        void MarkStep(int step, bool isComplete)
        {
            UIImageView stepImage = null;
            UILabel stepText = null;

            switch (step)
            {
                case 1:
                    stepImage = StepOneImage;
                    stepText = StepOneText;
                    break;
                case 2:
                    stepImage = StepTwoImage;
                    stepText = StepTwoText;
                    break;
            }

            if (stepImage != null && stepText != null)
            {
                if (isComplete)
                {
                    stepImage.Image = UIImage.FromBundle("check-icon_green");
                    stepText.Hidden = true;
                }
                else
                {
                    stepImage.Image = null;
                    stepText.Hidden = false;

                    //AddPosterButton.Hidden = true;
                    UpdateButton(true);
                }
            }
        }

        public void Setup()
        {
            //UploadCountText.Text = Images.Count > 0 ? Images.Count + " Image" + (Images.Count > 1 ? "s" : "") + " Uploaded" : "";
            CollectionView.ReloadData();
        }

        partial void DeleteButton_TouchUpInside(UIButton sender)
        {
            ImageToUpload = null;
            PreviewImage.Image = UIImage.FromBundle("add-image-border");
            DeleteButton.Hidden = true;
            MarkStep(1, false);
        }

        private void UpdateButton(bool isPost)
        {
            if(!isPost)
            {
                CameraButton.SetBackgroundImage(UIImage.FromBundle("challenge-btn_submit"), UIControlState.Normal);
                return;
            }
            CameraButton.SetBackgroundImage(UIImage.FromBundle("challenge-btn_photo"), UIControlState.Normal);
        }
    }
}