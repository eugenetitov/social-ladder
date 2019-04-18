using Foundation;
using System;
using UIKit;
using SocialLadder.Models;
using FFImageLoading;
using SocialLadder.iOS.Constraints;
using WebKit;
using CoreGraphics;
using SocialLadder.Interfaces;
using static SocialLadder.iOS.PlatformServices.SmsService;
using ContactsUI;
using MessageUI;
using System.Threading.Tasks;
using Facebook.LoginKit;
using SocialLadder.iOS.Services;
using SocialLadder.Logger;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using CoreAnimation;
using System.Collections.Generic;
using MapKit;
using CoreLocation;
using SocialLadder.iOS.Delegates;
using CrashlyticsKit;
using System.Threading;

namespace SocialLadder.iOS.Challenges
{
    public partial class ChallengeDetailViewController : ChallengeDetailBaseViewController, IUINavigationControllerDelegate, IUIAlertViewDelegate, IMKMapViewDelegate
    {
        bool DidSetupMap { get; set; }
        public WKWebView WebView1;
        private ShareTemplateModel _shareTemplateModel;
        private ISmsService _messageService;
        private IDisposable _webViewObserver;
        private nfloat _lastWebViewHeight;
        private CancellationTokenSource _webViewCancellationTokenSource;

        public ChallengeDetailViewController(IntPtr handle) : base(handle)
        {
        }

        public override void LoadView()
        {
            base.LoadView();
        }

        public override void ViewDidLoad()
        {

            Crashlytics.Instance.Log("Challenges_ChallengeDetailViewController_ViewDidLoad()");
           
            base.ViewDidLoad();

            Reset();
            SetupFonts();
            _messageService = new PlatformServices.SmsService();
            ImageService.Instance.LoadUrl(Challenge.Image).Into(ChallengeImage);
            ChallengeTextLbl.Text = Challenge.Name;
            TimeText.Text = Challenge.NextEventCountDown;

            if (Challenge.TypeCode == "SHARE")
                SubmitButton.SetBackgroundImage(UIImage.FromBundle("challenge-btn_share"), UIControlState.Normal);
            else if (Challenge.TypeCode == "INVITE")
                SubmitButton.SetBackgroundImage(UIImage.FromBundle("challenge-btn_itb"), UIControlState.Normal);
            else if (new List<string> { "POSTERING", "COLLATERAL TRACKING", "FLYERING", "MANUAL" }.Contains(Challenge.TypeCode))//else if (Challenge.TypeCode == "COLLATERAL TRACKING" && Challenge.TypeCodeDisplayName == "Postering")
            {
                SubmitButton.SetBackgroundImage(UIImage.FromBundle("challenge-btn_photo"), UIControlState.Normal);
                SetupCollateral();
            }
            else if (Challenge.TypeCode == "FB ENGAGEMENT")
                SubmitButton.SetBackgroundImage(UIImage.FromBundle("challenge-btn_fb"), UIControlState.Normal);

            DetailCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            DetailCollectionView.RemoveConstraint(DetailCollectionViewAspect);
            DetailCollectionView.AddConstraint(ChallengesConstraints.ChallengesConstantHeightConstraint(DetailCollectionView, 0f));

            NSString viewportScriptString = (NSString)"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=500'); meta.setAttribute('initial-scale', '1.0'); meta.setAttribute('maximum-scale', '1.0'); meta.setAttribute('minimum-scale', '1.0'); meta.setAttribute('user-scalable', 'no'); document.getElementsByTagName('head')[0].appendChild(meta);";

            WebView.Configuration.UserContentController.AddUserScript(new WKUserScript(source: viewportScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true));

            WebView.ScrollView.ScrollEnabled = false;
            WebView.ScrollView.Bounces = false;
            WebView.AllowsBackForwardNavigationGestures = false;
            Platform.ClearBrowserCache();
        }

