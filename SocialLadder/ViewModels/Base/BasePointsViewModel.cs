using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;

namespace SocialLadder.ViewModels.Base
{
    public class BasePointsViewModel : BaseViewModel
    {
        public bool FirstInitialize { get; set; }

        public BasePointsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger = null) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            InitVM();
            Refresh();
            ScoreImage = _assetService.IconScoreTransactions;
            FirstInitialize = true;
        }

        public virtual async Task InitVM()
        {
            
        }

        public override void Start()
        {
            base.Start();
            ScoreImage = _assetService.IconScoreTransactions;
        }

        public void SetRefreshImage()
        {
            ScoreImage = _assetService.NetworkConnectingLoaderImage;
        }

        public async override Task RefreshOther()
        {
            ScoreImage = _assetService.NetworkConnectingLoaderImage;
            await base.RefreshOther();
        }

        public async override Task FinishRefresh()
        {
            await InitVM();
            ScoreCount = Profile.Score.ToString();
            await base.FinishRefresh();
            ScoreImage = _assetService.IconScoreTransactions;
        }

        public async Task UpdateFromView(bool isUpdate = false)
        {
            if (FirstInitialize || isUpdate)
            {
                FirstInitialize = false;
                await Refresh();
            }
        }
    }
}
