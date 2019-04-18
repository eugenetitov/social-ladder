using System;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using SocialLadder.iOS.Areas;
using SocialLadder.iOS.Challenges;
using SocialLadder.iOS.Helpers;
using SocialLadder.iOS.Models.Enums;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Rewards;
using SocialLadder.iOS.ViewControllers;
using SocialLadder.iOS.ViewControllers.Feed;
using SocialLadder.iOS.ViewControllers.Main;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Services
{
    public class ActionHandlerService : UIAlertViewDelegate
    {
        private string _dialogSubmisionUrl;

        public void HandleActionAsync(FeedActionModel model, string actionType = null, string AreaGUID = null)
        {

            if (model == null)
            {
                /*
                UIAlertView alert = new UIAlertView()
                {
                    Title = "Action error",
                    Message = "Action data empty"
                };
                alert.AddButton("OK");
                alert.Show();
                */
                return;
            }
            UIStoryboard storyboard = UIStoryboard.FromName("Main", null);
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;

            if ((model.ActionScreen == "CHALLENGE" || actionType == "REFERRAL_ACCEPTED" || actionType == "ITB_COMPLETE" || actionType == "CTAC") && model.ActionScreen != "DIALOG")
            {
                OpenChallengeDetailsScreenWithURL(model.ActionParamDict["ChallengeDetailURL"]);
                return;
            }
            if (model.ActionScreen == "REWARD")
            {
                SL.Manager.GetRewardByUrl(model.ActionParamDict["RewardDetailURL"], GetRewardByUrlComplete);

                return;
            }
            if (model.ActionScreen == "REWARDLIST")
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.REWARDS);

            }
            if (model.ActionScreen == "CHALLENGELIST")///////////////////
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.CHALLENGES);

            }
            if (model.ActionScreen == "FEED")
            {
                string feedUrl = string.Empty;
                model.ActionParamDict.TryGetValue("FeedURL", out feedUrl);           

                LoadFeedByUrl(feedUrl);
                //NavigateByAction("Main", "MainNav1", ENavigationTabs.FEED);

            }
            if (model.ActionScreen == "WEB")
            {
                string url = string.Empty;
                bool gotUrl = model.ActionParamDict.TryGetValue("WebRequestURL", out url);
                if (gotUrl == false)
                {
                    url = model.WebRequestURL;
                }
                if (url == string.Empty)
                {
                    //alert no url to open
                    //return;
                }
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }
            if (model.ActionScreen == "SETTINGS")
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.MORE);
            }
            if (model.ActionScreen == "SHARE")
            {
                if (!model.ActionParamDict.ContainsKey("ChallengeDetailURL"))
                    return;

                OpenChallengeDetailsScreenWithURL(model.ActionParamDict["ChallengeDetailURL"]);
            }
            if (model.ActionScreen == "SwitchArea")
            {
                SwitchAreaIfNeeded(AreaGUID);
            }
            if (model.ActionScreen == "INVITE")
            {
                SL.Manager.RefreshShareTemplate(model.ActionParamDict["ShareTemplateURL"], SendInviteMessage);
            }
            if (model.ActionScreen == "DIALOG")///////////////////
            {
                var dialogStyle = model.ActionParamDict["DialogStyle"];
                _dialogSubmisionUrl = model.ActionParamDict["SubmissionURL"];

                var message = model.ActionParamDict["Caption"];
                if (dialogStyle != string.Empty)
                {
                    if (dialogStyle == "NONE")
                    {

                    }
                    if (dialogStyle == "ENTRY")
                    {
                        var alert = new UIAlertView("", message, this as IUIAlertViewDelegate, "Cancel", null);
                        alert.AddButton("Ok");
                        alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;
                        var textField = alert.GetTextField(0);
                        alert.Show();
                    }
                }
            }

            if (model.ActionScreen == "IWEB" && actionType == "CTAIW")
            {
                string url = string.Empty;
                bool gotUrl = model.ActionParamDict.TryGetValue("WebRequestURL", out url);
                url = model.ActionParamDict["WebRequestURL"];
                if (gotUrl == false)
                {
                    //url = model.WebRequestURL;
                }
                if (url == string.Empty)
                {
                    //alert no url to open
                    //return;
                }
                GenericWebViewController webViewController = new GenericWebViewController();
                webViewController.Url = url;

                UIViewController parentController = Platform.TopViewController;
                parentController.PresentViewController(webViewController, false, null);

                return;
            }

            if (model.ActionScreen == "IWEB")
            {
                string url = string.Empty;
                bool gotUrl = model.ActionParamDict.TryGetValue("WebRequestURL", out url);
                if (gotUrl == false)
                {
                    url = model.WebRequestURL;
                }
                if (url == string.Empty)
                {
                    //alert no url to open
                    //return;
                }
                UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
            }

            if (model.ActionScreen == "SCORE")
            {
                NavigateByAction("Main", "MainNav1", ENavigationTabs.POINTS);
            }
            if (model.ActionScreen == "FRIENDLIST")
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
                UIStoryboard board = UIStoryboard.FromName("Challenges", null);
                ChallengeDetailBaseViewController controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("MultipleChoiceViewController");

                //check if user wasn't assigned to this challenge
                //
                bool isAssigned = true;
                if (challenge.TypeCode == "ITB" && !isAssigned)
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "Action error",
                        Message = "You have not been assigned this challenge!"
                    };
                    alert.AddButton("OK");
                    alert.Show();

                    return;
                }

                if (challenge.TypeCode == "INVITE")//ITJ
                {
                    controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("ChallengeDetailViewController");
                }

                if (challenge.TypeCode == "ITB")
                {
                    controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("ChallengeDetailViewController");
                }

                if (challenge.TypeCode == "MC" || challenge.TypeCode == "SIGNUP")
                {
                    controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("MultipleChoiceViewController");
                }
                else if (challenge.TypeCode == "INSTA")
                {
                    controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("InstagramViewController");
                }
                else if (challenge.TypeCode == "CHECKIN")
                {
                    controller = (CheckInViewController)board.InstantiateViewController("CheckInViewController");
                }
                else if (challenge.TypeCode == "SHARE" && challenge.TypeCodeDisplayName == "Facebook")
                {
                    controller = (FacebookDetailViewController)board.InstantiateViewController("FacebookDetailViewController");
                }
                else
                {
                    controller = (ChallengeDetailBaseViewController)board.InstantiateViewController("ChallengeDetailViewController");
                }
                controller.Challenge = challenge;
                var rootViewController = GetRootViewController();
                rootViewController.PushViewController(controller, true);
            }
            else
            {
                UIAlertView alert = new UIAlertView()
                {
                    Message = "This challenge has not been assigned to you."
                };
                alert.AddButton("OK");
                alert.Show();
            }
        }

        public static void SwitchAreaIfNeeded(string areaGuid)
        {

            if (SL.AreaGUID != areaGuid)
            {
                var area = SL.AreaList.Where(x => x.areaGUID == areaGuid).FirstOrDefault();
                if (area != null)
                {
                    SL.AreaGUID = area.areaGUID;
                    UIViewController parentController = Platform.RootViewController;
                    if (parentController?.GetType() == typeof(SLNavigationController))
                    {
                        var child = parentController.ChildViewControllers[0];
                        //(child as MainViewController).SelectedIndex = (int)ENavigationTabs.FEED;
                        (child as MainViewController).RefreshAreas();
                        //Platform.PreselectedFeedUrl = null;

                        return;
                    }

                    parentController = Platform.TopViewController;
                    if (parentController?.GetType() == typeof(SLNavigationController))
                    {
                        var child = parentController.ChildViewControllers[0];
                        //(child as MainViewController).SelectedIndex = (int)ENavigationTabs.FEED;
                        (child as MainViewController).RefreshAreas();
                        return;
                    }
                }
            }
        }

        public override void Dismissed(UIAlertView alertView, nint buttonIndex)
        {
            if (buttonIndex == 1)
            {
                var model = new UserInputModel();
                model.ButtonResponse = (int)buttonIndex;
                model.UserInputText = alertView.GetTextField(0).Text;
                SL.Manager.PostDialog(_dialogSubmisionUrl, model, DialogSubmited);
                _dialogSubmisionUrl = null;
            }
        }

        private void DialogSubmited(UpdatedFeedResponceModel responce)
        {
            if (!String.IsNullOrWhiteSpace(responce.ResponseMessage))
            {
                Platform.ShowAlert(null, responce.ResponseMessage);
            }
        }

        private void SendInviteMessage(ShareResponseModel model)
        {
            var _messageService = new PlatformServices.SmsService();
            var alertController = UIAlertController.Create("", "What kind of message?", UIAlertControllerStyle.ActionSheet);

            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
            // Add Actions
            alertController.AddAction(UIAlertAction.Create("WhatsApp", UIAlertActionStyle.Default, new Action<UIAlertAction>((handler) =>
            {
                _messageService.SendSmsToWatsApp("", $"{model.ShareTemplate.InviteText} {model.ShareTemplate.ActionLink}");
            })));

            alertController.AddAction(UIAlertAction.Create("SMS", UIAlertActionStyle.Default, new Action<UIAlertAction>((handler) =>
            {
                UIStoryboard board = UIStoryboard.FromName("Challenges", null);
                UIViewController controller = (UIViewController)board.InstantiateViewController("ContactViewController");
                rootViewController.PushViewController(controller, true);
            })));

            alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel sending message")));
            rootViewController.PresentViewController(alertController, true, null);
        }

        private void LoadFeedByUrl(string url)
        {
            UIStoryboard storyboard = UIStoryboard.FromName("Main", null);

            UIViewController parentController = Platform.RootViewController;
            if (parentController?.GetType() == typeof(SLNavigationController))
            {
                var child = parentController.ChildViewControllers[0];
                (child as MainViewController).SelectedIndex = (int)ENavigationTabs.FEED;
                PopToViewControllerIfNeeded(child as MainViewController);
                ((FeedViewController)((child as MainViewController).SelectedViewController)).RefreshFeedByUrl(url);
                //Platform.PreselectedFeedUrl = null;
                return;
            }

            parentController = Platform.TopViewController;
            if (parentController?.GetType() == typeof(SLNavigationController))
            {
                var child = parentController.ChildViewControllers[0];
                (child as MainViewController).SelectedIndex = (int)ENavigationTabs.FEED;
                PopToViewControllerIfNeeded(child as MainViewController);
                ((FeedViewController)((child as MainViewController).SelectedViewController)).RefreshFeedByUrl(url);
                return;
            }

        }

        private void PopToViewControllerIfNeeded(MainViewController mainViewController)
        {
            var presentedViewController = (mainViewController?.ParentViewController as SLNavigationController)?.TopViewController;
            if (presentedViewController is MainViewController)
                return;

            presentedViewController?.NavigationController?.PopToRootViewController(true);
            (mainViewController.NavigationController as SLNavigationController)?.NavTitle?.RootMode();
        }

        private void NavigateByAction(string storyboardName, string viewControllerName, ENavigationTabs tabToNavigate)
        {
            UIStoryboard storyboard = UIStoryboard.FromName(storyboardName, null);

            Platform.PreselectedMainViewTab = tabToNavigate;

            SLNavigationController parentController = Platform.RootViewController as SLNavigationController;
            if (parentController != null)
            //if (parentController?.GetType() == typeof(SLNavigationController))
            {
                var child = parentController.ChildViewControllers[0];
                (child as MainViewController).SelectedIndex = (int)tabToNavigate;
                PopToViewControllerIfNeeded(child as MainViewController);
                return;
            }

            parentController = Platform.TopViewController as SLNavigationController;
            if (parentController != null)
            //if (parentController?.GetType() == typeof(SLNavigationController))
            {
                var child = parentController.ChildViewControllers[0];
                (child as MainViewController).SelectedIndex = (int)tabToNavigate;
                PopToViewControllerIfNeeded(child as MainViewController);
                return;
            }

            SLNavigationController viewController = storyboard.InstantiateViewController(viewControllerName) as SLNavigationController;
            parentController.PresentViewController(viewController, false, null);
        }

        private void GetRewardByUrlComplete(RewardItemModel rewardItem)
        {
            var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController as UINavigationController;
            UIStoryboard board = UIStoryboard.FromName("Rewards", null);

            if (rewardItem.Type == "REWARD")
            {
                var vc = (RewardDetailViewController)board.InstantiateViewController("RewardDetailViewController");
                vc.Reward = rewardItem;
                rootViewController.PushViewController(vc, true);
                return;
            }

            if (rewardItem.Type == "CATEGORY")
            {
                RewardsViewController ctrl = (RewardsViewController)board.InstantiateViewController("RewardsViewController") as RewardsViewController;
                ctrl.RewardsCategoryName = rewardItem.Name;
                if (rewardItem.ChildList != null)
                    ctrl.RewardList = rewardItem.ChildList;
                else
                {
                    ctrl.CategoryID = rewardItem.ID;
                    SL.RewardList = null;
                }

                rootViewController.PushViewController(ctrl, true);
            }
        }

        private SLNavigationController GetRootViewController()
        {
            UIViewController parentController = Platform.RootViewController;
            if (parentController?.GetType() == typeof(SLNavigationController))
            {
                return (SLNavigationController)parentController;
            }

            parentController = Platform.TopViewController;
            return (SLNavigationController)parentController;
        }
    }
}