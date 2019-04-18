using Foundation;
using System;
using UIKit;
using SocialLadder.Models;
using CoreGraphics;
using SocialLadder.iOS.Constraints;
using SocialLadder.iOS.Sources.Feed;
using SocialLadder.iOS.Views.Cells;

namespace SocialLadder.iOS.Challenges
{
    public partial class ChallengeCompleteView : UIView
    {
        nfloat CellHeight { get; set; }

        public FeedTableSource FeedTableSource;
        public UIView Container { get; set; }
        public int ResponseCode { get; set; }
        public ChallengeDetailBaseViewController Controller { get; set; }

        public ChallengeCompleteView(IntPtr handle) : base(handle)
        {

        }

        public static ChallengeCompleteView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("ChallengeCompleteView", null, null);
            ChallengeCompleteView view = arr.Count > 0 ? arr.GetItem<ChallengeCompleteView>(0) : null;

            view.TableView.AllowsSelection = false;
            view.TableView.ScrollEnabled = true;
            view.TableView.RegisterNibForCellReuse(FeedTableViewCell.Nib, FeedTableViewCell.ClassName);
            //view.TableView.RegisterNibForCellReuse(MasterFeedTableViewCell.Nib, MasterFeedTableViewCell.ClassName);

            view.FeedTableSource = new FeedTableSource(view.TableView);
            view.TableView.Source = view.FeedTableSource;
            view.TableView.RowHeight = UITableView.AutomaticDimension;
            view.TableView.EstimatedRowHeight = 130.0f;

            view.TableView.ReloadData();

            view.vWhiteBackground.Layer.CornerRadius = 4.0f;//view.Layer.CornerRadius = 4.0f;

            //view.TableHeightConstraint.Constant = view.FeedTableSource.GetFirstItemHeigth(view.TableView);

            //var tableViewFrame = view.TableView.Frame;
            //view.TableView.Frame = new CGRect(tableViewFrame.X, tableViewFrame.Y, tableViewFrame.Width, 5000);
            return view;
        }

        //public void Update(FeedItemModel feed)
        //public void Update(UIView container, ChallengeResponseModel challengeResponse, ChallengeModel challenge)
        public void Update(UIView container, ChallengeResponseModel challengeResponse, ChallengeModel challenge, ChallengeDetailBaseViewController ctrl = null)
        {
            ResponseCode = challengeResponse?.ResponseCode ?? 0;
            Controller = ctrl;

            Container = container;
            (Container as UIButton).TouchUpInside += CloseAction;
            btnCloseBgBottom.TouchUpInside += CloseAction;
            btnCloseBg.TouchUpInside += CloseAction;

            RoundImage.Image = UIImage.FromBundle("ScoreCircle");
            //Commit
            //CGRect cell = TableView.RectForRowAtIndexPath(NSIndexPath.FromIndex(0));
            float backgroundToOverlayWidthRatio = 0.9227f;      //from spec
            nfloat width = container.Frame.Width * backgroundToOverlayWidthRatio;
            //float backgroundHeightToWidthRatio = 0.9529f;       //from spec
            //nfloat height = width * backgroundHeightToWidthRatio;
            nfloat height = TableView.Frame.Y + CellHeight + 200;//cell.Height;
            nfloat x = (container.Frame.Width - width) / 2.0f;    //center in overlay
            nfloat y = (container.Frame.Height - height) / 2.0f;  //center in overlay
            Frame = new CGRect(x, y, width, height);

            //ChallengeImage.SetNeedsLayout();
            /*
            TableView.Source = new FeedTableSource();
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 130.0f;
            TableView.ReloadData();
            */

            MessageText.Text = challengeResponse.ResponseMessage;
            if (challenge.CollateralReview ?? false)
            {
                PointsText.Text = String.Empty;
            }
            else
            {
                PointsText.Text = challengeResponse.ResponseCode > 0 && challenge.PointValue > 0 ? (challenge.CompPointValue + (challenge.PointsPerInstance * challenge.CompletedCount)) + " pts added to your account" : "";
            }

            AddTriangularView();
        }

