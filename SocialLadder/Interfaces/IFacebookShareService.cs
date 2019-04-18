using SocialLadder.Enums;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IFacebookShareService
    {
        void VerifyPermissions(string[] permissions = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null, object param = null);
        Task ShareFacebookChallenge(object source, ChallengeModel model, string message = null, byte[] data = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null);
        void SetAccessToken(string accessToken, string userId, bool needRemove = false);
        Task SendOpenGraph(object source, ChallengeModel model, string message = null, byte[] data = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null, ShareTemplateModel shareTemplate = null, ShareResponseModel shareResponse = null);
    }
}