        UIViewController LoadChallengeDetail(string storyboardID)
        {
            UIStoryboard board = UIStoryboard.FromName("Challenges", null);
            ChallengeDetailBaseViewController controller = (ChallengeDetailBaseViewController)board.InstantiateViewController(storyboardID);
            controller.Challenge = Challenge;
            return controller;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ScrollView.Scrolled += MainScroll_Scrolled;
            //RemoveOverlay();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            if (new List<string> { "POSTERING", "COLLATERAL TRACKING", "FLYERING", "MANUAL" }.Contains(Challenge.TypeCode))
            {
                SetupCollateral();
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ScrollView.Scrolled -= MainScroll_Scrolled;
            _webViewCancellationTokenSource?.Cancel();
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();
            _webViewObserver?.Dispose();
        }

        private void MainScroll_Scrolled(object sender, EventArgs e)
        {
            MapViewBase.RemoveConstraint(cnsMapTop1);
            ScrollView.RemoveConstraint(cnsMapTop1);
            ScrollView.AddConstraint(cnsMapTop1 = NSLayoutConstraint.Create(MapViewBase, NSLayoutAttribute.Top, NSLayoutRelation.Equal, ScrollView, NSLayoutAttribute.Top, 1, ScrollView.ContentOffset.Y));

            ChallengeImage.RemoveConstraint(AspectHeight);
            var scrollView = (sender as UIScrollView);
            double offset = scrollView.ContentOffset.Y;
            nfloat newMultiplier = (nfloat)(ChallengeImage.Frame.Width + offset) / ChallengeImage.Frame.Width * 1.709f;
            AspectHeight = NSLayoutConstraint.Create(ChallengeImage, NSLayoutAttribute.Width, NSLayoutRelation.Equal, ChallengeImage, NSLayoutAttribute.Height, newMultiplier, 0);
            ChallengeImage.AddConstraint(AspectHeight);
        }

        void Reset()
        {
            TimeText.Text = null;
            PointsText.Text = null;
            ChallengeTextLbl.Text = null;
            ChallengeImage.Image = null;
            //ChallengeDescription.Text = null;
        }

        public override void Refresh(ChallengeResponseModel challengeResponse)
        {
            Crashlytics.Instance.Log("ChallengeDetailViewController_Refresh()");
            base.Refresh(challengeResponse);

            if (challengeResponse == null)
                return;

            Challenge = challengeResponse.Challenge;
            TimeText.Text = Challenge.NextEventCountDown;
            PointsText.Text = "+" + Challenge.PointValue.ToString() + " pts";
            ChallengeTextLbl.Text = Challenge.Name;

            var navigationDelegate = new ChallengeDetailWebViewNavigationDelegate();
            navigationDelegate.NavigationFinished += SetupConstraint;
            this.WebView.NavigationDelegate = navigationDelegate;
            WebView.LoadHtmlString(Challenge.Desc, null);
            ImageService.Instance.LoadUrl(Challenge.Image).Into(ChallengeImage);

            if (!DidSetupMap && Challenge.LocationLat != null && Challenge.LocationLong != null)
            {
                ChallengeImage.Hidden = true;
                vImagePlaceholder.RemoveConstraint(cnImagePlaceholderAspect);
                vImagePlaceholder.AddConstraint(cnImagePlaceholderAspect = NSLayoutConstraint.Create(vImagePlaceholder, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 0));

                double radius = Challenge.RadiusMeters ?? 100.0;
                if (radius > 6000000)
                {
                    radius = 6000000;
                }
                double mapRegion = radius * 2.5;

                CLLocationCoordinate2D mapCoordinate = new CLLocationCoordinate2D(Challenge.LocationLat.Value, Challenge.LocationLong.Value);
                MapViewBase.SetRegion(MKCoordinateRegion.FromDistance(mapCoordinate, mapRegion, mapRegion), true);

                MKCircle circle = MKCircle.Circle(mapCoordinate, radius);
                MapViewBase.AddOverlay(circle);

                MKPointAnnotation annotation = new MKPointAnnotation();
                annotation.Coordinate = new CLLocationCoordinate2D(Challenge.LocationLat.Value, Challenge.LocationLong.Value);
                MapViewBase.AddAnnotation(annotation);

                DidSetupMap = true;
            }
            else
            {
                MapViewBase.Hidden = true;
                paddingMap.RemoveConstraint(cnMapPlaceholderAspect);
                paddingMap.AddConstraint(cnMapPlaceholderAspect = NSLayoutConstraint.Create(paddingMap, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 0));
            }

            CheckStatus();
        }

        private void CheckStatus()
        {
            var details = Challenge.ChallengeDetailsURL;

            if (Challenge.Status == "Pending" || Challenge.Status == "Complete")
            {
                SubmitButton.Hidden = true;
                if (new List<string> { "POSTERING", "COLLATERAL TRACKING", "FLYERING", "MANUAL" }.Contains(Challenge.TypeCode))
                {
                    CollateralDisplayCountIfNeed();
                }
                return;
            }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
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

            CGSize scrollContentSize = ScrollView.ContentSize;
            scrollContentSize.Height = this.WebView.Frame.Top + actualHeight;
            ScrollView.ContentSize = scrollContentSize;

            View.LayoutIfNeeded();
        }

        private bool LoadChallengeDetail()
        {
            UIViewController controller = null;
            if (Challenge.TypeCode == "SHARE")
            {
                controller = LoadChallengeDetail("ShareViewController");
                ShareViewController shareViewController = controller as ShareViewController;
                shareViewController.SubmitButton = SubmitButton;
            }
            else if (Challenge.TypeCode == "INVITE")
            {
                SL.Manager.RefreshShareTemplate(Challenge.ShareTemplateURL, SendInviteMessage);
            }
            else if (Challenge.TypeCode == "POSTERING")//else if (Challenge.TypeCode == "COLLATERAL TRACKING" && Challenge.TypeCodeDisplayName == "Postering")
                controller = LoadChallengeDetail("PosteringViewController");
            else if (Challenge.TypeCode == "COLLATERAL TRACKING")
                //controller = LoadChallengeDetail("CollateralViewController");
                controller = LoadChallengeDetail("PosteringViewController");//display always Postering
            else if (Challenge.TypeCode == "FB ENGAGEMENT")
            {//controller = LoadChallengeDetail("FacebookEngagementViewController");
                var permissions = new string[] { "user_posts" };
                var loginManager = new LoginManager();

                if (Facebook.CoreKit.AccessToken.CurrentAccessToken != null)
                {
                    var graphRequest = new Facebook.CoreKit.GraphRequest("me/permissions", null);
                    graphRequest.Start(new Facebook.CoreKit.GraphRequestHandler(
                        (Facebook.CoreKit.GraphRequestConnection connection, NSObject result, NSError error) =>
                        {
                            bool hasPermission = false;
                            if (error == null)
                            {
                                NSArray permissions1 = (NSArray)result.ValueForKey(new NSString("data"));
                                foreach (NSDictionary dict in NSArray.FromArray<NSObject>(permissions1))
                                {
                                    if (dict.ValueForKey(new NSString("permission")).Description == permissions[0])
                                    {
                                        if (dict.ValueForKey(new NSString("status")).Description == "granted")
                                        {
                                            hasPermission = true;
                                        }
                                    }
                                }
                            }
                            if (hasPermission)
                            {
                                if (Challenge?.IsReshareReq ?? false)
                                {
                                    CheckFacebookSharingEnabled(permissions);
                                }
                                else
                                {
                                    SubmitEngagement();
                                }
                            }
                        }
                    ));
                }
                else
                {
                    loginManager.LogInWithReadPermissions(permissions, this,
                    (LoginManagerLoginResult result, NSError error) =>
                    {
                        LogHelper.LogUserMessage("FB_USER_POST", "asking for permissions");
                        if (result?.GrantedPermissions != null && result.GrantedPermissions.Contains(permissions[0]))
                        {
                            LogHelper.LogUserMessage("FB_USER_POST", "permission failed");
                            if (Challenge?.IsReshareReq ?? false)
                            {
                                CheckFacebookSharingEnabled(permissions);
                            }
                            else
                                SubmitEngagement();
                        }
                        else
                        {
                            LogHelper.LogUserMessage("FB_USER_POST", "permission failed");
                            new UIAlertView(null, "There was a problem getting permission for " + permissions[0], this, "Ok").Show();
                        }
                    });
                }
            }

            if (controller != null)
                NavigationController.PushViewController(controller, true);
            return controller != null;
        }

        private void CheckFacebookSharingEnabled(string[] permissions) =>
            new IOSFacebookService().VerifyPermissions(permissions, OnFacebookFeedPermissionSuccess, OnFacebookFeedPermissionFailed);

        public void OnFacebookFeedPermissionSuccess() =>
            SubmitEngagement();

        public void OnFacebookFeedPermissionFailed() =>
            new UIAlertView(null, "Go into your iOS Settigs>Facebook and make sure SocialLadder has permissions.", this, "Ok").Show();

        private void SubmitEngagement() =>
            SL.Manager.PostSubmitEngagement(Challenge.ID, PostEngagementCompleteResponse);

        public void PostEngagementCompleteResponse(ShareResponseModel shareResponse) =>
            SubmitChallengeComplete(SubmitButton, shareResponse);

        private void SendInviteMessage(ShareResponseModel model)
        {
            if (model?.ShareTemplate == null)
                return;

            _shareTemplateModel = model.ShareTemplate;

            var alertController = UIAlertController.Create("", "What kind of message?", UIAlertControllerStyle.ActionSheet);


            // Add Actions
            alertController.AddAction(UIAlertAction.Create("WhatsApp", UIAlertActionStyle.Default, SendInitationViaWatsApp));
            alertController.AddAction(UIAlertAction.Create("SMS", UIAlertActionStyle.Default, SendInvitationViaSms));
            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel sending message")));

            // Show the alert
            NavigationController.PresentViewController(alertController, true, null);
        }

        private void SendInvitationViaSms(UIAlertAction handler)
        {
            CheckPermission(new Action(() =>
            {
                if (MFMessageComposeViewController.CanSendText)
                {
                    UIStoryboard board = UIStoryboard.FromName("Challenges", null);
                    UIViewController controller = (UIViewController)board.InstantiateViewController("ContactViewController");
                    this.NavigationController.PushViewController(controller, true);

                    var smsController = new MFMessageComposeViewController { Body = $"{_shareTemplateModel.InviteText} {_shareTemplateModel.ActionLink}" };
                    var pickerDelegate = new ContactPickerDelegate();

                    smsController.Finished += (sender, e) =>
                    {
                        NSRunLoop.Main.BeginInvokeOnMainThread(() =>
                        {
                            e.Controller.DismissViewController(true, null);
                        });
                    };

                    (controller as ContactViewController).SelectContacts += ((string[] contactsArr) =>
                    {
                        smsController.Recipients = contactsArr;
                        NSRunLoop.Main.BeginInvokeOnMainThread(() =>
                        {
                            this.PresentViewController(smsController, true, null);
                        });
                    });
                }
            }));
        }

        private async void CheckPermission(Action compledted)
        {
            var ContactsPermissions = await HasContactsPermission();
            if (!ContactsPermissions)
            {

                InvokeOnMainThread(() =>
                {
                    var alertController = UIAlertController.Create("Access to contacts is disable", "Please, go to settings and turn on access to contacts for this app", UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Cancel, alert => { }));
                    this.NavigationController.PresentViewController(alertController, true, null);
                });
                return;
            }

            compledted();
        }

