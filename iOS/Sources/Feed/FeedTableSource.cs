using UIKit;
using SocialLadder.Models;
using System;
using Foundation;
using SocialLadder.iOS.Navigation;
using CoreGraphics;
using CoreAnimation;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.UserInfo;
using MvvmCross.Binding.iOS.Views;
using SocialLadder.iOS.Views;
using SocialLadder.iOS.Views.Cells;
using SocialLadder.iOS.ViewControllers.Feed;
using System.Collections.Generic;
using System.Linq;
using SocialLadder.Enums;

namespace SocialLadder.iOS.Sources.Feed
{
    public class FeedTableSource : MvxTableViewSource 
    {
        private UIViewController _feedViewController;
        private bool _isProfileSpinnerDisplayed;

        public event Action OnLoadNextPage;
        public UIView FooterView;

        #region Ctors

        public FeedTableSource(UITableView tableView) : base(tableView)
        {
            FooterView = GetViewForFooter();
            FooterView.Hidden = true;
            _isProfileSpinnerDisplayed = false;
        }

        public FeedTableSource(UITableView tableView, UIViewController viewController) : base(tableView)
        {
            _feedViewController = viewController;
            FooterView = GetViewForFooter();
            FooterView.Hidden = true;
            _isProfileSpinnerDisplayed = false;
        }
        #endregion
        
        #region lifecycle

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
        {
            FeedTableViewCell cell = (FeedTableViewCell)tableView.DequeueReusableCell("FeedTableViewCell", indexPath);
            var feedItem = item as FeedItemModel;
            if (feedItem != null)
            {
                cell.Source = this;
                cell.SelectionStyle = UITableViewCellSelectionStyle.None;
                cell.Reset();
                cell.OnLoadUserProfile = DisplayProfileFeed;
                cell.UpdateCellData(feedItem);

                //TO DO Move this check to view model
                if (Platform.IsInternetConnectionAvailable() == false)
                {
                    return cell;
                }

                if (!((FeedTableView)(tableView)).IsLoading && (indexPath.Row >= SL.Feed.FeedPage.FeedItemList.Count - 1))
                {
                    FooterView.Hidden = false;
                    OnLoadNextPage();
                }
            }
            else
            {
                tableView.ReloadData();
            }

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            if (string.IsNullOrEmpty(SL.FeedNextPage))
            {
                return 0;
            }
            return 120.0f;
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            return FooterView;
        }

        public override void WillDisplayFooterView(UITableView tableView, UIView footerView, nint section)
        {
            FooterView.RemoveFromSuperview();
            FooterView.Hidden = true;
        }

        public override void FooterViewDisplayingEnded(UITableView tableView, UIView footerView, nint section)
        {
            FooterView = GetViewForFooter();
        }

        #endregion

        private UIView GetViewForFooter()
        {
            var view = new UIView(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 120));
            var spinner = new UIImageView();
            spinner.Image = UIImage.FromBundle("loading-indicator");
            var rotationAnimation = new CABasicAnimation
            {
                KeyPath = "transform.rotation.z",
                To = new NSNumber(Math.PI * 2),
                Duration = 0.7,
                Cumulative = true,
                RepeatCount = float.MaxValue
            };

            spinner.Layer.AddAnimation(rotationAnimation, "rotationAnimation");
            spinner.TranslatesAutoresizingMaskIntoConstraints = false;
            view.AddSubview(spinner);

            view.AddConstraint(FeedConstraints.FeedFooterViewHeight(spinner, view));
            view.AddConstraint(FeedConstraints.FeedFooterViewWidth(spinner));
            view.AddConstraint(FeedConstraints.FeedFooterViewX(spinner, view));
            //view.AddConstraint(FeedConstraints.FeedFooterViewY(spinner, view));
            view.AddConstraint(FeedConstraints.FeedFooterTopToView(spinner, view));

            return view;
        }

        public void SetSpinnerToImg(UIImageView view)
        {
            if (!_isProfileSpinnerDisplayed)
            {
                view.Image = UIImage.FromBundle("loading-indicator");
                Platform.AnimateRotation(view);
                _isProfileSpinnerDisplayed = true;
            }
        }

        private void DisplayProfileFeed(string url, string profileUrl = null)
        {
            if (_feedViewController is UserInfoViewController)
            {
                return;
            }

            if (((FeedViewController)_feedViewController).IsModal == true)
            {
                return;
            }
            UIStoryboard storyboard = UIStoryboard.FromName("UserInfo", null);
            UserInfoViewController viewController = storyboard.InstantiateViewController("UserInfoViewController") as UserInfoViewController;
            viewController.ShouldGetProfileByFriendId = false;
            viewController.ProfileUrl = profileUrl ?? string.Empty;
            viewController.FeedUrl = url;

            UIViewController parentController = Platform.TopViewController;
            parentController.PresentViewController(viewController, false, null);
        }

