using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.iOS.Services;
using UIKit;

namespace SocialLadder.iOS.Helpers
{
    class NativeFileLogger
    {
        static NativeFileLogger _current;

        public event EventHandler<FileChangedArgs> OnFileChanged;

        public static NativeFileLogger Current
        {
            get
            {
                if (_current == null)
                    _current = new NativeFileLogger();
                return _current;
            }
        }

        public DateTime LastModifiedDate(string path)
        {
            return File.GetLastWriteTime(path);
        }

        public string CachePath(string filename = "")
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
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.CachePath({filename}): {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public bool DeleteFile(string path)
        {
            try
            {
                NSError error;
                var result = NSFileManager.DefaultManager.Remove(path, out error);
                if (!result)
                    System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.DeleteFile({path}): {error.ToString()}");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.DeleteFile({path}): {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public bool Exists(string path)
        {
            try
            {
                if (path == null)
                    return false;
                return NSFileManager.DefaultManager.FileExists(path);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.Exists({path}): {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public void SendChanged(object sender, FileChangedArgs arg)
        {
            try
            {
                UIDevice.CurrentDevice.InvokeOnMainThread(() => {
                    OnFileChanged?.Invoke(sender, arg);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.SendChanged({sender?.GetType()?.Name}, {arg}): {ex.Message}\n{ex.StackTrace}");
            }
        }

        public string PrivatePath(string filename = "")
        {
            try
            {
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (string.IsNullOrEmpty(filename))
                    return directory;
                var path = System.IO.Path.Combine(directory, filename);
                return path;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.PersonalPath({filename}): {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public string PublicPath(string filename = "")
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
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.PublicPath({filename}): {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public bool WriteBytes(string path, IEnumerable<byte> bytes)
        {
            try
            {
                System.IO.File.WriteAllBytes(path, bytes.ToArray());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.WriteFile(path: {path}, bytes): {ex.Message}\n{ex.StackTrace}");
                return false;
            }
            return true;
        }

        public byte[] ReadBytes(string path)
        {
            try
            {
                return System.IO.File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {this?.GetType()?.Name}.ReadBytes({path}): {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public string FinishLaunchingPath
        {
            get
            {
                return PrivatePath($"FinishLaunching{SL.DeviceUUID}.txt");
            }
        }

        public string ProcessNotification
        {
            get
            {
                return PrivatePath($"ProcessNotification{SL.DeviceUUID}.txt");
            }
        }



        public string GetImagePath(string name = "", string prefix = "", string suffix = "")
        {
            return PrivatePath($"{prefix}{name}{suffix}.png");
        }

        public void WriteAllText(string path, string contents)
        {
            try
            {
                System.IO.File.WriteAllText(path, contents);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.WriteAllText({path}, {contents?.Length}): {ex.Message}\n{ex.StackTrace}");
            }
        }

        public string ReadLine(string path, int skip)
        {
            try
            {
                return System.IO.File.ReadLines(path).Skip(skip).Take(1).FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.ReadLine({path}, skip: {skip}: {ex.Message}\n{ex.StackTrace}");
                return null;
            }
        }

        public bool WriteLine(string path, string content, bool create = true, bool append = true)
        {
            System.IO.StreamWriter writer = null;
            try
            {
                if (!create && !Exists(path))
                    return false;
                writer = new System.IO.StreamWriter(path, append);
                writer.WriteLine(content);
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"{this?.GetType()?.Name}.WriteLine({path}, content, {create}, {append}): {ex.Message}\n{ex.StackTrace}");
                writer?.Close();
                return false;
            }
        }

        public StreamReader OpenStreamReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
