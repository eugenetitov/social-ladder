using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreGraphics;
using FFImageLoading;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using SocialLadder.iOS.Areas;
using UIKit;

namespace SocialLadder.iOS.ViewControllers.Main
{
    public class BaseTabViewController<T> : MvxViewController<T> where T : class, IMvxViewModel
    {
        nfloat AreaCollectionHeightConstant
        {
            get; set;
        }
        UISwipeGestureRecognizer SwipeDown
        {
            get; set;
        }
        UITapGestureRecognizer Tap
        {
            get; set;
        }

        protected AreaCollectionView AreaCollectionOutlet
        {
            get; set;
        }
        protected NSLayoutConstraint AreaCollectionHeightOutlet
        {
            get; set;
        }
        protected NSLayoutConstraint AreaCollectionTopOutlet
        {
            get; set;
        }

        protected bool AreaCollectionHidden;

        public bool IsChangingAreasDisabled;

        private static bool _isLoaded;

        public BaseTabViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public BaseTabViewController(IntPtr handle) : base(handle)
        {
        }

        protected void UpdateNavBar()
        {
            //SLNavigationController navigationController = NavigationController as SLNavigationController;
            //if (navigationController != null)
            //    navigationController.Update();
        }

        private void CheckColor(UIColor color)
        {
            var components = color.CGColor.Components;
            var result = (components[0] * 0.299 + components[1] * 0.587 + components[2] * 0.114) * 255;

            if (result > 186)
            {
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;
            }
            else
            {
                UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            

            UpdateNavBar();
        }

        public async Task RefreshAreas()
        {
            await AreaCollectionOutlet.Refresh();
        }

        public virtual void HideAreaCollection()
        {
            if (SL.Area != null)
            {
                CheckColor(SL.Area.areaPrimaryColor.ToUIColor());
            }

            AreaCollectionHeightOutlet.Constant = 0;
            AreaCollectionOutlet.SetNeedsLayout();
            AreaCollectionHidden = true;

            //Show Nav bar
            if (NavigationController != null)
                NavigationController.SetNavigationBarHidden(false, true);

            Debug.WriteLine(@"Hide Area Collection " + this.Class.Name);
        }

        public void ShowAreaCollection()
        {
            

            Debug.WriteLine(@"Show Area Collection " + this.Class.Name);
        }

        private void NavAreasClicked()
        {
            if (IsChangingAreasDisabled)
            {
                return;
            }
            //ToggleAreaCollection();
            //AreaCollectionHidden = false;
            ShowAreaCollection();
        }

        private void NavNotificationClicked()
        {

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void HandleDrag(UISwipeGestureRecognizer recognizer)
        {
            //ToggleAreaCollection();
            //AreaCollectionHidden = false;
            ShowAreaCollection();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            //float inset = 30.0f;
            //float inset2 = 29.5f;
            //AreaCollectionOutlet.Source = new AreaCollectionSource();
            //AreaCollectionOutlet.CollectionViewLayout = new UICollectionViewFlowLayout
            //{
            //    ItemSize = new CGSize(138, 161),
            //    ScrollDirection = UICollectionViewScrollDirection.Horizontal,
            //    SectionInset = new UIEdgeInsets(inset2, inset2, inset2, inset2),
            //    MinimumInteritemSpacing = inset,
            //    MinimumLineSpacing = inset
            //};
            //AreaCollectionOutlet.ReloadData();


            //if (_isLoaded)
            //{
            //    HideAreaCollection();
            //}
            //else
            //{
            //    ShowAreaCollection();
            //}
        }



        public virtual async Task Refresh()
        {

            //SlNavController.NavTitle.CloseLoadIndicator();
        }

        public virtual void BeginRefresh()
        {

        }

 
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            UpdateNavBar();
        }
    }
}