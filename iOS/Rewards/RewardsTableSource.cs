using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;

namespace SocialLadder.iOS.Rewards
{
    public class RewardsTableSource : UITableViewSource
    {
        RewardsViewController RewardsViewController { get; set; }

        List<RewardItemModel> RewardList => RewardsViewController.RewardList ?? SL.RewardList;

        public RewardsTableSource(RewardsViewController rewardsViewController)
        {
            RewardsViewController = rewardsViewController;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            var list = RewardList;
            return list != null ? list.Count : 0;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            var sizeOfHeader = (nfloat)(UIScreen.MainScreen.Bounds.Width / 10);
            return sizeOfHeader;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var list = RewardList;
            RewardItemModel reward = list != null ? list[indexPath.Row] : null;
            nfloat heightForRow;
            if (reward != null && reward.Type == "REWARD")
            {
                heightForRow = tableView.Bounds.Width * 0.43f;
            }
            else
            {
                heightForRow = tableView.Bounds.Width * 0.915f;
            }
            return heightForRow;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var tableHeaderView = TableHeaderView.Create();
            var list = RewardList;
            RewardItemModel reward = (list != null && (list.Count > 0)) ? list[0] : null;
            if (reward == null)
            {
                return tableHeaderView;
            }
            if (reward?.Type == "REWARD")
            {
                tableHeaderView.UpdateText(RewardsViewController.RewardsCategoryName, RewardsViewController.RewardsSubcategoryName);
            }
            else
            {
                tableHeaderView.UpdateText(RewardsViewController.RewardsCategoryName, RewardsViewController.RewardsSubcategoryName);
            }
            return tableHeaderView;
        }

        

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var list = RewardList;
            RewardItemModel reward = list != null ? list[indexPath.Row] : null;

            UITableViewCell cell;
            if (reward != null && reward.Type == "REWARD")
            {
                cell = tableView.DequeueReusableCell(RewardsTableViewCell.ClassName);
                RewardsTableViewCell rewardCell = cell as RewardsTableViewCell;
                cell.Frame = new CoreGraphics.CGRect(cell.Frame.X, cell.Frame.Y, tableView.Frame.Width, cell.Frame.Height);
                cell.UpdateConstraintsIfNeeded();
                if (rewardCell != null)
                {
                    int itemIndex = list.IndexOf(reward);

                    nfloat rewardLittleOffset = UIScreen.MainScreen.Bounds.Width / 100 * 2.17f;
                    nfloat rewardBigOffset = UIScreen.MainScreen.Bounds.Width / 100 * 7.73f;

                    bool noPoints = reward.MinScore > SL.Profile.Score;
                    bool locked = (reward.AutoUnlockDate?.ToLocalTime() ?? DateTime.Now) > DateTime.Now;

                    rewardCell.PrepareForReuse();
                    if (noPoints && locked)
                    {
                        rewardCell.UpdateCellData(reward, rewardLittleOffset, false);
                    }
                    else if (noPoints && !locked)
                    {
                        rewardCell.UpdateCellData(reward, rewardBigOffset, false);

                    }
                    else if (!noPoints && locked)
                    {
                        rewardCell.UpdateCellData(reward, -rewardBigOffset, true);
                    }
                    else if (!noPoints && !locked)//Available
                    {
                        rewardCell.UpdateCellData(reward, -rewardLittleOffset, true);
                    }
                }
                

            }
            else
            {
                cell = tableView.DequeueReusableCell(RewardCategoryTableViewCell.ClassName);
                RewardCategoryTableViewCell rewardCell = cell as RewardCategoryTableViewCell;
                rewardCell.SelectionStyle = UITableViewCellSelectionStyle.None;
                if (reward != null && rewardCell != null)
                {
                    rewardCell.UpdateCellData(reward);
                }
            }
            return cell;
        }
        
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }
            RewardsTableView rewardTable = tableView as RewardsTableView;
            if (rewardTable != null)
            {
                var list = RewardList;
                RewardItemModel reward = list != null ? list[indexPath.Row] : null;
                if (reward != null)
                    DrillDown(reward);
            }
        }

        private void DrillDown(RewardItemModel reward)
        {
            if (reward != null)
            {
                if (reward.Type == "REWARD")
                {
                    UIStoryboard board = UIStoryboard.FromName("Rewards", null);
                    RewardDetailViewController ctrl = (RewardDetailViewController)board.InstantiateViewController("RewardDetailViewController") as RewardDetailViewController;
                    ctrl.Reward = reward;
                    ctrl.Subtitle = string.Format("{0} | {1}",RewardsViewController.RewardsCategoryName, RewardsViewController.RewardsSubcategoryName);
                    RewardsViewController.NavigationController.PushViewController(ctrl, true);
                }
                else if (reward.Type == "CATEGORY")
                {
                    UIStoryboard board = UIStoryboard.FromName("Rewards", null);
                    RewardsViewController ctrl = (RewardsViewController)board.InstantiateViewController("RewardsViewController") as RewardsViewController;
                    ctrl.RewardsCategoryName = RewardsViewController.RewardsCategoryName;
                    ctrl.RewardsSubcategoryName = reward.Name;
                    if (reward.ChildList != null)
                        ctrl.RewardList = reward.ChildList;
                    else
                    {
                        ctrl.CategoryID = reward.ID;
                        SL.RewardList = null;
                    }
                    RewardsViewController.NavigationController.PushViewController(ctrl, true);
                }
            }
        }

    }
}