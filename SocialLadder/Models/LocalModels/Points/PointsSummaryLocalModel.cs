using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Points
{
    public class PointsSummaryLocalModel
    {
        public string ImageName { get; set; }
        public float Progress { get; set; }
        public string UnlockedText { get; set; }
        public string PurchasedText { get; set; }    
    }
}
