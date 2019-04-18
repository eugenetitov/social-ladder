using Foundation;
using ObjCRuntime;
using System;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public partial class TableHeaderView : UIView
    {
        public TableHeaderView (IntPtr handle) : base (handle)
        {
        }

        public static TableHeaderView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("TableHeaderView", null, null);
            var view = Runtime.GetNSObject<TableHeaderView>(arr.ValueAt(0));
            view.BackgroundColor = UIColor.Clear;
            view.BackgroundView.BackgroundColor = UIColor.Clear;
            view.UpdateText(string.Empty, string.Empty);
            return view;
        }

        public void UpdateText(string titleText, string subtitleText)
        {
            this.BackgroundView.BackgroundColor = UIColor.White;
            this.BackgroundColor = UIColor.White;
            separatorText.Hidden = false;
            if ((subtitleText == string.Empty) || (subtitleText == null))
            {
                separatorText.Hidden = true;
            }
            TitleLabel.Text = titleText;
            subTitleLabel.Text = subtitleText;
        }
    }
}