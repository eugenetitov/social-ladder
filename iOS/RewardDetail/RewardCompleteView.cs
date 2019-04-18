using Foundation;
using System;
using UIKit;
using SocialLadder.Models;
using CoreGraphics;
using CoreAnimation;
using FFImageLoading;
using SocialLadder.iOS.Constraints;
using WebKit;
using SocialLadder.iOS.Sources.Feed;
using SocialLadder.iOS.Views.Cells;

namespace SocialLadder.iOS.Rewards
{
    public partial class RewardCompleteView : UIView
    {
        public static UIView Overlay { get; set; }
        nfloat CellHeight { get; set; }
        public int ResponseCode { get; set; }
        public UIView Container { get; set; }
        public RewardDetailViewController Controller { get; set; }

        public event Action onViewClosed;

        public RewardCompleteView(IntPtr handle) : base(handle)
        {
        }

        public override void RemoveFromSuperview()
        {
            base.RemoveFromSuperview();
        }

        public static RewardCompleteView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("RewardCompleteView", null, null);
            RewardCompleteView view = arr.Count > 0 ? arr.GetItem<RewardCompleteView>(0) : null;

            view.TableView.AllowsSelection = false;
            view.TableView.ScrollEnabled = false;
            view.TableView.RegisterNibForCellReuse(FeedTableViewCell.Nib, FeedTableViewCell.ClassName);
            //view.TableView.RegisterNibForCellReuse(MasterFeedTableViewCell.Nib, MasterFeedTableViewCell.ClassName);

            view.TableView.Source = new FeedTableSource(view.TableView);
            view.TableView.RowHeight = UITableView.AutomaticDimension;
            view.TableView.EstimatedRowHeight = 130.0f;
            //view.TableView.SeparatorColor = UIColor.Clear;
            view.TableView.Layer.BorderWidth = 0;
            view.TableView.Layer.BorderColor = new CoreGraphics.CGColor(0, 0, 0, 0);//UIColor.Clear;                
            view.Layer.CornerRadius = 4.0f;

            view.TableView.ReloadData();

            return view;
        }

