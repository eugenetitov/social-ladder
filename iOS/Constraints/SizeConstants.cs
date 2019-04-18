using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SocialLadder.iOS.Constraints
{
    public static class SizeConstants
    {
        public static readonly CGRect Screen = UIScreen.MainScreen.Bounds;
        public static nfloat ScreenWidth = Screen.Width;
        public static nfloat ScreenMultiplier = ScreenWidth / CodeBehindUIConstants.BaseMarkupWidth;
    }
}