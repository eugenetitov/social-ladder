using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Models
{
    public class PageModel
    {
        public PageModel(string stroryboard, string viewController)
        {
            Storyboard = stroryboard;
            ViewController = viewController;
        }
        public string Storyboard
        {
            get; set;
        }
        public string ViewController
        {
            get; set;
        }
    }
}