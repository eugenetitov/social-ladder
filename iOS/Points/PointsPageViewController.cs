using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public partial class PointsPageViewController : UIPageViewController
    {
        public PointsPageViewController (IntPtr handle) : base (handle)
        {
            
        }

        /*
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //DataSource = new PointsPageViewControllerDataSource(this);

            //SetViewControllers(DataSource.)
        }

        public UIViewController ViewControllerAtIndex(int index)
        {
            UIViewController vc;//this.Storyboard.InstantiateViewController("ContentViewController") as ContentViewController;
            switch(index)
            {
                case 0:
                    vc = this.Storyboard.InstantiateViewController("PointsViewController");
                    break;
                default:
                    vc = null;
                    break;
            }
            //if (vc != null)
                //vc.pageIndex = index;
            return vc;
        }
        */
    }
}