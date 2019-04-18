using System;
using System.Threading;
using CoreGraphics;
using Facebook.CoreKit;
using Facebook.LoginKit;
using Facebook.ShareKit;
using FFImageLoading;
using Foundation;
using SocialLadder.Interfaces;
using SocialLadder.iOS.Delegates;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Services
{
    public class IOSFacebookService : IFacebookService
    {
        private int Iterate;

        public async void ShareFacebookChallenge(object source, ChallengeModel model, string message = null)
        {
            ShareTemplateModel shareTemplate = null;
            ShareResponseModel shareResponse = null;
            await SL.Manager.RefreshShareTemplate(model.ShareTemplateURL, (response) =>
            {
                shareTemplate = response?.ShareTemplate;
                shareResponse = response;
            });

            NSUrl url = new NSUrl(shareTemplate?.PostHref ?? model.ShareImage);

            ShareDialog dialog = new ShareDialog();
            dialog.SetShouldFailOnDataError(true);
            dialog.FromViewController = source as UIViewController;
            dialog.SetDelegate(source as ISharingDelegate ?? new FBSharingDelegate());

            if (string.IsNullOrEmpty(message) && model.FBShareType == "image")
            {
                UIImageView imageView = new UIImageView();
                try
                {
                    await ImageService.Instance.LoadUrl(model.ShareImage).IntoAsync(imageView);
                }
                catch (Exception)
                {
                    try
                    {
                        await ImageService.Instance.LoadUrl(model.Image).IntoAsync(imageView);
                    }
                    catch (Exception)
                    {
                    }
                }

                dialog.SetShareContent(new SharePhotoContent() { Photos = new SharePhoto[] { SharePhoto.From(imageView.Image, true) } });
                dialog.Mode = ShareDialogMode.Native;
            }
            else if (string.IsNullOrEmpty(message) && (model.FBShareType == "link" || model.FBShareType == null))
            {
                ShareLinkContent contentLink = new ShareLinkContent();
                if (string.IsNullOrEmpty(url?.AbsoluteString))
                {
                    new UIAlertView("Sharing Error", string.IsNullOrEmpty(shareResponse?.ResponseMessage) ? "Sorry. No url come from server" : shareResponse.ResponseMessage, source as IUIAlertViewDelegate, "Ok").Show();
                    return;
                }
                contentLink.SetContentUrl(url);

                dialog.SetShareContent(contentLink);
                dialog.Mode = ShareDialogMode.Web;
            }
            else
            {
                SendOpenGraph(source, model, message);
                return;
            }

            if (!dialog.CanShow())
            {
                SendOpenGraph(source, model, message);
                return;
            }
            dialog.Show();

            if (dialog.Mode == ShareDialogMode.Web)
                StylishFbWebDialog();
        }

        public async void SendOpenGraph(object source, ChallengeModel model, string message = null)
        {
            ShareTemplateModel shareTemplate = null;
            ShareResponseModel shareResponse = null;

            await SL.Manager.RefreshShareTemplate(model.ShareTemplateURL, (response) =>
            {
                shareTemplate = response?.ShareTemplate;
                shareResponse = response;
            });

            NSUrl url = new NSUrl(shareTemplate?.PostHref ?? model.ShareImage);

            if (string.IsNullOrEmpty(url?.AbsoluteString))
            {
                new UIAlertView("Sharing Error", string.IsNullOrEmpty(shareResponse?.ResponseMessage) ? "Sorry. No url come from server" : shareResponse.ResponseMessage, source as IUIAlertViewDelegate, "Ok").Show();
                return;
            }

            ShareDialog dialog = new ShareDialog();
            dialog.SetShouldFailOnDataError(true);
            dialog.FromViewController = source as UIViewController;
            dialog.SetDelegate(source as ISharingDelegate ?? new FBSharingDelegate());

            NSString[] keys;
            NSObject[] objects;

            string imgUrl = null;

            try
            {
                if (!string.IsNullOrEmpty(model.ShareImage))
                {
                    await ImageService.Instance.LoadUrl(model.ShareImage).IntoAsync(new UIImageView());
                    imgUrl = model.ShareImage;
                }
            }
            catch (Exception) { }

            if (string.IsNullOrEmpty(imgUrl))
            {
                keys = new NSString[]{
                        new NSString("og:type"),
                        new NSString("og:url"),
                        new NSString("og:title"),
                        new NSString("og:description"),
                };
                objects = new NSObject[]{
                        NSObject.FromObject("article"),
                        NSObject.FromObject(url),
                        NSObject.FromObject(string.IsNullOrEmpty(shareTemplate?.PostTitle) ? url.AbsoluteString : shareTemplate.PostTitle),
                        NSObject.FromObject(string.IsNullOrEmpty(message) ? shareTemplate?.PostDescription ?? " " : message),
                };
            }
            else
            {
                keys = new NSString[]{
                        new NSString("og:type"),
                        new NSString("og:url"),
                        new NSString("og:title"),
                        new NSString("og:description"),
                        new NSString("og:image"),
                };
                objects = new NSObject[]
                {
                        NSObject.FromObject("article"),
                        NSObject.FromObject(url),
                        NSObject.FromObject(string.IsNullOrEmpty(shareTemplate?.PostTitle) ? url.AbsoluteString : shareTemplate.PostTitle),
                        NSObject.FromObject(string.IsNullOrEmpty(message) ? shareTemplate?.PostDescription ?? " " : message),
                        NSObject.FromObject(imgUrl),
                };
            }

            var propesties = new NSDictionary<NSString, NSObject>(keys, objects);
            var openGraph = ShareOpenGraphObject.ObjectWithProperties(propesties);
            var action = ShareOpenGraphAction.Action("news.publishes", openGraph, "article");
            var contentOpenGraph = new ShareOpenGraphContent
            {
                Action = action,
                PreviewPropertyName = "article"
            };

            dialog.SetShareContent(contentOpenGraph);
            dialog.Mode = ShareDialogMode.Web;

            dialog.Show();

            if (dialog.Mode == ShareDialogMode.Web)
            {
                StylishFbWebDialog();
            }
        }

        private void StylishFbWebDialog()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var overlay = Platform.AddOverlay(UIColor.White);
            window.AddSubview(overlay);

            nfloat s = overlay.Frame.Width * 0.2f;
            var progress = new UIImageView(new CGRect((overlay.Frame.Width - s) / 2, (overlay.Frame.Height - s) / 2, s, s)) { Image = UIImage.FromBundle("loading-indicator") };
            overlay.AddSubview(progress);
            Platform.AnimateRotation(progress);

            new System.Threading.Tasks.Task(() =>
            {
                Thread.Sleep(1000);
                window.InvokeOnMainThread(() => overlay.RemoveFromSuperview());
            }).Start();

            new System.Threading.Tasks.Task(() =>
                MakeWebDialogFullscreen(window)).Start();
        }

        private void MakeWebDialogFullscreen(UIWindow window)
        {
            Thread.Sleep(200);
            window.InvokeOnMainThread(() =>
            {
                foreach (var view in window.Subviews)
                {
                    if (view.Class.Name == "FBSDKWebDialogView")
                    {
                        view.TranslatesAutoresizingMaskIntoConstraints = false;
                        var frame = window?.SafeAreaLayoutGuide?.LayoutFrame ?? UIApplication.SharedApplication.KeyWindow.Frame;
                        int margin = 10;

                        view.Frame = new CGRect(frame.X - margin, frame.Y - margin, frame.Width + margin * 2, frame.Height + margin * 2);

                        foreach (var subview in view.Subviews)
                            if (subview is UIButton)
                                subview.Hidden = true;
                    }
                }
            });

            if (Iterate++ > 20)
                return;
            new System.Threading.Tasks.Task(() => MakeWebDialogFullscreen(window)).Start();
        }

        public void VerifyPermissions(string[] permissions, Action success, Action fail)
        {
            if (String.IsNullOrEmpty(permissions[0]) && !AccessToken.CurrentAccessToken.HasGranted(permissions[0]))
            {
                LoginManager login = new LoginManager();
                login.LogInWithReadPermissions(permissions, null,
                    (LoginManagerLoginResult result, NSError error) =>
                    {
                        if (result?.GrantedPermissions != null && result.GrantedPermissions.Contains(permissions[0]))
                        {
                            success();
                        }
                        else
                        {
                            fail();
                        }
                    }
                );
            }
            else
            {
                success();
            }
        }
    }
}