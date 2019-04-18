// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SocialLadder.iOS.More
{
    [Register ("LocationViewController")]
    partial class LocationViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAddAdditionalCity { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAdditionalLocations { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblAdditionalLocationsDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLocationsDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel lblLocationsState { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView left_margin_location { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView right_margin_location { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (lblAddAdditionalCity != null) {
                lblAddAdditionalCity.Dispose ();
                lblAddAdditionalCity = null;
            }

            if (lblAdditionalLocations != null) {
                lblAdditionalLocations.Dispose ();
                lblAdditionalLocations = null;
            }

            if (lblAdditionalLocationsDescription != null) {
                lblAdditionalLocationsDescription.Dispose ();
                lblAdditionalLocationsDescription = null;
            }

            if (lblLocation != null) {
                lblLocation.Dispose ();
                lblLocation = null;
            }

            if (lblLocationsDescription != null) {
                lblLocationsDescription.Dispose ();
                lblLocationsDescription = null;
            }

            if (lblLocationsState != null) {
                lblLocationsState.Dispose ();
                lblLocationsState = null;
            }

            if (left_margin_location != null) {
                left_margin_location.Dispose ();
                left_margin_location = null;
            }

            if (right_margin_location != null) {
                right_margin_location.Dispose ();
                right_margin_location = null;
            }
        }
    }
}