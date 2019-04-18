using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Graphics.Drawable;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SocialLadder.Droid.CustomControls
{
    public class GalleryImageView : ImageView
    {
        public GalleryImageView(Context context) : base(context)
        {
        }

        public GalleryImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public GalleryImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public GalleryImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected GalleryImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private byte[] _rawImage;
        public byte[] RawImage
        {
            get
            {
                return _rawImage;
            }
            set
            {
                _rawImage = value;
                if (_rawImage == null)
                    return;

                var bitmap = BitmapFactory.DecodeByteArray(_rawImage, 0, _rawImage.Length); 
                //Canvas canvas = new Canvas(bitmap);

                //Paint paint = new Paint();
                //Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
                //RectF rectF = new RectF(rect);
                //float roundPx = 10;

                //paint.AntiAlias = (true);
                //canvas.DrawARGB(0, 0, 0, 0);
                //paint.Color = Color.Red;
                //canvas.DrawRoundRect(rectF, roundPx, roundPx, paint);

                //paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
                //canvas.DrawBitmap(bitmap, rect, rect, paint); 
                //SetImageDrawable(bitmap);

                SetImageBitmap(bitmap);
            }
        }
        private byte[] _roundedrawImage;
        public byte[] RoundedRawImage
        {
            get
            {
                return _rawImage;
            }
            set
            {
                _rawImage = value;
                if (_rawImage == null)
                    return;

                var bitmap = BitmapFactory.DecodeByteArray(_rawImage, 0, _rawImage.Length);

                var reoundedBitmap = RoundedBitmapDrawableFactory.Create(Resources, bitmap);

                reoundedBitmap.CornerRadius = 50f;
                //Canvas canvas = new Canvas(bitmap);

                //Paint paint = new Paint();
                //Rect rect = new Rect(0, 0, bitmap.Width, bitmap.Height);
                //RectF rectF = new RectF(rect);
                //float roundPx = 10;

                //paint.AntiAlias = (true);
                //canvas.DrawARGB(0, 0, 0, 0);
                //paint.Color = Color.Red;
                //canvas.DrawRoundRect(rectF, roundPx, roundPx, paint);

                //paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
                //canvas.DrawBitmap(bitmap, rect, rect, paint);
                reoundedBitmap.SetAntiAlias(true);
                SetImageDrawable(reoundedBitmap);

                //SetImageBitmap(bitmap);
            }
        }
    }
}