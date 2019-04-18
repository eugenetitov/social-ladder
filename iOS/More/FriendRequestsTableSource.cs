using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;

namespace SocialLadder.iOS.More
{
    public class TableOption
    {
        public TableOption(string text, string image, string date)
        {
            Text = text;
            Image = image;
            Date = date;
        }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
    }

    public class FriendRequestsTableSource : UITableViewSource
    {
        FriendRequestsViewController ViewController { get; set; }
        List<TableOption> FriendRequests { get; set; }
        float headerHeight { get; set; }


        public FriendRequestsTableSource(FriendRequestsViewController controller)
        {
            ViewController = controller;
            FriendRequests = new List<TableOption>();
            FriendRequests.Add(new TableOption("Taylor S. sent you a friend request.", "account-question-icon", "1m"));
            FriendRequests.Add(new TableOption("Gary O. sent you a friend request.", "account-settings-icon", "2d"));
            FriendRequests.Add(new TableOption("Nick J. sent you a friend request.", "account-docs-icon", "3w"));
            FriendRequests.Add(new TableOption("Taylor S. sent you a friend request.", "account-question-icon", "4m"));
            FriendRequests.Add(new TableOption("Gary O. sent you a friend request.", "account-settings-icon", "5d"));
            FriendRequests.Add(new TableOption("Nick J. sent you a friend request.", "account-docs-icon", "6w"));
            FriendRequests.Add(new TableOption("Taylor S. sent you a friend request.", "account-question-icon", "7m"));
            FriendRequests.Add(new TableOption("Gary O. sent you a friend request.", "account-question-icon", "8d"));
            FriendRequests.Add(new TableOption("Nick J. sent you a friend request.", "account-docs-icon", "9w"));
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return FriendRequests.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (FriendRequestsTableViewCell)tableView.DequeueReusableCell(FriendRequestsTableViewCell.ClassName);

            cell.UpdateCellData(FriendRequests[indexPath.Row]);

            if (indexPath.Row == FriendRequests.Count - 1)
            {
                cell.SeparatorView.BackgroundColor = UIColor.Clear;
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            //return base.GetViewForHeader(tableView, section);
            var topView = FriendRequestsTableHeaderView.Create();
            topView.TitleText = "3 Pending Friend Requests";
            topView.UpdateControlls();
            return topView;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return UIScreen.MainScreen.Bounds.Width * 0.148f;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
    }
}