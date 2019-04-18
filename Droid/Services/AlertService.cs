using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Support.V7.AppCompat;
using SocialLadder.Droid.CustomListeners;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.Services
{

    public class AlertService : IAlertService
    {
        public static Activity Activity
        {
            get; set;
        }

        public void ShowAlertMessage(string message, List<ConfigurableAlertAction> actions)
        { 
            Activity.RunOnUiThread(() =>
            {
                var titles = actions.Select(x => x.Text).ToArray(); 
                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                builder.SetTitle(string.IsNullOrEmpty(message) ? "Report This" : message);

                builder.SetItems(titles, (s, event_args) =>
                {
                    actions[event_args.Which]?.OnClickHandler.Invoke(); 
                });
                var dialog = builder.Create();
                dialog.Show(); 
            });
        }

        public void ShowAlertMessage(string message, ConfigurableAlertAction okAnsver, ConfigurableAlertAction dismissAnswer, ConfigurableAlertAction neutralAnsver)
        {
            Activity.RunOnUiThread(() =>
            {
                var alert = new Android.App.AlertDialog.Builder(Activity);

                alert.SetMessage(message);
                alert.SetPositiveButton(okAnsver.Text, (senderAlert, args) => { okAnsver.OnClickHandler?.Invoke(); });
                alert.SetNegativeButton(dismissAnswer.Text, (senderAlert, args) => { dismissAnswer.OnClickHandler?.Invoke(); });
                alert.SetNeutralButton(neutralAnsver.Text, (senderAlert, args) => { neutralAnsver.OnClickHandler?.Invoke(); });

                Dialog dialog = alert.Create();
                dialog.Show();
            });
        }

        public bool ShowAlertConfirmation(string message)
        {

            bool result = false;
            Activity.RunOnUiThread(() =>
            {
                var alert = new Android.App.AlertDialog.Builder(Activity);
                alert.SetMessage(message);
                alert.SetPositiveButton("Delete", (senderAlert, args) => { result = true; });
                alert.SetNegativeButton("Cancel", (senderAlert, args) => { result = false; });

                Dialog dialog = alert.Create();
                dialog.ShowEvent += (s, e) =>
                {
                    result = true;
                };

                dialog.CancelEvent += (s, e) =>
                {
                    result = false;
                };
                dialog.Show();
            });
            return result;
        }

        public void ShowMessage(string title, string message)
        {

            Activity.RunOnUiThread(() =>
            {
                var alert = new Android.App.AlertDialog.Builder(Activity);
                alert.SetTitle(title);
                alert.SetMessage(message);
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                Dialog dialog = alert.Create();
                dialog.Show();
            });

        }

        public void ShowErrorMessage(Exception ex)
        {
            Activity.RunOnUiThread(() =>
            {
                var alert = new Android.App.AlertDialog.Builder(Activity);
                alert.SetMessage(ex.Message);
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                Dialog dialog = alert.Create();
                dialog.Show();
            });
        }

        public bool ShowOkCancelMessage(string title, string message, Action onOk, Action onCancel, bool showNegativeButton = true)
        {
            bool result = false;
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(Activity);
            alert.SetTitle(title);
            alert.SetMessage(message);
            alert.SetPositiveButton("Ok", (senderAlert, args) => onOk?.Invoke());
            if (showNegativeButton)
            {
                alert.SetNegativeButton("Cancel", (senderAlert, args) => onCancel?.Invoke());
            }          
            Dialog dialog = alert.Create();
            dialog.Show();
            return result;
        }





        public void ShowSingle(string message)
        {
            Activity.RunOnUiThread(() =>
            {
                var alert = new Android.App.AlertDialog.Builder(Activity);
                alert.SetMessage(message);
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                Dialog dialog = alert.Create();
                dialog.Show();
            });
        }

        public void ShowSingle(string message, Activity activity)
        {
            Activity.RunOnUiThread(() =>
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(activity);
                alert.SetMessage(message);
                alert.SetPositiveButton("Ok", (senderAlert, args) => { });
                Dialog dialog = alert.Create();
                dialog.Show();
            });

        }

        public void ShowToast(string message, Activity activity)
        {
            Activity.RunOnUiThread(() =>
            {
                Toast.MakeText(activity, message, ToastLength.Long).Show();
            });
        }

        public void ShowToast(string message)
        {
            Activity.RunOnUiThread(() =>
            {
                Toast.MakeText(Activity, message, ToastLength.Long).Show();
            });
        }

        public Task<bool> ShowAlertConfirmation(string message, string buttonYesTitle = "Yes", string buttonNoTitle = "No")
        {
            var tcs = new TaskCompletionSource<bool>();

            Activity.RunOnUiThread(() =>
            {
                var alert = new Android.App.AlertDialog.Builder(Activity);
                alert.SetMessage(message);
                alert.SetPositiveButton(buttonYesTitle, (senderAlert, args) =>
                {
                    tcs.SetResult(true);
                });
                alert.SetNegativeButton(buttonNoTitle, (senderAlert, args) =>
                {
                    tcs.SetResult(false);
                });
                alert.SetOnCancelListener(new OnCancelListener(() =>
                {
                    tcs.SetResult(false);
                }));
                Dialog dialog = alert.Create();

                dialog.Show();
            });

            return tcs.Task;
        }

        private class OnCancelListener :  Java.Lang.Object, IDialogInterfaceOnCancelListener
        {
            private Action _onCancel;

            public OnCancelListener(Action onCancel)
            {
                _onCancel = onCancel;
            }

            public void OnCancel(IDialogInterface dialog)
            {
                _onCancel?.Invoke();
            }
        }



        public Task<string> ShowAlertWithInput(string title, string message = "", string text = "", string buttonYesTitle = "Yes", string buttonNoTitle = "No")
        {
            var tcs = new TaskCompletionSource<string>();

            Activity.RunOnUiThread(() =>
            {
                var alert = new Android.App.AlertDialog.Builder(Activity);
                EditText userInput = new EditText(Activity);
                userInput.Text = text;
                alert.SetView(userInput);
                alert.SetMessage(message);
                alert.SetTitle(title);
                alert.SetOnCancelListener(new OnCancelListener(() =>
                {
                    tcs.SetResult(string.Empty);
                }));

                alert.NothingSelected += (senderAlert, args) =>
                {
                    tcs.SetResult(string.Empty);
                };
                alert.SetPositiveButton(buttonYesTitle, (senderAlert, args) =>
                {
                    tcs.SetResult(userInput.Text);
                });
                alert.SetNegativeButton(buttonNoTitle, (senderAlert, args) =>
                {
                    tcs.SetResult(string.Empty);
                });
                Dialog dialog = alert.Create();

                dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
                dialog.Show();
            });

            return tcs.Task;
        }
    }
        
}