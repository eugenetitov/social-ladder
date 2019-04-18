using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IFacebookService
    {
        void ShareFacebookChallenge(object source, ChallengeModel model, string message = null);
        void SendOpenGraph(object source, ChallengeModel model, string message = null);
        void VerifyPermissions(string[] permissions, Action success, Action fail);
    }
}
