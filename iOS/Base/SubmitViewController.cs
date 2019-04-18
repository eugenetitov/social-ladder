using System;
using UIKit;
using SocialLadder.Models;
using CoreGraphics;

namespace SocialLadder.iOS.Base
{
    public class SubmitViewController : UIViewController
    {
        public bool DidLoadData { get; set; }
        public UIView Overlay { get; set; }
        UIImage SubmitButtonImage { get; set; }

        public SubmitViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            RemoveOverlay();
        }

        public void AddOverlay()
        {
            Overlay = Platform.AddOverlay(View.Frame, UIColor.White, true);
            if (Overlay != null)
            {
                nfloat w = Overlay.Frame.Width;
                nfloat h = Overlay.Frame.Height;
                nfloat s = w * 0.2f;
                UIImageView progress = new UIImageView(new CGRect((w - s) / 2.0f, ((h - s) / 2.0f) - s / 2, s, s));
                progress.Image = UIImage.FromBundle("loading-indicator");
                Overlay.AddSubview(progress);
                Platform.AnimateRotation(progress);
            }
        }

        public void RemoveOverlay()
        {
            if (Overlay != null)
            {
                Overlay.RemoveFromSuperview();
                Overlay = null;
            }
        }

        public virtual void LoadData()
        {
            if (!DidLoadData)
            {
                AddOverlay();
                //SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, Refresh);
            }
        }

        public virtual void Refresh(ChallengeModel challenge)
        {
            DidLoadData = true;
            RemoveOverlay();
        }

        public virtual void Submit(UIWindow window, UIButton button, Action onClosed)
        {
            Overlay = Platform.AddOverlay(window, window.Frame, UIColor.FromRGBA(1, 1, 1, 0), true, onClosed);

            button.SetBackgroundImage(UIImage.FromBundle("loading-indicator"), UIControlState.Normal);
            Platform.AnimateRotation(button);
        }

        public virtual void Submit(UIWindow window, UIButton button)
        {
            Action onClosed = (() =>
            {
                button.SetBackgroundImage(SubmitButtonImage, UIControlState.Normal);
            });
          
            SubmitButtonImage = button.BackgroundImageForState(UIControlState.Normal);
            Submit(window, button, onClosed);
        }

        public virtual void SubmitComplete(UIButton button, ResponseModel response, Action onClosed)
        {
            RemoveOverlay();
            Platform.AnimateRotationComplete(button);
            button.SetBackgroundImage(SubmitButtonImage, UIControlState.Normal);

            //if (response != null)
            //    Overlay = Platform.AddOverlay(response.ResponseCode > 0, onClosed);
        }

        public virtual void SubmitComplete(UIButton button, ResponseModel response)
        {
            Action onClosed = (() =>
            {
                button.SetBackgroundImage(SubmitButtonImage, UIControlState.Normal);
            });

            SubmitComplete(button, response, onClosed);
        }

   
    }
}