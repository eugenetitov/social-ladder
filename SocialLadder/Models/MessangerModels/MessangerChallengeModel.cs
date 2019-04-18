using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.MessangerModels
{
    public class MessangerChallengeModel : MvxMessage
    {
        public ChallengeModel Model { get; set; }

        public MessangerChallengeModel(object sender, ChallengeModel model) : base(sender)
        {
            Model = model;
        }   
    }
}
