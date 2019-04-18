using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public partial class SLMountainView : PointsBaseView
    {
        CGColor HeaderColor { get; set; }
        CGPath HeaderPath { get; set; }

        public static float TopToHeaderTopToScreenWidithRatio = 0.3f;     //from spec

        public SLMountainView (IntPtr handle) : base (handle)
        {
            ApplyStyle();
            //Build();
        }

        void ApplyStyle()
        {
            //iOS Styles
            Layer.ZPosition = 1;
            //BackgroundColor = UIColor.Clear;
            //Opaque = false;

            //Custom Styles
            HeaderColor = UIColor.White.CGColor;
        }

        public void Build()
        {
            if (!DidBuild)
            {
                float heightToWidthRatio = 0.1594f;//0.3129f;            //from spec

                nfloat extension = Width * 0.317f;

                //build header
                nfloat headerWidth = Width;
                nfloat headerHeight = Width * heightToWidthRatio;

                //nfloat headerY = Frame.Height - headerHeight;
                nfloat headerY = TopToHeaderTopToScreenWidithRatio * Width;//HeaderY;//Frame.Height - headerHeight - extension - CustomHeight;
                HeaderPath = CreatePentagonPath(0, headerY, headerWidth, headerHeight, 0);

                DidBuild = true;
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            var ctx = UIGraphics.GetCurrentContext();
            ctx.SaveState();

            //draw header triangle
            ctx.BeginPath();
            ctx.AddPath(HeaderPath);
            ctx.ClosePath();
            ctx.SetFillColor(HeaderColor);
            ctx.FillPath();
            ctx.DrawPath(CGPathDrawingMode.Fill);

            ctx.RestoreState();
        }

        CGPath CreatePentagonPath(nfloat x, nfloat y, nfloat width, nfloat height, nfloat cornerRadius)
        {
            //calc 5 points for the triangle
            var point1 = new CGPoint(x + width, y + height);    //mid right
            var point2 = new CGPoint(x + width / 2.0f, y);             //upper center
            var point3 = new CGPoint(x, y + height);            //mid left
            var point4 = new CGPoint(x, y + Frame.Height);                   //lower left
            var point5 = new CGPoint(x + width, y + Frame.Height);           //lower right
            /*
            var point1 = new CGPoint(x + width, y + height / 2.0f);    //mid right
            var point2 = new CGPoint(x + width / 2.0f, y);             //upper center
            var point3 = new CGPoint(x, y + height / 2.0f);            //mid left
            var point4 = new CGPoint(x, y + height);                   //lower left
            var point5 = new CGPoint(x + width, y + height);           //lower right
            */
            //calc the center of an edge
            var baseMidpoint = new CGPoint((point4.X + point5.X) / 2.0f, (point4.Y + point5.Y) / 2.0f);

            //create the path starting with base midpoint and adding arc counter clockwise following the points
            var path = new CGPath();
            path.MoveToPoint(baseMidpoint.X, baseMidpoint.Y);
            path.AddArcToPoint(point5.X, point5.Y, point1.X, point1.Y, cornerRadius);
            path.AddArcToPoint(point1.X, point1.Y, point2.X, point2.Y, cornerRadius);
            path.AddArcToPoint(point2.X, point2.Y, point3.X, point3.Y, cornerRadius);
            path.AddArcToPoint(point3.X, point3.Y, point4.X, point4.Y, cornerRadius);
            path.AddArcToPoint(point4.X, point4.Y, point5.X, point5.Y, cornerRadius);

            path.CloseSubpath();

            return path;
        }
    }
}