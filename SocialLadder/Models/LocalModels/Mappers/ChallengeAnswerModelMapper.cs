using SocialLadder.Models.LocalModels.Challenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Mappers
{
    public static class ChallengeAnswerModelMapper
    {
        public static LocalChallengeAnswerModel ItemToLocalItem(ChallengeAnswerModel item)
        {
            LocalChallengeAnswerModel localItem = new LocalChallengeAnswerModel()
            {
                AnswerCode = item.AnswerCode,
                AnswerName = item.AnswerName,
                ChallengeID = item.ChallengeID,
                ID = item.ID,
                isWriteIn = item.isWriteIn,
                Sequence = item.Sequence,
                writeInPrompt = item.writeInPrompt
            };
            localItem.IsSelected = false;
            return localItem;
        }
    }
}
