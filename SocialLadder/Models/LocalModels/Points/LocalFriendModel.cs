using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Points
{
    public class LocalFriendModel : FriendModel
    {
        public double CurrentScoreValue { get; set; }
        public bool IsSelected { get; set; }
    }
}