        #region ReloadComments
        //private void AddNewComments(FeedItemModel listItem, string comment)
        //{
        //    var row = Items.IndexOf(listItem);
        //    if (listItem.FilteredEngagementList == null)
        //    {
        //        listItem.FilteredEngagementList = new List<FeedEngagementModel>();
        //    }
        //    //Must add correct comment to local storage and current collection
        //    var autor = SL.Profile.UserName; //listItem.Header.Actor + " " + listItem.Header.ActionText == string.Empty ? "author" : listItem.Header.Actor + " " + listItem.Header.ActionText;
        //    listItem.FilteredEngagementList.Add(new FeedEngagementModel { EngagementType = "COMMENT", FeedTextQuoteID = 5, ID = 5, Notes = comment, UserName = autor });

        //    if (!listItem.LayoutSections.Contains(Enums.FeedContentType.Engagement))
        //    {
        //        var list = listItem.LayoutSections.ToList();
        //        list.Add(FeedContentType.Engagement);
        //        listItem.LayoutSections = list.ToArray();
        //    }
        //}

        public void UpdateComments(FeedItemModel item, string comment = "")
        {
            //{
            //    var listItem = ItemsSource?.Where(x => x.ID == item.ID).FirstOrDefault();
            //    if (listItem != null)
            //    {
            //        if (!string.IsNullOrEmpty(comment))
            //        {
            //            AddNewComments(listItem, comment);
            //        }
            //        listItem.TextQuote.IsModified = true;
            //        //FeedTableView.ShowLoader();
            //        //UpdateCommentView(item);
            //        //FeedTableView.HideLoader();
            //    }
            //}
        }

        //public void UpdateCommentView(FeedItemModel item)
        //{
        //    if (ItemsSource != null)
        //    {
        //        var row = ItemsSource.IndexOf(item);
        //        var index = NSIndexPath.FromRowSection(row, 0);
        //        FeedItemModel feedItem = ItemsSource[row];
        //        string cellId = CellType(feedItem);
        //        FeedBaseTableViewCell cell = (FeedBaseTableViewCell)FeedTableView.DequeueReusableCell(cellId, index);
        //        FeedTableView.ReloadRows(new NSIndexPath[] { index }, UITableViewRowAnimation.Automatic);
        //    }

        //    FeedTableView.SetNeedsUpdateConstraints();
        //    FeedTableView.LayoutIfNeeded();
        //}

        //public void UnhideCommentsInCell(int row)
        //{
        //    if (ItemsSource != null)
        //    {
        //        var index = NSIndexPath.FromRowSection(row, 0);
        //        FeedItemModel feedItem = ItemsSource[row];
        //        if (feedItem.FilteredEngagementList == null)
        //        {
        //            return;
        //        }
        //        string cellId = CellType(feedItem);
        //        FeedTableViewCell cell = (FeedTableViewCell)FeedTableView.DequeueReusableCell(cellId, index);

        //        cell.ReadAllComments(feedItem);
        //        UpdateComments(feedItem);
        //        UpdateCommentView(feedItem);
        //    }
        //}
        #endregion

        //SLViewController SLViewController
        //{
        //    get; set;
        //}

        //public override void DecelerationStarted(UIScrollView scrollView)
        //{
        //    SLViewController?.HideAreaCollection();
        //}

        //public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        //{
        //    try
        //    {
        //        FeedItemModel feedItem = null;
        //        if (ItemsSource != null)
        //        {
        //            feedItem = ItemsSource[indexPath.Row];
        //            if (feedItem.TextQuote == null)
        //            {
        //                feedItem.TextQuote = new FeedTextQuote() { EngagementList = new List<FeedEngagementModel>() };
        //            }
        //        }
        //        string cellId = CellType(feedItem);

        //        FeedTableViewCell cell = (FeedTableViewCell)tableView.DequeueReusableCell(cellId, indexPath);
        //        cell.SelectionStyle = UITableViewCellSelectionStyle.None;
        //        cell.Reset();

        //        if (feedItem != null)
        //        {

        //            cell.FeedTableView = tableView as FeedTableView;
        //            cell.Source = this;
        //            cell.UpdateCellData(feedItem, SLViewController, _feedViewController);
        //            Console.WriteLine("feedItem ----------------------" + feedItem.ID);
        //            cell.OnLoadUserProfile = DisplayProfileFeed;

        //            if (Platform.IsInternetConnectionAvailable() == false)
        //            {
        //                return cell;
        //            }

        //            if (!FeedTableView.IsLoading && (SL.Feed != null && SL.Feed.FeedPage != null && indexPath.Row >= SL.Feed.FeedPage.FeedItemList.Count - 1))
        //            {
        //                Footer.Hidden = false;
        //                FeedTableView.LoadMore();

        //            }
        //        }
        //        else
        //            tableView.ReloadData(); //indexpath.row does not match our dataset size so need to reload (most likely feed updated before finishing laying out the cells

        //        return cell;
        //    }
        //    catch
        //    {
        //        return new UITableViewCell();
        //    }

        //}
    }
}