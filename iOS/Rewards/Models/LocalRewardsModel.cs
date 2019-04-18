using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Rewards.Models
{
    public class LocalRewardsModel
    {
        public UIViewController Controller
        {
            get; set;
        }
        public UIPageViewControllerNavigationDirection Direction
        {
            get; set;
        }
    }
}