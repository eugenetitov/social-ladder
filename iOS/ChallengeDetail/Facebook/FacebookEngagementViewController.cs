using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.Models;
using System;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class FacebookEngagementViewController : ChallengeDetailBaseViewController//, IUITextViewDelegate
    {
        public ShareTemplateModel ShareTemplate { get; set; }

        public FacebookEngagementViewController(IntPtr handle) : base(handle)
        {
        }
        /*
        public void ShareTemplateRefreshed(ShareResponseModel shareResponse)
        {
            if (shareResponse != null && shareResponse.ShareTemplate != null)
            {
                ShareTemplate = shareResponse.ShareTemplate;
                ShareText.Text = ShareTemplate.PostTitle;
            }
        }
        */
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            EngagementLink.Font = DescriptionShareLinkLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.038f);
            LinksView.Layer.BorderWidth = 0.5f;
            LinksView.Layer.BorderColor = UIColor.Gray.CGColor;

            EngagementLink.ScrollEnabled = false;

            //LinkText.ReturnKeyType = UIReturnKeyType.Done;
            //LinkText.ShouldReturn += LinkText_ShouldReturn;

            //LinkText.ShouldChangeCharacters += (field, range, replacementString) => false;  //readonly
            //LinkText.InputView = new UIView(new CGRect(0, 0, 0, 0));    //no keyboard
            //LinkText.Delegate = this;
            //LinkText.Selectable = true;
            //LinkText.DataDetectorTypes = UIDataDetectorType.Link;

            //LinkText.ShouldBeginEditing += (textField) => false;

            ShareText.ReturnKeyType = UIReturnKeyType.Send;
            ShareText.ShouldChangeText += ShareText_ShouldChangeText;

            //Refresh(Challenge);
        }
        /*
        void RefreshShareTemplateIfNeeded()
        {
            if (ShareTemplate == null && !string.IsNullOrWhiteSpace(Challenge.ShareTemplateURL))
                SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, ShareTemplateRefreshed);
        }
        */
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, Refresh);
            //RefreshShareTemplateIfNeeded();
        }

        public override void Refresh(ChallengeResponseModel challengeResponse)
        {
            Challenge = challengeResponse.Challenge;

            //NSAttributedString attributedString = new NSAttributedString(Challenge.TargetObjectURL, )
            //NSAttributedString attributedString = [[NSAttributedString alloc] initWithString: @"Google"
            //                                                           attributes:@{ NSLinkAttributeName: [NSURL URLWithString: @"http://www.google.com"] }];
            //self.textView.attributedText = attributedString;

            //LinkText.Text = Challenge.TargetObjectURL;

            //Platform.LinkText(LinkText, Challenge.TargetObjectURL);

            //RefreshShareTemplateIfNeeded();

            RemoveOverlay();
        }
        /*
        [Export("textView:shouldInteractWithURL:inRange:")]
        public bool ShouldInteractWithUrl(UITextView textView, NSUrl URL, NSRange characterRange)
        {
            return true;
        }
        */
        /*
        bool LinkText_ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            return false; // We do not want UITextField to insert line-breaks.
        }
        */
        bool ShareText_ShouldChangeText(UITextView textView, NSRange range, string text)
        {
            if (text.Length == 1 && text == Environment.NewLine)
            {
                if (ShareTemplate != null)
                {
                    ShareModel share = new ShareModel();
                    share.ShareTransactionId = ShareTemplate.ShareTransactionID;
                    share.BodyMessage = textView.Text;
                    share.ShareEntryList = System.Linq.Enumerable.ToList(ShareTemplate.NetworkShareList);
                    SL.Manager.PostShare(share, PostShareResponse);
                }

                textView.ResignFirstResponder();
                return false;
            }
            return true;
        }

        public void PostShareResponse(ShareResponseModel shareResponse)
        {
            UIView overlay = Platform.AddOverlay(UIColor.FromRGBA(36, 209, 180, 153));  //from spec
            if (overlay != null)
            {
                ChallengeCompleteView challengeComplete = ChallengeCompleteView.Create();
                overlay.AddSubview(challengeComplete);
                challengeComplete.Update(overlay, shareResponse, Challenge, this);
            }
        }
    }
}