using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Views;
using Android.Content;
using SocialLadder.Droid.Assets;

namespace SocialLadder.Droid
{
    [Activity(
            Label = "SocialLadder"
            , MainLauncher = true
            , Icon = "@drawable/Icon"
            , Theme = "@style/Theme.Splash"
            , NoHistory = true
            , ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreen : MvxSplashScreenActivity
    {
        public SplashScreen() : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void OnStart()
        {
            base.OnStart();
            var text = FindViewById<TextView>(Resource.Id.textView);
            text.Typeface = Typeface.CreateFromAsset(Application.Context.Assets, FontsConstants.HD_M);
            var metrics = Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.ScaledDensity;
            text.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)(metrics * (float)0.125));
        }
    }
}