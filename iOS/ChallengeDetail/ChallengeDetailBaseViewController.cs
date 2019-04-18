using System;
using UIKit;
using SocialLadder.Models;
using CoreGraphics;
using System.Diagnostics;
using SocialLadder.iOS.Constraints;
using FFImageLoading;
using SocialLadder.Logger;
using System.Threading;
using WebKit;
using System.Threading.Tasks;
using SocialLadder.iOS.Navigation;

namespace SocialLadder.iOS.Challenges
{
    public class ChallengeDetailBaseViewController : UIViewController
    {
        public bool DidLoadChallengeData { get; set; }
        public ChallengeModel Challenge { get; set; }
        public UIView Overlay { get; set; }
        public bool IsDisappeared { get; set; }
        UIImage SubmitButtonImage { get; set; }
        public ChallengeDetailBaseViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            LoadChallengeData();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //RemoveOverlay();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            IsDisappeared = true;
        }

        public void AddOverlay()
        {
            var statusBarFrameHeight = UIApplication.SharedApplication.StatusBarFrame.Height;
            var navBarHeihgt = this.NavigationController.NavigationBar.Frame.Size.Height;
            var overlayFrame = new CGRect(View.Frame.X , View.Frame.Y+ statusBarFrameHeight + navBarHeihgt, View.Frame.Width, View.Frame.Height - statusBarFrameHeight - navBarHeihgt);
            Overlay = Platform.AddOverlay(this.View, overlayFrame, UIColor.White, true);
            if (Overlay != null)
            {
                nfloat w = Overlay.Frame.Width;//View.Frame.Width;//UIScreen.MainScreen.Bounds.Width;
                nfloat h = Overlay.Frame.Height;//View.Frame.Height;//UIScreen.MainScreen.Bounds.Height;
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

        public virtual void LoadChallengeData()
        {
           
                if (!DidLoadChallengeData)
                {
                    AddOverlay();
                    //button.SetBackgroundImage(UIImage.FromBundle("loading-indicator"), UIControlState.Normal);
                    //Platform.AnimateRotation(button);
                    SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, Refresh);
                }
   
        }

        public virtual void ExceptionResponse(string ex)
        {
            RemoveOverlay();
            Debug.WriteLine(@"              ExceptionResponse.");
        }

        public virtual void Refresh(ChallengeResponseModel response)
        {
            DidLoadChallengeData = true;
            RemoveOverlay();
        }

        public virtual void SubmitChallenge(UIButton button)
        {
            SubmitButtonImage = button.BackgroundImageForState(UIControlState.Normal);
            RemoveOverlay();
            Overlay = Platform.AddOverlay(View, View.Frame, UIColor.Clear, true);
            button.SetBackgroundImage(UIImage.FromBundle("loading-indicator"), UIControlState.Normal);
            Platform.AnimateRotation(button);
            LogHelper.LogChallengeSubmition(Challenge.ID.ToString(), Challenge.Name);
        }

        public virtual void SubmitChallengeComplete(UIButton button, ResponseModel response)
        {
            RemoveOverlay();
            Platform.AnimateRotationComplete(button);

            if (SubmitButtonImage != null)
                button.SetBackgroundImage(SubmitButtonImage, UIControlState.Normal);

            if (response != null)
            {
                UIView overlay = Platform.AddOverlay(response.ResponseCode > 0);
                if (overlay != null)
                {
                    ChallengeCompleteView challengeComplete = ChallengeCompleteView.Create();
                    overlay.AddSubview(challengeComplete);
                    if (response is ChallengeResponseModel)
                    {
                        challengeComplete.Update(overlay, response as ChallengeResponseModel, Challenge, this);
                    }
                    else if (response is ShareResponseModel)
                    {
                        challengeComplete.Update(overlay, response as ShareResponseModel, Challenge, this);
                    }

                    challengeComplete.TranslatesAutoresizingMaskIntoConstraints = false;
                    overlay.AddConstraint(ChallengesConstraints.ChallengesCollectionCellCenterXConstraint(challengeComplete, overlay));
                    overlay.AddConstraint(ChallengesConstraints.ChallengesConstantTopConstraint(challengeComplete, overlay, 0f));
                    overlay.AddConstraint(ChallengesConstraints.ChallengesCollectionCellWidthConstraint(challengeComplete, overlay, 0.94f));
                    overlay.AddConstraint(ChallengesConstraints.ChallengesCollectionCellHeightConstraint(challengeComplete, overlay, 0.81f));
                }
            }
        }

        public async void CheckReloadWebView(CancellationToken token, WKWebView webView)
        {
            try
            {
                await Task.Delay(5000, token);

                webView.EvaluateJavaScript("document.body.getBoundingClientRect().bottom", completionHandler: (height, error) =>
                {
                    if (!IsDisappeared && (error != null || String.IsNullOrWhiteSpace(height.Description) || int.Parse(height.Description) < 4))
                    {
                        Platform.ClearBrowserCache();
                        webView.LoadHtmlString(Challenge.Desc, null);
                    }
                });
            }
            catch (Exception) { }
        }
    }
}