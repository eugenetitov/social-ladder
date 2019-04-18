using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;
using SocialLadder.Helpers;
using SocialLadder.Interfaces;
using static Android.Content.PM.PackageManager;

namespace SocialLadder.Droid.CustomListeners
{
    [BroadcastReceiver]
    public class InviteIntentReceiver : BroadcastReceiver
    {

        public InviteIntentReceiver()
        { }

        public override void OnReceive(Context context, Intent intent)
        {
            var shareActivity = intent.Extras.Get(Intent.ExtraChosenComponent);
            if (shareActivity != null)
            {
                if (shareActivity is ComponentName && (shareActivity as ComponentName).PackageName == "com.whatsapp")
                {
                    try
                    {
                        NavigationHelper.NeedSubmitInviteChallenge = true;
                        return;
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
                }
                else if (shareActivity is ComponentName && (shareActivity as ComponentName).PackageName == "com.google.android.apps.messaging")
                {
                    try
                    {
                        NavigationHelper.NeedSubmitInviteChallenge = true;
                        return;
                    }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
                }
                else if (shareActivity is ComponentName && (shareActivity as ComponentName).PackageName == "com.google.android.apps.docs")
                {
                    return;
                }
            }
            Mvx.Resolve<IAlertService>().ShowToast("Invalid selection. Please select WhatsApp or SMS.");
        }

        private bool AppInstalled(Context c, string targetPackage)
        {
            PackageManager pm = c.PackageManager;
            try
            {
                PackageInfo info = pm.GetPackageInfo(targetPackage, PackageInfoFlags.MetaData);
            }
            catch (NameNotFoundException e)
            {
                return false;
            }
            return true;
        }
    }
}