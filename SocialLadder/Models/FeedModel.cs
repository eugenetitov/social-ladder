using System;
using System.Collections.Generic;
using SQLite;
using Newtonsoft.Json;
using SQLiteNetExtensions.Attributes;
using SocialLadder.Enums;

namespace SocialLadder.Models
{
    public class UserInputModel
    {
        public string UserInputText
        {
            get; set;
        }
        public int ButtonResponse
        {
            get; set;
        }
    }

    public class FeedModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedResponseModel))]
        public int SLFeedID
        {
            get; set;
        }

        [OneToMany("FeedModelID", CascadeOperations = CascadeOperation.All)]
        public List<FeedItemModel> FeedItemList
        {
            get; set;
        }

        public string NextPage
        {
            get; set;
        }
    }

    public class FeedActionModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedContentModel))]
        public int FeedContentModelID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedItemModel))]
        public int FeedItemModelID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(NotificationModel))]
        public int NotificationModelID
        {
            get; set;
        }

        public string ActionLinkName
        {
            get; set;
        }
        public string ActionScreen
        {
            get; set;
        }

        [TextBlob("ActionParamDictBlobbed")]
        public Dictionary<string, string> ActionParamDict
        {
            get; set;
        }
        [JsonIgnore]
        public string ActionParamDictBlobbed
        {
            get; set;
        }

        public string ActionLinkIconURL
        {
            get; set;
        }

        public string WebRequestURL
        {
            get; set;
        }
    }

    public class FeedHeaderModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore]
        public int FeedItemModelID
        {
            get; set;
        }

        public string Actor
        {
            get; set;
        }
        public string ActorProfileURL
        {
            get; set;
        }
        public string ActorFeedURL
        {
            get; set;
        }
        public string ActionText
        {
            get; set;
        }
        public string ActionTextHTML
        {
            get; set;
        }
        public string ProfilePicURL
        {
            get; set;
        }
        public DateTime? CreationDate
        {
            get; set;
        }

        [OneToMany("FeedHeaderModelID", CascadeOperations = CascadeOperation.All)]
        public List<FeedIcon> ActionTextIconList
        {
            get; set;
        }
    }

    //Used to parse field Content on FeedItemModel
    public class FeedContentModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedItemModel))]
        public int FeedItemModelID
        {
            get; set;
        }

        //[JsonIgnore]
        //public bool IsLiked { get; set; }

        //FeedContentModel
        public string ContentType
        {
            get; set;
        }
        //

        [JsonIgnore]
        [Ignore]
        public FeedContentType[] ContentTypes
        {
            get; set;
        }

        [JsonIgnore]
        public bool IsLiked
        {
            get; set;
        }

        //FeedContentMapModel
        public double Lat
        {
            get; set;
        }
        public double Long
        {
            get; set;
        }
        public string LocationName
        {
            get; set;
        }
        //

        //FeedContentOfferModel
        public string OfferImageURL
        {
            get; set;
        }
        public string OfferTitle
        {
            get; set;
        }
        public string OfferSubTitle
        {
            get; set;
        }
        public string OfferCost
        {
            get; set;
        }
        public string OfferMinScore
        {
            get; set;
        }
        //

        //FeedContentImageModel
        public string ImageURL
        {
            get; set;
        }
        public string ImageURLLowRes
        {
            get; set;
        }
        public string ImageCaption
        {
            get; set;
        }
        [Ignore]
        public FeedActionModel TapAction
        {
            get; set;
        }
        [Ignore]
        public FeedActionModel TapAction2
        {
            get; set;
        }
        public int ImageWidth
        {
            get; set;
        }
        public int ImageHeight
        {
            get; set;
        }
        //

        //FeedContentProfilePictureModel
        public string ProfilePicURL
        {
            get; set;
        }
        public string Caption
        {
            get; set;
        }
        //
    }

    public class FeedContentModelBase
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedItemModel))]
        public int FeedItemModelID
        {
            get; set;
        }

        //[JsonIgnore]
        //public FeedContentType[] ContentTypes { get; set; }

        //[JsonIgnore]
        //public bool IsLiked { get; set; }

        //FeedContentModel
        public string ContentType
        {
            get; set;
        }
        //
        [OneToOne("FeedContentModelID")]
        public FeedActionModel TapAction
        {
            get; set;
        }
        [OneToOne("FeedContentModelID")]
        public FeedActionModel TapAction2
        {
            get; set;
        }

        //FeedContentMapModel
        //public double Lat { get; set; }
        //public double Long { get; set; }
        //public string LocationName { get; set; }
        //

        //FeedContentOfferModel
        //public string OfferImageURL { get; set; }
        //public string OfferTitle { get; set; }
        //public string OfferSubTitle { get; set; }
        //public string OfferCost { get; set; }
        //public string OfferMinScore { get; set; }
        //

        //FeedContentImageModel
        //public string ImageURL { get; set; }
        //public string ImageURLLowRes { get; set; }
        //public string ImageCaption { get; set; }
        //[OneToOne("FeedContentModelID")]
        //public FeedActionModel TapAction { get; set; }
        //public int ImageWidth { get; set; }
        //public int ImageHeight { get; set; }
        //

        //FeedContentProfilePictureModel
        //public string ProfilePicURL { get; set; }
        //public string Caption { get; set; }
        //

        public void Build(string ActionType, List<FeedContentType> sections)
        {
            //Determine applicable content types by ContentType and ActionType
            //ContentType represents the base type of content that is available to display;
            //ActionType allows us to override the layout and display the content in a layout other than the base layout 

            //priority 1: set contenttypes based on actiontype
            if (ActionType == "ITB_COMPLETE")
                sections.Add(FeedContentType.ProductSold);
            else if (ActionType == "REFERRAL_ACCEPTED")
                sections.Add(FeedContentType.FriendJoined);
            //else if (ActionType == "SHARE")
            //sections.Add(FeedContentType.Share);
            else if (ActionType == "COMMIT" || ActionType == "REDEEM")
                sections.Add(FeedContentType.RewardClaimed);
            else
            {
                //if we did not determine an override layout based on actiontype
                //priority 2: set contenttypes based on contenttype
                if (ContentType == "MAP")
                    sections.Add(FeedContentType.Map);
                else if (ContentType == "MEDIA")
                    sections.Add(FeedContentType.Image);
                else if (ContentType == "PROFILE")
                    sections.Add(FeedContentType.Image);
            }
        }
    }

    public class FeedContentMapModel : FeedContentModelBase
    {
        public double Lat
        {
            get; set;
        }
        public double Long
        {
            get; set;
        }
        public string LocationName
        {
            get; set;
        }
    }

    public class FeedContentOfferModel : FeedContentModelBase
    {
        public string OfferImageURL
        {
            get; set;
        }
        public string OfferTitle
        {
            get; set;
        }
        public string OfferSubTitle
        {
            get; set;
        }
        public string OfferCost
        {
            get; set;
        }
        public string OfferMinScore
        {
            get; set;
        }
    }

    public class FeedContentImageModel : FeedContentModelBase
    {
        public string ImageURL
        {
            get; set;
        }
        public string ImageURLLowRes
        {
            get; set;
        }
        public string ImageCaption
        {
            get; set;
        }
        //[OneToOne("FeedContentModelID")]
        //public FeedActionModel TapAction { get; set; }
        public int ImageWidth
        {
            get; set;
        }
        public int ImageHeight
        {
            get; set;
        }
    }

    public class FeedContentProfilePictureModel : FeedContentModelBase
    {
        public string ProfilePicURL
        {
            get; set;
        }
        public string Caption
        {
            get; set;
        }
    }

    public class FeedTapAction
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        public string ActionScreen
        {
            get; set;
        }

        [TextBlob("ActionParamDictBlobbed")]
        public Dictionary<string, string> ActionParamDict
        {
            get; set;
        }
        [JsonIgnore]
        public string ActionParamDictBlobbed
        {
            get; set;
        }
    }

    public class FeedIcon
    {
        public string IconImageURL
        {
            get; set;
        }

        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedTextQuote))]
        public int FeedTextQuoteID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedHeaderModel))]
        public int FeedHeaderModelID
        {
            get; set;
        }
    }

    public class FeedEngagementModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore, ForeignKey(typeof(FeedTextQuote))]
        public int FeedTextQuoteID
        {
            get; set;
        }

        public string EngagementType
        {
            get; set;
        }
        public string Notes
        {
            get; set;
        }
        public string UserName
        {
            get; set;
        }
    }

    public class FeedTextQuote
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [JsonIgnore]
        public bool IsModified { get; set; } = false;

        [JsonIgnore, ForeignKey(typeof(FeedItemModel))]
        public int FeedItemModelID
        {
            get; set;
        }

        public string TextQuoteText
        {
            get; set;
        }
        public string TextQuoteTextHTML
        {
            get; set;
        }
        public string TextQuoteTextColor
        {
            get; set;
        }

        [OneToMany("FeedTextQuoteID", CascadeOperations = CascadeOperation.All)]
        public List<FeedIcon> TextQuoteIconList
        {
            get; set;
        }

        [OneToMany("FeedTextQuoteID", CascadeOperations = CascadeOperation.All)]
        public List<FeedEngagementModel> EngagementList
        {
            get; set;
        }
    }

    public class FeedItemModel
    {
        [JsonIgnore, ForeignKey(typeof(FeedModel))]
        public int FeedModelID
        {
            get; set;
        }

        [OneToOne("FeedItemModelID")]
        public FeedHeaderModel Header
        {
            get; set;
        }

        [OneToOne("FeedItemModelID")]
        public FeedContentModel Content
        {
            get; set;
        }   //parsed from server then used to build BaseContent (cleared after parsing into the specific content received from server and should not be used for layout binding or contraints setup)

        [JsonIgnore]//, OneToOne("FeedItemModelID")]
        [Ignore]
        public FeedContentModelBase BaseContent
        {
            get; set;
        }   //built from Content property to simplify the content data to the specific kind of content (use this field for data binding and constraints setup)

        public string ActionType
        {
            get; set;
        }
        public string Scope
        {
            get; set;
        }
        public string City
        {
            get; set;
        }

        [JsonIgnore, PrimaryKey]
        public int FeedItemModelID
        {
            get; set;
        }

        public string ID
        {
            get; set;
        }

        [OneToOne("FeedItemModelID")]
        public FeedTextQuote TextQuote
        {
            get; set;
        }

        [OneToMany("FeedItemModelID", CascadeOperations = CascadeOperation.All)]
        public List<FeedActionModel> ActionList
        {
            get; set;
        }


        public bool DidLike
        {
            get; set;
        }
        public int Likes
        {
            get; set;
        }

        public int? Points
        {
            get; set;
        }
        public string Hashtag
        {
            get; set;
        }
        public int SCSChallengeID
        {
            get; set;
        }

        public List<string> AggregateProfileImageUrls
        {
            get; set;
        }

        public string ChallengeName
        {
            get; set;
        }
        public string ChallengeTypeDisplayName
        {
            get; set;
        }
        public DateTime ChallengeEndDate
        {
            get; set;
        }
        public string OfferName
        {
            get; set;
        }
        public DateTime OfferEndDate
        {
            get; set;
        }

        //not setup for db table
        [Ignore]
        public List<FeedEngagementModel> FilteredEngagementList
        {
            get; set;
        }

        [JsonIgnore]
        [Ignore]
        public Dictionary<string, FeedActionModel> ActionDictionary
        {
            get; set;
        }

        [JsonIgnore]
        [Ignore]
        public FeedContentType[] LayoutSections
        {
            get; set;
        }

        [JsonIgnore]
        public string PostedComment
        {
            get; set;
        }

        [JsonIgnore]
        public bool IsCommentsExpanded
        {
            get; set;
        }

        public string BackgroundImage
        {
            get; set;
        }

        void CreateBaseContent()
        {
            if (Content.ContentType == "MAP")
            {
                FeedContentMapModel mapContent = new FeedContentMapModel();
                mapContent.ContentType = Content.ContentType;
                mapContent.Lat = Content.Lat;
                mapContent.Long = Content.Long;
                mapContent.LocationName = Content.LocationName;
                mapContent.TapAction = Content.TapAction;
                mapContent.TapAction2 = Content.TapAction2;
                BaseContent = mapContent;
                Content = null;
            }
            else if (Content.ContentType == "PROFILE")
            {
                FeedContentProfilePictureModel profileContent = new FeedContentProfilePictureModel();
                profileContent.ContentType = Content.ContentType;
                profileContent.Caption = Content.Caption;
                profileContent.ProfilePicURL = Content.ProfilePicURL;
                profileContent.TapAction = Content.TapAction;
                profileContent.TapAction2 = Content.TapAction2;
                BaseContent = profileContent;
                Content = null;
            }
            else if (Content.ContentType == "OFFER")
            {
                FeedContentOfferModel offerContent = new FeedContentOfferModel();
                offerContent.ContentType = Content.ContentType;
                offerContent.OfferCost = Content.OfferCost;
                offerContent.OfferImageURL = Content.OfferImageURL;
                offerContent.OfferMinScore = Content.OfferMinScore;
                offerContent.OfferTitle = Content.OfferTitle;
                offerContent.OfferSubTitle = Content.OfferSubTitle;
                offerContent.TapAction = Content.TapAction;
                offerContent.TapAction2 = Content.TapAction2;
                BaseContent = offerContent;
                Content = null;
            }
            else if (Content.ContentType == "MEDIA")
            {
                FeedContentImageModel imageContent = new FeedContentImageModel();
                imageContent.ContentType = Content.ContentType;
                imageContent.ImageCaption = Content.ImageCaption;
                imageContent.ImageURL = Content.ImageURL;
                imageContent.ImageURLLowRes = Content.ImageURLLowRes;
                imageContent.ImageCaption = Content.ImageCaption;
                imageContent.ImageWidth = Content.ImageWidth;
                imageContent.ImageHeight = Content.ImageHeight;
                imageContent.TapAction = Content.TapAction;
                imageContent.TapAction2 = Content.TapAction2;
                BaseContent = imageContent;
                Content = null;
            }

            else
            {
                FeedContentModelBase baseContent = new FeedContentModelBase();
                baseContent.ContentType = Content.ContentType;
                baseContent.TapAction = Content.TapAction;
                BaseContent = baseContent;
                Content = null;

            }
        }

        public bool HasComments
        {
            get
            {
                return FilteredEngagementList != null ? FilteredEngagementList.Count > 0 : false;
            }
        }

        public void Build()
        {
            List<FeedContentType> sections = new List<FeedContentType>();

            if (Header != null)
                sections.Add(FeedContentType.Header);

            if (TextQuote != null)
            {
                sections.Add(FeedContentType.Text);

                //if (TextQuote.EngagementList != null && TextQuote.EngagementList.Count > 0)
                //sections.Add(FeedContentType.Engagement);
            }

            if (ActionList != null)
            {
                ActionDictionary = new Dictionary<string, FeedActionModel>();
                foreach (FeedActionModel action in ActionList)
                    ActionDictionary[action.ActionLinkName] = action;

                sections.Add(FeedContentType.Engagement);
            }

            if (ActionType == "ITB_COMPLETE")
            {
                sections.Add(FeedContentType.ProductSold);
            }
            if ((ActionType == "AGG_CHALCOMP") || (ActionType == "AGG_COMMIT"))
            {
                sections.Add(FeedContentType.Aggregate);
            }

            if (Content != null)
            {
                CreateBaseContent();

                if (BaseContent != null)
                    BaseContent.Build(ActionType, sections);

                //if ((ActionType == "AGG_CHALCOMP") || (ActionType == "AGG_COMMIT"))
                //{
                //    BaseContent.TapAction = Content.TapAction;
                //}
            }

            LayoutSections = sections.ToArray();
        }
    }

    public class ActionResponseModel : ResponseModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }
        [Ignore]
        public FeedActionModel Action
        {
            get; set;
        }
    }

    public class UpdatedFeedResponceModel : ResponseModel
    {
        public FeedItemModel UpdatedFeedItem
        {
            get; set;
        }
    }

    public class AttributionTrackingResponse : ActionResponseModel
    {
        public string GiftCode
        {
            get; set;
        }
        public string GiftPromptMessage
        {
            get; set;
        }
        public string ReferralKey
        {
            get; set;
        }

    }

    public class FeedResponseModel : ResponseModel
    {
        [JsonIgnore, PrimaryKey]
        public int ID
        {
            get; set;
        }

        [OneToOne("SLFeedID")]
        public FeedModel FeedPage
        {
            get; set;
        }

        public int Position
        {
            get; set;
        }
    }

    public class LikeResponceModel : ResponseModel
    {
        public FeedItemModel UpdatedFeedItem
        {
            get; set;
        }
    }

}
