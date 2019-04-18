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
using static Android.Views.View;

namespace SocialLadder.Droid.CustomListeners
{
    public class ControlTouchListener : Java.Lang.Object, IOnTouchListener
    {
        private float FirstYPrecision { get; set; }
        private float SecondYPrecision { get; set; }
        private readonly int _defaultCheckHeight = 12;
        public event EventHandler SwipeUp;
        public event EventHandler SwipeDown;

        public bool OnTouch(View v, MotionEvent e)
        {
            var checkheight = v.Height <= 0 ? _defaultCheckHeight : v.Height * 0.04;
            if (e.ActionMasked == MotionEventActions.Move)
            {
                if (FirstYPrecision != 0 && SecondYPrecision == 0)
                {
                    SecondYPrecision = e.GetY();
                }
                if (FirstYPrecision == 0)
                {
                    FirstYPrecision = e.GetY(); ;
                }
            }
            if (e.ActionMasked == MotionEventActions.Up)
            {
                if (FirstYPrecision != 0 && SecondYPrecision != 0 && (FirstYPrecision - SecondYPrecision) > checkheight)
                {
                    try
                    {
                        SwipeUp.Invoke(null, null);
                    }
                    catch (Exception ex) { Console.WriteLine("SwipeUp Exeption" + ex.Message); }
                }
                if (FirstYPrecision != 0 && SecondYPrecision != 0 && (SecondYPrecision - FirstYPrecision) > 0)
                {
                    try
                    {
                        SwipeDown.Invoke(null, null);
                    }
                    catch (Exception ex) { Console.WriteLine("SwipeDown Exeption" + ex.Message); }
                }
                FirstYPrecision = 0;
                SecondYPrecision = 0;
            }

            return false;
        }
    }
}