using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Content.Res;
using static Android.Graphics.Path;
using static Android.Graphics.Paint;

namespace SocialLadder.Droid.Views
{
    [Register("socialLadder.droid.views.TriangePointsView")]
    public class TriangePointsView : View
    {
        private static float density =  Resources.System.DisplayMetrics.ScaledDensity;
        private int screenWidth;
        private int width, height;
        private readonly int HeightDp = 100;
        private double Filling { get; set; }
        private List<string> chartText = new List<string> { "Fan", "Superfan", "Ambassador", "Champion" };
        private List<double> chartTextPosition = new List<double> { 0.1, 0.4, 0.7, 1.0 };
        private double topMargin = 0.15;
        private double topTextMargin = 0.06;
        private double leftMargin = 0.07;
        private double widthPercent = 0.86;
        private double heightPercent = 0.75;
        private double textPercentHeight = 0.1;

        public TriangePointsView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public TriangePointsView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public void SetParameters(double filling, double width, double height)
        {
            this.Filling = filling;
            screenWidth = ((int)width > 0 ? (int)width : Resources.System.DisplayMetrics.WidthPixels);
            this.width = (int)Math.Ceiling(screenWidth * widthPercent);
            this.height = ((int)height > 0 ? (int)height : (int)Math.Ceiling(HeightDp * density));
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Paint paint = new Paint();

            paint.StrokeWidth = 2;
            
            paint.Color = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.custom_triangle_background_color));
            paint.SetStyle(Paint.Style.FillAndStroke);
            paint.AntiAlias = true;

            paint.SetPathEffect(new CornerPathEffect((float)(HeightDp * 0.1)));
            var leftCathetusHeight = (int)Math.Ceiling((height * heightPercent * leftMargin) / (leftMargin + widthPercent));

            Point a = new Point((int)Math.Ceiling(screenWidth * leftMargin), (int)Math.Ceiling(height * (topMargin + heightPercent)));
            Point b = new Point(width + a.X, (int)Math.Ceiling(height * (topMargin + heightPercent)));
            Point c = new Point(width + a.X, (int)Math.Ceiling(height * topMargin));
            Point d = new Point((int)Math.Ceiling(screenWidth * leftMargin), (int)Math.Ceiling(height * (topMargin + heightPercent) - leftCathetusHeight));

            canvas.DrawPath(GetTrianglePath(a,b,c,d), paint);

            Point a1 = new Point((int)Math.Ceiling(screenWidth * leftMargin), (int)Math.Ceiling(height * (topMargin + heightPercent)));
            Point b1 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * Filling), (int)Math.Ceiling(height * (topMargin + heightPercent)));
            Point c1 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * Filling), GetCorrectHeight(Filling));
            Point d1 = new Point((int)Math.Ceiling(screenWidth * leftMargin), (int)Math.Ceiling(height * (topMargin + heightPercent) - leftCathetusHeight));

            
            paint.Color = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.custom_triangle_body_color));
            canvas.DrawPath(GetTrianglePath(a1,b1,c1,d1), paint);

            paint.SetPathEffect(new CornerPathEffect(0));

            Point a2 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[2]), (int)Math.Ceiling(height * (topMargin + heightPercent)));
            Point b2 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[2]), GetCorrectHeight(chartTextPosition[2]));
            canvas.DrawPath(GetLinePath(a2, b2), ChangeLineColor(paint, chartTextPosition[2]));

            Point a3 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[1]), (int)Math.Ceiling(height * (topMargin + heightPercent)));
            Point b3 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[1]), GetCorrectHeight(chartTextPosition[1]));
            canvas.DrawPath(GetLinePath(a3, b3), ChangeLineColor(paint, chartTextPosition[1]));

            Point a4 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[0]), (int)Math.Ceiling(height * (topMargin + heightPercent)));
            Point b4 = new Point((int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[0]), GetCorrectHeight(chartTextPosition[0]));
            canvas.DrawPath(GetLinePath(a4, b4), ChangeLineColor(paint, chartTextPosition[0]));

            canvas.DrawText(chartText[0], (int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[0]), GetCorrectHeight(chartTextPosition[0] + topTextMargin), GetTextPaint());
            canvas.DrawText(chartText[1], (int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[1]), GetCorrectHeight(chartTextPosition[1] + topTextMargin), GetTextPaint());
            canvas.DrawText(chartText[2], (int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[2]), GetCorrectHeight(chartTextPosition[2] + topTextMargin), GetTextPaint());
            canvas.DrawText(chartText[3], (int)Math.Ceiling(screenWidth * leftMargin + width * chartTextPosition[3]), GetCorrectHeight(chartTextPosition[3] + topTextMargin), GetTextPaint());
        }

        private Paint GetTextPaint()
        {
            var paint = new Paint();
            paint.TextSize = (float)(height * textPercentHeight);
            paint.TextScaleX = 1;
            paint.AntiAlias = true;
            paint.TextAlign = Align.Right;
            paint.Color = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.areas_description_tex_color));
            return paint;
        }

        private Paint ChangeLineColor(Paint paint, double koef)
        {
            if (Filling >= koef)
            {      
                paint.Color = new Color(ContextCompat.GetColor(Application.Context, Resource.Color.custom_triangle_line_color));
            }
            if (Filling < koef)
            {
                paint.Color = Android.Graphics.Color.White;
            }
            return paint;
        }

        private int GetCorrectHeight(double filling)
        {
            var fillingCorrect = ((screenWidth * leftMargin + width * filling) / width * (leftMargin + widthPercent));
            var correctedHeight = (int)Math.Ceiling(height * (topMargin + heightPercent) - (height * heightPercent * fillingCorrect));
            return correctedHeight;
        }

        private Path GetLinePath(Point point1, Point point2)
        {
            Path path = new Path();
            path.SetFillType(FillType.EvenOdd);
            path.MoveTo(point1.X, point1.Y);
            path.LineTo(point2.X, point2.Y);
            path.Close();
            return path;
        }

        private Path GetTrianglePath(Point point1, Point point2, Point point3, Point point4)
        {
            Path path = new Path();
            path.SetFillType(FillType.EvenOdd);
            path.MoveTo(point1.X, point1.Y);
            path.LineTo(point2.X, point2.Y);
            path.LineTo(point3.X, point3.Y);
            path.LineTo(point4.X, point4.Y);
            path.LineTo(point1.X, point1.Y);
            path.Close();
            return path;
        }
    }
}