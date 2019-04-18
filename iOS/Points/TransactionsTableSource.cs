using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;
using CoreGraphics;
using System.Linq;

namespace SocialLadder.iOS.Points
{
    public class TransactionsTableSource : UITableViewSource
    {
        //private bool _isNextPageAvailable;

        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;
        private float _cellAspectRatio = 52f / 414f;
        private UITableView _transactionsTable;

        UIImageView ScoreImage { get; set; } //used to align the custom time line drawing

        //public Action OnLoadMoreCompleted;
        public Action NeedsLoadMore;

        public TransactionsTableSource(UITableView transactionsTable, UIImageView scoreImage)
        {
            _transactionsTable = transactionsTable;
            ScoreImage = scoreImage;

            _transactionsTable.RegisterNibForCellReuse(TransactionsTableViewCell.Nib, TransactionsTableViewCell.ClassName);
            _transactionsTable.RowHeight = UITableView.AutomaticDimension;
            _transactionsTable.EstimatedRowHeight = _cellAspectRatio * _screenWidth;

        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            //var transactionList = SL.TransactionList;
            //return transactionList != null ? transactionList.Count : 0;

            //_isNextPageAvailable = !String.IsNullOrEmpty(SL.TransactionPages?.NextPage);

            if (SL.TransactionPages?.TransactionList == null)
                return 0;

            return SL.TransactionPages.TransactionList.Count;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            //TransactionsTableViewCell cell = tableView.DequeueReusableCell(TransactionsTableViewCell.ClassName) as TransactionsTableViewCell;

            //var transactionList = SL.TransactionList;
            //var transaction = transactionList != null && indexPath.Row < transactionList.Count ? transactionList[indexPath.Row] : null;

            //if (transaction != null)
            //{
            //    cell.StringLength = 64 * (indexPath.Row + 1);

            //    cell.ScoreImage = ScoreImage;
            //    cell.UpdateCellData(transaction);
            //}
            //else
            //    tableView.ReloadData(); //expected data set size does not match; most likely data was refreshed so reload tableview

            //return cell;

            TransactionsTableViewCell cell = tableView.DequeueReusableCell(TransactionsTableViewCell.ClassName) as TransactionsTableViewCell;
            var transactionPage = SL.TransactionPages;
            var transactionList = transactionPage?.TransactionList;
            if (transactionList == null || transactionList.Count == 0)
            {
                return cell;
            }

            var transaction = transactionList[indexPath.Row];
            cell.StringLength = 64 * (indexPath.Row + 1);
            cell.ScoreImage = ScoreImage;
            cell.UpdateCellData(transaction);

            //bool isVisible = tableView.IndexPathsForVisibleRows.Contains(indexPath);
            //_isNextPageAvailable && 
            if (indexPath.Row >= transactionPage.TransactionList.Count - 1)
            {
                NeedsLoadMore?.Invoke();
                //LoadMore(transactionPage.NextPage);
            }

            return cell;
        }

        //private void LoadMore(string nextPageUrl)
        //{
        //    try
        //    {
        //        SL.Manager.GetNextTransactionsPageByUrlAsync(nextPageUrl, LoadMoreCompleted);
        //    }
        //    catch (Exception ex)
        //    {
        //        ;
        //    }
        //}

        //private void LoadMoreCompleted(TransactionResponseModel transactionResponse)
        //{
        //    //transactionResponse.ResponseCode != 1 || 
        //    if (String.IsNullOrEmpty(transactionResponse?.TransactionPage?.NextPage))
        //    {
        //        _isNextPageAvailable = false;
        //    }
        //    _transactionsTable?.ReloadData(); //expected data set size does not match; most likely data was refreshed so reload tableview

        //    OnLoadMoreCompleted?.Invoke();
        //}
    }
}