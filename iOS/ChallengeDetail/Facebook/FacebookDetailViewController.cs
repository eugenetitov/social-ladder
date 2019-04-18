using CoreGraphics;
using Facebook.ShareKit;
using FFImageLoading;
using Foundation;
using SocialLadder.Interfaces;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Delegates;
using SocialLadder.iOS.Services;
using SocialLadder.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class FacebookDetailViewController : ChallengeDetailBaseViewController, ISharingDelegate, IUIAlertViewDelegate
    {
        public ShareTemplateModel ShareTemplate { get; set; }
        private IDisposable _webViewObserver;
        private nfloat _lastWebViewHeight;
        private CancellationTokenSource _webViewCancellationTokenSource;

        public FacebookDetailViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //Refresh(Challenge);
            Reset();
            HeaderTextLbl.Text = Challenge.Name;
            TimeLastLbl.Text = Challenge.NextEventCountDown;

            NSString viewportScriptString = (NSString)"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=500'); meta.setAttribute('initial-scale', '1.0'); meta.setAttribute('maximum-scale', '1.0'); meta.setAttribute('minimum-scale', '1.0'); meta.setAttribute('user-scalable', 'no'); document.getElementsByTagName('head')[0].appendChild(meta);";

            WebView.Configuration.UserContentController.AddUserScript(new WKUserScript(source: viewportScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true));

            WebView.ScrollView.ScrollEnabled = false;
            WebView.ScrollView.Bounces = false;
            WebView.AllowsBackForwardNavigationGestures = false;
            Platform.ClearBrowserCache();

            if (!SL.IsNetworkConnected("Facebook"))
            {
                SubmitButton.Hidden = true;
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, ShareTemplateRefreshed);

            //LoadChallengeData();

            FBCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            FBCollectionView.RemoveConstraint(FBCollectionViewAspect);
            FBCollectionView.AddConstraint(ChallengesConstraints.ChallengesConstantHeightConstraint(FBCollectionView, 0f));
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            MainScroll.Scrolled += MainScroll_Scrolled;
            //SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, Refresh);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            MainScroll.Scrolled -= MainScroll_Scrolled;
            _webViewCancellationTokenSource?.Cancel();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            _webViewObserver?.Dispose();
        }

        private void MainScroll_Scrolled(object sender, EventArgs e)
        {
            ChallengesImage.RemoveConstraint(AspectHeight);
            var scrollView = (sender as UIScrollView);
            double offset = scrollView.ContentOffset.Y;
            nfloat newMultiplier = (nfloat)(ChallengesImage.Frame.Width + offset) / ChallengesImage.Frame.Width * 1.709f;
            AspectHeight = NSLayoutConstraint.Create(ChallengesImage, NSLayoutAttribute.Width, NSLayoutRelation.Equal, ChallengesImage, NSLayoutAttribute.Height, newMultiplier, 0);
            ChallengesImage.AddConstraint(AspectHeight);
        }

        public void ShareTemplateRefreshed(ShareResponseModel shareResponse)
        {
            if (shareResponse != null && shareResponse.ShareTemplate != null)
            {
                ShareTemplate = shareResponse.ShareTemplate;
            }

            RemoveOverlay();
        }

        void Reset()
        {
            TimeLastLbl.Text = null;
            CountPeopleLbl.Text = null;
            HeaderTextLbl.Text = null;
            ChallengesImage.Image = null;
            //MainTextLbl.Text = null;
        }

        public override void Refresh(ChallengeResponseModel challengeResponse)
        {
            base.Refresh(challengeResponse);

            if (challengeResponse == null)
                return;

            Challenge = challengeResponse.Challenge;
            TimeLastLbl.Text = Challenge.NextEventCountDown;
            CountPeopleLbl.Text = "+" + Challenge.PointValue.ToString() + " pts";
            HeaderTextLbl.Text = Challenge.Name;

            var navigationDelegate = new ChallengeDetailWebViewNavigationDelegate();
            navigationDelegate.NavigationFinished += SetupConstraint;
            this.WebView.NavigationDelegate = navigationDelegate;
            WebView.LoadHtmlString(Challenge.Desc, null);
            ImageService.Instance.LoadUrl(Challenge.Image).Into(ChallengesImage);
        }

        private void SetupConstraint()
        {
            if (_webViewObserver == null)
            {
                _webViewObserver = WebView.ScrollView.AddObserver("contentSize", NSKeyValueObservingOptions.New, (o) =>
                {
                    if (WebView.ScrollView.ContentSize.Height != _lastWebViewHeight)
                    {
                        _lastWebViewHeight = WebView.ScrollView.ContentSize.Height;
                        SetupConstraint();
                    }
                });
            }

            if (_webViewCancellationTokenSource == null && !IsDisappeared)
            {
                _webViewCancellationTokenSource = new CancellationTokenSource();
                CheckReloadWebView(_webViewCancellationTokenSource.Token, WebView);
            }

            nfloat actualHeight = WebView.ScrollView.ContentSize.Height;// < 3 ? 1000 : WebView.ScrollView.ContentSize.Height;
            cnsWebViewHeight.Constant = actualHeight;

            CGSize scrollContentSize = MainScroll.ContentSize;
            scrollContentSize.Height = this.WebView.Frame.Top + actualHeight;
            MainScroll.ContentSize = scrollContentSize;

            View.LayoutIfNeeded();
        }

        public override void SubmitChallenge(UIButton button)
        {
            base.SubmitChallenge(button);
            SL.Manager.SubmitAnswerAsync(Challenge.ID, null, null, SubmitResponse);
        }

        partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            SharePhotoToFacebook();
            //SubmitChallenge(SubmitButton);
            ////SL.Manager.SubmitAnswerAsync(Challenge.ID, null, null, SubmitResponse);
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            CountPeopleLbl.Font = TimeLastLbl.Font = CountOthersLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.035f);
            TimeLastLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.040f);
            HeaderTextLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.060f);

            //MainTextLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.040f);
            //MainTextLbl.LineBreakMode = UILineBreakMode.WordWrap;
            //MainTextLbl.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
            //var labelString = new NSMutableAttributedString(MainTextLbl.Text);
            //var paragraphStyle = new NSMutableParagraphStyle { LineSpacing = 2f };
            //var style = UIStringAttributeKey.ParagraphStyle;
            //var range = new NSRange(0, labelString.Length);

            //labelString.AddAttribute(style, paragraphStyle, range);
            //MainTextLbl.AttributedText = labelString;
        }
        void SubmitResponse(ChallengeResponseModel challengeResponse)
        {
            SubmitChallengeComplete(SubmitButton, challengeResponse);

            //UIView overlay = Platform.AddOverlay(challengeResponse.ResponseCode > 0);  //from spec
            //if (overlay != null)
            //{
            //    ChallengeCompleteView challengeComplete = ChallengeCompleteView.Create();
            //    overlay.AddSubview(challengeComplete);
            //    challengeComplete.Update(overlay, challengeResponse, Challenge);
            //}
        }

        private void SharePhotoToFacebook()
        {
            IFacebookService service = new IOSFacebookService();
            service.ShareFacebookChallenge(this, Challenge);
        }

        public void DidComplete(ISharing sharer, NSDictionary results)
        {
            SubmitResponse(new ChallengeResponseModel
            {
                ResponseCode = 1,
                ResponseMessage = "Congratulations!\r\nChallenge Completed!"
            });
        }

        public void DidFail(ISharing sharer, NSError error)
        {
            var alert = new UIAlertView("Sharing Error", error?.LocalizedDescription ?? "You need the native Facebook for iOS app installed for sharing images", this, "Ok");
            alert.Show();

            //var service = sharer as IOSFacebookService;
            //service.SendOpenGraph(this, Challenge);
        }

        public void DidCancel(ISharing sharer)
        {
        }
    }
}