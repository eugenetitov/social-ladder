using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using SocialLadder.Enums;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.MessangerModels;
using SocialLadder.ViewModels.Challenges;
using SocialLadder.ViewModels.Challenges.ChallengesDetails;
using SocialLadder.ViewModels.Feed;
using SocialLadder.ViewModels.Rewards;
using SocialLadder.ViewModels.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Services
{
    public class ActionHandlerService : IActionHandlerService
    {
        private string _dialogSubmisionUrl;
        private IAlertService _alertService;
        private IMvxNavigationService _navigationService;
        private IMvxMessenger _messenger;
        private IPlatformNavigationService _platformNavigationService;

        public async Task HandleActionAsync(IMvxNavigationService navigationService, IAlertService alertService, IMvxMessenger messenger, FeedActionModel model, string actionType = null, string AreaGUID = null)
        {
            _navigationService = navigationService;
            _alertService = alertService;
            _messenger = messenger;
            _platformNavigationService = Mvx.Resolve<IPlatformNavigationService>();
            if (model == null)
            {
                return;
            }
            if ((model.ActionScreen == ActionTypeConstants.ChallengeActionType || actionType == ActionTypeConstants.ReferralAcceptedActionType || 
                actionType == ActionTypeConstants.ItbCompletedActionType || actionType == ActionTypeConstants.CtacActionType) && model.ActionScreen != ActionTypeConstants.DialogActionType)
            {
                OpenChallengeDetailsScreenWithURL(model.ActionParamDict[ActionTypeConstants.ChallengeDetailURLActionParamDict]);
                return;
            }
            if (model.ActionScreen == ActionTypeConstants.RewardActionType)
            {
                await SL.Manager.GetRewardByUrl(model.ActionParamDict[ActionTypeConstants.RewardDetailURLActionParamDict], GetRewardByUrlComplete);
                return;
            }
            if (model.ActionScreen == ActionTypeConstants.RewardListActionType)
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.REWARDS);
            }
            if (model.ActionScreen == ActionTypeConstants.ChallengeListActionType)
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.CHALLENGES);
            }
            if (model.ActionScreen == ActionTypeConstants.FeedActionType)
            {
                string feedUrl = string.Empty;
                model.ActionParamDict.TryGetValue(ActionTypeConstants.FeedURLActionParamDict, out feedUrl);
                if ((actionType == ActionTypeConstants.AggChalcompActionType) || (actionType == ActionTypeConstants.AggCommitActionType))
                {
                    await LoadFeedByUrl(feedUrl);
                }
                else
                {
                    await LoadFeedByUrl(feedUrl, true);
                }
            }
            if (model.ActionScreen == ActionTypeConstants.WebActionType)
            {
                string url = string.Empty;
                bool gotUrl = model.ActionParamDict.TryGetValue(ActionTypeConstants.WebRequestURLActionParamDict, out url);
                if (gotUrl == false)
                {
                    url = model.WebRequestURL;
                }
                if (url == string.Empty)
                {
                    _alertService.ShowToast("There isn't url to open");
                }
                _platformNavigationService.NavigateToUrl(url);
            }
            if (model.ActionScreen == ActionTypeConstants.SettingsActionType)
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.MORE);
            }
            if (model.ActionScreen == ActionTypeConstants.ShareActionType)
            {
                if (!model.ActionParamDict.ContainsKey(ActionTypeConstants.ChallengeDetailURLActionParamDict))
                    return;

                OpenChallengeDetailsScreenWithURL(model.ActionParamDict[ActionTypeConstants.ChallengeDetailURLActionParamDict]);
            }
            if (model.ActionScreen == ActionTypeConstants.SwitchAreaActionType)
            {
                await SwitchAreaIfNeeded(AreaGUID);
            }
            if (model.ActionScreen == ActionTypeConstants.InviteActionType)
            {
                await SL.Manager.RefreshShareTemplate(model.ActionParamDict[ActionTypeConstants.ShareTemplateURLActionParamDict], SendInviteMessage);
            }
            if (model.ActionScreen == ActionTypeConstants.DialogActionType)
            {
                var dialogStyle = model.ActionParamDict[ActionTypeConstants.DialogStyleActionParamDict];
                _dialogSubmisionUrl = model.ActionParamDict[ActionTypeConstants.SubmissionURLActionParamDict];

                var message = model.ActionParamDict[ActionTypeConstants.CaptionActionParamDict];
                if (dialogStyle != string.Empty)
                {
                    if (dialogStyle == ActionTypeConstants.NoneDialogStyle)
                    {

                    }
                    if (dialogStyle == ActionTypeConstants.EntryDialogStyle)
                    {
                        var result = await _alertService.ShowAlertWithInput("", message, string.Empty, "Ok", "Cancel");
                        Dismissed(result);
                    }
                }
            }

            if (model.ActionScreen == ActionTypeConstants.IWebActionType && actionType == ActionTypeConstants.CtaiwActionType)
            {
                string actionUrl = string.Empty;
                bool gotUrl = model.ActionParamDict.TryGetValue(ActionTypeConstants.WebRequestURLActionParamDict, out actionUrl);
                if (string.IsNullOrEmpty(actionUrl))
                {
                    var firstUrl = model.ActionParamDict.Values.Count > 0 ? model.ActionParamDict.Values.FirstOrDefault() : string.Empty;
                    actionUrl = model.ActionParamDict[ActionTypeConstants.WebRequestURLActionParamDict] ?? firstUrl;
                    
                }
                if (gotUrl == false)
                {
                    //url = model.WebRequestURL;
                }
                if(string.IsNullOrEmpty(actionUrl))
                {
                    _alertService.ShowToast("There isn't url to open");
                    return;
                }

                //actionUrl = "https://goo.gl/?link=" + actionUrl;
                await _navigationService.Navigate<WebViewModel>();
                _messenger.Publish<MessangerWebModel>(new MessangerWebModel(this, actionUrl, true));
                return;
            }

            if (model.ActionScreen == ActionTypeConstants.IWebActionType)
            {
                string url = string.Empty;
                bool gotUrl = model.ActionParamDict.TryGetValue(ActionTypeConstants.WebRequestURLActionParamDict, out url);
                if (gotUrl == false)
                {
                    url = model.WebRequestURL;
                }
                if (url == string.Empty)
                {
                    _alertService.ShowToast("There isn't url to open");
                }
                _platformNavigationService.NavigateToUrl(url);
            }
            if (model.ActionScreen == ActionTypeConstants.ScoreActionType)
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.POINTS);
            }
            if (model.ActionScreen == ActionTypeConstants.FriendListActionType)
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.POINTS);
            }
        }

        private async void OpenChallengeDetailsScreenWithURL(string url)
        {
            var response = await SL.Manager.GetChallengeByUrl(url);
            if (response != null && response.Challenge != null)
            {
                var challenge = response.Challenge;
                bool isAssigned = true;
                if (challenge.TypeCode == ChallengesConstants.ChallengeItb && !isAssigned)
                {
                    _alertService.ShowOkCancelMessage("Action error", "You have not been assigned this challenge!", null, null, false);
                    return;
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeInvite && (challenge.TypeCodeDisplayName == ChallengesConstants.ChallengeInviteToBuyDisplayNames || challenge.TypeCodeDisplayName == ChallengesConstants.ChallengeInviteToJoinDisplayNames))//ITJ
                {
                    await _navigationService.Navigate<InviteViewModel>();
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeMC || challenge.TypeCode == ChallengesConstants.ChallengeSignUp)
                {
                    await _navigationService.Navigate<MultipleChoiceViewModel>();
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeInsta)
                {
                    await _navigationService.Navigate<InstagramViewModel>();
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeCheckin)
                {
                    await _navigationService.Navigate<CheckInViewModel>();
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeShare && challenge.TypeCodeDisplayName == ChallengesConstants.ChallengeTwitterDisplayNames)
                {
                    await _navigationService.Navigate<TwitterViewModel>();
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeShare && challenge.TypeCodeDisplayName == ChallengesConstants.ChallengeFacebookDisplayNames)
                {
                    await _navigationService.Navigate<FacebookViewModel>();
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeCollateralTracking)
                {
                    await _navigationService.Navigate<CollateralTrackingViewModel>();
                }
                if (challenge.TypeCode == ChallengesConstants.ChallengeFBEngagement)
                {
                    await _navigationService.Navigate<FBEngagementViewModel>();
                }
                _messenger.Publish(new MessangerChallengeModel(this, challenge));
            }
            else
            {
                _alertService.ShowOkCancelMessage(string.Empty, "This challenge has not been assigned to you.", null, null, false);
            }
        }

        public async Task SwitchAreaIfNeeded(string areaGuid)
        {
            if (SL.AreaGUID != areaGuid)
            {
                var area = SL.AreaList.Where(x => x.areaGUID == areaGuid).FirstOrDefault();
                if (area != null)
                {
                    await _platformNavigationService.ChangeArea(area.areaID);
                }
            }
        }

        public void Dismissed(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                var model = new UserInputModel();
                model.ButtonResponse = 1;
                model.UserInputText = result;
                SL.Manager.PostDialog(_dialogSubmisionUrl, model, DialogSubmited);
                _dialogSubmisionUrl = null;
            }
        }

        private void DialogSubmited(UpdatedFeedResponceModel responce)
        {
            if (!String.IsNullOrWhiteSpace(responce.ResponseMessage))
            {
                _alertService.ShowMessage(null, responce.ResponseMessage);
            }
        }

        private async void SendInviteMessage(ShareResponseModel model)
        {
            var smsService = Mvx.Resolve<ISmsService>();
            _alertService.ShowAlertMessage("What kind of message?", new List<ConfigurableAlertAction> {
                new ConfigurableAlertAction{ Text = "WhatsApp", OnClickHandler = new Action(() => { smsService.SendSmsToWatsApp(string.Empty, $"{model.ShareTemplate.InviteText} {model.ShareTemplate.ActionLink}"); }) },
                new ConfigurableAlertAction{ Text = "SMS", OnClickHandler = new Action(async() => {
                    await _navigationService.Navigate<ContactsPickerViewModel>();
                   _messenger.Publish(new MessengerActionHandlerModel(this, model)); }) },
                new ConfigurableAlertAction{ Text = "Cancel", OnClickHandler = new Action(() => { }) },
            });
        }

        private async Task LoadFeedByUrl(string url, bool isSingleItem = false)
        {
            //var tabToNavigate = ENavigationTabs.FEED;
            //_platformNavigationService.NavigateToTab(tabToNavigate);
            if (isSingleItem)
            {
                //Do something             
            }
            await _navigationService.Navigate<FeedDetailsViewModel>();
            _messenger.Publish(new MessangerFeedUrlModel(this, url, string.Empty, 0, false));
        }

        private void NavigateByAction(string storyboardName, string viewControllerName, ENavigationTabs tabToNavigate)
        {
            _platformNavigationService.NavigateToTab(tabToNavigate);
        }

        private async void GetRewardByUrlComplete(RewardItemModel rewardItem)
        {
            if (rewardItem.Type == ActionTypeConstants.RewardRewardType)
            {
                var parameter = new MessangerRewardModel(this, rewardItem);
                await _navigationService.Navigate<RewardsDetailsViewModel>();
                _messenger.Publish<MessangerRewardModel>(parameter);
                return;
            }

            if (rewardItem.Type == ActionTypeConstants.CategoryRewardType)
            {
                var parameter = new MessangerRewardModel(this, rewardItem, rewardItem.Name);
                await _navigationService.Navigate<RewardsViewModel>();
                _messenger.Publish<MessangerRewardModel>(parameter);
                return;
            }
        }
    }
}
