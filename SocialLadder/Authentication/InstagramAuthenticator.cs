using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialLadder.Authentication.InstagramModels;
using SocialLadder.Models;
using Xamarin.Auth;

namespace SocialLadder.Authentication
{
    public class InstagramAuthenticator
    {
        private const string RequestTokenUrl = "https://api.instagram.com/oauth/authorize";
        private const string AuthorizeUrl = "https://api.instagram.com/oauth/authorize";
        private const string AccessTokenUrl = "https://api.instagram.com/oauth/access_token";
        private const string CallbackUrl = "http://socialladderapp.com";
        //private const string CallbackUrl = "https://localhost:3000/callback";
        //private const string CallbackUrl = "https://socialladder.rkiapps.com/prospect.html";

        private OAuth2Authenticator _auth;
        private IInstagramAuthenticationDelegate _authenticationDelegate;

        public InstagramAuthenticator(string consumerKey, string consumerSecret, string scope, IInstagramAuthenticationDelegate authenticationDelegate)
        {
            _authenticationDelegate = authenticationDelegate;

            //_auth = new InstagramOAuth2Authenticator(
            //                clientId: consumerKey,
            //                scope: scope,
            //                authorizeUrl: new Uri(AuthorizeUrl),
            //                redirectUrl: new Uri(CallbackUrl),
            //                isUsingNativeUI: false);
            //_auth.DoNotEscapeScope = false;

            _auth = new OAuth2Authenticator(
                clientId: consumerKey,
                scope: scope,
                authorizeUrl: new Uri(AuthorizeUrl),
                redirectUrl: new Uri(CallbackUrl),
                getUsernameAsync: null,
                isUsingNativeUI: false);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;

            _auth.AllowCancel = true;
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _auth;
        }

        public void OnPageLoading(Uri uri)
        {
            _auth.OnPageLoading(uri);
        }

        private async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                string token = string.Empty;
                e.Account.Properties.TryGetValue("access_token", out token);
                var socialNetworkModel = await GetInstaNetworkModel(token);
                await _authenticationDelegate.OnInstagramAuthenticationCompleted(socialNetworkModel);
            }
            else
            {
                _authenticationDelegate.OnInstagramAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticationDelegate.OnInstagramAuthenticationFailed(e.Message, e.Exception);
        }

        private async Task<SocialNetworkModel> GetInstaNetworkModel(string token)
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            string responce = await client.GetStringAsync(string.Format("https://api.instagram.com/v1/users/self/?access_token={0}", token));

            var networkResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<InstagramUserResponce>(responce);
            var socialNetworkModel = new SocialNetworkModel();
            socialNetworkModel.AccessToken = token;
            socialNetworkModel.UserID = networkResponse?.data?.id;
            socialNetworkModel.NetworkName = "Instagram";
            return socialNetworkModel;
        }
    }


}
