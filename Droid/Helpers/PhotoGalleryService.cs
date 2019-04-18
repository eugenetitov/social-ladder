using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.Media;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;
using SocialLadder;
using SocialLadder.Droid.Helpers;
using SocialLadder.Droid.Models;
using SocialLadder.Droid.Services;
using SocialLadder.Enums.Constants;
using SocialLadder.Interfaces;
using static Android.Content.PM.PackageManager;
using static Android.Provider.MediaStore.Images;

namespace SocialLadder.Droid.Helpers
{
    public class PhotoGalleryService
    {
        public MvxFragment Fragment { get; set; }
        private ImageButton ImageButton { get; set; }
        private bool UseFrontCamera { get; set; }
        private byte[] byteArray;
        private readonly string pathToNewPhoto = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/User_photo";
        private Android.Net.Uri imageUri;
        public bool UserImageWasAdded { get; set; } = false;
        private bool NeedCrop { get; set; }
        public double imageLat, imageLong;

        public PhotoGalleryService(MvxFragment fragment, ImageButton imageButton, bool useFrontCamera = false)
        {
            imageLat = imageLong = 0;
            Fragment = fragment;
            ImageButton = imageButton;
            UseFrontCamera = useFrontCamera;
            NeedCrop = true;
            PermissionHelper.AddPermission(Fragment.Activity, new List<string> { Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera });
            if (ImageButton == null)
            {
                return;
            }
            ImageButton.Click += (s, e) =>
            {
                if (!AppInstalled(fragment.Context, "com.instagram.android"))
                {
                    AlertDialog.Builder alertDialog = new AlertDialog.Builder(fragment.Context);
                    alertDialog.SetMessage("Unable to find the Instagram App on your device. This challenge requires Instagram.");
                    alertDialog.SetPositiveButton("OK", (sender, args) =>
                    {
                    });
                    Dialog dialod = alertDialog.Create();
                    dialod.Show();

                }
                else if ((Fragment.ViewModel as IChallengeShareViewModel).CheckNetworkConnected())
                {
                    BtnUpdatePhoto();
                }
            };
        }

        public void BtnUpdatePhoto()
        {
            var popupMenu = new PopupMenu(Fragment.Activity, ImageButton);
            if (!UserImageWasAdded)
            {
                popupMenu.Menu.Add(ChallengesConstants.Camera);
                popupMenu.Menu.Add(ChallengesConstants.Library);
            }
            else
            {
                popupMenu.Menu.Add(ChallengesConstants.SendToInstagram);
            }
            popupMenu.Menu.Add(ChallengesConstants.Cancel);
            popupMenu.MenuItemClick += OnMenuItemCameraClicked;
            popupMenu.Show();
        }

        public Intent CroupImage(Intent data)
        {
            data.PutExtra("crop", "true");
            data.PutExtra("aspectX", 1);
            data.PutExtra("aspectY", 1);
            data.PutExtra("outputX", 280);
            data.PutExtra("outputY", 280);
            data.PutExtra("return-data", true);
            data.PutExtra("scaleUpIfNees", true);
            data.PutExtra("return-data", true);

            return data;
        }

        public void SetImageUriFromFragment(string imagePath, bool needCrope = false)
        {
            imageUri = Android.Net.Uri.FromFile(new Java.IO.File(imagePath));
            NeedCrop = needCrope;
        }

        public byte[] OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            try
            {
                if (resultCode != (int)Result.Ok)
                {
                    UserImageWasAdded = false;
                    return null;
                }
                if (requestCode == AndroidRequestCode.CameraRequestCode)
                {
                    var bitmap = CheckRotation(imageUri, null);
                    bitmap = Bitmap.CreateScaledBitmap(bitmap, bitmap.Width, bitmap.Height, true);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        byteArray = stream.ToArray();
                        return byteArray;
                    }
                }
                if (requestCode == AndroidRequestCode.GalleryRequestCode)
                {
                    var bitmap = MediaStore.Images.Media.GetBitmap(Fragment.Activity.ContentResolver, data.Data);
                    bitmap = CheckRotation(data.Data, bitmap);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                        byteArray = stream.ToArray();
                        return byteArray;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
            UserImageWasAdded = false;
            return null;
        }

