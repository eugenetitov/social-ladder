using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.JobDispatcher;

namespace SocialLadder.Droid.PlatformService.Firebase
{
    [Service]
    [IntentFilter(new[] { "com.firebase.jobdispatcher.ACTION_EXECUTE" })]
    public class FirebaseJobService : JobService
    {
        public override bool OnStartJob(IJobParameters jobParameters)
        {
            return false;
        }

        public override bool OnStopJob(IJobParameters jobParameters)
        {
            return false;
        }
    }
}