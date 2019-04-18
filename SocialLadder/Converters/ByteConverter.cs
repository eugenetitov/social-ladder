using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialLadder.Converters
{
    public class ByteConverter
    {
        public string ConvertFromByteToString(string strInByte)
        {
            try
            {
                var convertStr = string.Join("-", System.Text.RegularExpressions.Regex.Matches(strInByte, @"..").Cast<System.Text.RegularExpressions.Match>().ToList());
                string[] tempArr = convertStr.Split('-');
                byte[] decBytes = new byte[tempArr.Length];
                for (int i = 0; i < tempArr.Length; i++)
                {
                    decBytes[i] = System.Convert.ToByte(tempArr[i], 16);
                }
                string strWithEmoji = Encoding.BigEndianUnicode.GetString(decBytes, 0, decBytes.Length);
                return strWithEmoji;
            }
            catch (FormatException)
            {
                return strInByte;
            }
        }
    }
}