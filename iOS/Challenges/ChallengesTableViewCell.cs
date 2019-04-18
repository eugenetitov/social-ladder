using System;
using Foundation;
using UIKit;
using SocialLadder.Models;
using FFImageLoading;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Services;
using CrashlyticsKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class ChallengesTableViewCell : UITableViewCell
    {
        public static readonly string ClassName = "ChallengesTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;
        private ChallengeTypeModel _challengeTypeModel;

        static ChallengesTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        void ApplyStyles()
        {
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        protected ChallengesTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            ApplyStyles();
        }

        public override void PrepareForReuse()
        {
            ChallengeName.Text = string.Empty;
            ChallengeTime.Text = string.Empty;
            PointsText.Text = string.Empty;
            ChallengeImage.Image = null;
            base.PrepareForReuse();
        }

        public void UpdateCellData(ChallengeModel item)
        {
            Crashlytics.Instance.Log("Challenges_ChallengesTableCell_UpdateCellData()");
            try
            {
                SetupFonts();

                _challengeTypeModel = new ChallengeTypeModel
                {
                    TypeCode = (item as ChallengeModel).TypeCode,
                    DisplayName = (item as ChallengeModel).TypeCodeDisplayName,
                    ImageUrl = (item as ChallengeModel).IconImageURL
                };

                int color = Convert.ToInt32(ChallengeModel.GetTypeCodeColor(item.TypeCode, item.TypeCodeDisplayName), 16);
                int r = (color & 0xff0000) >> 16;
                int g = (color & 0xff00) >> 8;
                int b = (color & 0xff);
                LeftSideView.BackgroundColor = UIColor.FromRGB(r, g, b);
                SeperatorView.BackgroundColor = UIColor.FromRGB(r, g, b);
                ChallengeName.Text = item.Name;

                LoadImages(_challengeTypeModel);
                ChallengeImage.Image = ChallengeImage.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
                //ImgMapService.SetImage(item.IconImageURL, ChallengeImage);

                PointsText.Text = "+" + item.PointValue.ToString() + " pts";

                ChallengeTime.Text = item.NextEventCountDown;

                ChallengeImage.TintColor = UIColor.LightGray;
                if (item.LockStatus)
                {
                    ClosedImage.Hidden = false;
                    ChellangeRightImage.Hidden = PointsText.Hidden = true;
                    ClosedImage.Image = UIImage.FromBundle("lock_icon");
                }
                else
                {
                    var icon = UIImage.FromBundle("points-icon_white");
                    ClosedImage.Hidden = PointsText.Hidden = false;
                    ClosedImage.Image = icon;
                }
            }
            catch (Exception e)
            {
                Crashlytics.Instance.Log($"Challenges_ChallengesTableCell_UpdateCellData() - {e.Message}");
            }

        }

        void LoadImages(ChallengeTypeModel challengeType)
        {
            ChallengeImage.Image = null;

            if (challengeType.TypeCode == "CHECKIN")
            {
                ChallengeImage.Image = UIImage.FromBundle("location-icon_white");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "COLLATERAL TRACKING")
            {
                ChallengeImage.Image = UIImage.FromBundle("location-icon_white");
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "POSTERING")
            {
                ChallengeImage.Image = UIImage.FromBundle("location-icon_white");
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
                ChallengeImage.Image = UIImage.FromBundle("postering-icon_large");//it`s like white
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
                ChallengeImage.Image = UIImage.FromBundle("flyering-icon_large");//it`s like white
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "MANUAL")
            {
                ChallengeImage.Image = UIImage.FromBundle("manual-icon_large");//it`s like white
                ImageIsNull(ChallengeImage.Image, challengeType);
            }
            if (challengeType.TypeCode == "MC")
            {
                ChallengeImage.Image = UIImage.FromBundle("quiz-icon_white");
                ImageIsNull(ChallengeImage.Image, challengeType);
                ChallengeImage.TintColor = UIColor.White;
            }
            if (challengeType.TypeCode == "SIGNUP")
            {
                ChallengeImage.Image = UIImage.FromBundle("signup-icon_large");//it`s like white
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
            if (_challengeTypeModel?.ImageUrl != null && !String.Equals(args.ImageUrl, _challengeTypeModel.ImageUrl))
            {
                return;
            }
            ;
            InvokeOnMainThread(() =>
            {
                ChallengeImage.Image = args.Image;
            });
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetupFonts();
        }

        private void SetupFonts()
        {
            ChallengeName.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.Screen.Width * 0.04f);
            ChallengeTime.Font = PointsText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.034f);
        }
    }
}