        public void Update(UIView container, ShareResponseModel shareResponse, ChallengeModel challenge, ChallengeDetailBaseViewController ctrl = null)
        {
            ResponseCode = shareResponse?.ResponseCode ?? 0;
            Controller = ctrl;

            //CGRect cell = TableView.RectForRowAtIndexPath(NSIndexPath.FromIndex(0));
            Container = container;
            (Container as UIButton).TouchUpInside += CloseAction;
            btnCloseBgBottom.TouchUpInside += CloseAction;
            btnCloseBg.TouchUpInside += CloseAction;

            RoundImage.Image = UIImage.FromBundle("IconScoreTransactions");
            //Commit
            float backgroundToOverlayWidthRatio = 0.9227f;      //from spec
            nfloat width = container.Frame.Width * backgroundToOverlayWidthRatio;
            //float backgroundHeightToWidthRatio = 0.9529f;       //from spec
            //nfloat height = width * backgroundHeightToWidthRatio;
            nfloat height = TableView.Frame.Y + CellHeight + 200;//cell.Height;
            nfloat x = (container.Frame.Width - width) / 2.0f;    //center in overlay
            nfloat y = (container.Frame.Height - height) / 2.0f;  //center in overlay
            Frame = new CGRect(x, y, width, height);

            //ChallengeImage.SetNeedsLayout();
            /*
            TableView.Source = new FeedTableSource();
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 130.0f;
            TableView.ReloadData();
            */
            MessageText.Text = shareResponse.ResponseMessage;
            PointsText.Text = shareResponse.ResponseCode > 0 && challenge.PointValue > 0 ? challenge.PointValue + " pts added to your account" : "";
            AddTriangularView();
        }

        private void AddTriangularView()
        {
            var margin = SizeConstants.Screen.Width * 0.03f;
            var heigthOffcet = ((SizeConstants.Screen.Width * 0.30362f * 0.8f) - (SizeConstants.Screen.Width * 0.28f * 0.8f)) / 4;
            var widthOffcet = ((SizeConstants.Screen.Width * 0.30362f) - (SizeConstants.Screen.Width * 0.28f)) / 4;
            var triangularBorder = new TriangularView(new CGRect(0, 0, SizeConstants.Screen.Width * 0.30362f, SizeConstants.Screen.Width * 0.30362f * 0.8f));

            //var triangularBackgrpundView = new TriangularView(new CGRect(0, 0, SizeConstants.Screen.Width * 0.2907f, SizeConstants.Screen.Width * 0.2907f), 0f)
            //{
            //};
            var triangularView = new TriangularView(new CGRect(widthOffcet, heigthOffcet, SizeConstants.Screen.Width * 0.28f, SizeConstants.Screen.Width * 0.28f * 0.8f));

            //triangularView2.BackgroundColor = UIColor.Black;

            TopViewContainer.AddSubview(triangularBorder);
            TopViewContainer.AddSubview(triangularView);
            TopViewContainer.BringSubviewToFront(triangularView);

            var image = new UIImageView();
            image.Frame = triangularView.Frame;
            image.Image = UIImage.FromBundle("LaunchScreen");
            image.ContentMode = UIViewContentMode.ScaleAspectFill;
            triangularView.ClipsToBounds = true;
            triangularView.AddSubview(image);
        }

        private void AddTopImageView()
        {
        }

        partial void CloseButton_TouchUpInside(UIButton sender) =>
            CloseAction(sender, null);

        private void CloseAction(object sender, EventArgs e)
        {
            Container.RemoveFromSuperview();
            if (ResponseCode > 0)
            {
                if (Controller is ShareViewController || Controller is PosteringViewController)
                {
                    Controller?.NavigationController?.PopViewController(true);
                }
                Controller?.NavigationController?.PopViewController(true);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            //CGRect cell = TableView.RectForRowAtIndexPath(NSIndexPath.FromIndex(0));
            CellHeight = TableView.GreatestCellHeight;

            TableHeightConstraint.Constant = 0f; // FeedTableSource.GetFirstItemHeigth(TableView);

            TableView.Layer.BorderColor = UIColor.FromRGB(240, 240, 240).CGColor;
            TableView.Layer.BorderWidth = 1f;
            TableView.Layer.CornerRadius = SizeConstants.Screen.Width * 0.01f;
            MainView.Layer.CornerRadius = SizeConstants.Screen.Width * 0.006f;
            RoundImage.Layer.CornerRadius = SizeConstants.Screen.Width * 0.1f;

            MessageText.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.Screen.Width * 0.06f);
            PointsText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.Screen.Width * 0.036f);
        }
    }
}