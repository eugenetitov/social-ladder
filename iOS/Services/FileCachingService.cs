using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Services
{
    public class FileLoadedArgs : EventArgs
    {
        public string ImageUrl { get; private set; }
        public UIImage Image { get; private set; }

        public FileLoadedArgs(string imageUrl, UIImage image)
        {
            ImageUrl = imageUrl;
            Image = image;
        }
    }

    public class FileChangedArgs : EventArgs
    {
        public LocalFile LocalFile { get; private set; }

        public FileChangedArgs(LocalFile file)
        {
            LocalFile = file;
        }
    }

    public static class FileCachingService
    {
        static List<string> _downloadStack = new List<string>();
        static bool isDownloading = false;
        static object locker = new object();
        public static event EventHandler<FileChangedArgs> OnFileChanged;
        public static event EventHandler<FileLoadedArgs> OnImageLoaded;

        public static LocalFile GetFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;
            try
            {
                var file = SL.GetItems<LocalFile>(x => x.Url == url).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(file?.Path) || !Exists(file.Path))
                {
                    Task.Run(async () =>
                    {
                        await DownloadFile(url);
                    });
                    return null;
                }
                else
                {
                    return file;
                }
            }
            catch (Exception ex)
            {
                SL.Report("Error GetFile()", ex);
                return null;
            }
        }

        public static async Task DownloadFile(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || _downloadStack.Contains(url))
                return;
            _downloadStack.Add(url);
            if (_downloadStack.Count == 1)
                await DownloadNext();
        }

        static async Task DownloadNext()
        {
            if (isDownloading || _downloadStack.Count == 0)
                return;
            isDownloading = true;
            var url = _downloadStack[0];

            try
            {
                var files = SL.GetItems<LocalFile>(x => x.Url == url);
                foreach (var file in files)
                {
                    try
                    {
                        if (Exists(file.Path))
                            DeleteFile(file.Path);
                        SL.DeleteItem(file);
                    }
                    catch (Exception) { }
                }
                var name = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(url);
                var filename = name + extension;
                var path = CachePath(filename);
                var pp = PublicPath(filename);
                await DownloadAsync(url, path);
                var localFile = new LocalFile { ID = name, Url = url, Path = path };
                var result = SL.SaveItem(localFile, true, true) != null;
                _downloadStack.Remove(url);
                isDownloading = false;

                DownloadNext();

                if (result)
                {
                    SendChanged(new Object(), new FileChangedArgs(localFile));
                }

                return;
            }
            catch (Exception ex)
            {
                SL.Report("Error DownloadFile()", ex);
                _downloadStack.Remove(url);
                isDownloading = false;
                DownloadNext();
                return;
            }
        }

        #region localFileManagement
        static public void SendChanged(object sender, FileChangedArgs arg)
        {
            try
            {
                UIDevice.CurrentDevice.InvokeOnMainThread(() =>
                {
                    OnFileChanged?.Invoke(sender, arg);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {typeof(FileCachingService)}.SendChanged({sender?.GetType()?.Name}, {arg}): {ex.Message}\n{ex.StackTrace}");
            }
        }

        static public bool Exists(string path)
        {
            try
            {
                return NSFileManager.DefaultManager.FileExists(path);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {typeof(FileCachingService)}.Exists({path}): {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        static public bool DeleteFile(string path)
        {
            try
            {
                NSError error;
                var result = NSFileManager.DefaultManager.Remove(path, out error);
                if (!result)
                    System.Diagnostics.Debug.WriteLine($"EXCEPTION: {typeof(FileCachingService)}.DeleteFile({path}): {error.ToString()}");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {typeof(FileCachingService)}.DeleteFile({path}): {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        static public string CachePath(string filename = "")
        {
            try
            {
                var t = NSSearchPath.GetDirectories(NSSearchPathDirectory.CachesDirectory, NSSearchPathDomain.User, true);
                if (t == null || t.Length == 0)
                    return null;
                if (string.IsNullOrEmpty(filename))
                    return t[0];
                var path = (new NSString(t[0])).AppendPathComponent(new NSString(filename)).ToString();
                return path;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {typeof(FileCachingService)}.CachePath({filename}): {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        static public string PublicPath(string filename = "")
        {
            try
            {
                var t = NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, true);
                if (t == null || t.Length == 0)
                    return null;
                if (string.IsNullOrEmpty(filename))
                    return t[0];
                var path = (new NSString(t[0])).AppendPathComponent(new NSString(filename)).ToString();
                return path;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {typeof(FileCachingService)}.PublicPath({filename}): {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }
        #endregion

        #region downloadingFileFromUrl
        static public async Task DownloadAsync(string url, string path)
        {
                using (var client = new WebClient())
                {
                    client.DownloadFileCompleted += DownloadCompleted;
                    await client.DownloadFileTaskAsync(url, path);

                    return;
                }
        }

        static void OnLoadingFinished(IScheduledWork scheduledWork)
        {
            if (scheduledWork.IsCompleted)
            {

            }
        }

        static void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion

        public static void BeginDownloadImageFromUrl(string url, Action<FileLoadedArgs> downloadImageFromUrlCompleted = null)
        {
            Task.Run(async () =>
            {
                UIImage currentImage = await ImageService.Instance.LoadUrl(url).AsUIImageAsync();
                OnImageLoaded?.Invoke(new Object(), new FileLoadedArgs(url, currentImage));
                downloadImageFromUrlCompleted?.Invoke(new FileLoadedArgs(url, currentImage));
            });
        }

        public static void PreloadImagesToDiskFromUrl(List<string> urlList)
        {
            Task.Run(async() =>
            {
                UIImage currentImage = null;

                foreach (var url in urlList)
                {
                    currentImage = await ImageService.Instance.LoadUrl(url).AsUIImageAsync();
                }
            });
        }
    }
}