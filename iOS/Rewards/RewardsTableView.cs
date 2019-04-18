using Foundation;
using SocialLadder.iOS.CustomControlls;
using System;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class RewardsTableView : UITableView
    {
        private CustomRefreshControl _customRefreshControl;

        public event EventHandler ValueChanged;

        public void AddRefreshControl()
        {
            _customRefreshControl = new CustomRefreshControl();
            this.RefreshControl = _customRefreshControl;
            _customRefreshControl.AddImage(UIImage.FromBundle("loading-indicator"));
            _customRefreshControl.ValueChanged += (s, e) =>
            {
                ValueChanged?.Invoke(s,e);
            };
        }

        public void ShowLoader()
        {
            this.SetContentOffset(new CoreGraphics.CGPoint(0, -_customRefreshControl.Frame.Size.Height), true);
            _customRefreshControl.BeginRefreshing();
        }

        public void HideLoader()
        {
            _customRefreshControl.EndRefreshing();
        }

        public RewardsTableView (IntPtr handle) : base (handle)
        {
            BackgroundColor = UIColor.Clear;
        }
    }
}