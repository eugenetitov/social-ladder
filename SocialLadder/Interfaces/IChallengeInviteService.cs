using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IChallengeInviteService
    {
        Task UseSmsToSendInviteContact(List<string> contact);
        Task UseWatsAppToSendInviteContact(string contact);
        void ShowToastIfPermissionsDenided();
        void CancelInviteAction();
    }
}
