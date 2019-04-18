using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums;
using SocialLadder.Interfaces;
using SocialLadder.Interfaces.Base;
using SocialLadder.Models;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Feed;

namespace SocialLadder.ViewModels.Base
{
    public class BaseFeedViewModel : BaseViewModel
    {
        #region Fields
        private readonly IActionHandlerService _actionHandlerService;
        private bool _isLikePerfomming;
        private object _likeLocker = new object();
        #endregion

        #region ctors
        public BaseFeedViewModel(IMvxNavigationService navigationService, IAlertService alertService, IPlatformAssetService assetService, ILocalNotificationService localNotificationService, IMvxMessenger messanger) : base(navigationService, alertService, assetService, localNotificationService, messanger)
        {
            _actionHandlerService = Mvx.Resolve<IActionHandlerService>();
            FeedItems = new MvxObservableCollection<FeedItemModel>();
            FeedLoaderHidden = false;
        }
        #endregion

        #region Properties
        private MvxObservableCollection<FeedItemModel> _feedItems;
        public MvxObservableCollection<FeedItemModel> FeedItems
        {
            get
            {
                return _feedItems;
            }
            set
            {
                _feedItems = value;
                RaisePropertyChanged(() => FeedItems);
            }
        }

        private bool _feedLoaderHidden;
        public bool FeedLoaderHidden
        {
            get => _feedLoaderHidden;
            set => SetProperty(ref _feedLoaderHidden, value);
        }

        #endregion
        #region Interactions
        private MvxInteraction<UpdatedFeedItemModel> _feedItemUpdateInteraction;
        public IMvxInteraction<UpdatedFeedItemModel> FeedItemUpdateInteraction
        {
            get
            {
                if (_feedItemUpdateInteraction == null)
                {
                    _feedItemUpdateInteraction = new MvxInteraction<UpdatedFeedItemModel>();
                }
                return _feedItemUpdateInteraction;
            }
        }

        private MvxInteraction<List<FeedEngagementModel>> _showCommentsInteraction = new MvxInteraction<List<FeedEngagementModel>>();
        public IMvxInteraction<List<FeedEngagementModel>> ShowCommentsInteraction => _showCommentsInteraction;
        #endregion

        #region Commands
        private MvxCommand<FeedItemModel> _showAllCommentsCommand;
        public MvxCommand<FeedItemModel> ShowAllCommentsCommand
        {
            get
            {
                return _showAllCommentsCommand != null ? _showAllCommentsCommand : new MvxCommand<FeedItemModel>((item) =>
                {
                    var engagements = item.FilteredEngagementList.Where(x => x.EngagementType == "COMMENT").ToList();
                    _showCommentsInteraction.Raise(engagements);
                });
            }
        }

        private MvxAsyncCommand _loadNextCommand;
        public MvxAsyncCommand LoadNextPageCommand
        {
            get
            {
                if (_loadNextCommand == null)
                {
                    _loadNextCommand = new MvxAsyncCommand(async () =>
                    {
                        await LoadNextPage();
                    });
                }
                return _loadNextCommand;
            }
        }

        //private MvxAsyncCommand<FeedItemModel> _likeFeedCommand;
        //public MvxAsyncCommand<FeedItemModel> LikeFeedCommand
        //{
        //    get
        //    {
        //        if (_likeFeedCommand == null)
        //        {
        //            _likeFeedCommand = new MvxAsyncCommand<FeedItemModel>(async (item) =>
        //            {
        //                //lock (_likeLocker)
        //                //{
        //                //    if (_isLikePerfomming)
        //                //    {
        //                //        return;
        //                //    }
        //                //    _isLikePerfomming = true;
        //                //}
        //                await PostLike(item);
        //                //_isLikePerfomming = true;

        //            });
        //        }
        //        return _likeFeedCommand;

        //    }
        //}

        public virtual MvxAsyncCommand<FeedItemModel> LikeFeedCommand
        {
            get
            {
                return new MvxAsyncCommand<FeedItemModel>(async (item) =>
                {
                    await PostLike(item);
                });
            }
        }

