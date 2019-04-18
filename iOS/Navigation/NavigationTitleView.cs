using Foundation;
using System;
using UIKit;
using SocialLadder.Models;
using FFImageLoading;
using SocialLadder.iOS.CustomControlls;
using CoreGraphics;

namespace SocialLadder.iOS.Navigation
{
    public partial class NavigationTitleView : UIView
    {
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;
        private nfloat _actionBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height;

        public NSLayoutConstraint CnsVActionBarAlignmentBasisHeight => cnsVActionBarAlignmentBasisHeight;

        public UIButton BtnBackOutlet => btnBack;
        public UIButton btnDoneOutlet => btnDone;

        private bool _isRootMode;

        public UIButton NotificationButtonOutlet
		{
			get
			{
				return NotificationButton;
			}
		}

		public static NavigationTitleView Create()
        {
			var arr = NSBundle.MainBundle.LoadNib("NavigationTitleView", null, null);
			NavigationTitleView view = arr.Count > 0 ? arr.GetItem<NavigationTitleView>(0) : null;
            view.RootMode();
            return view;
        }

        public NavigationTitleView (IntPtr handle) : base (handle)
        {

        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            if (_isRootMode)
            {
                NotificationButton.Hidden = false;
                NotificationButtonImage.Hidden = false;
            }
            NotificationButtonImage.Image = UIImage.FromBundle("notification-icon_white");
            NotificationButton.UserInteractionEnabled = true;
            BringSubviewToFront(NotificationButton);

            btnBack.SetTitleColor(UIColor.White, UIControlState.Normal);
            btnDone.SetTitleColor(UIColor.White, UIControlState.Normal);
            btnBack.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            btnBack.SetImage(UIImage.FromBundle("bold-back-arrow"), UIControlState.Normal);

            Score.Font = UIFont.FromName("ProximaNova-Bold", _screenWidth / 414f * 9f);//ExtraBold
            Title.Font = UIFont.FromName("ProximaNova-Bold", _screenWidth / 414f * 12f);
            btnBack.TitleLabel.Font = UIFont.FromName("ProximaNova-Bold", _screenWidth / 414f * 20f);
            btnDone.TitleLabel.Font = UIFont.FromName("ProximaNova-Bold", _screenWidth / 414f * 20f);

            if (cnsVActionBarAlignmentBasisHeight.Constant != 0.5f * (Frame.Height - _actionBarHeight))
            {
                cnsVActionBarAlignmentBasisHeight.Constant = 0.5f * (Frame.Height - _actionBarHeight);
                LayoutIfNeeded();
            }

        }

        public void SetTitle(string title)
		{
			Title.Text = title;
		}

        public void Update()
		{
            AreaModel area = SL.Area;
            if (area != null)
			{
				Title.Text = area.areaName;
                ImageService.Instance.LoadUrl(area.areaDefaultImageURL).Into(Image);
                BackgroundColor = area.areaPrimaryColor.ToUIColor();
                //float radius = (float)ScoreImage.Frame.Height / 2;
                //ScoreImage.Layer.CornerRadius = radius;
                if (SL.HasProfile)
                    Score.Text = SL.Profile.Score.ToString();
     

			}
		}

        public void ShowLoadIndicator()
        {
            ScoreImage.Image = UIImage.FromBundle("loading-indicator");
            Score.Hidden = true;
            Platform.AnimateRotation(ScoreImage);
        }

        public void CloseLoadIndicator()
        {
            ScoreImage.Image = UIImage.FromBundle("small-score-icon");
            Platform.AnimateRotationComplete(ScoreImage);
            if (_isRootMode == false)
            {
                return;
            }
             Score.Hidden = false;
        }

        public void RootMode()
        {
            btnBack.Hidden = true;
            btnDone.Hidden = true;
            Image.Hidden = false;
            ScoreImage.Hidden = false;
            HandleImage.Hidden = false;
            NotificationButton.Hidden = false;
            NotificationButtonImage.Hidden = false;

            Title.Hidden = false;
            Score.Hidden = false;

            _isRootMode = true;
        }

        public void BackMode()
        {
            btnBack.Hidden = false;
            btnDone.Hidden = true;
            Image.Hidden = true;
            ScoreImage.Hidden = true;
            HandleImage.Hidden = true;
            NotificationButton.Hidden = true;
            NotificationButtonImage.Hidden = true;

            Score.Hidden = true;
            Title.Hidden = true;

            _isRootMode = false;           
        }
    }
}