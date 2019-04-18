using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace SocialLadder.iOS.Helpers
{
    public static class StringWithEmojiConverter
    {
        public static string ConvertEmojiToServer(string stringWithEmoji)
        {
            if (String.IsNullOrEmpty(stringWithEmoji))
                return String.Empty;

            var tempString = stringWithEmoji;
            for (int i = tempString.Length; i != 0; tempString = tempString.Substring(0, tempString.Length - 1))
            {
                NSData data = NSData.FromString(stringWithEmoji, NSStringEncoding.NonLossyASCII);
                NSString valueUnicode = NSString.FromData(data, NSStringEncoding.UTF8);
                if (valueUnicode != null)
                {
                    return valueUnicode.ToString();
                }
            }
            return stringWithEmoji;
        }


        public static string ConvertEmojiFromServer(string stringWithEmoji)
        {
            if (String.IsNullOrEmpty(stringWithEmoji))
                return String.Empty;

            var tempString = stringWithEmoji;
            for (int i = tempString.Length; i != 0; tempString = tempString.Substring(0, tempString.Length - 1))
            {
                NSData data = NSData.FromString(tempString, NSStringEncoding.UTF8);
                NSString valueUnicode = NSString.FromData(data, NSStringEncoding.NonLossyASCII);
                if (valueUnicode != null)
                {
                    return valueUnicode.ToString();
                }
            }
            return stringWithEmoji;
        }
    }
}