using SocialLadder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Mappers
{
    public static class AreasViewMapper
    {
        public static LocalAreaModel ItemToLocalItem(AreaModel model, IItemActionService clickCommand, bool isClickable = false, bool isSuggestedArea = false)
        {
            LocalAreaModel item = new LocalAreaModel()
            {
                allowSuggestion = model.allowSuggestion,
                areaButtonColor = model.areaButtonColor,
                areaDefaultImageLowResURL = model.areaDefaultImageLowResURL,
                areaDefaultImageURL = model.areaDefaultImageURL,
                areaDescription = model.areaDescription,
                areaGUID = model.areaGUID,
                areaIconURL = model.areaIconURL,
                areaID = model.areaID,
                areaName = model.areaName,
                areaPrimaryColor = model.areaPrimaryColor,
                JoinCode = model.JoinCode,
                ProfileID = model.ProfileID,
                termsOfService = model.termsOfService,
            };
            item.IsSuggestedArea = isSuggestedArea;
            item.ImageClickCommand = isClickable? clickCommand.ItemActionCommand : null;
            return item;
        }
    }
}
