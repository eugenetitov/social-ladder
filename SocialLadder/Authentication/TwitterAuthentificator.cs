using Newtonsoft.Json;
using SocialLadder.Models;
using SocialLadder.Services;
using SocialLadder.Services.ServiceModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace SocialLadder.Authentication
{
    public class TwitterAuthentificator
    {
        private const string RequestTokenUrl = "https://api.twitter.com/oauth/request_token";
        private const string AuthorizeUrl = "https://api.twitter.com/oauth/authorize";
        private const string AccessTokenUrl = "https://api.twitter.com/oauth/access_token";
        private const string CallbackUrl = "https://mobile.twitter.com/home";
        //private const string CallbackUrl = "http://mobile.twitter.com/";
        //private const string CallbackUrl = "https://mobile.twitter.com";

        private OAuth1Authenticator _auth;
        private ITwitterAuthenticationDelegate _authenticationDelegate;

        public TwitterAuthentificator(string consumerKey, string consumerSecret, string scope, ITwitterAuthenticationDelegate authenticationDelegate)
        {
            _authenticationDelegate = authenticationDelegate;
            _auth = new OAuth1Authenticator(
                consumerKey: consumerKey,
                consumerSecret: consumerSecret,
                requestTokenUrl: new Uri(RequestTokenUrl),
                authorizeUrl: new Uri(AuthorizeUrl),
                accessTokenUrl: new Uri(AccessTokenUrl),
                callbackUrl: new Uri(CallbackUrl));

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;
            _auth.AllowCancel = true;
        }

        public OAuth1Authenticator GetAuthenticator()
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
                var authToken = new TwitterOAuthToken()
                {
                    oauth_token = e.Account.Properties["oauth_token"],
                    oauth_token_secret = e.Account.Properties["oauth_token_secret"]
                };
                var network = new SocialNetworkModel()
                {
                    AccessToken = JsonConvert.SerializeObject(authToken),
                    UserID = e.Account.Properties["user_id"],
                    NUserName = e.Account.Properties["screen_name"],
                    NetworkName = "Twitter"
                };
                await _authenticationDelegate.OnTwitterAuthenticationCompleted(network);
            }
            else
            {
                _authenticationDelegate.OnTwitterAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticationDelegate.OnTwitterAuthenticationFailed(e.Message, e.Exception);
        }
    }

    public class TwitterOAuthToken
    {
        public string oauth_token
        {
            get; set;
        }
        public string oauth_token_secret
        {
            get; set;
        }
    }

    public interface ITwitterAuthenticationDelegate
    {
        Task OnTwitterAuthenticationCompleted(SocialNetworkModel network);
        void OnTwitterAuthenticationFailed(string message, Exception exception);
        void OnTwitterAuthenticationCanceled();
    }
}
