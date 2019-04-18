using System;
using UIKit;
using SocialLadder.Authentication;
using SocialLadder.Services;
using SocialLadder.Models;
using Xamarin.Auth;
using Newtonsoft.Json;
using SocialLadder.iOS.More;
using CoreAnimation;
using Foundation;
using System.Linq;
using System.Collections.Generic;
using SocialLadder.iOS.Areas;
using SocialLadder.ViewModels.Intro;

namespace SocialLadder.iOS.ViewControllers.Intro
{
    public partial class NetworksViewController : IntroBaseViewController, IFacebookAuthenticationDelegate, ITwitterAuthenticationDelegate, IInstagramAuthenticationDelegate
    {
        public bool IsAddNetworkFlow
        {
            get; set;
        }
        public NetworksViewController(IntPtr handle) : base(handle)
        {

        }

        public override void PrepareContainer()
        {
            if (IntroContainerViewController != null)
            {
                IntroContainerViewController.PrepareForNetworks();
            }
        }

        void ApplyStyle()
        {
            NextButton.Enabled = SL.HasNetworks;
            // DoneButton.Layer.BorderColor = DoneButton.TitleLabel.TextColor.CGColor;
            // DoneButton.Layer.BorderWidth = 1f;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            ApplyStyle();

            HideViews();

            UpdateView();
        }

        private void AddBindongs()
        {
            //var set = this.CreateBindingSet<NetworksViewController, NetworksViewModel>();
            //set.Bind(NextButton).For(btn => btn.Enabled).To(vm => vm.BtnNextEnable);
            //set.Apply();
        }

        public void HideViews()
        {
            DoneButtonView.Hidden = true;
            DoneButtonStackView.Hidden = true;
            ProgressBarandNextButtonView.Hidden = true;
            ProgressBarNextButtonStackView.Hidden = true;
        }

        public void UnHideViews()
        {
            DoneButtonView.Hidden = false;
            DoneButtonStackView.Hidden = false;

            //ProgressBarandNextButtonView.Hidden = false;
            //ProgressBarNextButtonStackView.Hidden = false;
        }

        partial void FacebookLoginButton_TouchUpInside(UIButton sender)
        {
            //animate indicator
            FacebookConnectedImage.Image = UIImage.FromBundle("loading-indicator");
            Platform.AnimateRotation(FacebookConnectedImage);
            //push fb login
            var auth = new FacebookAuthenticator(Configuration.FbClientId, Configuration.Scope, this);
            var authenticator = auth.GetAuthenticator();
            var viewController = authenticator.GetUI();
            PresentViewController(viewController, true, null);
        }

