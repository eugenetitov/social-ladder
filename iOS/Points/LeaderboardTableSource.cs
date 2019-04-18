using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;
using System.Linq;

namespace SocialLadder.iOS.Points
{
    public class LeaderboardTableSource : UITableViewSource
    {
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;
        private float _leaderboardCellAspectRatio = 100f / 777f;

        public Action<int> OnRowSelected { get; set; }
        //count of items which we can display
        // 0 means display all

        private int _countItemsToDisplay;
        public int CountItemsToDisplay
        {
            get => _countItemsToDisplay;
            set => _countItemsToDisplay = value;
        }

        public float MaxScoreValue
        {
            get; set;
        }

        public LeaderboardTableSource()
        {
            MaxScoreValue = 1;
            var list = SL.FriendList;
            if (list != null)
            {
                CountItemsToDisplay = list.Count;
            } 
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }
        
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return CountItemsToDisplay;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var list = SL.FriendList;
            FriendModel friend = (list != null && list.Count > indexPath.Row) ? list[indexPath.Row] : null;

            LeaderboardTableViewCell cell = (LeaderboardTableViewCell)tableView.DequeueReusableCell(LeaderboardTableViewCell.ClassName);

            if (indexPath.Row == (CountItemsToDisplay - 1))
            {
                int position = list.IndexOf(list.FirstOrDefault(x => x.Name.Equals("Me")));
                if (position > (CountItemsToDisplay - 1))
                {
                    friend = list[position];
                    cell.TopSeparatorView.Hidden = false;
                }
            }

            if (friend == null)
            {
                //list doesnt match indexpath reload since data prob refreshed before finished building table
                tableView.ReloadData();
                return cell;
            }

            //if ((CountItemsToDisplay != 0) && (CountItemsToDisplay > list.Count) && (indexPath.Row == --CountItemsToDisplay) && (list.FindIndex(x=>x.Name == "Me")> CountItemsToDisplay))
            //{
            //    friend = list.Where(x => x.Name == "Me").First();
            //    cell.UpdateCellData(friend, (float)(friend.Score / MaxScoreValue), true);
            //    return cell;
            //}
            float scoreValue = (float)(friend.Score / MaxScoreValue);
            cell.UpdateCellData(friend, scoreValue);
             
            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return _leaderboardCellAspectRatio * _screenWidth;

        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var friends = SL.FriendList;
            FriendModel friend = (friends != null && friends.Count > indexPath.Row) ? friends[indexPath.Row] : null;
            if (friend == null)
            {
                //alert couldn't find this friend on device
                return;
            }

            //int.TryParse(friend.SCSUserProfileID, out int profileId);

            OnRowSelected?.Invoke(friend.SCSUserProfileID);//784167
        }
    }
}