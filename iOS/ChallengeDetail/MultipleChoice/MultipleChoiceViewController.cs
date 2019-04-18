using System;

using UIKit;
using CoreGraphics;
using SocialLadder.Models;
using FFImageLoading;
using Foundation;
using SocialLadder.iOS.Constraints;
using WebKit;
using System.Threading.Tasks;
using SocialLadder.iOS.Delegates;
using System.Threading;

namespace SocialLadder.iOS.Challenges
{
    public partial class MultipleChoiceViewController : ChallengeDetailBaseViewController, IUIAlertViewDelegate
    {
        private IDisposable _webViewObserver;
        private nfloat _lastWebViewHeight;
        private CancellationTokenSource _webViewCancellationTokenSource;

        #region constructors
        public MultipleChoiceViewController(IntPtr handle) : base(handle)
        {
        }

        #endregion

        #region lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            TableView.ScrollEnabled = true;
            TableView.RegisterNibForCellReuse(MultipleChoiceTableViewCell.Nib, MultipleChoiceTableViewCell.ClassName);

            Platform.ClearBrowserCache();

           
            
            //CollectionView.RegisterNibForCell(ChallengeCollectionViewCell.Nib, ChallengeCollectionViewCell.ClassName);

            //SL.Manager.GetChallengesAsync(Platform.LocalDataBasePath);
            //View.InsertSubview(new UIImageView(UIImage.FromBundle("Background")), 0);

            //TableView.ViewController = this;
            //QuestionText.Text = Challenge.Question;

            //Reset();
            //SL.Manager.RefreshChallengeDetail(Challenge.ChallengeDetailsURL, Refresh);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            nfloat multiplier = UIScreen.MainScreen.Bounds.Width / 414;

            btnTopNotify.Layer.BorderWidth = 1 * multiplier;
            btnTopNotify.Layer.BorderColor = UIColor.FromRGB(250, 250, 250).CGColor;
            btnTopNotify.Layer.CornerRadius = 4 * multiplier;