        private MvxAsyncCommand<FeedItemModel> _commentFeedCommand;
        public MvxAsyncCommand<FeedItemModel> CommentFeedCommand
        {
            get
            {
                if (_commentFeedCommand == null)
                {
                    _commentFeedCommand = new MvxAsyncCommand<FeedItemModel>(async (item) =>
                    {
                        await PostComment(item);
                    });
                }
                return _commentFeedCommand;
            }
        }

        private MvxAsyncCommand _refreshFeedCommand;
        public MvxAsyncCommand RefreshFeedCommand
        {
            get
            {
                if (_refreshFeedCommand == null)
                {
                    _refreshFeedCommand = new MvxAsyncCommand(async () =>
                    {
                        await Refresh();
                    });
                }
                return _refreshFeedCommand;
            }
        }

        public virtual MvxAsyncCommand<FeedItemModel> LoadFeedByUrlCommand
        {
            get
            {
                return new MvxAsyncCommand<FeedItemModel>(async (item) =>
                {
                    if (this is IBaseChildViewModel)
                    {
                        return;
                    }
                    _feedItemUpdateInteraction.Raise(null);
                    await Task.Delay(200);
                    await _navigationService.Navigate<FeedDetailsViewModel>();
                    _messenger.Publish(new MessangerFeedUrlModel(this, item.Header.ActorFeedURL, item.Header.ActorProfileURL, 0, true));
                    _feedItemUpdateInteraction.Raise(null);
                });
            }
        }

        public MvxAsyncCommand<FeedItemModel> PostReportItCommand
        {
            get
            {
                return new MvxAsyncCommand<FeedItemModel>(async (item) =>
                {
                    IsBusy = true;
                    var listActions = new List<ConfigurableAlertAction>();
                    if (item.ActionDictionary.ContainsKey("Report This"))
                    {
                        listActions.Add(new ConfigurableAlertAction { Text = "Report", OnClickHandler = new Action(async () => { await _actionHandlerService.HandleActionAsync(_navigationService, _alertService, _messenger, item.ActionDictionary["Report This"], item.ActionType); }) });
                    }
                    if (item.ActionDictionary.ContainsKey("Flag It"))
                    {
                        listActions.Add(new ConfigurableAlertAction { Text = "Flag It", OnClickHandler = new Action(async () => { await _actionHandlerService.HandleActionAsync(_navigationService, _alertService, _messenger, item.ActionDictionary["Flag It"], item.ActionType); }) });
                    }
                    listActions.Add(new ConfigurableAlertAction { Text = "Cancel", OnClickHandler = new Action(() => { }) });
                    _alertService.ShowAlertMessage("Actions", listActions);
                    IsBusy = false;
                });
            }
        }

        public MvxAsyncCommand<FeedItemModel> InviteToBuyCommand
        {
            get
            {
                return new MvxAsyncCommand<FeedItemModel>(async (item) =>
                {
                    IsBusy = true;
                    await _actionHandlerService.HandleActionAsync(_navigationService, _alertService, _messenger, item.BaseContent?.TapAction, item.ActionType);
                    IsBusy = false;
                });
            }
        }

        public MvxAsyncCommand<FeedItemModel> InviteToJoinCommand
        {
            get
            {
                return new MvxAsyncCommand<FeedItemModel>(async (item) =>
                {
                    IsBusy = true;
                    await _actionHandlerService.HandleActionAsync(_navigationService, _alertService, _messenger, item.BaseContent?.TapAction, item.ActionType);
                    IsBusy = false;
                });
            }
        }

        public MvxAsyncCommand<FeedItemModel> FeedItemClick
        {
            get
            {
                return new MvxAsyncCommand<FeedItemModel>(async (item) =>
                {
                    IsBusy = true;
                    if ((item?.BaseContent as FeedContentImageModel)?.TapAction != null)
                    {
                        await _actionHandlerService.HandleActionAsync(_navigationService, _alertService, _messenger, (item.BaseContent as FeedContentImageModel).TapAction, item.ActionType);
                        return;
                    }
                    if (item?.BaseContent?.TapAction != null)
                    {
                        await _actionHandlerService.HandleActionAsync(_navigationService, _alertService, _messenger, item.BaseContent.TapAction, item.ActionType);
                        return;
                    }
                    IsBusy = false;
                });
            }
        }

        #endregion

