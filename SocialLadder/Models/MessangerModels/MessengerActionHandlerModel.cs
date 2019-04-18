using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.MessangerModels
{
    public class MessengerActionHandlerModel : MvxMessage
    {
        public ShareResponseModel Model { get; set; }

        public MessengerActionHandlerModel(object sender, ShareResponseModel model) : base(sender)
        {
            Model = model;
        }
    }
}
