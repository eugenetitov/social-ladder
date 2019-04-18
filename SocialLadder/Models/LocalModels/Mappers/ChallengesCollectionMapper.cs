using SocialLadder.Enums.Constants;
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
    public static class ChallengesCollectionMapper
    {
        public static LocalChallengeTypeModel ItemToLocalItem(ChallengeTypeModel item, IPlatformAssetService assetService, bool isSelected = false)
        {
            LocalChallengeTypeModel localItem = new LocalChallengeTypeModel()
            {
                 Color = item.Color,
                 DisplayName = item.DisplayName,
                 Group = item.Group,
                 ImageUrl = item.ImageUrl,
                 Total = item.Total,
                 TotalComplete = item.TotalComplete,
                 TypeCode = item.TypeCode
            };
            localItem.ItemState = isSelected ? Enums.ChallengesCollectionItemState.Selected : Enums.ChallengesCollectionItemState.Default;
            localItem.TotalCompleteText = item.TotalComplete + " of " + item.Total;
            localItem.Progress = (int)Math.Round(((double)item.TotalComplete / (double)item.Total) * 100);
            localItem.Color = "#" + ChallengeModel.GetTypeCodeColor(item.TypeCode, item.DisplayName);
            //localItem.Icon = LoadImages(item.TypeCode, item.DisplayName, assetService);
            localItem.Icon = new ChallengeIcon { Icon = ChallengesIconHelper.LoadImages(item.TypeCode, item.DisplayName, assetService), IconUrl = localItem.ImageUrl };
            return localItem;
        }
    }
}
