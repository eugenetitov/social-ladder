using System;
using System.Drawing;

using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.CurrentConstants;
using UIKit;

namespace SocialLadder.iOS.Points.CustomViews
{
    public partial class PointsTabButton : UIButton
    {
        public UIView BottomLine { get; set; }
        public UILabel TopText { get; set; }
        private nfloat FontSize = UIScreen.MainScreen.Bounds.Width / 414 * 14f;

        public PointsTabButton(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public PointsTabButton()
        {
            Initialize();
        }

        public void Initialize()
        {
            this.SetTitle(null, UIControlState.Normal);
            TopText = new UILabel { TextColor = Colors.PointsTabButtonColor, TextAlignment = UITextAlignment.Center, Font = UIFont.FromName("ProximaNova-Regular", FontSize) };
            BottomLine = new UIView { BackgroundColor = Colors.PointsTabButtonColor };
            this.AddSubviews(TopText, BottomLine);

            TopText.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonTopTextCenterX(TopText, this));
            this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonTopTextTop(TopText, this));
            this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonTopTextHeight(TopText, this));
            //this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonTopTextWidth(TopText, this));

            BottomLine.TranslatesAutoresizingMaskIntoConstraints = false;
            this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonBottomLineCenterX(BottomLine, this));
            this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonBottomLineBottom(BottomLine, this));
            this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonBottomLineHeight(BottomLine, this));
            this.AddConstraint(PointsTabButtonConstraint.PointsTabButtonBottomLineWidth(BottomLine, TopText));
        }

        public void SetTitle(string title)
        {
            TopText.Text = title;
        }

        public void SetSelected()
        {
            TopText.AttributedText = new NSAttributedString(
                TopText.Text,
                Font = UIFont.FromName("ProximaNova-Bold", FontSize),
                foregroundColor: Colors.PointsTabButtonSelectedColor,
                backgroundColor: UIColor.Clear
            );
            BottomLine.BackgroundColor = Colors.PointsTabButtonSelectedColor;
        }

        public void SetUnselected()
        {
            TopText.AttributedText = new NSAttributedString(
                TopText.Text,
                Font = UIFont.FromName("ProximaNova-Regular", FontSize),
                foregroundColor: Colors.PointsTabButtonColor,
                backgroundColor: UIColor.Clear
            );
            BottomLine.BackgroundColor = UIColor.Clear;
        }
    }
}