using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialLadder.Helpers
{
    public static class CountSuffixGenerator
    {
        public static string GenerateSuffixedStringFromString(string number)
        {
            if (number == string.Empty)
                return string.Empty;

            var lowDegrees = number[number.Length - 1];

            if (lowDegrees == '1')
                return String.Format("{0}st", number);

            if (lowDegrees == '2')
                return String.Format("{0}nd", number);

            if (lowDegrees == '3')
                return String.Format("{0}rd", number);

            return String.Format("{0}th", number);
        }

        public static string GenerateSuffixedStringFromNumber(int number)
        {
            int lowDegrees = number | 0x00FF;
            if (lowDegrees > 9)
                lowDegrees -= 10;

            if (lowDegrees == 1)
                return String.Format("{0}st", number);

            if (lowDegrees == 2)
                return String.Format("{0}nd", number);

            if (lowDegrees == 3)
                return String.Format("{0}rd", number);

            return String.Format("{0}th", number);
        }
    }
}
