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
using MvvmCross.Plugins.Messenger;

namespace SocialLadder.Droid.Models.MvxMessanger
{
    public class ImagePickerMessangerModel : MvxMessage
    {
        public System.Collections.IList Images;

        public ImagePickerMessangerModel(object sender, System.Collections.IList model) : base(sender)
        {
            Images = model;
        }
    }

}