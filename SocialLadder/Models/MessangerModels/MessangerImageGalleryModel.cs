using MvvmCross.Plugins.Messenger;
using SocialLadder.Models.LocalModels.Challenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.MessangerModels
{
    public class MessangerImageGalleryModel : MvxMessage
    {
        public ChallengeModel Challenge
        {
            get;set;
        }

        public List<LocalPosterModel> Posters
        {
            get; set;
        }

        public MessangerImageGalleryModel(object sender, List<LocalPosterModel> images, ChallengeModel challenge) : base(sender)
        {
            Posters = images;
            Challenge = challenge;
        }
    }
}
