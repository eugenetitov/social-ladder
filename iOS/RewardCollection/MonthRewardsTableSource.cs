using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using SocialLadder.Enums;
using SocialLadder.iOS.Rewards;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.RewardCollection
{
    public class MonthRewardsTableSource : UITableViewSource
    {
        private MonthRewardsViewController _controller;
        private string _headerText;

        public MonthRewardsTableSource(MonthRewardsViewController controller, string headerText)
        {
            _controller = controller;
            _headerText = headerText;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var list = SL.RewardList;
            RewardModel reward = list != null ? list[indexPath.Row] : null;
            var cell = (MonthRewardsTableCell)tableView.DequeueReusableCell(nameof(MonthRewardsTableCell)) as MonthRewardsTableCell;
            cell?.Update(reward);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            var list = SL.RewardList;
            return list == null ? 0 : list.Count;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var tableHeaderView = TableHeaderView.Create();
            tableHeaderView.UpdateText(_headerText, string.Empty);
            return tableHeaderView;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            RewardsTableView rewardsTable = tableView as RewardsTableView;
            if (rewardsTable != null)
            {
                var list = SL.RewardList;
                RewardModel rewardCategory = list != null ? list[indexPath.Row] : null;
                if (rewardCategory != null)
                    DrillDown(rewardCategory);
                    //DrillDown(rewardCategory);
                    //SL.Manager.GetRewardsDrilldownAsync(rewardCategory.ID, DrillDown);
            }
        }

        private async void DrillDown(RewardModel reward)
        {
            if (reward == null)
            {
                return;
            }
            if (reward.ChildList != null)
            {
                UIStoryboard board = UIStoryboard.FromName("Rewards", null);
                AvailableRewardsViewController ctrl = board.InstantiateViewController("RewardsViewController") as AvailableRewardsViewController;
                ctrl.RewardList = reward.ChildList;
                ctrl.HeaderTitle = _headerText;
                _controller.NavigationController.PushViewController(ctrl, true);
                return;
            }
            UIView overlay = Platform.AddOverlay(_controller.View.Frame, UIColor.White, true, true);
            await SL.Manager.GetRewardsDrilldownAsync(reward.ID, null);
            overlay.RemoveFromSuperview();
            _controller.RefreshRewards();    
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            var sizeOfHeader = (nfloat)(UIScreen.MainScreen.Bounds.Height / 16);
            return sizeOfHeader;
        }
    }
}