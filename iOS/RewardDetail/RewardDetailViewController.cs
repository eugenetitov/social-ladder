using FFImageLoading;
using SocialLadder.Models;
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;
using SocialLadder.iOS.Base;
using SocialLadder.iOS.CurrentConstants;
using Foundation;
using System.Text;
using WebKit;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SocialLadder.Logger;
using SocialLadder.iOS.Constraints;

namespace SocialLadder.iOS.Rewards
{
    public partial class RewardDetailViewController : SubmitViewController
    {
        private nfloat _backgroundImageAspectRatio = 240f / 414f;
        private nfloat _backgroundImageHeight;
        //private string HtmlMock = "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\"><html><head><style>body {background-color: #FFFF00FF;}</style><META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"></head><body><div><p style=\"color:#555555;font-family:Helvetica Neue;font-size:96;padding-left:5px;padding-top:5px;\"><span style=\"color:red;font-size:192;\"><b>Note - your email address is not approved, please visit the questions section of the app to confirm your email</b></span><br/><br/>You clearly know everyone in town!  So 14th sep subcategory and SocialLadder have teamed up to hook you up with t shirt. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut pulvinar orci non elit ultricies condimentum. Suspendisse rhoncus ut dui ac vulputate. Fusce pharetra pharetra velit a euismod. In quis nisi lobortis enim consectetur placerat eu ac risus. Sed aliquet egestas mauris vitae auctor. Phasellus suscipit velit sed sagittis ultrices. Phasellus sit amet volutpat odio, vel vehicula lorem. Proin rhoncus maximus nibh eu accumsan. Pellentesque id consequat lorem. Phasellus vitae commodo nunc. Ut malesuada, est sit amet tempor laoreet, tellus nibh posuere ante, placerat volutpat tellus metus in diam. Sed ornare sodales magna, quis dapibus erat placerat ut. Duis tristique euismod nulla at lobortis. Curabitur consequat, dolor congue fringilla elementum, arcu enim fringilla magna, at malesuada libero tortor in lectus. Donec convallis urna ligula, hendrerit pulvinar massa fringilla at. Mauris non neque id neque molestie ullamcorper. Mauris lacinia blandit pretium. Maecenas aliquam, nulla vitae feugiat ultrices, sem tellus fermentum neque, sollicitudin posuere arcu mi iaculis erat. Phasellus pellentesque tincidunt dui vel ultrices. Sed eu tincidunt nisl. Proin dignissim aliquet commodo. Curabitur interdum, turpis tristique mattis molestie, massa purus rutrum nunc, id iaculis tellus sapien vehicula metus. Nunc feugiat felis sem, vitae aliquam neque ultricies sed. Curabitur convallis velit scelerisque nisl tincidunt luctus in quis lacus. Donec finibus purus lobortis hendrerit dignissim. Aliquam egestas augue arcu, et varius nisl condimentum vel. Sed in mollis dui. Nulla sed purus quis odio molestie egestas. Donec eget mi at sem pellentesque commodo in vel nibh. Sed tristique tellus erat, ac pellentesque turpis efficitur et. Nulla id nisl sed risus aliquet iaculis sed at erat. Fusce nec lacus vel ante pharetra vehicula. Maecenas scelerisque dictum tortor. Nulla odio est, aliquet quis nunc sed, tincidunt lacinia dui. Nam placerat magna ornare tincidunt ultrices. Aenean malesuada lacus neque, sit amet fringilla ante luctus vitae. Fusce ac bibendum quam. Etiam nulla ex, euismod nec dapibus id, pharetra a lorem. Nunc placerat maximus ipsum sed dictum. Donec sit amet tincidunt erat. Pellentesque et tristique mi. Mauris risus magna, fringilla in augue at, ultrices fringilla nisi. Sed nec aliquam mi. Donec aliquam ex et viverra finibus. Morbi id luctus lectus. In non ornare diam, nec tristique est. Mauris egestas nisl nec quam ultrices pharetra. Mauris nec vestibulum metus. Aenean et tellus risus. Etiam lobortis rhoncus lacinia. Vivamus at urna sed metus ultricies sodales. Sed at vehicula nunc. Pellentesque tempor vulputate diam, quis dapibus sapien aliquet vitae. Pellentesque consequat vulputate porttitor. Cras vitae mattis risus. <br/> <br/>test desc for 14th sep subcategory</p></div></body></html>";
        private string _submitButtonTitle;
        private string _submitButtonSubtitle;
        private bool _isSubmitButtonLocked;
        private string _rewardDescriptionNormalized;

