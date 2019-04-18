using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Foundation;
using SocialLadder.Interfaces;
using UIKit;

namespace SocialLadder.iOS.Services
{
    public class AlertService : IAlertService
    {
        public void ShowToast(string title, string message)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alert = new UIAlertView()
                {
                    Title = title,
                    Message = message
                };
                alert.AddButton("Ok");
                alert.Show();
            });
        }

        public bool ShowOkCancelMessage(string title, string message, Action onOk, Action onCancel, bool showNegativebutton = true)
        {
            var result = false;
            var alert = new UIAlertView()
            {
                Title = title,
                Message = message

            };
            alert.AddButton("Ok");
            if (showNegativebutton)
            {
                alert.AddButton("Cancel");
            }
            alert.Clicked += (object s, UIButtonEventArgs e) =>
            {
                if (e.ButtonIndex == 0)
                {
                    onOk?.Invoke();
                }
                if (e.ButtonIndex == 1)
                {
                    onCancel?.Invoke();
                }
            };
            alert.Show();
            return result;
        }

        public void ShowErrorMessage(Exception exception)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                try
                {
                    var alert = new UIAlertView()
                    {
                        Message = exception.Message
                    };
                    alert.AddButton("Ok");
                    alert.Show();
                }
                catch (Exception)
                {
                }
            });
        }

        public void ShowSingle(string message)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                try
                {
                    var alert = new UIAlertView()
                    {
                        Message = message
                    };
                    alert.AddButton("Ok");
                    alert.Show();
                }
                catch (Exception ex)
                {
                }
            });
        }

        public void ShowSingleWithTimeout(string message, int seconds)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alert = new UIAlertView()
                {
                    Message = message,
                    Alpha = 1.0f
                };
                NSTimer tmr;
                alert.Show();

                tmr = NSTimer.CreateTimer(seconds, delegate
                {
                    alert.DismissWithClickedButtonIndex(0, true);
                    alert = null;
                });
                NSRunLoop.Main.AddTimer(tmr, NSRunLoopMode.Common);
            });
        }

        [DllImport("__Internal", EntryPoint = "exit")]
        public static extern void Exit(int status);
        public void ShowMessage(string title, string message)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alert = new UIAlertView()
                {
                    Title = title,
                    Message = message
                };
                alert.AddButton("Ok");
                alert.Show();
            });
        }

        public void ShowToast(string message)
        {
            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                var alert = new UIAlertView()
                {
                    Message = message,
                    Alpha = 1.0f
                };
                alert.Frame = new CoreGraphics.CGRect(alert.Frame.X, UIScreen.MainScreen.Bounds.Y * 0.7f, alert.Frame.Width, alert.Frame.Height);
                NSTimer tmr;
                alert.Show();

                tmr = NSTimer.CreateTimer(1.5, delegate
                {
                    alert.DismissWithClickedButtonIndex(0, true);
                    alert = null;
                });
                NSRunLoop.Main.AddTimer(tmr, NSRunLoopMode.Common);
            });
        }

        public Task<bool> ShowAlertConfirmation(string message, string buttonYesTitle = "Yes", string buttonNoTitle = "No")
        {
            var tcs = new TaskCompletionSource<bool>();

            UIApplication.SharedApplication.InvokeOnMainThread(new Action(() =>
            {
                var alert = new UIAlertView()
                {
                    Message = message
                };

                var yesIndex = alert.AddButton(buttonYesTitle);
                var noIndex = alert.AddButton(buttonNoTitle);

                alert.Clicked += (sender, buttonArgs) => tcs.SetResult(buttonArgs.ButtonIndex == yesIndex);
                alert.Show();
            }));

            return tcs.Task;
        }

        public void ShowAlertMessage(string message, ConfigurableAlertAction okAnsver, ConfigurableAlertAction dismissAnswer, ConfigurableAlertAction neutralAnsver)
        {
            throw new NotImplementedException();
        }

        public void ShowAlertMessage(string message, List<ConfigurableAlertAction> actions)
        {
            throw new NotImplementedException();
        }

        public Task<string> ShowAlertWithInput(string title, string message = "", string text = "", string buttonYesTitle = "Yes", string buttonNoTitle = "No")
        {
            throw new NotImplementedException();
        }
    }
}