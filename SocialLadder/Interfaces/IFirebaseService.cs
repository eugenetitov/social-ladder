using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialLadder.Droid.Interfaces
{
    public interface IFirebaseService
    {
        bool IsPushNotificationServiceAlailable();
        void RegisterPushNotificationService();
    }
}