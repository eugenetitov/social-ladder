using SocialLadder.Models.LocalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Helpers
{
    public static class LocalPlaceholderHelper
    {
        public static LocalPlaceholderModel GetChallengesPlacehplder()
        {
            return new LocalPlaceholderModel {
                Title = "Oh wow!!!",
                Description = "It looks like you've completed all of the challenges we had for you!"
            };
        }

        public static LocalPlaceholderModel GetRewardsPlacehplder()
        {
            return new LocalPlaceholderModel
            {
                Title = "Oh no!",
                Description = "It doesn't look like there are any rewards available right now."
            };
        }
    }
}
