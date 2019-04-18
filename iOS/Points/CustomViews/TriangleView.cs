using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SocialLadder.iOS.Points.CustomViews
{
    public class TriangleView : UIView
    {
        public TriangleView(CGRect frame) : base(frame)
        {

        }

        public TriangleView(NSCoder sCoder) : base(sCoder)
        {

        }

        public override void Draw(CGRect area)
        {
            var ctx = UIGraphics.GetCurrentContext();
            var myRectanglePath = new CGPath();
            myRectanglePath.AddRect(new RectangleF(new PointF((float)area.GetMinX(), (float)area.GetMaxY()), new SizeF((float)area.GetMaxX() / 2, (float)area.GetMinY())));
            ctx.AddPath(myRectanglePath);
            ctx.DrawPath(CGPathDrawingMode.Stroke);
        }

    }
}