            lblTopUnlocksIn.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);
            lblTopTime.Font = UIFont.FromName("ProximaNova-Regular", 32 * multiplier);
            btnTopNotify.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);
            ChallengeText.Font = UIFont.FromName("ProximaNova-Bold", 24 * multiplier);
            TimeText.Font = UIFont.FromName("ProximaNova-Regular", 16 * multiplier);
            SelectAllThatApply.Font = UIFont.FromName("ProximaNova-Regular", 16 * multiplier);
            //XOthersHave.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);
            PointsText.Font = UIFont.FromName("ProximaNova-Regular", 14 * multiplier);

            cnsTableViewHeight.Constant = TableView.ContentSize.Height;

            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);

            InvokeOnMainThread(() =>
            {
                View.LayoutIfNeeded();
            });
            cnsTableViewHeight.Constant = TableView.ContentSize.Height;
            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);
            TableView.ScrollEnabled = false;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //SL.Manager.GetChallengesAsync(Platform.LocalDataBasePath);
            imgTopLock.Image = UIImage.FromBundle("lock_icon");
            PointsImage.Image = UIImage.FromBundle("points-icon_white");

            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.Source = new MultipleChoiceTableSource(this);
            TableView.RowHeight = UITableView.AutomaticDimension;
            //TableView.EstimatedRowHeight = 130.0f;
            TableView.Layer.BorderColor = UIColor.FromRGB(210, 210, 210).CGColor;
            TableView.Layer.BorderWidth = 2;
            //TableView.BackgroundColor = UIColor.FromRGB(250, 250, 250);
            TableView.ReloadData();

            CollectionViewHeightConstraint.Constant = 0;

            //MCCollectionView.TranslatesAutoresizingMaskIntoConstraints = false;
            //MCCollectionView.RemoveConstraint(MCCollectionViewAspect);
            //MCCollectionView.AddConstraint(ChallengesConstraints.ChallengesConstantHeightConstraint(MCCollectionView, 0));

            Reset();
            ChallengeText.Text = Challenge.Name;
            TimeText.Text = Challenge.NextEventCountDown;
            //Refresh(Challenge);

            PointsImage.Image = null;
            PointsText.Text = string.Empty;

            NSString viewportScriptString = (NSString)"var meta = document.createElement('meta'); meta.setAttribute('name', 'viewport'); meta.setAttribute('content', 'width=500'); meta.setAttribute('initial-scale', '1.0'); meta.setAttribute('maximum-scale', '1.0'); meta.setAttribute('minimum-scale', '1.0'); meta.setAttribute('user-scalable', 'no'); document.getElementsByTagName('head')[0].appendChild(meta);";

            WebView.Configuration.UserContentController.AddUserScript(new WKUserScript(source: viewportScriptString, injectionTime: WKUserScriptInjectionTime.AtDocumentEnd, isForMainFrameOnly: true));

            WebView.ScrollView.ScrollEnabled = false;
            WebView.ScrollView.Bounces = false;
            WebView.AllowsBackForwardNavigationGestures = false;
            
            SetupImageHeight(0);

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ScrollView.Scrolled += MainScroll_Scrolled;
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
        #endregion

        #region private
        private void Reset()
        {
            TimeText.Text = null;
            PointsText.Text = null;
            ChallengeText.Text = null;
            ChallengeImage.Image = null;
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

        private void MainScroll_Scrolled(object sender, EventArgs e)
        {
            var scrollView = (sender as UIScrollView);
            nfloat offset = scrollView.ContentOffset.Y;

            SetupImageHeight(offset);
        }

        private void SetupImageHeight(nfloat offset)
        {       
            nfloat height = ImageOverlayView.Frame.Height;
            ImageHeightConstraint.Constant = height + (-offset);
            View.LayoutIfNeeded();
        }

        #endregion

        #region public

        public override void Refresh(ChallengeResponseModel response)
        {
            base.Refresh(response);

            if (response == null)
                return;

            Challenge = response.Challenge;
            TimeText.Text = Challenge.NextEventCountDown;
            PointsText.Text = "+" + Challenge.PointValue.ToString() + " pts";
            ChallengeText.Text = Challenge.Name;
            ImageService.Instance.LoadUrl(Challenge.Image).Into(ChallengeImage);

            var navigationDelegate = new ChallengeDetailWebViewNavigationDelegate();
            navigationDelegate.NavigationFinished += SetupConstraint;
            this.WebView.NavigationDelegate = navigationDelegate;
            WebView.LoadHtmlString(Challenge.Desc, null);

            TableView.ReloadData();

            if (response.Challenge.IsSurvey)
            {
                SelectAllThatApply.Text = "Select all that apply";
            }
            else
            {
                SelectAllThatApply.Text = "Select an answer below!";
            }
        }

        public void SubmitResponse(ChallengeResponseModel challengeResponse)
        {
            SubmitChallengeComplete(SubmitButton, challengeResponse);
        }

        public override void SubmitChallenge(UIButton button)
        {

            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }

            if (Challenge == null && Challenge.AnswerList == null)
            {
                return;
            }

            if (0 <= TableView.SelectedRow && TableView.SelectedRow < Challenge.AnswerList.Count)
            {
                ChallengeAnswerModel answer = Challenge.AnswerList[TableView.SelectedRow];
                if (answer == null)
                {
                    return;
                }

                if (answer.isWriteIn)
                {
                    UIAlertView alert = new UIAlertView(string.Empty, answer.writeInPrompt, this, "Cancel", null);
                    alert.AddButton("Ok");
                    alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                    alert.Show();
                    return;
                }
                base.SubmitChallenge(button);
                SL.Manager.SubmitAnswerAsync(Challenge.ID, answer.ID, null, SubmitResponse);
            }

        }

        [Export("alertView:didDismissWithButtonIndex:")]
        public void DidDismissWithButtonIndex(UIAlertView alertView, nint buttonIndex)
        {
            if (buttonIndex == 1)
            {
                ChallengeAnswerModel answer = Challenge.AnswerList[TableView.SelectedRow];
                var textField = alertView.GetTextField(0);
                SL.Manager.SubmitAnswerAsync(Challenge.ID, answer.ID, textField.Text, SubmitResponse);
            }
        }
        #endregion

        #region partial

        partial void SubmitButton_TouchUpInside(UIButton sender)
        {
            SubmitChallenge(SubmitButton);
        }

        #endregion


    }
}

