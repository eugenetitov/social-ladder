using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using SocialLadder.iOS.Navigation;
using UIKit;

namespace SocialLadder.iOS.Rewards
{
    public class RewardsBaseViewController : SLViewController
    {
        public int PageIndex
        {
            get; set;
        }
        public RewardsContainerViewController RewardsContainerViewController
        {
            get; set;
        }



        public RewardsBaseViewController(IntPtr handle) : base(handle)
        {
        }

        public void NextPage()
        {
            RewardsContainerViewController.NextPage();
        }

        public void PrevPage()
        {
            RewardsContainerViewController.PrevPage();
        }
    }
}