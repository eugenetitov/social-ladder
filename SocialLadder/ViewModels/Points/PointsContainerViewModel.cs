using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.ViewModels.Points
{
    public class PointsContainerViewModel : BaseViewModel
    {
        public event EventHandler AddCurrentVM;
        public PointsContainerViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {

        }

        public void SetDefaultCurrentVM(int tabPosition)
        {
            try {
                AddCurrentVM.Invoke(tabPosition, null);
            } catch {
                Debug.WriteLine("Can't set default current VM");
            }
           
        }

        public bool[] IsShown = new bool[] { false, false, false };
    }
}
