using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface INotificationService
    {
        void ProcessNotification(NotificationModel model);
        void SwitchAreaIfNeeded(string areaGuid);
    }
}
