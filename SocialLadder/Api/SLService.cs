using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SocialLadder.Logger;
using SocialLadder.Models;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using MvvmCross.Platform;

namespace SocialLadder
{
    public class RestService : IRestService
    {
        HttpClient client;        

        public RestService()
        {
            var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));
            try
            {
                //ProductHeaderValue.Parse("SocialLadder/4.2.1.0 (iPhone; iOS 11.3; Scale/3.00)");
                client = new HttpClient();/*
                {
                    DefaultRequestHeaders =
                {
                    UserAgent = { ProductInfoHeaderValue.Parse(SL.UserAgent) }//exception parsing
                }
                };*/
                client.DefaultRequestHeaders.Add("User-Agent", SL.UserAgent);
                client.MaxResponseContentBufferSize = 256000;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            //client.DefaultRequestHeaders.Add("UserAgent", Constants.PlatformVersion);


            /*
            #if __IOS__
                client.DefaultRequestHeaders.Add("PlatformVersion", Constants.PlatformVersion);
            #endif
            */

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        string DeviceUUID
        {
            get
            {
                return "?DeviceUUID=" + SL.DeviceUUID;
            }
        }

        string DomainKey
        {
            get
            {
                return "&DomainKey=" + Constants.DomainKey;
            }
        }

        string AreaGUID
        {
            get
            {
                string areaGUID = SL.AreaGUID;
                return !string.IsNullOrEmpty(areaGUID) ? "&AreaGUID=" + areaGUID : string.Empty;
            }
        }

        public async Task<ProfileResponseModel> RefreshProfileAsync(Action<ProfileResponseModel> callback)
        {
            ProfileResponseModel profileResponce = new ProfileResponseModel();
            var deviceUUID = SL.DeviceUUID;
            var uri = new Uri(string.Format(Constants.ProfileUrl + DeviceUUID + AreaGUID + "&includeSuggestedAreas=true", string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    if (deviceUUID == SL.DeviceUUID)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        profileResponce = JsonConvert.DeserializeObject<ProfileResponseModel>(content);

                        if (profileResponce != null)
                        {
                            if (profileResponce.Profile != null)
                            { 
                                var encodingService = Mvx.Resolve<IEncodingService>(); 
                                profileResponce.Profile.UserName = encodingService.DecodeFromNonLossyAscii(profileResponce.Profile.UserName);
                                SL.Profile = profileResponce.Profile;
                            } 
                            if (profileResponce.ResponseNotificationList != null && profileResponce.ResponseNotificationList.Count > 0)
                                SL.NotificationList = profileResponce.ResponseNotificationList;
                        }
                    }
                    else
                        Debug.WriteLine(@"              User logged out before refresh profile completed.");

                    Debug.WriteLine(@"              Profile refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(profileResponce);
            return profileResponce;
        }

        public async Task<ProfileResponseModel> GetProfileByUrlAsync(string url, Action<ProfileResponseModel> callback)
        {
            ProfileResponseModel profilerofile = new ProfileResponseModel();
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();
                    profilerofile = JsonConvert.DeserializeObject<ProfileResponseModel>(stringResponse);



                    Debug.WriteLine(@"              Profile Friend refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(profilerofile);
            return profilerofile;
        }

        public async Task<ProfileResponseModel> RefreshProfileFriendAsync(int FriendID, Action<ProfileResponseModel> callback)
        {
            ProfileResponseModel profilerofile = new ProfileResponseModel();
            var uri = new Uri(string.Format(Constants.ProfileUrl + DeviceUUID + AreaGUID + "&FriendID=" + FriendID.ToString(), string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();
                    profilerofile = JsonConvert.DeserializeObject<ProfileResponseModel>(stringResponse);



                    Debug.WriteLine(@"              Profile Friend refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(profilerofile);
            return profilerofile;
        }

        public async Task<FeedResponseModel> RefreshFeedAsync(Action<FeedResponseModel> callback)
        {
            string areaGUID = SL.AreaGUID;
            FeedResponseModel feedResponse = new FeedResponseModel();
            var uri = new Uri(string.Format(Constants.FeedUrl + DeviceUUID + AreaGUID, string.Empty));
            //var uri = new Uri("http://ericdevrki.cloudapp.net/SocialLadderAPI/api/v1/feed?deviceUUID=AF8BD435D7184527999CC21EFDDF4B66&AreaGUID=0CD0151C-D9AF-4852-88DD-AE4D27843103");///string.Format(Constants.FeedUrl + DeviceUUID + AreaGUID, string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    feedResponse = JsonConvert.DeserializeObject<FeedResponseModel>(content);

                    if (feedResponse != null)
                    {

                        //StringParser.ParseEmoji(feedResponse, content, response);

                        if (areaGUID == SL.AreaGUID)
                            SL.Feed = feedResponse;
                        else
                            Debug.WriteLine(@"Feed refresh completed for previous area, ignoring refresh data");

                        if (feedResponse.ResponseNotificationList != null && feedResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = feedResponse.ResponseNotificationList;
                    }
                    Debug.WriteLine(@"              Feed refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(feedResponse);
            return feedResponse;
        }

        public async Task<FeedResponseModel> RefreshFeedFriendsAsync(int FriendID, Action<FeedResponseModel> callback)
        {
            string areaGUID = SL.AreaGUID;
            FeedResponseModel feedResponse = new FeedResponseModel();
            var uri = new Uri(string.Format(Constants.FeedFriendUrl + DeviceUUID + AreaGUID + "&FriendID=" + FriendID.ToString(), string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    feedResponse = JsonConvert.DeserializeObject<FeedResponseModel>(content);

                    if (feedResponse != null)
                    {
                        StringParser.ParseEmoji(feedResponse, content, response);

                        SL.Feed = feedResponse;
                    }
                    Debug.WriteLine(@"              Feed Friends refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(feedResponse);
            return feedResponse;
        }

        public async Task<FeedResponseModel> GetFeedAsync(string url, Action<FeedResponseModel> callback)
        {
            FeedResponseModel feedResponse = new FeedResponseModel();
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    feedResponse = JsonConvert.DeserializeObject<FeedResponseModel>(content);

                    if (feedResponse != null)
                    {
                        //foreach (FeedItemModel item in feedResponse.FeedPage.FeedItemList)
                        //    item.Build();
                        SL.Feed = feedResponse;
                        if (feedResponse.ResponseNotificationList != null && feedResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = feedResponse.ResponseNotificationList;
                    }
                    Debug.WriteLine(@"              Feed refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(feedResponse);
            return feedResponse;
        }

        public async Task<FeedResponseModel> AddFeedAsync(string url, Action<FeedResponseModel> callback)
        {
            FeedResponseModel feedResponse = new FeedResponseModel();
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    feedResponse = JsonConvert.DeserializeObject<FeedResponseModel>(content);

                    if (feedResponse != null)
                    {
                        if (feedResponse.FeedPage != null)
                        {
                            StringParser.ParseEmoji(feedResponse, content, response);

                            foreach (FeedItemModel item in feedResponse.FeedPage.FeedItemList)
                                item.Build();

                            SL.Feed.FeedPage.FeedItemList.AddRange(feedResponse.FeedPage.FeedItemList);
                            SL.Feed.FeedPage.NextPage = feedResponse.FeedPage.NextPage;

                            Debug.WriteLine(@"              Feed added successfully.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(feedResponse);
            return feedResponse;
        }

        public async Task<RewardItemModel> GetRewardByURLAsync(string url, Action<RewardItemModel> callback)
        {
            RewardResponseModel rewardResponse = new RewardResponseModel();
            RewardItemModel reward = new RewardItemModel();
            var uri = new Uri(string.Format(url));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    rewardResponse = JsonConvert.DeserializeObject<RewardResponseModel>(content);

                    //SL.Manager.Rewards = Reward;

                    if (rewardResponse != null)
                    {
                        reward = rewardResponse.RewardList.FirstOrDefault();
                        while (reward.Type == "CATEGORY" && reward.ChildList != null && reward.ChildList.Count > 0)
                            reward = reward.ChildList.FirstOrDefault();
                    }
                    Debug.WriteLine(@"              Rewards refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(reward);
            return reward;
        }

        public async Task<RewardResponseModel> RefreshRewardsAsync(Action<RewardResponseModel> callback)
        {
            string areaGUID = SL.AreaGUID;
            RewardResponseModel rewardResponse = new RewardResponseModel();
            var uri = new Uri(string.Format(Constants.RewardUrl + DeviceUUID + AreaGUID, string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    rewardResponse = JsonConvert.DeserializeObject<RewardResponseModel>(content);

                    //SL.Manager.Rewards = Reward;

                    if (rewardResponse != null)
                    {
                        if (areaGUID == SL.AreaGUID)
                        {
                            if (rewardResponse.RewardList != null && rewardResponse.RewardList.Count > 0)
                                SL.RewardList = rewardResponse.RewardList;
                        }
                        else
                            Debug.WriteLine(@"Reward refresh completed for previous area, ignoring refresh data");

                        if (rewardResponse.ResponseNotificationList != null && rewardResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = rewardResponse.ResponseNotificationList;
                    }

                    Debug.WriteLine(@"              Rewards refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(rewardResponse);
            return rewardResponse;
        }

        public async Task<RewardResponseModel> DrilldownRewardsAsync(int categoryID, Action<RewardResponseModel> callback)
        {
            RewardResponseModel rewardResponse = new RewardResponseModel();
            var uri = new Uri(string.Format(Constants.RewardUrl + "/drilldown" + DeviceUUID + AreaGUID + "&CategoryID=" + categoryID.ToString(), string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    rewardResponse = JsonConvert.DeserializeObject<RewardResponseModel>(content);

                    //SL.Manager.Rewards = Reward;

                    if (rewardResponse != null)
                    {
                        if (rewardResponse.RewardList != null && rewardResponse.RewardList.Count > 0)
                        {
                            //if (rewardResponse.RewardList.Count == 1 && rewardResponse.RewardList[0].ID == categoryID)
                            //SL.RewardList = rewardResponse.RewardList[0].ChildList;
                            //else
                            SL.RewardList = rewardResponse.RewardList;
                        }

                        if (rewardResponse.ResponseNotificationList != null && rewardResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = rewardResponse.ResponseNotificationList;
                    }

                    Debug.WriteLine(@"              Rewards Drilldown refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(rewardResponse);
            return rewardResponse;
        }

        //check for each chall type that we have an instance of that chall type
        List<ChallengeTypeModel> FilterChallengeTypes(List<ChallengeModel> challengeInstances, List<ChallengeTypeModel> challengeTypes)
        {
            Dictionary<string, bool> challengeInstanceExistsTable = new Dictionary<string, bool>();
            foreach (ChallengeModel challenge in challengeInstances)
            {
                challengeInstanceExistsTable[challenge.TypeCodeDisplayName] = true;
            }

            List<ChallengeTypeModel> verifiedExistingChallengeTypes = new List<ChallengeTypeModel>();
            foreach (ChallengeTypeModel type in challengeTypes)
            {
                if (challengeInstanceExistsTable.ContainsKey(type.DisplayName) && challengeInstanceExistsTable[type.DisplayName])
                    verifiedExistingChallengeTypes.Add(type);
            }
            return verifiedExistingChallengeTypes;
        }

        public async Task<ChallengeResponseModel> RefreshChallengesAsync(Action<ChallengeResponseModel> callback)
        {
            string areaGUID = SL.AreaGUID;
            ChallengeResponseModel challengeResponse = new ChallengeResponseModel();
            var uri = new Uri(string.Format(Constants.ChallengeUrl + DeviceUUID + AreaGUID, string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(content);

                    //SL.Manager.Challenges = Challenge;

                    if (challengeResponse != null)
                    {
                        if (areaGUID == SL.AreaGUID)
                        {
                            if (challengeResponse.ChallengeList != null && challengeResponse.ChallengeList.Count > 0)
                                SL.ChallengeList = challengeResponse.ChallengeList;

                            if (challengeResponse.ChallengeTypeList != null && challengeResponse.ChallengeTypeList.Count > 0)
                                SL.ChallengeTypeList = FilterChallengeTypes(challengeResponse.ChallengeList, challengeResponse.ChallengeTypeList);
                        }
                        else
                            Debug.WriteLine(@"Challenge refresh completed for previous area, ignoring refresh data");

                        if (challengeResponse.ResponseNotificationList != null && challengeResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = challengeResponse.ResponseNotificationList;
                    }

                    //SocialLadder.UpdateChallengesDB();//SocialLadder.Manager.DataBasePath);

                    Debug.WriteLine(@"              Challenges refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }


        public async Task<ChallengeResponseModel> GetChallengeWithUrl(string url, Action<ChallengeResponseModel> callback = null)
        {
            ChallengeResponseModel challengeResponse = new ChallengeResponseModel();
            try
            {
                var uri = new Uri(string.Format(url));
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<NetworkResponseModel> SaveNetworkAsync(SocialNetworkModel network, bool isNew, Action<NetworkResponseModel> callback)
        {
            NetworkResponseModel networkResponse = new NetworkResponseModel();
            var uri = new Uri(string.Format(Constants.NetworkUrl + DeviceUUID, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(network);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                if (isNew)
                {
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    networkResponse = JsonConvert.DeserializeObject<NetworkResponseModel>(responseContent);
                    if (networkResponse != null)
                    {
                        Debug.WriteLine(@"              Network successfully saved.");
                    }

                    if ((networkResponse.ResponseCode < 0) && (networkResponse.ResponseCode != -2))
                    {
                        LogHelper.LogUserMessage("ADDUPDATENETWORK_FAILED", string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(networkResponse);
            return networkResponse;
        }


        public async Task<bool> DeleteNetworkAsync(string id, Action<bool> callback)
        {
            bool didSucceed = false;
            var uri = new Uri(string.Format(Constants.NetworkUrl + DeviceUUID, id));
            try
            {
                var response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    didSucceed = true;
                    Debug.WriteLine(@"              Network successfully deleted.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(didSucceed);
            return didSucceed;
        }

        public async Task<ChallengeResponseModel> RefreshChallengeDetail(string url, Action<ChallengeResponseModel> callback)
        {
            ChallengeResponseModel challengeResponse = new ChallengeResponseModel();
            var uri = new Uri(url);
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(content);
                    if (challengeResponse != null)
                    {
                        Debug.WriteLine(@"              Refreshed challenge detail response successfully received.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<ShareResponseModel> RefreshShareTemplate(string url, Action<ShareResponseModel> callback)
        {
            ShareResponseModel shareResponse = new ShareResponseModel();
            var uri = new Uri(url);
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    shareResponse = JsonConvert.DeserializeObject<ShareResponseModel>(content);
                    if (shareResponse != null)
                    {
                        Debug.WriteLine(@"              Share Template response successfully received.");
                    }
                    if (shareResponse == null || shareResponse.ShareTemplate == null)
                    {
                        LogHelper.LogUserMessage("SHARE", "Client/Server network out of sync");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(shareResponse);
            return shareResponse;
        }

        public async Task<ShareResponseModel> PostSubmitShare(string shareTransactionID, Action<ShareResponseModel> callback)
        {
            ShareResponseModel challengeResponse = new ShareResponseModel();
            var uri = new Uri(string.Format(Constants.ShareUrl + "/report" + DeviceUUID + "&sharetransactionid=" + shareTransactionID + "&externalid=" + "&sharemessage=", string.Empty));
            //SocialLadderAPI/api/v1/share/report?deviceUUID=3393FFAFAEEF4A10BCB72D8931E81138&sharetransactionid=ba9ac8a2-f1eb-43f3-a0c0-3e089bb0de65&externalid=&sharemessage="

            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ShareResponseModel>(responseContent);
                    if (challengeResponse != null)
                    {
                        Debug.WriteLine(@"              PostReportShare Submit successful");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<ShareResponseModel> PostSubmitEngagement(int id, Action<ShareResponseModel> callback)
        {
            ShareResponseModel challengeResponse = new ShareResponseModel();
            var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + id + "/submitResponse" + DeviceUUID + "&SelectedResponseID=(null)", string.Empty));

            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ShareResponseModel>(responseContent);
                    if (challengeResponse != null)
                    {
                        Debug.WriteLine(@"              CompleteChallenge Submit successful");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<ChallengeResponseModel> PostSubmitCollateral(int id, Action<ChallengeResponseModel> callback)
        {
            var challengeResponse = new ChallengeResponseModel();
            //var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + id + "/submitResponse" + DeviceUUID + "&SelectedResponseID=(null)", string.Empty));
            var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + id + "/userDone" + DeviceUUID, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(responseContent);
                    if (challengeResponse != null)
                    {
                        Debug.WriteLine(@"              CompleteChallenge Submit successful");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<ShareResponseModel> PostShare(ShareModel share, Action<ShareResponseModel> callback)
        {
            ShareResponseModel shareResponse = new ShareResponseModel();
            var uri = new Uri(string.Format(Constants.ShareUrl + DeviceUUID, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(share);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    shareResponse = JsonConvert.DeserializeObject<ShareResponseModel>(responseContent);
                    if (shareResponse != null)
                    {
                        Debug.WriteLine(@"              Share Posted successfully saved.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(shareResponse);
            return shareResponse;
        }

        public async Task<ChallengeResponseModel> PostSubmitContent(int id, string ContentReference, string Comment, double lat, double lon, Action<ChallengeResponseModel> callback)
        {
            ChallengeResponseModel challengeResponse = new ChallengeResponseModel();
            var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + id + "/submitContent" + DeviceUUID + "&ContentReference=" + ContentReference + "&Comment=" + Comment + "&Lat=" + lat + "&Long=" + lon, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(responseContent);
                    if (challengeResponse != null)
                    {
                        Debug.WriteLine(@"              PostSubmitContent successful");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<ChallengeResponseModel> PostSubmitDocContent(int id, string DocURL, string Comment, Action<ChallengeResponseModel> callback)
        {
            ChallengeResponseModel challengeResponse = new ChallengeResponseModel();
            var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + id + "/submitDocContent" + DeviceUUID + "&DocURL=" + DocURL + "&Comment=" + Comment, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(responseContent);
                    if (challengeResponse != null)
                    {
                        Debug.WriteLine(@"              PostSubmitDocContent successful");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<ChallengeResponseModel> SubmitAnswerAsync(int challengeID, int? answerID, string writeIn, Action<ChallengeResponseModel> callback)
        {
            ChallengeResponseModel challengeResponse = new ChallengeResponseModel();
            var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + challengeID.ToString() + "/submitResponse" + DeviceUUID + "&SelectedResponseID=" + (answerID != null ? answerID.ToString() : ""), string.Empty));
            try
            {
                StringContent param;
                if (!string.IsNullOrEmpty(writeIn))
                {
                    var model = new WriteInModel();
                    model.WriteIn = writeIn;
                    var json = JsonConvert.SerializeObject(model);
                    param = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                    param = null;
                HttpResponseMessage response = await client.PostAsync(uri, param);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(content);
                    if (challengeResponse != null)
                    {
                        Debug.WriteLine(@"              Challenge submit answer response successfully received.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<RewardResponseModel> PostCommitReward(int RewardID, Action<RewardResponseModel> callback)
        {
            RewardResponseModel rewardResponse = new RewardResponseModel();
            var uri = new Uri(string.Format(Constants.RewardUrl + "/commit" + DeviceUUID + "&RewardID=" + RewardID, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    rewardResponse = JsonConvert.DeserializeObject<RewardResponseModel>(responseContent);
                    if (rewardResponse != null)
                    {
                        Debug.WriteLine(@"              PostCommitReward successful");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(rewardResponse);
            return rewardResponse;
        }

        public async Task<ActionResponseModel> AreaInviteAsync(string code, Action<ActionResponseModel> callback)
        {
            ActionResponseModel actionResponse = new ActionResponseModel();
            var uri = new Uri(string.Format(Constants.AreaInviteUrl + DeviceUUID + "&GiftCode=" + code, string.Empty));
            try
            {
                StringContent param = null;
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, param);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    actionResponse = JsonConvert.DeserializeObject<ActionResponseModel>(content);

                    if (actionResponse != null)
                    {
                        if (actionResponse.ResponseNotificationList != null && actionResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = actionResponse.ResponseNotificationList;

                        Debug.WriteLine(@"              Area Invite response successfully received.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(actionResponse);
            return actionResponse;
        }

        public async Task<ProfileResponseModel> CheckInUpdateScoreAsync(double lat, double lon, Action<ProfileResponseModel> callback)
        {
            ProfileResponseModel profileResponse = new ProfileResponseModel();
            var uri = new Uri(string.Format(Constants.CheckInUrl + DeviceUUID + "&Lat=" + lat + "&Long=" + lon + AreaGUID + "&includeSuggestedAreas=true", string.Empty));
            try
            {
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    profileResponse = JsonConvert.DeserializeObject<ProfileResponseModel>(content);

                    if (profileResponse != null)
                    {
                        if (profileResponse.Profile != null)
                            SL.Profile = profileResponse.Profile;

                        if (profileResponse.ResponseNotificationList != null && profileResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = profileResponse.ResponseNotificationList;
                    }

                    Debug.WriteLine(@"              CheckInUpdateScore completed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(profileResponse);
            return profileResponse;
        }

        public async Task<ProfileResponseModel> SaveProfile(ProfileModel profile, Action<ProfileResponseModel> callback)
        {
            ProfileResponseModel profileResponse = new ProfileResponseModel();
            var uri = new Uri(string.Format(Constants.ProfileUrl + DeviceUUID + DomainKey, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(profile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    profileResponse = JsonConvert.DeserializeObject<ProfileResponseModel>(responseContent);

                    if (profileResponse != null)
                    {
                        if (profileResponse.Profile != null)
                            SL.Profile = profileResponse.Profile;

                        if (profileResponse.ResponseNotificationList != null && profileResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = profileResponse.ResponseNotificationList;
                    }

                    Debug.WriteLine(@"              Profile successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(profileResponse);

            return profileResponse;
        }

        public async Task<ProfileResponseModel> UpdateProfile(ProfileUpdateModel profile, Action<ProfileResponseModel> callback)
        {
            ProfileResponseModel profileResponse = new ProfileResponseModel();
            var uri = new Uri(string.Format(Constants.ProfileUrl + DeviceUUID + DomainKey, string.Empty));

            try
            {
                var json = JsonConvert.SerializeObject(profile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    profileResponse = JsonConvert.DeserializeObject<ProfileResponseModel>(responseContent);

                    if (profileResponse != null)
                    {
                        if (profileResponse.Profile != null)
                            SL.Profile = profileResponse.Profile;

                        if (profileResponse.ResponseNotificationList != null && profileResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = profileResponse.ResponseNotificationList;
                    }

                    Debug.WriteLine(@"              Update profile successfully completed.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(profileResponse);

            return profileResponse;
        }

        public async Task<CityResponseModel> GetCityListWithLatitude(double lat, double lon, Action<CityResponseModel> callback)
        {
            CityResponseModel citiesResponse = new CityResponseModel();

            string url = Constants.ServerUrl + "/popwebservice/popwebservice.ashx/";
            var uri = new Uri(url);

            try
            {
                var values = new Dictionary<string, string>();
                values.Add("DeviceUUID", SL.DeviceUUID);
                values.Add("lat", lat.ToString());
                values.Add("lon", lon.ToString());

                RpcCallModel model = new RpcCallModel();
                model.Id = "1";
                model.Method = "GetCityForLoc";
                model.Params = values;

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await client.PostAsync(uri, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    citiesResponse = JsonConvert.DeserializeObject<CityResponseModel>(responseContent);
                    Debug.WriteLine(@"              GetCityList call successfull.");
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogUserMessage("GETCITYFORLOC_FAILED", ex.Message);

                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(citiesResponse);
            return citiesResponse;
        }

        public async Task<CitiesResponseModel> GetCityList(Action<CitiesResponseModel> callback)
        {
            CitiesResponseModel citiesResponse = new CitiesResponseModel();

            string url = Constants.ServerUrl + "/popwebservice/popwebservice.ashx/";
            var uri = new Uri(url);

            try
            {
                var values = new Dictionary<string, string>();
                values.Add("DeviceUUID", SL.DeviceUUID);

                RpcCallModel model = new RpcCallModel();
                model.Id = "1";
                model.Method = "GetCities";
                model.Params = values;

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await client.PostAsync(uri, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    citiesResponse = JsonConvert.DeserializeObject<CitiesResponseModel>(responseContent);
                    Debug.WriteLine(@"              GetCityList call successfull.");

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogUserMessage("GETCITYFORLOC_FAILED", ex.Message);

                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(citiesResponse);
            return citiesResponse;
        }

        public async Task<NotificationResponseModel> RefreshNotificationsAsync(Action<NotificationResponseModel> callback)
        {
            NotificationResponseModel notificationResponse = null;
            var uri = new Uri(string.Format(Constants.NotificationPendingUrl + DeviceUUID, string.Empty));
            try
            {

                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    notificationResponse = JsonConvert.DeserializeObject<NotificationResponseModel>(content);

                    if (notificationResponse != null)
                    {
                        //SL.PendingNotification = notificationResponse;

                        //if (notificationResponse.ResponseNotificationList != null && notificationResponse.ResponseNotificationList.Count > 0)
                        //    SL.NotificationList = notificationResponse.ResponseNotificationList;
                    }

                    Debug.WriteLine(@"              Pending Notifications refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(notificationResponse);
            return notificationResponse;
        }

        public async Task<TransactionResponseModel> RefreshTransactionsAsync(Action<TransactionResponseModel> callback)
        {
            SL.AreTransactionsLoading = true;
            TransactionResponseModel transactionsResponse = new TransactionResponseModel();
            var uri = new Uri(string.Format(Constants.TransactionUrl + DeviceUUID + AreaGUID, string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    //alert server response error
                    SL.TransactionPages = null;
                    callback?.Invoke(transactionsResponse);
                    return transactionsResponse;
                }

                var content = await response.Content.ReadAsStringAsync();
                transactionsResponse = JsonConvert.DeserializeObject<TransactionResponseModel>(content);

                int? count = transactionsResponse?.TransactionPage?.TransactionList?.Count;
                if (count == null || count == 0)
                {
                    //alert couldn't parse response body or body is empty
                    SL.TransactionPages = null;
                    callback?.Invoke(transactionsResponse);
                    return transactionsResponse;
                }

                SL.TransactionPages = transactionsResponse.TransactionPage;
                SL.AreTransactionsLoading = false;
                callback?.Invoke(transactionsResponse);

                Debug.WriteLine(@"              Transactions refreshed successfully.");

                return transactionsResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);

                SL.AreTransactionsLoading = false;
                callback?.Invoke(transactionsResponse);

                return transactionsResponse;
            }
        }

        public async Task<TransactionResponseModel> GetNextTransactionsPageByUrlAsync(string url, Action<TransactionResponseModel> callback)
        {
            SL.AreTransactionsLoading = true;

            TransactionResponseModel transactionsResponse = new TransactionResponseModel();
            var uri = new Uri(url);
            try
            {
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    //alert server response error
                    callback?.Invoke(transactionsResponse);
                    return transactionsResponse;
                }

                var content = await response.Content.ReadAsStringAsync();
                transactionsResponse = JsonConvert.DeserializeObject<TransactionResponseModel>(content);
                if (transactionsResponse == null)
                {
                    //alert couldn't parse response body or body is empty
                    callback?.Invoke(transactionsResponse);
                    return transactionsResponse;
                }

                List<TransactionModel> orderedList = transactionsResponse.TransactionPage.TransactionList/*.OrderBy(tm => tm.TransactionDate).ToList()*/;
                if (orderedList != null)
                {
                    SL.TransactionPages.TransactionList.AddRange(orderedList);
                }
                SL.TransactionPages.NextPage = transactionsResponse.TransactionPage.NextPage;
                SL.AreTransactionsLoading = false;
                callback?.Invoke(transactionsResponse);

                Debug.WriteLine(@"              Transactions refreshed successfully.");

                return transactionsResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);

                SL.AreTransactionsLoading = false;
                callback?.Invoke(transactionsResponse);

                return transactionsResponse;
            }
        }

        public async Task<ResponseModel> GetInstagram(string url)
        {
            ResponseModel responseModel = new ResponseModel();
            var uri = new Uri(string.Format(url, string.Empty));
            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    responseModel = JsonConvert.DeserializeObject<ResponseModel>(content);

                    if (responseModel != null)
                    {

                    }

                    Debug.WriteLine(@"               Get Instagram successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            return responseModel;
        }

        public async Task<ChallengeModel> GetChallengeByUrlAsync(string url)
        {
            var challenge = new ChallengeModel();
            var uri = new Uri(url);//string.Format(Constants.ChallengeUrl + "/" + /*id +*/ DeviceUUID, string.Empty)
            try
            {
                HttpResponseMessage response = null;
                response = await client.GetAsync(uri);

                string stringResponse = await response.Content.ReadAsStringAsync();
                ChallengeResponseModel challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(stringResponse);
                challenge = challengeResponse.Challenge;

                if (!response.IsSuccessStatusCode || challenge == null)
                {
                    Debug.WriteLine(@"             Get Challenge error: code: {0}, model is null: {1}", response.StatusCode, challenge == null);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);

                return null;
            }
            return challenge;
        }

        public async Task<bool> UpdateChallenge(ChallengeModel challenge, int id, Action<bool> callback)
        {
            bool didSucceed = false;
            var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + id + DeviceUUID, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(challenge);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    didSucceed = true;
                    Debug.WriteLine(@"              Challenge successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(didSucceed);
            return didSucceed;
        }

        public async Task<ChallengeResponseModel> PostCheckInAndVerify(int id, double lat, double lon, Action<ChallengeResponseModel> callback)
        {
            ChallengeResponseModel challengeResponse = new ChallengeResponseModel();
            var uri = new Uri(string.Format(Constants.ChallengeUrl + "/" + id + "/checkInAndverify" + DeviceUUID + "&Lat=" + lat + "&Long=" + lon, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<ChallengeResponseModel>(content);

                    Debug.WriteLine(@"              Posted CheckInAndVerify successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<UpdatedFeedResponceModel> PostDialog(string url, UserInputModel input, Action<UpdatedFeedResponceModel> callback)
        {
            UpdatedFeedResponceModel actionResponse = new UpdatedFeedResponceModel();
            var uri = new Uri(string.Format(url, string.Empty));
            try
            {
                StringContent postContent = null;
                if (input != null)
                {
                    var json = JsonConvert.SerializeObject(input);
                    postContent = new StringContent(json, Encoding.UTF8, "application/json");
                }
                HttpResponseMessage response = await client.PostAsync(uri, postContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    actionResponse = JsonConvert.DeserializeObject<UpdatedFeedResponceModel>(content);

                    Debug.WriteLine(@"              Post Dialog successfull.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(actionResponse);
            return actionResponse;
        }

        public async Task<RpcResponceModel> UpdateAPNToken(string token, Action<RpcResponceModel> callback)
        {
            RpcResponceModel actionResponse = new RpcResponceModel();
            string url = Constants.PopWebServiceUrl;
            var uri = new Uri(url);
            try
            {
                RpcCallModel model = new RpcCallModel();
                model.Id = "1";
                model.Method = "UpdateAPNToken";
                model.Params = new System.Collections.Generic.Dictionary<string, string>();
                model.Params.Add("DeviceUUID", SL.DeviceUUID);
                model.Params.Add("APNToken", token);
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var responceContent = await response.Content.ReadAsStringAsync();
                    actionResponse = JsonConvert.DeserializeObject<RpcResponceModel>(responceContent);

                    Debug.WriteLine(@"              Token update successfull.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(actionResponse);
            return actionResponse;
        }

        public async Task<ResponseModel> Subscribe(string areaGUID, Action<ResponseModel> callback)
        {
            ResponseModel response = new ActionResponseModel();
            var uri = new Uri(string.Format(Constants.SubscribeUrl + DeviceUUID + "&AreaGUID=" + areaGUID, string.Empty));
            try
            {
                HttpResponseMessage httpResponse = await client.PostAsync(uri, null);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ResponseModel>(content);

                    Debug.WriteLine(@"              Subscribe call successfull.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(response);
            return response;
        }

        public async Task<AttributionTrackingResponse> TrackAttribution(string trackingKey, Action<AttributionTrackingResponse> callback)
        {
            AttributionTrackingResponse response = new AttributionTrackingResponse();
            var uri = new Uri(string.Format(Constants.TrackAttributionUrl + DeviceUUID + (!string.IsNullOrEmpty(trackingKey) ? "&TrackingKey=" + trackingKey : ""), string.Empty));
            try
            {
                HttpResponseMessage httpResponse = await client.PostAsync(uri, null);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<AttributionTrackingResponse>(content);

                    Debug.WriteLine(@"              Subscribe call successfull.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(response);
            return response;
        }

        public async Task LogUserEvent(string transactionType, System.Collections.Generic.Dictionary<string, string> parameters)
        {
            string methodName = "LogUserEvent2";
            var uri = new Uri(string.Format(Constants.PopWebServiceUrl, string.Empty));
            try
            {
                var values = new Dictionary<string, string>();
                values.Add("DeviceUUID", SL.DeviceUUID);
                values.Add("TransactionType", transactionType);
                if (parameters != null)
                    values.Add("ParamJSONDict", JsonConvert.SerializeObject(parameters));

                RpcCallModel model = new RpcCallModel();
                model.Id = "1";
                model.Method = methodName;
                model.Params = values;

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await client.PostAsync(uri, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

                    Debug.WriteLine(@"              LogEvent call successfull.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }

        }

        public async Task<NotificationModel> GetNotification(string deviceUUID, string notificationUUID, Action<NotificationModel> callback)
        {
            NotificationModel challengeResponse = new NotificationModel();
            var uri = new Uri(string.Format("{0}?deviceUUID={1}&notificationUID={2}", Constants.NotificationAcknowledgeURL, deviceUUID, notificationUUID));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<NotificationModel>(content);

                    Debug.WriteLine(@"              Posted CheckInAndVerify successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse);
            return challengeResponse;
        }

        public async Task<NotificationModel> AcknowledgeNotification(string notificationUUID, Action<NotificationModel> callback)
        {
            NotificationResponseModel challengeResponse = new NotificationResponseModel();
            var uri = new Uri(string.Format("{0}{1}&notificationUID={2}", Constants.NotificationAcknowledgeURL, DeviceUUID, notificationUUID));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    challengeResponse = JsonConvert.DeserializeObject<NotificationResponseModel>(content);

                    Debug.WriteLine(@"              Posted CheckInAndVerify successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(challengeResponse.NotificationObject);
            return challengeResponse.NotificationObject;
        }

        public async Task<NotificationResponseModel> GetNotificationsAsync(string AreaGUID, Action<NotificationResponseModel> callback)
        {
            NotificationResponseModel notiResponse = new NotificationResponseModel();
            var uri = new Uri(string.Format(Constants.NotificationsUrl + DeviceUUID + (!string.IsNullOrEmpty(AreaGUID) ? "&AreaGUID=" + AreaGUID : ""), string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    notiResponse = JsonConvert.DeserializeObject<NotificationResponseModel>(content);

                    if (notiResponse != null)
                    {
                        if (notiResponse.ResponseNotificationList != null && notiResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList = notiResponse.ResponseNotificationList;
                    }
                    Debug.WriteLine(@"              Notifications refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(notiResponse);
            return notiResponse;
        }

        public async Task<NotificationResponseModel> GetNotificationsSummaryAsync(Action<NotificationResponseModel> callback)
        {
            NotificationResponseModel notiResponse = new NotificationResponseModel();
            var uri = new Uri(string.Format(Constants.NotificationsSummaryUrl + DeviceUUID, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    notiResponse = JsonConvert.DeserializeObject<NotificationResponseModel>(content);

                    Debug.WriteLine(@"              Notifications Summary refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(notiResponse);
            return notiResponse;
        }


        public async Task<NotificationResponseModel> AddNotificationsByUrl(string url, Action<NotificationResponseModel> callback)
        {
            NotificationResponseModel notiResponse = new NotificationResponseModel();
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    notiResponse = JsonConvert.DeserializeObject<NotificationResponseModel>(content);
                    if (notiResponse != null)
                    {
                        if (notiResponse.ResponseNotificationList != null && notiResponse.ResponseNotificationList.Count > 0)
                            SL.NotificationList.AddRange(notiResponse.ResponseNotificationList);
                    }
                    Debug.WriteLine(@"              Notifications Summary refreshed successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(notiResponse);
            return notiResponse;
        }

        public async Task<LikeResponceModel> PostLikeFeed(string url, Action<LikeResponceModel> callback = null)
        {
            LikeResponceModel actionResponse = new LikeResponceModel();
            var uri = new Uri(string.Format(url, string.Empty));
            try
            {
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    actionResponse = JsonConvert.DeserializeObject<LikeResponceModel>(content);

                    Debug.WriteLine(@"              Post  Like successfull.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(actionResponse);
            return actionResponse;
        }

        public async Task<GuideResponseModel> GetGuideUrl(string areaGuid, Action<GuideResponseModel> callback)
        {
            string url = String.Format("{0}?deviceUUID={1}&areaGUID={2}", Constants.GuideUrl, SL.DeviceUUID, areaGuid);
            GuideResponseModel actionResponse = new GuideResponseModel();
            var uri = new Uri(string.Format(url, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    actionResponse = JsonConvert.DeserializeObject<GuideResponseModel>(content);

                    Debug.WriteLine(@"              Post  Like successfull.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            if (callback != null)
                callback(actionResponse);
            return actionResponse;
        }
    }
}
