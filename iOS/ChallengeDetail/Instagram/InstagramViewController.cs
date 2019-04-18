using CoreGraphics;
using FFImageLoading;
using Foundation;
using SocialLadder.Authentication;
using SocialLadder.iOS.Camera;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Delegates;
using SocialLadder.iOS.Services;
using SocialLadder.iOS.ViewControllers;
using SocialLadder.Models;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class InstagramViewController : CameraViewController, IInstagramAuthenticationDelegate
    {
        UIDocumentInteractionController DocumentController { get; set; }
        UILongPressGestureRecognizer tagLongPress;
        UITapGestureRecognizer tagTap;
        NSObject keyboardShowObj;
        NSObject keyboardHideObj;
        private IDisposable _webViewObserver;
        private nfloat _lastWebViewHeight;
        private CancellationTokenSource _webViewCancellationTokenSource;

        public InstagramViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Platform.ClearBrowserCache();

            SubmitButton.Hidden = false;

            CollectionViewDescription.Font = PointsText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenWidth * 0.035f);
            ChallengeText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenWidth * 0.065f);
            TimeText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenWidth * 0.042f);
            HashTitle.Font = UIFont.FromName("SFProText-Regular", SizeConstants.ScreenWidth * 0.029f);
            HashText.Font = UIFont.FromName("SFProText-Regular", SizeConstants.ScreenWidth * 0.058f);
            HashBottomText.Font = UIFont.FromName("SFProText-Regular", SizeConstants.ScreenWidth * 0.03f);
            HashtagCopiedText.Font = UIFont.FromName("SFProText-Regular", SizeConstants.ScreenWidth * 0.029f);

            HashText.ShouldReturn = (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };

            tagLongPress = new UILongPressGestureRecognizer(ShowCopiedView);
            HashText.AddGestureRecognizer(tagLongPress);
            tagTap = new UITapGestureRecognizer(ShowCopiedView);
            HashText.AddGestureRecognizer(tagTap);

            Reset();
            ChallengeText.Text = Challenge.Name;
            TimeText.Text = Challenge.NextEventCountDown;

            NSString viewportScriptString = (NSString)"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=500'); meta.setAttribute('initial-scale', '1.0'); meta.setAttribute('maximum-scale', '1.0'); meta.setAttribute('minimum-scale', '1.0'); meta.setAttribute('user-scalable', 'no'); document.getElementsByTagName('head')[0].appendChild(meta);";

            WebView.Configuration.UserContentController.AddUserScript(new WKUserScript(source: viewportScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true));
            WebView.ScrollView.ScrollEnabled = false;
            WebView.ScrollView.Bounces = false;
            WebView.AllowsBackForwardNavigationGestures = false;
            Platform.ClearBrowserCache();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            MainScroll.Scrolled += MainScroll_Scrolled;
            RegisterForKeyboardNotifications();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            MainScroll.Scrolled -= MainScroll_Scrolled;
            RemoveForKeyboardNotifications();
            _webViewCancellationTokenSource?.Cancel();
        }

        protected virtual void RegisterForKeyboardNotifications()
        {
            if (keyboardShowObj == null)
            {
                keyboardShowObj = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardDidShowNotification);
                keyboardHideObj = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardWillHideNotification);
            }
        }

        protected virtual void RemoveForKeyboardNotifications()
        {
            if (keyboardShowObj != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardShowObj);
                NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardHideObj);
                keyboardShowObj = null;
                keyboardHideObj = null;
            }
        }

        private void OnKeyboardDidShowNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
            {
                return;
            }

            var keyboardFrame = UIKeyboard.FrameEndFromNotification(notification);
            MainSrollBottomConstraint.Constant = -keyboardFrame.Height;
            View.UpdateConstraints();
        }

        private void OnKeyboardWillHideNotification(NSNotification notification)
        {
            if (!IsViewLoaded)
            {
                return;
            }

            MainSrollBottomConstraint.Constant = 0;
            View.UpdateConstraints();
        }

        private void MainScroll_Scrolled(object sender, EventArgs e)
        {
            ChallengeImage.RemoveConstraint(AspectHeight);
            var scrollView = (sender as UIScrollView);
            double offset = scrollView.ContentOffset.Y;
            nfloat newMultiplier = (nfloat)(ChallengeImage.Frame.Width + offset) / ChallengeImage.Frame.Width;
            AspectHeight = NSLayoutConstraint.Create(ChallengeImage, NSLayoutAttribute.Width, NSLayoutRelation.Equal, ChallengeImage, NSLayoutAttribute.Height, newMultiplier, 0);
            ChallengeImage.AddConstraint(AspectHeight);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            InstaCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            InstaCollectionView.RemoveConstraint(InstaCollectionViewAspect);
            InstaCollectionView.AddConstraint(ChallengesConstraints.ChallengesConstantHeightConstraint(InstaCollectionView, 0f));
            PointsImage.Image = null;
            PointsText.Text = string.Empty;
        }

        public override void Refresh(ChallengeResponseModel response)
        {
            base.Refresh(response);

            if (response == null)
                return;

            CheckStatus(response.Challenge);
            Challenge = response.Challenge;
            ChallengeText.Text = Challenge.Name;
            TimeText.Text = Challenge.NextEventCountDown;
            HashText.Text = Challenge.InstaCaption;

            var navigationDelegate = new ChallengeDetailWebViewNavigationDelegate();
            navigationDelegate.NavigationFinished += SetupConstraint;
            this.WebView.NavigationDelegate = navigationDelegate;

            WebView.LoadHtmlString(Challenge.Desc, null);
            ImageService.Instance.LoadUrl(Challenge.Image).Into(ChallengeImage);

            HashText.Hidden = false;
            HashBottomText.Hidden = false;
            HashText.Hidden = false;
        }

        //private string ParseHTMLString()
        //{
        //    string pattern = @"font-family:\w{0,}\s{0,}\w{0,};font-size:\d{0,2}";
        //    string replacement = string.Format("font-family:SFProText-Regular;font-size:{0}", Math.Ceiling(SizeConstants.ScreenWidth * 0.058f));
        //    string result = Regex.Replace(Challenge.Desc, pattern, replacement);
        //    return result;
        //}

        private void CheckStatus(ChallengeModel response)
        {
            if (response.Status == "Active")
            {
                return;
            }
            if (response.Status == "Pending")
            {
                SubmitButton.Hidden = true;
                return;
            }
            if (response.Status == "Complete")
            {
                SubmitButton.Hidden = true;
                return;
            }
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            tagLongPress.Dispose();
            tagTap.Dispose();
            _webViewObserver?.Dispose();
        }

        private void ShowCopiedView()
        {
            NSTimer timer;
            TagCopiedView.Hidden = false;

            //Some action
            UIPasteboard.General.String = HashText.Text;

            timer = NSTimer.CreateTimer(2, delegate
            {
                TagCopiedView.Hidden = true;
            });
            NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Common);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            TagCopiedView.Layer.CornerRadius = SizeConstants.ScreenWidth * 0.03f;
            TagCopiedView.Layer.ShadowColor = UIColor.Black.CGColor;
            TagCopiedView.Layer.ShadowOffset = new CoreGraphics.CGSize(0f, SizeConstants.ScreenWidth * 0.02f);
            TagCopiedView.Layer.ShadowOpacity = 0.5f;
            TagCopiedView.Layer.ShadowRadius = SizeConstants.ScreenWidth * 0.02f;

            HashText.Layer.BorderWidth = 2f;
            HashText.Layer.BorderColor = UIColor.FromRGB(245, 245, 245).CGColor;
            HashText.Layer.CornerRadius = SizeConstants.ScreenWidth * 0.02f;
        }

        public override void SubmitChallenge(UIButton button)
        {
            base.SubmitChallenge(button);
            Challenge.Status = "Pending";
            SL.Manager.UpdateChallenge(Challenge, Challenge.ID, UpdateChallengeComplete);
        }

        void UpdateChallengeComplete(bool didSucceed)
        {
            //ChallengeResponseModel response = new ChallengeResponseModel();
            //response.ResponseCode = didSucceed ? 1 : 0;
            //response = null;
            SubmitChallengeComplete(SubmitButton, null);
            NavigationController.PopViewController(true);
        }

        partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            SL.RefreshProfile();
            if (!SL.IsNetworkConnected("Instagram"))
            {
                UIStoryboard board = UIStoryboard.FromName("Web", null);
                WebViewController ctrl = (WebViewController)board.InstantiateViewController("WebViewController");
                const string ClientID = "cf88ac6682e24ffe83441b6950e3134a";
                const string RedirectUrl = "http://socialladderapp.com";
                ctrl.Url = "https://api.instagram.com/oauth/authorize/?client_id=" + ClientID + "&redirect_uri=" + RedirectUrl + "&response_type=token";
                ctrl.InstagramDelegate = this;
                this.PresentViewController(ctrl, false, null);
            }
            if (SL.IsNetworkConnected("Instagram"))
            {
                NSUrl instagramURL = new NSUrl("instagram://");
                if (UIApplication.SharedApplication.CanOpenUrl(instagramURL))
                {
                    TakePicture();
                }
                else
                {
                    Platform.ShowAlert("Unable to find the Instagram App on your device. This challenge requires Instagram.", string.Empty);

                    /*
                    var overlay = Platform.AddOverlay();
                    if (overlay != null)
                    {
                        var background = Platform.AddOverlayBackground(overlay, UIColor.White);
                    }
                    */
                }
            }
        }

        async public Task OnInstagramAuthenticationCompleted(SocialNetworkModel network)
        {
            DismissViewController(true, null);

            Platform.ClearBrowserCache();
            var response = await SL.CheckInNetwork(network, Platform.Lat, Platform.Lon);

            if (response.ResponseCode > 0)
            {
                if (SL.HasAreas)
                {
                    SL.RefreshAll();

                    DidLoadChallengeData = false;
                    LoadChallengeData();
                }
            }
            else
            {
                await Platform.ShowAlert(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", "OK");
            }
        }

        public void OnInstagramAuthenticationCanceled()
        {
            DismissViewController(true, null);
        }

        public void OnInstagramAuthenticationFailed(string message, Exception exception)
        {
            DismissViewController(true, null);
        }

        void Reset()
        {
            ChallengeImage.Image = null;
            ChallengeText.Text = null;
            TimeText.Text = null;
            HashText.Text = null;
            HashText.Hidden = true;
            HashBottomText.Hidden = true;
        }

        void SendToInstagram(UIImage image)
        {
            NSUrl instagramURL = new NSUrl("instagram://");
            if (UIApplication.SharedApplication.CanOpenUrl(instagramURL))
            {
                UIImage imageToUse = PhotoService.Rotate(image, image.Orientation);
                NSString documentDirectory = (new NSString(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));
                string saveImagePath = documentDirectory.AppendPathComponent(new NSString(@"Image.ig"));
                NSData imageData = PhotoService.ResizeImage(imageToUse).AsPNG();
                imageData.Save(saveImagePath, true);
                NSUrl imageURL = NSUrl.FromFilename(saveImagePath);

                DocumentController = new UIDocumentInteractionController();
                DocumentController.Url = imageURL;
                DocumentController.Delegate = new DocumentInteractionControllerDelegate(this);
                DocumentController.Uti = "com.instagram.photo";
                DocumentController.Annotation = new NSMutableDictionary<NSString, NSString> { { new NSString("InstagramCaption"), new NSString(Challenge.InstaCaption ?? "") } };
                DocumentController.PresentOpenInMenu(new RectangleF(1, 1, 1, 1), View, true);
            }
        }

        public override void OnTakePictureComplete(CameraPicture picture = null)
        {
            base.OnTakePictureComplete(picture);
            //SubmitChallenge(SubmitButton);
            if (picture == null)
            {
                SendToInstagram(ChallengeImage.Image);
                return;
            }
            ChallengeImage.Image = picture.Image;
            SendToInstagram(picture.Image);
        }

        public void SendToInstagramComplete()
        {
            SubmitChallenge(SubmitButton);
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
            scrollContentSize.Height = this.WebView.Frame.Top + (HashBottomText.Frame.Bottom - WebView.Frame.Bottom) + actualHeight;
            MainScroll.ContentSize = scrollContentSize;

            View.LayoutIfNeeded();
        }
    }


}