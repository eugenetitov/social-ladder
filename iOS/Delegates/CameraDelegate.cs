using System;
using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Camera;
using UIKit;

namespace SocialLadder.iOS.Delegates
{
    public class CameraDelegate : UIImagePickerControllerDelegate
    {
        CameraViewController CameraViewController { get; set; }

        public CameraDelegate(CameraViewController cameraViewController)
        {
            CameraViewController = cameraViewController;
        }

        public override void FinishedPickingMedia(UIImagePickerController picker, NSDictionary info)
        {
            picker.DismissModalViewController(true);

            var image = info.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
            var file = info.ValueForKey(new NSString("UIImagePickerControllerImageURL")) as NSUrl;
            var refUrl = info.ValueForKey(new NSString("UIImagePickerControllerReferenceUrl")) as NSUrl;
            image = MaxResizeImage(image, 1080, 1080);
            CameraPicture picture = new CameraPicture();
            picture.Image = image;
            if (file != null)
                picture.FileName = file.ToString();
      
            CameraViewController.OnTakePictureComplete(picture);
         
        }

        private static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
        {
            var sourceSize = sourceImage.Size;
            var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
            if (maxResizeFactor > 1) return sourceImage;
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
