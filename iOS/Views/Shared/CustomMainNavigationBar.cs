using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace SocialLadder.iOS.Views.Shared
{
   public class CustomMainNavigationBar: UINavigationBar
    {
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;

        public override CGSize SizeThatFits(CGSize size)
        {
            return new CGSize(_screenWidth, 132f / 414f * _screenWidth);

        }
    }
}