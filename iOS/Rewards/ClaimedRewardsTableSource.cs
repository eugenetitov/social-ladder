using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.iOS.Rewards.Models;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public class ClaimedRewardsTableSource : UITableViewSource
    {
        ClaimedRewardsViewController RewardsViewController
        {
            get; set;
        }

        List<RewardItemModel> RewardList => RewardsViewController.RewardList ?? SL.RewardList;

        public ClaimedRewardsTableSource(ClaimedRewardsViewController rewardsViewController)
        {
            RewardsViewController = rewardsViewController;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var tableHeaderView = TableHeaderView.Create();
            //tableHeaderView.SetHeader("November", "Paul Van Dyke @ Webster");
            return tableHeaderView;
        }



        public override nint RowsInSection(UITableView tableview, nint section)
        {
            var list = RewardList;
            var monthGroup = RewardList.GroupBy(x => x.AvailabilityDate.Value.Month).Select(g => new
            {
                Key = g.Key,
                Count = g.Count()
            })
            .ToList()[(int)section].Count;
            return monthGroup;//list != null ? list.Count : 0;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            var sizeOfHeader = (nfloat)(tableView.Bounds.Width * 0.105);
            return sizeOfHeader;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var sizeOfRow = (nfloat)(tableView.Bounds.Width * 0.42);
            return sizeOfRow;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            var countOfSection = RewardList.GroupBy(x => x.AvailabilityDate.Value.Month).Count();
            return countOfSection;
            //return base.NumberOfSections(tableView);
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            var monthInt = RewardList.GroupBy(x => x.AvailabilityDate.Value.Month).Select(grp => grp.Key).ToList();
            DateTimeFormatInfo mfi = new DateTimeFormatInfo();
            List<string> monthStr = new List<string>();
            monthInt.ForEach(x => monthStr.Add(mfi.GetMonthName(x)));
            return string.Format("{0} | {1}", monthStr[(int)section], "Paul Van Dyke @ Webster");
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var list = RewardList;
            RewardItemModel reward = list != null ? list[indexPath.Row] : null;

            UITableViewCell cell;
            if (reward.Type == "REWARD")
            {
                cell = tableView.DequeueReusableCell(ClaimedRewardsTableCell.ClassName);
                IRewardTableCell rewardCell = cell as IRewardTableCell;
                if (rewardCell != null)
                {
                    rewardCell.UpdateCellData(reward, -10, RewardStatus.Claimed, false);
                }
            }
            else //if (reward.Type == "CATEGORY")
            {
                cell = tableView.DequeueReusableCell(ClaimedRewardsTableCell.ClassName);
                IRewardTableCell rewardCell = cell as IRewardTableCell;
                if (rewardCell != null)
                    rewardCell.UpdateCellData(reward);
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            RewardsTableView rewardTable = tableView as RewardsTableView;
            if (rewardTable != null)
            {
                var list = RewardList;
                RewardItemModel reward = list != null ? list[indexPath.Row] : null;
                if (reward != null)
                    DrillDown(reward);
            }
        }

        void DrillDown(RewardItemModel reward)
        {
            if (reward != null)
            {
                if (reward.Type == "REWARD")
                {
                    UIStoryboard board = UIStoryboard.FromName("Rewards", null);
                    RewardDetailViewController ctrl = (RewardDetailViewController)board.InstantiateViewController("RewardDetailViewController") as RewardDetailViewController;
                    ctrl.Reward = reward;
                    RewardsViewController.NavigationController.PushViewController(ctrl, true);
                }
                else if (reward.Type == "CATEGORY")
                {
                    UIStoryboard board = UIStoryboard.FromName("Rewards", null);
                    RewardsViewController ctrl = (RewardsViewController)board.InstantiateViewController("RewardsViewController") as RewardsViewController;
                    if (reward.ChildList != null)
                        ctrl.RewardList = reward.ChildList;
                    else
                    {
                        ctrl.CategoryID = reward.ID;
                        //SL.RewardList = null;
                    }
                    RewardsViewController.NavigationController.PushViewController(ctrl, true);
                }
            }
        }
    }
}