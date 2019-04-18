using Foundation;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Rewards;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.RewardCollection
{
    public partial class MonthRewardsViewController : SLViewController
    {

        public List<RewardItemModel> RewardList
        {
            get; set;
        }

        public int CategoryID
        {
            get;set;
        }

        public string HeaderTitle
        {
            get;set;
        }

        public int CurrentPage
        {
            get;set;
        }

        //private bool _didReloadDataRewards;

        public override void ViewDidLoad()
        {
            tabBarView.Hidden = true;
            base.AreaCollectionOutlet = AreaCollection;
            base.AreaCollectionHeightOutlet = AreaCollectionHeight;

            base.ViewDidLoad();

            var iv = new UIImageView(UIImage.FromBundle("Background"));
            iv.ContentMode = UIViewContentMode.ScaleToFill;
            RewardsTableView.BackgroundView = iv;
            AllRewardsButton.SetTitle("All Rewards");
            AviableRewardsButton.SetTitle("Aviable");
            ClaimedButton.SetTitle("Claimed");

            // Perform any additional setup after loading the view, typically from a nib.
            RewardsTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            RewardsTableView.RegisterNibForCellReuse(MonthRewardsTableCell.Nib,  MonthRewardsTableCell.Key);
            ChangeSelectedButton(CurrentPage);
        }

        public MonthRewardsViewController() : base("MonthRewardsViewController", null)
        {

        }

        public MonthRewardsViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            RewardsTableView.Source = new MonthRewardsTableSource(this, HeaderTitle);
            RewardsTableView.RowHeight = (nfloat)(View.Frame.Width * 0.92);
            RefreshAsync();
        }


        private void UpdateTabButtons()
        {
            var buttons = new RewardsTabButton[] { AllRewardsButton, AviableRewardsButton, ClaimedButton };
            foreach (var button in buttons)
            {
                button?.SetUnselected();
            }
        }

        private void ChangeSelectedButton(int page)
        {
            UpdateTabButtons();
            if (page == 0)
            {
                AllRewardsButton.SetSelected();
            }
            if (page == 1)
            {
                AviableRewardsButton.SetSelected();
            }
            if (page == 2)
            {
                ClaimedButton.SetSelected();
            }
            CurrentPage = page;
        }

        public async override Task Refresh()
        {
            RefreshRewards();
            await RefreshProfile(); 
            await base.Refresh();
        }

        private async void RefreshAsync()
        {
            await Refresh();
        }

        public async Task RefreshProfile()
        {
            await SL.Manager.GetProfileAsync();
            
            AreaCollection.ReloadData();
            UpdateNavBar();
        }

        public /*async*/ void RefreshRewards()
        {
            //if (!_didReloadDataRewards)
            //{
            //    UIView overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
            //    await SL.Manager.GetRewardsAsync();
            //    _didReloadDataRewards = true;
            //    overlay.RemoveFromSuperview(); 
            //}
            //RewardsTableView.ReloadData();
        }

    }
}