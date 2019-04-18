using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Authentication
{
    public interface IInstagramAuthenticationDelegate
    {
        //void OnInstagramAuthenticationCompleted(InstagramOAuthToken token);
        void OnInstagramAuthenticationFailed(string message, Exception exception);
        void OnInstagramAuthenticationCanceled();
        //void OnInstagramAuthenticationCompleted(Account account);
        Task OnInstagramAuthenticationCompleted(SocialNetworkModel network);
    }
}