        public void OnMenuItemCameraClicked(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            Java.IO.File file = new Java.IO.File(pathToNewPhoto);
            bool deleted = file.Delete();


            var label = e.Item.TitleFormatted.ToString();
            if (label == ChallengesConstants.Camera)
            {
                if (!PermissionHelper.CheckPermissions(Fragment.Activity, new List<string> { Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera }))
                {
                    var _alertService = Mvx.Resolve<IAlertService>();
                    _alertService.ShowToast("Please, go to settings and turn on access to camera for this app");
                }
                else
                    GetImageFromCamera();
                return;
            }
            if (label == ChallengesConstants.Library)
            {
                if (!PermissionHelper.CheckPermissions(Fragment.Activity, new List<string> { Manifest.Permission.WriteExternalStorage }))
                {
                    var _alertService = Mvx.Resolve<IAlertService>();
                    _alertService.ShowToast("Please, go to settings and turn on access to gallery for this app");
                }
                else
                {
                    var intent = new Intent(Intent.ActionPick);
                    intent.SetType("image/*");
                    intent.SetAction(Intent.ActionGetContent);
                    UserImageWasAdded = true;
                    Fragment.StartActivityForResult(intent, AndroidRequestCode.GalleryRequestCode);
                }
            }
            if (label == ChallengesConstants.SendToInstagram)
            {
                (Fragment.ViewModel as IChallengeShareViewModel).ShareToSocialNetwork();
            }
            if (label == ChallengesConstants.Cancel)
            {
                return;
            }
        }

        public void GetImageFromCamera()
        {
            if (!PermissionHelper.CheckPermissions(Fragment.Activity, new List<string> { Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera }))
            {
                (Fragment.ViewModel as IChallengeShareViewModel).ShowToastIfPermissionsDenided();
                return;
            }
            NeedCrop = false;
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            var intent = new Intent(MediaStore.ActionImageCapture);
            imageUri = Android.Net.Uri.FromFile(new Java.IO.File(pathToNewPhoto));
            intent.PutExtra(MediaStore.ExtraOutput, imageUri);
            intent.PutExtra("android.intent.extras.CAMERA_FACING", (int)Android.Hardware.Camera.CameraInfo.CameraFacingBack);
            intent.PutExtra("android.intent.extras.LENS_FACING_FRONT", 0);
            if (UseFrontCamera)
            {
                intent.PutExtra("android.intent.extra.USE_FRONT_CAMERA", false);
            }
            if (!UseFrontCamera)
            {
                intent.PutExtra("android.intent.extra.USE_FRONT_CAMERA", false);
            }
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            UserImageWasAdded = true;
            Fragment.StartActivityForResult(intent, AndroidRequestCode.CameraRequestCode);
        }

        public void SendToInstagram()
        {
            (Fragment.ViewModel as IChallengeShareViewModel).ShareToSocialNetwork();
        }

        public Bitmap CroppBitmap(Bitmap croppedBitmap)
        {
            if (!NeedCrop)
            {
                return croppedBitmap;
            }
            Bitmap rotatedReturnedBitmap = null;
            if (croppedBitmap.Width >= croppedBitmap.Height)
            {
                rotatedReturnedBitmap = Bitmap.CreateBitmap(
                   croppedBitmap,
                   croppedBitmap.Width / 2 - croppedBitmap.Height / 2,
                   0,
                   croppedBitmap.Height,
                   croppedBitmap.Height
                   );
            }
            else
            {
                rotatedReturnedBitmap = Bitmap.CreateBitmap(
                   croppedBitmap,
                   0,
                   croppedBitmap.Height / 2 - croppedBitmap.Width / 2,
                   croppedBitmap.Width,
                   croppedBitmap.Width
                   );
            }
            return rotatedReturnedBitmap;
        }

