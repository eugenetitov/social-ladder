﻿// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace SocialLadder.iOS.Notifications
{
    [Register ("NotificationsHeaderCell")]
    partial class NotificationsHeaderCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView AreaImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BottomLine { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CircleView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationsCount { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NotificationText { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AreaImage != null) {
                AreaImage.Dispose ();
                AreaImage = null;
            }

            if (BottomLine != null) {
                BottomLine.Dispose ();
                BottomLine = null;
            }

            if (CircleView != null) {
                CircleView.Dispose ();
                CircleView = null;
            }

            if (NotificationsCount != null) {
                NotificationsCount.Dispose ();
                NotificationsCount = null;
            }

            if (NotificationText != null) {
                NotificationText.Dispose ();
                NotificationText = null;
            }
        }
    }
}