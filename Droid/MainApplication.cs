using System;
using Android.App;
using Android.Runtime;
using Android.Support.Text.Emoji;
using Android.Support.Text.Emoji.Bundled;
using Android.Util;
using Firebase;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid
{
    [Application]
    public class MainApplication : MvxAndroidApplication
    {
        //private IEmojiToWebApiService _emojiToWebApiService;

        public MainApplication(IntPtr handle, JniHandleOwnership ownerShip/*, IEmojiToWebApiService emojiToWebApiService*/) : base(handle, ownerShip)
        {
            //_emojiToWebApiService = emojiToWebApiService;
        }

        public override void OnCreate()
        {
            //var _emojiService = Mvx.Resolve<IEmojiToWebApiService>();
            // If OnCreate is overridden, the overridden c'tor will also be called.
            SL.Manager = new SLManager(new RestService(/*_emojiToWebApiService*/), Platform.LocalDataBasePath);
            EmojiCompat.Config config;
            config = new BundledEmojiCompatConfig(this);
            EmojiCompat.Init(config);

            Firebase.FirebaseApp.InitializeApp(this);
            base.OnCreate();
        }
    }
}
