using FFImageLoading;
using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using UIKit;

namespace SocialLadder.iOS.Notifications
{
    public partial class NotificationsViewController : UIViewController
    {
        public static string DefaultAreaName => "SocialLadder";
        private UIImage _defaultAreaImage =>
            UIImage.FromBundle("add-area_large");

        public NotificationsViewController(IntPtr handle) : base(handle)
        {
        }

        void ApplyStyle()
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ApplyStyle();
            //TableView.RegisterNibForCellReuse(NotificationsTableViewCell.Nib, NotificationsTableViewCell.ClassName);

            List<NotificationModel> list = SL.NotificationList;

            //if (SL.NotificationList != null)
            //{
            //    RefreshTable(list);
            //}

            RefreshNotifications();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = SizeConstants.ScreenMultiplier * 56.0f;
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (TableView?.Source != null)
            {
                (TableView.Source as NotificationsTableSource<NotificationItemModel>).ItemInvoked = false;
            }
        }

        public void RefreshNotifications()
        {
            if (false == Platform.IsInternetConnectionAvailable())
            {
                return;
            }

            //TableView.ShowLoader();

            SL.Manager.GetNotificationsAsync(RefreshNotificationsComplete);
        }

        private void RefreshNotificationsComplete(NotificationResponseModel response)
        {
            if (response.ResponseCode > 0)
            {
                TableView.ReloadData();
            }

            //TableView.HideLoader();

            RefreshTable(response.ResponseNotificationList);
        }

        private void RefreshTable(List<NotificationModel> notificationList)
        {
            var Headers = new List<AreaModel>();

            foreach (var item in notificationList)
            {
                if (String.IsNullOrEmpty(item.AreaGUID))
                {
                    item.AreaGUID = DefaultAreaName;//SL.AreaGUID;

                    if (!Headers.Exists((x) => x.areaGUID == DefaultAreaName))
                        Headers.Insert(0, new AreaModel() { areaName = DefaultAreaName, areaGUID = DefaultAreaName });

                    continue;
                }

                var area = SL.AreaList?.Find((x) => x.areaGUID == item.AreaGUID);
                if (area != null && !Headers.Exists((x) => x.areaGUID == area.areaGUID))
                    Headers.Add(area);
            }

            //create the data
            var list = new List<ExpandableTableModel<NotificationItemModel>>();
            foreach (var section in Headers)
            {
                var filteredNotificationList = notificationList.FindAll((x) =>
                    (string.IsNullOrEmpty(x.AreaGUID) ? DefaultAreaName : x.AreaGUID) == section.areaGUID);

                if (filteredNotificationList == null || filteredNotificationList.Count == 0)
                    continue;

                var sectionData = new ExpandableTableModel<NotificationItemModel>()
                {
                    Title = section.areaName,
                    Icon = GetImageByUrlOrDefault(section.areaDefaultImageURL, section.areaGUID == DefaultAreaName),
                    NotificationsCount = filteredNotificationList.Count.ToString()
                };

                foreach (var row in filteredNotificationList)
                {
                    var notificationItem = new NotificationItemModel()
                    {
                        Message = row.Message,
                        Icon = sectionData.Icon,//if no special icon for row
                        Item = row
                    };
                    sectionData.Add(notificationItem);
                }

                list.Add(sectionData);
            }

            TableView.Source = new NotificationsTableSource<NotificationItemModel>(list, TableView);
        }


        private UIImage GetImageByUrlOrDefault(string url, bool isDefault = false)
        {
            if (isDefault)
            {
                return _defaultAreaImage;
            }
            else
            {
                try
                {
                    var areaImage = new UIImageView();
                    ImageService.Instance.LoadUrl(url).Into(areaImage);
                    return areaImage.Image;
                }
                catch (Exception)
                {
                    return _defaultAreaImage;
                }
            }
        }
    }
}