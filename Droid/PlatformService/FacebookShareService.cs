using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Org.Json;
using SocialLadder.Converters;
using SocialLadder.Droid.Activities.Main;
using SocialLadder.Enums;
using SocialLadder.Interfaces;
using SocialLadder.Models;
using SocialLadder.Models.LocalModels.Challenges;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Share.Model;
using Xamarin.Facebook.Share.Widget;

namespace SocialLadder.Droid.PlatformService
{
    public class FacebookShareService : Java.Lang.Object, GraphRequest.ICallback, IFacebookCallback, IFacebookShareService
    {
        Action<ChallengesFacebookShareResponseType> ViewModelResponse { get; set; }
        private object Param { get; set; }
        ShareTemplateModel ShareTemplate { get; set; }
        ChallengeModel Model { get; set; }
        Activity Activity { get; set; }
        private bool OpenGraphWasTry { get; set; } = false;

        #region Permissions
        public void VerifyPermissions(string[] permissions = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null, object param = null)
        {
            //var token = AccessToken.CurrentAccessToken;
            ViewModelResponse = viewModelResponse;
            Param = param;
            if (Xamarin.Facebook.AccessToken.CurrentAccessToken != null)
            {
                var request = new GraphRequest(AccessToken.CurrentAccessToken, "/me/permissions", null, HttpMethod.Get, this);
                request.ExecuteAsync();
            }
        }

        public void OnCompleted(GraphResponse response)
        {
            try
            {
                List<string> unGrantedPermissions = new List<string>();
                var array = response.JSONObject.OptJSONArray("data");
                for (int i = 0; i < array.Length(); i++)
                {
                    JSONObject actor = array.GetJSONObject(i);
                    string name = actor.GetString("permission");
                    if (actor.GetString("status") != "granted")
                    {
                        unGrantedPermissions.Add(name);
                    }
                }
                if (unGrantedPermissions.Count > 0)
                {
                    LoginManager loginManager = LoginManager.Instance;
                    loginManager.RegisterCallback(null, this);
                    loginManager.LogInWithReadPermissions(Param as Activity, unGrantedPermissions);
                    return;
                }
                UpdateAccessToken();
                ViewModelResponse(ChallengesFacebookShareResponseType.Successed);
            }
            catch (Java.Lang.Exception ex)
            {
                ViewModelResponse(ChallengesFacebookShareResponseType.Error);
            }
        }
        #endregion
        #region AccessToken
        async void UpdateAccessToken()
        {
            SocialNetworkModel network = SL.GetNetworkForName("Facebook");
            if (network == null)
            {
                Console.WriteLine("Unexpected: Facebook network model not found for logged in user");
                return;
            }
            string AccessTokenString = AccessToken.CurrentAccessToken?.Token;
            if (!string.IsNullOrEmpty(AccessTokenString) && network.AccessToken != AccessTokenString)
            {
                network.AccessToken = AccessTokenString;
                await SL.Manager.SaveNetworkAsync(network, true, (responseModel) => { Console.WriteLine("Access Token Update " + (responseModel.ResponseCode == 1 ? "Successful" : "Failed")); });
                Console.WriteLine("End Updating Facebook Access Token");
                return;
            }
            Console.WriteLine("Unexpected: Facebook AccessToken is null for logged in user");
        }

