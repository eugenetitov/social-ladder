using SocialLadder.Services;
using System;
using Xamarin.Auth;

namespace SocialLadder.Authentication
{
    public class FacebookAuthenticator
    {
        private const string AuthorizeUrl = "https://www.facebook.com/dialog/oauth/";
        private const string RedirectUrl = "https://socialladder.rkiapps.com/prospect.html";
        private const bool IsUsingNativeUI = false;

        private OAuth2Authenticator _auth;
        private IFacebookAuthenticationDelegate _authenticationDelegate;

        public FacebookAuthenticator(string clientId, string scope, IFacebookAuthenticationDelegate authenticationDelegate)
        {
            _authenticationDelegate = authenticationDelegate;

            _auth = new OAuth2Authenticator(clientId, scope,
                                            new Uri(AuthorizeUrl),
                                            new Uri(RedirectUrl),
                                            null, IsUsingNativeUI);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;
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
                var token = new FacebookOAuthToken
                {
                    AccessToken = e.Account.Properties["access_token"]
                };
                var facebookService = new FacebookService();
                var network = await facebookService.GetSocialNetworkAsync(token.AccessToken);

                await _authenticationDelegate.OnFacebookAuthenticationCompleted(network);
            }
            else
            {
                _authenticationDelegate.OnFacebookAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticationDelegate.OnFacebookAuthenticationFailed(e.Message, e.Exception);
        }
    }
}
