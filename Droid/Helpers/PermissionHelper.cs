using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.Helpers
{
    public static class PermissionHelper
    {
        public static void AddPermission(Activity context, List<string> permissions)
        {
            var listDeniedPermissions = new List<string>();
            foreach (var item in permissions)
            {
                if (ContextCompat.CheckSelfPermission(context, item) != Permission.Granted)
                {
                    listDeniedPermissions.Add(item);
                }
            }
            if (listDeniedPermissions.Count == 0)
            {
                Console.WriteLine("Permissions has already been granted");
                return;
            }
            if (listDeniedPermissions.Count > 0)
            {
                ActivityCompat.RequestPermissions(context, listDeniedPermissions.ToArray(), 0);
            }
        }

        public static bool CheckPermissions(Activity context, List<string> permissions)
        {
            bool permissionsGranted = true;
            foreach (var item in permissions)
            {
                if (ContextCompat.CheckSelfPermission(context, item) != Permission.Granted)
                {
                    permissionsGranted = false;
                }
            }
            return permissionsGranted;
        }
    }
}