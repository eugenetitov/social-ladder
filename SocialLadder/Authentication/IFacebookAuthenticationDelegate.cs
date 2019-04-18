using System;
using System.Threading.Tasks;
using SocialLadder.Models;

namespace SocialLadder.Authentication
{
    public interface IFacebookAuthenticationDelegate
    {
        Task OnFacebookAuthenticationCompleted(SocialNetworkModel network);
        void OnFacebookAuthenticationFailed(string message, Exception exception);
        void OnFacebookAuthenticationCanceled();

    }
}
