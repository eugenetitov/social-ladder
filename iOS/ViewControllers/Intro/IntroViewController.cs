using System;
using UIKit;
using AVFoundation;
using Foundation;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    public partial class IntroViewController : UIViewController
    {
        partial void LoginButton_TouchUpInside(UIButton sender)
        {
            LoadNetworks();
        }

        public IntroViewController(IntPtr handle) : base(handle)
        {
            
        }

        void ApplyStyle()
        {
            Platform.AddVideo(View);
            Platform.AddCover(View);
        }

        private void HandleDrag(UIPanGestureRecognizer recognizer)
        {
            // If it's just began, cache the location of the image
            if (recognizer.State == UIGestureRecognizerState.Began)
            {
                //originalImageFrame = DragImage.Frame;
            }

            // Move the image if the gesture is valid
            if (recognizer.State != (UIGestureRecognizerState.Cancelled 
                                    | UIGestureRecognizerState.Failed
                 | UIGestureRecognizerState.Possible))
            {
                /*
                CoreGraphics.CGRect f = startFrame;//playerLayer.Frame;
                CoreGraphics.CGPoint offset = recognizer.TranslationInView(View);
                playerLayer.Frame = new CoreGraphics.CGRect(f.X+offset.X, f.Y+offset.Y, f.Width, f.Height);
                //playerLayer.VideoGravity = AVLayerVideoGravity.ResizeAspect;
                */
                /*
                // Move the image by adding the offset to the object's frame
                PointF offset = recognizer.TranslationInView(DragImage);
                RectangleF newFrame = originalImageFrame;
                newFrame.Offset(offset.X, offset.Y);
                DragImage.Frame = newFrame;
                */
            }
        }

        private void WireUpDragGestureRecognizer()
        {
            // Create a new tap gesture
            UIPanGestureRecognizer gesture = new UIPanGestureRecognizer();
            // Wire up the event handler (have to use a selector)
            gesture.AddTarget(() => HandleDrag(gesture));  // to be defined
            // Add the gesture recognizer to the view
            View.AddGestureRecognizer(gesture);
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            WireUpDragGestureRecognizer();
            //LoopVideo();
            ApplyStyle();
        }

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}

        private void LoadNetworks()
        {
            //View.Layer.rem

            UIStoryboard board = UIStoryboard.FromName("Networks", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("Networks");
            this.PresentViewController(ctrl, false, null);
        }

		public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

