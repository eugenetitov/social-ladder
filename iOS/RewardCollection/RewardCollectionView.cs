using Foundation;
using SocialLadder.iOS.CustomControlls;
using System;
using UIKit;

namespace SocialLadder.iOS
{
    public partial class RewardCollectionView : UICollectionView
    {

        public CustomRefreshControl CustomRefreshControl;

        public RewardCollectionView (IntPtr handle) : base (handle)
        {
        }

        public void AddRefreshControl()
        {
            CustomRefreshControl = new CustomRefreshControl();
            this.RefreshControl = CustomRefreshControl;
            CustomRefreshControl.AddImage(UIImage.FromBundle("loading-indicator"));
        }

        public void ShowLoader()
        {
            this.SetContentOffset(new CoreGraphics.CGPoint(0, -CustomRefreshControl.Frame.Size.Height), true);
            CustomRefreshControl.BeginRefreshing();
        }

        public void HideLoader()
        {
            CustomRefreshControl.EndRefreshing();
        }
    }
}