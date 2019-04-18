using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Points;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Points
{
    public class PointsViewModel : BasePointsViewModel, IMainTabViewModel
    {
        public int ProgressFactor { get; set; } = 100;
        public event EventHandler<bool> DetailsViewOpened;
        #region Properties
        private MvxObservableCollection<PointsSummaryLocalModel> _points;
        public MvxObservableCollection<PointsSummaryLocalModel> Points
        {
            get => _points;
            set => SetProperty(ref _points, value);
        }

        private bool _detailsViewHidden;
        public bool DetailsViewHidden
        {
            get => _detailsViewHidden;
            set => SetProperty(ref _detailsViewHidden, value);
        }
        #endregion

        public PointsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService) : base(navigationService, alertService, assetService, localNotificationService)
        {
            IsBusy = true;
            Points = new MvxObservableCollection<PointsSummaryLocalModel>();
        }

        private void AddPoints()
        {
            Points.Clear();
            var pointsSource = SL.SummaryList;
            foreach (var point in pointsSource)
            {
                var item = new PointsSummaryLocalModel();
                if (point is ChallengeTypeSummaryModel)
                    UpdateCellData(point as ChallengeTypeSummaryModel, item);
                else if (point is ChallengeSummaryModel)
                    UpdateCellData(point as ChallengeSummaryModel, item);
                else if (point is RewardSummaryModel)
                    UpdateCellData(point as RewardSummaryModel, item);
                item.Progress = item.Progress * ProgressFactor;
                Points.Add(item);
            }
        }

        public async override Task FinishRefresh()
        {
            await base.FinishRefresh();
            await Task.Run(async () =>
            {

                await InitVM();
            });
        }

        public async override Task InitVM()
        {
            DetailsViewHidden = false;
            InvokeOnMainThread(() => { AddPoints(); });          
            await base.InitVM();
        }

        public void UpdateCellData(ChallengeSummaryModel summary, PointsSummaryLocalModel item)
        {
            item.ImageName = "challenges_icon";
            item.UnlockedText = summary.ChallengeCompCount + " of " + summary.Total + " Challenges completed";
            item.PurchasedText = "";
            if (summary.ChallengeCompCount == 0)
            {
                summary.ChallengeCompCount = 1;
            }
            item.Progress = summary.Total > 0 ? (float)summary.ChallengeCompCount / (float)summary.Total : 0;
        }
        public void UpdateCellData(ChallengeTypeSummaryModel summary, PointsSummaryLocalModel item)
        {
            item.ImageName = "challenges_icon";
            item.UnlockedText = summary.ChallengeCompCount + " of " + summary.Total + " " + summary.DisplayName + " Challenges completed";
            item.PurchasedText = "";
            item.Progress = summary.Total > 0 ? (float)summary.ChallengeCompCount / (float)summary.Total : 0;
        }
        public void UpdateCellData(RewardSummaryModel summary, PointsSummaryLocalModel item)
        {
            item.ImageName = "challenges_icon";
            item.UnlockedText = summary.UnlockedRewardsCount + " of " + summary.Total + " Rewards unlocked";
            item.PurchasedText = summary.PurchasedRewardsCount + " reward" + (summary.PurchasedRewardsCount != 1 ? "s" : "") + " purchased";
            item.Progress = summary.Total > 0 ? (float)summary.UnlockedRewardsCount / (float)summary.Total : 0;
        }

        #region Commands
        public MvxAsyncCommand Command
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                });
            }
        }

        public MvxCommand DetailsChartHiddenCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    DetailsViewHidden = !DetailsViewHidden;
                    try { 
                        DetailsViewOpened.Invoke(null, DetailsViewHidden);
                    }
                    catch (Exception ex) {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }       
        #endregion
    }
}
