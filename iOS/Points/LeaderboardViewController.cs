using Foundation;
using System;
using UIKit;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.ViewControllers;
using FFImageLoading;
using CoreGraphics;
using SocialLadder.iOS.Points.CustomViews;
using CoreAnimation;
using SocialLadder.Helpers;
using SocialLadder.Models;
using System.Linq;
using SocialLadder.iOS.UserInfo;
using SocialLadder.iOS.Constraints;
using System.Threading.Tasks;
using SocialLadder.iOS.Models.Enums;

namespace SocialLadder.iOS.Points
{
    public partial class LeaderboardViewController : PointsBaseViewController//UIViewController
    {
        #region Fields

        private nfloat _mainScreenWidth = UIScreen.MainScreen.Bounds.Width;
        private LeaderboardFilterTableSource _filterTableSource;
        private LeaderboardTableSource _leaderBoardTableSource;



        bool didRefreshProfile;
        bool didRefreshChallenges;
        bool didRefreshRewards;
        bool DidRefreshAll
        {
            get
            {
                return didRefreshRewards && didRefreshProfile && didRefreshChallenges;
            }
        }

        #endregion

        #region Ctor

        public LeaderboardViewController(IntPtr handle) : base(handle)
        {

        }

        #endregion

        #region Lifecycle

