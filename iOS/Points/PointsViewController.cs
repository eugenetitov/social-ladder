using System;
using System.Reflection.Emit;
using UIKit;
using SocialLadder.iOS.Navigation;
using System.Diagnostics;
using SocialLadder.iOS.ViewControllers;
using Foundation;
using CoreGraphics;
using SocialLadder.iOS.Helpers;
using SocialLadder.iOS.Constraints;
using System.Linq;
using SocialLadder.iOS.Points.CustomViews;
using CoreAnimation;
using SocialLadder.Models;
using System.Threading.Tasks;
using SocialLadder.ViewModels.Points;
using SocialLadder.iOS.ViewControllers.Main;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace SocialLadder.iOS.Points
{
    [MvxFromStoryboard("Main")]
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Points", TabIconName = "points-icon_on", TabSelectedIconName = "points-icon_off")]
    public partial class PointsViewController : BaseTabViewController<PointsViewModel>//PointsBaseViewController
    {
        #region Fields & Properties

        public UIImageView IvBackground;
        public NSLayoutConstraint CnBgCenterX;

        private CGRect _screenSize = UIScreen.MainScreen.Bounds;
        private UIView _overlay;
        private RankInfoView _rankInfoView;

        bool didRefreshProfile;
        //bool didRefreshChallenges;
        //bool didRefreshRewards;

        UIView Overlay
        {
            get; set;
        }

        bool DidRefreshAll
        {
            get
            {
                //return didRefreshRewards && didRefreshProfile && didRefreshChallenges;
                return didRefreshProfile;
            }
        }

        #endregion

        #region Ctor

        public PointsViewController(IntPtr handle) : base(handle)
        {

        }

        #endregion
        
        #region Lifecycle

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;
            base.AreaCollectionTopOutlet = cnsAreaCollectionTop;

            base.ViewDidLoad();

            SL.Manager.GetProfileAsync();

            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, ScrollView.Frame.Height);

            _rankInfoView = RankInfoView.Create();
            _rankInfoView.Initialize();

            ScoreView.SetupViewStyle();
            ScoreView.SetupViewComponents();

            btnInfo.SetImage(UIImage.FromBundle("info-icon_gray"), UIControlState.Normal);
            PointsAndStatsButton.Underline = true;

            if (SL.HasProfile)
            {
                Score.Text = SL.Profile.Score.ToString();
            }

            UINib nib = PointsTableViewCell.Nib;
            TableView.RegisterNibForCellReuse(nib, PointsTableViewCell.ClassName);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 69f;
            TableView.Source = new PointsTableSource();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            var marin = UIScreen.MainScreen.Bounds.Width * 0.056f;
            btnInfo.ContentEdgeInsets = new UIEdgeInsets(marin, marin, marin, marin);

            cnsTableViewHeight.Constant = TableView.ContentSize.Height;

            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);

            ScoreView.LayoutSubviews();

            Score.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 24);

            lblScoreGroup.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 14);
            lblScoreGroup.SizeToFit();

            InvokeOnMainThread(() =>
            {
                View.LayoutIfNeeded();
            });

            cnsTableViewHeight.Constant = TableView.ContentSize.Height;
            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);

            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(ivTriangle.Layer.Bounds.GetMaxX(), ivTriangle.Layer.Bounds.Y));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.GetMaxX(), ivTriangle.Layer.Bounds.GetMaxY() + 4));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.X, ivTriangle.Layer.Bounds.GetMaxY() + 4));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.X, ivTriangle.Layer.Bounds.GetMaxY()));
            bezierPath.ClosePath();

            ivTriangle.Layer.AddSublayer(new CAShapeLayer
            {
                Frame = ivTriangle.Bounds,
                Path = bezierPath.CGPath,
                FillColor = UIColor.White.CGColor
            });
            ivTriangle.Image = null;

            //ScrollToOffset(ScrollView);
        }

        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);

        //    IvBackground = ivBackground;
        //    CnBgCenterX = cnBgCenterX;

        //    btnInfo.TouchUpInside += InfoButton_TouchUpInside;
        //    _rankInfoView.CloseButton.TouchUpInside += CloseInfoButton_TouchUpInside;

        //    ScrollView.Scrolled += ScrollViewScrolled;
        //    ScrollToOffset(ScrollView);

        //    //if (ShouldRefreshEndpoint) //PointsContainerViewController.ShouldRefreshEndpoints
        //    //{
        //    //    Refresh();

        //    //    return;
        //    //}
        //    //RefreshLocally();

        //}

        //public override void ViewWillDisappear(bool animated)
        //{
        //    base.ViewWillDisappear(animated);

        //    btnInfo.TouchUpInside -= InfoButton_TouchUpInside;
        //    _rankInfoView.CloseButton.TouchUpInside -= CloseInfoButton_TouchUpInside;
        //}

        //public override void ViewDidAppear(bool animated)
        //{
        //    base.ViewDidAppear(animated);
        //    PointsContainerViewController.PageYOffset = ScrollView.ContentOffset.Y;
        //    PointsContainerViewController.ChangeSelectedButton(PageIndex);


        //}

        #endregion

        #region Refresh methods

        public async override Task Refresh()
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }

            //UIWindow window = UIApplication.SharedApplication.KeyWindow;
            ScoreImage.Image = UIImage.FromBundle("loading-indicator");
            Platform.AnimateRotation(ScoreImage);

            Score.Hidden = true;

            await base.Refresh();

            didRefreshProfile = false;
            await SL.Manager.GetProfileAsync(RefreshProfileComplete);
            //RefreshRewards();
            //RefreshChallenges();
            //await RefreshProfile();
        }

        //public override void RefreshLocally()
        //{
        //    base.RefreshLocally();

        //    TableView.ReloadData();

        //    UpdateNavBar();

        //    //Platform.AnimateRotationComplete(ScoreImage);
        //    ScoreImage.Image = UIImage.FromBundle("IconScoreTransactions");

        //    Score.Text = SL.Profile.Score.ToString();
        //    Score.Hidden = false;

        //    AreaCollection.ReloadData();
        //}

        /*
        void RefreshChallenges()
        {
            didRefreshChallenges = false;
            SL.Manager.GetChallengesAsync(RefreshChallengesComplete);
        }

        void RefreshChallengesComplete(ChallengeResponseModel response)
        {
            didRefreshChallenges = true;
            RefreshComplete();
        }
        */
        //async Task RefreshProfile()
        //{
        //    didRefreshProfile = false;
        //    await SL.Manager.GetProfileAsync(RefreshProfileComplete);
        //}

        void RefreshProfileComplete(ProfileResponseModel response)
        {
            TableView.ReloadData();

            UpdateNavBar();

            didRefreshProfile = true;
            RefreshComplete();
        }
        /*
        void RefreshRewards()
        {
            didRefreshRewards = false;
            SL.Manager.GetRewardsAsync(RefreshRewardsComplete);
        }

        void RefreshRewardsComplete(RewardResponseModel response)
        {
            didRefreshRewards = true;
            RefreshComplete();
        }
        */

        void RefreshComplete()
        {
            if (DidRefreshAll)
            {
                Platform.AnimateRotationComplete(ScoreImage);
                ScoreImage.Image = UIImage.FromBundle("IconScoreTransactions");
                Score.Text = SL.Profile.Score.ToString();
                Score.Hidden = false;
                AreaCollection.ReloadData();
            }
        }

        #endregion

        #region EventHandlers

        private void ScrollViewScrolled(object sender, EventArgs e)
        {
            if (!AreaCollectionHidden)
            {
                HideAreaCollection();
            }
           //PointsContainerViewController.PageYOffset = ((UIScrollView)sender).ContentOffset.Y;
        }

        public override void UpdateViewConstraints()
        {
            base.UpdateViewConstraints();
        }

        void InfoButton_TouchUpInside(object sender, EventArgs e)
        {
            _overlay = Platform.AddOverlay(_screenSize, new UIColor(0, 0, 0, 0.5f), true);//
            if (_overlay != null)
            {
                _overlay.AddSubview(_rankInfoView);

                NSLayoutConstraint centerX = RankInfoPointsViewConstraints.RankInfoPointsViewCenterX(_rankInfoView, _overlay);
                NSLayoutConstraint centerY = RankInfoPointsViewConstraints.RankInfoPointsViewCenterY(_rankInfoView, _overlay);
                NSLayoutConstraint width = RankInfoPointsViewConstraints.RankInfoPointsViewWidth(_rankInfoView, _overlay);
                NSLayoutConstraint aspectRatio = RankInfoPointsViewConstraints.RankInfoPointsViewAspectRatio(_rankInfoView);

                centerX.Active = true;
                centerY.Active = true;
                width.Active = true;
                aspectRatio.Active = true;

                _overlay.AddConstraint(centerX);
                _overlay.AddConstraint(centerY);
                _overlay.AddConstraint(width);
                _rankInfoView.AddConstraint(aspectRatio);


            }

        }

        void CloseInfoButton_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (_overlay != null)
                {
                    _overlay.RemoveFromSuperview();
                    _overlay = null;
                }
            }

            catch (Exception)
            {
            }

        }

        //partial void LeaderboardButton_TouchUpInside(UIButton sender)
        //{
        //    HideAreaCollection();
        //    NextPage();
        //    //Platform.SwapIn(NavigationController, "Main", "LeaderboardViewController", ENavigationTabs.POINTS);
        //}

        //partial void TransactionsButton_TouchUpInside(UIButton sender)
        //{
        //    HideAreaCollection();
        //    PrevPage();
        //    //Platform.SwapIn(NavigationController, "Main", "TransactionsViewController", ENavigationTabs.POINTS);
        //}

        partial void PointsAndStatsButton_TouchUpInside(SLButton sender)
        {
            //HideAreaCollection();
            //Platform.SwapIn(NavigationController, "Main", "PointsViewController", ENavigationTabs.POINTS);
        }

        #endregion
    }
}

