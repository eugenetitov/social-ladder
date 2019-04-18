using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Helpers
{
    public static class NavigationHelper
    {
        public static bool ShowAreasCollectionPageFromMainVM { get; set; } = false;
        public static bool ShowAreasCollectionPageAfterLogout { get; set; } = false;
        public static bool ShowAreasCollectionNeedShowBackButton { get; set; } = true;
        public static bool AreasCollectionPageAfterLogoutDoneClicked { get; set; } = false;
        public static bool ShowNetworksPageFromMainVM { get; set; } = false;
        public static bool ShowWebViewNeedShowBackButton { get; set; } = true;
        public static bool ShowWebViewFromIntroVM { get; set; } = false;
        public static bool NeedSubmitInstagramChallenge { get; set; } = false;
        public static bool NeedSubmitInviteChallenge { get; set; } = false;
    }
}
