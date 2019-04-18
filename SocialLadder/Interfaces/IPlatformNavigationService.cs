using SocialLadder.Enums;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IPlatformNavigationService
    {
        void NavigateToTab(ENavigationTabs tabToNavigate, bool needRefreshAreas = false);
        void NavigateToUrl(string url);
        void NavigateToMainWithSwitchArea(AreaModel area, bool newAreaAdded = false);
        Task ChangeArea(int areaId);
        void ShowWizardWebView(string url);
        void RefreshScoreView();
        void ChangeNotificationIndicatorStatus(bool status);
    }
}
