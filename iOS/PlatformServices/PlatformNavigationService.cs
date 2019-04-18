using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using SocialLadder.Enums;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.PlatformServices
{
    public class PlatformNavigationService : IPlatformNavigationService
    {
        public Task ChangeArea(int areaId)
        {
            throw new NotImplementedException();
        }

        public void ChangeNotificationIndicatorStatus(bool status)
        {

        }

        public void NavigateToMainWithSwitchArea(AreaModel area, bool newAreaAdded = false)
        {

        }

        public void NavigateToTab(ENavigationTabs tabToNavigate)
        {

        }

        public void NavigateToTab(ENavigationTabs tabToNavigate, bool needRefreshAreas = false)
        {

        }

        public void NavigateToUrl(string url)
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        }

        public void RefreshScoreView()
        {

        }

        public void ShowWizardWebView(string url)
        {

        }
    }
}