        private UIImage _submitButtonBakgroundImage;
        public bool _isDisappeared;

        public int Taps { get; set; }
        public RewardItemModel Reward { get; set; }
        public string DrillDownUrl { get; set; }
        public string Subtitle
        {
            get; set;
        }
        private NSTimer Timer { get; set; }
        private IDisposable _webViewObserver;
        private nfloat _lastWebViewHeight;
        private CancellationTokenSource _webViewCancellationTokenSource;

        #region lifecycle
        public RewardDetailViewController(IntPtr handle) : base(handle)
        {
            _backgroundImageHeight = _backgroundImageAspectRatio * SizeConstants.ScreenWidth;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupWebViewContent();
            AdjustHtmlContentFontSize();

            cnsBackgroundImageHeight.Constant = _backgroundImageHeight;
            cnsBackgroundImageWidth.Constant = SizeConstants.ScreenWidth;
            ivGenericBackground.UpdateConstraints();

            lblPointsButtonText.Text = Reward.MinScore + " pts";
            lblStatus.Text = " ";

            NotifyButton.Layer.BorderColor = new CGColor(255, 255, 255, 255);
            NotifyButton.Layer.CornerRadius = 5;
            NotifyButton.Layer.BorderWidth = 1.0f;
            NotifyButton.ContentEdgeInsets = new UIEdgeInsets(3, 2, 0, 0);
            NotifyButton.TitleEdgeInsets = new UIEdgeInsets(3, 2, 0, 0);

            peopleIcon.Image = UIImage.FromBundle("people_pink");
            statusTitleLabel.Text = null;

            WebView.ScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);
            WebView.ScrollView.DidChangeAdjustedContentInset += ScrollView_DidChangeAdjustedContentInset;
            Platform.ClearBrowserCache();

            SetupFonts();
            Refresh();

        }

        private void ScrollView_DidChangeAdjustedContentInset(object sender, EventArgs e)
        {

        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            scrlMainContent.Scrolled += MainScroll_Scrolled;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            scrlMainContent.Scrolled -= MainScroll_Scrolled;
            _webViewCancellationTokenSource?.Cancel();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            StopTimer();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            _webViewObserver?.Dispose();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                SetupConstraint();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

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

            if (_webViewCancellationTokenSource == null && !_isDisappeared)
            {
                _webViewCancellationTokenSource = new CancellationTokenSource();
                CheckReloadWebView(_webViewCancellationTokenSource.Token, WebView);
            }

            nfloat actualHeight = WebView.ScrollView.ContentSize.Height;// < 3 ? 1000 : WebView.ScrollView.ContentSize.Height;

            cnsDescriptionHeight.Constant = actualHeight;
            WebView.UpdateConstraints();

            CGSize contentSize = scrlMainContent.ContentSize;
            contentSize.Height = WebView.Frame.Top + actualHeight + lblDescriptionFooter.Frame.Height;
            scrlMainContent.ContentSize = contentSize;
            View.UpdateConstraints();
            WebView.ScrollView.ScrollRectToVisible(new CGRect(new CGPoint(0, -1), WebView.Frame.Size), true);
            View.LayoutIfNeeded();
        }

        private async void CheckReloadWebView(CancellationToken token, WKWebView webView)
        {
            try
            {
                await Task.Delay(5000, token);

                webView.EvaluateJavaScript("document.body.getBoundingClientRect().bottom", completionHandler: (height, error) =>
                {
                    if (!_isDisappeared && (error != null || String.IsNullOrWhiteSpace(height.Description) || int.Parse(height.Description) < 4))
                    {
                        Platform.ClearBrowserCache();
                        webView.LoadHtmlString(Reward.Description, null);
                    }
                });
            }
            catch (Exception) { }
        }

