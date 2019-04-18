using Foundation;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class AvailableRewardsViewController : RewardsBaseViewController
    {
        public AvailableRewardsViewController (IntPtr handle) : base (handle)
        {

        }

        public int CategoryID
        {
            get; set;
        }

        public List<RewardItemModel> RewardList
        {
            get; set;
        }

        public bool DidRefresh
        {
            get; set;
        }


        public string HeaderTitle
        {
            get;set;
        }

        public override void ViewDidLoad()
        {
            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;

            base.ViewDidLoad();
            this.TableView.SectionHeaderHeight = UITableView.AutomaticDimension;
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

            TableView.RegisterNibForCellReuse(RewardCategoryTableViewCell.Nib, RewardCategoryTableViewCell.ClassName);
            TableView.RegisterNibForCellReuse(AvailableRewardsTableCell.Nib, AvailableRewardsTableCell.ClassName);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            TableView.Source = new AvailableRewardsTableSource(this, HeaderTitle, SL.RewardList[0]?.Name);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 130.0f;
            TableView.ReloadData();
        }

        async void RefreshChallenges()
        {
            await SL.Manager.GetChallengesAsync();
        }

        async Task RefreshProfile()
        {
            await SL.Manager.GetProfileAsync();
            AreaCollection.ReloadData();
            UpdateNavBar();
        }

        async void RefreshRewards()
        {
            if (!DidRefresh)
            {
                UIView overlay = Platform.AddOverlay(View.Frame, UIColor.White, true, true);

                if (CategoryID > 0)
                    await SL.Manager.GetRewardsDrilldownAsync(CategoryID, null);
                //else
                //    await SL.Manager.GetRewardsAsync();

                DidRefresh = true;
                overlay.RemoveFromSuperview();

                TableView.ReloadData();
            }
        }

        async void RefreshFeed()
        {
            await SL.Manager.GetFeedAsync();
        }

        public async override Task Refresh()
        {
            RefreshRewards();
            await RefreshProfile();
            await base.Refresh();
            //RefreshChallenges();
            //RefreshFeed();
        }



    }
}