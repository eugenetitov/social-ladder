using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Camera
{
    public class CameraPicture
    {
        public UIImage Image { get; set; }
        public string FileName { get; set; }
        public NSData Data { get; set; }
        public string Description { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }
}
