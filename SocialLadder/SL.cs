using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SQLite;
using SocialLadder.Models;
using SocialLadder.Services;
using SQLiteNetExtensions.Extensions;
using System.Diagnostics;
using Xamarin.Auth;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

namespace SocialLadder
{
    public interface IPathInfo
    {
        string GetPersonalFolder();
    }

    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Device
    {
        public string Name { get; set; }
        public string SystemVersion { get; set; }
        public string Model { get; set; }
        public string IdentifierForVendor { get; set; }
        public bool IsNotificationsEnabled { get; set; }
    }

    public class SL
    {
        static string userID => "";//{ get { return AccountStorage.GetPropertyValue(AccessToken.UserId); } }
        static object locker = new object();

        public static bool IsBusy;
        public static bool AreTransactionsLoading;
        //public static event EventHandler OnRefreshFeedDidBegin;
        //public static event EventHandler<FeedResponseModel> OnRefreshFeedComplete;

        static Device _device = new Device();
        public static Device Device { get { return _device; } }

        public static ISettings AppSettings => CrossSettings.Current;
        //static SLManager _manager = new SLManager(new RestService());
        //public static SLManager Manager {get {return _manager;}}
        public static SLManager Manager { get; set; }

        public static bool IsOrm = false;

        public static string UserAgent
        {
            get
            {
                string userAgent = Constants.PlatformVersion;
                bool hasModel = !string.IsNullOrEmpty(Device.Model);
                bool hasOs = !string.IsNullOrEmpty(Device.SystemVersion);
                if (hasModel || hasOs)
                {
                    userAgent += " (";
                    if (hasModel)
                    {
                        userAgent += Device.Model;
                        if (hasOs)
                            userAgent += "; ";
                    }
                    if (hasOs)
                        userAgent += Device.SystemVersion;
                    userAgent += ")";
                }
                return userAgent;
            }
        }

        public static double? SecondsUntil(DateTime? date)
        {
            double? secondsUntil;
            if (date != null)
                secondsUntil = (date.Value.ToLocalTime() - DateTime.Now).TotalSeconds;
            else
                secondsUntil = null;
            return secondsUntil;
        }

        public static string TimeAgo(DateTime date, bool isUtc = true)
        {
            string display;
            if (date != null)
            {
                TimeSpan diff = (isUtc ? DateTime.UtcNow : DateTime.Now) - date;
                if (diff.TotalDays >= 1)
                    display = (int)diff.TotalDays + " day" + ((int)diff.TotalDays > 1 ? "s" : "") + " ago";
                else if (diff.TotalHours >= 1)
                    display = (int)diff.TotalHours + " hour" + ((int)diff.TotalHours > 1 ? "s" : "") + " ago";
                else if (diff.TotalMinutes >= 1)
                    display = (int)diff.TotalMinutes + " minute" + ((int)diff.TotalMinutes > 1 ? "s" : "") + " ago";
                else if (diff.TotalSeconds >= 1)
                    display = (int)diff.TotalSeconds + " second" + ((int)diff.TotalSeconds > 1 ? "s" : "") + " ago";
                else
                    display = "Just Now";
            }
            else
                display = "";
            return display;
        }

        public static string TimeAgoShort(DateTime date, bool isUtc = true)
        {
            string display;
            if (date != null)
            {
                TimeSpan diff = (isUtc ? DateTime.UtcNow : DateTime.Now) - date;
                if (diff.TotalDays >= 1)
                    display = (int)diff.TotalDays + "d\nago";
                else if (diff.TotalHours >= 1)
                    display = (int)diff.TotalHours + "h\nago";
                else if (diff.TotalMinutes >= 1)
                    display = (int)diff.TotalMinutes + "m\nago";
                else if (diff.TotalSeconds >= 1)
                    display = (int)diff.TotalSeconds + "s\nago";
                else
                    display = "Just Now";
            }
            else
                display = "";
            return display;
        }

        public static int AreaCount
        {
            get
            {
                return Profile != null && Profile.AreaSubsList != null ? Profile.AreaSubsList.Count : 0;
            }
        }

        public static int SuggestedAreaCount
        {
            get
            {
                return Profile != null && Profile.SuggestedAreaList != null ? Profile.SuggestedAreaList.Count : 0;
            }
        }

        public static AreaModel GetArea(int index)
        {
            return Profile != null && Profile.AreaSubsList != null && index < Profile.AreaSubsList.Count ? Profile.AreaSubsList[index] : null;
        }

        public static List<AreaModel> GetAreas()
        {
            return Profile != null && Profile.AreaSubsList != null ? Profile.AreaSubsList : null;
        }

        public static AreaModel GetSuggestedArea(int index)
        {
            return Profile != null && Profile.SuggestedAreaList != null && index < Profile.SuggestedAreaList.Count ? Profile.SuggestedAreaList[index] : null;
        }

        public static List<AreaModel> GetSuggestedAreas()
        {
            return Profile != null && Profile.SuggestedAreaList != null ? Profile.SuggestedAreaList : null;
        }

