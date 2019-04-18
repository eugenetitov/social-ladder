using System;
using CoreGraphics;
using UIKit;

namespace SocialLadder.iOS.Services
{
    public static class PhotoService
    {
        public static UIImage Rotate(UIImage src, UIImageOrientation orientation)
        {
            if (orientation == UIImageOrientation.Up || orientation == UIImageOrientation.UpMirrored)
            {
                return src;
            }

            nfloat angle = 0;

            if (orientation == UIImageOrientation.Right || orientation == UIImageOrientation.RightMirrored)
            {
                angle = 90;
            }

            if (orientation == UIImageOrientation.Down || orientation == UIImageOrientation.DownMirrored)
            {
                angle = 180;
            }

            if (orientation == UIImageOrientation.Left || orientation == UIImageOrientation.LeftMirrored)
            {
                angle = 270;
            }

            var radians = GetRadians(angle);
            CGAffineTransform t = CGAffineTransform.MakeRotation(radians);
            UIView rotatedViewBox = new UIView(new CGRect(0, 0, src.Size.Width, src.Size.Height));
            rotatedViewBox.Transform = t;
            CGSize rotatedSize = rotatedViewBox.Frame.Size;

            UIGraphics.BeginImageContext(src.Size);
            var context = UIGraphics.GetCurrentContext();
            context.TranslateCTM(src.Size.Width / 2.0f, src.Size.Height / 2.0f);
            context.RotateCTM(radians);
            context.ScaleCTM(1.0f, -1.0f);
            context.TranslateCTM(-rotatedSize.Width / 2.0f, -rotatedSize.Height / 2.0f);
            context.DrawImage(new CGRect(0, 0, rotatedSize.Width, rotatedSize.Height), src.CGImage);
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return image;
        }

        private static nfloat GetRadians(double degrees)
        {
            return (nfloat)(degrees * Math.PI / 180.0);
        }

        public static UIImage ResizeImage(UIImage sourceImage, float maxWidth = 1080, float maxHeight = 1080)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Min(maxWidth / (sourceSize.Width == 0 ? maxWidth : sourceSize.Width), maxHeight / (sourceSize.Height == 0 ? maxHeight : sourceSize.Height));
            if (maxResizeFactor >= 1)
            {
                return sourceImage;
            }

            var width = maxResizeFactor * sourceSize.Width;
            var height = maxResizeFactor * sourceSize.Height;
            UIGraphics.BeginImageContext(new CGSize((nfloat)width, (nfloat)height));
            sourceImage.Draw(new CGRect(0, 0, (nfloat)width, (nfloat)height));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return resultImage;
        }
    }
}