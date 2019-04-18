using Foundation;
using SocialLadder.iOS.CustomControlls;
using SocialLadder.iOS.Interfaces.ViewControllers;
using SocialLadder.iOS.Sources.Feed;
using SocialLadder.iOS.ViewControllers.Feed;
using SocialLadder.Models;
using System;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.Views
{
    public partial class FeedTableView : UITableView
    {
        public nfloat GreatestCellHeight { get; set; }
        public CustomRefreshControl CustomRefreshControl;
        public bool IsLoading { get; set; }
        public FeedViewController ViewController { get; set; }
        public IFeedViewController FeedViewController { get; set; }

        public FeedTableView (IntPtr handle) : base (handle)
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

        //public void LoadMore()
        //{
        //    string nextPage = SL.FeedNextPage;
        //    if (!string.IsNullOrEmpty(nextPage))
        //    {
        //        IsLoading = true;
        //        SL.Manager.AddFeedAsync(nextPage, LoadMoreComplete);
        //    }
        //    else {
        //        (this.Source as FeedTableSource).Footer.Hidden = true;
        //    }
        //}

        //void LoadMoreComplete(FeedResponseModel response)
        //{
        //    ReloadData();
        //    IsLoading = false;
        //    (this.Source as FeedTableSource).Footer.Hidden = true;
        //}
    }
}