using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using SocialLadder.Models.LocalModels.Challenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Mappers
{
    public static class ChallengesListMapper
    {
        public static LocalChallengeModel ItemToLocalItem(ChallengeModel item, string sectionName, bool sectionHidden, IPlatformAssetService assetService)
        {
            LocalChallengeModel localItem = new LocalChallengeModel()
            {
                AllowUserCompletion = item.AllowUserCompletion,
                AnswerList = item.AnswerList,
                AutoUnlockDate = item.AutoUnlockDate,
                AvailabilityDate = item.AvailabilityDate,
                ChallengeDetailsURL = item.ChallengeDetailsURL,
                CollateralReview = item.CollateralReview,
                CompletedCount = item.CompletedCount,
                CompPointValue = item.CompPointValue,
                Desc = item.Desc,
                DisallowSharing = item.DisallowSharing,
                EffectiveEndDate = item.EffectiveEndDate,
                EffectiveStartDate = item.EffectiveStartDate,
                FBShareType = item.FBShareType,
                Group = item.Group,
                IconImageURL = item.IconImageURL,
                ID = item.ID,
                Image = item.Image,
                ImageLowRes = item.ImageLowRes,
                InstaCaption = item.InstaCaption,
                InstaURL = item.InstaURL,
                InviteToChallengeTemplateURL = item.InviteToChallengeTemplateURL,
                IsCommentReq = item.IsCommentReq,
                IsEventAttendReq = item.IsEventAttendReq,
                IsFixedContent = item.IsFixedContent,
                IsGuestList = item.IsGuestList,
                IsLikeReq = item.IsLikeReq,
                IsReshareReq = item.IsReshareReq,
                IsReviewReq = item.IsReviewReq,
                IsSurvey = item.IsSurvey,
                LeaderBoardList = item.LeaderBoardList,
                LocationLat = item.LocationLat,
                LocationLong = item.LocationLong,
                LockIndicatorImageURL = item.LockIndicatorImageURL,
                LockReason = item.LockReason,
                LockStatus = item.LockStatus,
                MinStars = item.MinStars,
                MinTags = item.MinTags,
                MulipleShops = item.MulipleShops,
                Name = item.Name,
                PointIconURL = item.PointIconURL,
                PointsPerDollar = item.PointsPerDollar,
                PointsPerInstance = item.PointsPerInstance,
                PointValue = item.PointValue,
                Question = item.Question,
                RadiusMeters = item.RadiusMeters,
                SecondsUntilExpire = item.SecondsUntilExpire,
                SecondsUntilUnlock = item.SecondsUntilUnlock,
                SelectAnswerID = item.SelectAnswerID,
                Sequence = item.Sequence,
                ShareImage = item.ShareImage,
                ShareTemplateURL = item.ShareTemplateURL,
                SmallImageURL = item.SmallImageURL,
                Status = item.Status,
                Subtitle = item.Subtitle,
                TargetCount = item.TargetCount,
                TargetObjectId = item.TargetObjectId,
                TargetObjectURL = item.TargetObjectURL,
                templateDiscountAmount = item.templateDiscountAmount,
                templateDiscountNum = item.templateDiscountNum,
                templateDiscountType = item.templateDiscountType,
                templateDiscountUse = item.templateDiscountUse,
                templateEndOffset = item.templateEndOffset,
                templateName = item.templateName,
                templateStartOffset = item.templateStartOffset,
                TypeCode = item.TypeCode,
                TypeCodeDisplayName = item.TypeCodeDisplayName,
                UpdateAll = item.UpdateAll,
                UseDefaultCodes = item.UseDefaultCodes,
                UsePointsPerDollar = item.UsePointsPerDollar,
                UseTeamCodes = item.UseTeamCodes
            };
            localItem.Color = "#" + ChallengeModel.GetTypeCodeColor(item.TypeCode, item.TypeCodeDisplayName);
            localItem.PointsText = "+" + item.PointValue.ToString() + " pts"; 
            localItem.SectionHidden = sectionHidden;
            localItem.SectionName = sectionName;
            localItem.Icon = new ChallengeIcon { Icon = ChallengesIconHelper.LoadImages(item.TypeCode, item.TypeCodeDisplayName, assetService), IconUrl = localItem.IconImageURL };
            return localItem;
        }
    }
}
