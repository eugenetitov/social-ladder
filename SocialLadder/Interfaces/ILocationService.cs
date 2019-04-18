using SocialLadder.Models.LocalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface ILocationService
    {
        //double Lat { get; }
        //double Long { get; }
        LocationModel CurrentLocation { get; set; }
        Task<LocationModel> GetCurrentLocation();
    }
}
