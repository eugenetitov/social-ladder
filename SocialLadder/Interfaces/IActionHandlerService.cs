using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IActionHandlerService
    {
        Task HandleActionAsync(IMvxNavigationService navigationService, IAlertService alertService, IMvxMessenger messenger, FeedActionModel model, string actionType = null, string AreaGUID = null);
        Task SwitchAreaIfNeeded(string areaGuid);
    }
}
