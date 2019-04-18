using System;
using UIKit;
using SocialLadder.iOS.Areas;
using Foundation;
using CoreGraphics;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using FFImageLoading;

namespace SocialLadder.iOS.Navigation
{
    public partial class SLViewController : UIViewController
    {
        nfloat AreaCollectionHeightConstant { get; set; }
        UISwipeGestureRecognizer SwipeDown { get; set; }
        UITapGestureRecognizer Tap { get; set; }

        protected AreaCollectionView AreaCollectionOutlet { get; set; }
        protected NSLayoutConstraint AreaCollectionHeightOutlet { get; set; }
        protected NSLayoutConstraint AreaCollectionTopOutlet { get; set; }
        protected bool AreaCollectionHidden;

        public bool IsChangingAreasDisabled;

        private static bool _isLoaded;

        public SLViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public SLViewController(IntPtr handle) : base(handle)
        {
        }

        protected void UpdateNavBar()
        {
            SLNavigationController navigationController = NavigationController as SLNavigationController;
            if (navigationController != null)
                navigationController.Update();
        }

        void CheckColor(UIColor color)
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

            // Perform any additional setup after loading the view, typically from a nib.
            AreaCollectionOutlet.ViewController = this;
            AreaCollectionOutlet.RegisterNibForCell(AreaCollectionViewCell.Nib, AreaCollectionViewCell.ClassName);

            AreaCollectionHeightConstant = AreaCollectionHeightOutlet.Constant;
            AreaCollectionHeightOutlet.Constant = 0;
            AreaCollectionOutlet.SetNeedsLayout();

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
            UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.Default;

            if (SL.IsBusy)
            {
                return;
            }
            AreaCollectionHeightOutlet.Constant = AreaCollectionHeightConstant;
            AreaCollectionHidden = false;
            AreaCollectionOutlet.ReloadData();
            AreaCollectionOutlet.SetNeedsLayout();

            //Hide Nav bar
            if (NavigationController != null && (_isLoaded ? View?.Window != null : true))//fix floating bug when Settings hides navbar
                NavigationController.SetNavigationBarHidden(true, true);

            _isLoaded = true;

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

            float inset = 30.0f;
            float inset2 = 29.5f;
            AreaCollectionOutlet.Source = new AreaCollectionSource();
            AreaCollectionOutlet.CollectionViewLayout = new UICollectionViewFlowLayout
            {
                ItemSize = new CGSize(138, 161),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                SectionInset = new UIEdgeInsets(inset2, inset2, inset2, inset2),
                MinimumInteritemSpacing = inset,
                MinimumLineSpacing = inset
            };
            AreaCollectionOutlet.ReloadData();

            if (NavigationController != null && !IsChangingAreasDisabled)
            {
                SwipeDown = new UISwipeGestureRecognizer();
                SwipeDown.AddTarget(() => HandleDrag(SwipeDown));
                SwipeDown.Direction = UISwipeGestureRecognizerDirection.Down;
                NavigationController.NavigationBar.AddGestureRecognizer(SwipeDown);
                (NavigationController as SLNavigationController)?.NavTitle?.AddGestureRecognizer(SwipeDown);

                Tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
                Tap.AddTarget(() => NavAreasClicked());
                Tap.ShouldReceiveTouch += (recognizer, touch) => !(touch.View is UIControl);
                NavigationController.NavigationBar.AddGestureRecognizer(Tap);
                (NavigationController as SLNavigationController)?.NavTitle?.AddGestureRecognizer(Tap);

                Debug.WriteLine(@"Added Swipe Down Gesture " + this.Class.Name);
            }

            if (_isLoaded)
            {
                HideAreaCollection();
            }
            else
            {
                ShowAreaCollection();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            if (NavigationController != null && SwipeDown != null)
            {
                NavigationController.NavigationBar.RemoveGestureRecognizer(SwipeDown);
                (NavigationController as SLNavigationController)?.NavTitle?.RemoveGestureRecognizer(SwipeDown);

                Debug.WriteLine(@"Removed Swipe Down Gesture " + this.Class.Name);
            }
            if (NavigationController != null && Tap != null)
            {
                NavigationController.NavigationBar.RemoveGestureRecognizer(Tap);
                (NavigationController as SLNavigationController)?.NavTitle?.RemoveGestureRecognizer(Tap);
                Debug.WriteLine(@"Removed Tap Down Gesture " + this.Class.Name);
            }

            //if (!AreaCollectionHidden)
            //AreaCollectionHidden = true;
            HideAreaCollection();
        }

        public virtual async Task Refresh()
        {

            //SlNavController.NavTitle.CloseLoadIndicator();
        }

        public virtual void BeginRefresh()
        {

        }

        public override UINavigationController NavigationController
        {
            get
            {

                //return base.NavigationController != null ? base.NavigationController : Platform.TopViewController as UINavigationController;
                SLNavigationController n1 = base.NavigationController as SLNavigationController;
                if (n1 == null)
                    n1 = Platform.TopViewController as SLNavigationController;
                return n1 as UINavigationController;

            }
        }

        public SLNavigationController SlNavController
        {
            get { return NavigationController as SLNavigationController; }
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            UpdateNavBar();
        }
    }
}