        #region override
        public override void Submit(UIWindow window, UIButton button)
        {
            SaveRewardButtonState();
            base.Submit(window, button);
            SL.Manager.PostCommitReward(Reward.ID, RewardCommitResponse);
            LogHelper.LogRewardSubmition(Reward.ID.ToString(), Reward.Name);
            //RewardCommitResponse(new RewardResponseModel() { ResponseCode = 0, ResponseMessage = "test" });
        }

        public override void SubmitComplete(UIButton button, ResponseModel response)
        {
            base.SubmitComplete(button, response);
        }
        #endregion

        #region events
        private void MainScroll_Scrolled(object sender, EventArgs e)
        {
            var scrollView = (sender as UIScrollView);
            double offset = scrollView.ContentOffset.Y;

            nfloat newHeight = _backgroundImageHeight - (nfloat)offset;

            cnsBackgroundImageHeight.Constant = newHeight;
            cnsBackgroundImageWidth.Constant = offset > 0 ? SizeConstants.ScreenWidth : newHeight / _backgroundImageAspectRatio;

            ivGenericBackground.UpdateConstraints();
            ivGenericBackground.LayoutIfNeeded();
        }

        async partial void RewardButton_TouchUpInside(UIButton sender)
        {
            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            lblTapsCount.Text = string.Format("{0} Taps", ++Taps);

            if (Reward.MinScore > SL.Profile.Score)
            {
                return;
            }

            if (Reward.AutoUnlockDate?.ToLocalTime() > DateTime.Now)//Reward.LockStatus
            {
                AddButtonTapRedAnimation(() =>
                {
                    return;
                });

                return;
            }

            //if (!Reward.ButtonLockStatus)
            //{
            if (!string.IsNullOrEmpty(Reward.ConfirmationQuestion))
            {
                nint button = await Platform.ShowAlert(null, Reward.ConfirmationQuestion, "OK", "Cancel");
                if (button == 0)
                    Submit(window, RewardButton);
            }
            else
            {
                Submit(window, RewardButton);
            }
            //}
            //else
            //{
            //    Submit(window, RewardButton);
            //}
        }
        #endregion

        #region methods
        private void SetupWebViewContent()
        {
            //WebView.NavigationDelegate = this;
            WebView.NavigationDelegate = new NavDelegate2(this);

            WebView.ScrollView.ContentInset = new UIEdgeInsets(0, 0, 0, 0);

            var width = UIScreen.MainScreen.Bounds.Width * cnsDescriptionWidth.Multiplier;
            //device-width
            NSString viewportScriptString = (NSString)$@"var meta = document.createElement('meta');
                meta.setAttribute('name', 'viewport');
                meta.setAttribute('content', 'width={width} * 1');
                meta.setAttribute('initial-scale', '1.0');
                meta.setAttribute('maximum-scale', '1.0');
                meta.setAttribute('minimum-scale', '1.0');
                meta.setAttribute('user-scalable', 'no');
                document.getElementsByTagName('head')[0].appendChild(meta);";
            NSString disableSelectionScriptString = (NSString)"document.documentElement.style.webkitUserSelect='none';";
            NSString disableCalloutScriptString = (NSString)"document.documentElement.style.webkitTouchCallout='none';";

            // 1 - Make user scripts for injection
            var viewportScript = new WKUserScript(source: viewportScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true);
            var disableSelectionScript = new WKUserScript(source: disableSelectionScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true);
            var disableCalloutScript = new WKUserScript(source: disableCalloutScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true);

            // 2 - Initialize a user content controller, add scripts
            // From docs: "provides a way for JavaScript to post messages and inject user scripts to a web view."
            var controller = WebView.Configuration.UserContentController;
            //var controller = new WKUserContentController();
            controller.AddUserScript(viewportScript);
            controller.AddUserScript(disableSelectionScript);
            controller.AddUserScript(disableCalloutScript);

            // 6 - Webview options 
            WebView.ScrollView.ScrollEnabled = true;               // Make sure our view is interactable
            WebView.ScrollView.Bounces = false;                    // Things like this should be handled in web code
            WebView.AllowsBackForwardNavigationGestures = false;   // Disable swiping to navigate
            WebView.ContentMode = UIViewContentMode.ScaleToFill;   // Scale the page to fill the web view
        }

