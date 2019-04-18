using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.More
{
    public partial class FriendRequestsViewController : UIViewController
    {
        public FriendRequestsViewController(IntPtr handle) : base(handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.RegisterNibForCellReuse(FriendRequestsTableViewCell.Nib, FriendRequestsTableViewCell.ClassName);
        }
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            TableView.Source = new FriendRequestsTableSource(this);
            //TableView.SectionHeaderHeight = UITableView.AutomaticDimension;

            //TableView.RowHeight = UITableView.AutomaticDimension;
            //TableView.ReloadData();
            //TableView.SetNeedsLayout();
            //TableView.LayoutIfNeeded();
        }
    }
}