        public async void OnFacebookAuthenticationCompleted(SocialNetworkModel network)
        {
            DismissViewController(true, null);

            Platform.ClearBrowserCache();
            //Update SL server
            var response = await SL.CheckInNetwork(network, Platform.Lat, Platform.Lon);

            Platform.AnimateRotationComplete(FacebookConnectedImage);

            UpdateView();

            if (response.ResponseCode > 0)
            {
                if (SL.HasAreas)
                    SL.RefreshAll();
            }
            else
            {
                await Platform.ShowAlert(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", "OK");
            }
        }

        public void OnFacebookAuthenticationFailed(string message, Exception exception)
        {
            Platform.AnimateRotationComplete(FacebookConnectedImage);
            UpdateView();
        }

        public void OnFacebookAuthenticationCanceled()
        {
            Platform.AnimateRotationComplete(FacebookConnectedImage);
            UpdateView();
        }

        partial void TwitterLoginButton_TouchUpInside(UIButton sender)
        {
            TwitterConnectedImage.Image = UIImage.FromBundle("loading-indicator");
            Platform.AnimateRotation(TwitterConnectedImage);

            var auth = new TwitterAuthentificator("QkS2cTMjtUD1F8N7NvA9w", "NVaB0TL4CmyWlFl0mXoTv9S5LncEdQx2uh2wkP7DsOo", Configuration.Scope, this);
            var authenticator = auth.GetAuthenticator();
            var viewController = authenticator.GetUI();
            PresentViewController(viewController, true, null);
        }

        public async void OnTwitterAuthenticationCompleted(SocialNetworkModel network)
        {
            DismissViewController(true, null);

            Platform.ClearBrowserCache();
            //Update SL server
            var response = await SL.CheckInNetwork(network, Platform.Lat, Platform.Lon);

            Platform.AnimateRotationComplete(TwitterConnectedImage);

            UpdateView();

            if (response.ResponseCode > 0)
            {
                if (SL.HasAreas)
                    SL.RefreshAll();
            }
            else
            {
                await Platform.ShowAlert(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", "OK");
            }
        }

        public void OnTwitterAuthenticationFailed(string message, Exception exception)
        {
            Platform.AnimateRotationComplete(TwitterConnectedImage);
            UpdateView();
        }

        public void OnTwitterAuthenticationCanceled()
        {
            Platform.AnimateRotationComplete(TwitterConnectedImage);
            UpdateView();
        }

        partial void InstagramLoginButton_TouchUpInside(UIButton sender)
        {
            InstagramConnectedImage.Image = UIImage.FromBundle("loading-indicator");
            Platform.AnimateRotation(InstagramConnectedImage);

            UIStoryboard board = UIStoryboard.FromName("Web", null);
            WebViewController ctrl = (WebViewController)board.InstantiateViewController("WebViewController");
            const string ClientID = "cf88ac6682e24ffe83441b6950e3134a";
            const string RedirectUrl = "http://socialladderapp.com";
            ctrl.Url = "https://api.instagram.com/oauth/authorize/?client_id=" + ClientID + "&redirect_uri=" + RedirectUrl + "&response_type=token";
            ctrl.InstagramDelegate = this;
            this.PresentViewController(ctrl, false, null);
        }

        async public void OnInstagramAuthenticationCompleted(SocialNetworkModel network)
        {
            DismissViewController(true, null);

            Platform.ClearBrowserCache();
            //var network1 = await SL.CheckInInstagram("Instagram", network.AccessToken, Platform.Lat, Platform.Lon);
            var response = await SL.CheckInNetwork(network, Platform.Lat, Platform.Lon);
            Platform.AnimateRotationComplete(InstagramConnectedImage);

            UpdateView();

            if (response.ResponseCode > 0)
            {
                InstagramLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_insta-connected"), UIControlState.Normal);
                Platform.AnimateRotationComplete(InstagramConnectedImage);
                InstagramConnectedImage.Image = UIImage.FromBundle("check-icon_green");
                NextButton.Enabled = true;
                if (SL.HasAreas)
                    SL.RefreshAll();
            }
            else
            {
                await Platform.ShowAlert(null, !string.IsNullOrEmpty(response.ResponseMessage) ? response.ResponseMessage : "Login failed", "OK");
            }
        }

        public void OnInstagramAuthenticationCanceled()
        {
            DismissViewController(true, null);
            Platform.AnimateRotationComplete(InstagramConnectedImage);
            InstagramConnectedImage.Image = null;
        }

        public void OnInstagramAuthenticationFailed(string message, Exception exception)
        {
            DismissViewController(true, null);
            Platform.AnimateRotationComplete(InstagramConnectedImage);
            InstagramConnectedImage.Image = null;
        }

        void LoadAreas()
        {
            UIStoryboard board = UIStoryboard.FromName("Areas", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("Areas");
            PresentViewController(ctrl, false, null);
        }

        void LoadPrivacyPolicy()
        {
            UIStoryboard board = UIStoryboard.FromName("More", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("PrivacyPolicyViewController");
            PresentViewController(ctrl, true, null);//NavigationController.PushViewController(ctrl, true);      
        }

        void LoadTermsService()
        {
            UIStoryboard board = UIStoryboard.FromName("More", null);
            UIViewController ctrl = (UIViewController)board.InstantiateViewController("TermsServiceViewController");
            PresentViewController(ctrl, true, null);

            //NavigationController.PushViewController(ctrl, true);
        }

        partial void BtnTermsService_TouchUpInside(UIButton sender)
        {
            LoadTermsService();
        }

        partial void BtnPrivacyPolicy_TouchUpInside(UIButton sender)
        {
            LoadPrivacyPolicy();
        }

        partial void DoneButton_TouchUpInside(UIButton sender)
        {
            if (SL.IsNetworkConnected("Facebook") || SL.IsNetworkConnected("Twitter") || SL.IsNetworkConnected("Instagram"))
            {
                if (SL.HasAreas)
                {
                    SL.Profile.SetDefaultAreaIfNeeded();
                    //IntroContainerViewController.LoadMain();

                    List<string> areaImageUrlList = SL.AreaList.Select(a => a.areaDefaultImageURL).ToList();
                    Services.FileCachingService.PreloadImagesToDiskFromUrl(areaImageUrlList);

                    var storyboard = UIStoryboard.FromName("Main", null);
                    UIViewController viewController = storyboard.InstantiateViewController("MainNav1") as UIViewController;
                    PresentViewController(viewController, true, null);
                }
                else
                {
                    UIStoryboard board = UIStoryboard.FromName("Areas", null);
                    UIViewController viewController = (UIViewController)board.InstantiateViewController("Areas");
                    var areasView = (viewController as AreasViewController);

                    Platform.AddVideo(areasView.View);
                    Platform.AddCover(areasView.View);

                    areasView.UnHideViews();
                    PresentViewController(viewController, false, null);
                    //Platform.ShowAlert(null, "You must join an area to complete setup!");
                }

            }
        }

        void UpdateView()
        {
            var profile = SL.Profile;
            if (profile != null)
            {
                CheckNetworks();
                ScoreImage.Image = UIImage.FromBundle("ob-score-bg");
            }
            else
            {
                ScoreFill.Image = null;
                ScoreImage.Image = UIImage.FromBundle("SLCircleLogo");
            }

            if (SL.IsNetworkConnected("Facebook"))
            {
                FacebookConnectedImage.Image = UIImage.FromBundle("check-icon_green");
                FacebookLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_fb-connected"), UIControlState.Normal);
            }
            else
            {
                FacebookConnectedImage.Image = null;
                FacebookLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_fb-unconnected"), UIControlState.Normal);
            }

            if (SL.IsNetworkConnected("Twitter"))
            {
                TwitterConnectedImage.Image = UIImage.FromBundle("check-icon_green");
                TwitterLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_twitter-connected"), UIControlState.Normal);
            }
            else
            {
                TwitterConnectedImage.Image = null;
                TwitterLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_twitter-unconnected"), UIControlState.Normal);
            }

            if (SL.IsNetworkConnected("Instagram"))
            {
                InstagramConnectedImage.Image = UIImage.FromBundle("check-icon_green");
                InstagramLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_insta-connected"), UIControlState.Normal);
            }
            else
            {
                InstagramConnectedImage.Image = null;
                InstagramLoginButton.SetBackgroundImage(UIImage.FromBundle("social-connect_insta-unconnected"), UIControlState.Normal);
            }

            NextButton.Enabled = SL.HasNetworks;
            LogoutButton.Hidden = SL.NetworkList == null || SL.NetworkList.Count == 0;
        }

        void CheckNetworks()
        {
            if (SL.NetworkList == null || SL.NetworkList.Count == 0)
            {
                ScoreFill.Image = UIImage.FromBundle("SLCircleLogo");
                return;
            }
            if (SL.NetworkList.Count == 1)
            {
                ScoreFill.Image = UIImage.FromBundle("on-board-33");
            }
            if (SL.NetworkList.Count == 2)
            {
                ScoreFill.Image = UIImage.FromBundle("on-board-66");
            }
            if (SL.NetworkList.Count == 3)
            {
                ScoreFill.Image = UIImage.FromBundle("on-board-100");
            }
        }

        partial void NextButton_TouchUpInside(UIButton sender)
        {
            if (SL.HasNetworks)
                LoadAreas();
        }

        partial void LogoutButton_TouchUpInside(UIButton sender)
        {
            SL.Logout();
            Platform.ClearBrowserCache();
            UpdateView();
        }
    }
}

