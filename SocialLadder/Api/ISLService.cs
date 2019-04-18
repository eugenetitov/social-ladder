using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialLadder.Models;

namespace SocialLadder
{
    public interface IRestService
    {
        Task<FeedResponseModel> RefreshFeedAsync(Action<FeedResponseModel> callback);
        Task<FeedResponseModel> RefreshFeedFriendsAsync(int FreindID, Action<FeedResponseModel> callback);
        Task<ChallengeModel> GetChallengeByUrlAsync(string url);
        Task<FeedResponseModel> GetFeedAsync(string url, Action<FeedResponseModel> callback);
        Task<ChallengeResponseModel> RefreshChallengesAsync(Action<ChallengeResponseModel> callback);
        Task<ChallengeResponseModel> GetChallengeWithUrl(string url, Action<ChallengeResponseModel> callback);
        Task<RewardItemModel> GetRewardByURLAsync(string url, Action<RewardItemModel> callback);
        Task<RewardResponseModel> RefreshRewardsAsync(Action<RewardResponseModel> callback);
        Task<RewardResponseModel> DrilldownRewardsAsync(int categoryID, Action<RewardResponseModel> callback);
        Task<ProfileResponseModel> RefreshProfileAsync(Action<ProfileResponseModel> callback);
        Task<ProfileResponseModel> GetProfileByUrlAsync(string url, Action<ProfileResponseModel> callback);
        Task<ProfileResponseModel> RefreshProfileFriendAsync(int FriendID, Action<ProfileResponseModel> callback);
        Task<NotificationResponseModel> RefreshNotificationsAsync(Action<NotificationResponseModel> callback);
        Task<NetworkResponseModel> SaveNetworkAsync(SocialNetworkModel network, bool isNew, Action<NetworkResponseModel> callback);
        Task<bool> DeleteNetworkAsync(string id, Action<bool> callback);

        Task<GuideResponseModel> GetGuideUrl(string areaGuid, Action<GuideResponseModel> callback);
        Task<ActionResponseModel> AreaInviteAsync(string code, Action<ActionResponseModel> callback);
        Task<ProfileResponseModel> CheckInUpdateScoreAsync(double lat, double lon, Action<ProfileResponseModel> callback);
        Task<ProfileResponseModel> SaveProfile(ProfileModel profile, Action<ProfileResponseModel> callback);
        Task<ProfileResponseModel> UpdateProfile(ProfileUpdateModel profile, Action<ProfileResponseModel> callback);
        Task<CitiesResponseModel> GetCityList(Action<CitiesResponseModel> callback);
        Task<CityResponseModel> GetCityListWithLatitude(double lat, double lon, Action<CityResponseModel> callback);
        Task<FeedResponseModel> AddFeedAsync(string url, Action<FeedResponseModel> callback);
        Task<TransactionResponseModel> RefreshTransactionsAsync(Action<TransactionResponseModel> callback);
        //Task<TransactionResponseModel> NewRefreshTransactionsAsync(Action<TransactionResponseModel> callback);
        Task<TransactionResponseModel> GetNextTransactionsPageByUrlAsync(string url, Action<TransactionResponseModel> callback);
        Task<ChallengeResponseModel> SubmitAnswerAsync(int challengeID, int? answerID, string writeIn, Action<ChallengeResponseModel> callback);
        Task<ChallengeResponseModel> RefreshChallengeDetail(string url, Action<ChallengeResponseModel> callback);
        Task<ShareResponseModel> RefreshShareTemplate(string url, Action<ShareResponseModel> callback);
        Task<ShareResponseModel> PostSubmitShare(string shareTransactionID, Action<ShareResponseModel> callback);
        Task<ShareResponseModel> PostSubmitEngagement(int id, Action<ShareResponseModel> callback);
        Task<ChallengeResponseModel> PostSubmitCollateral(int id, Action<ChallengeResponseModel> callback);
        Task<ShareResponseModel> PostShare(ShareModel share, Action<ShareResponseModel> callback);
        Task<ChallengeResponseModel> PostSubmitContent(int id, string ContentReference, string Comment, double lat, double lon, Action<ChallengeResponseModel> callback);
        Task<ChallengeResponseModel> PostSubmitDocContent(int id, string DocURL, string Comment, Action<ChallengeResponseModel> callback);
        Task<ResponseModel> GetInstagram(string url);
        Task<bool> UpdateChallenge(ChallengeModel challenge, int id, Action<bool> callback);
        Task<ChallengeResponseModel> PostCheckInAndVerify(int id, double lat, double lon, Action<ChallengeResponseModel> callback);
        Task<RewardResponseModel> PostCommitReward(int RewardID, Action<RewardResponseModel> callback);
        Task<UpdatedFeedResponceModel> PostDialog(string url, UserInputModel input, Action<UpdatedFeedResponceModel> callback);
        Task<LikeResponceModel> PostLikeFeed(string url, Action<LikeResponceModel> callback = null);
        Task<RpcResponceModel> UpdateAPNToken(string token, Action<RpcResponceModel> callback);
        Task<ResponseModel> Subscribe(string areaGUID, Action<ResponseModel> callback);
        Task<AttributionTrackingResponse> TrackAttribution(string trackingKey, Action<AttributionTrackingResponse> callback);
        Task LogUserEvent(string transactionType, Dictionary<string, string> parameters);
        Task<NotificationModel> GetNotification(string deviceUUID, string notificationUUID, Action<NotificationModel> callback);
        Task<NotificationModel> AcknowledgeNotification(string notificationUUID, Action<NotificationModel> callback);
        Task<NotificationResponseModel> GetNotificationsAsync(string AreaGUID, Action<NotificationResponseModel> callback);
        Task<NotificationResponseModel> GetNotificationsSummaryAsync(Action<NotificationResponseModel> callback);
        Task<NotificationResponseModel> AddNotificationsByUrl(string url, Action<NotificationResponseModel> callback);
    }
}
