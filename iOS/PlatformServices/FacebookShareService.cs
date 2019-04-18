using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using SocialLadder.Enums;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.PlatformServices
{
    public class FacebookShareService : IFacebookShareService
    {
        public Task SendOpenGraph(object source, ChallengeModel model, string message = null, byte[] data = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null, ShareTemplateModel shareTemplate = null, ShareResponseModel shareResponse = null)
        {
            throw new NotImplementedException();
        }

        public void SetAccessToken(string accessToken, string userId, bool needRemove = false)
        {
            throw new NotImplementedException();
        }

        public Task ShareFacebookChallenge(object source, ChallengeModel model, string message = null, byte[] data = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null)
        {
            throw new NotImplementedException();
        }

        public void VerifyPermissions(string[] permissions = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null, object param = null)
        {
            throw new NotImplementedException();
        }
    }
}