using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public class LeaderboardFilterTableSource : UITableViewSource
    {
        public List<string> ItemsSource { get; set; }

        private int _selecteditem;
        public int SelectedItem
        {
            get
            {
                return _selecteditem;
            }
            set
            {
                _selecteditem = value;
                _tableView.ReloadData();
            }
        }
        public UIView ParentView { get; set; }


        private UITableView _tableView;

        public LeaderboardFilterTableSource(UITableView tableView, UIView parentView)
        {
            tableView.RegisterNibForCellReuse(LeaderboardFilterTableCell.Nib, LeaderboardFilterTableCell.Key);

            _tableView = tableView;

            ParentView = parentView;

            ItemsSource = new List<string>
            {
                "Overall Leaderboard",
                "Friends Leaderboard",
                "My Ranking Leaderboard"
            };
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            LeaderboardFilterTableCell cell = tableView.DequeueReusableCell(LeaderboardFilterTableCell.Key) as LeaderboardFilterTableCell;

            if (cell == null)
            {
                return new UITableViewCell();
            }

            if (indexPath.Row == SelectedItem)
            {
                cell.Selected = true;
            }
            cell.UpdateCell(ItemsSource[indexPath.Row]);

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            LeaderboardFilterTableCell cell = tableView.DequeueReusableCell(LeaderboardFilterTableCell.Key) as LeaderboardFilterTableCell;
            cell.Selected = true;
            cell.UpdateCell(ItemsSource[indexPath.Row]);
            SelectedItem = indexPath.Row;
            ParentView.Hidden = true;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ItemsSource == null ? 0 : ItemsSource.Count;
        }
    }
}