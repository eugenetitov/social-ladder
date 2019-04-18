using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Connectivity
{
    public partial class ConnectionMessageView : UIView
    {

        public static float CollectionViewHeight = 40;

        public ConnectionMessageView (IntPtr handle) : base (handle)
        {
        }

        public static ConnectionMessageView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("ConnectionMessageView", null, null);
            ConnectionMessageView view = arr.Count > 0 ? arr.GetItem<ConnectionMessageView>(0) : null;
            var screen = UIScreen.MainScreen.Bounds;
            view.Frame = new CoreGraphics.CGRect(0, 0, screen.Width, CollectionViewHeight);
            view.MessageLb.Font = UIFont.FromName("ProximaNova-Regular", 17);
            view.DismissButton.TouchUpInside += (sender, e) =>
            {
                view.RemoveFromSuperview();
            };
            return view;
        }
    }
}