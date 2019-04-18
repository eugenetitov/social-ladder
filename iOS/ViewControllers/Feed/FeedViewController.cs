using System;
using CoreGraphics;
using UIKit;
using SocialLadder.iOS.Navigation;
using SocialLadder.Models;
using System.Threading.Tasks;
using System.Linq;
using Foundation;
using SocialLadder.iOS.Services;
using FFImageLoading;
using System.Collections.Generic;
using SocialLadder.ViewModels.Feed;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views.Presenters.Attributes;
using SocialLadder.iOS.Sources.Feed;
using SocialLadder.iOS.Views.Cells;

namespace SocialLadder.iOS.ViewControllers.Feed
{
    [MvxFromStoryboard("Main")]
    //[MvxRootPresentation]
    [MvxTabPresentation(WrapInNavigationController = true, TabName = "Feed", TabIconName = "feed-icon_on", TabSelectedIconName = "feed-icon_off")]
    public partial class FeedViewController :  MvxViewController<FeedViewModel>//BaseTabViewController<FeedViewModel> //SLViewController, IFeedViewController
    {
        #region fields
        public List<FeedItemModel> Feed;
        private PushNotificationService _pushService;
        private FeedTableSource _tableSource;
        //private UIView _overlay;
        public bool _didFeedActionInvoked;
        private bool _isFeedRefreshing;
        private bool _isFeedRefreshedFromNotification;
         
        #endregion

        #region properties
        public bool IsModal
        {
            get; set;
        }

        public string FeedUrl
        {
            get; set;
        }
        #endregion

        #region ctrors
        public FeedViewController(IntPtr handle) : base(handle)
        {
            Feed = null;
            IsModal = false;

        }
        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetupTableView();
            SetupBindingSet();
            AreaCollectionHeight.Constant = 0f;
            cnsAreaCollectionTop.Constant = 0f;
            //base.AreaCollectionOutlet = AreaCollection;
            //base.AreaCollectionHeightOutlet = AreaCollectionHeight;
            //base.AreaCollectionTopOutlet = cnsAreaCollectionTop;

            //_didFeedActionInvoked = false;

            //_pushService = new PushNotificationService();
        }

       

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            _didFeedActionInvoked = false;

            //if (_isFeedRefreshing || _isFeedRefreshedFromNotification)
            //{
            //    return;
            //}

