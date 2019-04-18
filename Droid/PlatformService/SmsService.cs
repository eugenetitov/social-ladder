using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.PlatformService
{
    public class SmsService : ISmsService
    {
        public bool SendSmsToNumbers(List<string> numbers, string message)
        {
            try
            {
                //SmsManager.Default.SendTextMessage("1234567890", null, "text", null, null);
                //var smsUri = Android.Net.Uri.Parse("smsto:" + number);
                var contacts = "smsto:";
                for(int i = 0; i<= numbers.Count-1; i++)
                {
                    contacts += numbers[i];
                    if (i < numbers.Count - 1)
                    {
                        contacts += ";";
                    }
                }
                var smsUri = Android.Net.Uri.Parse(contacts);
                
                var smsIntent = new Intent(Intent.ActionSendto, smsUri);
                smsIntent.SetFlags(ActivityFlags.NewTask);
                smsIntent.PutExtra("sms_body", message);
                Application.Context.StartActivity(smsIntent);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendSmsToWatsApp(string number, string message)
        {
            try
            {
                //Android.Net.Uri uri = Android.Net.Uri.Parse("smsto:" + number);
                //Intent i = new Intent(Intent.ActionSendto, uri);
                //i.SetPackage("com.whatsapp");
                //Application.Context.StartActivity(Intent.CreateChooser(i, ""));

                var link = "https://api.whatsapp.com/send?phone=" + number + "&text=" + message;
                Application.Context.StartActivity(new Intent(Intent.ActionView,
                            Android.Net.Uri.Parse(link)));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}