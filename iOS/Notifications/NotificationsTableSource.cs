using UIKit;
using SocialLadder.Models;
using System.Collections.Generic;
using System;
using Foundation;
using CoreGraphics;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Services;

namespace SocialLadder.iOS.Notifications
{
    public class NotificationsTableSource<T> : UITableViewSource
    {
        List<ExpandableTableModel<T>> TableItems;
        private bool[] _isSectionOpen;
        private EventHandler _headerButtonCommand;
        public bool ItemInvoked;

        public NotificationsTableSource(List<ExpandableTableModel<T>> items, UITableView tableView)
        {
            TableItems = items;
            _isSectionOpen = new bool[items.Count];

            tableView.RegisterNibForCellReuse(UINib.FromName(NotificationsTableViewCell.Key, NSBundle.MainBundle), NotificationsTableViewCell.Key);
            tableView.RegisterNibForHeaderFooterViewReuse(UINib.FromName(NotificationsHeaderCell.Key, NSBundle.MainBundle), NotificationsHeaderCell.Key);

            _headerButtonCommand = (sender, e) =>
            {
                var button = sender as UIButton;
                var section = button.Tag;
                _isSectionOpen[(int)section] = !_isSectionOpen[(int)section];
                tableView.ReloadData();

                // Animate the section cells
                var paths = new NSIndexPath[RowsInSection(tableView, section)];
                for (int i = 0; i < paths.Length; i++)
                {
                    paths[i] = NSIndexPath.FromItemSection(i, section);
                }

                tableView.ReloadRows(paths, UITableViewRowAnimation.Automatic);
            };
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return TableItems.Count;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _isSectionOpen[(int)section] ? TableItems[(int)section].Count : 0;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return SizeConstants.ScreenMultiplier * 70f;
        }

        public override nfloat EstimatedHeightForHeader(UITableView tableView, nint section)
        {
            return SizeConstants.ScreenMultiplier * 70f;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            NotificationsHeaderCell header = tableView.DequeueReusableHeaderFooterView(NotificationsHeaderCell.Key) as NotificationsHeaderCell;
            var CurrentSectionTableModel = (ExpandableTableModel<T>)TableItems[(int)section];
            header.UpdateCellData(CurrentSectionTableModel);

            foreach (var view in header.Subviews)
            {
                if (view is HiddenHeaderButton)
                {
                    view.RemoveFromSuperview();
                }
            }
            var hiddenButton = CreateHiddenHeaderButton(header.Bounds, section);
            header.AddSubview(hiddenButton);
            header.BringSubviewToFront(hiddenButton);
            header.SetupView();
            return header;
        }

        private HiddenHeaderButton CreateHiddenHeaderButton(CGRect frame, nint tag)
        {
            var button = new HiddenHeaderButton(frame);
            button.Tag = tag;
            button.TouchUpInside += _headerButtonCommand;
            return button;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(NotificationsTableViewCell.Key, indexPath) as NotificationsTableViewCell;
            if (typeof(T) == typeof(NotificationItemModel))
            {
                var rowData = TableItems[indexPath.Section][indexPath.Row] as NotificationItemModel;
                if (rowData != null)
                {
                    cell.UpdateCellData(rowData);
                }
            }
            cell.SetupView();
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.CellAt(indexPath) as NotificationsTableViewCell;

            NotificationsTableView notificationsTable = tableView as NotificationsTableView;
            if (notificationsTable == null)
                return;
            
            var item = (TableItems[indexPath.Section][indexPath.Row] as NotificationItemModel)?.Item;
            tableView.DeselectRow(indexPath, false);
            if (item?.Action?.ActionScreen == null || item.Action.ActionScreen == "SHARE" && !(item.Action.ActionParamDict?.ContainsKey("ChallengeDetailURL") ?? false))
                return;

            ItemSelectedAction(item);
        }

        private void ItemSelectedAction(NotificationModel item)
        {
            if (ItemInvoked)
                return;

            ItemInvoked = true;

            if (item.AreaGUID != NotificationsViewController.DefaultAreaName)
                ActionHandlerService.SwitchAreaIfNeeded(item.AreaGUID);

            var notificationActionHandler = new ActionHandlerService();
            notificationActionHandler.HandleActionAsync(item.Action);
        }
    }

    public class HiddenHeaderButton : UIButton
    {
        public HiddenHeaderButton(CGRect frame) : base(frame)
        {

        }
    }

    public class ExpandableTableModel<T> : List<T>
    {
        public string Title { get; set; } 
        public UIImage Icon { get; set; }
        public string NotificationsCount { get; set; }

        public ExpandableTableModel(IEnumerable<T> collection) : base(collection) { }
        public ExpandableTableModel() : base() { }
    }

    public class NotificationItemModel
    {
        public string Message { get; set; }
        public UIImage Icon { get; set; }
        public NotificationModel Item { get; set; }
    }
}