using System;
using System.Threading.Tasks;
using CoreGraphics;
using SocialLadder.iOS.Navigation;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public class PointsBaseViewController : SLViewController
    {
        //public bool ShouldRefreshEndpoint { get; set; }
        public int PageIndex { get; set; }
        public PointsContainerViewController PointsContainerViewController { get; set; }

        public PointsBaseViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (PointsContainerViewController.ShouldRefreshEndpoint(PageIndex))
            {
                RefreshAsync();

                return;
            }
            RefreshLocally();

        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public async override Task Refresh()
        {
            await base.Refresh();
            PointsContainerViewController.EndpointRefreshDidBegin(PageIndex);
        }

        private async void RefreshAsync()
        {
            await Refresh();
        }

        public virtual void RefreshLocally()
        {
            PointsContainerViewController.EndpointRefreshDidBegin(PageIndex);

        }

        public void NextPage()
        {
            PointsContainerViewController.NextPage();
        }

        public void PrevPage()
        {
            PointsContainerViewController.PrevPage();
        }

        public void ScrollToOffset(UIScrollView scrollView)
        {
            scrollView.ScrollRectToVisible(new CGRect(0, PointsContainerViewController.PageYOffset, scrollView.Frame.Width, scrollView.Frame.Height), false);

        }
    }
}