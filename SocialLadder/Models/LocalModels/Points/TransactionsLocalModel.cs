using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Models.LocalModels.Points
{
    public class TransactionsLocalModel
    {
        public string TransactionImage { get; set; }
        public string TimeText { get; set; }
        public string PointsText { get; set; }
        public string TransactionText { get; set; }

        public TransactionsLocalModel(string transactionImage, DateTime time, double transactionValue, string transactionType)
        {
            TransactionImage = transactionImage;
            TimeText = SL.TimeAgoShort(time);
            PointsText = (transactionValue > 0 ? "+" : "") + transactionValue.ToString() + " pts"; ;
            TransactionText = transactionType;
        }

        //public void UpdateCellData(TransactionModel transaction)
        //{
        //    lblTransactionText.TextAlignment = UITextAlignment.Left;
        //    lblTransactionText.LineBreakMode = UILineBreakMode.WordWrap;

        //    TransactionImage.Image = UIImage.FromBundle("challenges-icon_off");
        //    //lblTransactionText.Text = _lorem.Substring(StringLength);

        //    TimeText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);
        //    PointsText.Font = UIFont.FromName("ProximaNova-Bold", SizeConstants.ScreenMultiplier * 16);
        //    lblTransactionText.Font = UIFont.FromName("ProximaNova-Regular", SizeConstants.ScreenMultiplier * 14);

        //    if (transaction == null)
        //    {
        //        return;
        //    }

        //    TransactionImage.Image = UIImage.FromBundle("challenges-icon_off");
        //    TimeText.Text = SL.TimeAgoShort(transaction.TransactionDate);
        //    PointsText.Text = (transaction.TransactionValue > 0 ? "+" : "") + transaction.TransactionValue.ToString() + " pts";
        //    lblTransactionText.Text = transaction.TransactionType;
        //}
    }
}
