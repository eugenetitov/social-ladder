using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Challenges
{
    public class LocalChallengeModel : ChallengeModel
    {
        public string ChallengeTime { get; set; }
        public string PointsText { get; set; }
        public string Color { get; set; }
        //public string ImageName { get; set; }
        public string SectionName { get; set; }
        public bool SectionHidden { get; set; }
        public ChallengeIcon Icon { get; set; }
    }
}
