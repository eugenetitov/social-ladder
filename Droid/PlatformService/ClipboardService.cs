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
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.PlatformService
{
    public class ClipboardService : IClipboardService
    {
        public void SaveToClipboard(string text)
        {
            ClipboardManager clipboard = (ClipboardManager)Application.Context.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("clip_text", text);
            clipboard.PrimaryClip = clip;
        }
    }
}