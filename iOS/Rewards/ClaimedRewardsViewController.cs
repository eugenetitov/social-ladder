using Foundation;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class ClaimedRewardsViewController : RewardsBaseViewController
    {
        public ClaimedRewardsViewController (IntPtr handle) : base (handle)
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


        public override void ViewDidLoad()
        {
            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;

            base.ViewDidLoad();
            this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.RegisterNibForCellReuse(RewardCategoryTableViewCell.Nib, RewardCategoryTableViewCell.ClassName);
            TableView.RegisterNibForCellReuse(ClaimedRewardsTableCell.Nib, ClaimedRewardsTableCell.ClassName);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            RewardList = GetMockData();
            TableView.Source = new ClaimedRewardsTableSource(this);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 130.0f;
            TableView.ReloadData();
            //RefreshRewards();
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
                //SL.RewardList = null;
                TableView.ReloadData();

                UIView overlay = Platform.AddOverlay(View.Frame, UIColor.White, true, true);

                if (CategoryID > 0)
                    await SL.Manager.GetRewardsDrilldownAsync(CategoryID, null);
                else
                    await SL.Manager.GetRewardsAsync();

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


        private List<RewardItemModel> GetMockData()
        {
            List<RewardItemModel> rewardList = new List<RewardItemModel>();
            RewardItemModel rewardModel1 = new RewardItemModel();
            rewardModel1.AutoUnlockDate = DateTime.Now + TimeSpan.FromDays(1);
            rewardModel1.AvailabilityDate = DateTime.Now + TimeSpan.FromDays(40);
            rewardModel1.ButtonLockStatus = true;
            rewardModel1.ID = 1;
            rewardModel1.isOffersInline = false;
            rewardModel1.LocationLat = null;
            rewardModel1.Name = "3 Day GA Ticket";
            rewardModel1.SubTitle = "SubTitle 2";
            rewardModel1.Type = "REWARD";
            rewardModel1.Description = "test1231";
            rewardModel1.RemainingUnits = 90;
            rewardModel1.SmallImageURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";
            rewardModel1.SmallImageClickURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";

            RewardItemModel rewardModel2 = new RewardItemModel();
            rewardModel2.AutoUnlockDate = DateTime.Now + TimeSpan.FromDays(1);
            rewardModel2.AvailabilityDate = DateTime.Now + TimeSpan.FromDays(4);
            rewardModel2.ButtonLockStatus = true;
            rewardModel2.ID = 1;
            rewardModel2.isOffersInline = false;
            rewardModel2.LocationLat = null;
            rewardModel2.Name = "3 Day Parking Pass";
            rewardModel2.SubTitle = "SubTitle 2";
            rewardModel2.Type = "REWARD";
            rewardModel2.Description = "test1231";
            rewardModel2.RemainingUnits = 90;
            rewardModel2.SmallImageURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";
            rewardModel2.SmallImageClickURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";

            RewardItemModel rewardModel3 = new RewardItemModel();
            rewardModel3.AutoUnlockDate = DateTime.Now + TimeSpan.FromDays(1);
            rewardModel3.AvailabilityDate = DateTime.Now + TimeSpan.FromDays(4);
            rewardModel3.ButtonLockStatus = true;
            rewardModel3.ID = 1;
            rewardModel3.isOffersInline = false;
            rewardModel3.LocationLat = null;
            rewardModel3.Name = "3 Day Parking Pass";
            rewardModel3.SubTitle = "SubTitle 2";
            rewardModel3.Type = "REWARD";
            rewardModel3.Description = "test1231";
            rewardModel3.RemainingUnits = 90;
            rewardModel3.SmallImageURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";
            rewardModel3.SmallImageClickURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";

            RewardItemModel rewardModel4 = new RewardItemModel();
            rewardModel4.AutoUnlockDate = DateTime.Now + TimeSpan.FromDays(1);
            rewardModel4.AvailabilityDate = DateTime.Now + TimeSpan.FromDays(70);
            rewardModel4.ButtonLockStatus = true;
            rewardModel4.ID = 1;
            rewardModel4.isOffersInline = false;
            rewardModel4.LocationLat = null;
            rewardModel4.Name = "3 Day Parking";
            rewardModel4.SubTitle = "SubTitle 2";
            rewardModel4.Type = "REWARD";
            rewardModel4.Description = "test1231";
            rewardModel4.RemainingUnits = 90;
            rewardModel4.SmallImageURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";
            rewardModel4.SmallImageClickURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";


            RewardItemModel rewardModel5 = new RewardItemModel();
            rewardModel5.AutoUnlockDate = DateTime.Now + TimeSpan.FromDays(1);
            rewardModel5.AvailabilityDate = DateTime.Now + TimeSpan.FromDays(4);
            rewardModel5.ButtonLockStatus = true;
            rewardModel5.ID = 1;
            rewardModel5.isOffersInline = false;
            rewardModel5.LocationLat = null;
            rewardModel5.Name = "Testete";
            rewardModel5.SubTitle = "SubTitle 2";
            rewardModel5.Type = "REWARD";
            rewardModel5.Description = "test1231";
            rewardModel5.RemainingUnits = 90;
            rewardModel5.SmallImageURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";
            rewardModel5.SmallImageClickURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";


            RewardItemModel rewardModel6 = new RewardItemModel();
            rewardModel6.AutoUnlockDate = DateTime.Now + TimeSpan.FromDays(1);
            rewardModel6.AvailabilityDate = DateTime.Now + TimeSpan.FromDays(4);
            rewardModel6.ButtonLockStatus = true;
            rewardModel6.ID = 1;
            rewardModel6.isOffersInline = false;
            rewardModel6.LocationLat = null;
            rewardModel6.Name = "Testetdsfgsde";
            rewardModel6.SubTitle = "SubTitle 2";
            rewardModel6.Type = "REWARD";
            rewardModel6.Description = "test1231";
            rewardModel6.RemainingUnits = 90;
            rewardModel6.SmallImageURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";
            rewardModel6.SmallImageClickURL = "https://cdn4.iconfinder.com/data/icons/miu-square-flat-social/60/linkedin-square-social-media-128.png";

            rewardList.Add(rewardModel1);
            rewardList.Add(rewardModel2);
            rewardList.Add(rewardModel3);
            rewardList.Add(rewardModel4);
            rewardList.Add(rewardModel5);
            rewardList.Add(rewardModel6);

            return rewardList;
        }
    }
}