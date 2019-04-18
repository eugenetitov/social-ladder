using System;
using MvvmCross.Binding.iOS.Views;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Sources.Feed;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Views.Cells
{
    public class FeedBaseTableViewCell : MvxTableViewCell
    {
        public FeedTableView FeedTableView { get; set; }
        public FeedItemModel FeedItem { get; set; }
        public FeedTableSource Source { get; set; }

        public Action<string, string> OnLoadUserProfile;

        protected FeedBaseTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public virtual void UpdateCellData(FeedItemModel item)
        {

        }

        public virtual void ApplyStyles()
        {

        }

        public virtual void Reset()
        {
            
        }

        public virtual void ShowRefreshLoader()
        {

        }

        public virtual void HideRefreshLoader()
        {

        }

        public virtual nfloat GetViewHeight()
        {
            return 0;
        }
    }
}