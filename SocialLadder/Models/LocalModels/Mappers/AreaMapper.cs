using SocialLadder.Models.LocalModels.Areas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Mappers
{
    public static class AreaMapper
    {
        public static LocalAreasModel ItemToLocalItem(AreaModel currentItem)
        {
            LocalAreasModel item = new LocalAreasModel()
            {
                areaID = currentItem.areaID,
                ProfileID = currentItem.ProfileID,
                areaName = currentItem.areaName,
                areaGUID = currentItem.areaGUID,
                areaIconURL = currentItem.areaIconURL,
                areaDefaultImageURL = currentItem.areaDefaultImageURL,
                areaDefaultImageLowResURL = currentItem.areaDefaultImageLowResURL,
                areaPrimaryColor = currentItem.areaPrimaryColor,
                areaButtonColor = currentItem.areaButtonColor,
                areaDescription = currentItem.areaDescription,
                termsOfService = currentItem.termsOfService,
                allowSuggestion = currentItem.allowSuggestion,
                JoinCode = currentItem.JoinCode
            };
            return item;
        }

        public static LocalCurrentAreaModel ItemToCurrentLocalItem(AreaModel item, string scoreImage, string scoreCount)
        {
            LocalCurrentAreaModel currentItem = new LocalCurrentAreaModel()
            {
                areaID = item.areaID,
                ProfileID = item.ProfileID,
                areaName = item.areaName,
                areaGUID = item.areaGUID,
                areaIconURL = item.areaIconURL,
                areaDefaultImageURL = item.areaDefaultImageURL,
                areaDefaultImageLowResURL = item.areaDefaultImageLowResURL,
                areaPrimaryColor = item.areaPrimaryColor,
                areaButtonColor = item.areaButtonColor,
                areaDescription = item.areaDescription,
                termsOfService = item.termsOfService,
                allowSuggestion = item.allowSuggestion,
                JoinCode = item.JoinCode
            };
            currentItem.ScoreCount = scoreCount;
            currentItem.ScoreImage = scoreImage;
            return currentItem;
        }
    }
}
