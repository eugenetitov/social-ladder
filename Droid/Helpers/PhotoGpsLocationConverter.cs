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

namespace SocialLadder.Droid.Helpers
{
    public static class PhotoGpsLocationConverter
    {
        public static double CheckAndConvert(string gpsValue, string gpsValuePer, bool isLatitude)
        {
            double value = 0;
            if (string.IsNullOrEmpty(gpsValue) || string.IsNullOrEmpty(gpsValuePer))
            {
                return value;
            }
            if (isLatitude)
            {
                value = gpsValuePer.Equals("N") ? ConvertToDegree(gpsValue) : 0 - ConvertToDegree(gpsValue);
            }
            if (!isLatitude)
            {
                value = gpsValuePer.Equals("E") ? ConvertToDegree(gpsValue) : 0 - ConvertToDegree(gpsValue);
            }
            return value;
        }

        private static double ConvertToDegree(string stringDMS)
        {
            string[] DMS = stringDMS.Split(",".ToCharArray(), 3);

            string[] stringD = DMS[0].Split("/".ToCharArray(), 2);
            double D0 = double.Parse(stringD[0]);
            double D1 = double.Parse(stringD[1]);
            double FloatD = D0 / D1;

            string[] stringM = DMS[1].Split("/".ToCharArray(), 2);
            double M0 = double.Parse(stringM[0]);
            double M1 = double.Parse(stringM[1]);
            double FloatM = M0 / M1;

            string[] stringS = DMS[2].Split("/".ToCharArray(), 2);
            double S0 = double.Parse(stringS[0]);
            double S1 = double.Parse(stringS[1]);
            double FloatS = S0 / S1;

            var result = FloatD + (FloatM / 60) + (FloatS / 3600);
            return result;
        }
    }
}