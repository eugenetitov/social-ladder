using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IChallengeShareViewModel
    {
        bool CheckNetworkConnected();
        void AddPhotoFromCameraOrLibrary(byte[] image);
        Task ShareToSocialNetwork();
        void ShowToastIfPermissionsDenided();
    }
}