        public Bitmap RotatePhoto(Bitmap image, int width, int height, float degrees)
        {
            var matrix = new Matrix();
            var scaleWidth = ((float)width) / image.Width;
            var scaleHeight = ((float)height) / image.Height;
            matrix.PostRotate(degrees);
            matrix.PreScale(scaleWidth, scaleHeight);
            return Bitmap.CreateBitmap(image, 0, 0, image.Width, image.Height, matrix, true);
        }

        private Bitmap CheckRotation(Android.Net.Uri imageUri, Bitmap bitmap)
        {
            using (System.IO.Stream input = Fragment.Context.ContentResolver.OpenInputStream(imageUri))
            {
                //System.IO.Stream input = Fragment.Context.ContentResolver.OpenInputStream(imageUri);
                var exifInterface = new ExifInterface(input);
                TryGetLocation(exifInterface);
                int orientation = exifInterface.GetAttributeInt(ExifInterface.TagOrientation, ExifInterface.OrientationFlipHorizontal);
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InSampleSize = 8;
                Bitmap rotatedBitmap = null;
                if (bitmap != null)
                {
                    return CroppBitmap(ExecuteRotation(bitmap, orientation));
                }
                using (bitmap = BitmapFactory.DecodeFile(imageUri.Path, options))
                {
                    rotatedBitmap = ExecuteRotation(bitmap, orientation);
                }
                return CroppBitmap(rotatedBitmap);
            }
        }

        //public int CheckRotationByExifInterface(string path)
        //{
        //    imageUri = Android.Net.Uri.FromFile(new Java.IO.File(path));
        //    System.IO.Stream input = Fragment.Context.ContentResolver.OpenInputStream(imageUri);
        //    var exifInterface = new ExifInterface(input);
        //    TryGetLocation(exifInterface);
        //    int orientation = exifInterface.GetAttributeInt(ExifInterface.TagOrientation, ExifInterface.OrientationFlipHorizontal);
        //    return orientation;
        //}

        private void TryGetLocation(ExifInterface exifInterface)
        {
            try
            {
                imageLat = 0;
                imageLong = 0;
                var _gps_Latitude = exifInterface.GetAttribute(ExifInterface.TagGpsLatitude);
                var _gps_Longitude = exifInterface.GetAttribute(ExifInterface.TagGpsLongitude);
                var _gps_LatitudeRef = exifInterface.GetAttribute(ExifInterface.TagGpsLatitudeRef);
                var _gps_LongitudeRef = exifInterface.GetAttribute(ExifInterface.TagGpsLongitudeRef);
                imageLat = PhotoGpsLocationConverter.CheckAndConvert(_gps_Latitude, _gps_LatitudeRef, true);
                imageLong = PhotoGpsLocationConverter.CheckAndConvert(_gps_Longitude, _gps_LongitudeRef, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Can't get image location: " + ex.Message);
            }
        }

            private Bitmap ExecuteRotation(Bitmap bitmap, int orientation)
            {
                Bitmap rotatedBitmap = null;
                if (orientation == ExifInterface.OrientationRotate90)
                {
                    rotatedBitmap = RotatePhoto(bitmap, bitmap.Width, bitmap.Height, 90);
                }
                if (orientation == ExifInterface.OrientationRotate180)
                {
                    rotatedBitmap = RotatePhoto(bitmap, bitmap.Width, bitmap.Height, 180);
                }
                if (orientation == ExifInterface.OrientationRotate270)
                {
                    rotatedBitmap = RotatePhoto(bitmap, bitmap.Width, bitmap.Height, 270);
                }
                if (orientation == ExifInterface.OrientationNormal || orientation == ExifInterface.OrientationUndefined)
                {
                    rotatedBitmap = RotatePhoto(bitmap, bitmap.Width, bitmap.Height, 180);
                    rotatedBitmap = RotatePhoto(rotatedBitmap, bitmap.Width, bitmap.Height, 180);
                }
                return rotatedBitmap;
            }

            private bool AppInstalled(Context c, string targetPackage)
            {
                PackageManager pm = c.PackageManager;
                try
                {
                    PackageInfo info = pm.GetPackageInfo(targetPackage, PackageInfoFlags.MetaData);
                }
                catch (NameNotFoundException e)
                {
                    return false;
                }
                return true;
            }
        }
    }