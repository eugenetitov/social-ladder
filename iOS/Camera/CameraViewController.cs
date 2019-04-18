using System;
using GMImagePicker;
using MobileCoreServices;
using SocialLadder.iOS.Challenges;
using SocialLadder.iOS.Delegates;
using UIKit;

namespace SocialLadder.iOS.Camera
{
    public class CameraViewController : ChallengeDetailBaseViewController
    {
        static UIImagePickerController picker;
        public bool _didPicked { get; set; } = false;

        public CameraViewController(IntPtr handle) : base(handle)
        {
        }

        void Pick(UIImagePickerControllerSourceType type)
        {
            if (UIImagePickerController.IsSourceTypeAvailable(type))
            {
                picker = new UIImagePickerController();
                picker.Delegate = new CameraDelegate(this);
                picker.SourceType = type;
                picker.MediaTypes = new string[] { UTType.Image };
                NavigationController.PresentViewController(picker, true, null);
            }
        }

        void TakePictureWithCamera(UIAlertAction action)
        {
            Pick(UIImagePickerControllerSourceType.Camera);
        }

        void TakePictureFromLibrary(UIAlertAction action)
        {
            Pick(UIImagePickerControllerSourceType.PhotoLibrary);
        }

        async void TakeMultimpePicturesFromLibrary(UIAlertAction action)
        {
            var picker = new GMImagePickerController();
            picker.MediaTypes = new Photos.PHAssetMediaType[] { Photos.PHAssetMediaType.Image };
            picker.FinishedPickingAssets += (sender, args) =>
            {
                (this as PosteringViewController).OnTakePicturesComplete(args);
            };
            picker.AllowsMultipleSelection = (Challenge.TargetCount ?? 0) != 1;
            picker.Title = "Select folder";
            await PresentViewControllerAsync(picker, true);
        }

        public void TakePicture(bool isMultipleSelectionAllowed = false)
        {
            UIAlertController alertController;
            // Add Actions
            if (Challenge.IsFixedContent == true || _didPicked)
            {
                alertController = UIAlertController.Create("Share picture", "", UIAlertControllerStyle.ActionSheet);
                alertController.AddAction(UIAlertAction.Create("Send to Instagram", UIAlertActionStyle.Default, (a) => { (this as InstagramViewController).OnTakePictureComplete(null); }));
                alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel")));
            }
            else
            {
                alertController = UIAlertController.Create("Take picture", "", UIAlertControllerStyle.ActionSheet);
                alertController.AddAction(UIAlertAction.Create("Camera", UIAlertActionStyle.Default, TakePictureWithCamera));
                if (isMultipleSelectionAllowed)
                {
                    alertController.AddAction(UIAlertAction.Create("Library", UIAlertActionStyle.Default, TakeMultimpePicturesFromLibrary));
                }
                else
                {
                    alertController.AddAction(UIAlertAction.Create("Library", UIAlertActionStyle.Default, TakePictureFromLibrary));
                }

                //if (this is InstagramViewController)
                //{
                //    alertController.AddAction(UIAlertAction.Create("Send to Instagram", UIAlertActionStyle.Default, (a) => { (this as InstagramViewController).OnTakePictureComplete(null); }));
                //}
                alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel Camera")));
            }

            //var alertController = UIAlertController.Create("Take Picture", "", UIAlertControllerStyle.ActionSheet);

            //// Add Actions
            //alertController.AddAction(UIAlertAction.Create("Camera", UIAlertActionStyle.Default, TakePictureWithCamera));
            //alertController.AddAction(UIAlertAction.Create("Library", UIAlertActionStyle.Default, TakePictureFromLibrary));
            //if (this is InstagramViewController)
            //{
            //    alertController.AddAction(UIAlertAction.Create("Send to Instagram", UIAlertActionStyle.Default, (a) => { (this as InstagramViewController).OnTakePictureComplete(null); }));
            //}
            //alertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel Camera")));

            // Show the alert
            NavigationController.PresentViewController(alertController, true, null);
        }

        public virtual void OnTakePictureComplete(CameraPicture picture)
        {
            _didPicked = true;
        }

        public virtual void OnTakePicturesComplete(MultiAssetEventArgs pictures)
        {

        }
    }
}
