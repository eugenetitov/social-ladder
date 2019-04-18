using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using SocialLadder.Droid.Helpers;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.CustomListeners
{
    [BroadcastReceiver]
    public class InstaShareIntentReceiver : BroadcastReceiver
    {
        public InstaShareIntentReceiver()
        {

        }

        public override void OnReceive(Context context, Intent intent)
        {
            var shareActivity = intent.Extras.Get(Intent.ExtraChosenComponent);
            if (shareActivity != null)
            {
                if (shareActivity is ComponentName && (shareActivity as ComponentName).PackageName == "com.instagram.android")
                {
                    try
                    {
                        NavigationHelper.NeedSubmitInstagramChallenge = true;
                        return;
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
                }
            }
            Mvx.Resolve<IAlertService>().ShowToast("Invalid selection. Please select Instagram.");
        }
    }
}