        private void SendInitationViaWatsApp(UIAlertAction handler)
        {
            bool result = _messageService.SendSmsToWatsApp(string.Empty, $"{_shareTemplateModel.InviteText} {_shareTemplateModel.ActionLink}");
            if (!result)
            {

                UIAlertView alert = new UIAlertView()
                {
                    //Title = "alert title",
                    Message = "Unable to find the WhatsApp on your device. This challenge requires WhatsApp."
                };
                alert.AddButton("OK");
                alert.Show();
            }
        }

        partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }
            LoadChallengeDetail();
        }

        private async Task<bool> HasContactsPermission()
        {
            try
            {
                // Check permission
                var hasCameraPermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts);

                // Ask for permissions
                if (hasCameraPermission != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts); // Only shows prompt once
                    hasCameraPermission = results[Permission.Contacts];
                }

                return Equals(hasCameraPermission, PermissionStatus.Granted);
            }
            catch (Exception ex)
            {
                Console.WriteLine("              " + ex.Message);
            }
            return false;
        }

        private void SetupCollateral()
        {
            MapViewBase.Delegate = this;
            vImagesCount.TranslatesAutoresizingMaskIntoConstraints = false;

            CollateralDisplayCountIfNeed();
        }

        private void CollateralDisplayCountIfNeed()
        {
            if ((Challenge.TargetCount ?? 0) == 1 || Challenge.Status == "Pending" || Challenge.Status == "Complete")
            {
                vImagesCount.Hidden = true;
                cnImagesCountHeight.Constant = 0;
                cnMarginCount.Constant = 0;
            }
            else
            {
                vImagesCount.Hidden = false;
                vImagesCount.ClipsToBounds = false;

                cnImagesCountHeight.Constant = SizeConstants.ScreenWidth / 13;
                cnMarginCount.Constant = SizeConstants.ScreenWidth / 15;

                vImagesCount.Layer.BorderColor = UIColor.FromRGB(238, 238, 238).CGColor;
                vImagesCount.Layer.BorderWidth = 1;
                vImagesCount.Layer.CornerRadius = 2;

                UploadedCountText1.Text = (Challenge.CompletedCount ?? 0).ToString();
                NeedUploadCountText1.Text = "/ " + ((Challenge.TargetCount ?? 0) == 0 ? "∞" : Challenge.TargetCount.ToString());
            }
        }

        private void SetupFonts()
        {
            TimeText.Font = CountChallengeLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.038f);
            ChallengeTextLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.060f);
            NeedUploadCountText1.Font = UIFont.FromName("SFProText-Bold", SizeConstants.ScreenMultiplier * 18);
            UploadedCountText1.Font = UIFont.FromName("SFProText-Bold", SizeConstants.ScreenMultiplier * 18);
        }

        [Export("mapView:viewForAnnotation:")]
        public MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = new MKAnnotationView(annotation, "pin");
            annotationView.Image = UIImage.FromBundle("location-icon_on");
            annotationView.CanShowCallout = true;
            return annotationView;
        }

        [Export("mapView:rendererForOverlay:")]
        public MKOverlayRenderer OverlayRenderer(MKMapView mapView, IMKOverlay overlay)
        {
            MKCircleRenderer circleRenderer = new MKCircleRenderer((MKCircle)overlay);
            circleRenderer.FillColor = UIColor.Blue;
            circleRenderer.Alpha = 0.1f;
            return circleRenderer;
        }
    }


}