        public static AreaModel Area
        {
            get
            {
                if (Profile == null || Profile.AreaSubsList == null)
                {
                    return null;
                }
                return Profile != null && Profile.AreaSubsList != null && Profile.AreaSubsDict.ContainsKey(AreaGUID) ? Profile.AreaSubsDict[AreaGUID] : Profile.AreaSubsList[0];
            }
        }

        public static string AreaName
        {
            get
            {
                AreaModel area = Area;
                return area != null ? area.areaName : "";
            }
        }

        public static bool HasProfile
        {
            get
            {
                return Profile != null;
                //SLProfile profile = SLProfile;
                //return profile != null && profile.Profile != null;
            }
        }

        public static bool HasNetworks
        {
            get
            {
                return Profile != null ? Profile.NetworkList != null && Profile.NetworkList.Count > 0 : false;
            }
        }

        public static List<SocialNetworkModel> NetworkList
        {
            get
            {
                var profile = Profile;
                return profile != null ? profile.NetworkList : null;
            }
        }

        public static SocialNetworkModel GetNetworkForName(string name)
        {

            var profile = SL.Profile;
            if (profile != null)
            {
                var networks = profile.NetworkList;
                if (networks != null)
                {
                    foreach (SocialNetworkModel network in networks)
                    {
                        if (network.NetworkName == name)
                        {
                            return network;
                        }
                    }
                }
            }
            return null;
        }

        public static bool IsNetworkConnected(string networkName)
        {
            bool didFind = false;
            var networkList = NetworkList;
            if (networkList != null)
            {
                foreach (SocialNetworkModel network in networkList)
                {
                    if (network.NetworkName == networkName)
                    {
                        didFind = true;
                        break;
                    }
                }
            }
            return didFind;
        }

        public static bool HasAreas
        {
            get
            {
                //SLProfile profile = SLProfile;
                //return profile != null && profile.Profile != null && profile.Profile.AreaSubsList != null && profile.Profile.AreaSubsList.Count > 0;
                return Profile != null ? Profile.GetFirstArea() != null : false;
            }
        }

        public static List<AreaModel> AreaList
        {
            get
            {
                return Profile != null ? Profile.AreaSubsList : null;
            }
        }

        public static List<AreaModel> SuggestedAreaList
        {
            get
            {
                return Profile != null ? Profile.SuggestedAreaList : null;
            }
        }

