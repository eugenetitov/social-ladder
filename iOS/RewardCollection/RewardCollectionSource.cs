using System;
using Foundation;
using UIKit;
using SocialLadder.Models;
using SocialLadder.iOS.Rewards;
using SocialLadder.iOS.Navigation;

namespace SocialLadder.iOS.RewardCollection
{
    public class RewardCollectionSource : UICollectionViewSource
    {
        RewardCollectionViewController RewardCollectionViewController { get; set; }


        public RewardCollectionSource(RewardCollectionViewController rewardCollection)
        {
            RewardCollectionViewController = rewardCollection;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override void DecelerationStarted(UIScrollView scrollView)
        {
            RewardCollectionViewController.HideAreaCollection();
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint index)
        {
           
            if (RewardCollectionViewController.IsContentShouldBeCleared)
            {
                return 0;
            }
            var list = SL.RewardList;
            return list != null ? list.Count : 0;
        }

        public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var list = SL.RewardList;
            RewardModel reward = list?[indexPath.Row] ?? null;
            
            var cell = (RewardCollectionViewCell)collectionView.DequeueReusableCell(RewardCollectionViewCell.ClassName, indexPath);

            if (reward != null)
            {
                cell.UpdateCellData(reward);
            }
            return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var list = SL.RewardList;
            RewardModel rewardCategory = list != null ? list[indexPath.Row] : null;
            if (rewardCategory != null)
                DrillDown(rewardCategory);
        }

        private void DrillDown(RewardModel reward)
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }
            if (reward != null)
            {
                UIStoryboard board = UIStoryboard.FromName("Rewards", null);
                RewardsViewController ctrl = (RewardsViewController)board.InstantiateViewController("RewardsViewController") as RewardsViewController;
                ctrl.RewardsCategoryName = reward.Name;
                if (reward.ChildList != null)
                    ctrl.RewardList = reward.ChildList;
                else
                {
                    ctrl.CategoryID = reward.ID;
                    SL.RewardList = null;
                }
                RewardCollectionViewController.ShouldDrillDownImmediately = false;
                RewardCollectionViewController.NavigationController.PushViewController(ctrl, true);
            }
        }


        private async void DrillDown(RewardModel reward, UICollectionView collectionView)
        {
            if (reward != null)
            {
                UIStoryboard board = UIStoryboard.FromName("Rewards", null);
                MonthRewardsViewController ctrl = board.InstantiateViewController("RewardsViewController") as MonthRewardsViewController;
                ctrl.CurrentPage = RewardCollectionViewController.CurrentPage;
                ctrl.HeaderTitle = reward.Name;
                if (reward.ChildList != null)
                    ctrl.RewardList = reward.ChildList;
                else
                {
                    ctrl.CategoryID = reward.ID;
                }
                var navbarHeight = RewardCollectionViewController.NavigationController.NavigationBar.Frame.Height;
                var overlayRect = new CoreGraphics.CGRect(0, 0, RewardCollectionViewController.View.Frame.Width, UIScreen.MainScreen.Bounds.Height - navbarHeight);
                UIView overlay = Platform.AddOverlay(overlayRect, UIColor.White, true, true);
                await SL.Manager.GetRewardsDrilldownAsync(reward.ID, null);
                overlay.RemoveFromSuperview();
                RewardCollectionViewController.NavigationController.PushViewController(ctrl, true);
            }
        }
    }
}