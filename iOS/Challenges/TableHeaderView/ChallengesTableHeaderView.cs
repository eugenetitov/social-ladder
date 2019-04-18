using Foundation;
using ObjCRuntime;
using SocialLadder.iOS.Constraints;
using System;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class ChallengesTableHeaderView : UIView
    {
        public string TitleText
        {
            get { return Title.Text; }
            set { Title.Text = value; }
        }

        public bool BadgeHidden
        {
            get { return BadgeView.Hidden; }
            set { BadgeView.Hidden = value; }
        }

        public void UpdateControlls()
        {
            BadgeView.Layer.CornerRadius = SizeConstants.Screen.Width * 0.008f;
            Title.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.Screen.Width * 0.035f);
        }

        public static ChallengesTableHeaderView Create()
        {
            var nsArr =  NSBundle.MainBundle.LoadNib("ChallengesTableHeaderView", null, null);
            var view = Runtime.GetNSObject<ChallengesTableHeaderView>(nsArr.ValueAt(0));
            return view;
        }

        public ChallengesTableHeaderView(IntPtr handle) : base (handle)
        {

        }
    }
}