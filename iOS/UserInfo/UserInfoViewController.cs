using CoreGraphics;
using FFImageLoading;
using Foundation;
using SocialLadder.iOS.Helpers;
using SocialLadder.iOS.Interfaces.ViewControllers;
using SocialLadder.iOS.Services;
using SocialLadder.iOS.Sources.Feed;
using SocialLadder.iOS.Views.Cells;
using SocialLadder.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.UserInfo
{
    public partial class UserInfoViewController : UIViewController, IFeedViewController //SLViewController
    {
        #region fields
        //private DateTime _startTime;
        //private int _getFeedApiSeconds;
        //private int _tableReloadSeconds;
        //private List<FeedItemModel> _feeds;
        private PushNotificationService _pushService;
        private FeedTableSource _tableSource;
        private UIView _overlay;

        //private bool _isFeedLoaded;
        #endregion

        #region properties
        public bool ShouldGetProfileByFriendId { get; set; }

        public int FriendId { get; set; }
        public bool IsModal
        {
            get; set;
        }
        public string ProfileUrl
        {
            get; set;
        }
        public string FeedUrl
        {
            get; set;
        }
        #endregion

        #region ctrors
        public UserInfoViewController(IntPtr handle) : base(handle)
        {
            IsModal = false;
        }
        #endregion

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            btnClose.SetImage(UIImage.FromBundle("close-icon"), UIControlState.Normal);
            btnClose.TouchUpInside += ((object sender, EventArgs args) =>
            {
                DismissViewController(true, null);
            });

            _pushService = new PushNotificationService();

            tvFeedItems.RegisterNibForCellReuse(FeedTableViewCell.Nib, FeedTableViewCell.ClassName);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //SL.OnRefreshFeedDidBegin += OnRefreshFeedDidBegin;
            //SL.OnRefreshFeedComplete += OnRefreshFeedComplete;

            if (_tableSource == null)
            {
                _tableSource = new FeedTableSource(tvFeedItems,  this);
            }
            // _tableSource.ItemSelected += OnFeedItemSelected;

            //var tap = new UITapGestureRecognizer();
            //tap.AddTarget(() => HideAreaCollection());
            tvFeedItems.FeedViewController = this;
            tvFeedItems.Source = _tableSource;
            tvFeedItems.RowHeight = UITableView.AutomaticDimension;
            tvFeedItems.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tvFeedItems.AddRefreshControl();

            tvFeedItems.CustomRefreshControl.ValueChanged += (s, e) =>
            {
                if (ShouldGetProfileByFriendId)
                {
                    SL.Manager.GetFeedByFriendIdAsync(FriendId, GetFeedComplete);
                    return;
                }

                SL.Manager.GetFeedByUrlAsync(FeedUrl, GetFeedComplete);
                //ChangeFeed();
                //SlNavController.NavTitle.ShowLoadIndicator();
            };
            tvFeedItems.EstimatedRowHeight = 50.0f;

            Refresh();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            nfloat fontMultiplier = UIScreen.MainScreen.Bounds.Width / 414;

            lblUserName.Font = UIFont.FromName("ProximaNova-Bold", 16.5f * fontMultiplier);
            lblRankCategory.Font = UIFont.FromName("ProximaNova-Bold", 14 * fontMultiplier);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            //SL.OnRefreshFeedDidBegin -= OnRefreshFeedDidBegin;
            //SL.OnRefreshFeedComplete -= OnRefreshFeedComplete;
            //_tableSource.ItemSelected -= OnFeedItemSelected;
        }



        //public override void HideAreaCollection()
        //{
        //    //RefreshFeed();
        //    base.HideAreaCollection();
        //}

        public async void Refresh()//override
        {
            tvFeedItems.ShowLoader();
            if (ShouldGetProfileByFriendId)
            {
                await SL.Manager.GetFeedByFriendIdAsync(FriendId, GetFeedComplete);
                await SL.Manager.GetProfileByFriendIdAsync(FriendId, GetProfileComplete);
            }
            else
            {
                await SL.Manager.GetFeedByUrlAsync(FeedUrl, GetFeedComplete);
                await SL.Manager.GetProfileByUrlAsync(ProfileUrl, GetProfileComplete);
            }
            tvFeedItems.HideLoader();
            //base.Refresh();
        }

        public void BeginRefresh() //override
        {
            _overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
        }

        //public async void OnFeedItemSelected(FeedItemModel model)
        //{
        //    ;
        //HideAreaCollection();

        //if ((model?.BaseContent as FeedContentImageModel)?.TapAction != null)
        //{
        //    ActionHandlerService service = new ActionHandlerService();
        //    service.HandleActionAsync((model.BaseContent as FeedContentImageModel).TapAction, model.ActionType);
        //    return;
        //}
        //if (model?.ActionDictionary != null && model.ActionDictionary.Keys.Contains("View It"))
        //{
        //    var viewItAction = model.ActionDictionary["View It"];
        //    string url = viewItAction.ActionParamDict["FeedURL"];
        //    RefreshFeedByUrl(url);
        //}
        //}

        private void OnRefreshFeedDidBegin(object sender, EventArgs args)
        {
            tvFeedItems.ShowLoader();
            //_overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
        }

        private void OnRefreshFeedComplete(object sender, FeedResponseModel response)
        {
            RefreshFeedComplete(response);
        }

        private void RefreshFeed()
        {
            tvFeedItems.ReloadData();
            tvFeedItems.ShowLoader();
            SL.Manager.GetFeedAsync(RefreshFeedComplete);
        }

        private void RefreshFeedComplete(FeedResponseModel response)
        {
            try
            {

                if (response.ResponseCode <= 0)
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "Oops!",
                        Message = "Sorry about that - it looks like we're unable to load your content right now. Please try again in a minute."
                    };
                    alert.AddButton("OK");
                    alert.Show();
                    tvFeedItems.HideLoader();
                    //show alert
                    return;
                }
                tvFeedItems.ReloadData();

                tvFeedItems.HideLoader();

            }
            catch (Exception)
            {
            }
        }

        private void ChangeFeed()
        {
            if (ShouldGetProfileByFriendId)
            {
                SL.Manager.GetFeedByFriendIdAsync(FriendId, ChangeFeedComplete);
                return;
            }

            SL.Manager.GetFeedByUrlAsync(FeedUrl, ChangeFeedComplete);

            //if (IsModal)
            //{
            //    SL.Manager.GetFeedByUrlAsync(FeedUrl, ChangeFeedComplete);
            //    return;
            //}
            //SL.Manager.GetFeedAsync(ChangeFeedComplete);
        }

        private void ChangeFeedComplete(FeedResponseModel response)
        {
            try
            {
                ImageService.Instance.InvalidateMemoryCache();

                if (response.ResponseCode > 0)
                    tvFeedItems.ReloadData();
                tvFeedItems.HideLoader();
            }
            catch
            {
                tvFeedItems.HideLoader();
            }
        }

        private void RefreshFeedByUrl(string feedUrl)
        {
            tvFeedItems.ShowLoader(); // _overlay = Platform.AddOverlay(UIScreen.MainScreen.Bounds, UIColor.White, true, true);
            SL.Manager.GetFeedByUrlAsync(feedUrl, GetFeedComplete);
        }

        private void RefreshProfileByUrlComplete(ProfileResponseModel profileResponse)
        {

        }

        private void GetProfileComplete(ProfileResponseModel profileResponse)
        {
            try
            {
                if (profileResponse.ResponseCode > 0)
                {
                    ImageService.Instance.LoadUrl(profileResponse.Profile.ProfilePictureURL).Into(ivUserImage);
                    lblUserName.Text = StringWithEmojiConverter.ConvertEmojiFromServer(profileResponse.Profile.UserName);
                    lblScoreValue.Text = profileResponse.Profile.Score.ToString();
                    lblRankCategory.Text = profileResponse.Profile.ScoreLabel;

                    lblFriendsInfo.Text = profileResponse.Profile.FriendPreviewList?.Count > 0 ? profileResponse.Profile.FriendPreviewList?.Count + "Friends" : string.Empty;
                }
            }
            catch (Exception)
            {
            }
        }

        private void GetFeedComplete(FeedResponseModel feedResponse)
        {

            ImageService.Instance.InvalidateMemoryCache();

            if (feedResponse.ResponseCode > 0)
            {
                tvFeedItems.ReloadData();

                if (tvFeedItems.NumberOfRowsInSection(0) > 0)
                {
                    tvFeedItems.ScrollToRow(Foundation.NSIndexPath.FromRowSection(0, 0), UITableViewScrollPosition.Top, true);
                }
            }
            tvFeedItems.HideLoader();
        }

        private async Task RefreshProfile()
        {
            await SL.Manager.GetProfileAsync(ProcessNotificationList);
            //AreaCollection.ReloadData();
            //UpdateNavBar();

        }

        private void ProcessNotificationList(ProfileResponseModel profile)
        {
            //var notifications = profile?.ResponseNotificationList;
            //if (notifications?.Count > 0)
            //{

            //}
        }
    }
}