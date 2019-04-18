using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels
{
    public class LocationModel
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public bool IsAvailable { get; set; }
    }
}
