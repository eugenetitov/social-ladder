using Foundation;
using SocialLadder.iOS.Navigation;
using SocialLadder.iOS.Points;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;

namespace SocialLadder.iOS.Areas
{
    public partial class AreaCollectionView : UICollectionView
    {
        public SLViewController ViewController { get; set; }

        public AreaCollectionView(IntPtr handle) : base(handle)
        {
        }

        public void Show()
        {
            ViewController.ShowAreaCollection();
        }

        public void Hide()
        {
            ViewController.HideAreaCollection();
        }
        /*
        public void Toggle()
        {
            ViewController.ToggleAreaCollection();
            
        }
        */
        public void AddMoreAreas()
        {
            UIStoryboard board = UIStoryboard.FromName("Areas", null);
            AreasViewController controller = (AreasViewController)board.InstantiateViewController("Areas");
            controller.View.BackgroundColor = UIColor.Black;
            ViewController.NavigationController.PushViewController(controller, true);
        }

        public void BeginRefresh()
        {
            ViewController.BeginRefresh();
        }

        public async Task Refresh()
        {
            if (Platform.IsInternetConnectionAvailable() == false)
            {
                return;
            }
            ViewController.SlNavController.NavTitle.ShowLoadIndicator();

            //PointsBaseViewController pointsController = ViewController as PointsBaseViewController;
            //List<bool> _shouldRfereshEndpoints = pointsController?.PointsContainerViewController?.SeparatelyShouldRefreshEndpoints;
            //if (_shouldRfereshEndpoints != null)
            //{
            //    List<bool> _updatedShouldRfereshEndpoints = new List<bool>();
            //    foreach (bool item in _shouldRfereshEndpoints)
            //    {
            //        _updatedShouldRfereshEndpoints.Add(true);
            //    }
            //    _shouldRfereshEndpoints = _updatedShouldRfereshEndpoints;
            //}

            await ViewController.Refresh();
            ViewController.SlNavController.NavTitle.CloseLoadIndicator();
        }
    }
}