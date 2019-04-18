using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Mappers;
using SocialLadder.Models.LocalModels.Notifications;
using SocialLadder.ViewModels.Base;

namespace SocialLadder.ViewModels.Notification
{
    public class NotificationsViewModel : BaseViewModel
    {
        public static string DefaultAreaName => "SocialLadder";
        private readonly IActionHandlerService _actionHandlerService;

        private MvxObservableCollection<ExpandableNotificationModel> _notifications;
        public MvxObservableCollection<ExpandableNotificationModel>  Notifications
        {
            get => _notifications;
            set => SetProperty(ref _notifications, value);
        }

        //workaround for mvvmcross less 6.0 version
        //GroupClick binding replaces group expanding functionality

        private MvxInteraction<int> _groupSelectedInteractions = new MvxInteraction<int>();
        public IMvxInteraction<int> GroupSelectedInteractions => _groupSelectedInteractions;

        public MvxAsyncCommand<LocalNotificationModel> SelectedItemCommand
        {
            get
            {
                return new MvxAsyncCommand<LocalNotificationModel>(async (param) =>
                {
                    if (param == null)
                    {
                        return;
                    }
                    foreach (var group in Notifications)
                    {
                        if ((group as List<LocalNotificationModel>).Contains(param) && param.Message == "You're all caught up!")
                        {
                            OnItemSelected(group);
                            return;
                        }
                    }
                    if (param?.Action?.ActionScreen == null || param.Action.ActionScreen == "SHARE" && !(param.Action.ActionParamDict?.ContainsKey("ChallengeDetailURL") ?? false))
                    {
                        return;
                    }
                    ItemSelectedAction(param);
                });
            }
        }

        private void ItemSelectedAction(LocalNotificationModel item)
        {
            if (!string.IsNullOrEmpty(item.AreaGUID) && item.AreaGUID != DefaultAreaName)
            {
                _actionHandlerService.SwitchAreaIfNeeded(item.AreaGUID);
            }

            SL.Manager.AcknowledgeNotification(item.NotificationUID, (model) =>
            {
                _actionHandlerService.HandleActionAsync(_navigationService, _alertService, _messenger, item.Action);
            });
        }

        private void OnItemSelected(ExpandableNotificationModel model)
        {
            if (!string.IsNullOrEmpty(model.AcknowledgedNotificationUrl))
            {
                SL.Manager.AddNotificationsByUrl(model.AcknowledgedNotificationUrl, async (responce) =>
                {
                    model.AcknowledgedNotificationUrl = !string.IsNullOrEmpty(responce.AcknowledgedPage) ? responce.AcknowledgedPage : model.AcknowledgedNotificationUrl;
                    var areaGuid = responce.ResponseNotificationList.FirstOrDefault()?.AreaGUID;
                    var sectionDetails = Notifications.Where(x => x.AreaGuid == areaGuid).FirstOrDefault();
                    int tableIndex = Notifications.IndexOf(sectionDetails);
                    var list = new List<LocalNotificationModel>();
                    if (sectionDetails == null)
                    {
                        _alertService.ShowToast("There is no new posts");
                        return;
                    }
                    if (sectionDetails != null)
                    {                      
                        foreach (var notiItem in responce.ResponseNotificationList)
                        {
                            var localItem = NotificationsMapper.ItemToLocalItem(notiItem);
                            localItem.Icon = sectionDetails.AreaImage;
                            list.Add(localItem);
                        }
                    }
                    sectionDetails.Clear();
                    sectionDetails.AddRange(list);
                    Notifications[tableIndex] = sectionDetails;
                });
            }
        }

        public IMvxCommand SelectGroupCommand => new MvxCommand<ExpandableNotificationModel>(SelectGroup);
        public async void SelectGroup(ExpandableNotificationModel notificationGroup)
        {
            int index = Notifications.IndexOf(notificationGroup);
            var result = await SL.Manager.GetNotificationsAsync(notificationGroup.AreaGuid, null);
            if (result != null)
            {
                var notification = Notifications.Where(x => x.AreaGuid == notificationGroup.AreaGuid).FirstOrDefault();
                notification.AcknowledgedNotificationUrl = result.AcknowledgedPage;
                notification.Clear();
                if (result.ResponseNotificationList.Count == 0)
                {
                    notification?.Add(new LocalNotificationModel() { Message = "You're all caught up!", NotificationUID = null });
                }
                else
                {
                    int count = 0;
                    foreach (var item in result.ResponseNotificationList)
                    {
                        var localItem = NotificationsMapper.ItemToLocalItem(item);
                        localItem.Icon = notification.AreaImage;
                        notification?.Add(localItem);
                        count += 1;
                    }
                    notification.NotificationCount = count > 0 && count <= 10 ? count.ToString() : "10+";
                    Notifications[index] = notification;
                }             
                _groupSelectedInteractions.Raise(index);
            }
        }

        public NotificationsViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger, IActionHandlerService actionHandlerService) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _actionHandlerService = actionHandlerService;
        }

        public async override void ViewAppearing()
        {
            base.ViewAppearing();
            await Refresh();
        }

        public async override Task FinishRefresh()
        {
            await SL.Manager.GetNotificationSummaryAsync(OnNotificationSummaryRefreshed);
            await base.FinishRefresh();
        }

        public override void Start()
        {
            Mvx.Resolve<IPlatformNavigationService>().ChangeNotificationIndicatorStatus(true);
            base.Start();
        }

        public void OnNotificationSummaryRefreshed(NotificationResponseModel notificationResponse)
        {
            if(notificationResponse.ResponseNotificationList == null)
            {
                return;
            }
            List<ExpandableNotificationModel> notificationSummary = new List<ExpandableNotificationModel>();
            foreach(var item in notificationResponse.ResponseNotificationList)
            {
                var area = SL.AreaList.Find(x => x.areaGUID == item.AreaGUID);
                var areaTitle = area?.areaName;
                var areaImage = area?.areaDefaultImageURL;
                notificationSummary.Add(new ExpandableNotificationModel(new List<LocalNotificationModel>() { new LocalNotificationModel() { Message = "You're all caught up!" } })
                {
                    AreaTitle = areaTitle,
                    AreaGuid = item.AreaGUID,
                    NotificationCount = item.Count > 10 ? "10+" : item.Count.ToString(),
                    AreaImage = areaImage
                });
            }
            Notifications = new MvxObservableCollection<ExpandableNotificationModel>(notificationSummary);
        } 
    }
}
