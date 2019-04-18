using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialLadder.Models;
using SocialLadder.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Services
{
    
    public class InstagramService
    {
        public async Task<SocialNetworkModel> GetSocialNetworkAsync(string token)
        {
            var httpClient = new HttpClient();

            try
            {
                SocialNetworkModel network = new SocialNetworkModel();
                var request = await httpClient.GetStringAsync(String.Format("https://api.instagram.com/v1/users/self/?access_token={0}", token));

                var response = JsonConvert.DeserializeObject<RootInstagramModel>(request);
            
                network.EmailAddress = null;
                network.NUserName = response.data.full_name;
                network.ID = response.data.id;

                return network;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
           
        }
    }
}
