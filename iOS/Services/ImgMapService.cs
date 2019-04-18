using FFImageLoading;
using System.Collections.Generic;
using UIKit;

namespace SocialLadder.iOS.Services
{
    public static class ImgMapService
    {
        public static Dictionary<string, string> UrlToAsset { get; set; }

        public static void SetImage(string url, UIImageView imageView)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            if (UIImage.FromBundle(url) != null)
            {
                imageView.Image = UIImage.FromBundle(url);
            }
            else if (UrlToAsset.ContainsKey(url) && UIImage.FromBundle(UrlToAsset[url]) != null)
            {
                imageView.Image = UIImage.FromBundle(UrlToAsset[url]);
            }
            else
            {
                ImageService.Instance.LoadUrl(url).Into(imageView);
            }
        }

        static ImgMapService()
        {
            UrlToAsset = new Dictionary<string, string>();

            #region Hardcoded dictionary mapping url to assets

            UrlToAsset.Add("http://dev2.rkiapps.com/images/instagramchallengeicon.png", "camera-icon_off");
            UrlToAsset.Add("http://dev2.rkiapps.com/images/invitechallengeicon.png", "invite-icon_on");
            UrlToAsset.Add("http://dev2.rkiapps.com/images/mcchallengeicon.png", "quiz-icon_on");

            #endregion  Hardcoded dictionary mapping url to assets
        }
    }
}