        private void Refresh()
        {
            if (DrillDownUrl == string.Empty)
            {

            }
            ImageService.Instance.LoadUrl(Reward.MainImageURL).Into(ivGenericBackground);//RewardImage
            NameText.Text = Reward.Name;
            SubTitleText.Text = Subtitle;

            if (_rewardDescriptionNormalized != null)
                WebView.LoadHtmlString(_rewardDescriptionNormalized, null);//Reward.Description HtmlMock styledHTMLRaw

            UnitsAvailable.Text = Reward.RemainingUnits + " units available";

            //TO DO Change binding 
            //display hardcoded value
            UpdateRewardUnlockStatus();
        }

        private void UpdateRewardUnlockStatus()
        {
            RewardButton.ShowsTouchWhenHighlighted = true;
            CountdownText.Text = null;
            LockImage.Image = null;
            LockImageStatus.Image = null;
            statusTitleLabel.Text = "GO";
            if (Reward.MinScore > SL.Profile.Score)
            {
                RewardButton.SetBackgroundImage(UIImage.FromBundle("scoreButtonBackground1"), UIControlState.Normal);
                statusTitleLabel.Text = string.Empty;
            }
            else
            {
                RewardButton.SetBackgroundImage(UIImage.FromBundle("claim-btn_unlocked"), UIControlState.Normal);
            }
            if (Reward.AutoUnlockDate?.ToLocalTime() > DateTime.Now)
            {
                StartTimer();
                CountdownText.TextColor = UIColor.White;
                CountdownText.Hidden = false;
                CountdownText.TextAlignment = UITextAlignment.Left;
                UnlockInTitle.Hidden = false;
                //NotifyButton.Hidden = false;
                statusTitleLabel.Hidden = true;
                LockImageStatus.Image = UIImage.FromBundle("lock-icon");
                if (Reward.AutoUnlockDate?.ToLocalTime() > DateTime.Now)//Reward.ButtonLockStatus
                {
                    LockImage.Hidden = false;
                    lblClaimIt.Text = "Claim It";
                    LockImage.Image = UIImage.FromBundle("lock-icon");
                }
            }
            else
            {
                StopTimer();
                //NotifyButton.Hidden = true;
                statusTitleLabel.TextAlignment = UITextAlignment.Center;
                statusTitleLabel.Hidden = false;
                UnlockInTitle.Hidden = true;
                CountdownText.Hidden = true;
            }
            if (Reward.MinScore > SL.Profile.Score)
            {
                lblPointsButtonText.Hidden = true;
                lblStatus.Hidden = false;
                lblTapsCount.Hidden = true;
                lblClaimIt.TextColor = UIColor.Red;
                //Reward.MinRelScore;
                lblClaimIt.Text = ((float)Reward.MinScore - SL.Profile.Score).ToString();
                RewardButton.SetBackgroundImage(UIImage.FromBundle("scoreButtonBackground1"), UIControlState.Normal);
                return;
            }
            else
            {
                lblStatus.Hidden = false;
                statusTitleLabel.TextAlignment = UITextAlignment.Center;
                LockImage.Hidden = true;
                RewardButton.SetBackgroundImage(UIImage.FromBundle("claim-btn_unlocked"), UIControlState.Normal);
                lblClaimIt.TextColor = UIColor.White;
                lblClaimIt.Text = "Claim It";
            }
        }

        void StartTimer()
        {
            Timer = NSTimer.CreateScheduledTimer(1, true, UpdateTimer);
        }

        void StopTimer()
        {
            if (Timer != null)
            {
                Timer.Invalidate();
                Timer.Dispose();
                Timer = null;
            }
        }

