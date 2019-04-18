using System;
using CoreGraphics;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public class PointsBaseView : UIView
    {
        public bool DidBuild { get; set; }
        //public float HeaderY { get; set; }

        public static float TopToBackgroundBottomSpaceToScreenWidthRatio = 0.75f; //from spec

        public PointsBaseView(IntPtr handle) : base(handle)
        {

        }

        public PointsBaseView(CGRect frame) : base(frame)
        {

        }

        float _maxWidth;
        public float MaxWidth
        {
            get
            {
                return _maxWidth;
            }
            set
            {
                float w = Width;
                _maxWidth = value;
                if (Math.Abs(w - Width) > 0.1f)
                    ApplySize();
            }
        }

        public float Width
        {
            get
            {
                return MaxWidth > 0 && MaxWidth < Frame.Width ? MaxWidth : (float)Frame.Width;
            }
        }

        public virtual void ApplySize()
        {
            
        }

        public float CustomHeight
        {
            get
            {
                float TriangleW = Width * 0.750f;       //from spec
                float TriangleH = TriangleW * 0.3129f;  //from spec
                return TriangleH;
            }
        }
    }
}