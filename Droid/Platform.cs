using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Android.Content.Res;
using Android.Graphics;
using Android.Widget;

namespace SocialLadder.Droid
{
    public class Platform
    {
        public static string LocalDataBasePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "DatabaseName.txt");

        public Platform()
        {
        }

        public static double Lat
        {
            get
            {
                return 0;
            }
        }

        public static double Lon
        {
            get
            {
                return 0;
            }
        }

        public static void ClearBrowserCache()
        {
          
        }

        //imageView.SetImageURI(Android.Net.Uri.Parse(""));
        public static async Task<Bitmap> LoadImage(string imageUrl, ImageView view = null)
        {
            Bitmap image = null;
            try
            {
                byte[] contents = await SL.LoadImage(imageUrl);
                image = BitmapFactory.DecodeByteArray(contents, 0, contents.Length);
                if (view != null)
                {
                    view.SetImageBitmap(image);
                    //view.RequestLayout();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"              ERROR {0}", ex.Message);
            }
            return image;
        }

        public static int ScreenWidth
        {
            get
            {
               return  Resources.System.DisplayMetrics.WidthPixels;
            }
        }

        public static int ScreenHeight
        {
            get
            {
                return Resources.System.DisplayMetrics.HeightPixels;
            }
        }
    }
}