        void UpdateTimer(NSTimer timer)
        {
            if (Reward.AutoUnlockDate?.ToLocalTime() < DateTime.Now)
            {
                UpdateRewardUnlockStatus();
                return;
            }
            var timeSpan = (Reward.AutoUnlockDate?.ToLocalTime() ?? DateTime.Now) - DateTime.Now;

            if (0 < timeSpan.TotalSeconds && timeSpan.TotalSeconds <= 10)
                CountdownText.TextColor = UIColor.Red;
            if (timeSpan != null)
            {
                CountdownText.Text = string.Format(timeSpan.Days > 0 ? "{0:dd}d:{0:hh}h:{0:mm}m" : "{0:hh}h:{0:mm}m:{0:ss}s", timeSpan);
            }
        }

        private void RewardCommitResponse(RewardResponseModel rewardResponse)
        {
            Action onSubmmitCompleted = (() =>
            {
                RestoreRewardButtonState();
            });

            if (rewardResponse != null)
            {
                if (rewardResponse.RewardAction == "UPDATE" && rewardResponse.UpdatedReward != null)
                {
                    this.Reward = rewardResponse.UpdatedReward;
                    Refresh();
                }

                if (rewardResponse.ResponseCode > 0 && string.IsNullOrEmpty(rewardResponse.ResponseMessage))
                {
                    AddButtonTapGreenAnimation(() =>
                    {
                        var overlay = Platform.AddOverlay(UIApplication.SharedApplication.KeyWindow.Frame, Colors.RewardsGreenAnimationCircleColor, true);
                        SubmitComplete(RewardButton, rewardResponse, onSubmmitCompleted);
                        RewardCompleteView rewardComplete = RewardCompleteView.Create();
                        rewardComplete.onViewClosed += RestoreRewardButtonState;
                        overlay.AddSubview(rewardComplete);
                        RewardButton.SetBackgroundImage(UIImage.FromBundle("claim-btn_success"), UIControlState.Normal);
                        lblClaimIt.Text = string.Empty;
                        lblPointsButtonText.Text = "Claimed";
                        rewardComplete.Update(overlay, rewardResponse, Reward, this);
                    });
                    return;
                }

                //if (Overlay != null)
                //{
                AddButtonTapRedAnimation(() =>
                {
                    var overlay = Platform.AddOverlay(UIApplication.SharedApplication.KeyWindow.Frame, Colors.RewardsRedAnimationCircleColor, true);
                    SubmitComplete(RewardButton, rewardResponse, onSubmmitCompleted);
                    RewardCompleteView rewardComplete = RewardCompleteView.Create();
                    rewardComplete.onViewClosed += RestoreRewardButtonState;
                    overlay.AddSubview(rewardComplete);
                    RewardButton.SetBackgroundImage(UIImage.FromBundle("claim-btn_fail"), UIControlState.Normal);
                    lblClaimIt.Text = string.Empty;
                    lblPointsButtonText.Text = "Didn't Get";
                    LockImage.Hidden = true;
                    rewardComplete.Update(overlay, rewardResponse, Reward, this);
                });
                //}
            }

        }

        private void AddButtonTapRedAnimation(Action onCompleted)
        {
            var circleView = new CircleView(RewardButton.Frame, Colors.RewardsRedAnimationCircleColor, UIScreen.MainScreen.Bounds.Height, 2.0f);
            circleView.OnCompleted += onCompleted;
            View.InsertSubviewBelow(circleView, RewardButton);
        }

        private void AddButtonTapGreenAnimation(Action onCompleted)
        {
            var circleView = new CircleView(RewardButton.Frame, Colors.RewardsRedAnimationCircleColor, UIScreen.MainScreen.Bounds.Height, 2.0f);
            circleView.OnCompleted += onCompleted;
            View.InsertSubviewBelow(circleView, RewardButton);
        }

        private void SaveRewardButtonState()
        {
            _submitButtonBakgroundImage = RewardButton.BackgroundImageForState(UIControlState.Normal);
            _isSubmitButtonLocked = !LockImage.Hidden;
            _submitButtonTitle = lblPointsButtonText.Text;
            _submitButtonSubtitle = lblClaimIt.Text;
        }

        private void RestoreRewardButtonState()
        {
            RewardButton.SetBackgroundImage(_submitButtonBakgroundImage, UIControlState.Normal);
            //LockImage.Hidden = !_isSubmitButtonLocked;
            lblPointsButtonText.Text = _submitButtonTitle;
            lblClaimIt.Text = _submitButtonSubtitle;
        }

