using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform;
using SocialLadder.Droid.Services;
using SocialLadder.Interfaces;
using SocialLadder.Models;

namespace SocialLadder.Droid.Helpers
{
    public static class CommentsHelper
    {
        public static SpannableStringBuilder BuildComments(List<FeedEngagementModel> commentList)
        {
            Mvx.RegisterType<IEncodingService, EncodingService>();
            var encodingService = Mvx.Resolve<IEncodingService>();
            SpannableStringBuilder sb = new SpannableStringBuilder();
            for (int i = 0; i < commentList.Count; i++)
            {
                string commentText = string.Empty;
                FeedEngagementModel comment = commentList[i];

                if(!string.IsNullOrEmpty(comment.UserName))
                {
                    try
                    {
                        var decodingNameUser = encodingService.DecodeFromNonLossyAscii(comment.UserName);
                        comment.UserName = decodingNameUser;
                    }
                    catch (FormatException)
                    {
                    }
                }

                if(!string.IsNullOrEmpty(comment.Notes))
                {
                    try
                    {
                        var decodingNameUser = encodingService.DecodeFromNonLossyAscii(comment.UserName);
                        comment.UserName = decodingNameUser;
                    }
                    catch (FormatException)
                    {
                    }
                }

                var start = sb.Length();
                var end = start + comment.UserName.Length;
                sb.Append(comment.UserName);
                sb.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold), start, end, SpanTypes.ExclusiveExclusive);
                sb.SetSpan(new ForegroundColorSpan(Color.Black), start, end, SpanTypes.ExclusiveExclusive);
                commentText += " " + comment.Notes;
                sb.Append(commentText);
                if (i + 1 < commentList.Count)
                    sb.Append("\n");
            }
            return sb;
        }

        public static SpannableStringBuilder BuildCommentsWithEmoji(List<FeedEngagementModel> engagementList)
        {
            Mvx.RegisterType<IEncodingService, EncodingService>();
            var encodingService = Mvx.Resolve<IEncodingService>();
            if (engagementList == null)
            {
                return BuildComments(engagementList);
            }
            List<FeedEngagementModel> namesList = new List<FeedEngagementModel>();
            List<FeedEngagementModel> commentList = new List<FeedEngagementModel>();
            foreach (FeedEngagementModel engagement in engagementList)
            {
                    try
                    {
                        var decodingNameUser = encodingService.DecodeFromNonLossyAscii(engagement.Notes);
                        engagement.UserName = decodingNameUser;
                        namesList.Add(engagement);
                    }
                    catch(FormatException)
                    {
                    namesList.Add(engagement);
                    }
            }

            foreach(FeedEngagementModel engagement in namesList)
            {
                    try
                    {
                        var decodingNameUser = encodingService.DecodeFromNonLossyAscii(engagement.Notes);
                        engagement.UserName = decodingNameUser;
                        namesList.Add(engagement);
                    }
                    catch (FormatException)
                    {
                        commentList.Add(engagement);
                    }
            }

            return 
                BuildComments(commentList);
        }
    }
}