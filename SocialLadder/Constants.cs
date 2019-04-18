using MvvmCross.Platform.UI;

namespace SocialLadder
{
    public static class Constants
    {
        public static string EricDevUrl = "http://ericdevrki.cloudapp.net";
        public static string DevelopmentUrl = "http://socialladderapidev.cloudapp.net";
        public static string ProductionUrl = "https://socialladder.rkiapps.com";

        //public static string ServerUrl = EricDevUrl;
        //public static string ServerUrl = DevelopmentUrl;
        public static string ServerUrl = ProductionUrl;

        public static string WebViewLink = "https://socialladder.rkiapps.com/terms-and-conditions-mobileonly.html";


        public static string BaseUrl = ServerUrl + "/SocialLadderAPI/api/v1/";

        public static string ProfileUrl = BaseUrl + "profile";
        public static string FeedUrl = BaseUrl + "feed";
        public static string NotificationsUrl = BaseUrl + "notifications";
        public static string FeedFriendUrl = BaseUrl + "feed/friends";
        public static string RewardUrl = BaseUrl + "reward";
        public static string ChallengeUrl = BaseUrl + "challenge";
        public static string NetworkUrl = BaseUrl + "network";
        public static string ShareUrl = BaseUrl + "share";
        public static string AreaInviteUrl = BaseUrl + "gift/registercode";
        public static string CheckInUrl = BaseUrl + "profile/checkinandupdatescore";
		public static string NotificationPendingUrl = BaseUrl + "notification/pending";
        public static string TransactionUrl = BaseUrl + "transaction";
        public static string SubscribeUrl = BaseUrl + "area/subscribe";
        public static string TrackAttributionUrl = BaseUrl + "profile/trackAttribution";
        public static string PopWebServiceUrl = ServerUrl + "/popwebservice/popwebservice.ashx/";
        public static string NotificationAcknowledgeURL = BaseUrl + "notification/acknowledge";
        public static string GuideUrl = BaseUrl + "guide/gettingStartedURL";
        public static string NotificationsSummaryUrl = BaseUrl + "notifications/summary";

        public static string deviceuuid = "05C1D94CD5A0445FA4DAA92539F44521";

        public static string DomainKey = "ECEAE108-233E-43E5-AA8C-0F8A6130E8EA";
        public static string PlatformVersion = "SocialLadder/5.7.8.0";//"SocialLadder/5.7.3.0";

        // URL of REST service
        //public static string RestUrl = "http://developer.xamarin.com:8081/api/todoitems/{0}";
        // Credentials that are hard coded into the REST service
        public static string Username = "Xamarin";
        public static string Password = "Pa$$w0rd";

        public static MvxColor ToolbarDefaultColor = new MvxColor(232, 31, 138);
        public static bool NeedFirebaseImpl = false;
    }
}
