using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Interfaces
{
    public interface IShareService
    {
        Task<bool> SharePhotoToInsta(string title, string message, string url, byte[] data = null);
        void CleanFile();
    }
}
