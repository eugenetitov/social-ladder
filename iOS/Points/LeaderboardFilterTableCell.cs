using System;
using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Constraints;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public partial class LeaderboardFilterTableCell : UITableViewCell
    {
        private nfloat _screenWidth = UIScreen.MainScreen.Bounds.Width;

        public static readonly NSString Key = new NSString("LeaderboardFilterTableCell");
        public static readonly UINib Nib;

        public override bool Selected
        {
            get
            {
                return base.Selected;
            }
            set
            {
                base.Selected = value;
            }
        }

        static LeaderboardFilterTableCell()
        {
            Nib = UINib.FromName("LeaderboardFilterTableCell", NSBundle.MainBundle);
        }

        protected LeaderboardFilterTableCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        public void UpdateCell(string model)
        {
            lblFilterName.Text = model;
            lblFilterName.TextColor = Selected == true ? UIColor.FromRGB(0f, 122f / 255f, 194f / 255f) : UIColor.Black;
            if (Selected)
            {
                //Draw(vBasis.Frame);
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            lblFilterName.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 16);
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            //if (!Selected)
            //{
            //    return;
            //}
            //var ctx = UIGraphics.GetCurrentContext();
            //ctx.SaveState();

            //nfloat x = vBasis.Frame.Width / 2;
            //nfloat y = vBasis.Frame.Height;
            //nfloat radius = _screenWidth / CodeBehindUIConstants.BaseMarkupWidth * 2.0f;
            //ctx.SetFillColor(0f, 122f / 255f, 194f / 255f, 1);
            //ctx.SetLineWidth(0.1f);
            //ctx.AddArc(x, y, radius, 0, (nfloat)Math.PI * 2.0f, true);
            //ctx.FillPath();
            //ctx.RestoreState();
        }
    }
}
