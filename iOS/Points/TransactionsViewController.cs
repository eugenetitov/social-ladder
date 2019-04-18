using Foundation;
using System;
using UIKit;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.ViewControllers;
using CoreGraphics;
using CoreAnimation;
using SocialLadder.Models;
using System.Threading.Tasks;
using FFImageLoading;
using SocialLadder.iOS.Constraints;
using System.Linq;
using SocialLadder.iOS.Models.Enums;

namespace SocialLadder.iOS.Points
{
    public partial class TransactionsViewController : PointsBaseViewController
    {
        #region Fields

        public UIImageView IvBackground;
        public NSLayoutConstraint CnBgCenterX;
        private TransactionsTableSource _transactionsTableSource;

        bool didRefreshProfile;
        bool didRefreshChallenges;
        bool didRefreshRewards;
        bool didRefreshTransactions;
        bool DidRefreshAll
        {
            get
            {
                return didRefreshRewards && didRefreshProfile && didRefreshChallenges && didRefreshTransactions;
            }
        }
        
        #endregion

        #region Ctor

        public TransactionsViewController(IntPtr handle) : base(handle)
        {
        }

        #endregion

        #region Lifecycle

        public override void ViewDidLoad()
        {
            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;
            base.AreaCollectionTopOutlet = cnsAreaCollectionTop;

            base.ViewDidLoad();

            lblScoreValue.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 24);
            lblTransactionHistoryTitle.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 16);
            lblChallengesTitle.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 12);
            lblChallengesValue.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 14);
            lblRewardsTitle.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 12);
            lblRewarsValue.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 14);
            btnSeeRewards.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);
            btnDoMoreChallenges.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);

            btnSeeRewards.Layer.BorderColor = btnSeeRewards.TitleLabel.TextColor.CGColor;
            btnSeeRewards.Layer.BorderWidth = 1f;
            btnSeeRewards.Layer.CornerRadius = 2f;

            btnDoMoreChallenges.Layer.BorderColor = btnDoMoreChallenges.TitleLabel.TextColor.CGColor;
            btnDoMoreChallenges.Layer.BorderWidth = 1f;
            btnDoMoreChallenges.Layer.CornerRadius = 2f;

            _transactionsTableSource = new TransactionsTableSource(TableView, ivScoreImage);
            _transactionsTableSource.NeedsLoadMore = NeedsLoadMoreTransactions;
            TableView.Source = _transactionsTableSource;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            cnsTableViewHeight.Constant = TableView.ContentSize.Height;
            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);

            InvokeOnMainThread(() =>
            {
                View.LayoutIfNeeded();
            });

            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(ivTriangle.Layer.Bounds.X, ivTriangle.Layer.Bounds.Y));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.GetMaxX(), ivTriangle.Layer.Bounds.GetMaxY()));
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

            ScrollToOffset(ScrollView);
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                base.ViewWillAppear(animated);

                IvBackground = ivRightBackground;
                CnBgCenterX = cnRightBgCenterX;

                ScrollView.Scrolled += ScrollViewScrolled;
                ScrollView.DecelerationEnded += OnDecelerationEnded;

                ScrollToOffset(ScrollView);
            }
            catch (Exception)
            {
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            PointsContainerViewController.PageYOffset = ScrollView.ContentOffset.Y;
            PointsContainerViewController.ChangeSelectedButton(PageIndex);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            ScrollView.DecelerationEnded -= OnDecelerationEnded;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            didRefreshProfile = false;
            didRefreshChallenges = false;
            didRefreshRewards = false;
            didRefreshTransactions = false;
        }

        #endregion

        #region Refresh methods

        public async override Task Refresh()
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }
            didRefreshRewards = false;
            didRefreshChallenges = false;
            didRefreshProfile = false;
            didRefreshTransactions = false;

            //UIWindow window = UIApplication.SharedApplication.KeyWindow;
            ivScoreImage.Image = UIImage.FromBundle("loading-indicator");
            Platform.AnimateRotation(ivScoreImage);

            lblScoreValue.Hidden = true;

            await base.Refresh();

            await SL.Manager.GetChallengesAsync(RefreshChallengesComplete);
            await SL.Manager.GetRewardsAsync(RefreshRewardsComplete);
            await SL.Manager.GetProfileAsync(RefreshProfileComplete);
            await SL.Manager.RefreshTransactionsAsync(RefreshTransactionComplete);
        }

        public override void RefreshLocally()
        {
            base.RefreshLocally();

            didRefreshProfile = true;
            didRefreshChallenges = true;
            didRefreshRewards = true;
            didRefreshTransactions = true;

            if (SL.HasProfile)
            {
                lblScoreValue.Text = SL.Profile.Score.ToString();
                lblChallengesValue.Text = SL.Profile.ChallengeCompCount + " of " + SL.Profile.ChallengeCount;
                lblRewarsValue.Text = SL.Profile.RewardLabel + " of " + SL.Profile.RewardCount;
            }

            UpdateNavBar();

            TableView.ReloadData();

            Platform.AnimateRotationComplete(ivScoreImage);
            ivScoreImage.Image = UIImage.FromBundle("IconScoreTransactions");

            lblScoreValue.Hidden = false;

            AreaCollection.ReloadData();
        }

        void RefreshChallengesComplete(ChallengeResponseModel response)
        {
            didRefreshChallenges = true;
            RefreshComplete();
        }

        void RefreshRewardsComplete(RewardResponseModel response)
        {
            didRefreshRewards = true;
            RefreshComplete();
        }

        void RefreshProfileComplete(ProfileResponseModel response)
        {
            try
            {
                if (SL.HasProfile)
                {
                    lblScoreValue.Text = SL.Profile.Score.ToString();
                    lblChallengesValue.Text = SL.Profile.ChallengeCompCount + " of " + SL.Profile.ChallengeCount;
                    lblRewarsValue.Text = SL.Profile.RewardLabel + " of " + SL.Profile.RewardCount;
                }

                UpdateNavBar();
            }
            catch (Exception)
            {
            }

            didRefreshProfile = true;
            RefreshComplete();
        }

        void RefreshTransactionComplete(TransactionResponseModel response)
        {
            try
            {
                TableView.ReloadData();

                cnsTableViewHeight.Constant = TableView.ContentSize.Height;
                ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);

            }
            catch (Exception)
            {
            }

            didRefreshTransactions = true;
            RefreshComplete();
        }
        private void LoadMoreTransactionsCompleted(TransactionResponseModel transactionResponse)
        {
            TableView.ReloadData();

            cnsTableViewHeight.Constant = TableView.ContentSize.Height;
            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);

            didRefreshTransactions = true;
            RefreshComplete();
        }

        void RefreshComplete()
        {
            ;
            if (DidRefreshAll)
            {
                Platform.AnimateRotationComplete(ivScoreImage);
                ivScoreImage.Image = UIImage.FromBundle("IconScoreTransactions");

                lblScoreValue.Hidden = false;

                AreaCollection.ReloadData();
            }
        }

        #endregion

        #region EventHandlers
        public void NeedsLoadMoreTransactions()
        {
            int? visibleCellsCount = TableView.VisibleCells?.Count();

            if (visibleCellsCount == null || visibleCellsCount == 0)
            {
                return;
            }

            UITableViewCell lastCell = TableView.VisibleCells[visibleCellsCount == 1? 0 : (int)visibleCellsCount - 2];

            CGRect globalPosition = lastCell.Superview.ConvertRectToView(lastCell.Frame, UIApplication.SharedApplication.KeyWindow);//
            if (didRefreshTransactions == true && globalPosition.Bottom <= View.Frame.Height && !string.IsNullOrEmpty(SL.TransactionPages?.NextPage))
            {
                ;
                didRefreshTransactions = false;
                SL.Manager.GetNextTransactionsPageByUrlAsync(SL.TransactionPages.NextPage, LoadMoreTransactionsCompleted);

            }

            ;
        }
        //public void OnLoadMoreCompleted()
        //{
        //    //cnsTableViewHeight.Constant = TableView.ContentSize.Height;
        //    //ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, TableView.Frame.Y + TableView.ContentSize.Height);
        //}

        private void ScrollViewScrolled(object sender, EventArgs e)
        {
            if (!AreaCollectionHidden)
            {
                HideAreaCollection();
            }
            PointsContainerViewController.PageYOffset = ((UIScrollView)sender).ContentOffset.Y;

        //    if (ScrollView.ContentOffset.Y + ScrollView.Bounds.Height == ScrollView.ContentSize.Height)
        //    {
        //        ;
        //    }
        //;
        }

        private void OnDecelerationEnded(object sender, EventArgs e)
        {
            NeedsLoadMoreTransactions();
        }

        //private void OnScrollAnimationEnded(object sender, EventArgs e)
        //{
        //    UITableViewCell lastCell = TableView.VisibleCells?.LastOrDefault();

        //    if (lastCell == null)
        //    {
        //        return;
        //    }

        //    CGRect globalPosition = lastCell.Superview.ConvertRectToView(lastCell.Frame, UIApplication.SharedApplication.KeyWindow);//
        //    if (globalPosition.Bottom <= View.Frame.Height)
        //    {
        //        ;
        //    }
        //    ;
        //}

        partial void TransactionsButton_TouchUpInside(SLButton sender)
        {
            //HideAreaCollection();
            //Platform.SwapIn(NavigationController, "Main", "TransactionsViewController", ENavigationTabs.POINTS);
        }

        partial void PointsAndStatsButton_TouchUpInside(UIButton sender)
        {
            HideAreaCollection();
            NextPage();
            //Platform.SwapIn(NavigationController, "Main", "PointsViewController", ENavigationTabs.POINTS);
        }

        partial void LeaderboardButton_TouchUpInside(UIButton sender)
        {
            HideAreaCollection();
            PrevPage();
            //Platform.SwapIn(NavigationController, "Main", "LeaderboardViewController", ENavigationTabs.POINTS);
        }

        partial void btnRewards_TouchUpInside(UIButton sender)
        {
            //HideAreaCollection();
            Platform.SelectTab(NavigationController, ENavigationTabs.REWARDS);
        }

        partial void btnChallenges_TouchUpInside(UIButton sender)
        {
            //HideAreaCollection();
            Platform.SelectTab(NavigationController, ENavigationTabs.CHALLENGES);
        }

        #endregion


    }

    public class CustomScrollViewDelegate : UIScrollViewDelegate
    {
        public Action OnDecelerationEnded;

        public CustomScrollViewDelegate() : base()
        {
            ;
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            try
            {
                OnDecelerationEnded?.Invoke();
                ;

            }
            catch (Exception)
            {
            }
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            try
            {
                ;//base.Scrolled(scrollView);

            }
            catch (Exception)
            {
            }
        }

        public override void ScrollAnimationEnded(UIScrollView scrollView)
        {
            try
            {
                ;//base.ScrollAnimationEnded(scrollView);

            }
            catch (Exception)
            {
            }
        }
    }
}