        #region PrivateMethods
        public override async Task Refresh()
        {
            IsBusy = IsRefreshing = true;
            await RefreshProfile();
            await RefreshFeed();
            await FinishRefresh();
            IsBusy = IsRefreshing = false;
        }

        private async Task RefreshFeed()
        {
            await Task.Run(async () =>
            {
                await SL.Manager.GetFeedAsync((response) =>
                {
                    if (response.ResponseCode <= 0)
                    {
                        _alertService.ShowMessage("Oops", response.ResponseMessage);
                    }
                    FeedItems = new MvxObservableCollection<FeedItemModel>();
                    var items = SL.FeedList;
                    if (items != null)
                    {
                        FeedItems.AddRange(items);
                    }
                });
            });
        }

        private async Task RefreshProfile()
        {
            await SL.Manager.GetProfileAsync(null);
        }

        private async Task LoadNextPage()
        {
            if (SL.FeedNextPage != null)
            {
                FeedLoaderHidden = true;
                _feedItemUpdateInteraction.Raise(new UpdatedFeedItemModel { LoaderMode = FeedLoadingIndicatorMode.NeedShow });
                var feedResponse = await SL.Manager.AddFeedAsync(SL.FeedNextPage);
                FeedItems.AddRange(feedResponse.FeedPage.FeedItemList);
                _feedItemUpdateInteraction.Raise(new UpdatedFeedItemModel { LoaderMode = FeedLoadingIndicatorMode.NeedHide });
                FeedLoaderHidden = false;
            }
        }

        private async Task PostLike(FeedItemModel feedItem)
        {
            IDictionary<string, string> paramDictionary;
            if (feedItem.ActionDictionary.ContainsKey(FeedActionType.Like.ToString()))
            {
                paramDictionary = feedItem.ActionDictionary[FeedActionType.Like.ToString()]?.ActionParamDict;
            }
            else
            {
                paramDictionary = feedItem.ActionDictionary[FeedActionType.Boost.ToString()].ActionParamDict;
            }
            string submisionUrl = paramDictionary[FeedActionParam.SubmissionURL.ToString()];
            var likeResponce = await SL.Manager.PostLikeFeedAsync(submisionUrl);
            if (likeResponce == null)
            {
                return;
            }

            _feedItemUpdateInteraction.Raise(new UpdatedFeedItemModel() { UpdatedFeedItem = likeResponce.UpdatedFeedItem, OldFeedItem = feedItem, LoaderMode = FeedLoadingIndicatorMode.Default });

            var indexItem = FeedItems.IndexOf(feedItem);
            var newFeedItem = FeedItems[indexItem];
            newFeedItem.DidLike = likeResponce.UpdatedFeedItem.DidLike;
            newFeedItem.Likes = likeResponce.UpdatedFeedItem.Likes;
            FeedItems[indexItem] = newFeedItem;
        }

        private async Task PostComment(FeedItemModel feedItem)
        {
            var paramDictionary = feedItem.ActionDictionary[FeedActionType.Comment.ToString()].ActionParamDict;
            string submisionUrl = paramDictionary[FeedActionParam.SubmissionURL.ToString()];
            var comment = await _alertService.ShowAlertWithInput("Please enter your comments");
            
            if (string.IsNullOrEmpty(comment))
            {
                return;
            }
            comment = comment.Trim();

            string commentInByte = BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(comment)).Replace("-", "");

            UserInputModel userInputModel = new UserInputModel()
            {
                ButtonResponse = 1,
                UserInputText = commentInByte
            };

            var commentResponce = await SL.Manager.PostDialog(submisionUrl, userInputModel);
            if (commentResponce == null)
            {
                return;
            }

            _feedItemUpdateInteraction.Raise(new UpdatedFeedItemModel() { UpdatedFeedItem = commentResponce.UpdatedFeedItem, OldFeedItem = feedItem, LoaderMode = FeedLoadingIndicatorMode.Default });

            var indexItem = FeedItems.IndexOf(feedItem);
            var newFeedItem = FeedItems[indexItem];
            
            newFeedItem.FilteredEngagementList = commentResponce.UpdatedFeedItem.FilteredEngagementList;
            FeedItems[indexItem] = newFeedItem;
            //FeedItems[indexItem].FilteredEngagementList = commentResponce.UpdatedFeedItem.FilteredEngagementList;
        }
        #endregion
    }
}
