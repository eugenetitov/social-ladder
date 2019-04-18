using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;

namespace SocialLadder.Models
{
    public class RewardModel
    {
        public string MainImageURL { get; set; }
        public string MainImageURLLowRes { get; set; }
        public string SmallImageURL { get; set; }
        public string SmallImageClickURL { get; set; }
        public double? LocationLat { get; set; }
        public double? LocationLong { get; set; }
        public string LocationName { get; set; }
        public string LocationLine2 { get; set; }
        public string LocationLine3 { get; set; }
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string FavoribilityString { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<FriendModel> FriendsVisitedList { get; set; }
        public string Type { get; set; }
        [PrimaryKey]
        public int ID { get; set; }
        public DateTime? AvailabilityDate { get; set; }
        public DateTime? AutoUnlockDate { get; set; }
        public bool LockStatus { get; set; }
        public bool ButtonLockStatus { get; set; }
        public string LockReason { get; set; }
        public int? SecondsUntilUnlock { get; set; }
        public int? SecondsUntilExpire { get; set; }
        public string RewardRefreshURL { get; set; }

        [JsonIgnore, ForeignKey(typeof(RewardModel))]
        public int ParentRewardID { get; set; }

        //RewardCategory Properties
        public int? NumChildren { get; set; }
        public bool isOffersInline { get; set; }
        public string DrillDownURL { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<RewardItemModel> ChildList { get; set; }

        DateTime? NextEvent
        {
            get
            {
                return SL.NextEvent(AutoUnlockDate, AvailabilityDate);
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
                        countDown = SL.NextEventStatus(nextEvent, AutoUnlockDate, "", AvailabilityDate, "") + countDown;
                }
                return countDown;
            }
        }
    }

    public class RewardItemModel : RewardModel
    {
        [JsonIgnore, ForeignKey(typeof(ProfileModel))]
        public int ProfileID { get; set; }

        public DateTime? OfferExpirationDate { get; set; }
        public decimal? Savings { get; set; }
        public string Description { get; set; }
        public int MinScore { get; set; }
        public string Vendor { get; set; }
        public string Udf1 { get; set; }
        public string Udf2 { get; set; }
        public string CostString { get; set; }
        public string Tier { get; set; }
        public int? RemainingUnits { get; set; }
        public string AccessibilityString { get; set; }
        public string CallToActionString { get; set; }
        public double? MinRelScore { get; set; }
        public bool AllowGifting { get; set; }
        public int MinimumAge { get; set; }
        public string TermsAndConditions { get; set; }
        public string AccessibilityIndicatorImageURL { get; set; }
        public string ShareTemplateURL { get; set; }
        public string GiftTemplateURL { get; set; }
        public string CommitShareTemplateURL { get; set; }
        public string LockIndicatorImageURL { get; set; }
        public string ConfirmationQuestion { get; set; }
        public bool UserWouldQualify { get; set; }
    }
    /*
     * Moved to RewardModel
    public class RewardCat : RewardModel
    {
        public int? NumChildren { get; set; }
        public bool isOffersInline { get; set; }
        public string DrillDownURL { get; set; }
        public List<RewardModel> ChildList { get; set; }
    }
    */
    public class RewardResponseModel : ResponseModel
    {
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<RewardItemModel> RewardList { get; set; }   //server uses RewardModel, but client uses RewardItemModel for deserialize
        public string PassURL { get; set; }
        public string ConfirmationURL { get; set; }
        public RewardItemModel UpdatedReward { get; set; }  //server uses RewardModel, but client uses RewardItemModel for deserialize
        public string RewardAction { get; set; }
    }
}
