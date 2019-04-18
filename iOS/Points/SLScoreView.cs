using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Constraints;
using UIKit;

namespace SocialLadder.iOS.Points
{

    public partial class SLScoreView : PointsBaseView
    {
        public float ScoreFill { get; set; }

        private float _scoreTriangleTangent = 122f / 414f;
        public float CornerRadius { get; set; }

        public UIColor LabelTextColor { get; set; }
        public CGColor ScoreFillColor { get; set; }
        public UIColor ScoreBackgroundColor { get; set; }
        public CGColor LinesFillColor { get; set; }
        public CGColor LinesBackgroundColor { get; set; }

        public List<string> LabelTitles { get; set; }
        public List<nfloat> ScoreValueProportions { get; set; }

        public List<UILabel> Labels { get; set; }
        CGPath ScoreBackgroundPath { get; set; }
        CGPath ScoreFillPath { get; set; }
        CGPath LinesBackgroundPath { get; set; }
        CGPath LinesFillPath { get; set; }

        public SLScoreView(CGRect frame) : base(frame)
        {
        }
        public SLScoreView(IntPtr handle) : base(handle)
        {
        }

        public void SetupViewStyle()
        {
            var profile = SL.Profile;
            ScoreFill = profile != null ? (float)profile.OverallSLScore / 100.0f : 0.0f;
            CornerRadius = 5.0f;
            LabelTitles = new List<string>();
            ScoreValueProportions = new List<nfloat>();

            BackgroundColor = UIColor.Clear;
            Layer.ZPosition = 1;

            ScoreBackgroundColor = UIColor.FromRGBA(229.0f / 255.0f, 229.0f / 255.0f, 229.0f / 255.0f, 1.0f);
            ScoreFillColor = UIColor.FromRGBA(51.0f / 255.0f, 171.0f / 255.0f, 151.0f / 255.0f, 1.0f).CGColor;
            LinesBackgroundColor = UIColor.White.CGColor;
            LinesFillColor = new CGColor(0, 0, 0, 0.2f);

            byte[] textColor = CodeBehindUIConstants.PointsViewGrayTextColor;
            LabelTextColor = UIColor.FromRGBA(textColor[0], textColor[1], textColor[2], textColor[3]);

            LabelTitles.Add("Fan");
            LabelTitles.Add("Superfan");
            LabelTitles.Add("Ambassador");
            LabelTitles.Add("Champion");

            ScoreValueProportions.Add(28f / 347f);
            ScoreValueProportions.Add(122f / 347f);
            ScoreValueProportions.Add(244f / 347f);
            ScoreValueProportions.Add(347f / 347f);

        }

        public void SetupViewComponents()
        {
            try
            {
              if (LabelTitles == null || LabelTitles.Count == 0)
                {
                    return;
                }
                Labels = new List<UILabel>();
                foreach (string item in LabelTitles)
                {
                    UILabel label = new UILabel();
                    label.Text = item;   //shift i to access safely
                    label.TextColor = LabelTextColor;
                    Labels.Add(label);
                    AddSubview(label);
                }
            }
            catch (Exception)
            {
            }
        }

        public override void LayoutSubviews()
        {
            try
            {
                base.LayoutSubviews();

                var cornerXShift = (float)(CornerRadius / Math.Tan(0.5f * Math.Atan(_scoreTriangleTangent)) - CornerRadius);

                if (ScoreBackgroundPath != null && ScoreBackgroundPath.BoundingBox.X == -cornerXShift && ScoreBackgroundPath.BoundingBox.Width == Frame.Width + cornerXShift)
                {
                    return;
                }

                ScoreBackgroundPath = Platform.CreateTriangleBezierPath(-cornerXShift, Frame.Height - _scoreTriangleTangent * (Frame.Width + cornerXShift),
                    Frame.Width + cornerXShift, _scoreTriangleTangent * (Frame.Width + cornerXShift), CornerRadius);
                
                ScoreFillPath = Platform.CreateTriangleBezierPath(-cornerXShift, Frame.Height - _scoreTriangleTangent * (ScoreFill * Frame.Width + cornerXShift),
                    ScoreFill * Frame.Width + cornerXShift, _scoreTriangleTangent * (ScoreFill * Frame.Width + cornerXShift), CornerRadius);

                LinesBackgroundPath = new CGPath();
                LinesFillPath = new CGPath();

                int i = 0;
                nfloat ratingUnit = Frame.Width / LabelTitles.Count;
                foreach (UILabel item in Labels)
                {
                    nfloat pointX = ScoreValueProportions[i] * Frame.Width;
                    nfloat pointY = - _scoreTriangleTangent * (pointX + cornerXShift);

                    if (i < LabelTitles.Count - 1)
                    {
                        if (pointX / Frame.Width < ScoreFill)
                        {
                            LinesFillPath.MoveToPoint(pointX, Frame.Height);
                            LinesFillPath.AddLineToPoint(pointX, Frame.Height + pointY);
                        }
                        else
                        {
                            LinesBackgroundPath.MoveToPoint(pointX, Frame.Height);
                            LinesBackgroundPath.AddLineToPoint(pointX, Frame.Height + pointY);
                        }
                    }

                    item.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 12);//16f LabelFontSize
                    item.SizeToFit();

                    CGRect frame = item.Frame;
                    frame.X = pointX - frame.Width;
                    frame.Y = Frame.Height + pointY - frame.Height;
                    item.Frame = frame;

                    ++i;
                }              
            }
            catch (Exception)
            {
            }
        }

        public override void Draw(CGRect rect)
        {
            try
            {
                base.Draw(rect);

                var ctx = UIGraphics.GetCurrentContext();
                ctx.SaveState();

                ctx.BeginPath();
                ctx.AddPath(ScoreBackgroundPath);
                ctx.ClosePath();
                ctx.SetFillColor(ScoreBackgroundColor.CGColor);
                ctx.FillPath();
                ctx.DrawPath(CGPathDrawingMode.Fill);

                ctx.BeginPath();
                ctx.AddPath(ScoreFillPath);
                ctx.ClosePath();
                ctx.SetFillColor(ScoreFillColor);
                ctx.FillPath();
                ctx.DrawPath(CGPathDrawingMode.Fill);

                ctx.BeginPath();
                ctx.AddPath(LinesBackgroundPath);
                ctx.ClosePath();
                ctx.SetStrokeColor(LinesBackgroundColor);
                ctx.StrokePath();
                ctx.DrawPath(CGPathDrawingMode.Stroke);

                ctx.BeginPath();
                ctx.AddPath(LinesFillPath);
                ctx.ClosePath();
                ctx.SetStrokeColor(LinesFillColor);
                ctx.StrokePath();
                ctx.DrawPath(CGPathDrawingMode.Stroke);

                ctx.RestoreState();
            }
            catch (Exception)
            {
            }
        }
    }
}