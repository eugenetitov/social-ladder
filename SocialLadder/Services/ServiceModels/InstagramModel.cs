using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Services.ServiceModels
{
    public class InstagramModel
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "full_name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
public class RootInstagramModel
{
    public Data data { get; set; }
    public Meta meta { get; set; }
}

public class Data
{
    public string id { get; set; }
    public string username { get; set; }
    public string profile_picture { get; set; }
    public string full_name { get; set; }
    public string bio { get; set; }
    public string website { get; set; }
    public bool is_business { get; set; }
    public Counts counts { get; set; }
}

public class Counts
{
    public int media { get; set; }
    public int follows { get; set; }
    public int followed_by { get; set; }
}

public class Meta
{
    public int code { get; set; }
}

