using System;
using UIKit;
using SocialLadder.iOS.Navigation;
using System.Collections.Generic;
using SocialLadder.Models;
using System.Threading.Tasks;
using CoreGraphics;
using SocialLadder.iOS.ViewControllers;
using SocialLadder.iOS.RewardCollection;
using SocialLadder.iOS.Models.Enums;
using SocialLadder.iOS.ViewControllers.Main;
using SocialLadder.ViewModels.Points;
using SocialLadder.ViewModels.Rewards;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters.Attributes;

namespace SocialLadder.iOS.Rewards
{
    [MvxFromStoryboard("Rewards")]
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Rewards", TabIconName = "reward-icon_on", TabSelectedIconName = "reward-icon_off")]
    public partial class RewardsViewController : BaseTabViewController<RewardCategoriesViewModel>//RewardsBaseViewController
    {
        public bool ShouldDrillUpImmediately { get; set; }
        private nfloat vHeight;

        public int CategoryID
        {
            get; set;
        }
        public List<RewardItemModel> RewardList
        {
            get; set;
        }

        public bool DidRefresh
        {
            get; set;
        }

        public string RewardsCategoryName
        {
            get; set;
        }

        public string RewardsSubcategoryName
        {
            get; set;
        }

        public bool IsCategoryPageSkiped;

        public RewardsViewController(IntPtr handle) : base(handle)
        {
            IsChangingAreasDisabled = true;
        }

        public override void ViewDidLoad()
        {
            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;

            base.ViewDidLoad();
            ShouldDrillUpImmediately = true;

            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.RegisterNibForCellReuse(RewardCategoryTableViewCell.Nib, RewardCategoryTableViewCell.ClassName);
            TableView.RegisterNibForCellReuse(RewardsTableViewCell.Nib, RewardsTableViewCell.ClassName);

            if (NavigationController is SLNavigationController)
            {
                var btnBack = (NavigationController as SLNavigationController).NavTitle.BtnBackOutlet;
                btnBack.TouchUpInside += (s, e) => { SL.RewardList = null; };
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            TableView.Source = new RewardsTableSource(this);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.AddRefreshControl();
            //TableView.ReloadData();
            TableView.ValueChanged += (s, e) =>
            {
                TableView.HideLoader();
            };
            RefreshRewards();
            TableView.ReloadData();
        }

        public override void ViewDidLayoutSubviews()
        {
            var iv = new UIImageView(UIImage.FromBundle("more-bg.jpg"));
            iv.AddSubview(new UIView(iv.Bounds) { BackgroundColor = UIColor.FromRGBA(255, 255, 255, 128), ContentMode = UIViewContentMode.ScaleToFill });
            iv.Frame = new CGRect(View.Frame.X, View.Frame.Y, View.Frame.Width, vHeight != 0 ? vHeight : vHeight = View.Frame.Height + UIApplication.SharedApplication.KeyWindow.SafeAreaInsets.Bottom);
            View.AddSubview(iv);
            View.SendSubviewToBack(iv);
        }

        async void RefreshChallenges()
        {
            await SL.Manager.GetChallengesAsync();
        }

        async Task RefreshProfile()
        {
            await SL.Manager.GetProfileAsync();
            AreaCollection.ReloadData();
            UpdateNavBar();
        }

        async void RefreshRewards()
        {
            SL.RewardList = null;
            TableView.ReloadData();

            TableView.ShowLoader();

            if (CategoryID > 0)
            {
                await SL.Manager.GetRewardsDrilldownAsync(CategoryID, OnRewardDrillDownCompleted);
            }
            else
                await SL.Manager.GetRewardsAsync();

            DidRefresh = true;

            TableView.HideLoader();

            TableView.ReloadData();
        }

        private void OnRewardDrillDownCompleted(RewardResponseModel model)
        {
            //if (ShouldDrillUpImmediately && model.RewardList != null && model.RewardList.Count == 0)
            //{
            //    UIStoryboard board = UIStoryboard.FromName("Rewards", null);
            //    var ctrl = board.InstantiateViewController("RewardCollectionViewController") as RewardCollectionViewController;

            //    SL.RewardList = null;
            //    ShouldDrillUpImmediately = false;

            //    UIViewController parentController = Platform.RootViewController;
            //    if (parentController?.GetType() == typeof(SLNavigationController))
            //    {
            //        ctrl.IsChangingAreasDisabled = false;
            //        var child = parentController.ChildViewControllers[0];
            //        (child as MainViewController).SwapInViewController(ctrl, (int)ENavigationTabs.REWARDS);
            //        return;
            //    }

            //    parentController = Platform.TopViewController;
            //    if (parentController?.GetType() == typeof(SLNavigationController))
            //    {
            //        var child = parentController.ChildViewControllers[0];
            //        ctrl.IsChangingAreasDisabled = false;
            //        (child as MainViewController).SwapInViewController(ctrl, (int)ENavigationTabs.REWARDS);
            //        return;
            //    }
            //}

            //ShouldDrillUpImmediately = true;
            
            TableView.ReloadData();
            //if (model?.RewardList?.Count == 1)
            //{
            //    RewardList = model.RewardList[0].ChildList;
            //    RewardsSubcategoryName = model.RewardList[0].Name;
            //    await SL.Manager.GetRewardsAsync();
            //    TableView.ReloadData();
            //}
        }


        async void RefreshFeed()
        {
            await SL.Manager.GetFeedAsync();
        }

        public async override Task Refresh()
        {
            if (IsCategoryPageSkiped)
            {
                UIViewController parentController = Platform.RootViewController;
                if (parentController?.GetType() == typeof(SLNavigationController))
                {
                    var child = parentController.ChildViewControllers[0];
                    //(child as MainViewController).SwapInViewController("Rewards", "RewardCollectionViewController", ENavigationTabs.REWARDS);
                    return;
                }
                return;
            }

            RefreshRewards();
            await RefreshProfile();
            await base.Refresh();

            //RefreshChallenges();
            //RefreshFeed();
        }
    }
}

