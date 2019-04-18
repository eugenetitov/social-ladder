using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using CrashlyticsKit;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Services;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class ChallengeCollectionViewCell : UICollectionViewCell
    {
        private ChallengeTypeModel _challengeTypeModel;

        public static readonly string ClassName = "ChallengeCollectionViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;
        public readonly nfloat CellMultiplayer = 1.23f;

        bool DidSelect { get; set; }

        static bool DidGetConstraintsLayout { get; set; }

        static nfloat CellWidthConstant { get; set; }

        static ChallengeCollectionViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        public static void GetConstraintsLayout(ChallengeCollectionViewCell cell)
        {
            //CellWidthConstant = cell.CellWidth.Constant;
            DidGetConstraintsLayout = true;
        }

        protected ChallengeCollectionViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            //ApplyStyle();
        }

        public void ApplyStyle()
        {
            Layer.CornerRadius = 2.0f;
            //Layer.BorderWidth = 1.0f;
            //Layer.MasksToBounds = true;
            //ContentView.Layer.BorderColor = UIColor.Clear.CGColor;
            if (!ChallengeCollectionViewCell.DidGetConstraintsLayout)
                ChallengeCollectionViewCell.GetConstraintsLayout(this);
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
        }

        public void UpdateCellData(ChallengeTypeModel challengeType, bool isSelected, nfloat containerHeight)
        {
            Crashlytics.Instance.Log("ChallengeCollectionViewCell_UpdateCellData()");
            try
            {
                if (challengeType == null)
                {
                    return;
                }

                _challengeTypeModel = challengeType;

                ChallengeImage.TintColor = UIColor.Clear;
                int color = Convert.ToInt32(ChallengeModel.GetTypeCodeColor(challengeType.TypeCode, challengeType.DisplayName), 16);
                int r = (color & 0xff0000) >> 16;
                int g = (color & 0xff00) >> 8;
                int b = (color & 0xff);
                Background.BackgroundColor = UIColor.FromRGB(r, g, b);
                ChallengeName.Text = challengeType.DisplayName;//ChallengeModel.GetTypeCodeDisplayName(challengeType.TypeCode);

                LoadImages(challengeType);

                //else
                //    ChallengeImage.Image = UIImage.FromBundle("insta-icon_white");
                ChallengeProgressBar.Progress = challengeType.Total > 0 ? (float)challengeType.TotalComplete / (float)challengeType.Total : 0;
                ChallengeProgressText.Text = challengeType.TotalComplete + " of " + challengeType.Total;

                DidSelect = isSelected;
                UpdateItem(1.0f);
                if (isSelected)
                {
                    UpdateItem(CellMultiplayer);
                }
            }
            catch (Exception e)
            {
                Crashlytics.Instance.Log($"ChallengeCollectionViewCell_UpdateCellData() {e.Message}");
                throw;
            }
        }


        void LoadImages(ChallengeTypeModel challengeType)
        {
            ChallengeImage.Image = null;

            if (challengeType.TypeCode == "CHECKIN")
            {
                ChallengeImage.Image = UIImage.FromBundle("location-icon_large");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "COLLATERAL TRACKING")
            {
                ChallengeImage.Image = UIImage.FromBundle("location-icon_large");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "POSTERING")
            {
                ChallengeImage.Image = UIImage.FromBundle("location-icon_large");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "FB ENGAGEMENT")
            {
                ChallengeImage.Image = UIImage.FromBundle("fb-logo");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "INSTA")
            {
                ChallengeImage.Image = UIImage.FromBundle("insta-icon_white");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "INVITE")
            {
                if (challengeType.DisplayName == "Invite to Buy")
                {
                    ChallengeImage.Image = UIImage.FromBundle("ticket-icon_white");
                    ImageIsNull(ChallengeImage.Image, challengeType);
                }
                if (challengeType.DisplayName == "Invite to Join")
                {
                    ChallengeImage.Image = UIImage.FromBundle("invite-icon_white");
                    ImageIsNull(ChallengeImage.Image, challengeType);
                }
            }
            if (challengeType.TypeCode == "SHARE")
            {
                ChallengeImage.Image = UIImage.FromBundle("bullhorn_icon_white");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "POSTERING")
            {
                ChallengeImage.Image = UIImage.FromBundle("postering-icon_large");
                ImageIsNull(ChallengeImage.Image, challengeType);
                ChallengeImage.TintColor = UIColor.White;
            }
            if (challengeType.TypeCode == "SUBMIT")
            {
                ChallengeImage.Image = UIImage.FromBundle("");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "VERIFY")
            {
                ChallengeImage.Image = UIImage.FromBundle("");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "FLYERING")
            {
                ChallengeImage.Image = UIImage.FromBundle("flyering-icon_large");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "MANUAL")
            {
                ChallengeImage.Image = UIImage.FromBundle("manual-icon_large");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "MC")
            {
                ChallengeImage.Image = UIImage.FromBundle("quiz-icon_white");//.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                ImageIsNull(ChallengeImage.Image, challengeType);
                ChallengeImage.TintColor = UIColor.White;
            }
            if (challengeType.TypeCode == "SIGNUP")
            {
                ChallengeImage.Image = UIImage.FromBundle("signup-icon_large");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
        }

        void ImageIsNull(UIImage image, ChallengeTypeModel challengeType)
        {
            if (image == null)
            {
                FileCachingService.BeginDownloadImageFromUrl(challengeType.ImageUrl, DownloadImageFromUrlCompleted);
            }
        }

        public void DownloadImageFromUrlCompleted(FileLoadedArgs args)
        {
            if (!String.Equals(args.ImageUrl, _challengeTypeModel.ImageUrl))
            {
                return;
            }
            ;
            InvokeOnMainThread(() =>
            {
                ChallengeImage.Image = args.Image;
            });
        }

        public void OnImageLoaded(object sender, FileLoadedArgs args)
        {
            if (!String.Equals(args.ImageUrl, _challengeTypeModel.ImageUrl))
            {
                return;
            }
            ;
            InvokeOnMainThread(() =>
            {
                ChallengeImage.Image = args.Image;
            });
        }

        public void UpdateItem(nfloat constraintSize)
        {
            Background.TranslatesAutoresizingMaskIntoConstraints = false;
            List<NSLayoutConstraint> constants = new List<NSLayoutConstraint>();
            foreach (var constraint in ContentView.Constraints)
            {
                if (constraint.FirstAttribute == NSLayoutAttribute.Width || constraint.FirstAttribute == NSLayoutAttribute.Height || constraint.FirstAttribute == NSLayoutAttribute.CenterX || constraint.FirstAttribute == NSLayoutAttribute.CenterY)
                {
                    constants.Add(constraint);
                }
            }
            ContentView.RemoveConstraints(constants.ToArray());
            ContentView.AddConstraint(ChallengesConstraints.ChallengesCollectionCellCenterXConstraint(Background, ContentView));
            ContentView.AddConstraint(ChallengesConstraints.ChallengesCollectionCellCenterYConstraint(Background, ContentView));
            ContentView.AddConstraint(ChallengesConstraints.ChallengesCollectionCellWidthConstraint(Background, ContentView, constraintSize));
            ContentView.AddConstraint(ChallengesConstraints.ChallengesCollectionCellHeightConstraint(Background, ContentView, constraintSize));
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            Background.Layer.CornerRadius = SizeConstants.Screen.Width * 0.01f;
            BadgeView.Layer.CornerRadius = BadgeView.Frame.Width * 0.5f;
            ChallengeProgressBar.Layer.CornerRadius = ChallengeProgressBar.Frame.Height * 0.5f;
            ChallengeName.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.Screen.Width * 0.035f);
            ChallengeProgressText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.035f);

            /*
            if (DidSelect)
            {
                CellWidth.Constant = 200;
                CellHeight.Constant = 200;
            }
            else
            {
                CellWidth.Constant = 160;
                CellHeight.Constant = 160;
            }
            SetNeedsLayout();
            */
        }
    }
}
