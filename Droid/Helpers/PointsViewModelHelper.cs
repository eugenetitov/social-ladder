using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Core.Navigation;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.ViewModels.Base;
using SocialLadder.ViewModels.Points;

namespace SocialLadder.Droid.Helpers
{
    public class PointsViewModelHelper
    {
        public void CreateVM()
        {
            var _navigationService = Mvx.Resolve<IMvxNavigationService>();
            var _assetService = Mvx.Resolve<IPlatformAssetService>();
            var _alertService = Mvx.Resolve<IAlertService>();
            var _messanger = Mvx.Resolve<IMvxMessenger>();
            var _localNotificationService = Mvx.Resolve<ILocalNotificationService>();
            _transactionsVM = new TransactionsViewModel(_navigationService, _alertService, _assetService, _localNotificationService);
            _leaderboardVM = new LeaderboardViewModel(_navigationService, _alertService, _assetService, _localNotificationService, _messanger);
            _pointsVM = new PointsViewModel(_navigationService, _alertService, _assetService, _localNotificationService);
        }

        public void SetFirstInitialise()
        {
            _transactionsVM.FirstInitialize = true;
            _leaderboardVM.FirstInitialize = true;
            _pointsVM.FirstInitialize = true;
        }

        private TransactionsViewModel _transactionsVM;
        public TransactionsViewModel TransactionsVM
        {
            get => _transactionsVM;
            set => _transactionsVM = value;
        }

        private LeaderboardViewModel _leaderboardVM;
        public LeaderboardViewModel LeaderboardVM
        {
            get => _leaderboardVM;
            set => _leaderboardVM = value;
        }

        private PointsViewModel _pointsVM;
        public PointsViewModel PointsVM
        {
            get => _pointsVM;
            set => _pointsVM = value;
        }
    }
}