using Newtonsoft.Json;
using SocialLadder.Models;
using SocialLadder.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace SocialLadder.Services
{
    public class TwitterService
    {
        public async Task<SocialNetworkModel> GetSocialNetworkAsync(Account account)
        {
            try
            {
                var request = new OAuth1Request("GET", new Uri("https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true"), null, account);
                
                var response = await request.GetResponseAsync();
                var json = response.GetResponseText();

                TwitterModel twitterUser = JsonConvert.DeserializeObject<TwitterModel>(json);
                
                return new SocialNetworkModel()
                {
                    ID = twitterUser.Id,
                    NUserName = twitterUser.Name,
                    EmailAddress = twitterUser.Email,

                };

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
           
        }
    }
}
