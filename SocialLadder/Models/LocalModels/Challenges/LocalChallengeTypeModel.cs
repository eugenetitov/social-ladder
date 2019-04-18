using SocialLadder.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Challenges
{
    public class LocalChallengeTypeModel : ChallengeTypeModel
    {
        public ChallengesCollectionItemState ItemState { get; set; }
        public string TotalCompleteText { get; set; }
        public int Progress { get; set; }
        public ChallengeIcon Icon { get; set; }
    }
}
