using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class CircleView : UIView
    {
        nfloat Radius { get; set; }
        public event Action OnCompleted;
        public CircleView(CGRect frame, UIColor color, nfloat radius, float duration) : base(frame)
        {
            this.BackgroundColor = color;
            this.Radius = radius;
            this.Layer.CornerRadius = Frame.Width / 2.0f;
            UIView.Animate(duration, HandleAnimation, HandleComplete);
        }

        void HandleAnimation()
        {
            nfloat r = this.Radius;
            CGRect f = this.Layer.Frame;

            this.Layer.Frame = new CGRect(f.X - (((2.0f * r) - f.Width) / 2.0f), f.Y - (((2.0f * r) - f.Height) / 2.0f), r * 2.0f, r * 2.0f);


            this.Layer.CornerRadius = r;
            this.Layer.BackgroundColor = UIColor.FromRGBA(241, 114, 117, 117).CGColor;
           
        }

        void HandleComplete()
        {
            OnCompleted();
            RemoveFromSuperview();
        }
    }
}