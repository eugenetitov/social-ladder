using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IEncodingService
    {
        String EncodeToNonLossyAscii(String original);
        String DecodeFromNonLossyAscii(String original);
    }
}
