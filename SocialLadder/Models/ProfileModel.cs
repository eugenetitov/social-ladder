using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace SocialLadder.Models
{
    public class ImageModel
    {
        [PrimaryKey]
        public int ID { get; set; }
        public string Key { get; set; }

    }

    public class SerializedModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Json { get; set; }
    }

    public class AreaModel
    {
        [PrimaryKey]
        public int areaID { get; set; }

        [JsonIgnore, ForeignKey(typeof(ProfileModel))]
        public int ProfileID { get; set; }

        public string areaName { get; set; }
        public string areaGUID { get; set; }
        public string areaIconURL { get; set; }
        public string areaDefaultImageURL { get; set; }
        public string areaDefaultImageLowResURL { get; set; }
        public string areaPrimaryColor { get; set; }
        public string areaButtonColor { get; set; }
        public string areaDescription { get; set; }
        public string termsOfService { get; set; }
        public string allowSuggestion { get; set; }
        public string JoinCode { get; set; }
    }

    public class FriendModel
    {
        public string Name { get; set; }
        public string ProfilePicURL { get; set; }
        public string ExternalID { get; set; }
        public string NetworkName { get; set; }

        [PrimaryKey]
        public int ID { get; set; }

        [JsonIgnore, ForeignKey(typeof(RewardModel))]
        public int RewardID { get; set; }

        [JsonIgnore, ForeignKey(typeof(ProfileModel))]
        public int ProfileID { get; set; }

        public string AppStatus { get; set; }
        public double? Score { get; set; }
        public double? RelScore { get; set; }
        public int Rank { get; set; }
        public string RankLabel { get; set; }
        public string SubLabel { get; set; }
        public string RequestID { get; set; }
        public int SCSUserProfileID { get; set; }
    }

    public class CityRequestParametersModel
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public Paramseters Params { get; set; }

        public class Paramseters
        {
            public string DeviceUUID { get; set; }
        }
    }


    public class ProfileModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string City { get; set; }
        public double Score { get; set; }
        public double RelScore { get; set; }
        public string ScoreLabel { get; set; }
        public string ChallengePreviewImageURL { get; set; }
        public string RewardPreviewImageURL { get; set; }
        public string ChallengeLabel { get; set; }
        public string RewardLabel { get; set; }
        public string ProfilePictureURL { get; set; }
        public string ProfilePictureLargeURL { get; set; }
        public int? LastTransactionID { get; set; }
        public string NoFriendsImageURL { get; set; }
        public string NoFriendsCaption { get; set; }

        [OneToMany("ProfileID", CascadeOperations = CascadeOperation.All)]
        public List<RewardItemModel> RewardPreviewList { get; set; }

        [OneToMany("ProfileID", CascadeOperations = CascadeOperation.All)]
        public List<FriendModel> FriendPreviewList { get; set; }

        public string ChallengeSubLabel { get; set; }
        public string RewardSubLabel { get; set; }
        public string FriendRank { get; set; }
        public int SCSUserProfileID { get; set; }

        [OneToMany("ProfileID", CascadeOperations = CascadeOperation.All)]
        public List<SocialNetworkModel> NetworkList { get; set; }

        public bool ResetTransactions { get; set; }
        public bool isNotificationEnabled { get; set; }
        public bool isGeoEnabled { get; set; }
        public double LocationLat { get; set; }
        public double LocationLon { get; set; }
        public int AppVersion { get; set; }

        [TextBlob("UserAdornmentListBlobbed")]
        public List<string> UserAdornmentList { get; set; }
        [JsonIgnore]
        public string UserAdornmentListBlobbed { get; set; }

        public string UserStatus { get; set; }
        public string UserGUID { get; set; }
        public bool Accepted { get; set; }

        [OneToMany("ProfileID", CascadeOperations = CascadeOperation.All)]
        public List<AreaModel> AreaSubsList { get; set; }
        [JsonIgnore]
        public Dictionary<string, AreaModel> AreaSubsDict { get; set; }

        public string DefaultPromoCode { get; set; }
        public string DefaultTeamCode { get; set; }

      

        public double OverallSLScore { get; set; }

        public int UnlockedRewardsCount { get; set; }
        public int PurchasedRewardsCount { get; set; }
        public int ChallengeCompCount { get; set; }
        public int RewardCount { get; set; }
        public int ChallengeCount { get; set; }

        public List<AreaModel> SuggestedAreaList { get; set; }

        public void BuildAreaSubsDict()
        {
            if (AreaSubsList != null)
            {
                AreaSubsDict = new Dictionary<string, AreaModel>();
                foreach (AreaModel area in AreaSubsList)
                    AreaSubsDict.Add(area.areaGUID, area);
            }
            else
                AreaSubsDict = null;
        }

        public AreaModel GetArea(string areaGUID)
        {
            AreaModel area = null;
            if (AreaSubsDict != null)
                area = AreaSubsDict[areaGUID];
            return area;
        }

        public AreaModel GetFirstArea()
        {
            AreaModel firstItem = AreaSubsList != null && AreaSubsList.Count > 0 ? AreaSubsList[0] : null;//SuggestedAreaList != null && SuggestedAreaList.Count > 0 ? SuggestedAreaList[0] : null;
            return firstItem;
        }

        string RewriteImageUrl(string inUrl)
        {
            string outUrl = null;
            var uri = new Uri(inUrl);
            if (uri.Segments != null && uri.Segments.Length > 0)
            {
                string url = uri.Scheme + "://" + uri.Authority;
                for (int i = 0; i < uri.Segments.Length - 1; i++)
                {
                    string segment = uri.Segments[i];
                    if (!segment.Contains(","))
                        url += segment;
                }
                //url += "c_fill,w_256,h_256,r_max/";
                url += "c_fill,w_256,h_256,r_max,bo_3px_solid_white/";
                string fileName = uri.Segments[uri.Segments.Length - 1];
                string[] comps = fileName.Split('.');
                if (comps != null && comps.Length > 0)
                {
                    url += comps[0] + ".png";
                    outUrl = url;
                }
            }
            return outUrl;
        }

        void RewriteImageUrls()
        {
            var areaList = SL.AreaList;
            if (areaList != null)
            {
                foreach (AreaModel area in areaList)
                {
                    area.areaDefaultImageURL = RewriteImageUrl(area.areaDefaultImageURL);
                    /*
                    var uri = new Uri(area.areaDefaultImageURL);
					if (uri.Segments != null && uri.Segments.Length > 0)
					{
						string url = uri.Scheme + "://" + uri.Authority;
						for (int i = 0; i < uri.Segments.Length - 1; i++)
						{
							string segment = uri.Segments[i];
							if (!segment.Contains(","))
								url += segment;
                        }
                        //url += "c_fill,w_256,h_256,r_max/";
                        url += "c_fill,w_256,h_256,r_max,bo_3px_solid_white/";
                        string fileName =  uri.Segments[uri.Segments.Length - 1];
                        string[] comps = fileName.Split('.');
                        url += comps[0] + ".png";
                        area.areaDefaultImageURL = url;
					}
					*/
                }
            }
            areaList = SL.SuggestedAreaList;
            if (areaList != null)
            {
                foreach (AreaModel area in areaList)
                {
                    area.areaDefaultImageURL = RewriteImageUrl(area.areaDefaultImageURL);
                    /*
                    var uri = new Uri(area.areaDefaultImageURL);
                    if (uri.Segments != null && uri.Segments.Length > 0)
                    {
                        string url = uri.Scheme + "://" + uri.Authority;
                        for (int i = 0; i < uri.Segments.Length - 1; i++)
                        {
                            string segment = uri.Segments[i];
                            if (!segment.Contains(","))
                                url += segment;
                        }
                        //url += "c_fill,w_256,h_256,r_max/";
                        url += "c_fill,w_256,h_256,r_max,bo_3px_solid_white/";
                        url += uri.Segments[uri.Segments.Length - 1];
                        area.areaDefaultImageURL = url;
                    }
                    */
                }
            }
        }

        public void SetDefaultAreaIfNeeded()
        {
            if (string.IsNullOrEmpty(SL.AreaGUID))
            {
                AreaModel area = GetFirstArea();
                if (area != null)
                    SL.AreaGUID = area.areaGUID;
            }
            else
            {
                if (GetArea(SL.AreaGUID) == null)
                {
                    AreaModel area = GetFirstArea();
                    SL.AreaGUID = area != null ? area.areaGUID : string.Empty;
                }
            }
        }

        public void Build()
        {
            //build fast area lookup dict
            BuildAreaSubsDict();

            //rewrite urls 
            RewriteImageUrls();

            //set areaguid if not already set or check to make sure areaguid is set to a subscribed area
            //SetDefaultArea();
        }
    }

    public class NetworkSettingsModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID { get; set; }

        [JsonIgnore, ForeignKey(typeof(ProfileResponseModel))]
        public int ProfileID { get; set; }

        [JsonIgnore, ForeignKey(typeof(ProfileResponseModel))]
        public int ProfileID2 { get; set; }

        public string NetworkName { get; set; }
        public bool RequiresClientUpdate { get; set; }
    }

    public class ProfileResponseModel : ResponseModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID { get; set; }

        [OneToOne("ProfileID2")]
        public ProfileModel Profile { get; set; }

        public bool InviteOnly { get; set; }
        public string HasInviteCodePrompt { get; set; }
        public string InvitePrompt { get; set; }

        [OneToMany("ProfileID", CascadeOperations = CascadeOperation.All)]
        public List<NetworkSettingsModel> NetworkSettingsList { get; set; }
        /*
        //after loading from DB, perform any structure performance and sync updates
        public void Build()
        {
            if (Profile != null)
                Profile.Build();
        }
        */
    }

    public class TransactionModel
    {
        public string TransactionType { get; set; }
        public double TransactionValue { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Data { get; set; }
        public string SocialNetwork { get; set; }
        public int? TransactionTypeID { get; set; }
        public string TransactionGUID { get; set; }
        public string CategoryColor { get; set; }
        public string CategoryName { get; set; }

        [JsonIgnore, PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [JsonIgnore, ForeignKey(typeof(TransactionPageModel))]
        public int PageID { get; set; }

        public string AreaGUID { get; set; }
    }

    public class TransactionResponseModel : ResponseModel
    {
        public TransactionPageModel TransactionPage { get; set; }
    }

    public class TransactionPageModel
    {
        [JsonIgnore, PrimaryKey()]
        public int ID { get; set; }

        [OneToMany("PageID", CascadeOperations = CascadeOperation.All)]
        public List<TransactionModel> TransactionList { get; set; }

        public string NextPage { get; set; }
    }

    public class CitiesResponseModel : ResponseModel
    {
        public int id { get; set; }
        public List<string> result { get; set; }
    }

    public class CityResponseModel : ResponseModel
    {
        public string id { get; set; }
        public string result { get; set; }
    }
}
