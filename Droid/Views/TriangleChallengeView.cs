using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content.Res;
using static Android.Graphics.Path;
using static Android.Graphics.Paint;
using Android.Graphics;
using Android.Support.V4.Content;
using SocialLadder.ViewModels.Base;

namespace SocialLadder.Droid.Views
{
    [Register("socialLadder.droid.views.TriangleChallengeView")]
    public class TriangleChallengeView : ImageView
    {
        private static float density = Resources.System.DisplayMetrics.ScaledDensity;
        private int spaceWidth;
        private int width, height;
        private readonly int HeightDp = 100;
        private double topMargin = 0;
        private double leftMargin = 0;
        private double widthPercent = 1.0;
        private double heightPercent = 1.0;
        private float borderSideWidth, borderBottomWidth;
        private BaseChallengesViewModel ViewModel { get; set; }
        private Bitmap bitmap;

        public TriangleChallengeView(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }

        public void SetParameters(double width, double height, BaseChallengesViewModel vm)
        {
            spaceWidth = ((int)width > 0 ? (int)width : (int)Math.Ceiling(Resources.System.DisplayMetrics.WidthPixels * 0.3));
            this.width = (int)Math.Ceiling(spaceWidth * widthPercent);
            this.height = ((int)height > 0 ? (int)height : (int)Math.Ceiling(Resources.System.DisplayMetrics.WidthPixels * 0.3 * 0.866));
            borderBottomWidth = spaceWidth * 0.015f;
            borderSideWidth = spaceWidth * 0.02f;
            ViewModel = vm;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Paint paint = new Paint();
            paint.Color = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.textIcon));
            paint.SetStyle(Paint.Style.Fill);
            paint.AntiAlias = true;
            paint.SetPathEffect(new CornerPathEffect((float)(HeightDp * 0.1)));

            canvas.DrawPath(GetTrianglePath(0,0), paint);

            GetBitmap();

            var scaledBitmap = Bitmap.CreateScaledBitmap(bitmap, spaceWidth, height, false);

            Rect rect = new Rect(0, 0, spaceWidth, height);

            var path = GetTrianglePath(borderSideWidth, borderBottomWidth);
            canvas.DrawPath(path, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
            canvas.ClipPath(path);
            canvas.DrawBitmap(scaledBitmap, rect, rect, paint);
        }

        private void GetBitmap()
        {
            var array = ViewModel.GetArrayImage();
            if (array != null)
            {
                bitmap = BitmapFactory.DecodeByteArray(array, 0, array.Length);
            }
            else
            {
                bitmap = BitmapFactory.DecodeResource(Application.Context.Resources, (int)typeof(Resource.Drawable).GetField("Challenges_Parallax_Background").GetValue(null));
            }
        }

        #region CreatePath
        private Point[] GetPoints(float borderSideWidth, float borderBottomWidth)
        {
            Point a = new Point((int)Math.Ceiling(spaceWidth * leftMargin + borderSideWidth), (int)Math.Ceiling(height * (topMargin + heightPercent) - borderBottomWidth));
            Point b = new Point((int)Math.Ceiling(width + spaceWidth * leftMargin - borderSideWidth), (int)Math.Ceiling(height * (topMargin + heightPercent) - borderBottomWidth));
            Point c = new Point(width / 2, (int)Math.Ceiling(height * topMargin + borderSideWidth));
            return new Point[] { a,b,c };
        }

        private Path GetTrianglePath(float sideWidth, float bottomWidth)
        {
            var points = GetPoints(sideWidth, bottomWidth);
            Path path = new Path();
            path.SetFillType(FillType.EvenOdd);
            path.MoveTo(points[0].X, points[0].Y);
            path.LineTo(points[1].X, points[1].Y);
            path.LineTo(points[2].X, points[2].Y);
            path.LineTo(points[0].X, points[0].Y);
            path.Close();
            return path;
        }
        #endregion
    }
}