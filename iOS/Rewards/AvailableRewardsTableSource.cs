using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Foundation;
using ObjCRuntime;
using SocialLadder.iOS.Rewards.Models;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public class AvailableRewardsTableSource : UITableViewSource
    {
        AvailableRewardsViewController RewardsViewController
        {
            get; set;
        }

        private string _headerText;
        private string _subHeaderText;

        List<RewardItemModel> RewardList => RewardsViewController.RewardList != null ? RewardsViewController.RewardList : SL.RewardList;

        public AvailableRewardsTableSource(AvailableRewardsViewController rewardsViewController, string headerText = null, string subHeaderText = null)
        {
            RewardsViewController = rewardsViewController;
            _headerText = headerText;
            _subHeaderText = subHeaderText;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var tableHeaderView = TableHeaderView.Create();
            tableHeaderView.UpdateText(_headerText, _subHeaderText);
            return tableHeaderView;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return RewardList != null ? RewardList.Count : 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            var countOfSection =  RewardList.GroupBy(x => x.AvailabilityDate.GetValueOrDefault().Month).Count();
            return countOfSection;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            var sizeOfHeader = (nfloat)(UIScreen.MainScreen.Bounds.Height / 16);
            return sizeOfHeader;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var sizeOfRow = (nfloat)(tableView.Bounds.Width * 0.42);
            return sizeOfRow;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var list = RewardList;
            RewardItemModel reward = list != null ? list[indexPath.Row] : null;

            UITableViewCell cell;
            if (reward.Type == "REWARD")
            {
                cell = tableView.DequeueReusableCell(AvailableRewardsTableCell.ClassName);
                IRewardTableCell rewardCell = cell as IRewardTableCell;
                if (rewardCell != null)
                {
                    rewardCell.UpdateCellData(reward, -10, RewardStatus.Aviable, false);
                }
            }
            else 
            {
                cell = tableView.DequeueReusableCell(RewardCategoryTableViewCell.ClassName);
                RewardCategoryTableViewCell rewardCell = cell as RewardCategoryTableViewCell;
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
                    }
                    RewardsViewController.NavigationController.PushViewController(ctrl, true);
                }
            }
        }
    }
}