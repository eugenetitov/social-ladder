using Foundation;
using System;
using UIKit;
using SocialLadder.Models;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Services;
using SocialLadder.Interfaces;
using System.Collections.Generic;
using Facebook.ShareKit;
using SocialLadder.Logger;

namespace SocialLadder.iOS.Challenges
{
    public partial class ShareViewController : ChallengeDetailBaseViewController, ISharingDelegate, IUIAlertViewDelegate
    {
        public ShareTemplateModel ShareTemplate { get; set; }
        public UIButton SubmitButton { get; set; }
        private readonly string placeholderText = "   ";
        private UITapGestureRecognizer tapGesture;
        private string shareText;

        public ShareViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnShare.SetBackgroundImage(UIImage.FromBundle("challenge-btn_share"), UIControlState.Normal);
            btnShare.TouchUpInside += ((object sender, EventArgs args) =>
            {
                PostShare(ShareMessage.Text);
            });

            ReferralString.Text = null;
            //ShareMessage.Text = null;
            ShareMessage.ReturnKeyType = UIReturnKeyType.Send;
            //ShareMessage.ShouldReturn += TextFieldShouldReturn;
            //ShareMessage.ShouldChangeText += ShareMessage_ShouldChangeText;
            FacebookSwitch.On = TwitterSwitch.On = false;
            TwitterSwitch.ValueChanged += (s, e) => { UpdateSwitch(TwitterImage, TwitterSwitch.On, "twitter"); };
            FacebookSwitch.ValueChanged += (s, e) => { UpdateSwitch(FacebookImage, FacebookSwitch.On, "fb"); };

