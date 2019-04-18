using Foundation;
using ObjCRuntime;
using System;
using UIKit;

namespace SocialLadder.iOS.More
{
    public partial class FriendRequestsTableHeaderView : UIView
    {
        public string TitleText
        {
            get { return Title.Text; }
            set { Title.Text = value; }
        }

        public void UpdateControlls()
        {
            Title.Font = UIFont.FromName("ProximaNova-Bold", UIScreen.MainScreen.Bounds.Width * 0.035f);
        }

        public static FriendRequestsTableHeaderView Create()
        {
            var nsArr =  NSBundle.MainBundle.LoadNib("FriendRequestsTableHeaderView", null, null);
            var view = Runtime.GetNSObject<FriendRequestsTableHeaderView>(nsArr.ValueAt(0));
            return view;
        }

        public FriendRequestsTableHeaderView(IntPtr handle) : base (handle)
        {

        }
    }
}