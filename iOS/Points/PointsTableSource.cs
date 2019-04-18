using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;
using SocialLadder.Mocks;
using CoreGraphics;

namespace SocialLadder.iOS.Points
{
    public class PointsTableSource : UITableViewSource
    {
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;
        private float _challengeCellAspectRatio = 69f / 414f;
        private float _challengeTypeCellAspectRatio = 69f / 414f;
        private float _rewardCellAspectRatio = 69f / 414f;

        public List<SummaryModel> ItemsSource { get; set; }

        public PointsTableSource()
        {

            ItemsSource = SL.SummaryList;//MockGenerator.GetSummaryModels();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ItemsSource == null ? 0 : ItemsSource.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            PointsTableViewCell cell = (PointsTableViewCell)tableView.DequeueReusableCell(PointsTableViewCell.ClassName);

            if (cell == null)
            {
                return new UITableViewCell();
            }

            //CGRect frame = cell.Frame;
            //frame.Width = tableView.Frame.Width;
            //frame.Height = _challengeCellAspectRatio * frame.Width;
            //cell.Frame = frame;

            if (ItemsSource != null && indexPath.Row < ItemsSource.Count)
                cell.UpdateCellData(ItemsSource[indexPath.Row]);
            else
                tableView.ReloadData(); //index does not match dataset; reload

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            CGRect frame = tableView.Frame;

            if (ItemsSource[indexPath.Row] is ChallengeSummaryModel)
            {
                return _challengeCellAspectRatio * _screenWidth;
            }
            if (ItemsSource[indexPath.Row] is ChallengeTypeSummaryModel)
            {
                return _challengeTypeCellAspectRatio * _screenWidth;
            }
            if (ItemsSource[indexPath.Row] is RewardSummaryModel)
            {
                return _rewardCellAspectRatio * _screenWidth;
            }

            return 128;
        }

        //public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        //{
        //    CGRect frame = tableView.Frame;

        //    if (ItemsSource[indexPath.Row] is ChallengeSummaryModel)
        //    {
        //        return _challengeCellAspectRatio * frame.Width;
        //    }
        //    if (ItemsSource[indexPath.Row] is ChallengeTypeSummaryModel)
        //    {
        //        return _challengeTypeCellAspectRatio * frame.Width;
        //    }
        //    if (ItemsSource[indexPath.Row] is RewardSummaryModel)
        //    {
        //        return _rewardCellAspectRatio * frame.Width;
        //    }

        //    return 128;
        //}
    }
}