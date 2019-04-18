using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Challenges
{
    public partial class MultipleChoiceTableView : UITableView
    {
        public int SelectedRow { get; set; }

        public MultipleChoiceTableView (IntPtr handle) : base (handle)
        {
            SelectedRow = -1;
        }
    }
}