        public void SetAccessToken(string accessToken, string userId, bool needRemove = false)
        {
            if (needRemove)
            {
                AccessToken.CurrentAccessToken = null;
                return;
            }
            AccessToken.CurrentAccessToken = new AccessToken(accessToken, Configuration.FbClientId, userId, null, null, null, null, null, null);
        }
        #endregion
        #region Sharing
        public async Task ShareFacebookChallenge(object source, ChallengeModel model, string message = null, byte[] data = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null)
        {
            OpenGraphWasTry = false;
            ViewModelResponse = viewModelResponse;
            Activity = source as Activity;
            Model = model;
            ShareTemplateModel shareTemplate = null;
            ShareResponseModel shareResponse = null;
            await SL.Manager.RefreshShareTemplate(Model.ShareTemplateURL, (response) =>
            {
                shareTemplate = response?.ShareTemplate;
                shareResponse = response;
            });
            try
            {
                ShareDialog dialog = new ShareDialog(Activity);
                dialog.RegisterCallback((Activity as MainActivity).CallBackManager, this);
                if (string.IsNullOrEmpty(message) && Model.FBShareType == "image")
                {
                    if (data == null && !string.IsNullOrEmpty(Model.Image))
                    {
                        data = await ImageUrlToByteArrayLocalConverter.ReadImageUrlToBytesArray(Model.Image);
                    }
                    if (data == null)
                    {
                        return;
                    }
                    var bitmapImage = await BitmapFactory.DecodeByteArrayAsync(data, 0, data.Length);
                    var photoBuilder = new SharePhoto.Builder();
                    photoBuilder.SetUserGenerated(true);
                    var sharePhoto = photoBuilder.SetBitmap(bitmapImage);
                    SharePhotoContent content = new SharePhotoContent.Builder().AddPhoto(sharePhoto.Build()).Build();
                    if (dialog.CanShow(content, ShareDialog.Mode.Web))
                    {
                        dialog.Show(content, ShareDialog.Mode.Web);
                        return;
                    }
                }
                if (string.IsNullOrEmpty(message) && (Model.FBShareType == null || Model.FBShareType == "link"))
                {
                    var uri = Android.Net.Uri.Parse(shareTemplate?.PostHref ?? Model.ShareImage);

                    var linkBuilder = new ShareLinkContent.Builder();
                    linkBuilder.SetContentUrl(Android.Net.Uri.Parse(shareTemplate?.PostHref ?? Model.ShareImage));
                    ShareLinkContent content = linkBuilder.Build();

                    if (dialog.CanShow(content, ShareDialog.Mode.Web))
                    {
                        dialog.Show(content, ShareDialog.Mode.Web);
                        return;
                    }
                }

                ViewModelResponse(ChallengesFacebookShareResponseType.NativeUninstallApp);
            }
            catch (System.Exception ex)
            {
                await SendOpenGraph(Activity, Model, message, null, null, shareTemplate, shareResponse);
            }
            return;
        }

        public async Task SendOpenGraph(object source, ChallengeModel model, string message = null, byte[] data = null, Action<ChallengesFacebookShareResponseType> viewModelResponse = null, ShareTemplateModel shareTemplate = null, ShareResponseModel shareResponse = null)
        {
            ViewModelResponse = viewModelResponse;
            ShareDialog dialog = new ShareDialog(source as Activity);
            dialog.RegisterCallback((source as MainActivity).CallBackManager, this);
            if (shareTemplate == null || shareResponse == null)
            {
                await SL.Manager.RefreshShareTemplate(model.ShareTemplateURL, (response) =>
                {
                    ShareTemplate = response?.ShareTemplate;
                    shareResponse = response;
                });
            }
            else
            {
                ShareTemplate = shareTemplate;
            }

            OpenGraphWasTry = true;
            var openGraphBuilder = new ShareOpenGraphObject.Builder();
            openGraphBuilder.PutString("og:type", "object");
            openGraphBuilder.PutString("og:title", string.IsNullOrEmpty(ShareTemplate?.PostTitle) ? ShareTemplate?.PostHref : ShareTemplate.PostTitle);
            openGraphBuilder.PutString("og:description", string.IsNullOrEmpty(message) ? ShareTemplate?.PostDescription ?? " " : message);
            //openGraphBuilder.PutString("og:url", ShareTemplate?.PostHref ?? model.ShareImage);
            if (model != null && (model.FBShareType == "image" || !string.IsNullOrEmpty(model.ShareImage)))
            {
                openGraphBuilder.PutString("og:image", model.ShareImage);
            }
            if (model != null && (model.FBShareType == "link" || model.FBShareType == null))
            {
                openGraphBuilder.PutString("og:url", ShareTemplate?.PostHref ?? model.ShareImage);
            }
            ShareOpenGraphObject openGraph = openGraphBuilder.Build();
            ShareOpenGraphAction action = new ShareOpenGraphAction.Builder()
                    .SetActionType("news.publishes")
                    .PutObject("object", openGraph)
                    .JavaCast<ShareOpenGraphAction.Builder>()
                    .Build();
            ShareOpenGraphContent contentOpenGraph = new ShareOpenGraphContent.Builder()
                    .SetPreviewPropertyName("object")
                    .SetAction(action)
                    .Build();

            dialog.Show(contentOpenGraph, ShareDialog.Mode.Web);
            //ShareDialog.Show(source as Activity, contentOpenGraph);
        }
        #endregion
        #region Callback
        public void OnCancel()
        {
            ViewModelResponse(ChallengesFacebookShareResponseType.Canceled);
        }

        public async void OnError(FacebookException error)
        {
            try
            {
                if (!OpenGraphWasTry && Activity!= null && Model != null)
                {
                    await SendOpenGraph(Activity, Model);
                    return;
                }
            }
            catch { }
            ViewModelResponse(ChallengesFacebookShareResponseType.Error);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            ViewModelResponse(ChallengesFacebookShareResponseType.Successed);
        }
        #endregion
    }
}