            ShareMessage.ShouldBeginEditing += (s) =>
            {
                if (s.Text.Equals(placeholderText))
                {
                    s.Text = string.Empty;
                    s.TextColor = UIColor.Black;
                }
                //s.BecomeFirstResponder();
                return true;
            };
            ShareMessage.ShouldEndEditing += (s) =>
            {

                if (ShareMessage.Text.Equals(string.Empty))
                {
                    s.Text = placeholderText;
                    s.TextColor = UIColor.LightGray;
                }
                //s.ResignFirstResponder();
                return true;
            };
            tapGesture = new UITapGestureRecognizer { CancelsTouchesInView = false };
            tapGesture.AddTarget(() => View.EndEditing(true));
            View.AddGestureRecognizer(tapGesture);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            tapGesture.Dispose();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //AddOverlay();
            SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, ShareTemplateRefreshed);
            ShareMessage.Text = placeholderText;
            ShareMessage.TextColor = UIColor.LightGray;
            TwitterImage.Layer.CornerRadius = FacebookImage.Layer.CornerRadius = SizeConstants.Screen.Width * (0.1376f / 2f);
            FacebookImage.Image = UIImage.FromBundle("fb-logo_gray");
            TwitterImage.Image = UIImage.FromBundle("twitter-logo_gray");
            TwitterImage.Layer.BorderColor = FacebookImage.Layer.BorderColor = UIColor.FromRGB(210, 210, 210).CGColor;
            TwitterImage.Layer.BorderWidth = FacebookImage.Layer.BorderWidth = SizeConstants.Screen.Width * 0.018f;
            FacebookImage.ContentMode = TwitterImage.ContentMode = UIViewContentMode.Center;
            ShareMessage.Font = ReferralString.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.04f);
            Count.Font = FacebookText.Font = TwitterText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.034f);
            SelectText.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.Screen.Width * 0.038f);
        }

        private void UpdateSwitch(UIImageView imageView, bool selected, string bundleName)
        {
            if (selected)
            {
                imageView.Layer.BorderWidth = 0f;
                imageView.Layer.BorderColor = UIColor.Clear.CGColor;
                imageView.Image = UIImage.FromBundle(bundleName + "-logo_white");
            }
            if (!selected)
            {
                imageView.Layer.BorderWidth = SizeConstants.Screen.Width * 0.018f;
                imageView.Layer.BorderColor = UIColor.FromRGB(210, 210, 210).CGColor;
                imageView.Image = UIImage.FromBundle(bundleName + "-logo_gray");
            }
        }

        public void ShareTemplateRefreshed(ShareResponseModel shareResponse)
        {
            if (shareResponse != null && shareResponse.ShareTemplate != null)
            {
                ShareTemplate = shareResponse.ShareTemplate;
                ReferralString.Text = ShareTemplate.ReferralString;
                //ShareMessage.Text = ShareTemplate.PostTitle;
            }
            RemoveOverlay();
        }

        public override void SubmitChallenge(UIButton button)
        {
            base.SubmitChallenge(button);

        }

        void PostShare(string shareText)
        {
            this.shareText = shareText;
            if (!FacebookSwitch.On)
            {
                return;
            }

            IFacebookService service = new IOSFacebookService();
            service.ShareFacebookChallenge(this, Challenge, shareText.Equals(placeholderText) ? string.Empty : shareText);

            //if (ShareTemplate != null)//commented error message before login view
            //{
            //    NavigationController.PopViewController(true);
            //    SubmitChallenge(SubmitButton);

            //    ShareModel share = new ShareModel();
            //    share.ShareTransactionId = ShareTemplate.ShareTransactionID;
            //    share.BodyMessage = shareText;
            //    share.ShareEntryList = System.Linq.Enumerable.ToList(ShareTemplate.NetworkShareList); ;//new List<ShareNetworkStatus>() { new ShareNetworkStatus { NetworkName = "Twitter", AllowSharing = true } }
            //    SL.Manager.PostShare(share, PostShareResponse);

            //}
        }

        bool TextFieldShouldReturn(UITextField textfield)
        {
            /*
            if (ShareTemplate != null)
            {
                ShareModel share = new ShareModel();
                share.ShareTransactionId = ShareTemplate.ShareTransactionID;
                share.BodyMessage = textfield.Text;
                share.ShareEntryList = System.Linq.Enumerable.ToList(ShareTemplate.NetworkShareList);
                SL.Manager.PostShare(share, PostShareResponse);
            }
            */
            PostShare(textfield.Text);
            textfield.ResignFirstResponder();
            return false; // We do not want UITextField to insert line-breaks.
        }

        bool ShareMessage_ShouldChangeText(UITextView textView, NSRange range, string text)
        {
            if (text.Length == 1 && text == Environment.NewLine)
            {
                /*
                if (ShareTemplate != null)
                {
                    ShareModel share = new ShareModel();
                    share.ShareTransactionId = ShareTemplate.ShareTransactionID;
                    share.BodyMessage = textView.Text;
                    share.ShareEntryList = System.Linq.Enumerable.ToList(ShareTemplate.NetworkShareList);
                    SL.Manager.PostShare(share, PostShareResponse);
                }
                */
                PostShare(textView.Text);
                textView.ResignFirstResponder();
                return false;
            }
            return true;
        }

        public void PostShareCompleteResponse(ShareResponseModel shareResponse)
        {
            if (shareResponse.ResponseCode > 0 && String.IsNullOrEmpty(shareResponse.ResponseMessage))
            {
                shareResponse.ResponseMessage = "Congratulations!\r\nChallenge Completed!";
            }
            SubmitChallengeComplete(SubmitButton, shareResponse);

            //SubmitChallengeComplete(SubmitButton, null);

            //UIView overlay = Platform.AddOverlay(shareResponse.ResponseCode > 0);  //from spec
            //if (overlay != null)
            //{
            //    ChallengeCompleteView challengeComplete = ChallengeCompleteView.Create();
            //    overlay.AddSubview(challengeComplete);
            //    challengeComplete.Update(overlay, shareResponse, Challenge);
            //}
        }

        public void DidComplete(ISharing sharer, NSDictionary results)
        {
            SL.Manager.PostSubmitShare(ShareTemplate.ShareTransactionID, PostShareCompleteResponse);
        }

        public void DidFail(ISharing sharer, NSError error)
        {
            var alert = new UIAlertView("Sharing Error", error?.LocalizedDescription ?? "You need the native Facebook for iOS app installed for sharing images", this, "Ok");
            alert.Show();
            LogHelper.LogUserMessage("FB SHARE DIALOG", error.Description);
            //IFacebookService service = new IOSFacebookService();
            //service.SendOpenGraph(this, Challenge, shareText.Equals(placeholderText) ? string.Empty : shareText);
        }

        public void DidCancel(ISharing sharer)
        {
        }
    }
}