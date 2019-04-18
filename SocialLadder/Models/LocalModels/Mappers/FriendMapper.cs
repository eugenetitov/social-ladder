using SocialLadder.Models.LocalModels.Points;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Mappers
{
    public static class FriendMapper
    {
        public static LocalFriendModel ItemToLocalItem(FriendModel currentItem)
        {
            var item = new LocalFriendModel()
            {
                 AppStatus = currentItem.AppStatus,
                 ExternalID = currentItem.ExternalID,
                 ID = currentItem.ID,
                 Name = currentItem.Name,
                 NetworkName = currentItem.NetworkName,
                 ProfileID = currentItem.ProfileID,
                 ProfilePicURL = currentItem.ProfilePicURL,
                 Rank = currentItem.Rank,
                 RankLabel = currentItem.RankLabel,
                 RelScore = currentItem.RelScore,
                 RequestID = currentItem.RequestID,
                 RewardID = currentItem.RewardID,
                 Score = currentItem.RewardID,
                 SCSUserProfileID = currentItem.SCSUserProfileID,
                 SubLabel = currentItem.SubLabel
            };
            item.IsSelected = false;
            return item;
        }
    }
}
