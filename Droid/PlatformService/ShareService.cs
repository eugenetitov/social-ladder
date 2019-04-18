using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Net;
using MvvmCross.Droid.Support.V4;
using SocialLadder.Droid.CustomListeners;
using SocialLadder.Droid.Models;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.PlatformService
{
    public class ShareService : IShareService
    {
        private readonly Context _context;
        public static Activity Activity { get; set; }
        private string filePath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "challenge.png");
        public ShareService()
        {
            _context = Application.Context;
        }

        public void CleanFile()
        {
            Java.IO.File file = new Java.IO.File(filePath);
            if (file != null)
            {
                file.Delete();
            }
        }

        public Task<bool> SharePhotoToInsta(string title, string message, string url, byte[] data = null)
        {
            var extension = url.Substring(url.LastIndexOf(".") + 1).ToLower();
            var contentType = string.Empty;

            switch (extension)
            {
                case "pdf":
                    contentType = "application/pdf";
                    break;
                case "jpg":
                    contentType = "image/jpg";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = "image/png";
                    break;
            }

            var intent = new Intent(Intent.ActionSend);
            intent.SetType(contentType);

            var filePath = ExportBitmapAsPNG(BitmapFactory.DecodeByteArray(data, 0, data.Length));
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filePath));

            intent.PutExtra(Intent.ExtraText, string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            Intent receiver = new Intent(_context, new InstaShareIntentReceiver().Class);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(_context, 0, receiver, PendingIntentFlags.UpdateCurrent);
            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty, pendingIntent.IntentSender);

            //var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);

            //Activity.StartActivityForResult(chooserIntent, ShareRequestType.InstagramRequestCode);
            _context.StartActivity(chooserIntent);
            return Task.FromResult(true);
        }

        private string ExportBitmapAsPNG(Bitmap bitmap)
        {
            var stream = new FileStream(filePath, FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
            return filePath;
        }
    }
}