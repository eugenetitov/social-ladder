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
    public class TransactionsViewModel : BasePointsViewModel, IMainTabViewModel
    {
        #region Properties
        public bool IsLoadingMore { get; set; } = false;

        private MvxObservableCollection<TransactionsLocalModel> _transactionsItems;
        public MvxObservableCollection<TransactionsLocalModel> TransactionsItems
        {
            get => _transactionsItems;
            set
            {
                SetProperty(ref _transactionsItems, value);
                PlaceholderHidden = TransactionsItems == null || TransactionsItems.Count == 0 ? true : false;
            }
        }

        private bool _placeholderHidden;
        public bool PlaceholderHidden
        {
            get => _placeholderHidden;
            set => SetProperty(ref _placeholderHidden, value);
        }

        private string _placeholderText;
        public string PlaceholderText
        {
            get => _placeholderText;
            set => SetProperty(ref _placeholderText, value);
        }

        private string _rewardsCount;
        public string RewardsCount
        {
            get => _rewardsCount;
            set => SetProperty(ref _rewardsCount, value);
        }

        private string _challengesCount;
        public string ChallengesCount
        {
            get => _challengesCount;
            set => SetProperty(ref _challengesCount, value);
        }
        #endregion

        public TransactionsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService) : base(navigationService, alertService, assetService, localNotificationService)
        {
            IsBusy = true;
            TransactionsItems = new MvxObservableCollection<TransactionsLocalModel>();
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            base.InitFromBundle(parameters);
        }

        public async override Task RefreshOther()
        {
            await base.RefreshOther();
            await Task.Run(async () =>
            {
                await SL.Manager.GetChallengesAsync((response) => { });
                await SL.Manager.GetRewardsAsync((response) => { });
                await SL.Manager.RefreshTransactionsAsync(async(response) => { await InitVM(); });
            });
        }

        public async override Task InitVM()
        {
            if (SL.HasProfile)
            {
                ChallengesCount = SL.Profile.ChallengeCompCount + " of " + SL.Profile.ChallengeCount + " Challenges";
                RewardsCount = SL.Profile.RewardLabel + " of " + SL.Profile.RewardCount + "\n Rewards";
            }
            InvokeOnMainThread(() => { LoadTransactions(); });               
            await base.InitVM();
        }

        public async Task LoadMoreTransactions()
        {
            if (!string.IsNullOrEmpty(SL.TransactionPages?.NextPage))
            {
                IsLoadingMore = true;
                await SL.Manager.GetNextTransactionsPageByUrlAsync(SL.TransactionPages.NextPage, (model) =>
                {
                    var transactionList = SL.TransactionPages?.TransactionList;
                    if (transactionList == null || transactionList.Count == 0)
                    {
                        return;
                    }
                    LoadTransactions();
                });
                IsLoadingMore = false;
            }
        }

        private void LoadTransactions()
        {
            TransactionsItems.Clear();
            var source = SL.TransactionPages;
            if (source == null || source.TransactionList == null)
            {
                return;
            }
            foreach (var item in source.TransactionList)
            {
                TransactionsItems.Add(new TransactionsLocalModel("challenges_icon", item.TransactionDate, item.TransactionValue, item.TransactionType));
            }
        }

        #region Commands
        public MvxAsyncCommand SeeEmCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    //This command for iOS
                    //Navigate to RewardsVM
                });
            }
        }

        public MvxAsyncCommand DoMoreCommand
        {
            get
            {
                return new MvxAsyncCommand(async () =>
                {
                    //This command for iOS
                    //Navigate to MoreVM
                });
            }
        }
        #endregion
    }
}
