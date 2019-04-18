using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using SocialLadder.iOS.Constraints;
using SocialLadder.Models;
using UIKit;

namespace SocialLadder.iOS.Points
{
    public partial class TransactionsTableViewCell : UITableViewCell
    {
        //private static string _lorem = "Lorem ipsum urna duis: sit nam urna nibh donec cursus - pellentesque mattis ultricies massa ultricies rutrum bibendum sagittis tempus mattis: mauris, urna. Sem, bibendum congue, eget at orci pharetra eu rutrum et eros lectus urna auctor metus orci curabitur vivamus elementum sit cursus nec sit sapien. Nam non, fusce eu, fusce lectus nulla eget orci risus nam elementum pellentesque. Elementum vitae commodo arcu: leo quisque sapien amet donec, ipsum cursus. Risus magna sodales auctor, fusce sodales, non auctor in quisque urna, malesuada arcu sed elementum, sodales risus magna. Rutrum diam ipsum duis ligula tellus: orci risus justo, gravida mauris, enim: et pharetra: rutrum mattis risus: in sit mattis amet sodales.";

        public static readonly string ClassName = "TransactionsTableViewCell";
        public static readonly NSString Key = new NSString(ClassName);
        public static readonly UINib Nib;

        public int StringLength { get; set; }
        public UIImageView ScoreImage { get; set; } //to center our time line points on the timeline

        static TransactionsTableViewCell()
        {
            Nib = UINib.FromName(ClassName, NSBundle.MainBundle);
        }

        protected TransactionsTableViewCell(IntPtr handle) : base(handle)
        {
        }

        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
        }

        public void UpdateCellData(TransactionModel transaction)
        {
            lblTransactionText.TextAlignment = UITextAlignment.Left;
            lblTransactionText.LineBreakMode = UILineBreakMode.WordWrap;

            TransactionImage.Image = UIImage.FromBundle("challenges-icon_off");
            //lblTransactionText.Text = _lorem.Substring(StringLength);

            TimeText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);
            PointsText.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 16);
            lblTransactionText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);

            if (transaction == null)
            {
                return;
            }

            TransactionImage.Image = UIImage.FromBundle("challenges-icon_off");
            TimeText.Text = SL.TimeAgoShort(transaction.TransactionDate);
            PointsText.Text = (transaction.TransactionValue > 0 ? "+" : "") + transaction.TransactionValue.ToString() + " pts";
            lblTransactionText.Text = transaction.TransactionType;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            CGRect frame = Frame;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            if (ScoreImage != null)
            {
                var ctx = UIGraphics.GetCurrentContext();
                ctx.SaveState();

                nfloat x = ScoreImage.Frame.X + ScoreImage.Frame.Width / 2.0f;  //center timeline on the scoreimage
                nfloat radius = SizeConstants.ScreenMultiplier * 4;
                ctx.SetFillColor(223.0f / 255.0f, 223.0f / 255.0f, 223.0f / 255.0f, 1.0f);
                ctx.SetLineWidth(2.0f);
                ctx.AddArc(x, this.Frame.Height / 2.0f, radius, 0, (nfloat)Math.PI * 2.0f, true);
                ctx.FillPath();

                ctx.RestoreState();
            }
        }
    }
}