        public override void ViewDidLoad()
        {
            vLeaderboardDropdown.Hidden = true;
            btnDropDown.Hidden = true;
            leaderboardTextView.Editable = false;

            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;
            base.AreaCollectionTopOutlet = cnsAreaCollectionTop;

            base.ViewDidLoad();

            ivLeaderboardDropdown.Image = UIImage.FromBundle("LeaderboardDropdownRectangle");
            btnDropDown.SetImage(UIImage.FromBundle("arrow-down_black"), UIControlState.Normal);
            ivLeaderboardDropdown.BackgroundColor = UIColor.Clear;

            string rank = CountSuffixGenerator.GenerateSuffixedStringFromString(SL.Profile.FriendRank);
            string temp = SL.Profile.ChallengeLabel;
            string nameArea = SL.AreaName;
            temp = SL.Profile.RewardLabel;
            temp = SL.Profile.ChallengeSubLabel;
            temp = SL.Profile.RewardSubLabel;

            leaderboardTextView.Text = "We don't see any of your friend in " + nameArea;
            lblProfileLeaderboardPosition.Text = String.Format("Ranked {0} among friends", rank);

            if (SL.Profile != null)
            {
                ImageService.Instance.LoadUrl(SL.Profile?.ProfilePictureURL).Into(ProfileImage);
            }

            InitializeChelengesButton();

            //tvLeaderboardTable.AllowsSelection = true;
            UINib nib = LeaderboardTableViewCell.Nib;
            tvLeaderboardTable.RegisterNibForCellReuse(nib, LeaderboardTableViewCell.ClassName);

            _leaderBoardTableSource = new LeaderboardTableSource();

            if (SL.FriendList != null)
            {
                _leaderBoardTableSource.CountItemsToDisplay = GetCountOfItems() + 1;
            }

            _leaderBoardTableSource.OnRowSelected = OnFriendSelected;
            //_leaderBoardTableSource.MaxScoreValue = SL.FriendList
            tvLeaderboardTable.Source = _leaderBoardTableSource;
            tvLeaderboardTable.ReloadData();

            _filterTableSource = new LeaderboardFilterTableSource(tvLeaderboardFilterTable, vLeaderboardDropdown);
            tvLeaderboardFilterTable.Source = _filterTableSource;
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            ProfileImage.Layer.CornerRadius = ProfileImage.Frame.Width / 2;
            ChallengesButton.Layer.CornerRadius = 2;

            ChallengesButton.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);
            lblLedaerboardCollectionTitle.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 16);
            lblProfileLeaderboardPosition.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);

            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, tvLeaderboardTable.Frame.Y + tvLeaderboardTable.ContentSize.Height);

            tvLeaderboardTable.UpdateConstraints();
            vContent.UpdateConstraints();

            InvokeOnMainThread(() =>
            {
                View.LayoutIfNeeded();
            });

            ScrollView.ContentSize = new CGSize(ScrollView.Frame.Width, tvLeaderboardTable.Frame.Y + tvLeaderboardTable.ContentSize.Height);

            var bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(ivTriangle.Layer.Bounds.GetMidX(), ivTriangle.Layer.Bounds.Y));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.X, ivTriangle.Layer.Bounds.GetMaxY()));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.X, ivTriangle.Layer.Bounds.GetMaxY() + 4));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.GetMaxX(), ivTriangle.Layer.Bounds.GetMaxY() + 4));
            bezierPath.AddLineTo(new CGPoint(ivTriangle.Layer.Bounds.GetMaxX(), ivTriangle.Layer.Bounds.GetMaxY()));
            bezierPath.ClosePath();

            ivTriangle.Layer.AddSublayer(new CAShapeLayer
            {
                Frame = ivTriangle.Bounds,
                Path = bezierPath.CGPath,
                FillColor = UIColor.White.CGColor
            });
            ivTriangle.Image = null;


            //tvLeaderboardTable.AllowsSelection = false;
            ScrollToOffset(ScrollView);

            SL.Manager.GetProfileAsync(CheckLeaderBoardTableView);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            ScrollView.Scrolled += ScrollViewScrolled;
            ScrollToOffset(ScrollView);
            CheckLeaderBoardTableView(null);

            //if (ShouldRefreshEndpoint)//PointsContainerViewController.ShouldRefreshEndpoints
            //{
            //    Refresh();

            //    return;
            //}

            //RefreshLocally();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            PointsContainerViewController.PageYOffset = ScrollView.ContentOffset.Y;
            PointsContainerViewController.ChangeSelectedButton(PageIndex);
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

            //UIWindow window = UIApplication.SharedApplication.KeyWindow;
            ProfileImage.Image = UIImage.FromBundle("loading-indicator");
            Platform.AnimateRotation(ProfileImage);

            await base.Refresh();

            await SL.Manager.GetChallengesAsync(RefreshChallengesComplete);
            await SL.Manager.GetRewardsAsync(RefreshRewardsComplete);
            await SL.Manager.GetProfileAsync(RefreshProfileComplete);
        }

        //void RefreshRewards()
        //{
        //    SL.Manager.GetRewardsAsync(RefreshRewardsComplete);
        //}

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
            int? friendsCount = SL.Profile?.FriendPreviewList?.Count;
            if (friendsCount != null)
            {
                var maxScore = friendsCount > 0 ? response.Profile.FriendPreviewList[0].Score : 0;
                _leaderBoardTableSource.MaxScoreValue = maxScore.HasValue == true ? (float)(maxScore.Value) : 0;

                _leaderBoardTableSource.CountItemsToDisplay = GetCountOfItems();
                tvLeaderboardTable.ReloadData();
            }

            EmptyView.Hidden = friendsCount != null && friendsCount > 0;

            string rank = CountSuffixGenerator.GenerateSuffixedStringFromString(SL.Profile.FriendRank);
            lblProfileLeaderboardPosition.Text = String.Format("Ranked {0} among friends", rank);

            //AreaCollection.ReloadData();
            UpdateNavBar();

            didRefreshProfile = true;
            RefreshComplete();
            //SetupLeaderBoardTableView();
        }

        public override void RefreshLocally()
        {
            base.RefreshLocally();

            int? friendsCount = SL.Profile?.FriendPreviewList?.Count;
            if (friendsCount != null)
            {
                var maxScore = friendsCount > 0 ? SL.Profile.FriendPreviewList[0].Score : 0;
                _leaderBoardTableSource.MaxScoreValue = maxScore.HasValue == true ? (float)(maxScore.Value) : 0;

                _leaderBoardTableSource.CountItemsToDisplay = GetCountOfItems();
                tvLeaderboardFilterTable.ReloadData();
            }

            EmptyView.Hidden = friendsCount != null && friendsCount > 0;

            string rank = CountSuffixGenerator.GenerateSuffixedStringFromString(SL.Profile.FriendRank);
            lblProfileLeaderboardPosition.Text = String.Format("Ranked {0} among friends", rank);

            UpdateNavBar();

            //AreaCollection.ReloadData();

            Platform.AnimateRotationComplete(ProfileImage);
            ImageService.Instance.LoadUrl(SL.Profile?.ProfilePictureURL).Into(ProfileImage);
            AreaCollection.ReloadData();
        }

        void RefreshComplete()
        {
            if (DidRefreshAll)
            {
                Platform.AnimateRotationComplete(ProfileImage);
                ImageService.Instance.LoadUrl(SL.Profile?.ProfilePictureURL).Into(ProfileImage);
                AreaCollection.ReloadData();
            }
        }

        #endregion

        #region Methods

        //private void SetupLeaderBoardTableView()
        //{
        //    _leaderBoardTableSource.CountItemsToDisplay = GetCountOfItems();
        //    tvLeaderboardFilterTable.ReloadData();
        //}

        int GetCountOfItems()
        {
            var rowHeight = _leaderBoardTableSource.GetHeightForRow(null, null);
            if (SL.FriendList == null)
            {
                return 0;
            }

            _leaderBoardTableSource.MaxScoreValue = (float)SL.FriendList.Max(x => x.Score);
            var numberOfRow = SL.FriendList.Count;
            var maxHeight = ScrollView.Frame.Height - vContentBackground.Frame.Y;
            var expectedheight = (rowHeight + 5) * numberOfRow;
            TableViewHeight.Constant = maxHeight;

            if (maxHeight < expectedheight)
            {
                return (int)(maxHeight / rowHeight) - 1;
            }
            return SL.FriendList.Count;
        }


        public void CheckLeaderBoardTableView(ProfileResponseModel callback)
        {
            EmptyView.Hidden = true;
            var itemsCount = tvLeaderboardTable.Source.RowsInSection(tvLeaderboardTable, 0);

            if (itemsCount == 0)
            {
                string nameArea = SL.AreaName;
                tvLeaderboardTable.Hidden = true;
                EmptyView.Hidden = false;
                leaderboardTextView.Text = "We don't see any of your friends in " + nameArea;
                return;
            }

            EmptyView.Hidden = true;
            tvLeaderboardTable.Hidden = false;
        }

        private void InitializeChelengesButton()
        {
            ChallengesButton.Layer.BorderColor = ChallengesButton.TitleLabel.TextColor.CGColor;
            ChallengesButton.Layer.BorderWidth = 1f;
        }

        #endregion

        #region EventHandlers

        private void OnFriendSelected(int profileId)
        {
            UIStoryboard storyboard = UIStoryboard.FromName("UserInfo", null);
            UserInfoViewController viewController = storyboard.InstantiateViewController("UserInfoViewController") as UserInfoViewController;
            viewController.ShouldGetProfileByFriendId = true;
            viewController.FriendId = profileId;
            viewController.FeedUrl = string.Empty;

            this.NavigationController.PresentViewController(viewController, false, null);
        }

        private void ScrollViewScrolled(object sender, EventArgs e)
        {
            if (!AreaCollectionHidden)
            {
                HideAreaCollection();
            }
            PointsContainerViewController.PageYOffset = ((UIScrollView)sender).ContentOffset.Y;
        }

        partial void ButtonDropDown_TouchUpInside(UIButton sender)
        {
            View.BringSubviewToFront(vLeaderboardDropdown);

            if (!vLeaderboardDropdown.Hidden)
            {
                vLeaderboardDropdown.Hidden = true;
                return;
            }
            if (vLeaderboardDropdown.Hidden)
            {
                vLeaderboardDropdown.Hidden = false;
                return;
            }
        }

        //partial void LeaderboardButton_TouchUpInside(SLButton sender)
        //{
        //    //HideAreaCollection();
        //    //Platform.SwapIn(NavigationController, "Main", "LeaderboardViewController", ENavigationTabs.POINTS);
        //}

        //partial void PointsAndStatsButton_TouchUpInside(UIButton sender)
        //{
        //    HideAreaCollection();
        //    PrevPage();
        //    //Platform.SwapIn(NavigationController, "Main", "PointsViewController", ENavigationTabs.POINTS);
        //}

        //partial void TransactionsButton_TouchUpInside(UIButton sender)
        //{
        //    HideAreaCollection();
        //    NextPage();
        //    //Platform.SwapIn(NavigationController, "Main", "TransactionsViewController", ENavigationTabs.POINTS);
        //}

        partial void ChallengesButton_TouchUpInside(UIButton sender)
        {
            //HideAreaCollection();
            Platform.SelectTab(NavigationController, ENavigationTabs.CHALLENGES);
        }

        #endregion
    }
}