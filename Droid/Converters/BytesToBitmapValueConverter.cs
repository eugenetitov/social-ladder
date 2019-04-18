using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;

namespace SocialLadder.Droid.Converters
{
    public class BytesToBitmapValueConverter : MvxValueConverter<byte[], Bitmap>
    {
        protected override Bitmap Convert(byte[] value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            byte[] _value = value as byte[];
            Bitmap bm = BitmapFactory.DecodeByteArray(_value, 0, _value.Length);
            return bm;
        }

        protected override byte[] ConvertBack(Bitmap value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new byte[0];
            }
            byte[] bitmapData;
            Bitmap bitmap = value as Bitmap;
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }
            return bitmapData;
        }
    }
}