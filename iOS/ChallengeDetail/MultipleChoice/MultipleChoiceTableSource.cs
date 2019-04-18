using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;

namespace SocialLadder.iOS.Challenges
{

    public class MultipleChoiceTableSource : UITableViewSource
    {
        //string cellIdentifier = "cell";
        MultipleChoiceViewController MultipleChoiceViewController { get; set; }
        
        public MultipleChoiceTableSource(MultipleChoiceViewController viewController)
        {
            MultipleChoiceViewController = viewController;
        }

        ChallengeModel Challenge
        {
            get
            {
                return MultipleChoiceViewController != null ? MultipleChoiceViewController.Challenge : null;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Challenge != null && Challenge.AnswerList != null ? Challenge.AnswerList.Count : 0;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var list = Challenge != null ? Challenge.AnswerList : null;
            ChallengeAnswerModel answer = list != null ? list[indexPath.Row] : null;

            MultipleChoiceTableViewCell cell = (MultipleChoiceTableViewCell)tableView.DequeueReusableCell(MultipleChoiceTableViewCell.ClassName);

            MultipleChoiceTableView table = tableView as MultipleChoiceTableView;
            cell.UpdateCellData(answer, table.SelectedRow == indexPath.Row);
            return cell;
        }

        //public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        //{
        //    var list = Challenge != null ? Challenge.AnswerList : null;
        //    ChallengeAnswerModel answer = list != null ? list[indexPath.Row] : null;

        //    MultipleChoiceTableViewCell cell = (MultipleChoiceTableViewCell)tableView.DequeueReusableCell(MultipleChoiceTableViewCell.ClassName);

        //    MultipleChoiceTableView table = tableView as MultipleChoiceTableView;

        //    return cell.ImageView.Frame.Height;
        //    //return base.GetHeightForRow(tableView, indexPath);
        //}

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            MultipleChoiceTableView table = tableView as MultipleChoiceTableView;
            table.SelectedRow = indexPath.Row;
            table.ReloadData();
        }
    }
}