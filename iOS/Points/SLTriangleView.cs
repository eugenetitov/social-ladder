using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public partial class SLTriangleView : PointsBaseView
    {
        CGColor HeaderColor { get; set; }
        CGPath HeaderPath { get; set; }

        public static float TopToHeaderTopToScreenWidithRatio = 0.46f;     //from spec

        public float ScoreImageWidthToTriangleViewDistanceRatio { get; set; }

        public UIImageView ScoreImage { get; set; }    //found on viewcontroller and connected in viewdidload so we can determine x coordinate to draw the timeline

        public SLTriangleView (IntPtr handle) : base (handle)
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

            ScoreImageWidthToTriangleViewDistanceRatio = 0.234f;  //from spec
        }

        public void Build()
        {
            if (!DidBuild)
            {
                float heightToWidthRatio = 0.317f;//29f;            //from spec

                //build header
                nfloat headerWidth = Width;
                nfloat headerHeight = Width * heightToWidthRatio;
                //nfloat headerY = Frame.Height - headerHeight;
                nfloat headerY = TopToHeaderTopToScreenWidithRatio * Width;//HeaderY;//Frame.Height - headerHeight - CustomHeight;
                HeaderPath = CreateTrianglePath(0, headerY, headerWidth, headerHeight, 0);

                //nfloat y = ScoreImage.Frame.Width * ScoreImageWidthToTriangleViewDistanceRatio;
                //HeaderPath = CreateTrianglePath(0, y, Frame.Width, Frame.Width * heightToWidthRatio, 0);

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

            //draw timeline
            nfloat x = ScoreImage.Frame.X + ScoreImage.Frame.Width / 2.0f;  //center timeline on the scoreimage
            nfloat y = ScoreImage.Frame.Y + ScoreImage.Frame.Height;
            ctx.SetStrokeColor(223.0f / 255.0f, 223.0f / 255.0f, 223.0f / 255.0f, 1.0f);
            ctx.SetLineWidth(2.0f);
            ctx.MoveTo(x, y);
            ctx.AddLineToPoint(x, this.Bounds.Size.Height);
            ctx.StrokePath();

            ctx.RestoreState();
        }

        CGPath CreateTrianglePath(nfloat x, nfloat y, nfloat width, nfloat height, nfloat cornerRadius)
        {
            //calc 3 points for the triangle         
            var point1 = new CGPoint(x + width, y + height);        //mid right
            var point2 = new CGPoint(x, y);                         //upper left
            var point3 = new CGPoint(x, y + height);                //mid left
            var point4 = new CGPoint(x, y + Frame.Height);          //lower left
            var point5 = new CGPoint(x + width, y + Frame.Height);  //lower right

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