            //if (IsModal == true)
            //{
            //    IsChangingAreasDisabled = true;
            //    if (FeedUrl != string.Empty)
            //    {
            //        RefreshFeedByUrl(FeedUrl);
            //        return;
            //    }
            //    Feed = new List<FeedItemModel>() { SL.Feed.FeedPage.FeedItemList[0] };
            //    TableView.ReloadData();
            //    (TableView.Source as FeedTableSource)?.UnhideCommentsInCell(0);
            //    return;
            //}
            //RefreshAsync();
        }

        private void SetupBindingSet()
        {
            var set = this.CreateBindingSet<FeedViewController, FeedViewModel>();
            set.Bind(_tableSource).For(source => source.ItemsSource).To(vm => vm.FeedItems);
            set.Apply();
        }

        private void SetupTableView()
        {
            if (_tableSource == null)
            {
                _tableSource = new FeedTableSource(TableView, this);
            }
            //_tableSource.ItemSelected += OnFeedItemSelected;

            TableView.RegisterNibForCellReuse(FeedTableViewCell.Nib, FeedTableViewCell.ClassName);
            TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 50.0f;
            TableView.ViewController = this;
            TableView.Source = _tableSource;
            TableView.AddRefreshControl();

            TableView.CustomRefreshControl.ValueChanged += (s, e) =>
            {
                ChangeFeed();
            };
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //_tableSource.ItemSelected -= OnFeedItemSelected;
            //_isFeedRefreshedFromNotification = false;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (IsModal)
            {
                TableView?.HideLoader();
            }
        }

        //public async override Task Refresh()
        //{
        //    if (Platform.IsInternetConnectionAvailable() == false)
        //    {
        //        return;
        //    }
        //    await base.Refresh();
        //    await RefreshFeed();
        //    await RefreshProfile();
        //}


        public void OnFeedItemSelected(FeedItemModel model)
        {
            //HideAreaCollection();

            if (_didFeedActionInvoked)
            {
                return;
            }

            if (!_didFeedActionInvoked)
            {
                _didFeedActionInvoked = true;
                if ((model?.BaseContent as FeedContentImageModel)?.TapAction != null)
                {
                    ActionHandlerService service = new ActionHandlerService();
                    service.HandleActionAsync((model.BaseContent as FeedContentImageModel).TapAction, model.ActionType);
                    _didFeedActionInvoked = false;
                    return;
                }

                if (model?.BaseContent?.TapAction != null)
                {
                    ActionHandlerService service = new ActionHandlerService();
                    service.HandleActionAsync(model.BaseContent.TapAction, model.ActionType);
                    _didFeedActionInvoked = false;
                    return;
                }

                if (model?.ActionDictionary != null && model.ActionDictionary.Keys.Contains("View It"))
                {
                    var viewItAction = model.ActionDictionary["View It"];
                    string url = viewItAction.ActionParamDict["FeedURL"];
                    RefreshFeedByUrl(url);
                    _didFeedActionInvoked = false;
                }
                _didFeedActionInvoked = false;
            }
        }

        private void OnRefreshFeedDidBegin(object sender, EventArgs args)
        {
            TableView.ShowLoader();
            //_overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
        }

        private void OnRefreshFeedComplete(object sender, FeedResponseModel response)
        {
            RefreshFeedComplete(response);
        }

        private async Task RefreshFeed()
        {
            //TableView.ReloadData();
            TableView.ShowLoader();
            await SL.Manager.GetFeedAsync(RefreshFeedComplete);
        }

        private void RefreshFeedComplete(FeedResponseModel response)
        {

            if (response.ResponseCode <= 0)
            {
                Platform.ShowAlert("Oops!", "Sorry about that - it looks like we're unable to load your content right now. Please try again in a minute.");
                //UIAlertView alert = new UIAlertView()
                //{
                //    Title = "Refresh error",
                //    Message = "Feed refresh error"
                //};
                //alert.AddButton("OK");
                //alert.Show();
                TableView.HideLoader();
                //show alert
                return;
            }
            TableView.ReloadData();

            TableView.HideLoader();
            _isFeedRefreshing = false;
        }

        private void ChangeFeed()
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                TableView.HideLoader();
                return;
            }
            if (IsModal)
            {
                if (FeedUrl == string.Empty)
                {
                    TableView.HideLoader();
                    return;
                }
                SL.Manager.GetFeedByUrlAsync(FeedUrl, ChangeFeedComplete);
                return;
            }
            SL.Manager.GetFeedAsync(ChangeFeedComplete);
        }

        private void ChangeFeedComplete(FeedResponseModel response)
        {
            try
            {
                ImageService.Instance.InvalidateMemoryCache();

                if (response.ResponseCode > 0)
                    TableView.ReloadData();
                TableView.HideLoader();
            }
            catch (Exception)
            {
                TableView.HideLoader();
            }
        }

        public void RefreshFeedByUrl(string feedUrl)
        {
            FeedUrl = feedUrl;
            if (false == Platform.IsInternetConnectionAvailable())
            {
                return;
            }
            if (!_isFeedRefreshing)
            {
                _isFeedRefreshing = true;
                TableView.ShowLoader(); // _overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
            }
            SL.Manager.GetFeedByUrlAsync(feedUrl, RefreshFeedByUrlComplete);
        }

        private void RefreshFeedByUrlComplete(FeedResponseModel response)
        {
            try
            {
                ImageService.Instance.InvalidateMemoryCache();

                if (response.ResponseCode > 0)
                {
                    TableView.ReloadData();
                }
            }
            finally
            {
                TableView.HideLoader();
                _isFeedRefreshedFromNotification = true;
                _isFeedRefreshing = false;

                if (TableView.NumberOfRowsInSection(0) > 0)
                {
                    TableView.ScrollToRow(NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, false);
                }
                TableView.ScrollRectToVisible(new CGRect(0, -10, 1, 1), false);

                OpenFeedItemAsModal();
            }
        }

        private void OpenFeedItemAsModal()
        {
            UIStoryboard storyboard = UIStoryboard.FromName("Main", null);
            FeedViewController modalViewController = storyboard.InstantiateViewController("FeedViewController") as FeedViewController;
            modalViewController.IsModal = true;
            modalViewController.FeedUrl = string.Empty;
            ((SLNavigationController)NavigationController).PushViewController(modalViewController, false);
        }

        private async Task RefreshProfile()
        {
            await SL.Manager.GetProfileAsync();
            AreaCollection.ReloadData();
            //UpdateNavBar();

        }

        //private async Task RefreshNotifications()
        //{
        //    // await SL.Manager.RefreshNotificationsAsync(ProcessNotificationList);
        //}

        private void ProcessNotificationList(NotificationResponseModel notification)
        {
            //if(notification.NotificationObject!=null)
            //    PushNotificationService.CreateLocalNotification(notification.NotificationObject);
            //var notifications = profile?.ResponseNotificationList;
            //if (notifications?.Count > 0)
            //{

            //}
        }
    }
}

