using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models
{
    public class RpcCallModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id; //seems like hardcoded value for RPC call;
        [JsonProperty(PropertyName = "method")]
        public string Method;
        [JsonProperty(PropertyName = "params")]
        public Dictionary<string,string> Params;
    }

    public class RpcNotifyCallModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id; //seems like hardcoded value for RPC call;
        [JsonProperty(PropertyName = "method")]
        public string Method;
        [JsonProperty(PropertyName = "params")]
        public Dictionary<string, string> Params;
    }

    public class RpcResponceModel
    {
        public string id;
        public string result; 
    }
}
