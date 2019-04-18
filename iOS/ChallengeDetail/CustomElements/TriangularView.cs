using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public class TriangularView : UIView
    {
        public TriangularView(CGRect frame)
        {
            BackgroundColor = UIColor.White;
            Frame = frame;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                g.SetLineWidth(5);
                UIColor.White.SetFill();
                UIColor.White.SetStroke();

                var path = new CGPath();

                var point1 = new CGPoint(0f, Frame.Height);
                var point2 = new CGPoint((Frame.Width / 2f), 0f);
                var point3 = new CGPoint(Frame.Width, Frame.Height);
                path.AddLines(new CGPoint[] { point1, point2, point3 });

                path.CloseSubpath();

                CAShapeLayer mask = new CAShapeLayer();
                mask.Frame = this.Frame;
                mask.Path = path;
                this.Layer.Mask = mask;

                g.AddPath(path);
                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

    }
}