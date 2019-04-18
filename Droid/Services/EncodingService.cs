using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Nio.Charset;
using Java.Util.Regex;
using SocialLadder.Interfaces;

namespace SocialLadder.Droid.Services
{
    public class EncodingService : IEncodingService
    {
        public System.String EncodeToNonLossyAscii(System.String original)
        {
            Charset asciiCharset = Charset.ForName("US-ASCII");
            if (asciiCharset.NewEncoder().CanEncode(original))
            {
                return original;
            }
            StringBuffer stringBuffer = new StringBuffer();
            for (int i = 0; i < original.Length; i++)
            {
                char c = original.ElementAt(i);
                if (c < 128)
                {
                    stringBuffer.Append(c.ToString());
                }
                else if (c < 256)
                {
                    System.String octal = Integer.ToOctalString(c);
                    stringBuffer.Append("\\");
                    stringBuffer.Append(octal);
                }
                else
                {
                    System.String hex = Integer.ToHexString(c);
                    stringBuffer.Append("\\u");
                    stringBuffer.Append(hex);
                }
            }
            return stringBuffer.ToString();
        }

        public System.String DecodeFromNonLossyAscii(System.String original)
        {
                Java.Util.Regex.Pattern UNICODE_HEX_PATTERN = Java.Util.Regex.Pattern.Compile("\\\\u([0-9A-Fa-f]{4})");
                Java.Util.Regex.Pattern UNICODE_OCT_PATTERN = Java.Util.Regex.Pattern.Compile("\\\\([0 - 7]{3})");
                Matcher matcher = UNICODE_HEX_PATTERN.Matcher(original);
                StringBuffer charBuffer = new StringBuffer(original.Length);
                while (matcher.Find())
                {
                    System.String match = matcher.Group(1);
                    char unicodeChar = (char)Integer.ParseInt(match, 16);
                    matcher.AppendReplacement(charBuffer, Character.ToString(unicodeChar));
                }
                matcher.AppendTail(charBuffer);
                System.String parsedUnicode = charBuffer.ToString();

                matcher = UNICODE_OCT_PATTERN.Matcher(parsedUnicode);
                charBuffer = new StringBuffer(parsedUnicode.Length);
                while (matcher.Find())
                {
                    System.String match = matcher.Group(1);
                    char unicodeChar = (char)Integer.ParseInt(match, 8);
                    matcher.AppendReplacement(charBuffer, Character.ToString(unicodeChar));
                }
                matcher.AppendTail(charBuffer);
                return charBuffer.ToString();
        }
    }
}