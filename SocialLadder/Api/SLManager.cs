using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SocialLadder
{
    public class SLManager
    {
        public string DataBasePath
        {
            get; set;
        }
        IRestService restService;
        public SQLite.SQLiteConnection DataBase
        {
            get; set;
        }

  

        public SLManager(IRestService service, string LocalDataBasePath = null)
        {
            restService = service;
            DataBasePath = LocalDataBasePath;
            DataBase = new SQLite.SQLiteConnection(DataBasePath);

            DataBase.CreateTable<TransactionPageModel>();
            DataBase.CreateTable<TransactionModel>();
            DataBase.CreateTable<LocalFile>();

            DataBase.DeleteAll<TransactionPageModel>();
            DataBase.DeleteAll<TransactionModel>();

            Debug.WriteLine(@"DeviceUUID=" + SL.DeviceUUID);
            Debug.WriteLine(@"AreaGUID=" + SL.AreaGUID);
        }

        public async Task<ChallengeModel> GetChallengeByUrlAsync(string url)
        {
            return await restService.GetChallengeByUrlAsync(url);
        }

        public Task<FeedResponseModel> GetFeedAsync(Action<FeedResponseModel> callback = null)
        {
            return restService.RefreshFeedAsync(callback);
        }

        public Task<FeedResponseModel> GetFeedByFriendIdAsync(int FriendID, Action<FeedResponseModel> callback = null)
        {
            return restService.RefreshFeedFriendsAsync(FriendID, callback);
        }

        public Task<FeedResponseModel> GetFeedByUrlAsync(string url,Action<FeedResponseModel> callback = null)
        {
            return restService.GetFeedAsync(url, callback);
        }

        public Task<NetworkResponseModel> SaveNetworkAsync(SocialNetworkModel network, bool isNew = false, Action<NetworkResponseModel> callback = null)
        {
            return restService.SaveNetworkAsync(network, isNew, callback);
        }

        public Task<bool> DeleteNetworkAsync(SocialNetworkModel network, Action<bool> callback = null)
        {
            return restService.DeleteNetworkAsync(network.ID, callback);
        }

        public Task<ActionResponseModel> AreaInviteAsync(string code, Action<ActionResponseModel> callback = null)
        {
            return restService.AreaInviteAsync(code, callback);
        }

        public Task<ProfileResponseModel> GetProfileAsync(Action<ProfileResponseModel> callback = null)
        {
            return restService.RefreshProfileAsync(callback);
        }

        public Task<ProfileResponseModel> GetProfileByUrlAsync(string url, Action<ProfileResponseModel> callback)
        {
            return restService.GetProfileByUrlAsync(url, callback);
        }

        public Task<ProfileResponseModel> GetProfileByFriendIdAsync(int FriendID, Action<ProfileResponseModel> callback)
        {
            return restService.RefreshProfileFriendAsync(FriendID, callback);
        }

        public Task<ChallengeResponseModel> GetChallengesAsync(Action<ChallengeResponseModel> callback = null)
        {
            return restService.RefreshChallengesAsync(callback);
        }

        public Task<ChallengeResponseModel> GetChallengeByUrl(string url, Action<ChallengeResponseModel> callback = null)
        {
            return restService.GetChallengeWithUrl(url, callback);
        }

        public Task<RewardItemModel> GetRewardByUrl(string url, Action<RewardItemModel> callback = null)
        {
            return restService.GetRewardByURLAsync(url, callback);
        }

        public Task<RewardResponseModel> GetRewardsAsync(Action<RewardResponseModel> callback = null)
        {
            return restService.RefreshRewardsAsync(callback);
        }

        public Task<RewardResponseModel> GetRewardsDrilldownAsync(int categoryID, Action<RewardResponseModel> callback = null)
        {
            return restService.DrilldownRewardsAsync(categoryID, callback);
        }

        public Task<ProfileResponseModel> FinalCheckInAsync(double lat, double lon, Action<ProfileResponseModel> callback = null)
        {
            return restService.CheckInUpdateScoreAsync(lat, lon, callback);
        }

        public Task<ProfileResponseModel> SaveProfileAsync(ProfileModel profile, Action<ProfileResponseModel> callback = null)
        {
            return restService.SaveProfile(profile, callback);
        }

        public Task<ProfileResponseModel> UpdateProfileAsync(ProfileUpdateModel profile, Action<ProfileResponseModel> callback = null)
        {
            return restService.UpdateProfile(profile, callback);
        }

        public Task<CitiesResponseModel> GetCityList(Action<CitiesResponseModel> callback = null)
        {
            return restService.GetCityList(callback);
        }
        public Task<CityResponseModel> GetCityListWithLatitude(double lat, double lon, Action<CityResponseModel> callback = null)
        {
            return restService.GetCityListWithLatitude(lat, lon, callback);
        }
        
        public Task<FeedResponseModel> AddFeedAsync(string url, Action<FeedResponseModel> callback = null)
        {
            return restService.AddFeedAsync(url, callback);
        }

        public Task<NotificationResponseModel> RefreshNotificationsAsync(Action<NotificationResponseModel> callback = null)
        {
            return restService.RefreshNotificationsAsync(callback);
        }

        public Task<TransactionResponseModel> RefreshTransactionsAsync(Action<TransactionResponseModel> callback = null)
        {
            return restService.RefreshTransactionsAsync(callback);
        }

        //public Task<TransactionResponseModel> NewRefreshTransactionsAsync(Action<TransactionResponseModel> callback = null)
        //{
        //    return restService.NewRefreshTransactionsAsync(callback);
        //}

        public Task<TransactionResponseModel> GetNextTransactionsPageByUrlAsync(string url, Action<TransactionResponseModel> callback = null)
        {
            return restService.GetNextTransactionsPageByUrlAsync(url, callback);
        }

        public Task<ChallengeResponseModel> SubmitAnswerAsync(int challengeID, int? answerID, string writeIn = null, Action<ChallengeResponseModel> callback = null)
        {
            return restService.SubmitAnswerAsync(challengeID, answerID, writeIn, callback);
        }

        public Task<ChallengeResponseModel> RefreshChallengeDetail(string url, Action<ChallengeResponseModel> callback = null)
        {
            return restService.RefreshChallengeDetail(url, callback);
        }

        public Task<ShareResponseModel> RefreshShareTemplate(string url, Action<ShareResponseModel> callback = null)
        {
            return restService.RefreshShareTemplate(url, callback);
        }

        public Task<ShareResponseModel> PostSubmitShare(string shareTransactionID, Action<ShareResponseModel> callback = null)
        {
            return restService.PostSubmitShare(shareTransactionID, callback);
        }

        public Task<ShareResponseModel> PostSubmitEngagement(int id, Action<ShareResponseModel> callback = null)
        {
            return restService.PostSubmitEngagement(id, callback);
        }

        public Task<ChallengeResponseModel> PostSubmitCollateral(int id, Action<ChallengeResponseModel> callback = null)
        {
            return restService.PostSubmitCollateral(id, callback);
        }

        public Task<ShareResponseModel> PostShare(ShareModel share, Action<ShareResponseModel> callback = null)
        {
            return restService.PostShare(share, callback);
        }

        public Task<ChallengeResponseModel> PostSubmitContent(int id, string ContentReference, string Comment, double lat, double lon, Action<ChallengeResponseModel> callback = null)
        {
            return restService.PostSubmitContent(id, ContentReference, Comment, lat, lon, callback);
        }

        public Task<ChallengeResponseModel> PostSubmitDocContent(int id, string DocURL, string Comment, Action<ChallengeResponseModel> callback = null)
        {
            return restService.PostSubmitDocContent(id, DocURL, Comment, callback);
        }

        public Task<ResponseModel> GetInstagram(string url)
        {
            return restService.GetInstagram(url);
        }

        public Task<bool> UpdateChallenge(ChallengeModel challenge, int id, Action<bool> callback = null)
        {
            return restService.UpdateChallenge(challenge, id, callback);
        }

        public Task<ChallengeResponseModel> PostCheckInAndVerify(int id, double lat, double lon, Action<ChallengeResponseModel> callback = null)
        {
            return restService.PostCheckInAndVerify(id, lat, lon, callback);
        }

        public Task<RewardResponseModel> PostCommitReward(int RewardID, Action<RewardResponseModel> callback = null)
        {
            return restService.PostCommitReward(RewardID, callback);
        }

        public Task<UpdatedFeedResponceModel> PostDialog(string url, UserInputModel input, Action<UpdatedFeedResponceModel> callback = null)
        {
            return restService.PostDialog(url, input, callback);
        }

        public Task<ResponseModel> Subscribe(string areaGUID, Action<ResponseModel> callback = null)
        {
            return restService.Subscribe(areaGUID, callback);
        }

        public Task<RpcResponceModel> UpdateAPNToken(string token, Action<RpcResponceModel> callback)
        {
            return restService.UpdateAPNToken(token, callback);
        }

        public Task<AttributionTrackingResponse> TrackAttribution(string trackingKey = null, Action<AttributionTrackingResponse> callback = null)
        {
            return restService.TrackAttribution(trackingKey, callback);
        }

        public Task LogUserEvent(String transactionType, Dictionary<string, string> parameters = null)
        {
            return restService.LogUserEvent(transactionType, parameters);
        }

        public Task<NotificationModel> GetNotification(string deviceUUID, string notificationUUID, Action<NotificationModel> callback = null)
        {
            return restService.GetNotification(deviceUUID, notificationUUID, callback);
        }

        public Task<NotificationModel> AcknowledgeNotification(string notificationUUID, Action<NotificationModel> callback = null)
        {
            return restService.AcknowledgeNotification(notificationUUID, callback);
        }

        public Task<NotificationResponseModel> GetNotificationsAsync(string areaGuid, Action<NotificationResponseModel> callback = null)
        {
            return restService.GetNotificationsAsync(areaGuid, callback);
        }

        public Task<NotificationResponseModel> GetNotificationSummaryAsync(Action<NotificationResponseModel> callback = null)
        {
            return restService.GetNotificationsSummaryAsync(callback);
        }

        public Task<LikeResponceModel> PostLikeFeedAsync(string url, Action<LikeResponceModel> callback = null)
        {
            return restService.PostLikeFeed(url, callback);
        }


        public Task<NotificationResponseModel> AddNotificationsByUrl(string url, Action<NotificationResponseModel> callback = null)
        {
            return restService.AddNotificationsByUrl(url, callback);
        }

        public Task<GuideResponseModel> GetGuideUrl(string areaGuid, Action<GuideResponseModel> callback = null)
        {
            return restService.GetGuideUrl(areaGuid, callback);
        }
    }
}
        