        private void AdjustHtmlContentFontSize()
        {
            if (String.IsNullOrEmpty(Reward.Description))
            {
                return;
            }
            _rewardDescriptionNormalized = Reward.Description;

            int currentIndex = 0;
            bool isDigitsSegment = false;
            int afterDigitsIndex = 0;
            int digitsSegmentLength = 0;
            while (currentIndex < _rewardDescriptionNormalized.Length)
            {
                isDigitsSegment = false;
                digitsSegmentLength = 0;

                afterDigitsIndex = currentIndex = _rewardDescriptionNormalized.IndexOf("font-size:", currentIndex);
                if (afterDigitsIndex == -1)
                {
                    break;
                }
                while (_rewardDescriptionNormalized[afterDigitsIndex] != ';' || _rewardDescriptionNormalized[afterDigitsIndex] != '"')
                {
                    if (isDigitsSegment == true)
                    {
                        ++digitsSegmentLength;
                    }
                    if (Char.IsDigit(_rewardDescriptionNormalized[afterDigitsIndex]) && isDigitsSegment == false)
                    {
                        isDigitsSegment = true;
                    }
                    if (!Char.IsDigit(_rewardDescriptionNormalized[afterDigitsIndex]) && isDigitsSegment == true)
                    {
                        break;
                    }
                    ++afterDigitsIndex;
                }
                ++currentIndex;

                string stringValue = _rewardDescriptionNormalized.Substring(afterDigitsIndex - digitsSegmentLength, digitsSegmentLength);
                bool isParsed = Int32.TryParse(stringValue, out int intValue);
                if (isParsed)
                {
                    intValue = (int)Math.Round(SizeConstants.ScreenMultiplier * intValue);
                    string adjustedStringValue = intValue.ToString();

                    _rewardDescriptionNormalized = _rewardDescriptionNormalized.Remove(afterDigitsIndex - digitsSegmentLength, digitsSegmentLength);
                    _rewardDescriptionNormalized = _rewardDescriptionNormalized.Insert(afterDigitsIndex - digitsSegmentLength, adjustedStringValue);
                    afterDigitsIndex += adjustedStringValue.Length - stringValue.Length;
                }



                if (isDigitsSegment == true && String.Equals(_rewardDescriptionNormalized.Substring(afterDigitsIndex, 2), "px"))
                {
                    continue;
                }
                if (isDigitsSegment == true && String.Equals(_rewardDescriptionNormalized.Substring(afterDigitsIndex, 2), "pt"))
                {
                    _rewardDescriptionNormalized.Remove(afterDigitsIndex, 2);
                    _rewardDescriptionNormalized = _rewardDescriptionNormalized.Insert(afterDigitsIndex, "px");
                    continue;
                }
                if (isDigitsSegment == true && !String.Equals(_rewardDescriptionNormalized.Substring(afterDigitsIndex, 2), "px"))
                {
                    _rewardDescriptionNormalized = _rewardDescriptionNormalized.Insert(afterDigitsIndex, "px");
                }
            }
        }

        private void SetupFonts()
        {
            nfloat width = UIScreen.MainScreen.Bounds.Width;
            UnlockInTitle.Font = UIFont.FromName("ProximaNova-Regular", (nfloat)(width * 0.036));
            lblStatus.Font = UIFont.FromName("ProximaNova-Regular", (nfloat)(width * 0.038));
            SubTitleText.Font = UIFont.FromName("ProximaNova-Regular", (nfloat)(width * 0.039));
            UnitsAvailable.Font = UIFont.FromName("ProximaNova-Regular", (nfloat)(width * 0.038));
            UsersEligible.Font = UIFont.FromName("ProximaNova-Regular", (nfloat)(width * 0.038));
            lblTapsCount.Font = UIFont.FromName("ProximaNova-Regular", (nfloat)(width * 0.034));
        }
        #endregion

        public class NavDelegate2 : WKNavigationDelegate
        {
            RewardDetailViewController rewardDetailViewController { get; set; }

            public NavDelegate2(RewardDetailViewController viewController)
            {
                rewardDetailViewController = viewController;
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
            }
        }
    }
}