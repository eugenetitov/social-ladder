using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace SocialLadder.Models
{
    public class LeaderBoardEntryModel
    {
        public string ImageURL { get; set; }
        public int Rank { get; set; }
        public string EntryName { get; set; }
        public int NumberValue { get; set; }

        [JsonIgnore, PrimaryKeyAttribute]
        public int ID { get; set; }
        [JsonIgnore, ForeignKey(typeof(LeaderBoardModel))]
        public int LeaderBoardID { get; set; }
    }

    public class LeaderBoardModel
    {
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LeaderBoardEntryModel> EntryList { get; set; }
        public string Name { get; set; }

        [JsonIgnore, PrimaryKeyAttribute]
        public int ID { get; set; }
        [JsonIgnore, ForeignKey(typeof(ChallengeModel))]
        public int ChallengeID { get; set; }
    }

    public class ChallengeAnswerModel
    {
        public string AnswerName { get; set; }
        public int Sequence { get; set; }
        public string AnswerCode { get; set; }
        [PrimaryKeyAttribute]
        public int ID { get; set; }
        [JsonIgnore, ForeignKey(typeof(ChallengeModel))]     // Specify the foreign key
        public int ChallengeID { get; set; }
        public bool isWriteIn { get; set; }
        public string writeInPrompt { get; set; }
    }

    public class WriteInModel
    {
        public string WriteIn { get; set; }
    }

    public class ChallengeModel
    {
        public string FBShareType { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string DocPathURL { get; set; }
        public string Image { get; set; }
        public string ImageLowRes { get; set; }
        public string TypeCode { get; set; }
        public string TypeCodeDisplayName { get; set; }
        public string Group { get; set; }
        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public string Status { get; set; }
        public string InstaURL { get; set; }
        public int? Sequence { get; set; }
        public int PointValue { get; set; }
        public int CompPointValue { get; set; }
        public int? PointsPerInstance { get; set; }
        [PrimaryKeyAttribute]
        public int ID { get; set; }
        public int? SelectAnswerID { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ChallengeAnswerModel> AnswerList { get; set; }
        public int? CompletedCount { get; set; }
        public string IconImageURL { get; set; }
        public int? TargetCount { get; set; }
        public string Subtitle { get; set; }
        public string InstaCaption { get; set; }
        public string Question { get; set; }
        public string SmallImageURL { get; set; }
        public string ShareTemplateURL { get; set; }
        public string ShareImage { get; set; }
        public string ChallengeDetailsURL { get; set; }
        public double? LocationLat { get; set; }
        public double? LocationLong { get; set; }
        public double? RadiusMeters { get; set; }
        public DateTime? AutoUnlockDate { get; set; }
        public bool LockStatus { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public string LockReason { get; set; }
        public int? SecondsUntilUnlock { get; set; }
        public int? SecondsUntilExpire { get; set; }
        public string LockIndicatorImageURL { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LeaderBoardModel> LeaderBoardList { get; set; }
        public string InviteToChallengeTemplateURL { get; set; }
        public string PointIconURL { get; set; }
        public bool IsFixedContent { get; set; }
        public bool DisallowSharing { get; set; }
        public bool UseDefaultCodes { get; set; }
        public bool UseTeamCodes { get; set; }
        public bool UpdateAll { get; set; }
        public string templateName { get; set; }
        public string templateStartOffset { get; set; }
        public string templateEndOffset { get; set; }
        public string templateDiscountNum { get; set; }
        public string templateDiscountAmount { get; set; }
        public string templateDiscountType { get; set; }
        public string templateDiscountUse { get; set; }
        public bool UsePointsPerDollar { get; set; }
        public decimal PointsPerDollar { get; set; }
        public bool IsGuestList { get; set; }
        public string TargetObjectURL { get; set; }
        public string TargetObjectId { get; set; }
        public int MinTags { get; set; }
        public bool IsCommentReq { get; set; }
        public bool IsLikeReq { get; set; }
        public bool IsReshareReq { get; set; }
        public bool IsEventAttendReq { get; set; }
        public bool IsReviewReq { get; set; }
        public int MinStars { get; set; }
        public bool IsSurvey { get; set; }
        public string MulipleShops { get; set; }
        public bool? AllowUserCompletion { get; set; }
        public bool? CollateralReview { get; set; }

        DateTime? NextEvent
        {
            get
            {
                return SL.NextEvent(AutoUnlockDate, EffectiveEndDate);
            }
        }

        public double? SecondsUntilNextEvent
        {
            get
            {
                return SL.SecondsUntil(NextEvent);
            }
        }

        public string NextEventCountDown
        {
            get
            {
                string countDown = string.Empty;
                DateTime? nextEvent = NextEvent;
                if (nextEvent != null)
                {
                    countDown = SL.FormatCountdownString(SL.SecondsUntil(nextEvent));
                    if (!string.IsNullOrEmpty(countDown))
                        countDown = SL.NextEventStatus(nextEvent, AutoUnlockDate, "Unlocks in ", EffectiveEndDate, "Expires in ") + countDown;
                }
                return countDown;
            }
        }
        /*
        public static string GetTypeCodeDisplayName(string typeCode)
        {
            string displayName;
            if (typeCode == "INSTA")
                displayName = "Instagram";
            else if (typeCode == "FACEBOOK")
                displayName = "Facebook";
            else if (typeCode == "TWITTER")
                displayName = "Twitter";
            else if (typeCode == "FB ENGAGEMENT")
                displayName = "Facebook Engagement";
            else if (typeCode == "SHARE")
                displayName = "Share";
            else if (typeCode == "COLLATERAL TRACKING")
                displayName = "Collateral Tracking";
            else if (typeCode == "CHECKIN")
                displayName = "Checkin";
            else if (typeCode == "MC")
                displayName = "Multiple Choice";
            else if (typeCode == "INVITE")
                displayName = "Invite";
            else if (typeCode == "POSTERING")
                displayName = "Postering";
            else if (typeCode == "FLYERING")
                displayName = "Flyering";
            else
                displayName = typeCode;
            return displayName;
        }
        */
        public static string GetTypeCodeColor(string typeCode, string displayName)
        {
            string color = "000000";
            if (typeCode == "INSTA")
                color = "4CD984";
            else if (typeCode == "SHARE")
                color = "684CD9";
            else if (typeCode == "MC")
            {
                if (displayName == "Survey")
                    color = "4AE8B1";
                else
                    color = "BD4CD9";
            }
            else if (typeCode == "INVITE")
            {
                if (displayName == "Invite to Buy")
                    color = "68D94C";
                else if (displayName == "Invite to Join")
                    color = "D94CA0";
            }
            else if (typeCode == "CHECKIN")
                color = "4C84D9";
            else if (typeCode == "COLLATERAL TRACKING")
                color = "684CD9";
            else if (typeCode == "POSTERING")
                color = "684CD9";
            else if (typeCode == "FB ENGAGEMENT")
                color = "BD4CD9";
            else if (typeCode == "FLYERING")
                color = "F36666";
            else if (typeCode == "POSTERING")
                color = "37BEEC";
            else if (typeCode == "MANUAL")
                color = "EA804B";
            else if (typeCode == "SIGNUP")
                color = "324868";
            else if (typeCode == "DOCSUBMIT")
                color = "724372";
            else if (typeCode == "FACEBOOK")
                color = "3B5998";
            else if (typeCode == "TWITTER")
                color = "38A1F3";
            return color;
        }
        /*
        public string TypeCodeDisplayName
        {
            get; set;
            //get
            //{
            //    return GetTypeCodeDisplayName(TypeCode);
            //}
        }
        */
    }

    public class ChallengeTypeModel
    {
        public int Total { get; set; }
        public int TotalComplete { get; set; }
        public string DisplayName { get; set; }
        public string TypeCode { get; set; }
        public string Group { get; set; }
        public string ImageUrl { get; set; }
        public string Color { get; set; }
    }

    public class ChallengeResponseDetailsModel
    {
        public ChallengeModel Challenge
        {
            get; set;
        }
    }

    public class ChallengeResponseModel : ResponseModel
    {
        public List<ChallengeModel> ChallengeList { get; set; }
        public ChallengeModel Challenge { get; set; }
        public List<ChallengeTypeModel> ChallengeTypeList { get; set; }
    }
}