        static ProfileModel _profile;
        public static ProfileModel Profile
        {
            get
            {
                try
                {
                    if (_profile == null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            Manager.DataBase.CreateTable<SerializedModel>();
                            var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE NAME = 'Profile'");
                            if (content.Count > 0)
                            {
                                _profile = JsonConvert.DeserializeObject<ProfileModel>(content[0].Json);
                                if (_profile != null)
                                    _profile.Build();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
                return _profile;
            }
            set
            {
                try
                {
                    //only update the profile if we have a profile since at no point to we want to erase an existing profile
                    if (value != null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            Manager.DataBase.CreateTable<SerializedModel>();

                            Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'Profile'");

                            if (value != null)
                            {
                                SerializedModel serializedModel = new SerializedModel();
                                serializedModel.Json = JsonConvert.SerializeObject(value);
                                serializedModel.Name = "Profile";

                                Manager.DataBase.Insert(serializedModel);
                            }
                        }

                        _profile = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
            }
        }
        /*
        static SLProfile _slProfile;
        public static SLProfile SLProfile
        {
            get
            {
                try
                {
                    if (_slProfile == null)
                    {
                        if (IsOrm)
                        {
                            
                        }
                        else
                        {
                            var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE NAME = 'SLProfile'");
                            if (content.Count > 0)
                            {
                                _slProfile = JsonConvert.DeserializeObject<SLProfile>(content[0].Json);
                                if (_slProfile != null)
                                    _slProfile.Build();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return _slProfile;
            }
            set
            {
                try
                {
                    if (IsOrm)
                    {
                        
                    }
                    else
                    {
                        Manager.DataBase.CreateTable<SerializedModel>();

                        Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'SLProfile'");

                        SerializedModel serializedModel = new SerializedModel();
                        serializedModel.Json = JsonConvert.SerializeObject(value);
                        serializedModel.Name = "SLProfile";

                        Manager.DataBase.Insert(serializedModel);
                    }

                    _slProfile = null;
                }
                catch (Exception ex)
                {

                }
            }
        }
        */

        public static List<FriendModel> FriendList
        {
            get
            {
                var profile = Profile;
                //return new List<FriendModel>
                //{
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Imagine Friend", Score = 40, Rank = 1},
                //    new FriendModel(){  Name = "Me", Score=1, Rank=13234}
                //};
                return profile != null ? profile.FriendPreviewList : null;
            }
        }

        public static List<LeaderBoardModel> LeaderBoardList
        {
            get
            {
                //temp; returns leaderboard for first chall
                List<LeaderBoardModel> leaderBoardList;
                var list = ChallengeList;
                if (list != null && list.Count > 0)
                {
                    leaderBoardList = list[0].LeaderBoardList;
                }
                else
                    leaderBoardList = null;
                return leaderBoardList;
            }
        }

        //static List<TransactionModel> _transactionList;
        //public static List<TransactionModel> TransactionList
        //{
        //    get
        //    {
        //        try
        //        {
        //            if (_transactionList == null)
        //                _transactionList = Manager.DataBase.GetAllWithChildren<TransactionModel>().OrderByDescending(x => x.TransactionDate).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine(@"              ERROR {0}", ex.Message);
        //        }
        //        return _transactionList;
        //    }
        //    set
        //    {
        //        //Manager.DataBase.CreateTable<TransactionModel>();
        //        Manager.DataBase.DeleteAll<TransactionModel>();

        //        if (value != null)
        //            Manager.DataBase.InsertAllWithChildren(value, recursive: true);

        //        _transactionList = null;
        //    }
        //}

        static TransactionPageModel _transactionPages;
        public static TransactionPageModel TransactionPages
        {
            get
            {
                try
                {
                    if (_transactionPages != null)
                    {
                        return _transactionPages;
                    }

                    TransactionPageModel transactionPages = Manager.DataBase.GetAllWithChildren<TransactionPageModel>().FirstOrDefault();
                    if (transactionPages != null)
                    {
                        List<TransactionModel> orderedList = transactionPages.TransactionList/*?.OrderByDescending(x => x.TransactionDate).ToList()*/;

                        _transactionPages = transactionPages;
                        _transactionPages.TransactionList = orderedList;
                    }

                    return _transactionPages;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                    return null;
                }
            }
            set
            {
                _transactionPages = value;
                if (_transactionPages?.TransactionList != null)
                {
                    _transactionPages.TransactionList = _transactionPages.TransactionList/*.OrderByDescending(tr => tr.TransactionDate).ToList()*/;
                }

                Manager.DataBase.DeleteAll<TransactionPageModel>();
                Manager.DataBase.DeleteAll<TransactionModel>();

                if (value == null)
                {
                    return;
                }

                Manager.DataBase.InsertWithChildren(value, recursive: true);

                var transactionPages = Manager.DataBase.GetAllWithChildren<TransactionPageModel>().FirstOrDefault();

            }
        }

        static List<NotificationModel> _notificationList;
        public static List<NotificationModel> NotificationList
        {
            get
            {
                try
                {
                    if (_notificationList == null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE NAME = 'NotificationList'");
                            if (content.Count > 0)
                            {
                                _notificationList = JsonConvert.DeserializeObject<List<NotificationModel>>(content[0].Json);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
                return _notificationList;
            }
            set
            {
                try
                {
                    //only update the profile if we have a profile since at no point to we want to erase an existing profile
                    if (value != null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            Manager.DataBase.CreateTable<SerializedModel>();

                            Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'NotificationList'");

                            if (value != null)
                            {
                                SerializedModel serializedModel = new SerializedModel();
                                serializedModel.Json = JsonConvert.SerializeObject(value);
                                serializedModel.Name = "NotificationList";

                                Manager.DataBase.Insert(serializedModel);
                            }
                        }

                        _notificationList = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
            }
        }

        static NotificationResponseModel _pendingNotification;
        public static NotificationResponseModel PendingNotification
        {
            get
            {
                try
                {
                    if (_pendingNotification == null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE NAME = 'Notification'");
                            if (content.Count > 0)
                            {
                                _pendingNotification = JsonConvert.DeserializeObject<NotificationResponseModel>(content[0].Json);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
                return _pendingNotification;
            }
            set
            {
                try
                {
                    //only update the profile if we have a profile since at no point to we want to erase an existing profile
                    if (value != null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            Manager.DataBase.CreateTable<SerializedModel>();

                            Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'Notification'");

                            if (value != null)
                            {
                                SerializedModel serializedModel = new SerializedModel();
                                serializedModel.Json = JsonConvert.SerializeObject(value);
                                serializedModel.Name = "Notification";

                                Manager.DataBase.Insert(serializedModel);
                            }
                        }

                        _pendingNotification = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
            }
        }

        static FeedResponseModel _feed;
        public static FeedResponseModel Feed
        {
            get
            {
                try
                {
                    if (_feed == null)
                    {
                        if (IsOrm)
                        {
                            var list = Manager.DataBase.GetAllWithChildren<FeedResponseModel>(recursive: true);
                            _feed = list != null ? list[0] : null;
                        }
                        else
                        {
                            var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE Name = 'SLFeed'");
                            if (content.Count > 0)
                                _feed = JsonConvert.DeserializeObject<FeedResponseModel>(content[0].Json);
                        }
                        if (_feed != null && _feed.FeedPage != null && _feed.FeedPage.FeedItemList != null)
                        {
                            foreach (FeedItemModel item in _feed.FeedPage.FeedItemList)
                                item.Build();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
                return _feed;
            }
            set
            {
                try
                {
                    if (IsOrm)
                    {

                        Manager.DataBase.DropTable<FeedIcon>();
                        Manager.DataBase.DropTable<FeedTapAction>();
                        Manager.DataBase.DropTable<FeedTextQuote>();
                        Manager.DataBase.DropTable<FeedActionModel>();
                        Manager.DataBase.DropTable<FeedHeaderModel>();
                        Manager.DataBase.DropTable<FeedContentModel>();
                        Manager.DataBase.DropTable<FeedItemModel>();
                        Manager.DataBase.DropTable<FeedModel>();
                        Manager.DataBase.DropTable<NotificationModel>();
                        Manager.DataBase.DropTable<FeedResponseModel>();

                        Manager.DataBase.CreateTable<FeedIcon>();
                        Manager.DataBase.CreateTable<FeedTapAction>();
                        Manager.DataBase.CreateTable<FeedTextQuote>();
                        Manager.DataBase.CreateTable<FeedActionModel>();
                        Manager.DataBase.CreateTable<FeedHeaderModel>();
                        Manager.DataBase.CreateTable<FeedContentModel>();
                        Manager.DataBase.CreateTable<FeedItemModel>();
                        Manager.DataBase.CreateTable<FeedModel>();
                        Manager.DataBase.CreateTable<NotificationModel>();
                        Manager.DataBase.CreateTable<FeedResponseModel>();

                        Manager.DataBase.DeleteAll<FeedIcon>();
                        Manager.DataBase.DeleteAll<FeedTapAction>();
                        Manager.DataBase.DeleteAll<FeedTextQuote>();
                        Manager.DataBase.DeleteAll<FeedActionModel>();
                        Manager.DataBase.DeleteAll<FeedHeaderModel>();
                        Manager.DataBase.DeleteAll<FeedContentModel>();
                        Manager.DataBase.DeleteAll<FeedItemModel>();
                        Manager.DataBase.DeleteAll<FeedModel>();
                        Manager.DataBase.DeleteAll<NotificationModel>();
                        Manager.DataBase.DeleteAll<FeedResponseModel>();

                        if (value != null)
                            Manager.DataBase.InsertOrReplace(value);
                    }
                    else
                    {
                        Manager.DataBase.CreateTable<SerializedModel>();

                        Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'SLFeed'");

                        if (value != null)
                        {
                            SerializedModel serializedModel = new SerializedModel();
                            serializedModel.Json = JsonConvert.SerializeObject(value);
                            serializedModel.Name = "SLFeed";

                            Manager.DataBase.InsertOrReplace(serializedModel);
                        }
                    }

                    _feed = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
            }
        }

        public static List<FeedItemModel> FeedList
        {
            get
            {
                var feed = SL.Feed;
                return feed != null && feed.FeedPage != null ? feed.FeedPage.FeedItemList : null;
            }
        }

        public static FeedItemModel FeedItem(int index)
        {
            FeedItemModel feedItem = null;
            var feed = SL.Feed;
            if (feed != null)
            {
                var page = feed.FeedPage;
                if (page != null)
                {
                    var list = page.FeedItemList;
                    if (list != null && index < list.Count)
                        feedItem = list[index];
                }
            }
            return feedItem;
        }

        static List<ChallengeTypeModel> _challengeTypeList;
        public static List<ChallengeTypeModel> ChallengeTypeList
        {
            get
            {
                try
                {
                    if (_challengeTypeList == null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE NAME = 'ChallengeTypeList'");
                            if (content.Count > 0)
                            {
                                _challengeTypeList = JsonConvert.DeserializeObject<List<ChallengeTypeModel>>(content[0].Json);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
                return _challengeTypeList;
            }
            set
            {
                try
                {
                    //only update the profile if we have a profile since at no point to we want to erase an existing profile
                    if (value != null)
                    {
                        if (IsOrm)
                        {

                        }
                        else
                        {
                            Manager.DataBase.CreateTable<SerializedModel>();

                            Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'ChallengeTypeList'");

                            if (value != null)
                            {
                                SerializedModel serializedModel = new SerializedModel();
                                serializedModel.Json = JsonConvert.SerializeObject(value);
                                serializedModel.Name = "ChallengeTypeList";

                                Manager.DataBase.Insert(serializedModel);
                            }
                        }

                        _challengeTypeList = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
            }
        }

        static List<ChallengeModel> _challengeList;
        public static List<ChallengeModel> ChallengeList
        {
            get
            {
                try
                {
                    if (IsOrm)
                    {
                        if (_challengeList == null)
                        {
                            _challengeList = Manager.DataBase.GetAllWithChildren<ChallengeModel>();
                        }
                    }
                    else
                    {
                        var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE Name = 'SLChallenge'");
                        if (content.Count > 0)
                            _challengeList = JsonConvert.DeserializeObject<List<ChallengeModel>>(content[0].Json);
                    }

                    if (_challengeList == null)
                    {
                        _challengeList = new List<ChallengeModel>();
                    }
                    //_challengeList = new List<ChallengeModel>();// Manager.DataBase.GetAllWithChildren<ChallengeModel>();
                }
                catch (Exception ex)
                {
                    _challengeList = new List<ChallengeModel>();
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
                return _challengeList;
            }
            set
            {
                if (IsOrm)
                {
                    Manager.DataBase.CreateTable<ChallengeModel>();
                    Manager.DataBase.CreateTable<ChallengeAnswerModel>();
                    Manager.DataBase.CreateTable<LeaderBoardModel>();
                    Manager.DataBase.CreateTable<LeaderBoardEntryModel>();

                    Manager.DataBase.DeleteAll<ChallengeModel>();
                    Manager.DataBase.DeleteAll<ChallengeAnswerModel>();
                    Manager.DataBase.DeleteAll<LeaderBoardModel>();
                    Manager.DataBase.DeleteAll<LeaderBoardEntryModel>();

                    if (value != null)
                        Manager.DataBase.InsertAllWithChildren(value, recursive: true);
                }
                else
                {
                    Manager.DataBase.CreateTable<SerializedModel>();

                    Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'SLChallenge'");

                    if (value != null)
                    {
                        SerializedModel serializedModel = new SerializedModel();
                        serializedModel.Json = JsonConvert.SerializeObject(value);
                        serializedModel.Name = "SLChallenge";

                        Manager.DataBase.InsertOrReplace(serializedModel);
                        var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE Name = 'SLChallenge'");
                    }
                }
                _challengeList = null;
            }
        }

        static List<RewardItemModel> _rewardList;
        public static List<RewardItemModel> RewardList
        {
            get
            {
                try
                {
                    if (IsOrm)
                    {
                        if (_rewardList == null)
                            _rewardList = Manager.DataBase.GetAllWithChildren<RewardItemModel>();
                    }
                    else
                    {
                        var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE Name = 'SLReward'");
                        if (content.Count > 0)
                            _rewardList = JsonConvert.DeserializeObject<List<RewardItemModel>>(content[0].Json);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"              ERROR {0}", ex.Message);
                }
                return _rewardList;
            }
            set
            {
                if (IsOrm)
                {
                    Manager.DataBase.CreateTable<RewardItemModel>();
                    Manager.DataBase.CreateTable<FriendModel>();

                    Manager.DataBase.DeleteAll<RewardItemModel>();
                    Manager.DataBase.DeleteAll<FriendModel>();

                    if (value != null)
                        Manager.DataBase.InsertAllWithChildren(value, recursive: true);
                }
                else
                {
                    Manager.DataBase.CreateTable<SerializedModel>();

                    Manager.DataBase.Query<SerializedModel>("DELETE FROM SerializedModel WHERE NAME = 'SLReward'");

                    if (value != null)
                    {
                        SerializedModel serializedModel = new SerializedModel();
                        serializedModel.Json = JsonConvert.SerializeObject(value);
                        serializedModel.Name = "SLReward";

                        Manager.DataBase.InsertOrReplace(serializedModel);
                        var content = Manager.DataBase.Query<SerializedModel>("SELECT * FROM SerializedModel WHERE Name = 'SLReward'");
                    }
                }
                _rewardList = null;
            }
        }

        public static string AreaGUID
        {
            get => AppSettings.GetValueOrDefault("AreaGUID", string.Empty);
            set
            {
                if (AreaGUID != value)
                {
                    //IsBusy = true;
                    //OnRefreshFeedDidBegin?.Invoke(typeof(SL), new EventArgs());
                    AppSettings.AddOrUpdateValue("AreaGUID", value);
                    //RefreshAll();
                }
            }
        }

        public static bool DidTrackAttribution
        {
            get
            {
                return AppSettings.GetValueOrDefault("DidTrackAttribution", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("DidTrackAttribution", value);
            }
        }

        public static string DeviceUUID
        {
            get
            {
                string deviceUUID = AppSettings.GetValueOrDefault("DeviceUUID", string.Empty);
                if (string.IsNullOrEmpty(deviceUUID))
                {
                    Guid guid = Guid.NewGuid();
                    deviceUUID = guid.ToString().Replace("-", "").ToUpper();
                    AppSettings.AddOrUpdateValue("DeviceUUID", deviceUUID);


                }
                return deviceUUID;
                //return "05C1D94CD5A0445FA4DAA92539F44521";
                //return "5EE16D6BFE814E87A90CAD331187C1DE";
            }
        }

        public static bool IsFirstLaunch
        {
            get
            {
                string value = AppSettings.GetValueOrDefault("IsFirstLaunch", string.Empty);
                if (string.IsNullOrEmpty(value) || value == "true")
                {

                    return true;


                }
                return false;
            }
            set
            {
                AppSettings.AddOrUpdateValue("IsFirstLaunch", "false");
            }
        }

        public SL()
        {

        }

        public static void CreateTable(string LocalDataBasePath)
        {
            using (var conn = new SQLite.SQLiteConnection(LocalDataBasePath))
            {
                conn.CreateTable<Person>();
            }
        }

        public static void AddPerson(string LocalDataBasePath, string firstName, string lastName)
        {
            var person = new Person { FirstName = firstName, LastName = lastName };
            using (var db = new SQLite.SQLiteConnection(LocalDataBasePath))
            {
                db.Insert(person);
            }
        }

        public static int GetCount(string LocalDataBasePath)
        {
            using (var db = new SQLite.SQLiteConnection(LocalDataBasePath))
            {
                var count = db.ExecuteScalar<int>("SELECT Count(*) FROM Person");
                return count;
            }
        }

        public static async Task<SocialNetworkModel> CheckInFacebook(string token, double lat, double lon)
        {
            var facebookService = new FacebookService();
            var network = await facebookService.GetSocialNetworkAsync(token);

            //SocialNetworkModel network = new SocialNetworkModel();
            //network.NetworkName = "Facebook";
            //network.AccessToken = token;
            //network.EmailAddress = email;

            //check if we have a profile, create if not before saving network
            if (!SL.HasProfile)
            {
                ProfileModel aNewProfile = new ProfileModel();
                //aNewProfile.UserStatus = "AUTOMERGE";
                aNewProfile.UserName = network.NUserName;
                aNewProfile.EmailAddress = network.EmailAddress;
                await Manager.SaveProfileAsync(aNewProfile);
            }

            await Manager.SaveNetworkAsync(network, true);

            //ProfileModel profile = new ProfileModel();
            //profile.UserName = "Test";
            //await Manager.SaveProfileAsync(profile);

            await Manager.FinalCheckInAsync(lat, lon);

            return network;
        }

        public static async Task<SocialNetworkModel> CheckInTwitter(Account account, double lat, double lon)
        {
            var twitterService = new TwitterService();
            var network = await twitterService.GetSocialNetworkAsync(account);

            //SocialNetworkModel network = new SocialNetworkModel();
            //network.NetworkName = "Facebook";
            //network.AccessToken = token;
            //network.EmailAddress = email;

            //check if we have a profile, create if not before saving network
            if (!SL.HasProfile)
            {
                ProfileModel aNewProfile = new ProfileModel();
                //aNewProfile.UserStatus = "AUTOMERGE";
                aNewProfile.UserName = network.NUserName;
                aNewProfile.EmailAddress = network.EmailAddress;
                await Manager.SaveProfileAsync(aNewProfile);
            }

            await Manager.SaveNetworkAsync(network, true);
            /*
            ProfileModel profile = new ProfileModel();
            //profile.UserName = "Test";
            await Manager.SaveProfileAsync(profile);
            */
            await Manager.FinalCheckInAsync(lat, lon);

            return network;
        }




        public static async Task<SocialNetworkModel> CheckInInstagram(string networkName, string token, double lat, double lon)
        {


            var instagramService = new InstagramService();
            var network = await instagramService.GetSocialNetworkAsync(token);
            //check if we have a profile, create if not before saving network
            if (!SL.HasProfile)
            {
                ProfileModel aNewProfile = new ProfileModel();
                await Manager.SaveProfileAsync(aNewProfile);
            }

            await Manager.SaveNetworkAsync(network, true);

            await Manager.FinalCheckInAsync(lat, lon);

            return network;
        }

        public static async Task<ResponseModel> CheckInNetwork(SocialNetworkModel network, double lat, double lon)
        {
            ResponseModel response = null;

            //check if we need to create or update profile
            bool shouldUpdateProfile = false;
            ProfileModel aProfile = SL.Profile;
            if (aProfile == null)
            {
                aProfile = new ProfileModel();
                shouldUpdateProfile = true;
            }

            if (string.IsNullOrEmpty(aProfile.EmailAddress) && !string.IsNullOrEmpty(network.EmailAddress))
            {
                aProfile.EmailAddress = network.EmailAddress;
                shouldUpdateProfile = true;
            }

            if (string.IsNullOrEmpty(aProfile.UserName) && !string.IsNullOrEmpty(network.NUserName))
            {
                aProfile.UserName = network.NUserName;
                shouldUpdateProfile = true;
            }

            if (shouldUpdateProfile)
                response = await Manager.SaveProfileAsync(aProfile);

            if (!shouldUpdateProfile || (response != null && response.ResponseCode > 0))
            {
                response = await Manager.SaveNetworkAsync(network, true);
                if (response.ResponseCode > 0)
                    response = await Manager.FinalCheckInAsync(lat, lon);
            }
            return response;
        }

        /*
        //FEED
        public static List<FeedItemModel> GetFeedItems
        {
            get
            {
                List<FeedItemModel> items = null;
                SLFeed feed = SL.Manager.Feed;
                if (feed != null)
                {
                    FeedModel fm = feed.FeedPage;
                    if (fm != null)
                    {
                        items = fm.FeedItemList;
                    }
                }
                return items;
            }
        }

        public static int GetFeedItemsCount
        {
            get
            {
                List<FeedItemModel> items = GetFeedItems;
                int count = (items != null) ? items.Count : 0;
                return count;
            }
        }

        public static FeedItemModel GetFeedItem(int index)
        {
            List<FeedItemModel> items = GetFeedItems;
            FeedItemModel item = (items != null && index < items.Count) ? items[index] : null;
            return item;
        }

        //Challenges
        public static List<ChallengeModel> GetChallengeItems
        {
            get
            {
                List<ChallengeModel> items = null;
                SLChallenge challenges = SL.Manager.Challenges;
                if (challenges != null)
                {
                    items = challenges.ChallengeList;
                }
                return items;
            }
        }

        public static int GetChallengeItemsCount
        {
            get
            {
                List<ChallengeModel> items = GetChallengeItems;
                int count = (items != null) ? items.Count : 0;
                return count;
            }
        }

        public static ChallengeModel GetChallengeItem(int index)
        {
            List<ChallengeModel> items = GetChallengeItems;
            ChallengeModel item = (items != null && index < items.Count) ? items[index] : null;
            return item;
        }

        //Rewards
        public static List<RewardModel> GetRewardItems
        {
            get
            {
                List<RewardModel> items = null;
                SLReward rewards = SL.Manager.Rewards;
                if (rewards != null)
                {
                    items = rewards.RewardList;
                }
                return items;
            }
        }

        public static int GetRewardItemsCount
        {
            get
            {
                List<RewardModel> items = GetRewardItems;
                int count = (items != null) ? items.Count : 0;
                return count;
            }
        }

        public static RewardModel GetRewardItem(int index)
        {
            List<RewardModel> items = GetRewardItems;
            RewardModel item = (items != null && index < items.Count) ? items[index] : null;
            return item;
        }

        //Database
        public static void UpdateChallengesDB(string LocalDataBasePath = null)
        {
            using (var db = new SQLite.SQLiteConnection(LocalDataBasePath))
            {
                List<ChallengeModel> items = Manager.Challenges != null ? Manager.Challenges.ChallengeList : null;
                if (items != null)
                {
                    db.CreateTable<ChallengeModel>();
                    db.CreateTable<ChallengeAnswerModel>();
                    db.CreateTable<LeaderBoardModel>();
                    db.CreateTable<LeaderBoardEntryModel>();

                    db.InsertAllWithChildren(items, recursive: true);


                }
            }
        }

        public static List<ChallengeModel> GetChallengesDB()
        {
            List<ChallengeModel> items = new List<ChallengeModel>();
            try
            {
                if (!string.IsNullOrEmpty(SL.Manager.DataBasePath))
                {
                    using (var db = new SQLite.SQLiteConnection(SL.Manager.DataBasePath))
                    {
                        items = db.GetAllWithChildren<ChallengeModel>();
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
            return items;
        }
        */
        public static async Task<byte[]> LoadImage(string imageUrl)
        {
            var httpClient = new HttpClient();

            Task<byte[]> contentsTask = httpClient.GetByteArrayAsync(imageUrl);

            // await! control returns to the caller and the task continues to run on another thread
            var contents = await contentsTask;

            // load from bytes
            return contents;//UIImage.LoadFromData(NSData.FromArray(contents));
        }

        public static void RefreshProfile()
        {
            Manager.GetProfileAsync();
        }

        public static void RefreshAll()
        {
            Manager.GetFeedAsync(RefreshFeedComplete);
            Manager.GetProfileAsync();
            Manager.GetRewardsAsync();
            Manager.GetChallengesAsync();
        }

        public static string FeedNextPage
        {
            get
            {
                FeedResponseModel feed = Feed;
                return feed != null && feed.FeedPage != null ? feed.FeedPage.NextPage : null;
            }
        }

        public static void Logout()
        {
            var t =  Manager.DataBase.DropTable<SerializedModel>();
            AppSettings.AddOrUpdateValue("DeviceUUID", string.Empty);
            AppSettings.AddOrUpdateValue("NotificationStatus", string.Empty);
            _profile = null;
            AreaGUID = string.Empty;
        }

        public static DateTime? NextEvent(DateTime? event1, DateTime? event2)
        {
            DateTime? nextEvent = null;
            double? secondsUntil = SL.SecondsUntil(event1);
            if (secondsUntil != null && secondsUntil.Value > 0)
                nextEvent = event1;
            else if (nextEvent == null)
            {
                secondsUntil = SL.SecondsUntil(event2);
                if (secondsUntil != null && secondsUntil.Value > 0)
                    nextEvent = event2;
            }
            return nextEvent;
        }

        public static string NextEventStatus(DateTime? nextEvent, DateTime? event1, string status1, DateTime? event2, string status2)
        {
            string status = string.Empty;
            if (nextEvent != null)
            {
                if (nextEvent == event1)
                    status = status1;
                else if (nextEvent == event2)
                    status = status2;
            }
            return status;
        }

        public static string FormatCountdownString(double? SecondsUntil)
        {
            string timeString = string.Empty;
            if (SecondsUntil != null && SecondsUntil.Value > 0)
            {
                TimeSpan span = new TimeSpan(TimeSpan.TicksPerSecond * (long)SecondsUntil.Value);
                timeString = span.Days > 0 ? span.Days + "d" : span.Hours + "h " + span.Minutes + "m " + span.Seconds + "s";
            }
            return timeString;
        }

        public static List<ChallengeTypeSummaryModel> ChallengeSummaryList
        {
            get
            {
                List<ChallengeTypeSummaryModel> summaryList = new List<ChallengeTypeSummaryModel>();
                List<ChallengeModel> challengeList = ChallengeList;
                if (challengeList != null)
                {
                    //add challenge summary for each challenge type
                    Dictionary<string, ChallengeTypeSummaryModel> challengeSummary = new Dictionary<string, ChallengeTypeSummaryModel>();
                    foreach (ChallengeModel challenge in challengeList)
                    {
                        //string key = challenge.TypeCode;
                        string key = challenge.TypeCodeDisplayName;
                        if (String.IsNullOrEmpty(key))
                            continue;

                        if (!challengeSummary.ContainsKey(key))
                            challengeSummary[key] = new ChallengeTypeSummaryModel(0, 0, challenge.TypeCode, challenge.TypeCodeDisplayName);
                        challengeSummary[key].Total++;
                        if (challenge.Status.ToLower() == "complete")
                            challengeSummary[key].ChallengeCompCount++;
                    }
                    foreach (var entry in challengeSummary)
                        summaryList.Add(entry.Value);
                }
                return summaryList;
            }
        }

        public static List<SummaryModel> SummaryList
        {
            get
            {
                List<SummaryModel> summaryList = new List<SummaryModel>();
                var profile = Profile;
                if (profile != null)
                {
                    //add all rewards summary
                    var rewardList = RewardList;
                    if (rewardList != null)
                        summaryList.Add(new RewardSummaryModel(profile.UnlockedRewardsCount, profile.PurchasedRewardsCount, rewardList.Count));

                    //add all challs summary
                    var challengeList = ChallengeList;
                    if (challengeList != null)
                        summaryList.Add(new ChallengeSummaryModel(profile.ChallengeCompCount, challengeList.Count));
                }

                //add for each chall type summary
                summaryList.AddRange(ChallengeSummaryList);

                return summaryList;
            }
        }

        public static List<T> GetItems<T>(Expression<Func<T, bool>> where = null) where T : BaseTable, new()
        {
            TableQuery<T> items;
            lock (locker)
            {
                if (where != null)
                    items = Manager.DataBase.Table<T>().Where(where);
                else
                    items = Manager.DataBase.Table<T>();
            }
            try
            {
                if (items != null)
                {
                    var result = new List<T>();
                    foreach (var item in items)
                    {
                        if (item != null)
                        {
                            try
                            {
                                var itemType = item.GetType();
                                var propObj = itemType.GetRuntimeProperty("Object");
                                if (propObj != null && propObj.CanWrite)
                                {
                                    var propCache = itemType.GetRuntimeProperty("Cache");
                                    if (propCache != null && propCache.CanRead && propCache.PropertyType == typeof(string))
                                        propObj.SetValue(item, JsonConvert.DeserializeObject(propCache.GetValue(item) as string, propObj.PropertyType));
                                }
                            }
                            catch (Exception ex)
                            {
                                Report($"GetItems(): Deserialization Error", ex);
                            }
                        }
                        result.Add(item);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Report("GetItems()", ex);
            }
            return null;
        }

        public static T SaveItem<T>(T item, bool insertIfNotHas = false, bool updateIfHas = true) where T : BaseTable, new()
        {
            if (item == null)
                return null;
            try
            {
                var t = item.GetType();
                var propCache = t.GetRuntimeProperty("Cache");
                if (propCache != null && propCache.CanWrite && propCache.PropertyType == typeof(string))
                {
                    var propObj = t.GetRuntimeProperty("Object");
                    if (propObj != null && propObj.CanRead)
                        propCache.SetValue(item, JsonConvert.SerializeObject(propObj.GetValue(item)));
                }
            }
            catch (Exception ex)
            {
                Report("SaveItem(): Serialization Error", ex);
            }
            item.UserId = userID;

            try
            {
                if (string.IsNullOrEmpty(item.ID))
                {
                    item.ID = Guid.NewGuid().ToString();
                    lock (locker)
                    {
                        if (Manager.DataBase.Insert(item) > 0)
                            return item;
                        else
                            return null;
                    }
                }
                else
                {
                    //var cache = GetItem<T>(item.ID);
                    var index = 0;
                    lock (locker)
                    {
                        var cache = Manager.DataBase.Table<T>().Where(x => x.ID == item.ID).ToList();
                        if ((cache == null || cache.Count == 0) && insertIfNotHas)
                            index = Manager.DataBase.Insert(item);
                        else if (updateIfHas)
                            index = Manager.DataBase.Update(item);
                    }

                    return (index > 0) ? item : null;
                }
            }
            catch (Exception ex)
            {
                Report("Error SaveItem()", ex);
                return null;
            }
        }

        public static int DeleteItem<T>(T item) where T : BaseTable
        {
            lock (locker)
            {
                try
                {
                    if (item != null && !string.IsNullOrEmpty(item.ID))
                        return Manager.DataBase.Delete(item);
                }
                catch (Exception ex)
                {
                    Report($"Error GetItem():", ex);
                }
                return -1;
            }
        }

        public static void Report(string message, Exception ex = null)
        {
            if (ex == null)
                System.Diagnostics.Debug.WriteLine($"{typeof(SL).Name}: {message}");
            else
            {
                var msg = $"{typeof(SL).Name}: {message}: {ex.Message}";
                System.Diagnostics.Debug.WriteLine(msg);
                throw new Exception(msg);
            }

        }



        private static void RefreshFeedComplete(FeedResponseModel response)
        {
            IsBusy = false;
            //OnRefreshFeedComplete?.Invoke(typeof(SL), response);
        }
    }
}
