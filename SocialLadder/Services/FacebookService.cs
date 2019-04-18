using Newtonsoft.Json;
using SocialLadder.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace SocialLadder.Services
{
    public class FacebookService
    {
        public async Task<string> GetEmailAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email&access_token={accessToken}");
            var email = JsonConvert.DeserializeObject<FacebookModel>(json);
            return email.Email;
        }

        public async Task<SocialNetworkModel> GetSocialNetworkAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,id,name&access_token={accessToken}");
            SocialNetworkModel network = new SocialNetworkModel();
            var fbModel = JsonConvert.DeserializeObject<FacebookModel>(json);
            network.EmailAddress = fbModel.Email;
            network.NUserName = fbModel.Name;
            network.UserID = fbModel.Id;
            network.AccessToken = accessToken;
            network.NetworkName = "Facebook";
            return network;
        }
    }
}
