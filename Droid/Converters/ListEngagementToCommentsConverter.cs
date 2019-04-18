using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
using SocialLadder.Droid.Helpers;
using SocialLadder.Models;

namespace SocialLadder.Droid.Converters
{
    public class ListEngagementToCommentsConverter : MvxValueConverter<List<FeedEngagementModel>, ISpanned>
    {
        protected override ISpanned Convert(List<FeedEngagementModel> engagementList, Type targetType, object parameter, CultureInfo culture)
        {
            int allowedTotal = 3;
            if (parameter != null)
            {
                allowedTotal = (Int32)parameter;
            }
            if (engagementList == null)
            {
                return CommentsHelper.BuildComments(engagementList);
            }
            List<FeedEngagementModel> commentList = new List<FeedEngagementModel>();
            foreach (FeedEngagementModel engagement in engagementList)
            {
                if (commentList.Count == allowedTotal)
                    break;
                var ContainErrorEmoji = engagement.Notes.Contains('?');
                if (!ContainErrorEmoji)
                {
                    try
                    {
                        var convertStr = string.Join("-", System.Text.RegularExpressions.Regex.Matches(engagement.Notes, @"..").Cast<System.Text.RegularExpressions.Match>().ToList());
                        string[] tempArr = convertStr.Split('-');
                        byte[] decBytes = new byte[tempArr.Length];
                        for (int i = 0; i < tempArr.Length; i++)
                        {
                            decBytes[i] = System.Convert.ToByte(tempArr[i], 16);
                        }
                        string strWithEmoji = Encoding.BigEndianUnicode.GetString(decBytes, 0, decBytes.Length);
                        engagement.Notes = strWithEmoji;
                        commentList.Add(engagement);
                    }
                    catch(FormatException)
                    {
                        commentList.Add(engagement);
                    }
                }
                else if (engagement.EngagementType == "COMMENT")
                    commentList.Add(engagement);
            }

           return CommentsHelper.BuildComments(commentList);
        }
    }
}