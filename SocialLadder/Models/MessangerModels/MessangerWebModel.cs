using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.MessangerModels
{
    public class MessangerWebModel : MvxMessage
    {
        public string URL { get; set; }
        public bool ToolbarScoreViewHidden { get; set; }

        public MessangerWebModel(object sender, string url, bool toolbarScoreViewHidden) : base(sender)
        {
            URL = url;
            ToolbarScoreViewHidden = toolbarScoreViewHidden;
        }
    }
}
