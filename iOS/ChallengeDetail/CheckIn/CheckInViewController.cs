using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Delegates;
using SocialLadder.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using WebKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class CheckInViewController : ChallengeDetailBaseViewController, IMKMapViewDelegate
    {
        //UIView Overlay { get; set; }
        bool DidSetupMap { get; set; }
        public CheckInViewController(IntPtr handle) : base(handle)
        {
        }
        private const float METERS_PER_MILE = 1609.344F;
        private IDisposable _webViewObserver;
        private nfloat _lastWebViewHeight;
        private CancellationTokenSource _webViewCancellationTokenSource;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Reset();
            HeaderLbl.Text = Challenge.Name;
            TimeDisLbl.Text = Challenge.NextEventCountDown;

            MapView.Delegate = this;

            //Refresh(Challenge);

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

            //LoadChallengeData();
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
            MapView.RemoveConstraint(cnsMapTop);
            MainScroll.RemoveConstraint(cnsMapTop);
            MainScroll.AddConstraint(cnsMapTop = NSLayoutConstraint.Create(MapView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, MainScroll, NSLayoutAttribute.Top, 1, MainScroll.ContentOffset.Y));
        }
        /*
        public override void LoadChallengeData()
        {
            base.LoadChallengeData();

            SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, Refresh);
        }
        */

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //LoadChallengeData();

            CheckInCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            CheckInCollectionView.RemoveConstraint(CheckInCollectionViewAspect);
            CheckInCollectionView.AddConstraint(ChallengesConstraints.ChallengesConstantHeightConstraint(CheckInCollectionView, 0f));

            //   CollectionViewsDescription.Font = PointText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.035f);
            //Overlay = Platform.AddCover(View);
            /*
            Overlay = Platform.AddOverlay(View.Frame, UIColor.White, true);
            if (Overlay != null)
            {
                nfloat w = Overlay.Frame.Width;//View.Frame.Width;//UIScreen.MainScreen.Bounds.Width;
                nfloat h = Overlay.Frame.Height;//View.Frame.Height;//UIScreen.MainScreen.Bounds.Height;
                nfloat s = w * 0.2f;
                UIImageView progress = new UIImageView(new CGRect((w - s) / 2.0f, ((h - s) / 2.0f), s, s));
                progress.Image = UIImage.FromBundle("loading-indicator");
                Overlay.AddSubview(progress);
                Platform.AnimateRotation(progress);
            }
            */
            //SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, Refresh);
        }

        public override void Refresh(ChallengeResponseModel challengeResponce)
        {
            base.Refresh(challengeResponce);

            if (challengeResponce == null)
                return;

            Challenge = challengeResponce.Challenge;
            HeaderLbl.Text = Challenge.Name;
            //MainTextLable.Text = Challenge.Desc;
            TimeDisLbl.Text = Challenge.NextEventCountDown;

            var navigationDelegate = new ChallengeDetailWebViewNavigationDelegate();
            navigationDelegate.NavigationFinished += SetupConstraint;
            this.WebView.NavigationDelegate = navigationDelegate;
            WebView.LoadHtmlString(Challenge.Desc, null);

            if (!DidSetupMap && Challenge.LocationLat != null && Challenge.LocationLong != null)
            {
                double radius = Challenge.RadiusMeters ?? 100.0;
                if (radius > 6000000)
                {
                    radius = 6000000;
                }
                double mapRegion = radius * 2.5;

                CLLocationCoordinate2D mapCoordinate = new CLLocationCoordinate2D(Challenge.LocationLat.Value, Challenge.LocationLong.Value);
                MapView.SetRegion(MKCoordinateRegion.FromDistance(mapCoordinate, mapRegion, mapRegion), true);

                MKCircle circle = MKCircle.Circle(mapCoordinate, radius);
                MapView.AddOverlay(circle);

                MKPointAnnotation annotation = new MKPointAnnotation();
                annotation.Coordinate = new CLLocationCoordinate2D(Challenge.LocationLat.Value, Challenge.LocationLong.Value);
                MapView.AddAnnotation(annotation);

                DidSetupMap = true;
            }
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
            cnWebViewHeight.Constant = actualHeight;

            CGSize scrollContentSize = MainScroll.ContentSize;
            scrollContentSize.Height = this.WebView.Frame.Top + actualHeight;
            MainScroll.ContentSize = scrollContentSize;

            View.LayoutIfNeeded();
        }

        public override void SubmitChallenge(UIButton button)
        {
            base.SubmitChallenge(button);
            SL.Manager.PostCheckInAndVerify(Challenge.ID, Platform.Lat, Platform.Lon, PostCheckInAndVerifyResponse);
        }

        partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            SubmitChallenge(SubmitButton);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            DescriptionCount.Font = TimeDisLbl.Font = CountPeopleLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.038f);
            HeaderLbl.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.060f);
            //MainTextLable.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.040f);
            //MainTextLable.LineBreakMode = UILineBreakMode.WordWrap;
            //MainTextLable.TintAdjustmentMode = UIViewTintAdjustmentMode.Automatic;
            //var labelString = new NSMutableAttributedString(MainTextLable.Text);
            //var paragraphStyle = new NSMutableParagraphStyle { LineSpacing = 2f };
            //var style = UIStringAttributeKey.ParagraphStyle;
            //var range = new NSRange(0, labelString.Length);

            //labelString.AddAttribute(style, paragraphStyle, range);
            //MainTextLable.AttributedText = labelString;
        }

        void Reset()
        {
            HeaderLbl.Text = null;
            TimeDisLbl.Text = null;
        }

        public void PostCheckInAndVerifyResponse(ChallengeResponseModel challengeResponse)
        {
            SubmitChallengeComplete(SubmitButton, challengeResponse);
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