using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Enums;
using SocialLadder.Interfaces;
using SocialLadder.Models;

namespace SocialLadder.Droid.PlatformService
{
    public class PlatformNavigationService : IPlatformNavigationService
    {
        public static Activity Activity { get; set; }

        public async Task ChangeArea(int areaId)
        {
            if (Activity != null && Activity is MainActivity)
            {
                await (Activity as MainActivity).ViewModel.AreaClicked(areaId);
            }
        }

        public void NavigateToMainWithSwitchArea(AreaModel area, bool newAreaAdded = false)
        {
            (Activity as MainActivity).OpenMainViewWithSwitchArea(area, newAreaAdded);
        }

        public void ChangeNotificationIndicatorStatus(bool status)
        {
            (Activity as MainActivity).ChangeNotificationIndicatorStatus(status);
        }

        public void NavigateToTab(ENavigationTabs tabToNavigate, bool needRefreshAreas = false)
        {
            if (!(Activity is MainActivity))
            {
                return;
            }
            if (needRefreshAreas && tabToNavigate == ENavigationTabs.FEED)
            {
                (Activity as MainActivity).SetFeedCurrentTabWithRefresh();
                return;
            }
            if (tabToNavigate == ENavigationTabs.FEED)
            {
                (Activity as MainActivity).SetRequiredCurrentTab(0);
            }
            if (tabToNavigate == ENavigationTabs.POINTS)
            {
                (Activity as MainActivity).SetRequiredCurrentTab(1);
            }
            if (tabToNavigate == ENavigationTabs.REWARDS)
            {
                (Activity as MainActivity).SetRequiredCurrentTab(2);
            }
            if (tabToNavigate == ENavigationTabs.CHALLENGES)
            {
                (Activity as MainActivity).SetRequiredCurrentTab(3);
            }
            if (tabToNavigate == ENavigationTabs.MORE)
            {
                (Activity as MainActivity).SetRequiredCurrentTab(4);
            }
        }

        public void NavigateToUrl(string url)
        {
            var uri = Android.Net.Uri.Parse(url);
            var intent = new Intent(Intent.ActionView, uri);
            Activity.StartActivity(intent);
        }

        public void RefreshScoreView()
        {
            (Activity as MainActivity).RefreshScoreView();
        }

        public void ShowWizardWebView(string url)
        {
            (Activity as MainActivity).ShowWizardWebView(url);
        }
    }
}