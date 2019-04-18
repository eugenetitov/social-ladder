using Foundation;
using SocialLadder.iOS.CustomControlls;
using System;
using UIKit;

namespace SocialLadder.iOS.Notifications
{
    public partial class NotificationsTableView : UITableView
    {
        public CustomRefreshControl CustomRefreshControl;

        public NotificationsTableView (IntPtr handle) : base (handle)
        {
        }

        public void AddRefreshControl()
        {
            CustomRefreshControl = new CustomRefreshControl();
            CustomRefreshControl.AddImage(UIImage.FromBundle("loading-indicator"));
            this.RefreshControl = CustomRefreshControl;
        }

        public void ShowLoader()
        {
            if (CustomRefreshControl == null)
            {
                this.RefreshControl.RemoveFromSuperview();
                AddRefreshControl();
            }
            this.SetContentOffset(new CoreGraphics.CGPoint(0, -CustomRefreshControl.Frame.Size.Height), true);
            CustomRefreshControl.BeginRefreshing();
        }

        public void HideLoader()
        {
            CustomRefreshControl.EndRefreshing();
        }
    }
}