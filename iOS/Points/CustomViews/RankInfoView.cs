using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Constraints;
using System;
using UIKit;

namespace SocialLadder.iOS.Points.CustomViews
{
    public partial class RankInfoView : UIView
    {
        private nfloat _screenSize = UIScreen.MainScreen.Bounds.Width;

        public UIButton CloseButton => btnClose;

        public static RankInfoView Create()
        {
            NSArray arr = NSBundle.MainBundle.LoadNib("RankInfoView", null, null);
            RankInfoView view = arr.GetItem<RankInfoView>(0);

            return view;
        }

        public RankInfoView (IntPtr handle) : base (handle)
        {
        }

        public void Initialize()
        {
            TranslatesAutoresizingMaskIntoConstraints = false;
            vScoreInfo.SetupViewStyle();
            vScoreInfo.SetupViewComponents();

            btnClose.SetImage(UIImage.FromBundle("close-icon"), UIControlState.Normal);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var marin = UIScreen.MainScreen.Bounds.Width * 0.038f;
            CloseButton.ContentEdgeInsets = new UIEdgeInsets(marin, marin, marin, marin);

            CGRect frame = Superview.Frame;
            frame = Frame;
            frame = UIApplication.SharedApplication.KeyWindow.Frame;

            lblRankInfoTitle.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 24);

            NSMutableParagraphStyle paragraph = new NSMutableParagraphStyle();
            paragraph.MinimumLineHeight = SizeConstants.ScreenMultiplier * 24;
            paragraph.MaximumLineHeight = SizeConstants.ScreenMultiplier * 24;
            NSAttributedString attributedString = new NSAttributedString(txtvRankInfoDescription.Text, UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 16), paragraphStyle: paragraph);

            txtvRankInfoDescription.TextContainerInset = new UIEdgeInsets(0f, 0, 0, 0);
            txtvRankInfoDescription.AttributedText = attributedString;

            vScoreInfo.LayoutSubviews();
        }
    }
}