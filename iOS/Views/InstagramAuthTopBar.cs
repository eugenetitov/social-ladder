using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS
{
    public partial class InstagramAuthTopBar : UIView
    {
        public UIButton BtnCancel => btnCancel;
        public UILabel LblTitle => lblTitle;

        public static InstagramAuthTopBar Create()
        {
            NSArray arr = NSBundle.MainBundle.LoadNib("InstagramAuthTopBar", null, null);
            InstagramAuthTopBar view = arr.GetItem<InstagramAuthTopBar>(0);

            return view;
        }

        public InstagramAuthTopBar (IntPtr handle) : base (handle)
        {
        }
    }
}