        private void UpdateFonts()
        {
            var font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 24f);
            apologizeLabel.Font = font;
            reasonLabel.Font = font;
            MessageText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 16f);
            btnSeeOtherRewards.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14f);
        }

        //public void Update(FeedItemModel feed)
        public void Update(UIView container, RewardResponseModel rewardResponse, RewardItemModel reward, RewardDetailViewController ctrl = null)
        {
            ResponseCode = rewardResponse?.ResponseCode ?? 0;
            Container = container;
            Controller = ctrl;

            UpdateFonts();

            //float backgroundToOverlayWidthRatio = 0.9227f;      //from spec
            //float backgroundToOverlayHegithRatio = 0.761f;

            //float backgroundToOverlayHegithRatio = 0.461f;
            nfloat width = container.Frame.Width;
            nfloat height = width * 0.89f;//cell.Height;
            nfloat x = (container.Frame.Width - width) / 2.0f;    //center in overlay
            nfloat y = (container.Frame.Height - height) / 2.0f;  //center in overlay
            Frame = new CGRect(x, y, width, height);

            CloseButton.TouchUpInside += CloseAction;
            (Container as UIButton).TouchUpInside += CloseAction;

            apologizeLabel.Text = rewardResponse.ResponseCode > 0 ? "Congratulations, " : "Oh No, we`re sorry!";
            reasonLabel.Text = rewardResponse.ResponseCode > 0 ? "You got this reward!" : "You did not get this reward...";

            MessageText.Text = !string.IsNullOrWhiteSpace(rewardResponse.ResponseMessage) ? rewardResponse.ResponseMessage
                : (rewardResponse.ResponseCode > 0 ? $"You spent {reward.MinScore} pts" : "There are no more units available");
            MessageText.Lines = 0;//can be 5 lines if email not confirmed

            if (rewardResponse.ResponseCode <= 0)
            {
                btnSeeOtherRewards.Hidden = false;
                btnSeeOtherRewards.SetTitle("See what other rewards you qualify for >", UIControlState.Normal);
                btnSeeOtherRewards.TouchUpInside += (object sender, EventArgs e) =>
                {
                    onViewClosed?.Invoke();
                    container.RemoveFromSuperview();
                    ctrl?.NavigationController.PopViewController(true);//go to rewards list
                };
            }

            EventImage.ContentMode = UIViewContentMode.ScaleAspectFit;
            EventImage.BackgroundColor = UIColor.White; // Border Color

            double borderMultiplier = 0.265;
            double contentMultiplier = 0.25;

            var borderMaskImageView = new UIImageView();
            borderMaskImageView.Image = UIImage.FromBundle("Polygon-mask1");
            borderMaskImageView.Frame = new CGRect(0, 0, container.Frame.Width * borderMultiplier, (container.Frame.Width * borderMultiplier) * 1.1);
            borderMaskImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            var contentMaskImageView = new UIImageView();
            contentMaskImageView.Image = UIImage.FromBundle("Polygon-mask1");
            contentMaskImageView.Frame = new CGRect(0, 0, container.Frame.Width * contentMultiplier, (container.Frame.Width * contentMultiplier) * 1.1);
            contentMaskImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            float startContentX = (float)(container.Frame.Width * borderMultiplier - container.Frame.Width * contentMultiplier) / 2;
            float startContentY = (float)((container.Frame.Width * borderMultiplier) * 1.1 - (container.Frame.Width * contentMultiplier) * 1.1) / 2;

            var profileImage = new UIImageView();
            //profileImage.Image = UIImage.FromBundle("CellImagePlaceholder"); //hardcode
            profileImage.BackgroundColor = UIColor.Gray;
            ImageService.Instance.LoadUrl(reward.MainImageURL).Into(profileImage);

            profileImage.ContentMode = UIViewContentMode.ScaleAspectFill;
            profileImage.MaskView = contentMaskImageView;

            // Make a little bit smaller to show "border" image behind it
            profileImage.Frame = new CGRect(startContentX, startContentY, container.Frame.Width * contentMultiplier, (container.Frame.Width * contentMultiplier) * 1.1);

            EventImage.MaskView = borderMaskImageView;
            EventImage.AddSubview(profileImage);

            if (rewardResponse.ResponseCode > 0)
            {
                collectButton.Hidden = false;
                collectButton.TouchUpInside += (object sender, EventArgs e) =>
                {
                    var webView = new WKWebView(ctrl.View.Frame, new WKWebViewConfiguration());
                    webView.NavigationDelegate = new NavDelegate(this);
                    Overlay = Platform.AddOverlay(ctrl.View, webView.Frame, UIColor.White, true, true, 0.1f);
                    webView.LoadRequest(new NSUrlRequest(new NSUrl("https://socialladder.rkiapps.com/SL/HelpDesk/RewardStatus?deviceUUID=" + SL.DeviceUUID + "&AreaGUID=" + SL.AreaGUID)));
                    ctrl.View.AddSubview(webView);

                    onViewClosed?.Invoke();
                    container.RemoveFromSuperview();
                };
            }
        }

        private void CloseAction(object sender, EventArgs e)
        {
            onViewClosed?.Invoke();
            Container.RemoveFromSuperview();

            if (ResponseCode > 0)
            {
                Controller?.NavigationController.PopViewController(true);
            }
        }

        public void Update(UIView container, ShareResponseModel shareResponse, ChallengeModel challenge)
        {
            Container = container;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            CellHeight = TableView.GreatestCellHeight;
        }

        public class NavDelegate : WKNavigationDelegate
        {
            RewardCompleteView rewardCompleteView { get; set; }

            public NavDelegate(RewardCompleteView viewController)
            {
                rewardCompleteView = viewController;
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                decisionHandler(WKNavigationActionPolicy.Allow);
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                Overlay.RemoveFromSuperview();
            }
        }
    }
}