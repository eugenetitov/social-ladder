using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels
{
    public class LocalWebDataModel
    {
        public string Data { get; set; }
        public string Url { get; set; }

        public LocalWebDataModel(string data = "", string url = "")
        {
            Data = data;
            Url = url;
        }
    }
}
