using Foundation;
using System;
using UIKit;

namespace SocialLadder.iOS.More
{
    public partial class LocationViewController : UIViewController
    {
        public LocationViewController(IntPtr handle) : base(handle)
        {
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            nfloat fontMultiplier = UIScreen.MainScreen.Bounds.Width / 414;

            lblLocation.Font = UIFont.FromName("SFProText-Medium", 18 * fontMultiplier);
            lblLocationsState.Font = UIFont.FromName("SFProText-Semibold", 13 * fontMultiplier);
            lblLocationsDescription.Font = UIFont.FromName("SFProText-Regular", 13 * fontMultiplier);
            lblAdditionalLocations.Font = UIFont.FromName("SFProText-Heavy", 12 * fontMultiplier);
            lblAdditionalLocationsDescription.Font = UIFont.FromName("SFProText-Regular", 13 * fontMultiplier);
            lblAddAdditionalCity.Font = UIFont.FromName("SFProText-Medium", 18 * fontMultiplier);
        }
    }
}